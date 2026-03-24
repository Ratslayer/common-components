using System;

namespace BB.Blackboard.Serialization
{
    [Serializable]
    public sealed class TargetHasBoardValue : ISerializedBoardValueCondition
    {
        public BaseBoardKey _key;

        public bool IsValid(IBoard board, in GetBoardContext context)
        {
            if (!_key)
                return false;
            if (context.TargetBoard is null)
                return false;

            var value = context.TargetBoard.Get(new GetBoardContext
            {
                Key = _key,
                Multiplier = context.Multiplier
            });
            return value.IsPositive();
        }
    }
}