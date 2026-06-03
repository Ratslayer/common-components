using System;
using System.Collections.Generic;
using System.Linq;

namespace BB
{
    public static class Board
    {
        public static BoardValues AddAndGetDiff(this IBoard board, BoardValues values, object source)
        {
            return new()
            {
                Values = GetDiffValues()
            };

            IEnumerable<BoardValue> GetDiffValues()
            {
                foreach (var value in values)
                {
                    var diff = board.AddAndGetDiff(value, source);
                    yield return (value.Key, diff);
                }
            }
        }

        public static double AddAndGetDiff(this IBoard board, BoardValue value, object source)
        {
            var oldValue = board.Get(value.Key);
            board.Add(value, source);
            var newValue = board.Get(value.Key);
            return newValue - oldValue;
        }

        public static AddBoardValueOnDispose AddTemp(this IBoard board, BoardValue value, object source)
        {
            board?.Add(value, source);

            return AddBoardValueOnDispose.GetPooled(board, -value, source);
        }

        public static void Add(in Entity entity, BoardValue value, object source)
            => entity.Get<IBoard>()?.Add(value, source);

        public static IDisposable AddTemp(this IBoard board, BoardValues values, object source)
        {
            if (!values || board is null)
                return null;
            board.Add(values, source);

            return ApplyBoardValuesOnDispose.GetPooled(
                board,
                -values,
                source);
        }

        public static void Add(this IBoard board, BoardValues values, object source)
        {
            if (!values || board is null)
                return;

            using var _ = board.FlushOnDispose();
            foreach (var value in values.Values)
                board.Add(value, source);
        }

        public static void Add(in Entity entity, BoardValues values, object source)
            => Add(entity.Get<IBoard>(), values, source);

        public static void Add(in Entity entity, IBoardValuesProvider values, object source, double multiplier = 1)
            => Add(entity.Get<IBoard>(), values, source, multiplier);

        public static void Add(in IBoard board, IBoardValuesProvider values, object source, double multiplier = 1)
        {
            if (board is not null && values is not null)
                Add(board,
                    values.GetBoardValues() * multiplier,
                    source);
        }


        public static bool TryAdd(in TryAddBoardContext context)
        {
            if (!CanAdd(context.Context))
                return false;

            if (context.Context.Cost is { } cost)
                context.Context.Board.Add(-cost, context.Source);
            if (context.Context.Value.Key is not null)
                context.Context.Board.Add(
                    context.Context.Value,
                    context.Source);

            return true;
        }

        public static bool TryAdd(
            this IBoard board,
            BoardValue value,
            object source)
        {
            if (!board.CanAdd(value))
                return false;

            board.Add(value, source);
            if (value.Key is not IBoardCostProvider keyCost)
                return true;

            var costValues = -keyCost.Cost.GetBoardValues();
            board.Add(costValues, source);

            return true;
        }

        public static bool TryAdd(in Entity entity, BoardValue value, object source)
            => TryAdd(entity.Require<IBoard>(), value, source);

        public static double Get(in Entity entity, IBoardKey key)
            => Get(entity.Require<IBoard>(), key);

        public static double Get(IBoard board, IBoardKey key)
            => board.Get(new()
            {
                Key = key,
            });

        public static double? GetMaxValue(IBoard board, IBoardKey key)
        {
            if (key is not IBoardKeyWithBounds keyWithBounds)
                return null;

            return keyWithBounds.MaxValue.Get(board);
        }

        public static bool Has(IBoard board, IBoardKey key)
            => Get(board, key).IsPositive();

        public static bool CanAdd(in CanAddBoardContext context)
        {
            var addValue = context.Value.Value;

            if (context.Value.Key is IBoardKeyWithBounds key)
            {
                var value = Get(context.Board, key);
                var newValue = value + addValue;
                var max = key.MaxValue.Get(context.Board);
                var min = key.MinValue.Get(context.Board);
                if (newValue < min || newValue > max)
                    return false;
            }

            if (!CanPayCost(context.Board, context.Requirements, context.MissingValues, addValue))
                return false;

            if (!CanPayCost(context.Board, context.Cost, context.MissingValues, addValue))
                return false;

            return true;
        }

        public static bool CanAdd(this IBoard board, BoardValue value)
            => CanAdd(new CanAddBoardContext
            {
                Board = board,
                Value = value,
                Cost = (value.Key as IBoardCostProvider)?.Cost.GetBoardValues(),
                Requirements = (value.Key as IBoardRequirementsProvider)?.Requirements.GetBoardValues()
            });

        static bool CanPayCost(
            IBoard board,
            BoardValues? values,
            IList<BoardValue> missingValues,
            double multiplier = 1)
        {
            if (values is null)
                return true;

            var canPay = true;
            foreach (var value in values.Value.Values)
            {
                var cost = value.Value * multiplier;
                var v = Get(board, value.Key);
                if (cost > v)
                {
                    canPay = false;
                    missingValues?.Add((value.Key, cost - v));
                }
            }

            return canPay;
        }

        public static bool CanAdd(in Entity entity, BoardValue value)
            => CanAdd(entity.Require<IBoard>(), value);
    }

    public interface IBoardRequirementsProvider
    {
        IBoardValuesProvider Requirements { get; }
    }

    public interface IBoardCostProvider
    {
        IBoardValuesProvider Cost { get; }
    }

    public readonly struct TryAddBoardContext
    {
        public CanAddBoardContext Context { get; init; }
        public object Source { get; init; }
    }

    public readonly struct CanAddBoardContext
    {
        public Entity Entity
        {
            init => Board = value.Get<IBoard>();
        }

        public IBoard Board { get; init; }
        public BoardValue Value { get; init; }
        public BoardValues? Requirements { get; init; }
        public BoardValues? Cost { get; init; }
        public IList<BoardValue> MissingValues { get; init; }
    }
}