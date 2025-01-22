//using DG.Tweening;
//using UnityEngine;

//public sealed class MoveTween : TweenComponent
//{
//	[SerializeField] TweenCurve _moveCurve;
//	[SerializeField] Transform _start, _end;

//	public override Tween CreateInvertedTween(Entity entity)
//	{
//		return CreateMoveTween(entity, _start);
//	}

//	public override Tween CreateTween(Entity entity)
//	{
//		return CreateMoveTween(entity, _end);
//	}
//	Tween CreateMoveTween(Entity entity, Transform end)
//	{
//		if (!entity.Has(out Root root))
//			return null;
//		var tween = root.Transform.DOMove(end.position, _moveCurve).Apply(_moveCurve);
//		return tween;
//	}
//}
