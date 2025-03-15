using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
namespace BB
{
	public interface ITweenCurve
	{
		void Apply(Tween tween);
		float Duration { get; }
	}
}