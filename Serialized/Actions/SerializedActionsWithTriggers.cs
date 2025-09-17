using System;
using UnityEngine;

namespace BB.Serialized.Actions
{
    [Serializable]
    public sealed class SerializedActionsWithTriggers
    {
        [SerializeField]
        SerializedTriggers _triggers = new();
        [SerializeField]
        SerializedActions _actions = new();

        public IDisposable Subscribe(Entity entity)
        {
            var subscription = _triggers.CreateSubscription(new()
            {
                Entity = entity,
                Action = () => _actions.Invoke(new() { Entity = entity })
            });

            subscription.Subscribe();

            return subscription;
        }
    }
}
