namespace BB
{
    public interface IBoardValue
    {
        IBoardKey Key { get; }
        double Get(in GetBoardContext context);
        void Add(in AddBoardContext context);
    }
    public sealed class PooledBoardValue : ProtectedPooledObject<PooledBoardValue>, IBoardValue
    {
        public IBoardKey Key { get; private set; }
        public double Value { get; private set; }

        public void Add(in AddBoardContext context)
            => context.Board.Add(context);
        public double Get(in GetBoardContext context)
            => context.Board.Get(context);

        public static PooledBoardValue GetPooled(IBoardKey key, double value)
        {
            var result = GetPooledInternal();
            result.Key = key;
            result.Value = value;
            return result;
        }
    }
}