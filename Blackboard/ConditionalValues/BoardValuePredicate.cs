using Sirenix.OdinInspector;
using System;

namespace BB
{
	[Serializable]
	public struct BoardValuePredicate
	{
		[HideLabel, HorizontalGroup]
		public BoardValuePredicateType _type;
		[HideLabel, HorizontalGroup, ShowIf(nameof(ShowTarget))]
		public double _target;

		readonly bool ShowTarget => _type
			is not BoardValuePredicateType.None
			and not BoardValuePredicateType.Exists;

		public static implicit operator BoardValuePredicate(BoardValuePredicateType type)
			=> new() { _type = type };

		public bool Compare(double value) => _type.Compare(value, _target);
	}
}
