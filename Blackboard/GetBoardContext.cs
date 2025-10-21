using System;
namespace BB
{
	public readonly struct GetBoardContext
    {
        public IBoardKey Key { get; init; }
        public IBoard Board { get; init; }
        public IBoard TargetBoard { get; init; }
        public double? Multiplier { get; init; }
        public static GetBoardContext FromEntity(in Entity entity)
            => new()
            {
                Board = entity.Get<IBoard>()
            };
        public GetBoardContext WithKey(IBoardKey key)
            => new()
            {
                Key = key,
                Board = Board,
                TargetBoard = TargetBoard,
                Multiplier = Multiplier
            };
        public GetBoardContext WithBoard(IBoard board)
            => new()
            {
                Key = Key,
                Board = board,
                TargetBoard = TargetBoard,
                Multiplier = Multiplier
            };
        public GetBoardContext WithSwappedBoards()
            => new()
            {
                Key = Key,
                Board = TargetBoard,
                TargetBoard = Board,
                Multiplier = Multiplier
            };
        public AddBoardContext ToAddContext()
            => new()
            {
                Key = Key,
                Board = Board,
                Value = 1
            };
        public double Get() => Board.Get(this);
        public int GetInt() => (int)Math.Floor(Get() + double.Epsilon);
        public bool GetBool() => Get() > double.Epsilon;
    }
}