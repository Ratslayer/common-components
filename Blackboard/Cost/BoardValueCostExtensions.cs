namespace BB
{
	public static class BoardValueCostExtensions
	{
		public static ICostBuilder SpendResource(
			this ICostBuilder builder,
			IBoardKey key,
			double value)
		{
			var component = BoardValueCostComponent.GetPooled();
			component._key = key;
			component._value = value;
			builder.AddComponent(component);
			return builder;
		}
	}
}