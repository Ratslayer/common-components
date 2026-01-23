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

        public static bool CanAdd(IBoard board, in CanAddBoardValueContext context)
        {
            var getContext = new GetBoardContext
            {
                Board = board,
                Multiplier = context.Multiplier
            };

            if (context.Key is IBoardKeyWithBounds bounds)
            {
                var value = Get(board, context.Key);
                var newValue = value + 1;
                var max = bounds.GetMaxValue(new()
                {
                    Board = board,
                    Key = context.Key,
                });
                var min = bounds.GetMinValue(new()
                {
                    Board = board,
                    Key = context.Key,
                });
                if (value < min || value > max)
                    return false;
            }

            if (!CanPayCost(board, context.Requirements, context.Multiplier ?? 1))
                return false;

            if (!CanPayCost(board, context.Cost, context.Multiplier ?? 1))
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
        public static bool CanAdd(in Entity entity, in CanAddBoardValueContext context)
            => CanAdd(entity.Require<IBoard>(), context);
        public static bool TryAdd(IBoard board, in CanAddBoardValueContext context)
        {
            if (!CanAdd(board, context))
                return false;

            var multiplier = context.Multiplier ?? 1;
            Add(board, context.Key, multiplier);
            if (context.Cost is null)
                return true;

            var spendCostContext = new AddBoardContext
            {
                Board = board,
                Value = -multiplier,
            };

            var costValues = context.Cost.GetBoardValues().ToPooledList();
            foreach (var cost in costValues)
                cost.Add(spendCostContext);
            costValues.DisposeAndClear();

            return true;
        }
        public static bool TryAdd(in Entity entity, in CanAddBoardValueContext context)
            => TryAdd(entity.Require<IBoard>(), context);

    }
    public readonly struct CanAddBoardValueContext
    {
        public IBoardKey Key { get; init; }
        public IBoardValuesProvider Requirements { get; init; }
        public IBoardValuesProvider Cost { get; init; }
        public double? Multiplier { get; init; }
    }
}