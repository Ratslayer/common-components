using DG.Tweening;
using UnityEngine;

public sealed class RotateTransformTweenAsset : AbstractTransformTweenAsset
{
	public TweenCurve _curve = new();

	public override Tween CreateTween(Transform transform, Vector3 target)
		=> transform.DOPunchScale(target, _curve).Apply(_curve);
}
