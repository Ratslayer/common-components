using System.Collections.Generic;
using UnityEngine;
namespace BB
{
	public sealed class BoardStat : BaseBoardKey, IBoardKeyWithMultipliers
	{
		[SerializeField] 
		BoardValueStackingMethod _additionType;
		[SerializeField]
		BaseBoardKey[] _multipliers = { };
		public IReadOnlyCollection<IBoardKey> Multipliers => _multipliers;
		public override BoardValueStackingMethod StackingMethod => _additionType;
		public BoardEventUsage MultiplierUsage => BoardEventUsage.Get;
		public override BoardEventUsage ClampingUsage => BoardEventUsage.Get;
	}
}