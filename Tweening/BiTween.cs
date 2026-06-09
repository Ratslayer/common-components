using DG.Tweening;
using UnityEngine;

namespace BB
{
    public sealed class PunchTween
    {
        private readonly float _from, _to, _strength;
        private readonly Tween _fadeTween, _punchTween;
        private float _punchValue, _fadeValue;

        public PunchTween(float from, float to, TweenPunch punch)
        {
            _from = from;
            _to = to;
            _strength = punch._strength;

            _punchTween = DOTween
                .Punch(
                    () => _punchValue * Vector3.right,
                    v => _punchValue = v.x,
                    Vector3.right,
                    punch.Duration,
                    punch._vibrato,
                    punch._elasticity)
                .SetAutoKill(false)
                .Pause();

            _fadeTween = DOTween
                .To(v => _fadeValue = v,
                    1,
                    0,
                    punch.Duration)
                .SetAutoKill(false)
                .Pause();
        }

        public float Value => _strength * (_punchValue + _fadeValue) * (_to - _from) + _from;
        public static implicit operator float(PunchTween t) => t.Value;

        public void Punch()
        {
            if (_punchTween.IsPlaying())
            {
                _fadeValue = _punchValue;
                _fadeTween.Restart();
            }

            _punchTween.Restart();
        }
    }

    public sealed class BiTween
    {
        private readonly Tween _currentTween;
        private bool _lastMoveForward;
        private readonly float _from, _to;
        private float _blend;

        public BiTween(float from, float to, TweenCurve curve)
        {
            _from = from;
            _to = to;
            _currentTween = DOTween.To(
                    x => _blend = x,
                    0,
                    1f,
                    curve.Duration)
                .SetEase(curve)
                .SetAutoKill(false)
                .Pause();
        }

        public float Value => _blend * (_to - _from) + _from;

        public void PlayForward()
        {
            if (_lastMoveForward)
                return;
            _lastMoveForward = true;
            _currentTween.PlayForward();
        }

        public void PlayBackward()
        {
            if (!_lastMoveForward)
                return;
            _lastMoveForward = false;
            _currentTween.PlayBackwards();
        }

        public static implicit operator float(BiTween t) => t.Value;
    }
}