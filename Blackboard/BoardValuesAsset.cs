using Sirenix.OdinInspector;
using UnityEngine;
namespace BB
{
	public sealed class BoardValuesAsset : EntityComponentAsset, IBoardValuesProvider
	{
		[SerializeField, InlineProperty, HideLabel]
		SerializedBoardValues _values = new();

		public void Add(in AddBoardContext context)
			=> _values.Add(context);

		public override void Apply(Entity target)
		{
			if (!target.Has(out IBoard board))
			{
				Log.Error($"Can't apply {name} to entity {target} with no {nameof(IBoard)}");
				return;
			}
			_values.Add(new(board, null, 1));
		}
	}
}