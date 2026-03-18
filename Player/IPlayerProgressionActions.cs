using System;
using System.Collections.Generic;
using System.Linq;
using BB.Actions;

namespace BB
{
    public interface IPlayerProgressionActions
    {
        public void AddAction(PlayerActionAsset asset);
    }

    public sealed class PlayerProgressionActions
        : IPlayerProgressionActions, IPlayerActionSource, ISerializableComponent
    {
        [Inject] private IPlayerActionsService _actionsService;
        public readonly List<PlayerActionAsset> _actions = new();

        public void AddAction(PlayerActionAsset asset)
        {
            if (asset)
                _actions.AddUnique(asset);
        }

        public IEnumerable<PlayerActionData> GetActions(UpdatePlayerActionsContext context)
            => _actions
                .Where(a => a.IsActive(context))
                .Select(a => a.GetData());

        public IEntityComponentSerializer[] GetSerializers()
            => new IEntityComponentSerializer[] { PlayerProgressionActionsSerializerV1.Default };

        [OnEvent]
        void OnSpawn(EntitySpawnedEvent _) => _actionsService.AddSource(this);
    }

    public sealed class PlayerProgressionActionsSerializerV1 : BaseSerializer<
        PlayerProgressionActionsSerializerV1,
        PlayerProgressionActions,
        PlayerProgressionActionsSerializerV1.SaveData>
    {
        public sealed class SaveData
        {
            public string[] ActionAssetKeys { get; init; }
        }

        protected override SaveData Serialize(PlayerProgressionActions target)
            => new SaveData
            {
                ActionAssetKeys = target._actions
                    .Where(a => IsValidLoadableAsset(a))
                    .Select(a => a.AssetLoadKey)
                    .ToArray()
            };

        protected override void ApplySpawn(PlayerProgressionActions target, SaveData data)
        {
            foreach (var key in data.ActionAssetKeys)
                if (HasLoadableAsset(key, out PlayerActionAsset action))
                    target.AddAction(action);
        }
    }
}

namespace BB.Serialized.Actions
{
    [Serializable]
    public sealed class AddProgressionAction : SerializedActionSync, ISerializedAllAction
    {
        public PlayerActionAsset _action;

        protected override void InvokeSync(SerializedActionContext context)
        {
            var actions = context.Entity.Require<IPlayerProgressionActions>();
            actions.AddAction(_action);
        }
    }
}