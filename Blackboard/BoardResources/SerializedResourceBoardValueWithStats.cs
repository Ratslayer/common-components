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
		public override void Add(in AddBoardContext context)
		{
			if (!_key)
				return;

			base.Add(context);

			context
				.WithKey(_key.GenRateKey)
				.WithMultiplier(_gainPerSecond)
				.Add();

			context
				.WithKey(_key.MaxValueKey)
				.WithMultiplier(_maxValue)
				.Add();

			
		}
	}
	[Serializable]
	public sealed class SerializedResourceBoardValue
		: AbstractSerializedBoardValue<BoardResource>
	{
	}
}