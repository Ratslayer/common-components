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
        protected BoardValueGetterAsset _asset;
        [SerializeField, ShowIf(nameof(ShowKey)), HorizontalGroup, HideLabel]
        protected BaseBoardKey _key;

        public TValue GetValue(Entity entity)
        {
            return _type switch
            {
                SerializedValueType.Random => GetRandom(_minValue, _maxValue),
                SerializedValueType.Asset => Convert(GetEntityValue(entity)),
                SerializedValueType.Board => Convert(
                    new GetBoardContext
                    {
                        Entity = entity,
                        Key = _key
                    }
                    .Get()),
                _ => _minValue,
            };
        }
        protected abstract TValue GetRandom(TValue min, TValue max);
        protected abstract TValue Convert(double value);
        protected double GetEntityValue(Entity entity)
        {
            return _asset.GetValue(new() { Entity = entity });
        }

        #region UI
        bool ShowMin => _type
            is SerializedValueType.Const
            or SerializedValueType.Random;
        bool ShowMax => _type is SerializedValueType.Random;
        bool ShowAsset => _type is SerializedValueType.Asset;
        bool ShowKey => _type is SerializedValueType.Board;
        #endregion
    }
}