using System.Collections.Generic;

namespace BB
{
    public interface IBoard : IAutoFlushable
    {
        Entity Entity { get; }
        void Add(in AddBoardContext context);
        double Get(in GetBoardContext context);
        void Set(in SetBoardContext context);
        void AddProcessor(IBoardProcessor processor);
        void RemoveProcessor(IBoardProcessor processor);
        IReadOnlyCollection<IBoardKey> Keys { get; }
        IReadOnlyCollection<IBoardValueContainer> Containers { get; }
        IReadOnlyCollection<IBoardValueContainer> DirtyContainers { get; }
    }
}