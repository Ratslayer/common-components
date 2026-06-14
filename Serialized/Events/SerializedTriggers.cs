using System;
using UnityEngine;
using BB.Serialized.Events;
using BB.Serialized.Actions;
using Sirenix.OdinInspector;

namespace BB.Serialized
{
    public interface IActionSubscriber
    {
        bool HasBeenTriggered { get; set; }
    }
    [Serializable]
    public sealed class SerializedActionsWithTriggers : SerializedActions<ISerializedAction>
    {
        [SerializeReference]
        ISerializedEvent[] _events = { };
        [SerializeField, HorizontalGroup] bool _oneShot = true, _enabled = true;
        public IDisposable Subscribe(Entity entity, IActionSubscriber subscriber)
        {
            if (_oneShot && subscriber.HasBeenTriggered)
                return null;
            
            var subscription = CreateSubscription(entity, subscriber);
            subscription.Subscribe();
            return subscription;
        }
        public SerializedTriggerSubscription CreateSubscription(Entity entity, IActionSubscriber subscriber)
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
                if (!_enabled)
                    return;
                this.Invoke(new() { Entity = entity });
                subscriber.HasBeenTriggered = true;
                if (_oneShot)
                    result.Dispose();
            }
        }
    }
}