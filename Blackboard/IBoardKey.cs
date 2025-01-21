namespace BB
{
	public interface IBoardKey
	{
		string Name { get; }
		IBoardValueContainer CreateValue(IBoard board);
		double AddValues(double v1, double v2);
	}
}