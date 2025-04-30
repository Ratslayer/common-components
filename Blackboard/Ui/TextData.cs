using TMPro;
using UnityEngine;

namespace BB
{
	public readonly struct TextData
	{
		public string Text { get; init; }
		public Color Color { get; init; }
		public float FontSize { get; init; }

		public void Apply(TextMeshProUGUI tmp)
		{
			if (!tmp)
				return;
			tmp.text = Text;
			tmp.color = Color;
			tmp.fontSize = FontSize;
		}
	}
}