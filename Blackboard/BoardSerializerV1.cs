using System;
using System.Collections.Generic;
namespace BB
{
	public sealed class BoardSerializerV1 : BaseSerializer<BoardSerializerV1, IBoard, BoardSerializerV1.BoardSaveData>
    {
        [Serializable]
        public sealed class BoardSaveData
        {
            public List<BoardRow> Values;
        }
        [Serializable]
        public struct BoardRow
        {
            public string KeyName;
            public double Value;
        }

        protected override void ApplyAfterSpawn(IBoard target, BoardSaveData data)
        {

        }

        protected override BoardSaveData Serialize(IBoard target)
        {
            var rows = new List<BoardRow>();
            foreach (var container in target.Containers)
            {
                if (container.Key is not ILoadableAsset key)
                    continue;

                if (string.IsNullOrWhiteSpace(key.AssetLoadKey))
                {
                    Log.Error($"{container.Key.Name} does not have an {nameof(ILoadableAsset.AssetLoadKey)}");
                    continue;
                }

                rows.Add(new()
                {
                    KeyName = key.AssetLoadKey,
                    Value = container.Value
                });
            }

            return new()
            {
                Values = rows
            };
        }
    }
}