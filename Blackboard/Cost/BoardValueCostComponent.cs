namespace BB
{
	public sealed class BoardValueCostComponent
		: PooledObject<BoardValueCostComponent>, ICanSpendCostComponent, ISpendCostComponent
	{
		public IBoardKey _key;
		public double _value;
		public bool CanSpend(ICostContext context)
		{
			if (_key is null
				|| !context.Entity.Has(out IBoard board))
				return false;

			var value = _key.Get(board);
			var result = value >= _value * context.Multiplier;
			if (!result)
				context.AddErrorMessage($"Not enough {_key.Name}");

			return result;
		}

		public void Spend(ICostContext context)
		{
			if (_key is null
				|| !context.Entity.Has(out IBoard board))
				return;

			_key.Add(board, -_value * context.Multiplier);
		}
		public override void Dispose()
		{
			base.Dispose();
			_key = null;
			_value = 0;
		}
	}
}