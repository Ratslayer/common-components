using BB.Di;
using System;
using UnityEngine.UI;

namespace BB
{
	public static class UnityUiExtensions
	{
		public static void SetOnClick(this Button button, Action action)
		{
			button.ClearOnClick();
			button.onClick.AddListener(() => action());
		}
		public static void ClearOnClick(this Button button)
            => button.onClick.RemoveAllListeners();
        public static void BindAllPointerEvents(this IDiContainer container)
		{
			container.Event<PointerEnterEvent>();
			container.Event<PointerExitEvent>();
			container.Event<PointerClickEvent>();
        }
	}
}
