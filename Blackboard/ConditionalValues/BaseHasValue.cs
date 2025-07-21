using Sirenix.OdinInspector;
using UnityEngine;

namespace BB.Serialized.Board.Conditions
{
	public abstract class BaseHasValue : ISerializedValueCondition
	{
		[SerializeField]
		protected BaseBoardKey _key;

		[SerializeField, InlineProperty]
		BoardValuePredicate _predicate = BoardValuePredicateType.Exists;

		public bool IsValid(BoardContext context)
		{
			if (!_key)
				return true;

			var value = GetValue(context);
			var result = _predicate.Compare(value);
			return result;
		}
		protected abstract double GetValue(BoardContext context);
	}
}
namespace BB
{
}