using Sirenix.OdinInspector;
using System;

namespace BB
{
    public enum BoardValueGetterType
    {
        Const = 0,
        Key = 1
    }

    public readonly struct BoardValueGetter
    {
        public BoardValueGetterType Type { get; init; }
        public double ConstValue { get; init; }
        public IBoardKey Key { get; init; }

        public double Get(IBoard board)
            => Type is BoardValueGetterType.Const
                ? ConstValue
                : Key is not null
                    ? Board.Get(board, Key)
                    : 0;
    }

    [Serializable]
    public sealed class SerializedBoardValueGetter : SerializedBoardValueGetter<BaseBoardKey>
    {
    }

    public abstract class SerializedBoardValueGetter<KeyType>
        where KeyType : BaseScriptableObject, IBoardKey
    {
        [HorizontalGroup, HideLabel] public BoardValueGetterType _type;

        [HorizontalGroup, HideLabel, ShowIf(nameof(ShowValue))]
        public double _value;

        [HorizontalGroup, HideLabel, ShowIf(nameof(ShowKey))]
        public KeyType _key;

        public double GetValue(in GetBoardContext context)
            => _type switch
            {
                BoardValueGetterType.Const => _value,
                BoardValueGetterType.Key => context.WithKey(_key).Get(),
                _ => 0
            };

        bool ShowValue => _type == BoardValueGetterType.Const;
        bool ShowKey => _type == BoardValueGetterType.Key;

        public KeyType Key => ShowKey ? _key : null;

        public BoardValueGetter ToGetter() => new()
        {
            ConstValue = _value,
            Key = _key,
            Type = _type
        };
    }
}