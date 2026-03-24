using System;
using System.Collections.Generic;

namespace BB
{
    [Flags]
    public enum BoardEventUsage
    {
        None = 0,
        Get = 1,
        Set = 1 << 1
    }

    public enum BoardValueStackingMethod
    {
        Additive,
        Multiplicative
    }

    public interface IBoardKey
    {
        string Name { get; }
        BoardValueStackingMethod StackingMethod { get; }
    }

    public interface IBoardKeyWithBounds : IBoardKey
    {
        BoardValueGetter MinValue { get; }
        BoardValueGetter MaxValue { get; }
        BoardEventUsage ClampingUsage { get; }
    }

    public interface IBoardKeyWithOnAddEffect : IBoardKey
    {
        void OnAdd(IBoard board, double value);
    }

    public interface IBoardKeyWithMultipliers : IBoardKey
    {
        IReadOnlyCollection<IBoardKey> Multipliers { get; }
        BoardEventUsage MultiplierUsage { get; }
    }

    public interface IBoardKeyWithAdders : IBoardKey
    {
        IReadOnlyCollection<IBoardKey> Adders { get; }
        BoardEventUsage AdderUsage { get; }
    }

    public interface IBoardKeyWithGeneration : IBoardKey
    {
        BoardValueGetter RegenValue { get; }
    }

    public interface IBoardKeyValue
    {
        double Value { get; }
    }

    public interface IBoardKeyValueWithCondition
    {
        bool IsActive(IBoard board);
    }

    public interface IBoardKeyValueWithTargetedCondition
    {
        bool IsActive(IBoard board, IBoard targetBoard);
    }
}