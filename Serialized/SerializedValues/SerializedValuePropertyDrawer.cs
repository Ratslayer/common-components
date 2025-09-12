using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace BB
{
	public abstract class SerializedValuePropertyDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var container = new VisualElement();

			var min = new PropertyField(property.FindPropertyRelative("_minValue"));
			var max = new PropertyField(property.FindPropertyRelative("_maxValue"));
			var asset = new PropertyField(property.FindPropertyRelative("_asset"));
			var typeProp = property.FindPropertyRelative("_type");
			var type = new PropertyField(typeProp);

			container.Add(min);
			container.Add(max);
			container.Add(asset);
			container.Add(type);

			type.RegisterValueChangeCallback(OnTypeChange);
			OnTypeChange(default);

			return container;

			void OnTypeChange(SerializedPropertyChangeEvent _)
			{
				var newType = (SerializedValueType)typeProp.enumValueIndex;
				SetVisibility(max, SerializedValueType.Random);
				SetVisibility(asset, SerializedValueType.Asset);
				min.visible = newType is SerializedValueType.Const or SerializedValueType.Random;

				void SetVisibility(VisualElement element, SerializedValueType visibleType)
					=> element.visible = visibleType == newType;
			}
		}
	}
	
}