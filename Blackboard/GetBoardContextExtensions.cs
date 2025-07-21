//namespace BB
//{
//	public static class GetBoardContextExtensions
//	{
//		public static GetBoardContext WithKey(this GetBoardContext context, IBoardKey key)
//			=> new(key, context._board, context._targetBoard);

//		public static double Apply(this GetBoardContext context)
//			=> context ? context._board.Get(context) : 0;

//		public static GetBoardContext WithInversedTarget(this GetBoardContext context)
//			=> new(context._key, context._targetBoard, context._board);

//	}
//}