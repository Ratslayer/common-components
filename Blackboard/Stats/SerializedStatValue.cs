using System;
using UnityEngine;
namespace BB
{
	[Serializable]
	public sealed class SerializedStatValue : AbstractSerializedBoardValue<Stat>, IBoardValueCondition
	{
		[SerializeReference]
		ISerializedValueCondition[] _conditions = { };

		public override void Add(in AddBoardContext context)
		{
			var container = context._board.GetOrCreate(Key);
			if (_conditions.IsNullOrEmpty() || container is not IBoardStatValue stat)
			{
				container.Add(Value, context);
				return;
			}

			var conditionalValue = new ConditionalBoardValue(Key, Value, this);
			stat.Add(conditionalValue, context);
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