using System.Collections.Generic;

namespace BB
{
	public static class CostUtils
	{

		public static bool TrySpend(this ICost cost)
		{
			if (!cost.CanSpend())
				return false;
			cost.Spend();
			return true;
		}
		public static ICostBuilder WithEntity(
			this ICostBuilder builder,
			Entity entity)
		{
			builder.Context.Entity = entity;
			return builder;
		}
		public static ICostBuilder WithMultiplier(
			this ICostBuilder builder,
			double multiplier)
		{
			builder.Context.Multiplier = multiplier;
			return builder;
		}
		public static ICostBuilder WithDefaultLogger(
			this ICostBuilder builder)
		{
			builder.Context.ProcessErrorMessages = Process;
			return builder;

			static void Process(List<string> msgs)
			{
				foreach (var msg in msgs)
					Log.Logger.Error(msg);
			}
		}
		public static TBuilder ModifyWith<TBuilder>(
			this TBuilder builder,
			IBuilderModifier<TBuilder> modifier)
			where TBuilder : IBuilder
		{
			modifier?.Modify(builder);
			return builder;
		}
	}
}