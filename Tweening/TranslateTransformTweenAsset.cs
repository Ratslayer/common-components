using DG.Tweening;
using UnityEngine;

public sealed class TranslateTransformTweenAsset : AbstractTransformTweenAsset
{
	public TweenCurve _curve = new();

	public override Tween CreateTween(Transform transform, Vector3 target)
		=> transform.DOMove(target, _curve).Apply(_curve);
}
