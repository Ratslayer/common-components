using System.Collections.Generic;

namespace BB
{
    public static class Board
    {
        public static AddBoardValueOnDispose Add(IBoard board, IBoardKey key, object source, double value)
        {
            board?.Add(new()
            {
                Key = key,
                Value = value,
                Source = source
            });

            return AddBoardValueOnDispose.GetPooled(board, key, source, -value);
        }

        public static void Add(IBoard board, IBoardValue value, object source, double multiplier = 1)
            => value.Add(board, new()
            {
                Value = multiplier,
                Source = source
            });

        public static AddBoardValueOnDispose Add(in Entity entity, IBoardKey key, object source, double value = 1)
            => Add(entity.Get<IBoard>(), key, source, value);

        public static ApplyBoardValuesOnDispose Add(IBoard board, IEnumerable<IBoardValue> values, object source,
            double multiplier = 1)
        {
            if (values is null || board is null)
                return default;

            var context = new AddBoardContext
            {
                Value = multiplier,
                Source = source
            };
            using var _ = board.FlushOnDispose();
            foreach (var value in values)
                value.Add(board, context);

            return ApplyBoardValuesOnDispose.GetPooled(board, values, source, -multiplier);
        }

        public static void Add(in Entity entity, IEnumerable<IBoardValue> values, object source, double multiplier = 1)
            => Add(entity.Get<IBoard>(), values, source, multiplier);

        public static void Add(in Entity entity, IBoardValuesProvider values, object source, double multiplier = 1)
            => Add(entity, values?.GetBoardValues(), source, multiplier);

        public static void Add(in IBoard board, IBoardValuesProvider values, object source, double multiplier = 1)
            => Add(board, values?.GetBoardValues(), source, multiplier);

        public static bool TryAdd(in TryAddBoardContext context)
        {
            if (!CanAdd(new CanAddBoardContext
                {
                    Board = context.Board,
                    Multiplier = context.Multiplier,
                    KeyBounds = context.Key as IBoardKeyWithBounds,
                    Cost = context.Cost,
                    Requirements = context.Reqs,
                }))
                return false;

            PayCost(context.Board, context.Cost, context.Source);
            if (context.Key is not null)
                Add(context.Board, context.Key, context.Source, context.Multiplier ?? 1);

            return true;
        }

        public static bool TryAdd(IBoard board, IBoardKey key, object source,
            in CanAddBoardValueOptions context = default)
        {
            if (!CanAdd(board, key, context))
                return false;

            var multiplier = context.Multiplier ?? 1;
            Add(board, key, source, multiplier);
            if (key is not IBoardCostProvider keyCost)
                return true;

            var spendCostContext = new AddBoardContext
            {
                Value = -multiplier,
                Source = source
            };

            var costValues = keyCost.Cost.GetBoardValues().ToPooledList();
            foreach (var cost in costValues)
                cost.Add(board, spendCostContext);
            costValues.DisposeElementsAndClear();

            return true;
        }

        public static bool TryAdd(in Entity entity, IBoardKey key, object source,
            in CanAddBoardValueOptions context = default)
            => TryAdd(entity.Require<IBoard>(), key, source, context);

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
            var addValue = context.Multiplier ?? 1;

            if (context.KeyBounds is not null)
            {
                var value = Get(context.Board, context.KeyBounds);
                var newValue = value + addValue;
                var max = context.KeyBounds.MaxValue.Get(context.Board);
                var min = context.KeyBounds.MinValue.Get(context.Board);
                if (newValue < min || newValue > max)
                    return false;
            }

            if (!CanPayCost(context.Board, context.Requirements, context.MissingValues, addValue))
                return false;

            if (!CanPayCost(context.Board, context.Cost, context.MissingValues, addValue))
                return false;

            return true;
        }

        public static bool CanAdd(IBoard board, IBoardKey key, in CanAddBoardValueOptions context = default)
            => CanAdd(new CanAddBoardContext
            {
                Board = board,
                KeyBounds = context.AllowOverflow is true ? null : key as IBoardKeyWithBounds,
                Multiplier = context.Multiplier,
                Cost = (key as IBoardCostProvider)?.Cost.GetBoardValues(),
                Requirements = (key as IBoardRequirementsProvider)?.Requirements.GetBoardValues()
            });

        static void PayCost(IBoard board, IEnumerable<IBoardValue> values, object source)
        {
            if (values is null)
                return;
            foreach (var value in values)
                Add(board, value, source, -1);
        }

        static bool CanPayCost(IBoard board, IEnumerable<IBoardValue> values, IList<IBoardValue> missingValues,
            double multiplier = 1)
        {
            if (values is null)
                return true;

            var getContext = new GetBoardContext
            {
                Multiplier = multiplier
            };

            var canPay = true;
            foreach (var value in values)
            {
                var cost = value.Get(board, getContext);
                var v = Get(board, value.Key);
                if (cost > v)
                {
                    canPay = false;
                    missingValues?.Add(PooledBoardValue.GetPooled(value.Key, cost));
                }
            }

            return canPay;
        }

        public static bool CanAdd(in Entity entity, IBoardKey key, in CanAddBoardValueOptions context = default)
            => CanAdd(entity.Require<IBoard>(), key, context);
    }

    public interface IBoardRequirementsProvider
    {
        IBoardValuesProvider Requirements { get; }
    }

    public interface IBoardCostProvider
    {
        IBoardValuesProvider Cost { get; }
    }

    public readonly struct CanAddBoardValueOptions
    {
        public double? Multiplier { get; init; }
        public bool? AllowOverflow { get; init; }
    }

    public readonly struct TryAddBoardContext
    {
        public Entity Entity
        {
            init => Board = value.Get<IBoard>();
        }

        public IBoard Board { get; init; }
        public IBoardKey Key { get; init; }
        public double? Multiplier { get; init; }
        public object Source { get; init; }
        public IEnumerable<IBoardValue> Cost { get; init; }
        public IEnumerable<IBoardValue> Reqs { get; init; }
    }

    public readonly struct CanAddBoardContext
    {
        public Entity Entity
        {
            init => Board = value.Get<IBoard>();
        }

        public IBoard Board { get; init; }
        public IBoardKeyWithBounds KeyBounds { get; init; }
        public double? Multiplier { get; init; }
        public IEnumerable<IBoardValue> Requirements { get; init; }
        public IEnumerable<IBoardValue> Cost { get; init; }
        public IList<IBoardValue> MissingValues { get; init; }
    }
}