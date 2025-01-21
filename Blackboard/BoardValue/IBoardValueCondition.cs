namespace BB
{
	public interface IBoardValueCondition
	{
		bool IsValid(in GetBoardContext context);
	}
}