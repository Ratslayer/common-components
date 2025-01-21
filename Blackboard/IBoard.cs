using BB.Di;
using System.Collections.Generic;
namespace BB
{
	public interface IBoard : IFlushable
	{
		Entity Entity { get; }
		IBoardValueContainer GetOrCreate(IBoardKey key);
		bool Has(IBoardKey key, out IBoardValueContainer wrapper);
		double GetValue(IBoardKey key, in GetBoardContext context);
		void AddProcessor(IBoardProcessor processor);
		void RemoveProcessor(IBoardProcessor processor);
		IEnumerable<IBoardValueContainer> Values { get; }
	}
}