using System.Collections.Generic;

namespace BB
{
    public sealed class AddBoardValueOnDispose : ProtectedPooledObject<AddBoardValueOnDispose>
    {
        IBoard _board;
        IBoardKey _key;
        double _value;
        object _source;

        public static AddBoardValueOnDispose GetPooled(IBoard board, IBoardKey key, object source, double value)
        {
            var result = GetPooledInternal();
            result._board = board;
            result._key = key;
            result._value = value;
            result._source = source;
            return result;
        }

        public override void Dispose()
        {
            if (_key is not null
                && _board is not null
                && _value.NotZero())
                Board.Add(_board, _key, _source, _value);
            base.Dispose();
        }
    }

    public sealed class ApplyBoardValuesOnDispose : ProtectedPooledObject<ApplyBoardValuesOnDispose>
    {
        readonly List<IBoardValue> _values = new();
        private object _source;
        private IBoard _board;
        private double _multiplier;

        public static ApplyBoardValuesOnDispose GetPooled(IBoard board, IEnumerable<IBoardValue> values, object source,
            double multiplier)
        {
            var result = GetPooledInternal();
            result._values.AddRange(values);
            result._board = board;
            result._source = source;
            result._multiplier = multiplier;
            return result;
        }

        public override void Dispose()
        {
            Board.Add(_board, _values, _source, _multiplier);
            _values.DisposeElementsAndClear();
            _board = default;
            _source = default;
            _multiplier = default;
            base.Dispose();
        }

        // public ApplyBoardValuesOnDispose WithContext(in AddBoardContext context)
        // {
        //     _context = context;
        //     return this;
        // }
        //
        // public ApplyBoardValuesOnDispose WithValues(IEnumerable<IBoardValue> values)
        // {
        //     _values.AddRange(values);
        //     return this;
        // }
    }
}