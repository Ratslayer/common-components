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
            using var _ = target.FlushOnDispose();
            foreach (var value in data.Values)
                if (HasLoadableAsset(value.KeyName, out BaseBoardKey key))
                    target.Set(new()
                    {
                        Key = key,
                        Value = value.Value,
                        Source = this
                    });
        }

        protected override BoardSaveData Serialize(IBoard target)
        {
            var rows = new List<BoardRow>();
            foreach (var container in target.Containers)
            {
                if (container.Key is not ILoadableAsset key)
                    continue;

                if (!IsValidLoadableAsset(key))
                    continue;

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