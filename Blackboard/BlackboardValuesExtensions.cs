using BB.Di;
namespace BB
{
	public interface IBoardValuesProvider
	{
		void Add(in AddBoardContext context);
	}
	public static class IBoardValuesProviderExtensions
	{
		public static void Add(this IBoardValuesProvider provider, Entity entity)
		{
			if (entity.Has(out IBoard board))
				provider.Add(new AddBoardContext(board, null, 1));
		}
	}
}