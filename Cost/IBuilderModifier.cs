namespace BB
{
	public interface IBuilderModifier<TBuilder>
	{
		void Modify(TBuilder builder);
	}
}