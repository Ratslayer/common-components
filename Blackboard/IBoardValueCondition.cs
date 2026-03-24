namespace BB
{
    public interface IBoardValueCondition
    {
        bool IsValid(IBoard board, in GetBoardContext context);
    }
}