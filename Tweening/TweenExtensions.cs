using DG.Tweening;
using TMPro;
using UnityEngine;
namespace BB
{
	public static class TweenExtensions
	{
		public static Tween LinkToEntity(
			this Tween tween,
			Entity entity)
		{
			var linkComponent = LinkToEntityTweenComponent.GetPooled();
			linkComponent._entity = entity;
			linkComponent.Bind(tween);
			return tween;
		}
		public static T SetEase<T>(this T tween, ITweenCurve curve)
			where T : Tween
		{
			if (curve.IsCustom)
			{
				tween.SetEase(curve.Curve);
				return tween;
			}
			tween.SetEase(curve.Ease);
			return tween;
		}

		public static TweenToken GetToken(this Tween tween) => new(tween);
		public static Tween OnEnd(this Tween tween, TweenCallback action)
			=> tween
			.OnKill(action)
			.OnComplete(action);
		public static Tween TweenAlpha(this CanvasGroup cg, float value, TweenCurve curve)
			=> cg
			.DOFade(value, curve.Duration)
			.SetEase(curve);
		public static Tween TweenPos(
			this Root root,
			Vector3 position,
			TweenCurve curve)
			=> root.Transform
			.DOMove(position, curve.Duration)
			.SetEase(curve);
		public static Tween TweenFont(this TextMeshProUGUI tmp, float font, TweenCurve curve)
			=> DOTween
			.To(() => tmp.fontSize, f => tmp.fontSize = f, font, curve.Duration)
			.SetEase(curve);
		public static void Forget(this Tween _) { }
	}
}