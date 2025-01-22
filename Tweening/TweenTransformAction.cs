//using DG.Tweening;
//using System;
//using UnityEngine;

//namespace BB.SerializedActions
//{
//	[Serializable]
//	public abstract class TweenTransformAction : IEntitySerializedAction
//	{
//		public TransformTarget _transform = new();
//		TweenCancelationToken _token;
//		public void Trigger(Entity entity)
//		{
//			if (!_transform.HasTransform(entity, out var t))
//				return;
//			_token.Cancel();
//			_token = TweenTransform(t).GetToken();
//		}
//		protected abstract Tween TweenTransform(Transform t);
//	}
//}