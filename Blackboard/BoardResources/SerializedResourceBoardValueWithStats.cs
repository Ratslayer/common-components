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
		public override void Add(BoardContext context)
		{
			if (!_key)
				return;

			base.Add(context);

			context
				.GetPooledCopy()
				.WithKey(_key.GenRateKey)
				.WithValue(_gainPerSecond)
				.AddAndDispose();

			if (_key.HasMaxKey(out var maxKey))
				context
					.GetPooledCopy()
					.WithKey(maxKey)
					.WithValue(_maxValue)
					.AddAndDispose();
		}
	}
	[Serializable]
	public sealed class SerializedResourceBoardValue
		: AbstractSerializedBoardValue<BoardResource>
	{
	}
}