using DG.Tweening;
namespace BB
{
	public sealed class LinkToEntityTweenComponent
		: TweenComponent<LinkToEntityTweenComponent>
	{
		public Entity _entity;
		protected override void Update()
		{
			if (!_entity)
			{
				_tween.Kill();
				return;
			}
			base.Update();
		}
	}
}