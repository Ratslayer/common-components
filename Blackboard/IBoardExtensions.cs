using System;
namespace BB
{
	public static class IBoardExtensions
	{
		public static double Get(this IBoard board, IBoardKey key)
			=> BoardContext
			.GetPooled(board, key)
			.GetAndDispose();
		public static void Add(this IBoard board, IBoardKey key, double value)
		=> BoardContext
			.GetPooled(board, key)
			.WithValue(value)
			.AddAndDispose();
		public static bool IsDirty(this IBoard board, IBoardKey key, out IBoardValueContainer container)
		{
			foreach (var c in board.DirtyContainers)
			{
				if (c.Key == key)
				{
					container = c;
					return true;
				}
			}
			container = null;
			return false;
		}
		public static double GetChange(this IBoard board, IBoardKey key)
		{
			if (board.IsDirty(key, out var container))
				return container.Value - container.PreviousValue;
			return 0;
		}
		public static bool HasChanged(this IBoard board, IBoardKey key, out double value)
		{
			if (!board.IsDirty(key, out var container))
			{
				value = 0;
				return false;
			}

			value = container.Value - container.PreviousValue;
			return value.NotZero();
		}
		public static void Set(this IBoardKey key, IBoard board, double value)
		{
			if (key is null || board is null)
				return;
			board.Set(key, value);
		}
		public static void Add(this IBoardKey key, IBoard board, double value)
			=> BoardContext
				.GetPooled(board, key)
				.WithValue(value)
				.AddAndDispose();
		public static IDisposable AddTemp(this IBoardKey key, Entity entity, double value)
		{
			var board = entity.Require<IBoard>();
			return key.AddTemp(board, value);
		}
		public static IDisposable AddTemp(this IBoardKey key, IBoard board, double value)
		{
			key.Add(board, value);
			return AddBoardValueOnDispose.GetPooled(board, key, -value);
		}
		public static double Get(
			this IBoardKey key,
			Entity entity)
		{
			if (entity.Has(out IBoard board))
				return key.Get(board);
			return 0;
		}
		public static double Get(this IBoardKey key, BoardContext context)
			=> context.Get(key);
		public static double Get(
			this IBoardKey key,
			IBoard board)
			=> BoardContext
			.GetPooled(board, key)
			.GetAndDispose();
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
		public static bool CanAdd(this IBoardKey key, IBoard board, double value)
		{
			if (key is null || board is null)
				return false;
			if (key is not IBoardKeyWithBounds bounds)
				return true;
			var currentValue = key.Get(board);
			var finalValue = currentValue + value;
			var min = bounds.GetMinValue(board);
			var max = bounds.GetMaxValue(board);
			if (max.IsZero())
				return finalValue.GreaterThanOrEquals(min);
			return finalValue.GreaterThanOrEquals(min) && finalValue.LessThanOrEquals(max);
		}
	}
}