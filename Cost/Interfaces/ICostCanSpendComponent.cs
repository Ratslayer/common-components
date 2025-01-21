namespace BB
{
	public interface ICostCanSpendComponent
	{
		bool CanSpend(in CostBuilderContext context);
	}
}