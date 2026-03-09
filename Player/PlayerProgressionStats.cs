using System.Collections.Generic;
using System.Linq;
namespace BB
{
    public interface IPlayerProgressionStats
    {
        void Add(ILoadableAsset key, double value);
        void ApplyToPlayer();
    }
    public sealed class PlayerProgressionStats : IPlayerProgressionStats, ISerializableComponent
    {
        [Inject] Player _player;

        readonly Dictionary<ILoadableAsset, double> _values = new();
        public void Add(ILoadableAsset key, double value)
        {
            if (key is null)
                return;
            if (_values.TryGetValue(key, out var currentValue))
                _values[key] = value + currentValue;
            else _values[key] = value;
        }
        public IEnumerable<KeyValuePair<ILoadableAsset, double>> Values => _values;

        public void ApplyToPlayer()
        {
            var board = _player.Require<IBoard>();
            using var __ = board.FlushOnDispose();
            foreach (var kvp in _values)
                if (kvp.Key is IBoardKey key)
                    Board.Add(board, key, kvp.Value);
        }

        public IEntityComponentSerializer[] GetSerializers()
            => new[] { PlayerProgressionStatsSerializerV1.Default };
    }
    public sealed class PlayerProgressionStatsSerializerV1 : BaseSerializer<
        PlayerProgressionStatsSerializerV1,
        PlayerProgressionStats,
        PlayerProgressionStatsSerializerV1.Data>
    {
        protected override Data Serialize(PlayerProgressionStats target)
            => new Data
            {
                Values = target.Values
                    .Select(x => (GetLoadableAsset<BaseBoardKey>(x.Key.AssetLoadKey) as ILoadableAsset, x.Value))
                    .Where(x => x.Item1 is not null)
                    .Select(x => new StatValue
                    {
                        BoardKeyName = x.Item1.AssetLoadKey,
                        Value = x.Value
                    })
                .ToArray()
            };
        protected override void ApplySpawn(PlayerProgressionStats target, Data data)
        {
            foreach (var kvp in data.Values)
            {
                if (!HasLoadableAsset(kvp.BoardKeyName, out BaseBoardKey boardKey))
                {
                    LogError($"Board key with name {kvp.BoardKeyName} not found.");
                    continue;
                }
                target.Add(boardKey as ILoadableAsset, kvp.Value);
            }
        }

        public sealed class Data
        {
            public StatValue[] Values { get; init; }
        }
        public sealed class StatValue
        {
            public string BoardKeyName { get; init; }
            public double Value { get; init; }
        }
    }
}
