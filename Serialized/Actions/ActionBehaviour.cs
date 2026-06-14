using BB.Serialized;
using System;
using Sirenix.OdinInspector;

namespace BB
{
    public sealed class ActionBehaviour : EntityComponent3D, IActionSubscriber, ISerializableComponent
    {
        public SerializedActionsWithTriggers[] _actions = { };
        IDisposable _disposable;

        [OnEvent(typeof(EntitySpawnedEvent), typeof(AfterGameLoadEvent))]
        void InitActions()
        {
            _disposable?.Dispose();
            var bag = DisposableBag.GetPooled();
            foreach (var action in _actions)
                bag.Add(action.Subscribe(Entity, this));
            _disposable = bag;
        }

        [OnEvent(typeof(EntityDespawnedEvent))]
        void OnDespawn() => _disposable?.Dispose();

        [ShowInInspector]
        public bool HasBeenTriggered { get; set; }

        public IEntityComponentSerializer[] GetSerializers()
            => new[] { new ActionBehaviourSerializerV1() };
    }

    public sealed class ActionBehaviourSerializerV1 : BaseSerializer<
        ActionBehaviourSerializerV1,
        ActionBehaviour, 
        ActionBehaviourSerializerV1.Data>
    {
        protected override ActionBehaviourSerializerV1.Data Serialize(ActionBehaviour target)
            => new() { HasBeenTriggered = target.HasBeenTriggered };

        protected override void ApplySpawn(ActionBehaviour target, ActionBehaviourSerializerV1.Data data)
        {
            target.HasBeenTriggered = data.HasBeenTriggered;
        }

        public sealed class Data
        {
            public bool HasBeenTriggered { get; init; }
        }
    }
}