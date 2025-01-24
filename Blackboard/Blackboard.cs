using System.Collections.Generic;
namespace BB
{
	public sealed record Blackboard(IEvent<BoardChangedEvent> Changed) : EntitySystem, IBoard
	{
		[InjectFromParent]
		readonly IBoard _parent;
		readonly Dictionary<IBoardKey, IBoardValueContainer> _values = new();
		readonly List<IBoardProcessor> _processors = new();
		public IEnumerable<IBoardValueContainer> Values => _values.Values;
		public void AddProcessor(IBoardProcessor processor) => _processors.Add(processor);
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

		public void FlushChanges()
		{
			foreach (var processor in _processors)
				processor.Process(this);
			Changed.Raise(new(this));
		}

		public double GetValue(IBoardKey key, in GetBoardContext context)
		{
			var result = 0d;

			if (_values.TryGetValue(key, out var value))
				result = key.AddValues(result, value.GetValue(context));
			if (Entity.AttachedToEntity.Has(out IBoard board))
				result = key.AddValues(result, board.GetValue(key, context));
			else if (_parent is not null)
				result = key.AddValues(result, _parent.GetValue(key, context));

			return result;
		}
	}
}