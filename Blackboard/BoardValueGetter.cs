using Sirenix.OdinInspector;
namespace BB
{
	public abstract class BoardValueGetter<KeyType>
		where KeyType : BaseScriptableObject, IBoardKey
	{
		public enum Type
		{
			Const = 0,
			Key = 1
		}
		[HorizontalGroup, HideLabel]
		public Type _type;
		[HorizontalGroup, HideLabel, ShowIf(nameof(ShowValue))]
		public double _value;
		[HorizontalGroup, HideLabel, ShowIf(nameof(ShowKey))]
		public KeyType _key;
		public double GetValue(IBoard board)
			=> _type switch
			{
				Type.Const => _value,
				Type.Key => _key.Get(board),
				_ => 0
			};

		bool ShowValue => _type == Type.Const;
		bool ShowKey => _type == Type.Key;

		public KeyType Key => ShowKey ? _key : null;
	}
}