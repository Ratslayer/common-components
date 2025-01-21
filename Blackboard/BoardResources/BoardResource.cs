using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine;
using BB.Di;
namespace BB
{
	public sealed class BoardResource : BaseBoardKey, IBoardResourceKey
	{
		[SerializeField]
		Stat _maxValue, _genValue;
		[SerializeField]
		BaseBoardKey[] _gainMultipliers;
		public StyleSheet _style;

		public IBoardKey MaxValueKey => _maxValue;

		public IEnumerable<IBoardKey> GainMultiplierKeys => _gainMultipliers;

		public IBoardKey GenRateKey => _genValue;

		public override IBoardValueContainer CreateValue(IBoard board)
		{
			var resourceChanged = board.Entity.Require<IEvent<ResourceChangedEvent>>();
			var result = new ResourceBoardValue(board, this, resourceChanged);
			if (board.Entity.Has(out BoardResources resources))
				resources.Add(result);
			return result;
		}
		public bool Has(IBoard board) => board.Has(this, out _);
	}
}
