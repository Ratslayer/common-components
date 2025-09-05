using System.Collections.Generic;

namespace BB
{
	public interface IBoardValuesProvider
	{
		void AddBoardValues(BoardContext context);
		bool CanAdd(BoardContext context, List<IBoardKey> errorKeys);
	}
}