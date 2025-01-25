using BB.Di;
using System;
namespace BB
{
	public static class BoardExtensions
	{
		public static AddBoardContext Add(this IBoard board, IBoardKey key, double value)
		{
			var context = new AddBoardContext(board, key, value);
			context.Add();
			return context;
		}
		public static double Get(
			this IBoard board,
			IBoardKey key)
			=> new GetBoardContext(key, board)
			.Apply();
		public static double GetMultiplier(
			this IBoard board,
			IBoardKey key)
		{
			var value = board.Get(key);
			return value + 1;
		}

		public static int GetInt(
			this IBoard board,
			IBoardKey key)
		{
			var value = board.Get(key);
			var intValue = (int)Math.Floor(value + double.Epsilon);
			return intValue;
		}

		public static bool GetBool(
			this IBoard board,
			IBoardKey key)
		{
			var value = board.Get(key);
			return value > double.Epsilon;
		}

	}
}