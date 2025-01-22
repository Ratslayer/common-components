using DG.Tweening;
using UnityEngine;
namespace BB
{
	public sealed class ScaleTransformTweenAsset : AbstractTransformTweenAsset
	{
		public TweenCurve _curve = new();

		public override Tween CreateTween(Transform transform, Vector3 target)
			=> transform.DOScale(target, _curve).Apply(_curve);
	}
}