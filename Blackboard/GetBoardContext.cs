using System;
using System.Collections.Generic;
namespace BB
{
	public readonly struct GetBoardContext
	{
		public readonly IBoard Board;
		public readonly IBoard TargetBoard;

		public GetBoardContext(
			IBoard board,
			IBoard targetBoard = null)
		{
			Board = board;
			TargetBoard = targetBoard;
		}
		public GetBoardContext Inverse()
			=> new(TargetBoard, Board);
	}
}