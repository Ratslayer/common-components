namespace BB
{
	public static class BoardResourceExtensions
	{
		public static void SetToMax(this IBoardResourceKey resc, IBoard board)
			 => resc.Add(board, 1e100);
		public static void AddMaxAndFill(this IBoardResourceKey resc, IBoard board, double value)
		{
			resc.MaxValueKey.Add(board, value);
			resc.SetToMax(board);
		}
		public static double GetFraction(this IBoardResourceKey resc, IBoard board)
		{
			if (resc is null)
				return 0;

			var maxValue = resc.MaxValueKey.Get(board);
			if (maxValue.IsZero())
				return 0;

			var value = resc.Get(board);
			return value / maxValue;
		}
		public static void TryAdd(this IBoardResourceValue resc, double value)
			=> resc.ResourceKey.TryAdd(value, resc.Board);
		public static bool TryAdd(this IBoardResourceKey resc, double value, IBoard board)
		{
			if (resc is null)
				return false;

			if (value.IsZero())
				return true;


			var currentValue = resc.Get(board);

			if (value < 0)
			{
				if (currentValue - value < 0)
					return false;
				resc.Add(board, value);
				return true;
			}

			var maxValue = resc.Get(board);
			if (maxValue < value + currentValue)
				return false;
			resc.Add(board, value);
			return true;
		}
		public static bool AdvanceGeneration(this IBoardResourceKey resc, IBoard board, float deltaTime)
		{
			var genStat = resc.GenRateKey.Get(board);
			var genValue = genStat * deltaTime;
			if (!genValue.IsZero())
			{
				resc.Add(board, genValue);
				return true;
			}
			return false;
		}
	}
}