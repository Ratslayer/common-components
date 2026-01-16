using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using System.Threading;
using UnityEngine;
namespace BB
{
    public static class TweenPunchExtensions
    {
        public static Tween PunchPos(
            this TweenPunch tween, in TransformAdapter transform, Vector3 dir)
            => transform._transform
            .DOPunchPosition(
                tween._strength * dir,
                tween.Duration,
                tween._vibrato,
                tween._elasticity)
            .SetEase(tween.Ease);
        public static UniTask PunchPos(
            this TweenPunch tween,
            in TransformAdapter transform,
            Vector3 dir,
            CancellationToken ct)
            => tween.PunchPos(transform, dir).WithCancellation(ct);
        public static Tween PunchScale(this TweenPunch tween, in TransformAdapter transform, in ScaleAdapter scale)
            => transform._transform
            .DOPunchScale(
                scale._scale * tween._strength,
                tween.Duration,
                tween._vibrato,
                tween._elasticity)
            .SetEase(tween.Ease);
        public static UniTask PunchScale(
            this TweenPunch tween,
            in TransformAdapter transform,
            in ScaleAdapter scale,
            in CancellationToken ct)
            => tween.PunchScale(transform, scale).WithCancellation(ct);

        public static Tween PunchColor(this TweenPunch tween, ColorableAdapter colorable, in Color color)
            => DOTween
            .To(
                () => colorable.Color,
                c => colorable.Color = c,
                color,
                tween.Duration)
            .SetEase(tween.Ease);

        public static UniTask PunchColor(
            this TweenPunch tween,
            ColorableAdapter colorable,
            in Color color,
            in CancellationToken ct)
            => tween
            .PunchColor(colorable, color)
            .WithCancellation(ct);
    }
}