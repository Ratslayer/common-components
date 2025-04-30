using TMPro;
using UnityEngine;

namespace BB
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public sealed class AutoSizeTmpGui : MonoBehaviour
	{
		private void Awake()
		{
			var tmp = GetComponent<TextMeshProUGUI>();
			tmp.autoSizeTextContainer = true;
		}
	}
}