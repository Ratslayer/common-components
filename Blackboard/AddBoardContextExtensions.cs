//namespace BB
//{
//	public static class AddBoardContextExtensions
//	{
//		public static AddBoardContext WithValue(this AddBoardContext context, double value)
//			=> new(context._board, context._key, value);

//		public static AddBoardContext WithKey(this AddBoardContext context, IBoardKey key)
//			=> new(context._board, key, context._value);
//		public static AddBoardContext WithMultiplier(this AddBoardContext context, double value)
//			=> new(context._board, context._key, context._value * value);
//		public static void Add(this AddBoardContext context)
//			=> context._board.Add(context);

//	}
//}