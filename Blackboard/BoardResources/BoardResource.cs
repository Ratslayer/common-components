using System.Collections.Generic;
using UnityEngine;
namespace BB
{
	public sealed class BoardResource : BaseBoardKey, IBoardKeyWithMultipliers, IBoardKeyWithGeneration
	{
		[SerializeField]
		BaseBoardKey _genValue;
		[SerializeField]
		BaseBoardKey[] _gainMultipliers;

		public IBoardKey GenRateKey => _genValue;

		public IReadOnlyCollection<IBoardKey> Multipliers => _gainMultipliers;

		public BoardEventUsage MultiplierUsage => BoardEventUsage.Set;

		public override BoardEventUsage ClampingUsage => BoardEventUsage.Set;

		public double GetGenerationValue(IBoard board)
		{
			return _genValue.Get(board);
		}

		public bool HasGenerationKey(out IBoardKey key)
		{
			key = _genValue;
			return _genValue;
		}
	}
}
