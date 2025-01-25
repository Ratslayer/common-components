namespace BB
{
	public interface ISpendCostComponent : ICostComponent
	{
		void Spend(ICostContext context);
	}
}