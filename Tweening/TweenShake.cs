using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;
namespace BB
{
	[Serializable]
	public sealed class TweenShake : BaseSerializedTween
	{
		public Vector3 _strength = new(0.1f, 0, 0.1f);
		public float _randomness = 50;
		public int _vibrato = 10;
	}

	public static class TweenShakeExtensions
	{
		public static Tween ShakePos(
			this TweenShake tween,
			in TransformAdapter transform)
		{
			var t = transform._transform;
			if (!t)
				return null;

			var shake = t.DOShakePosition(
				tween.Duration,
				tween._strength,
				tween._vibrato,
				tween._randomness, 
				fadeOut: false);

			if (!tween.IsCustom && tween.Ease == Ease.Unset)
				return shake;

			var sequence = DOTween
				.Sequence()
				.Join(shake)
				.SetEase(tween);
			return sequence;
		}
		public static UniTask ShakePos(
			this TweenShake tween,
			in TransformAdapter transform,
			CancellationToken ct)
			=> tween.ShakePos(transform).ToUniTask(ct);
	}
}