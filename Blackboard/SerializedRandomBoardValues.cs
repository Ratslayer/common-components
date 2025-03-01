using System;
using UnityEngine;
namespace BB
{
	[Serializable]
	public sealed class SerializedRandomBoardValues : IBoardValuesProvider
	{
		[SerializeField]
		SerializedRandomBoardValue[] _values = { };

		public void Add(in AddBoardContext context)
		{
			using var _ = context._board.FlushOnDispose();
			foreach (var v in _values)
				context
					.WithKey(v._key)
					.WithMultiplier(v._value.GetRandom())
					.Add();
		}
	}
	[Serializable]
	public sealed class SerializedRandomBoardValue
	{
		public BaseBoardKey _key;
		public FloatRange _value;
	}
}