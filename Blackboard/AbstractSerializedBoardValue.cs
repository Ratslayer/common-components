using Sirenix.OdinInspector;
using UnityEngine;
namespace BB
{
	public interface ISerializedBoardValue
	{
		void Add(in AddBoardContext context);
	}
	public abstract class AbstractSerializedBoardValue<KeyType> : ISerializedBoardValue, IBoardValue
		where KeyType : BaseBoardKey
	{
		[SerializeField, Required, HorizontalGroup, HideLabel]
		protected KeyType _key;
		[SerializeField, HorizontalGroup, ShowIf(nameof(_key)), HideLabel]
		double _value;
		IBoardKey IBoardValue.Key => _key;
		public KeyType Key => _key;
		public double Value => _value;
		public virtual void Add(in AddBoardContext context)
			=> context
			.WithKey(_key)
			.WithMultiplier(_value)
			.Add();
		public static implicit operator bool(AbstractSerializedBoardValue<KeyType> value)
			=> value.Key is not null && value.Value.NotZero();
	}
}
