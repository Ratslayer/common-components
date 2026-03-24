using System;

namespace BB.Blackboard.Serialization
{
    [Serializable]
    public sealed class HasBoardValue : ISerializedBoardValueCondition
    {
        public BaseBoardKey _key;

        public bool IsValid(IBoard board, in GetBoardContext context)
        {
            if (!_key)
                return false;
            var value = board.Get(context.WithKey(_key));
            return value.IsPositive();
        }
    }
}