namespace BB
{
	public sealed class RemoveBoardValuesOnDispose : ProtectedPooledObject<RemoveBoardValuesOnDispose>
	{
		IBoardValuesProvider _provider;
		BoardContext _context;
		public static RemoveBoardValuesOnDispose GetInversePooledFromContext(BoardContext context, IBoardValuesProvider provider)
		{
			var result = GetPooledInternal();
			result._provider = provider;
			var inverseContext = context
				.GetPooledCopy()
				.WithValue(-context.Value);
			result._context = inverseContext;
			return result;
		}
		public override void Dispose()
		{
			_provider.Add(_context);
			_context.Dispose();
			base.Dispose();
		}
	}
}