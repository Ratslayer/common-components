namespace BB
{
    public static class Board
    {
        public static void Add(IBoard board, IBoardKey key, double value = 1)
        {
            board?.Add(new()
            {
                Board = board,
                Key = key,
                Value = value
            });
        }

        public static void Add(in Entity entity, IBoardKey key, double value = 1)
            => Add(entity.Get<IBoard>(), key, value);

        public static void Add(IBoard board, IBoardValuesProvider provider, double multiplier = 1)
        {
            var values = provider.GetBoardValues().ToPooledList();
            var context = new AddBoardContext
            {
                Board = board,
                Value = multiplier,
            };
            using var _ = board.FlushOnDispose();
            foreach (var value in values)
                value.Add(context);
            values.DisposeAndClear();
        }

        public static void Add(in Entity entity, IBoardValuesProvider values, double multiplier = 1)
             => Add(entity.Get<IBoard>(), values, multiplier);
        public static double Get(IBoard board, IBoardKey key)
            => board.Get(new()
            {
                Board = board,
                Key = key,
            });
        public static double? GetMaxValue(IBoard board, IBoardKey key)
        {
            if (key is not IBoardKeyWithBounds keyWithBounds)
                return null;

            return keyWithBounds.GetMaxValue(new() { Board = board, Key = key });
        }
        public static bool Has(IBoard board, IBoardKey key)
            => Get(board, key).IsPositive();
        public static bool CanAdd(IBoard board, IBoardKey key, in CanAddBoardValueContext context = default)
        {
            var multiplier = context.Multiplier ?? 1;

            if (context.AllowOverflow is not true && key is IBoardKeyWithBounds bounds)
            {
                var value = Get(board, key);
                var newValue = value + 1;
                var max = bounds.GetMaxValue(new()
                {
                    Board = board,
                    Key = key,
                });
                var min = bounds.GetMinValue(new()
                {
                    Board = board,
                    Key = key,
                });
                if (value < min || value > max)
                    return false;
            }

            if (key is IBoardKeyWithRequirements reqs
                && !CanPayCost(board, reqs.Requirements, multiplier))
                return false;

            if (key is IBoardKeyWithCost cost
                && !CanPayCost(board, cost.Cost, multiplier))
                return false;

            return true;

            //bool CanPayCost(IBoardValuesProvider provider)
            //{
            //    if (provider is null)
            //        return true;

            //    var values = provider.GetBoardValues().ToPooledList();
            //    var canPay = true;
            //    foreach (var value in values)
            //    {
            //        var cost = value.Get(getContext);
            //        var v = Get(board, value.Key);
            //        if (cost > v)
            //        {
            //            canPay = false;
            //            break;
            //        }
            //    }

            //    values.DisposeAndClear();
            //    return canPay;
            //}
        }
        public static bool CanPayCost(IBoard board, IBoardValuesProvider provider, double multiplier = 1)
        {
            if (provider is null)
                return true;

            var getContext = new GetBoardContext
            {
                Board = board,
                Multiplier = multiplier
            };
            var values = provider.GetBoardValues().ToPooledList();
            var canPay = true;
            foreach (var value in values)
            {
                var cost = value.Get(getContext);
                var v = Get(board, value.Key);
                if (cost > v)
                {
                    canPay = false;
                    break;
                }
            }

            values.DisposeAndClear();
            return canPay;
        }
        public static bool CanAdd(in Entity entity, IBoardKey key, in CanAddBoardValueContext context = default)
            => CanAdd(entity.Require<IBoard>(), key, context);
        public static bool TryAdd(IBoard board, IBoardKey key, in CanAddBoardValueContext context = default)
        {
            if (!CanAdd(board, key, context))
                return false;

            var multiplier = context.Multiplier ?? 1;
            Add(board, key, multiplier);
            if (key is not IBoardKeyWithCost keyCost)
                return true;

            var spendCostContext = new AddBoardContext
            {
                Board = board,
                Value = -multiplier,
            };

            var costValues = keyCost.Cost.GetBoardValues().ToPooledList();
            foreach (var cost in costValues)
                cost.Add(spendCostContext);
            costValues.DisposeAndClear();

            return true;
        }
        public static bool TryAdd(in Entity entity, IBoardKey key, in CanAddBoardValueContext context = default)
            => TryAdd(entity.Require<IBoard>(), key, context);

    }
    public interface IBoardKeyWithRequirements
    {
        IBoardValuesProvider Requirements { get; }
    }
    public interface IBoardKeyWithCost
    {
        IBoardValuesProvider Cost { get; }
    }
    public readonly struct CanAddBoardValueContext
    {
        public double? Multiplier { get; init; }
        public bool? AllowOverflow { get; init; }
    }
}