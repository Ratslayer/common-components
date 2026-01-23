using System;
namespace BB
{
    public readonly struct BoardCostContext
    {
        public IBoard Board { get; init; }
    }
    public readonly struct AddBoardContext
    {
        public IBoardKey Key { get; init; }
        public IBoard Board { get; init; }
        public double? Value { get; init; }
        public Entity Entity
        {
            get => Board.Entity;
            init => Board = value.Get<IBoard>();
        }
        public static AddBoardContext FromEntity(in Entity entity)
            => new()
            {
                Board = entity.Get<IBoard>(),
                Value = 1,
            };
        public AddBoardContext WithKey(IBoardKey key)
            => new()
            {
                Key = key,
                Board = Board,
                Value = Value
            };
        public AddBoardContext WithValue(double value)
            => new()
            {
                Key = Key,
                Board = Board,
                Value = value
            };
        public AddBoardContext TimesValue(double value)
            => WithValue(GetValue() * value);
        public GetBoardContext ToGetContext()
            => new()
            {
                Key = Key,
                Board = Board,
            };
        public double GetValue() => Value ?? 1;
        public void Add() => Board.Add(this);
        public IDisposable AddTemp()
        {
            Add();
            return AddBoardContextOnDispose.GetPooled(TimesValue(-1));
        }
    }
}