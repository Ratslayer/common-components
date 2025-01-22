using DG.Tweening;

public readonly struct TweenCancelationToken
{
	readonly Tween _tween;
	readonly ulong _id;
	public TweenCancelationToken(Tween tween)
	{
		_tween = tween;
		_id = PooledIdsUtils.AddToPool(_tween);
		_tween.OnComplete(Clear).OnKill(Clear);
	}
	void Clear() => PooledIdsUtils.RemoveFromPool(_tween, _id);
	public void Cancel()
	{
		if (PooledIdsUtils.RemoveFromPool(_tween, _id))
			_tween.Kill();
	}
}
