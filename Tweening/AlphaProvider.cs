using TMPro;
using UnityEngine;

namespace BB
{
	public readonly struct AlphaProvider
	{
		public enum ProviderType
		{
			TextMeshProUGUI,
			CanvasGroup
		}
		public readonly Object _object;
		public readonly ProviderType _type;
		public AlphaProvider(Object obj, ProviderType type)
		{
			_object = obj;
			_type = type;
		}
		public static implicit operator AlphaProvider(CanvasGroup group)
			=> new(group, ProviderType.CanvasGroup);
		public static implicit operator AlphaProvider(TextMeshProUGUI tmp)
			=> new(tmp, ProviderType.TextMeshProUGUI);
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