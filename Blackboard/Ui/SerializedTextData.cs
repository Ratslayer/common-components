using System;
using UnityEngine;

namespace BB
{
	[Serializable]
	public sealed class SerializedTextData
	{
		public string _text;
		public Color _color = Color.black;
		public float _fontSize = 1f;
		public static implicit operator TextData(SerializedTextData data)
			=> new()
			{
				Color = data._color,
				FontSize = data._fontSize,
				Text = data._text
			};
	}
}