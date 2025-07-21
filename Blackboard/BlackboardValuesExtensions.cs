namespace BB
{
	public interface IBoardValuesProvider
	{
		RemoveBoardValuesOnDispose Add(BoardContext context);
	}
	public static class IBoardValuesProviderExtensions
	{
		public static RemoveBoardValuesOnDispose Add(this IBoardValuesProvider provider, Entity entity)
		{
			if (!entity.Has(out IBoard board))
				return null;
			var context = BoardContext.GetPooled(board);
			var result = provider.Add(context);
			context.Dispose();
			return result;
		}
	}
}