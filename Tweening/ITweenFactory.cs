using DG.Tweening;
namespace BB
{
	public interface ITweenFactory
	{
		Tween CreateTween(Entity entity);
		Tween CreateInvertedTween(Entity entity);
	}
}