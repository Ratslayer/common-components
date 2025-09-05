using System;

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
			   .AddAndDispose(provider);
		}
		public static IDisposable AddTemp(this IBoardValuesProvider provider, IBoard board, double value = 1)
		{
			provider.Add(board, value);
			var context = BoardContext
				.GetPooled(board)
				.WithValue(-value);
			return ApplyBoardValueProvidersOnDispose
				.GetPooled()
				.WithContext(context)
				.WithProvider(provider);
		}
		public static IDisposable AddTemp(this IBoardValuesProvider provider, Entity entity, double value = 1)
		{
			if (entity.Has(out IBoard board))
				return provider.AddTemp(board, value);
			return null;
		}
	}
}