using Sirenix.OdinInspector;
using System;
using UnityEngine;
namespace BB
{
	[Serializable]
	public sealed class SerializedResourceBoardValueWithStats
		: AbstractSerializedBoardValue<BoardResource>
	{
		[SerializeField, ShowIf(nameof(_key))]
		double _maxValue, _gainPerSecond;
		public override void Add(in AddBoardContext context = default)
		{
			if (!_key)
				return;

			_key.MaxValueKey.Add(_maxValue, context);
			_key.GenRateKey.Add(_gainPerSecond, context);

			base.Add(context);
		}
	}
	[Serializable]
	public sealed class SerializedResourceBoardValue
		: AbstractSerializedBoardValue<BoardResource>
	{
	}
}