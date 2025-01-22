//using DG.Tweening;
//using System;
//using UnityEngine;
//namespace BB.SerializedActions
//{
//	[Serializable]
//	public sealed class RotateTransformAction : TweenTransformAction
//	{
//		public Transform _rotationTarget;
//		public TweenCurve _curve = new();
//		protected override Tween TweenTransform(Transform t)
//		{
//			return t
//				.DORotateQuaternion(_rotationTarget.rotation, _curve)
//				.Apply(_curve);
//		}
//	}
//	[Serializable]
//	public sealed class MoveTransformAction : TweenTransformAction
//	{
//		public Transform _moveTarget;
//		public TweenCurve _curve = new();
//		protected override Tween TweenTransform(Transform t)
//		{
//			return t
//				.DOMove(_moveTarget.position, _curve)
//				.Apply(_curve);
//		}
//	}
//	[Serializable]
//	public sealed class ScaleTransformAction : TweenTransformAction
//	{
//		public Vector3 _targetScale;
//		public TweenCurve _curve = new();
//		protected override Tween TweenTransform(Transform t)
//		{
//			return t
//				.DOScale(_targetScale, _curve)
//				.Apply(_curve);
//		}
//	}
//}