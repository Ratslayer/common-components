using UnityEngine.UI;

namespace BB
{
	public abstract record BaseButtonSystem(Button Button)
	{
		protected abstract void OnButtonClick();

		[OnSpawn]
		void OnSpawn() => Button.onClick.AddListener(OnButtonClick);

		[OnDespawn]
		void OnDespawn() => Button.onClick.RemoveListener(OnButtonClick);

		protected void ShowButton()
		{
			Button.gameObject.SetActive(true);
		}
		protected void HideButton()
		{
			Button.gameObject.SetActive(false);
		}
	}
}
