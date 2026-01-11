using System;
using UnityEngine.UI;

namespace BB
{
	public static class UnityUiExtensions
	{
		public static void SetOnClick(this Button button, Action action)
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => action());
		}
	}
}
