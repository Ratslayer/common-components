using System;

namespace BB.Serialized.Board.Conditions
{
	[Serializable]
	public sealed class TargetHasValue : BaseHasValue
	{
		protected override double GetValue(BoardContext context)
			=> context
			.GetPooledCopy()
			.WithSwappedBoards()
			.GetAndDispose();
	}
}