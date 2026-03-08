using System.Collections.Generic;

namespace BB
{
    public sealed class BoardValueContainer : IBoardValueContainer
    {
        public IBoard Board { get; private set; }
        public IBoardKey Key { get; private set; }
        public double Value { get; set; }
        public double AddedValue { get; set; }
        public double PreviousValue { get; set; }
        public Dictionary<IBoardValueCondition, double> _conditionalValues;

        public BoardValueContainer(IBoard board, IBoardKey key)
        {
            Board = board;
            Key = key;
        }

        public override string ToString()
            => $"{Value.Nicify()}; {PreviousValue.Nicify()}; {Key.Name}; {Board.Entity}";
    }

    public interface IBoardValueContainer
    {
        IBoard Board { get; }
        IBoardKey Key { get; }
        double Value { get; }
        public double PreviousValue { get; }
    }
}