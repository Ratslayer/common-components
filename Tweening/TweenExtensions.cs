using DG.Tweening;
using UnityEngine;
namespace BB
{
	public static class TweenExtensions
	{
		public static Tween Apply(this Tween tween, ITweenCurve curve)
		{
			curve.Apply(tween);
			return tween;
		}
		public static TweenCancelationToken GetToken(this Tween tween) => new(tween);
		public static Tween OnEnd(this Tween tween, TweenCallback action)
			=> tween
			.OnKill(action)
			.OnComplete(action);
		public static Tween Alpha(this CanvasGroup cg, float value, TweenCurve curve)
			=> cg
			.DOFade(value, curve.Duration)
			.Apply(curve);
		public static Tween Move(
			this Root root,
			Vector3 position,
			TweenCurve curve)
			=> root.Transform
			.DOMove(position, curve.Duration)
			.Apply(curve);
	}
}