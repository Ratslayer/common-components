using System.Collections.Generic;

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
	public sealed class ApplyBoardValueProvidersOnDispose : PooledObject<ApplyBoardValueProvidersOnDispose>
	{
		readonly List<IBoardValuesProvider> _providers = new();
        AddBoardContext _context;
		public override void Dispose()
		{
			foreach (var provider in _providers)
				provider.Add(_context);
			_providers.DisposeAndClear();
			_context = default;
			base.Dispose();
		}

		public ApplyBoardValueProvidersOnDispose WithContext(in AddBoardContext context)
		{
			_context = context;
			return this;
		}
		
		public ApplyBoardValueProvidersOnDispose WithProvider(IBoardValuesProvider provider)
		{
			_providers.Add(provider);
			return this;
		}
		public ApplyBoardValueProvidersOnDispose WithProviders(IEnumerable<IBoardValuesProvider> providers)
		{
			_providers.AddRange(providers);
			return this;
		}
	}
}