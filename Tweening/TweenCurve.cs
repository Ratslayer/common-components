using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
[Serializable, InlineProperty]
public sealed class TweenCurve : ITweenCurve
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
	public void Apply(Tween tween)
	{
		if (_custom)
		{
			tween.SetEase(_curve);
			return;
		}
		tween.SetEase(_ease);
	}
	public static implicit operator float(TweenCurve curve) => curve._duration;
	public static TweenCurve Default => new()
	{
		_custom = false,
		_ease = Ease.OutQuad,
		_duration = 0.5f
	};
}
namespace BB.SerializedActions
{
}