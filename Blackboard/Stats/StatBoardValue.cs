using System.Collections.Generic;

namespace BB
{
	public sealed record StatBoardValue(
		IBlackboardStatKey Stat,
		IBoard Board) : BaseBoardValue(Stat, Board), IBoardStatValue
	{
		readonly List<ConditionalBoardValue> _values = new();
		public override double Get(in GetBoardContext context)
		{
			var value = Value;
			foreach (var bv in _values)
				value += bv.GetValue(context);
			return value;
		}
		//public void Add(in ConditionalBoardValue value, in AddBoardContext context)
		//{
		//	for (var i = 0; i < _values.Count; i++)
		//	{
		//		var v = _values[i];

		//		if (v._condition != value._condition
		//			|| v._key != value._key)
		//			continue;

		//		var stackedValue = v._value + value._value;
		//		_values[i] = new(context._board,v._key, stackedValue, v._condition);
		//		return;
		//	}

		//	var newValue = value._value * context._numStacks;
		//	_values.Add(new(context._board,value._key, newValue, value._condition));
		//}

	}
}