namespace BB
{
	public static class BoardResourceExtensions
	{
		public static void SetToMax(this IBoardResourceValue value)
			=> SetToMax(value.ResourceKey, value.Board);
		public static void SetToMax(this IBoardResourceKey resc, IBoard board)
			 => resc.Add(1e100, board);
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
				resc.Add(value, board);
				return true;
			}

			var maxValue = resc.Get(board);
			if (maxValue < value + currentValue)
				return false;
			resc.Add(value, board);
			return true;
		}
		public static bool AdvanceGeneration(this IBoardResourceValue value, float deltaTime)
		{
			var genStat = value.ResourceKey.GenRateKey.Get(value.Board);
			var genValue = genStat * deltaTime;
			if (!genValue.IsZero())
			{
				value.ResourceKey.Add(genValue, value.Board);
				return true;
			}
			return false;
		}
	}
}