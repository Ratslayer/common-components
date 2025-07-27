namespace BB
{
	public static class IBoardValuesProviderExtensions
	{
		public static RemoveBoardValuesOnDispose Add(this IBoardValuesProvider provider, Entity entity)
		{
			if (!entity.Has(out IBoard board))
				return null;
			var context = BoardContext.GetPooled(board);
			provider.Add(context);
			context.Dispose();
			return RemoveBoardValuesOnDispose.GetInversePooledFromContext(context, provider);
		}
	}
}