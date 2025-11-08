using BB.Di;
using BB.Serialized;
using System;

namespace BB
{
    public sealed class ActionBehaviour : EntityBehaviour
    {
        public SerializedActionsWithTriggers[] _actions = { };
        IDisposable _disposable;
        [OnSpawn]
        void OnSpawn()
        {
            var bag = DisposableBag.GetPooled();
            foreach (var action in _actions)
                bag.AddDisposable(action.Subscribe(Entity));
            _disposable = bag;
        }
        [OnDespawn]
        void OnDespawn() => _disposable?.Dispose();
    }
}
