using DG.Tweening;

public readonly struct TweenCancellationToken
{
	readonly Tween _tween;
	readonly ulong _id;
	public TweenCancellationToken(Tween tween)
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
