using DG.Tweening;
namespace BB
{
	public interface ITweenCurve
	{
		void Apply(Tween tween);
		float Duration { get; }
	}
}