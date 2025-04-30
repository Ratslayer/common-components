using Sirenix.OdinInspector;
using UnityEngine;

namespace BB
{
	[InlineProperty]
	public abstract class SerializedValue<TValue>
	{
		[SerializeField, HorizontalGroup(55), HideLabel]
		protected SerializedValueType _type;
		[SerializeField, ShowIf(nameof(ShowMin)), HorizontalGroup, HideLabel]
		protected TValue _minValue;
		[SerializeField, ShowIf(nameof(ShowMax)), HorizontalGroup, HideLabel]
		protected TValue _maxValue;
		[SerializeField, ShowIf(nameof(ShowAsset)), HorizontalGroup, HideLabel]
		protected EntityValueGetterAsset _asset;

		public TValue GetValue(Entity entity)
		{
			return _type switch
			{
				SerializedValueType.Random => GetRandom(_minValue, _maxValue),
				SerializedValueType.Asset => Convert(GetValueInternal(entity)),
				_ => _minValue,
			};
		}
		protected abstract TValue GetRandom(TValue min, TValue max);
		protected abstract TValue Convert(double value);
		protected double GetValueInternal(Entity entity)
		{
			return 0;
		}

		#region UI
		bool ShowMin => _type
			is SerializedValueType.Const
			or SerializedValueType.Random;
		bool ShowMax => _type is SerializedValueType.Random;
		bool ShowAsset => _type is SerializedValueType.Asset;
		#endregion
	}
}