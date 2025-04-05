using DG.Tweening;
namespace BB
{
	public abstract class TweenComponent<TSelf> : PooledObject<TSelf>
		where TSelf : TweenComponent<TSelf>, new()
	{
		readonly TweenCallback _update, _kill;
		protected Tween _tween;
		protected TweenComponent()
		{
			_update = Update;
			_kill = Kill;
		}
		public void Bind(Tween tween)
		{
			_tween = tween;

			tween.onUpdate -= _update;
			tween.onUpdate += _update;

			tween.onKill -= _kill;
			tween.onKill += _kill;
		}
		protected virtual void Update() { }
		protected virtual void Kill()
		{
			_tween.Kill();
			Dispose();
		}
		public override void Dispose()
		{
			base.Dispose();

			_tween.onUpdate -= _update;
			_tween.onKill -= _kill;

		}
	}
}