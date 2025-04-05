using DG.Tweening;
using UnityEngine;
namespace BB
{
	public static class ITweenCurveExtensions
	{
		public static Tween MoveTo(
			this ITweenCurve curve, in TransformAdapter transform, Vector3 position)
			=> transform._transform.DOMove(position, curve.Duration).Apply(curve);
		public static Tween MoveToTransform(
			this ITweenCurve curve, in TransformAdapter transform, in TransformAdapter target)
		{
			if (!transform || !target)
				return null;

			var from = transform._transform;
			var to = target._transform;
			var startingPos = from.position;
			return DOTween.To(GetPos, SetPos, 0, curve.Duration).Apply(curve);

			float GetPos() => 1;
			void SetPos(float value)
			{
				var diff = startingPos - to.position;
				var newPos = to.position + value * diff;
				from.position = newPos;
			}
		}
		public static Tween MoveAdd(
			this ITweenCurve curve, in TransformAdapter transform, Vector3 offset)
			=> transform._transform.DOMove(
				transform._transform.position + offset,
				curve.Duration)
			.Apply(curve);
		public static Tween Alpha(
			this ITweenCurve curve, AlphaAdapter alphaProvider, float alpha)
			=> DOTween.To(
				() => alphaProvider.Alpha,
				a => alphaProvider.Alpha = a,
				alpha,
				curve.Duration)
			.Apply(curve);
	}
}