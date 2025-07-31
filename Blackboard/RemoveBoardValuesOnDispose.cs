namespace BB
{
	public sealed class AddBoardValueOnDispose : ProtectedPooledObject<AddBoardValueOnDispose>
	{
		IBoard _board;
		IBoardKey _key;
		double _value;
		public static AddBoardValueOnDispose GetPooled(IBoard board, IBoardKey key, double value)
		{
			var result = GetPooledInternal();
			result._board = board;
			result._key = key;
			result._value = value;
			return result;
		}
		public override void Dispose()
		{
			if (_key is not null
				&& _board is not null
				&& _value.NotZero())
				_key.Add(_board, _value);
			base.Dispose();
		}
	}
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