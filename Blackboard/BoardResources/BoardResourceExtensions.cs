namespace BB
{
	public static class BoardResourceExtensions
	{
		public static void SetToMax(this IBoard board, IBoardResourceKey resc)
			 => board.Add(resc, 1e100);
		public static void TryAdd(this IBoardResourceValue resc, double value)
			=> resc.ResourceKey.TryAdd(value, resc.Board);
		public static bool TryAdd(this IBoardResourceKey resc, double value, IBoard board)
		{
			if (resc is null)
				return false;

			if (value.IsZero())
				return true;


			var currentValue = board.Get(resc);

			if (value < 0)
			{
				if (currentValue - value < 0)
					return false;
				board.Add(resc, value);
				return true;
			}

			var maxValue = board.Get(resc);
			if (maxValue < value + currentValue)
				return false;
			board.Add(resc, value);
			return true;
		}
		public static bool AdvanceGeneration(this IBoard board, IBoardResourceKey resc, float deltaTime)
		{
			var genStat = board.Get(resc.GenRateKey);
			var genValue = genStat * deltaTime;
			if (!genValue.IsZero())
			{
				board.Add(resc, genValue);
				return true;
			}
			return false;
		}
	}
}