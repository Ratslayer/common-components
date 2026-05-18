namespace BB
{
    public readonly struct BoardValue
    {
        private IBoardKey Key { get; init; }
        private double Value { get; init; }
        IBoardValueCondition Condition { get; }
    }
    public interface IBoardValue
    {
        IBoardKey Key { get; }
        double Value { get; }
        IBoardValueCondition Condition { get; }
        // double Get(IBoard board, in GetBoardContext dcontext);
        // void Add(IBoard board, in AddBoardContext context);
    }

    public sealed class PooledBoardValue : ProtectedPooledObject<PooledBoardValue>, IBoardValue
    {
        public IBoardKey Key { get; private set; }
        public double Value { get; private set; }

        public IBoardValueCondition Condition => null;

        public void Add(IBoard board, in AddBoardContext context)
            => board.Add(context);

        public double Get(IBoard board, in GetBoardContext context)
            => board.Get(context);

        public static PooledBoardValue GetPooled(IBoardKey key, double value)
        {
            var result = GetPooledInternal();
            result.Key = key;
            result.Value = value;
            return result;
        }
    }
}