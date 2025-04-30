
using UnityEngine;

namespace BB
{
	public class BoardBuff : BaseBoardKey
	{
		[SerializeField]
		protected SerializedBoardValues _values = new();
		public override IBoardValueContainer CreateValue(IBoard board)
			=> new BoardValue(this, board);
		sealed record BoardValue(BoardBuff Buff, IBoard Board)
			: BaseBoardValue(Buff, Board)
		{
			public override void Add(in AddBoardContext context)
			{
				base.Add(context);
				Buff._values.Add(context);
			}
		}
	}
}
