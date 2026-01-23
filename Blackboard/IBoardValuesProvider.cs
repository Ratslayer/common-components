using System.Collections.Generic;

namespace BB
{
	public interface IBoardValuesProvider
	{
		IEnumerable<IBoardValue> GetBoardValues();
	}
	public readonly struct CanAddBoardValuesContext
	{
		public AddBoardContext BoardContext { get; init; }
		public List<IBoardKey> ErrorKeys { get; init; }
		public double? Multiplier { get; init; }
	}
}