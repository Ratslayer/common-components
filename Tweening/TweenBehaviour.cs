//using DG.Tweening;
//using System.Collections.Generic;

//public sealed class TweenBehaviour : BaseBehaviour
//{
//	readonly List<ITweenFactory> _tweenFactories = new();
//	private void Awake()
//	{
//		_tweenFactories.AddRange(GetComponents<ITweenFactory>());
//	}
//	public Tween Tween(Entity entity, bool invert)
//	{
//		var result = DOTween.Sequence();
//		foreach (var factory in _tweenFactories)
//		{
//			var tween = invert
//				? factory.CreateInvertedTween(entity)
//				: factory.CreateTween(entity);
//			if (tween is not null)
//				result.Join(tween);
//		}
//		return result;
//	}
//}
