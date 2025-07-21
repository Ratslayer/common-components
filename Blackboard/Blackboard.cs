using System.Collections.Generic;
namespace BB
{
	public sealed record Blackboard(IEvent<IBoard> Changed)
		: EntitySystem, IBoard
	{
		[InjectFromParent]
		readonly IBoard _parent;
		readonly Dictionary<IBoardKey, BoardValueContainer> _values = new();
		readonly List<IBoardProcessor> _processors = new();
		readonly List<BoardValueContainer> _dirtyContainers = new();

		public bool AutoFlushDisabled { get; set; }
		public IReadOnlyCollection<IBoardKey> Keys => _values.Keys;
		public IReadOnlyCollection<IBoardValueContainer> DirtyContainers => _dirtyContainers;
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

			Changed.Publish(this);

			foreach (var container in _dirtyContainers)
				container.PreviousValue = container.Value;

			_dirtyContainers.Clear();
		}

		[OnDespawn]
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
				foreach (var container in _generationContainers)
				{
					var genValue = ((IBoardKeyWithGeneration)container.Key).GetGenerationValue(this);
					var value = genValue * seconds;
					this.Add(container.Key, value);
				}
			this.AutoFlushChangesIfDirty();
		}

		public void Add(BoardContext context)
		{
			var key = context.Key;
			_action = "Add";
			context.ActiveKeys.Add(context.Key);

			var container = GetOrCreate(key);

			var finalValue = context.Value;
			finalValue = ApplyMultipliers(context, finalValue, BoardEventUsage.Set);
			finalValue = AddValueInternal(key, container.Value, finalValue);
			finalValue = ClampValue(context, finalValue, BoardEventUsage.Set);

			var valueChanged = !finalValue.Approximately(container.Value);
			if (valueChanged && key is IBoardKeyWithOnAddEffect add)
				add.OnAdd(context);
			context.ActiveKeys.RemoveLast();

			if (!valueChanged)
				return;
			container.Value = finalValue;
			_dirtyContainers.AddUnique(container);
			this.SetDirtyAndAutoFlushChanges();
		}

		public double Get(BoardContext context)
		{
			_action = "Get";
			return GetInternal(context);
		}
		private double GetInternal(BoardContext context)
		{
			var key = context.Key;
			if (key is null)
				return 0;
			if (context.ActiveKeys.Contains(key))
				throw new GameException(
					$"Circular dependency detected during {_action} {context.Key?.Name}. " +
					$"{key.Name} is found multiple times.");
			context.ActiveKeys.Add(key);
			var container = GetOrCreate(key);
			var value = container.Value;
			//add conditional values
			if (container._conditionalValues is not null)
			{
				foreach (var (condition, v) in container._conditionalValues)
					if (condition?.IsValid(context) is true)
						value = AddValueInternal(key, value, v);
			}
			value *= context.Value;
			value = ApplyMultipliers(context, value, BoardEventUsage.Get);
			value = ClampValue(context, value, BoardEventUsage.Get);
			context.ActiveKeys.RemoveLast();
			return value;
		}
		private double AddValueInternal(IBoardKey key, double v1, double v2)
		{
			return key.StackingMethod switch
			{
				BoardValueStackingMethod.Multiplicative => (1 + v1) * (1 + v2) - 1,
				_ => v1 + v2
			};
		}
		private double ApplyMultipliers(BoardContext context, double value, BoardEventUsage usage)
		{
			if (context.Key is not IBoardKeyWithMultipliers km)
				return value;
			if (!km.MultiplierUsage.HasFlag(usage))
				return value;

			var result = value;
			foreach (var multiplier in km.Multipliers)
			{
				var multContext = context.GetPooledCopy().WithKey(multiplier);
				var multValue = GetInternal(multContext);
				multContext.Dispose();
				result *= multValue;
			}
			return result;
		}
		private double ClampValue(BoardContext context, double value, BoardEventUsage usage)
		{
			if (context.Key is not IBoardKeyWithBounds bounds)
				return value;
			if (!bounds.ClampingUsage.HasFlag(usage))
				return value;

			var min = bounds.GetMinValue(context.Board);
			var max = bounds.GetMaxValue(context.Board);
			if (min.IsZero() && max.IsZero())
				return value;

			var result = value.Clamp(min, max);
			return result;
		}
	}
}