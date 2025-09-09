using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
namespace BB
{
	public static class ITweenCurveExtensions
	{
		public static Tween MoveTo(
			this ITweenCurve curve, in TransformAdapter transform, in Vector3Adapter position)
			=> transform._transform
			.DOMove(position, curve.Duration)
			.SetEase(curve);
		public static Tween MoveToTransform(
			this ITweenCurve curve, in TransformAdapter transform, in TransformAdapter target)
		{
			if (!transform || !target)
				return null;

			var from = transform._transform;
			var to = target._transform;
			var startingPos = from.position;
			return DOTween
				.To(GetPos, SetPos, 0, curve.Duration)
				.SetEase(curve);

			float GetPos() => 1;
			void SetPos(float value)
			{
				var diff = startingPos - to.position;
				var newPos = to.position + value * diff;
				from.position = newPos;
			}
		}
		public static UniTask MoveToTransform(
			this ITweenCurve curve,
			in TransformAdapter transform,
			in TransformAdapter target,
			CancellationToken ct)
			=> curve.MoveToTransform(transform, target).ToUniTask(cancellationToken: ct);
		public static Tween MoveAdd(
			this ITweenCurve curve, in TransformAdapter transform, Vector3 offset)
			=> transform._transform
			.DOMove(transform._transform.position + offset, curve.Duration)
			.SetEase(curve);
		public static UniTask MoveAdd(
			this ITweenCurve curve, in TransformAdapter transform, Vector3 offset, CancellationToken ct)
			=> curve.MoveAdd(transform, offset).ToUniTask(ct);
		public static Tween MoveY(
			this ITweenCurve curve, in TransformAdapter t, float target)
			=> t._transform
			.DOMoveY(target, curve.Duration)
			.SetEase(curve);
		public static UniTask MoveY(
			this ITweenCurve curve, in TransformAdapter t, float target, CancellationToken ct)
			=> curve.MoveY(t, target).ToUniTask(cancellationToken: ct);

		public static Tween MoveXZ(
			this ITweenCurve curve, TransformAdapter t, Vector3 target)
			=> DOTween
			.To(() => t._transform.position.Flat(),
				v => t._transform.position = new(v.x, t._transform.position.y, v.z),
				target,
				curve.Duration)
			.SetEase(curve);
		public static UniTask MoveXZ(
			this ITweenCurve curve, TransformAdapter t, Vector3 target, CancellationToken ct)
			=> curve.MoveXZ(t, target).ToUniTask(cancellationToken: ct);
		public static Tween Alpha(
			this ITweenCurve curve, AlphaAdapter alphaProvider, float alpha)
			=> DOTween
			.To(() => alphaProvider.Alpha,
				a => alphaProvider.Alpha = a,
				alpha,
				curve.Duration)
			.SetEase(curve);

		public static Tween RotateForward(
			this ITweenCurve curve,
			in TransformAdapter transform,
			Vector3 dir)
		{
			var t = transform._transform;
			var rotation = Quaternion.LookRotation(dir);
			return t
				.DORotateQuaternion(rotation, curve.Duration)
				.SetEase(curve);
		}

		public static Tween Scale(this ITweenCurve curve, in TransformAdapter transform, float scale)
			=> transform._transform
			.DOScale(scale, curve.Duration)
			.SetEase(curve);
		public static UniTask Scale(
			this ITweenCurve curve,
			in TransformAdapter transform,
			float scale,
			CancellationToken ct)
			=> curve.Scale(transform, scale).ToUniTask(cancellationToken: ct);

		public static Tween Alpha(this ITweenCurve curve, CanvasGroup group, float alpha)
			=> group
			.DOFade(alpha, curve.Duration)
			.SetEase(curve);

		public static UniTask Alpha(this ITweenCurve curve, CanvasGroup group, float alpha, CancellationToken ct)
			=> curve
			.Alpha(group, alpha)
			.ToUniTask(cancellationToken: ct);

		public static Tween MoveAnchored(this ITweenCurve curve, RectTransform t, Vector2 pos)
			=> t
			.DOAnchorPos(pos, curve.Duration)
			.SetEase(curve);

		public static UniTask MoveAnchored(this ITweenCurve curve, RectTransform t, Vector2 pos, CancellationToken ct)
			=> curve
			.MoveAnchored(t, pos)
			.ToUniTask(cancellationToken: ct);
	}
}