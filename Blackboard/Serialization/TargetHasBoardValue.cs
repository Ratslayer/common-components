using System;

namespace BB.Blackboard.Serialization
{
	[Serializable]
    public sealed class TargetHasBoardValue : ISerializedBoardValueCondition
    {
        public BaseBoardKey _key;
        public bool IsValid(in GetBoardContext context)
        {
            if (!_key)
                return false;
            if (context.TargetBoard is null)
                return false;

            var value = context
                .WithSwappedBoards()
                .WithKey(_key)
                .Get();
            return value.IsPositive();
        }
    }
}
