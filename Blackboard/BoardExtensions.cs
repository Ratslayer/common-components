using BB.Di;
using System;
namespace BB
{
	public static class BoardExtensions
	{
		public static bool IsDirty(
			this IBoard board,
			IBoardKey key,
			out double change)
		{
			var changed = board.IsDirty(key);
			change = board.GetChange(key);
			return changed;
		}
		public static AddBoardContext Add(this IBoardKey key, IBoard board, double value)
		{
			var context = new AddBoardContext(board, key, value);
			context.Add();
			return context;
		}
		public static double Get(
			this IBoardKey key,
			IBoard board)
			=> new GetBoardContext(key, board)
			.Apply();
		public static double GetMultiplier(
			this IBoardKey key,
			IBoard board)
		{
			var value = key.Get(board);
			return value + 1;
		}

		public static int GetInt(
			this IBoardKey key,
			IBoard board)
		{
			var value = key.Get(board);
			var intValue = (int)Math.Floor(value + double.Epsilon);
			return intValue;
		}

		public static bool GetBool(
			this IBoardKey key,
			IBoard board)
		{
			var value = key.Get(board);
			return value > double.Epsilon;
		}

	}
}