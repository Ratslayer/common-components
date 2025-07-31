namespace BB
{
	public static class IBoardValuesProviderExtensions
	{
		public static void Add(this IBoardValuesProvider provider, Entity entity, double value = 1)
		{
			if (entity.Has(out IBoard board))
				provider.Add(board, value);
		}
		public static void Add(this IBoardValuesProvider provider, IBoard board, double value = 1)
		{
			BoardContext
			   .GetPooled(board)
			   .WithValue(value)
			   .AddAndDispose();
		}
	}
}