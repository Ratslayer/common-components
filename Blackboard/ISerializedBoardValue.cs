namespace BB
{
    public interface ISerializedBoardValue
    {
        void Add(IBoard board, in AddBoardContext context);
    }
}