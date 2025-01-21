namespace BB
{
	public readonly struct ConditionalBoardValue
	{
		public readonly IBoardKey _key;
		public readonly double _value;
		public readonly IBoardValueCondition _condition;
		public ConditionalBoardValue(IBoardKey key, double value, IBoardValueCondition condition)
		{
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