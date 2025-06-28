using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BB
{
	public sealed class MultiClickButton
		: MonoBehaviour,
		IPointerDownHandler,
		IPointerUpHandler,
		IPointerClickHandler
	{
		public event Action<PointerEventData> OnPointerDown, OnPointerUp, OnPointerClick;

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			OnPointerClick?.Invoke(eventData);
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			OnPointerDown?.Invoke(eventData);
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			OnPointerUp?.Invoke(eventData);
		}
	}
}
