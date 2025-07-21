using System;

namespace BB.Serialized.Board.Conditions
{

	[Serializable]
	public sealed class HasValue : BaseHasValue
	{
		protected override double GetValue(BoardContext context)
			=> context
			.GetPooledCopy()
			.WithKey(_key)
			.GetAndDispose();
	}
}