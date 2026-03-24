using System.Linq;
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
                board.Set(new SetBoardContext
                {
                    Key = newKey,
                    Value = 0,
                    Source = "Editor"
                });

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
                var value = board.Get(key);
                var newValue = EditorGUILayout.DoubleField(value);
                if (EditorGUI.EndChangeCheck())
                {
                    changedKey = key;
                    diff = newValue - value;
                }

#if DEBUG
                var container = board.Containers.FirstOrDefault(c => c.Key == key) as BoardValueContainer;
                var foldoutContainer = EditorGuiUtils.Foldout(container);
                EditorGUILayout.EndHorizontal();
                if (foldoutContainer)
                {
                    if (container is not null)
                        foreach (var source in container._sources)
                            if (source.Value.NotZero())
                            {
                                using var _ = LayoutUtils.Horizontal;
                                EditorGUILayout.LabelField(source.Key.ToString());
                                EditorGUILayout.LabelField($"{source.Value:n1}");
                            }
                }
#else
                EditorGUILayout.EndHorizontal();
#endif
            }

            if (changedKey is not null)
            {
                Board.Add(board, changedKey,"BoardEdit", diff);
                board.ForceFlushChanges();
            }
        }
    }

    [CustomEditor(typeof(BlackboardComponent))]
    public sealed class BlackboardBehaviourEditor : Editor
    {
        BlackboardComponent Target => target as BlackboardComponent;

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