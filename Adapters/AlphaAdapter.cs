using TMPro;
using UnityEngine;

namespace BB
{
    public readonly struct AlphaAdapter
    {
        public enum ProviderType
        {
            TextMeshProUGUI,
            CanvasGroup
        }
        public readonly Object _object;
        public readonly ProviderType _type;
        public AlphaAdapter(Object obj, ProviderType type)
        {
            _object = obj;
            _type = type;
        }
        public static implicit operator AlphaAdapter(CanvasGroup group)
            => new(group, ProviderType.CanvasGroup);
        public static implicit operator AlphaAdapter(TextMeshProUGUI tmp)
            => new(tmp, ProviderType.TextMeshProUGUI);
        public static implicit operator AlphaAdapter(Root2D root)
            => root.CanvasGroup;
        public float Alpha
        {
            get => _type switch
            {
                ProviderType.CanvasGroup => ((CanvasGroup)_object).alpha,
                ProviderType.TextMeshProUGUI => ((TextMeshProUGUI)_object).alpha,
                _ => 1
            };
            set
            {
                switch (_type)
                {
                    case ProviderType.CanvasGroup:
                        ((CanvasGroup)_object).alpha = value;
                        break;
                    case ProviderType.TextMeshProUGUI:
                        ((TextMeshProUGUI)_object).alpha = value;
                        break;
                }
            }
        }
    }
}