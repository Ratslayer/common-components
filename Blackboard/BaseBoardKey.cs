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
		[SerializeField, HorizontalGroup, HideLabel]
		Type _type;
		[SerializeField, HorizontalGroup, HideLabel, ShowIf(nameof(ShowValue))]
		double _value;
		[SerializeField, HorizontalGroup, HideLabel, ShowIf(nameof(ShowKey))]
		BaseBoardKey _key;
		public double GetValue(IBoard board)
			=> _type switch
			{
				Type.Const => _value,
				Type.Key => _key.Get(board),
				_ => 0
			};

		bool ShowValue => _type == Type.Const;
		bool ShowKey => _type == Type.Key;

	}
	public abstract class BaseBoardKey
		: AbstractKeyObject, IBoardKey, IBoardKeyDetails
	{
		public bool _addParentValues = true;
		public BoardValueGetter _min = new(), _max = new();

		public virtual double AddValues(double v1, double v2)
			=> v1 + v2;

		public virtual IBoardValueContainer CreateValue(IBoard board)
			=> new DefaultBoardValue(this, board);

		public double GetMaxValue(IBoard board)
			=> _max.GetValue(board);

		public double GetMinValue(IBoard board)
			=>_min.GetValue(board);

		public override double GetValue(Entity entity)
			=> this.Get(entity);
	}
	public abstract class AbstractKeyObject
		: EntityValueGetterAsset, INameDesc
	{
		[SerializeField]
		string _key;
		[SerializeField]
		StringAsset _name, _description;

		[SerializeField]
		BoardKeyColorScheme _colorScheme;
		public string Key => _key;
		public IBoardKeyColorScheme ColorScheme => _colorScheme ? _colorScheme : null;
		public string Name => _name.DefaultTo(name);

		public string Desc => _description;

		public override string ToString() => $"{name}:{_key}";
	}
}