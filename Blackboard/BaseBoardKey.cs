using UnityEngine;
namespace BB
{
	public abstract class BaseBoardKey
		: AbstractKeyObject, IBoardKey, IBoardKeyDetails
	{
		public bool _addParentValues = true;
		public Color _color = Color.white;
		public Color Color => _color;

		public virtual double AddValues(double v1, double v2)
			=> v1 + v2;

		public virtual IBoardValueContainer CreateValue(IBoard board)
			=> new DefaultBoardValue(this, board);
	}
	public abstract class AbstractKeyObject
		: BaseScriptableObject, INameDesc
	{
		[SerializeField]
		string _key;
		[SerializeField]
		StringAsset _name, _description;

		[SerializeField]
		BoardKeyColorScheme _colorScheme;
		public string Key => _key;
		public BoardKeyColorScheme ColorScheme => _colorScheme;
		public string Name => _name.DefaultTo(name);

		public string Desc => _description;

		public override string ToString() => $"{name}:{_key}";
	}
}