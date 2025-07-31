using Sirenix.OdinInspector;
using UnityEngine;
namespace BB
{
	public interface ISerializedBoardValue
	{
		void Add(BoardContext context);
	}
	public abstract class AbstractSerializedBoardValue<KeyType> : ISerializedBoardValue
		where KeyType : BaseScriptableObject, IBoardKey
	{
		[SerializeField, Required, HorizontalGroup, HideLabel]
		protected KeyType _key;
		[SerializeField, HorizontalGroup, ShowIf(nameof(_key)), HideLabel]
		double _value;
		public KeyType Key => _key;
		public double Value => _value;
		public virtual void Add(BoardContext context)
			=> context.GetPooledCopy()
			.WithKey(_key)
			.WithValue(_value * context.Value)
			.AddAndDispose();

		public static implicit operator bool(AbstractSerializedBoardValue<KeyType> value)
			=> value.Key is not null && value.Value.NotZero();
	}
}
