namespace BB
{
    public readonly struct BoardValue
    {
        public IBoardKey Key { get; init; }
        public double Value { get; init; }
        public IBoardValueCondition Condition { get; init; }
        public IBoardValueMultiplier Multiplier { get; init; }

        public BoardValue WithValue(double value)
            => new()
            {
                Key = Key,
                Value = value,
                Condition = Condition,
                Multiplier = Multiplier
            };

        public static implicit operator bool(BoardValue value)
            => value.Key is not null && value.Value.NotZero();

        public static BoardValue operator -(BoardValue v)
            => new()
            {
                Key = v.Key,
                Value = -v.Value,
                Condition = v.Condition,
                Multiplier = v.Multiplier
            };

        public static BoardValue operator *(BoardValue v, double multiplier)
            => new()
            {
                Key = v.Key,
                Value = v.Value * multiplier,
                Condition = v.Condition,
                Multiplier = v.Multiplier
            };

        public static implicit operator BoardValue((IBoardKey key, double value) kvp)
            => new()
            {
                Key = kvp.key,
                Value = kvp.value
            };
    }

    // public interface IBoardValue
    // {
    //     IBoardKey Key { get; }
    //     double Value { get; }
    //
    //     IBoardValueCondition Condition { get; }
    //     // double Get(IBoard board, in GetBoardContext dcontext);
    //     // void Add(IBoard board, in AddBoardContext context);
    // }

    // public sealed class PooledBoardValue : ProtectedPooledObject<PooledBoardValue>, IBoardValue
    // {
    //     public IBoardKey Key { get; private set; }
    //     public double Value { get; private set; }
    //
    //     public IBoardValueCondition Condition => null;
    //
    //     public void Add(IBoard board, in AddBoardContext context)
    //         => board.Add(context);
    //
    //     public double Get(IBoard board, in GetBoardContext context)
    //         => board.Get(context);
    //
    //     public static PooledBoardValue GetPooled(IBoardKey key, double value)
    //     {
    //         var result = GetPooledInternal();
    //         result.Key = key;
    //         result.Value = value;
    //         return result;
    //     }
    // }
}