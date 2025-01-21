namespace BB
{
	public static class ICostExtensions
	{
		public static bool TrySpend(this ICost cost)
		{
			if (cost.CanSpend())
			{
				cost.Spend();
				return true;
			}
			return false;
		}
	}
}