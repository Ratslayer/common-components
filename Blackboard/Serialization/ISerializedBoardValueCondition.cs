using System;

namespace BB.Board.Serialization
{
    public interface ISerializedBoardValueCondition : ISerializedBoardValueModifier, IBoardValueCondition
    {
    }
    public interface ISerializedBoardValueMultiplier
    {
        double GetMultiplier(in GetBoardContext context);
    }
    [Serializable]
    public sealed class ExpressionMultiplier : ISerializedBoardValueMultiplier
    {
        public EntityExpression _expression = new();

        public double GetMultiplier(in GetBoardContext context)
            => _expression.GetValue(context.Entity);
	}

    [Serializable]
    public sealed class BoardValueMultiplier : ISerializedBoardValueMultiplier
    {
        public BoardValueGetter _getter = new();

        public double GetMultiplier(in GetBoardContext context)
            => _getter.GetValue(context);
    }
}
