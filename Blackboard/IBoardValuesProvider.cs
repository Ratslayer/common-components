using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BB
{
    public interface IBoardValuesProvider
    {
        BoardValues GetBoardValues();
    }

    public readonly struct BoardValues : IEnumerable<BoardValue>
    {
        public IEnumerable<BoardValue> Values { get; init; }

        public static implicit operator bool(BoardValues v)
            => v.Values?.Any() ?? false;

        public static BoardValues operator -(BoardValues v)
            => v * -1;

        public static BoardValues operator *(BoardValues boardValues, double multiplier)
        {
            return new()
            {
                Values = boardValues.Values?.Select(v => v * multiplier)
            };
        }

        public static BoardValues operator +(BoardValues v1, BoardValues v2)
        {
            return new()
            {
                Values = v1.Values?.Concat(v2.Values)
            };
        }

        public static implicit operator BoardValues(BoardValue[] values)
            => new() { Values = values };

        public static implicit operator BoardValues(List<BoardValue> values)
            => new() { Values = values };

        public static implicit operator BoardValues(PooledList<BoardValue> values)
            => new() { Values = values };

        public IEnumerator<BoardValue> GetEnumerator() => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();
    }
}