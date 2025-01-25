using System.Collections.Generic;

namespace BB
{
	public interface IConditionalBoardValues
	{
		void Add(ConditionalBoardValue value);
		double GetValue(in GetBoardContext context);
	}
	public sealed record ConditionalBoardValues : IConditionalBoardValues
	{
		readonly Dictionary<IBoardKey, List<ConditionalBoardValue>> _values = new();

		public void Add(ConditionalBoardValue value)
		{
			if (value._key is null
				|| value._condition is null)
				return;
			var list = _values.GetOrAdd(value._key);
			foreach (var i in -list.Count)
			{
				var cv = list[i];
				if (cv._condition != value._condition)
					continue;
				var newValue = cv._value + value._value;
				if (newValue.IsZero())
					list.RemoveAt(i);
				else list[i] = new(cv._board, cv._key, newValue, cv._condition);
				break;
			}
		}

		public double GetValue(in GetBoardContext context)
		{
			if (context._key is null
				|| !_values.TryGetValue(context._key, out var values))
				return 0d;

			var result = 0d;
			foreach (var value in values)
				if (value._condition.IsValid(context))
					result = context._key.AddValues(result, value._value);

			return result;
		}
	}
}