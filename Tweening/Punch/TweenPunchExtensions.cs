using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
namespace BB
{
	public static class TweenPunchExtensions
	{
		public static Tween PunchPos(
			this TweenPunch tween, in TransformAdapter transform, Vector3 dir)
			=> transform._transform
			.DOPunchPosition(
				tween._strength * dir,
				tween.Duration,
				tween._vibrato,
				tween._elasticity)
			.SetEase(tween.Ease);
		public static UniTask PunchPos(
			this TweenPunch tween,
			in TransformAdapter transform,
			Vector3 dir,
			CancellationToken ct)
			=> tween.PunchPos(transform, dir).WithCancellation(ct);

	}
}