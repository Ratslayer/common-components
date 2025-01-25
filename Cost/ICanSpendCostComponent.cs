namespace BB
{
	public interface ICanSpendCostComponent : ICostComponent
	{
		bool CanSpend(ICostContext context);
	}
}