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
		double GetValue(in GetBoardContext context);
		void Add(double value, in AddBoardContext context);
	}
	public static class IBoardValueContainerExtensions
	{
		public static double GetValue(this IBoardValueContainer container)
			=> container is null ? 0 : container.GetValue(new(container.Board));
	}
}