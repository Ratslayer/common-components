using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
namespace BB
{
	public abstract class BaseSerializedTween : ITweenCurve
	{
		[SerializeField, HorizontalGroup(50), HideLabel]
		float _duration = 1f;
		[ShowIf(nameof(_custom))]
		[SerializeField, HorizontalGroup, HideLabel]
		AnimationCurve _curve = new();
		[HideIf(nameof(_custom))]
		[SerializeField, HorizontalGroup, HideLabel]
		Ease _ease;
		[SerializeField, HorizontalGroup(17), HideLabel]
		bool _custom;

		public float Duration => _duration;
		public bool IsCustom=> _custom;
		public Ease Ease => _ease;
		public AnimationCurve Curve => _curve;
	}
	[Serializable, InlineProperty]
	public sealed class TweenCurve : BaseSerializedTween
	{
	}
}