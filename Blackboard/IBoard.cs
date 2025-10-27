using System.Collections.Generic;
namespace BB
{
	public interface IBoard : IAutoFlushable
	{
		Entity Entity { get; }
		void Add(in AddBoardContext context);
		double Get(in GetBoardContext context);
		void Set(IBoardKey key, double value);
		void Add(IBoardKey key, IBoardValueCondition condition, double value);
		void AddProcessor(IBoardProcessor processor);
		void RemoveProcessor(IBoardProcessor processor);
		IReadOnlyCollection<IBoardKey> Keys { get; }
		IReadOnlyCollection<IBoardValueContainer> Containers { get; }
        IReadOnlyCollection<IBoardValueContainer> DirtyContainers { get; }
		void UpdateGeneration(float seconds);
	}
}