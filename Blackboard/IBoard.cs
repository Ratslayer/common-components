using System.Collections.Generic;

namespace BB
{
    public interface IBoard : IAutoFlushable
    {
        Entity Entity { get; }
        void Add(in BoardValue value, object source);
        double Get(in GetBoardContext context);
        void Set(in BoardValue value, object source);
        IReadOnlyCollection<IBoardKey> Keys { get; }
        IReadOnlyCollection<IBoardValueContainer> Containers { get; }
        IReadOnlyCollection<IBoardValueContainer> DirtyContainers { get; }
    }
}