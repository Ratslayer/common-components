using System.Collections.Generic;

namespace BB
{
	public interface IBoardValuesProvider
	{
		void Add(in AddBoardContext context);
		bool CanAdd(in CanAddBoardValuesContext context);
	}
	public readonly struct CanAddBoardValuesContext
	{
		public AddBoardContext BoardContext { get; init; }
		public List<IBoardKey> ErrorKeys { get; init; }
		public double? Multiplier { get; init; }
	}
}