using Sirenix.OdinInspector;
using UnityEngine;

namespace BB
{
    public abstract class AbstractSerializedBoardValue<KeyType> : ISerializedBoardValue
        where KeyType : BaseScriptableObject, IBoardKey
    {
        [SerializeField, Required, HorizontalGroup, HideLabel]
        protected KeyType _key;

        [SerializeField, HorizontalGroup, ShowIf(nameof(_key)), HideLabel]
        double _value;

        public KeyType Key => _key;
        public double Value => _value;

        public virtual void Add(IBoard board, in AddBoardContext context)
            => board.Add(context
                .WithKey(_key)
                .TimesValue(_value));

        public static implicit operator bool(AbstractSerializedBoardValue<KeyType> value)
            => value.Key is not null && value.Value.NotZero();
    }
}