using UnityEngine;

namespace BB
{
	public sealed class BoardKeyColorScheme : BaseScriptableObject, IBoardKeyColorScheme
	{
		public Color _textColor = Color.white;

		public Color TextColor => _textColor;
	}
	public interface IBoardKeyColorScheme
	{
		Color TextColor { get; }
	}
}
