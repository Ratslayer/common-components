using BB.Di;
using BB.Serialized;
using System;

namespace BB
{
    public sealed class ActionBehaviour : EntityBehaviour3D
    {
        public SerializedActionsWithTriggers[] _actions = { };
        IDisposable _disposable;
        [OnEvent(typeof(EntitySpawnedEvent))]
        void OnSpawn()
        {
            var bag = DisposableBag.GetPooled();
            foreach (var action in _actions)
                bag.AddDisposable(action.Subscribe(Entity));
            _disposable = bag;
        }
        [OnEvent(typeof(EntityDespawnedEvent))]
        void OnDespawn() => _disposable?.Dispose();
    }
}
