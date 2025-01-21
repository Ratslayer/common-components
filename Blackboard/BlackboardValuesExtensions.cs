using BB.Di;
namespace BB
{
	public interface IBoardValuesProvider
	{
		void Add(in AddBoardContext context);
	}
	public static class BlackboardValuesProviderExtensions
	{
		public static void Add(this IBoardValuesProvider provider, double numStacks, Entity entity)
		{
			if (provider is null || !entity.Has(out IBoard board))
				return;

			provider.Add(new(board,numStacks));
		}
	}
}