namespace BB
{
	public readonly struct GetBoardContext
	{
		public readonly IBoardKey _key;
		public readonly IBoard _board, _targetBoard;

		public GetBoardContext(
			IBoardKey key,
			IBoard board,
			IBoard targetBoard = null)
		{
			_key = key;
			_board = board;
			_targetBoard = targetBoard;
		}

		public static implicit operator bool(GetBoardContext context)
			=> context._key is not null
			&& context._board is not null;
	}
}