using System;
using UnityEngine;
using BB.Serialized.Events;
using BB.Serialized.Actions;

namespace BB.Serialized
{
    [Serializable]
    public sealed class SerializedActionsWithTriggers : SerializedActions<ISerializedAction>
    {
        [SerializeReference]
        ISerializedEvent[] _events = { };
        [SerializeField] bool _oneShot = false;
        public IDisposable Subscribe(Entity entity)
        {
            var subscription = CreateSubscription(entity);
            subscription.Subscribe();
            return subscription;
        }
        public SerializedTriggerSubscription CreateSubscription(Entity entity)
        {
            var result = SerializedTriggerSubscription.GetPooled(entity);
            var context = new SerializedEventSubscriptionContext
            {
                Entity = entity,
                Action = Invoke
            };
            foreach (var e in _events)
            {
                var subscription = e.CreateSubscription(context);
                if (subscription is not null)
                    result.WithSubscription(subscription);
            }
            return result;

            void Invoke()
            {
                this.Invoke(new() { Entity = entity });
                if (_oneShot)
                    result.Dispose();
            }
        }
    }
}