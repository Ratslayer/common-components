namespace BB
{
	public readonly struct ConditionalBoardValue
	{
		public readonly IBoardKey _key;
		public readonly double _value;
		public readonly IBoardValueCondition _condition;
		public readonly IBoard _board;
		public ConditionalBoardValue(IBoard board, IBoardKey key, double value, IBoardValueCondition condition)
		{
			_board = board;
			_key = key;
			_value = value;
			_condition = condition;
		}
		public double GetValue(in GetBoardContext context)
		{
			if (!_condition.IsValid(context))
				return 0;
			return _value;
		}
	}
}