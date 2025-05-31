using DG.Tweening;
using System;
using UnityEngine;
namespace BB
{
	[Serializable]
	public sealed class TweenShake
	{
		public Vector3 _strength;
		public float _randomness;
		public int _vibrato;
		public float _duration;

		public Vector3 Strength => _strength;
	}
	public static class TweenShakeExtensions
	{
		public static Tween ShakePos(this TweenShake tween, in TransformAdapter transform)
		{
			var t = transform._transform;
			if(!t)
				return null;
			return t.DOShakePosition(tween._duration, tween._strength, tween._vibrato, tween._randomness);
		}
	}
}