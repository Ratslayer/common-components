using Sirenix.OdinInspector;
using System;
using UnityEngine;
namespace BB
{
	[Serializable, InlineProperty]
	public sealed class BoardValueGetter
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
		public BaseBoardKey _key;
		public double GetValue(IBoard board)
			=> _type switch
			{
				Type.Const => _value,
				Type.Key => _key.Get(board),
				_ => 0
			};

		bool ShowValue => _type == Type.Const;
		bool ShowKey => _type == Type.Key;

		public BaseBoardKey Key => ShowKey ? _key : null;
	}
	public abstract class BaseBoardKey
		: AbstractKeyObject, IBoardKey, IBoardKeyWithBounds
	{
		public bool _addParentValues = true;
		public BoardValueGetter _min = new(), _max = new();
		public virtual BoardValueStackingMethod StackingMethod => BoardValueStackingMethod.Additive;

		public abstract BoardEventUsage ClampingUsage { get; }

		public double GetMaxValue(IBoard board)
			=> _max.GetValue(board);

		public double GetMinValue(IBoard board)
			=> _min.GetValue(board);

		public override double GetValue(Entity entity)
			=> this.Get(entity);

		public bool HasMaxKey(out IBoardKey key)
		{
			key = _max.Key;
			return _max._type == BoardValueGetter.Type.Key;
		}

		public bool HasMinKey(out IBoardKey key)
		{
			key = _min.Key;
			return _min._type == BoardValueGetter.Type.Key;
		}
	}
	public abstract class AbstractKeyObject
		: EntityValueGetterAsset, INameDesc
	{
		[SerializeField]
		string _key;
		[SerializeField]
		string _name, _description;

		[SerializeField]
		BoardKeyColorScheme _colorScheme;
		public string Key => _key;
		public IBoardKeyColorScheme ColorScheme => _colorScheme ? _colorScheme : null;
		public string Name => _name.DefaultTo(name);

		public string Desc => _description;

		public override string ToString() => $"{name}:{_key}";
	}
}