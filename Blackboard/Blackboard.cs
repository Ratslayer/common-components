using System.Collections.Generic;
namespace BB
{
	public sealed record Blackboard(
		IEvent<IBoard> Changed,
		IConditionalBoardValues ConditionalValues)
		: EntitySystem, IBoard
	{
		[InjectFromParent]
		readonly IBoard _parent;
		readonly Dictionary<IBoardKey, IBoardValueContainer> _values = new();
		readonly List<IBoardProcessor> _processors = new();
		readonly List<IBoardKey> _dirtyKeys = new();

		public bool AutoFlushDisabled { get; set; }
		public IEnumerable<IBoardKey> Keys => _values.Keys;
		public IEnumerable<IBoardKey> DirtyKeys => _dirtyKeys;

		public void InitKey(IBoardKey key)
		{
			GetOrCreate(key);
			this.SetDirtyAndFlushChanges();
		}
		public void AddProcessor(IBoardProcessor processor)
		{
			_processors.Add(processor);
			_processors.SortByPriority();
		}
		public void RemoveProcessor(IBoardProcessor processor) => _processors.Remove(processor);
		public IBoardValueContainer GetOrCreate(IBoardKey key)
		{
			if (key is null)
				return null;

			if (!_values.TryGetValue(key, out var wrapper))
			{
				wrapper = key.CreateValue(this);
				_values.Add(key, wrapper);

				if (wrapper is IBoardValueOnCreate init)
					init.OnCreate();

				if (key is IBoardValueOnCreate initKey)
					initKey.OnCreate();
			}

			return wrapper;
		}

		public bool Has(IBoardKey key, out IBoardValueContainer wrapper)
		{
			if (key is null)
			{
				wrapper = null;
				return false;
			}
			return _values.TryGetValue(key, out wrapper);
		}

		public void ForceFlushChanges()
		{
			using var _ = this.DisableAutoFlush();
			foreach (var processor in _processors)
				processor.Process(this);
			Changed.Publish(this);
			_dirtyKeys.Clear();
		}
		public double Get(in GetBoardContext context)
		{
			var result = 0d;

			//protection against circular dependencies
			if (IncrementResolution("GetValue"))
			{
				try
				{
					GetResult(context);
				}
				finally
				{
					_isResolving = false;
				}
			}
			else GetResult(context);

			return result;

			void GetResult(in GetBoardContext context)
			{
				var key = context._key;
				if (_values.TryGetValue(key, out var value))
					result = key.AddValues(result, value.Get(context));

				if (Entity.AttachedToEntity.Has(out IBoard board))
					result = key.AddValues(
						result,
						board.Get(new(key, board, context._targetBoard)));
				else if (_parent is not null)
					result = key.AddValues(
						result,
						_parent.Get(new(key, _parent, context._targetBoard)));
			}
		}

		public void Add(in AddBoardContext context)
		{
			var key = context._key;
			if (key is null)
				return;

			//protection against circular dependencies
			if (IncrementResolution("AddValue"))
			{
				try
				{
					AddValue(context);
				}
				finally
				{
					_isResolving = false;
				}
			}
			else AddValue(context);

			_dirtyKeys.Add(key);
			this.SetDirtyAndFlushChanges();

			void AddValue(in AddBoardContext context)
			{
				var container = GetOrCreate(key);
				container.Add(context);
			}
		}
		bool _isResolving;
		int _resolutionCount;
		const int MaxResolutionCount = 100;
		bool IncrementResolution(string nameOfMethod)
		{
			if (_isResolving)
			{
				_resolutionCount++;
				if (_resolutionCount > MaxResolutionCount)
				{
					_isResolving = false;
					throw new System.Exception(
						$"{Entity} board has reached {MaxResolutionCount} {nameOfMethod} calls. " +
						$"It's prolly an infinite loop.");
				}
				return false;
			}
			_isResolving = true;
			_resolutionCount = 0;
			return true;
		}

		[OnDespawn]
		void OnDespawn()
		{
			_processors.Clear();
			_values.DisposeAndClear();
		}

		public bool IsDirty(IBoardKey key)
			=> _dirtyKeys.Contains(key);
	}
}