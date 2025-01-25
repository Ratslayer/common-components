using BB.Di;
using System.Collections.Generic;
using UnityEngine;
namespace BB
{
	public sealed class BoardStat : BaseBoardKey, IBlackboardStatKey
	{
		enum AdditionType
		{
			Additive = 0,
			Multiplicative = 1
		}
		[SerializeField] 
		AdditionType _additionType;
		[SerializeField]
		BaseBoardKey[] _multipliers = { };
		public IEnumerable<IBoardKey> Multipliers => _multipliers;

		public override IBoardValueContainer CreateValue(IBoard board)
			=> new StatBoardValue(this, board);
		public override double AddValues(double v1, double v2)
			=> _additionType switch
			{
				AdditionType.Multiplicative => (1 + v1) * (1 + v2) - 1,
				_ => v1 + v2
			};
	}
}