using UnityEngine;
namespace BB
{
	public abstract class BaseBoardKey : AbstractKeyObject, IBoardKey
	{
		public bool _addParentValues = true;

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
		public string Key => _key;

		public string Name => _name.DefaultTo(name);

		public string Desc => _description;

		public override string ToString() => $"{name}:{_key}";
	}
}