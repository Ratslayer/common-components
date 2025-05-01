using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine;
using BB.Di;
namespace BB
{
	public sealed class BoardResource : BaseBoardKey, IBoardResourceKey
	{
		[SerializeField]
		BaseBoardKey _genValue;
		[SerializeField]
		BaseBoardKey[] _gainMultipliers;

		public IBoardKey MaxValueKey => _max.Key;

		public IEnumerable<IBoardKey> GainMultiplierKeys => _gainMultipliers;

		public IBoardKey GenRateKey => _genValue;

		public override IBoardValueContainer CreateValue(IBoard board)
		{
			var resourceChanged = board.Entity.Require<IEvent<ResourceChangedEvent>>();
			var result = new ResourceBoardValue(board, this, resourceChanged);
			if (board.Entity.Has(out BoardResources resources))
				resources.Add(this);
			return result;
		}
	}
}
