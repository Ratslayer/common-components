using DG.Tweening;
using UnityEngine;

public static class TweenUtils
{
	public static Tween Apply(this Tween tween, ITweenCurve curve)
	{
		curve.Apply(tween);
		return tween;
	}
	//public static Tween Bind(this Tween tween, Entity entity)
	//{
	//	entity._ref.Despawned += OnDelete;
	//	return tween.OnKill(OnComplete).OnComplete(OnComplete);
	//	void OnComplete() => entity._ref.Despawned -= OnDelete;
	//	void OnDelete()
	//	{
	//		OnComplete();
	//		tween.Kill();
	//	}
	//}
	public static TweenCancelationToken GetToken(this Tween tween) => new(tween);
	public static Tween OnEnd(this Tween tween, TweenCallback action)
		=> tween
		.OnKill(action)
		.OnComplete(action);
	public static Tween Alpha(this CanvasGroup cg, float value, TweenCurve curve)
		=> cg
		.DOFade(value, curve.Duration)
		.Apply(curve);
}