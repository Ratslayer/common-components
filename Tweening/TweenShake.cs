using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;
namespace BB
{
	[Serializable]
	public sealed class TweenShake
	{
		public Vector3 _strength = new(0.1f, 0, 0.1f);
		public float _randomness = 50;
		public int _vibrato = 10;
		public float _duration = 1;

		public Vector3 Strength => _strength;
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
			return t.DOShakePosition(tween._duration, tween._strength, tween._vibrato, tween._randomness);
		}
		public static UniTask ShakePos(
			this TweenShake tween,
			in TransformAdapter transform,
			CancellationToken ct)
			=> tween.ShakePos(transform).ToUniTask(ct);
	}
}