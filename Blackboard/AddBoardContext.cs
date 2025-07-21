//namespace BB
//{
//	public readonly struct AddBoardContext
//	{
//		public readonly IBoard _board;
//		public readonly IBoardKey _key;
//		public readonly double _value;
//		public AddBoardContext(
//			IBoard board,
//			IBoardKey key,
//			double value)
//		{
//			_board = board;
//			_key = key;
//			_value = value;
//		}
//		public static implicit operator bool(AddBoardContext context)
//			=> context._board is not null
//			&& context._key is not null
//			&& context._value.NotZero();
//		public override string ToString()
//			=> $"{_value:N2} {_key} {_board}";
//	}
//}