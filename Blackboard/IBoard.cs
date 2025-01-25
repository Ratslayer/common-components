using System.Collections.Generic;
namespace BB
{
	public interface IBoard : IFlushable
	{
		Entity Entity { get; }
		IConditionalBoardValues ConditionalValues { get; }
		void Add(in AddBoardContext context);
		double Get(in GetBoardContext context);
		void AddProcessor(IBoardProcessor processor);
		void RemoveProcessor(IBoardProcessor processor);
		IEnumerable<IBoardKey> Keys { get; }
		void InitKey(IBoardKey key);
	}
}