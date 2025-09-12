using System;
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
}