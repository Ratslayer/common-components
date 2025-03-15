using DG.Tweening;
using UnityEngine;
namespace BB
{
	public static class ITweenCurveExtensions
	{
		public static Tween MoveTo(
			this ITweenCurve curve, in TransformProvider transform, Vector3 position)
			=> transform._transform.DOMove(position, curve.Duration).Apply(curve);
		public static Tween MoveAdd(
			this ITweenCurve curve, in TransformProvider transform, Vector3 offset)
			=> transform._transform.DOMove(
				transform._transform.position + offset,
				curve.Duration)
			.Apply(curve);
		public static Tween Alpha(
			this ITweenCurve curve, AlphaProvider alphaProvider, float alpha)
			=> DOTween.To(
				() => alphaProvider.Alpha,
				a => alphaProvider.Alpha = a,
				alpha,
				curve.Duration)
			.Apply(curve);
	}
}