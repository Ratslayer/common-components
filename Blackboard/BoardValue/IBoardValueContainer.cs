namespace BB
{
	public interface IBoardValue
	{
		IBoardKey Key { get; }
		double Value { get; }
	}
	public interface IBoardValueContainer
	{
		IBoard Board { get; }
		IBoardKey Key { get; }
		double Get(in GetBoardContext context);
		void Add(in AddBoardContext context);
		void Set(double value);
		double Value { get; }
		double PreviousValue { get; }
		void FlushPreviousValue();
	}
	public static class IBoardValueContainerExtensions
	{
		public static double GetValue(this IBoardValueContainer container)
			=> container?.Get(new(container.Key, container.Board)) ?? 0;
	}
}