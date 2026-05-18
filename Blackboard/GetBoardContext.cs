using System;
namespace BB
{
    public readonly struct GetBoardContext
    {
        public IBoardKey Key { get; init; }
        public IBoard TargetBoard { get; init; }
        public double? Multiplier { get; init; }
        public double GetMultiplier() => Multiplier ?? 1;
        public GetBoardContext WithKey(IBoardKey key)
            => new()
            {
                Key = key,
                TargetBoard = TargetBoard,
                Multiplier = Multiplier
            };
        public GetBoardContext WithTargetBoard(IBoard board)
            => new()
            {
                Key = Key,
                TargetBoard = board,
                Multiplier = Multiplier
            };
        // public GetBoardContext WithMultiplier(double multiplier)
        //     => new()
        //     {
        //         Key = Key,
        //         Board = Board,
        //         TargetBoard = TargetBoard,
        //         Multiplier = multiplier
        //     };
        // public GetBoardContext WithSwappedBoards()
        //     => new()
        //     {
        //         Key = Key,
        //         Board = TargetBoard,
        //         TargetBoard = Board,
        //         Multiplier = Multiplier
        //     };
        // public AddBoardContext ToAddContext()
        //     => new()
        //     {
        //         Key = Key,
        //         Board = Board,
        //         Value = 1
        //     };
        // public double Get()
        // {
        //     if(Board is not null)
        //         return Board.Get(this);
        //     throw new ArgumentException("Board is null");
        // }
        // public int GetInt() => (int)Math.Floor(Get() + double.Epsilon);
        // public bool GetBool() => Get() > double.Epsilon;
    }
}