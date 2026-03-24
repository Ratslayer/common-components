using System;

namespace BB.Blackboard.Serialization
{
    public interface ISerializedBoardValueCondition : ISerializedBoardValueModifier, IBoardValueCondition
    {
    }

    public interface ISerializedBoardValueMultiplier
    {
        double GetMultiplier(IBoard board, in GetBoardContext context);
    }

    [Serializable]
    public sealed class ExpressionMultiplier : ISerializedBoardValueMultiplier
    {
        public EntityExpression _expression = new();

        public double GetMultiplier(IBoard board, in GetBoardContext context)
            => _expression.GetValue(board.Entity);
    }

    [Serializable]
    public sealed class BoardValueMultiplier : ISerializedBoardValueMultiplier
    {
        public SerializedBoardValueGetter _getter = new();

        public double GetMultiplier(IBoard board, in GetBoardContext context)
            => _getter.GetValue(board, context);
    }
}