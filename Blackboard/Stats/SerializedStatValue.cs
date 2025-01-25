using System;
using UnityEngine;
namespace BB
{
	[Serializable]
	public sealed class SerializedStatValue : AbstractSerializedBoardValue<BoardStat>, IBoardValueCondition
	{
		[SerializeReference]
		ISerializedValueCondition[] _conditions = { };

		public override void Add(in AddBoardContext context)
		{
			base.Add(context);

			var conditionalValue = new ConditionalBoardValue(context._board, Key, Value * context._value, this);
			context._board.ConditionalValues.Add(conditionalValue);
		}

		public bool IsValid(in GetBoardContext context)
		{
			foreach (var condition in _conditions)
				if (!condition.IsValid(context))
					return false;
			return true;
		}
	}
}