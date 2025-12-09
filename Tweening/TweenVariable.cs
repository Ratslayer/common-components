using DG.Tweening;
namespace BB
{
	public abstract class TweenVariable<TSelf> : Variable<TSelf, Tween>
		where TSelf : TweenVariable<TSelf>
	{
	}
	public static class TweenVariableExtensions
	{
		public static Tween Set<TVar>(this Tween tween, in Entity entity)
			where TVar : TweenVariable<TVar>
		{
			if (!entity.Has(out TVar v))
				return tween;

			v.Value?.Kill();
			v.Value = tween;
			return tween;
		}
	}
}