using System.Collections.Generic;

namespace BB
{
    public sealed class PlayerProgressionStats : IPlayerProgressionStats, ISerializableComponent
    {
        [Inject] Player _player;

        readonly Dictionary<ILoadableAsset, double> _values = new();

        public void Set(ILoadableAsset key, double value)
        {
            if (key is null)
                return;
            _values[key] = value;
        }

        public IEnumerable<KeyValuePair<ILoadableAsset, double>> Values => _values;

        public void ApplyToPlayer()
        {
            var board = _player.Require<IBoard>();
            using var __ = board.FlushOnDispose();
            foreach (var kvp in _values)
                if (kvp.Key is IBoardKey key)
                    Board.Add(board, key, this, kvp.Value);
        }

        public IEntityComponentSerializer[] GetSerializers()
            => new[] { PlayerProgressionStatsSerializerV1.Default };
    }
}