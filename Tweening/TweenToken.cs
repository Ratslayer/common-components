using BB;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;

public readonly struct TweenToken
{
    readonly Tween _tween;
    readonly ulong _id;
    public TweenToken(Tween tween)
    {
        _tween = tween;
        _id = PooledIdsUtils.AddToPool(_tween);
        _tween?.OnComplete(Clear).OnKill(Clear);
    }
    void Clear() => PooledIdsUtils.RemoveFromPool(_tween, _id);
    public void Cancel()
    {
        if (PooledIdsUtils.RemoveFromPool(_tween, _id))
            _tween.Kill();
    }
    public void OnEnd(Action action)
    {
        if (!this)
        {
            action();
            return;
        }
        _tween.OnEnd(() => action());
    }
    public static implicit operator bool(TweenToken token)
        => PooledIdsUtils.IsValid(token._tween, token._id);
    public static implicit operator UniTask(TweenToken token)
    {
        if (!token)
            return UniTask.CompletedTask;
        return token._tween.ToUniTask();
    }
}
