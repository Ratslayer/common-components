using System;
namespace BB
{
    public static class IBoardValueContainerExtensions
    {
        public static bool HasGained(this IBoardValueContainer container, out double gain)
        {
            gain = container.Value - container.PreviousValue;
            return gain.IsPositive();
        }
    }
    public static class IBoardExtensions
    {
        public static void SetToMax(this IBoardKeyWithBounds key, IBoard board)
        {
            new AddBoardContext
            {
                Board = board,
                Key = key,
                Value = 1e100
            }.Add();
        }
        public static double Stack(this IBoardKey key, double v1, double v2)
            => key.StackingMethod switch
            {
                BoardValueStackingMethod.Multiplicative => (1 + v1) * (1 + v2) - 1,
                _ => v1 + v2
            };
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
            => new AddBoardContext
            {
                Board = board,
                Key = key,
                Value = value
            }.Add();
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
        public static double Get(this IBoard board, IBoardKey key)
            => board.Get(new() { Board = board, Key = key });

        public static bool CanAdd(this IBoardKey key, IBoard board, double value)
        {
            if (key is null || board is null)
                return false;
            if (key is not IBoardKeyWithBounds bounds)
                return true;
            var getContext = new GetBoardContext { Board = board };
            var currentValue = getContext.WithKey(key).Get();
            var finalValue = currentValue + value;
            var min = bounds.GetMinValue(getContext);
            var max = bounds.GetMaxValue(getContext);
            if (max.IsZero())
                return finalValue.GreaterThanOrEquals(min);
            return finalValue.GreaterThanOrEquals(min) && finalValue.LessThanOrEquals(max);
        }
    }
}