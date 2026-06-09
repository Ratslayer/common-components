namespace BB
{
    public interface IBoardValueMultiplier
    {
        double GetMultiplier(IBoard board, in GetBoardContext context);
    }
}