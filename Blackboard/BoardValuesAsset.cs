using Sirenix.OdinInspector;
using UnityEngine;
namespace BB
{
	public sealed class BoardValuesAsset : EntityComponentAsset, IBoardValuesProvider
	{
		[SerializeField, InlineProperty, HideLabel]
		SerializedBoardValues _values = new();

		public BuffDisposable Add(in AddBoardContext context)
			=> _values.Add(context);

		public override void Apply(Entity target, IStateData data)
		{
			if (target.Has(out IBoard board))
				_values.Add(new AddBoardContext(board, null, 1));
		}
		public override void Unapply(Entity target, IStateData data)
		{
			if (target.Has(out IBoard board))
				_values.Add(new AddBoardContext(board, null, -1));
		}
	}
}