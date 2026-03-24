namespace BB
{
    public interface IBoardValue
    {
        IBoardKey Key { get; }
        double Get(IBoard board, in GetBoardContext context);
        void Add(IBoard board, in AddBoardContext context);
    }

    public sealed class PooledBoardValue : ProtectedPooledObject<PooledBoardValue>, IBoardValue
    {
        public IBoardKey Key { get; private set; }
        public double Value { get; private set; }

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