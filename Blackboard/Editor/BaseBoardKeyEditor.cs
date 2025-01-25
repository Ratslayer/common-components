using UnityEditor;
using UnityEngine;

namespace BB
{
	[CustomEditor(typeof(BaseBoardKey))]
	public sealed class BaseBoardKeyEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			using (LayoutUtils.Horizontal)
			{
				//if(GUILayout.Button(""))
			}
		}
	}
}