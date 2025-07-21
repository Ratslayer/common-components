namespace BB
{
	public sealed class BoardKey : BaseBoardKey
	{
		public override BoardEventUsage ClampingUsage => BoardEventUsage.Set;
	}
	//public sealed record DefaultBoardValue(
	//	IBoardKey Key,
	//	IBoard Board)
	//	: BaseBoardValue(Key, Board);
}