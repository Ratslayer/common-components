using DG.Tweening;
using DG.Tweening.Core;
using System;
namespace BB
{
	[Serializable]
	public sealed class TweenPunch : BaseSerializedTween
	{
		public float _strength = 0.1f;
		public int _vibrato = 10;
		public float _elasticity = 50;
    }
}