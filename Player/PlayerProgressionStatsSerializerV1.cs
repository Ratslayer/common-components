using System.Linq;

namespace BB
{
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
                target.Set(boardKey, kvp.Value);
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