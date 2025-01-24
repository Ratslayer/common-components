using BB.Di;
using System;
namespace BB
{
	public static class BlackboardExtensions
	{
		public readonly struct BlackboardKeyDisposable : IDisposable
		{
			readonly IBoard _board;
			readonly IBoardKey _key;
			readonly double _value;
			public BlackboardKeyDisposable(IBoard board, IBoardKey key, double value)
			{
				_board = board;
				_key = key;
				_value = value;
			}
			public void Dispose()
			{
				if (_board is null || _key is null || _value.IsZero())
					return;
				_key.Add(-_value, _board);
			}
		}
		public static BlackboardKeyDisposable Add(
			this IBoardKey key,
			double value,
			IBoard board)
			=> key.Add(value, new AddBoardContext(board));
		public static BlackboardKeyDisposable Add(
			this IBoardKey key,
			double value,
			AddBoardContext context)
		{
			if (key is null
				|| context._board is null
				|| value.IsZero())
				return default;

			var wrapper = context._board.GetOrCreate(key);
			wrapper.Add(value, context);
			return new(context._board, key, value);
		}

		public static double Get(
			this IBoardKey key,
			in GetBoardContext context)
		{
			if (key is null
				|| context.Board is null)
				return 0;

			return context.Board.GetValue(key, context);
		}
		public static double Get(this IBoardKey key, IBoard board)
			=> Get(key, new GetBoardContext(board));

		public static double GetMultiplier(
			this IBoardKey key,
			in GetBoardContext context)
		{
			var value = key.Get(context);
			return value + 1;
		}

		public static int GetInt(
			this IBoardKey key,
			in GetBoardContext context)
		{
			var value = key.Get(context);
			var intValue = (int)Math.Floor(value + double.Epsilon);
			return intValue;
		}

		public static bool GetBool(
			this IBoardKey key,
			in GetBoardContext context)
		{
			var value = key.Get(context);
			return value > double.Epsilon;
		}

	}
}