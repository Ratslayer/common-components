namespace BB
{
	public static class BoardValuePredicateExtensions
	{
		public static bool Compare(this BoardValuePredicateType predicate, double value, double target)
		{
			var result = predicate switch
			{
				BoardValuePredicateType.Equals => value.Approximately(target),
				BoardValuePredicateType.NotEquals => !value.Approximately(target),
				BoardValuePredicateType.GreaterThan => value >= target,
				BoardValuePredicateType.LessThan => value <= target,
				BoardValuePredicateType.Exists => value >= 1,
				_ => false
			};
			return result;
		}
	}
}
