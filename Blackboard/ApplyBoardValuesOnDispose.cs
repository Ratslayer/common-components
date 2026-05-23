using System.Collections.Generic;

namespace BB
{
    public sealed class AddBoardValueOnDispose : ProtectedPooledObject<AddBoardValueOnDispose>
    {
        IBoard _board;
        BoardValue _value;
        object _source;

        public static AddBoardValueOnDispose GetPooled(IBoard board, BoardValue value, object source)
        {
            var result = GetPooledInternal();
            result._board = board;
            result._value = value;
            result._source = source;
            return result;
        }

        public override void Dispose()
        {
            if (_value)
                _board.Add(_value, _source);
            base.Dispose();
        }
    }

    public sealed class ApplyBoardValuesOnDispose : ProtectedPooledObject<ApplyBoardValuesOnDispose>
    {
        readonly List<BoardValue> _values = new();
        private object _source;
        private IBoard _board;

        public static ApplyBoardValuesOnDispose GetPooled(
            IBoard board,
            BoardValues values,
            object source)
        {
            var result = GetPooledInternal();
            result._values.AddRange(values.Values);
            result._board = board;
            result._source = source;
            return result;
        }

        public override void Dispose()
        {
            _board.Add(
                new BoardValues
                {
                    Values = _values
                }, 
                _source);
            _values.DisposeElementsAndClear();
            _board = default;
            _source = default;
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