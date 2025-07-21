using System;
using System.Linq;
using UnityEngine;
namespace BB
{
	[Serializable]
	public sealed class SerializedBoardValueWithConditions : AbstractSerializedBoardValue<BoardStat>, IBoardValueCondition
	{
		[SerializeReference]
		ISerializedValueCondition[] _conditions = { };

		public override void Add(BoardContext context)
		{
			if (_conditions.Length == 0)
				base.Add(context);
			else context.Board.Add(Key, this, Value);
		}

		public bool IsValid(BoardContext context)
		{
			foreach (var condition in _conditions)
				if (!condition.IsValid(context))
					return false;
			return true;
		}
	}
}