using BB.Di;

namespace BB
{
	public readonly struct AddBoardContext
	{
		public readonly double _numStacks;
		public readonly IBoard _board;

		public AddBoardContext(
			IBoard board,
			double numStacks = 1)
		{
			_board = board;
			_numStacks = numStacks;
		}
		public static implicit operator AddBoardContext(in Entity entity)
			=> new(entity.Get<IBoard>(), 1);
		public AddBoardContext MultiplyStacks(double numStacks)
			=> new(_board, _numStacks * numStacks);
	}
}