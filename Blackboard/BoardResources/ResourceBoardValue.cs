namespace BB
{
	public sealed record ResourceBoardValue(
		IBoard Board,
		IBoardResourceKey ResourceKey,
		IEvent<ResourceChangedEvent> Changed)
		: BaseBoardValue(ResourceKey, Board),
		IBoardResourceValue
	{

		public override void Add(double value, in AddBoardContext context)
		{
			if (value.IsZero() || context._numStacks <= 0)
				return;

			var oldValue = Value;
			var additionalValue = value * context._numStacks;

			if (additionalValue < 0)
			{
				Value = (Value + additionalValue).Min0();
				RaiseEvents(oldValue, Value);
				return;
			}

			var multipliedValue = additionalValue;
			if (ResourceKey.GainMultiplierKeys is not null)
				foreach (var multiplierKey in ResourceKey.GainMultiplierKeys)
					multipliedValue *= multiplierKey.GetMultiplier(new(Board));

			var maxValue = ResourceKey.MaxValueKey.Get(new(Board));
			Value = (Value + multipliedValue).Clamp(0, maxValue);

			RaiseEvents(oldValue, Value);
		}
		void RaiseEvents(double oldValue, double newValue)
		{
			if (oldValue.Approximately(newValue))
				return;
			Changed.Raise(new(Board, ResourceKey, oldValue, newValue));
			Board.FlushChanges();
		}
	}
}