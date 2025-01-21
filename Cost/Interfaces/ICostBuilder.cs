using System.Collections.Generic;

namespace BB
{
	public interface ICostBuilder : IBuilder<CostBuilderContext>
	{
		ICost Build();
	}
	public interface IBuilder<TContext>
	{
		TContext Context { get; }
		List<object> Components { get; }
	}
	public interface IBuilderContextMutator<TContext>
	{
		void MutateContext(TContext context);
	}
	public static class IBuilderUtils
	{
		public static void MutateContext<TContext>(IBuilder<TContext> builder)
		{
			foreach (var component in builder.Components)
				if (component is IBuilderContextMutator<TContext> mutator)
					mutator.MutateContext(builder.Context);
		}
	}
}