namespace BB
{
	public interface IBoardKey
	{
		string Name { get; }
		IBoardValueContainer CreateValue(IBoard board);
		double AddValues(double v1, double v2);
		double GetMinValue(IBoard board);
		double GetMaxValue(IBoard board);
	}
	public interface IBoardKeyDetails
	{
		IBoardKeyColorScheme ColorScheme { get; }
		bool HasMaxKey(out IBoardKey key);
		bool HasMinKey(out IBoardKey key);
	}
}