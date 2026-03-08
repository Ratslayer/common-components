using System.Collections.Generic;

namespace BB.Blackboard
{
    public sealed class BlackboardV1 : EntitySystem, IBoard, ISerializableComponent
    {
        [Inject] IEvent<IBoard> _changed;
        [InjectFromParent] readonly IBoard _parent;
        readonly Dictionary<IBoardKey, BoardValueContainer> _values = new();
        readonly List<IBoardProcessor> _processors = new();
        readonly List<BoardValueContainer> _dirtyContainers = new(), _dirtyContainersBuffer = new();
        bool _isFlushing;

        public bool AutoFlushDisabled { get; set; }
        public IReadOnlyCollection<IBoardKey> Keys => _values.Keys;
        public IReadOnlyCollection<IBoardValueContainer> DirtyContainers => _dirtyContainers;
        public IReadOnlyCollection<IBoardValueContainer> Containers => _values.Values;
        string _action;
        List<IBoardValueContainer> _generationContainers;

        public void InitKey(IBoardKey key)
        {
            GetOrCreate(key);
            this.SetDirtyAndAutoFlushChanges();
        }

        public void AddProcessor(IBoardProcessor processor)
        {
            _processors.Add(processor);
            _processors.SortByPriority();
        }

        public void RemoveProcessor(IBoardProcessor processor) => _processors.Remove(processor);

        private BoardValueContainer GetOrCreate(IBoardKey key)
        {
            if (key is null)
                return null;

            if (!_values.TryGetValue(key, out var container))
            {
                container = new(this, key);
                if (key is IBoardKeyWithGeneration)
                {
                    _generationContainers ??= new();
                    _generationContainers.Add(container);
                }

                _values.Add(key, container);
            }

            return container;
        }

        public void ForceFlushChanges()
        {
            _isFlushing = true;
            Flush(0);
            _isFlushing = false;
        }

        void Flush(int counter)
        {
            if (counter >= 10)
            {
                LogError($"Board flush reached 10 cycles. Possible stack overflow.");
                return;
            }

            if (_dirtyContainers.Count == 0)
                return;

            using var _ = this.DisableAutoFlush();

            foreach (var container in _dirtyContainers)
                UpdateDirtyContainer(container);

            foreach (var processor in _processors)
                processor.Process(this);

            _changed.Publish(this);

            foreach (var container in _dirtyContainers)
                container.PreviousValue = container.Value;

            _dirtyContainers.Clear();

            if (_dirtyContainersBuffer.Count == 0)
                return;

            _dirtyContainers.AddRange(_dirtyContainersBuffer);
            _dirtyContainersBuffer.Clear();

            Flush(++counter);
        }

        void UpdateDirtyContainer(BoardValueContainer container)
        {
            if (container.AddedValue.IsZero())
                return;

            var initialDiff = container.AddedValue;
            container.AddedValue = 0;
            var key = container.Key;
            initialDiff = ApplyMultipliers(key, initialDiff, BoardEventUsage.Set);
            initialDiff = ApplyAdders(key, initialDiff, BoardEventUsage.Set);

            var finalValue = ClampValue(key, container.Value + initialDiff, BoardEventUsage.Set);
            var finalDiff = finalValue - container.Value;

            var valueChanged = finalDiff.NotZero();
            if (valueChanged && key is IBoardKeyWithOnAddEffect add)
                add.OnAdd(new AddBoardContext
                {
                    Board = this,
                    Key = key,
                    Value = finalDiff
                });
            container.Value = finalValue;
        }

        void SetDirty(BoardValueContainer container)
        {
            var containers = _isFlushing ? _dirtyContainersBuffer : _dirtyContainers;
            containers.AddUnique(container);
        }

        [OnEvent(typeof(EntityDespawnedEvent))]
        void OnDespawn()
        {
            _processors.Clear();
            _values.DisposeAndClear();
            _dirtyContainers.Clear();
        }

        public void Set(IBoardKey key, double value)
        {
            var container = GetOrCreate(key);
            container.PreviousValue = container.Value;
            container.Value = value;
            container.AddedValue = 0;
            container._conditionalValues?.Clear();
            SetDirty(container);
        }

        public void Add(IBoardKey key, IBoardValueCondition condition, double value)
        {
            var container = GetOrCreate(key);
            container._conditionalValues ??= new();
            if (container._conditionalValues.TryGetValue(condition, out var currentValue))
                value += currentValue;
            container._conditionalValues[condition] = value;
        }

        public void UpdateGeneration(float seconds)
        {
            if (_generationContainers is null)
                return;
            using (this.DisableAutoFlush())
            {
                var context = new GetBoardContext { Board = this };
                foreach (var container in _generationContainers)
                {
                    var genValue = ((IBoardKeyWithGeneration)container.Key).GetGenerationValue(context);
                    var value = genValue * seconds;
                    container.Key.Add(this, value);
                }
            }

            this.AutoFlushChangesIfDirty();
        }

        public void Add(in AddBoardContext context)
        {
            var key = context.Key;
            if (key.NullIfDestroyedUnityEngineObject() is null)
                return;
            var value = context.Value ?? 1;
            if (value.IsZero())
                return;

            var container = GetOrCreate(key);
            container.AddedValue += value;
            SetDirty(container);
            this.SetDirtyAndAutoFlushChanges();
        }

        public double Get(in GetBoardContext context)
        {
            var key = context.Key;
            if (key is null)
                return 0;
            var value = GetValueRecursive(context, this);
            value *= context.Multiplier ?? 1;
            value = ApplyMultipliers(key, value, BoardEventUsage.Get);
            value = ApplyAdders(key, value, BoardEventUsage.Get);
            value = ClampValue(key, value, BoardEventUsage.Get);
            return value;
        }

        private static double GetValueRecursive(in GetBoardContext context, IBoard startingBoard)
        {
            var board = (BlackboardV1)context.Board;
            var container = board.GetOrCreate(context.Key);
            var value = container.Value;
            //add conditional values
            if (container._conditionalValues is not null)
            {
                var conditionalContext = context.WithBoard(startingBoard);
                foreach (var (condition, v) in container._conditionalValues)
                {
                    if (condition?.IsValid(conditionalContext) is true)
                        value += v;
                }
            }

            if (board._parent is IBoard parent)
            {
                var parentValue = GetValueRecursive(context.WithBoard(parent), startingBoard);
                value += parentValue;
            }

            return value;
        }

        private double ApplyMultipliers(IBoardKey key, double value, BoardEventUsage usage)
        {
            if (key is not IBoardKeyWithMultipliers km)
                return value;
            if (!km.MultiplierUsage.HasFlag(usage))
                return value;

            var result = value;
            foreach (var multiplier in km.Multipliers)
            {
                if (!_values.TryGetValue(multiplier, out var multiplierContainer))
                    continue;
                UpdateDirtyContainer(multiplierContainer);
                var multValue = Get(new()
                {
                    Board = this,
                    Key = multiplier
                });
                result *= (1 + multValue);
            }

            return result;
        }

        private double ApplyAdders(IBoardKey key, double value, BoardEventUsage usage)
        {
            if (key is not IBoardKeyWithAdders ka)
                return value;
            if (!ka.AdderUsage.HasFlag(usage))
                return value;

            var result = value;
            foreach (var adder in ka.Adders)
            {
                if (!_values.TryGetValue(adder, out var adderContainer))
                    continue;
                UpdateDirtyContainer(adderContainer);
                var addValue = Get(new()
                {
                    Board = this,
                    Key = adder
                });
                result += addValue;
            }

            return result;
        }

        private double ClampValue(IBoardKey key, double value, BoardEventUsage usage)
        {
            if (key is not IBoardKeyWithBounds bounds)
                return value;
            if (!bounds.ClampingUsage.HasFlag(usage))
                return value;

            var context = new GetBoardContext
            {
                Board = this,
                Key = key
            };

            var min = GetValue(bounds.MinValue);
            var max = GetValue(bounds.MaxValue);
            if (min.IsZero() && max.IsZero())
                return value;

            var result = value.Clamp(min, max);
            return result;
        }

        double GetValue(in BoardValueGetter getter, bool updateIfDirty = true)
        {
            if (getter.Type is BoardValueGetterType.Const)
                return getter.ConstValue;
            if (getter.Key is null || !_values.TryGetValue(getter.Key, out var container))
                return 0;
            if (updateIfDirty)
                UpdateDirtyContainer(container);
            return container.Value;
        }

        public IEntityComponentSerializer[] GetSerializers()
            => new[] { BoardSerializerV1.Default };
    }
}