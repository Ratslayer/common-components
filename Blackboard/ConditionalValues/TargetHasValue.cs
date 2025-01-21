using System;

namespace BB.Serialized.Board.Conditions
{
	[Serializable]
	public sealed class TargetHasValue : BaseHasValue
	{
		protected override double GetValue(in GetBoardContext context)
			=> _key.Get(context.Inverse());
	}
}