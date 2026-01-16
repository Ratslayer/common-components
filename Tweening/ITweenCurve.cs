using DG.Tweening;
using UnityEngine;
namespace BB
{
    public interface ITweenCurve
    {
        float Duration { get; }
        bool IsCustom { get; }
        Ease Ease { get; }
        AnimationCurve Curve { get; }
    }
}