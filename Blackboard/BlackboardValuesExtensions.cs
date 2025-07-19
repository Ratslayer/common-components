using BB.Di;
namespace BB
{
	public interface IBoardValuesProvider
	{
		BuffDisposable Add(in AddBoardContext context);
	}
	public static class IBoardValuesProviderExtensions
	{
		public static BuffDisposable Add(this IBoardValuesProvider provider, Entity entity)
		{
			if (entity.Has(out IBoard board))
				return provider.Add(new AddBoardContext(board, null, 1));
			return null;
		}
	}
}