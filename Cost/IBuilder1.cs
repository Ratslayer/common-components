namespace BB
{
	public interface IBuilder<TContext, TComponent, TResult> : IBuilder
	{
		TContext Context { get; }
		void AddComponent(TComponent component);
		TResult Build();
	}
}