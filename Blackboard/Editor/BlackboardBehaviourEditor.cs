using UnityEditor;
using UnityEngine;
using BB.Di;
namespace BB
{
	[CustomEditor(typeof(BlackboardBehaviour))]
	public sealed class BlackboardBehaviourEditor : Editor
	{
		BlackboardBehaviour Target => target as BlackboardBehaviour;
		string _searchString;
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (!Application.isPlaying
				|| !Target.Has(out IBoard board))
				return;

			var newKey = EditorGUILayout.ObjectField("Add new key", null, typeof(BaseBoardKey), false) as BaseBoardKey;
			if (newKey)
				board.GetOrCreate(newKey);

			if (GUILayout.Button("Update Board"))
				board.FlushChanges();

			_searchString = EditorGUILayout.TextField("Search", _searchString)?.ToLower().Trim();
			IBoardKey changedKey = default;
			double diff = default;
			foreach (var v in board.Values)
			{
				var name = v.Key.Name;
				//if there is a search string and name does not match it, skip field
				if (!string.IsNullOrWhiteSpace(_searchString)
					&& !name.ToLower().Contains(_searchString))
					continue;
				EditorGUILayout.BeginHorizontal();
				if (v.Key is Object keyObj)
				{
					GUI.enabled = false;
					EditorGUILayout.ObjectField(keyObj, typeof(UnityEngine.Object), false);
					GUI.enabled = true;
				}
				else EditorGUILayout.LabelField(name);
				//draw editable field
				if (v is IValue<double> value)
				{
					EditorGUI.BeginChangeCheck();
					var newValue = EditorGUILayout.DoubleField(value.Value);
					if (EditorGUI.EndChangeCheck())
					{
						changedKey = v.Key;
						diff = newValue - value.Value;
					}
				}

				EditorGUILayout.EndHorizontal();
			}

			if (changedKey is not null)
			{
				changedKey.Add(diff, new(board));
				board.FlushChanges();
			}
		}
	}
}