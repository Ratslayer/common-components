using DG.Tweening;
using UnityEngine;
namespace BB
{
	public sealed class RotateTransformTweenAsset : AbstractTransformTweenAsset
	{
		public TweenCurve _curve = new();

		public override Tween CreateTween(Transform transform, Vector3 target)
			=> transform
			.DOPunchScale(target, _curve.Duration)
			.SetEase(_curve);
	}
}