namespace BB
{
	public sealed record ResourceChangedEvent(
		IBoard Board,
		IBoardResourceKey Key,
		double PreviousValue,
		double Value);
}