using System;

namespace BB.Blackboard.Serialization
{
	[Serializable]
    public sealed class HasBoardValue : ISerializedBoardValueCondition
    {
        public BaseBoardKey _key;
        public bool IsValid(in GetBoardContext context)
        {
            if (!_key)
                return false;
            var value = context
                .WithKey(_key)
                .Get();
            return value.IsPositive();
        }
    }
}
