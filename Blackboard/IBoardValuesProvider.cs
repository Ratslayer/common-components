using System.Collections.Generic;

namespace BB
{
	public interface IBoardValuesProvider
	{
		void AddBoardValues(BoardContext context);
		bool CanAdd(in CanAddBoardValuesContext context);
	}
	public readonly struct CanAddBoardValuesContext
	{
		public BoardContext BoardContext { get; init; }
		public List<IBoardKey> ErrorKeys { get; init; }
		public double? Multiplier { get; init; }
	}
}