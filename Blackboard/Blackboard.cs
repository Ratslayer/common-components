using System.Collections.Generic;
namespace BB
{
    public sealed class Blackboard
        : EntitySystem, IBoard, ISerializableComponent
    {
        [Inject] IEvent<IBoard> _changed;
        [InjectFromParent]
        readonly IBoard _parent;
        readonly Dictionary<IBoardKey, BoardValueContainer> _values = new();
        readonly List<IBoardProcessor> _processors = new();
        readonly List<BoardValueContainer> _dirtyContainers = new();

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
            if (_dirtyContainers.Count == 0)
                return;

            using var _ = this.DisableAutoFlush();

            foreach (var processor in _processors)
                processor.Process(this);

            _changed.Publish(this);

            foreach (var container in _dirtyContainers)
                container.PreviousValue = container.Value;

            _dirtyContainers.Clear();
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
            container._conditionalValues?.Clear();
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

            var container = GetOrCreate(key);

            var getContext = new GetBoardContext
            {
                Board = this,
                Key = key
            };
            var finalValue = context.GetValue();
            finalValue = ApplyMultipliers(getContext, finalValue, BoardEventUsage.Set);
            finalValue = key.Stack(container.Value, finalValue);
            finalValue = ClampValue(getContext, finalValue, BoardEventUsage.Set);

            var diff = finalValue - container.Value;
            var valueChanged = diff.NotZero();
            if (valueChanged && key is IBoardKeyWithOnAddEffect add)
                add.OnAdd(context.WithValue(diff));

            if (!valueChanged)
                return;
            container.Value = finalValue;
            _dirtyContainers.AddUnique(container);
            this.SetDirtyAndAutoFlushChanges();
        }

        public double Get(in GetBoardContext context)
        {
            var key = context.Key;
            if (key is null)
                return 0;
            var value = GetValueRecursive(context, this);
            value *= context.Multiplier ?? 1;
            value = ApplyMultipliers(context, value, BoardEventUsage.Get);
            value = ClampValue(context, value, BoardEventUsage.Get);
            return value;
        }
        private static double GetValueRecursive(in GetBoardContext context, IBoard startingBoard)
        {
            var board = (Blackboard)context.Board;
            var key = context.Key;
            var container = board.GetOrCreate(context.Key);
            var value = container.Value;
            //add conditional values
            if (container._conditionalValues is not null)
            {
                var conditionalContext = context.WithBoard(startingBoard);
                foreach (var (condition, v) in container._conditionalValues)
                {
                    if (condition?.IsValid(conditionalContext) is true)
                        value = key.Stack(value, v);
                }
            }
            if (board._parent is IBoard parent)
            {
                var parentValue = GetValueRecursive(context.WithBoard(parent), startingBoard);
                value = key.Stack(value, parentValue);
            }
            return value;
        }

        private double ApplyMultipliers(GetBoardContext context, double value, BoardEventUsage usage)
        {
            if (context.Key is not IBoardKeyWithMultipliers km)
                return value;
            if (!km.MultiplierUsage.HasFlag(usage))
                return value;

            var result = value;
            foreach (var multiplier in km.Multipliers)
            {
                var multValue = Get(context.WithKey(multiplier));
                result *= multValue;
            }
            return result;
        }
        private double ClampValue(in GetBoardContext context, double value, BoardEventUsage usage)
        {
            if (context.Key is not IBoardKeyWithBounds bounds)
                return value;
            if (!bounds.ClampingUsage.HasFlag(usage))
                return value;

            var min = bounds.GetMinValue(context);
            var max = bounds.GetMaxValue(context);
            if (min.IsZero() && max.IsZero())
                return value;

            var result = value.Clamp(min, max);
            return result;
        }

        public IEntityComponentSerializer GetSerializer()
            => BoardSerializerV1.Default;
    }
}