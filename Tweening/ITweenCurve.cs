using DG.Tweening;

public interface ITweenCurve
{
	void Apply(Tween tween);
	float Duration { get; }
}
