using System.Collections.Generic;

namespace BB
{
	public readonly struct GetBoardContext
	{
		public readonly IBoardKey _key;
		public readonly IBoard _board, _targetBoard;
		public readonly IList<BaseBoardKey> _tags;
		public GetBoardContext(
			IBoardKey key,
			IBoard board,
			IBoard targetBoard = null,
			IList<BaseBoardKey> tags = null)
		{
			_key = key;
			_board = board;
			_targetBoard = targetBoard;
			_tags = tags;
		}

		public static implicit operator bool(GetBoardContext context)
			=> context._key is not null
			&& context._board is not null;
	}
}