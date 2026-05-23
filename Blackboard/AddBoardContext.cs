// using System;
//
// namespace BB
// {
//     public readonly struct SetBoardContext
//     {
//         public IBoardKey Key { get; init; }
//         public double Value { get; init; }
//         public object Source { get; init; }
//         public IBoardValueCondition Condition { get; init; }
//     }
//
//     public readonly struct AddBoardContext
//     {
//         public BoardValue Value { get; init; }
//         public IBoardKey Key { get; init; }
//
//         public double Value { get; init; }
//         public object Source { get; init; }
//
//         public IBoardValueCondition Condition { get; init; }
//
//         public AddBoardContext WithKey(IBoardKey key)
//             => new()
//             {
//                 Key = key,
//                 Source = Source,
//                 Value = Value
//             };
//
//         public AddBoardContext WithValue(double value)
//             => new()
//             {
//                 Key = Key,
//                 Source = Source,
//                 // Board = Board,
//                 Value = value
//             };
//
//         public AddBoardContext TimesValue(double value)
//             => WithValue(Value * value);
//     }
// }