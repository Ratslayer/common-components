using UnityEditor;
using UnityEngine;
namespace BB
{
    public static class EditorBoardUtils
    {
        static string _searchString;
        public static void DrawBoard(IBoard board)
        {
            var newKey = EditorGUILayout.ObjectField("Add new key", null, typeof(BaseBoardKey), false) as BaseBoardKey;
            if (newKey)
                board.Set(newKey, 0);

            if (GUILayout.Button("Update Board"))
                board.ForceFlushChanges();

            _searchString = EditorGUILayout.TextField("Search", _searchString)?.ToLower().Trim();
            IBoardKey changedKey = default;
            double diff = default;
            foreach (var key in board.Keys)
            {
                var name = key.Name;
                //if there is a search string and name does not match it, skip field
                if (!string.IsNullOrWhiteSpace(_searchString)
                    && !name.ToLower().Contains(_searchString))
                    continue;
                EditorGUILayout.BeginHorizontal();
                if (key is Object keyObj)
                {
                    GUI.enabled = false;
                    EditorGUILayout.ObjectField(keyObj, typeof(UnityEngine.Object), false);
                    GUI.enabled = true;
                }
                else EditorGUILayout.LabelField(name);
                //draw editable field
                EditorGUI.BeginChangeCheck();
                var value = key.Get(board);
                var newValue = EditorGUILayout.DoubleField(value);
                if (EditorGUI.EndChangeCheck())
                {
                    changedKey = key;
                    diff = newValue - value;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (changedKey is not null)
            {
                changedKey.Add(board, diff);
                board.ForceFlushChanges();
            }
        }
    }
    [CustomEditor(typeof(BlackboardBehaviour))]
    public sealed class BlackboardBehaviourEditor : Editor
    {
        BlackboardBehaviour Target => target as BlackboardBehaviour;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Application.isPlaying
                || !Target.Entity.Has(out IBoard board))
                return;

            EditorBoardUtils.DrawBoard(board);
        }
    }
}