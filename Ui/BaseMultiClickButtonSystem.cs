using UnityEngine.EventSystems;

namespace BB.Ui
{
	public abstract record BaseMultiClickButtonSystem(MultiClickButton Button)
	{
		protected virtual void OnButtonClick(PointerEventData data)
		{
		}
		protected virtual void OnButtonDown(PointerEventData data)
		{
		}
		protected virtual void OnButtonUp(PointerEventData data)
		{
		}

		[OnSpawn]
		void OnSpawn()
		{
			Button.OnPointerClick += OnButtonClick;
			Button.OnPointerDown += OnButtonDown;
			Button.OnPointerUp += OnButtonUp;
		}

		[OnDespawn]
		void OnDespawn()
		{
			Button.OnPointerClick -= OnButtonClick;
			Button.OnPointerDown -= OnButtonDown;
			Button.OnPointerUp -= OnButtonUp;
		}

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
