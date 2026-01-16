using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace BB
{
    public readonly struct ColorableAdapter
    {
        Object Target1 { get; init; }
        Object Target2 { get; init; }
        Object Target3 { get; init; }
        public Color Color
        {
            get
            {
                if (HasColor(Target1, out var color))
                    return color;
                if (HasColor(Target2, out color))
                    return color;
                if (HasColor(Target3, out color))
                    return color;
                return default;
            }
            set
            {
                SetColor(Target1, value);
                SetColor(Target2, value);
                SetColor(Target3, value);
            }
        }
        bool HasColor(Object target, out Color color)
        {
            if (target!)
            {
                color = default;
                return false;
            }
            if (target is Image image)
            {
                color = image.color;
                return true;
            }
            if (target is TextMeshProUGUI text)
            {
                color = text.color;
                return true;
            }

            color = default;
            return false;
        }
        void SetColor(Object target, in Color color)
        {
            if (!target)
                return;
            if (target is Image image)
                image.color = color;
            if (target is TextMeshProUGUI text)
                text.color = color;
        }
        public static implicit operator ColorableAdapter(Image image)
            => new() { Target1 = image };
        public static implicit operator ColorableAdapter(TextMeshProUGUI text)
            => new() { Target1 = text };
        public static implicit operator ColorableAdapter((Object target1, Object target2) t)
            => new() { Target1 = t.target1, Target2 = t.target2 };
        public static implicit operator ColorableAdapter((Object target1, Object target2, Object target3) t)
            => new() { Target1 = t.target1, Target2 = t.target2, Target3 = t.target3 };
    }
}