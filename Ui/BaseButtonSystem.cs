using UnityEngine.UI;

namespace BB
{
	public abstract record BaseButtonSystem(Button Button)
	{
		protected abstract void OnButtonClick();

		[OnEvent(typeof(EntitySpawnedEvent))]
		void OnSpawn() => Button.onClick.AddListener(OnButtonClick);

		[OnEvent(typeof(EntityDespawnedEvent))]
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
