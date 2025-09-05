using System;

namespace BB.Board.Serialization
{
	public interface ISerializedBoardValueCondition : IBoardValueCondition
	{
	}
	[Serializable]
	public sealed class HasBoardValue : ISerializedBoardValueCondition
	{
		public BaseBoardKey _key;
		public bool IsValid(BoardContext context)
		{
			if (!_key)
				return false;
			var value = context
				.GetPooledCopy(context.StartingBoard)
				.WithKey(_key)
				.GetAndDispose();
			return value.IsPositive();
		}
	}
	[Serializable]
	public sealed class TargetHasBoardValue : ISerializedBoardValueCondition
	{
		public BaseBoardKey _key;
		public bool IsValid(BoardContext context)
		{
			if (!_key)
				return false;
			if (context.TargetBoard is null)
				return false;

			var value = context
				.GetPooledCopy(context.StartingBoard)
				.WithSwappedBoards()
				.WithKey(_key)
				.GetAndDispose();
			return value.IsPositive();
		}
	}
}
