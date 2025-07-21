//using System;
//using UnityEngine;
//namespace BB
//{
//	[Serializable]
//	public sealed class SerializedRandomBoardValues : IBoardValuesProvider
//	{
//		[SerializeField]
//		SerializedRandomBoardValue[] _values = { };

//		public BuffDisposable Add(BoardContext context)
//		{
//			using var _ = context._board.FlushOnDispose();
//			foreach (var v in _values)
//				context
//					.WithKey(v._key)
//					.WithMultiplier(v._value.GetRandom())
//					.Add();
//			return BuffDisposable.GetPooled(context._board, this);
//		}
//	}
//	[Serializable]
//	public sealed class SerializedRandomBoardValue
//	{
//		public BaseBoardKey _key;
//		public FloatRange _value;
//	}
//}