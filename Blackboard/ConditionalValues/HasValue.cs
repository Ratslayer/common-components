using System;

namespace BB.Serialized.Board.Conditions
{

	[Serializable]
	public sealed class HasValue : BaseHasValue
	{
		protected override double GetValue(in GetBoardContext context)
			=> context.WithKey(_key).Apply();
	}
}