//using Sirenix.OdinInspector;
//using System;
//using UnityEngine;
//namespace BB.SerializedActions
//{
//	[Serializable]
//	public sealed class ApplyTweenBehaviourToEntity : IEntitySerializedAction
//	{
//		[Required]
//		[SerializeField] TweenBehaviour _tween;
//		[SerializeField] bool _invert;
//		public void Trigger(Entity entity)
//		{
//			if (_tween)
//				_tween.Tween(entity, _invert);
//		}
//	}
//}