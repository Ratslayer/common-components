using Sirenix.OdinInspector;
using UnityEngine;
namespace BB
{
	public sealed class BoardValuesAsset : EntityComponentAsset, IBoardValuesProvider
	{
		[SerializeField, InlineProperty, HideLabel]
		SerializedBoardValues _values = new();

		public RemoveBoardValuesOnDispose Add(BoardContext context)
			=> _values.Add(context);

		public override void Apply(Entity target, IStateData data)
			=> Apply(target, data, 1);
		public override void Unapply(Entity target, IStateData data)
			=> Apply(target, data, -1);
		void Apply(Entity target, IStateData data, double multiplier)
		{
			if (!target.Has(out IBoard board))
				return;
			var context = BoardContext
				.GetPooled(board)
				.WithValue(multiplier);
			_values.Add(context);
			context.Dispose();
		}
	}
}