namespace BB
{
	public sealed class BoardKey : BaseBoardKey
	{
	}
	public sealed record DefaultBoardValue(
		IBoardKey Key,
		IBoard Board)
		: BaseBoardValue(Key, Board);
}