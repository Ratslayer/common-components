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
           board.Add( new AddBoardContext
            {
                Key = key,
                Value = 1e100
            });
        }

        // public static double Stack(this IBoardKey key, double v1, double v2)
        //     => key.StackingMethod switch
        //     {
        //         BoardValueStackingMethod.Multiplicative => (1 + v1) * (1 + v2) - 1,
        //         _ => v1 + v2
        //     };
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

        public static double Get(this IBoard board, IBoardKey key)
            => board.Get(new() { Key = key });

        public static bool CanAdd(this IBoardKey key, IBoard board, double value)
        {
            if (key is null || board is null)
                return false;
            if (key is not IBoardKeyWithBounds bounds)
                return true;
            var currentValue = Board.Get(board, key);
            var finalValue = currentValue + value;
            var min = bounds.MinValue.Get(board);
            var max = bounds.MaxValue.Get(board);
            if (max.IsZero())
                return finalValue.GreaterThanOrEquals(min);
            return finalValue.GreaterThanOrEquals(min) && finalValue.LessThanOrEquals(max);
        }
    }
}