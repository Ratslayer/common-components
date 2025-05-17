namespace BB
{
	public sealed record ResourceBoardValue(
		IBoard Board,
		IBoardResourceKey ResourceKey,
		IEvent<ResourceChangedEvent> Changed)
		: BaseBoardValue(ResourceKey, Board),
		IBoardResourceValue
	{

		public override void Add(in AddBoardContext context)
		{
			if (!context)
				return;

			var oldValue = Value;
			var additionalValue = context._value;

			if (additionalValue < 0)
			{
				Value = (Value + additionalValue).Min0();
				RaiseEvents(oldValue, Value);
				return;
			}

			var multipliedValue = additionalValue;
			if (ResourceKey.GainMultiplierKeys is not null)
				foreach (var multiplierKey in ResourceKey.GainMultiplierKeys)
					multipliedValue *= multiplierKey.GetMultiplier(Board);

			var maxValue = ResourceKey.MaxValueKey.Get(Board);
			Value = (Value + multipliedValue).Clamp(0, maxValue);

			RaiseEvents(oldValue, Value);
		}
		void RaiseEvents(double oldValue, double newValue)
		{
			if (oldValue.Approximately(newValue))
				return;
			Changed.Publish(new(Board, ResourceKey, oldValue, newValue));
		}
	}
}