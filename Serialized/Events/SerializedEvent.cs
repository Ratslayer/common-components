using BB.Di;
using System;
using System.Collections.Generic;

namespace BB.Serialized.Events
{
    public sealed class EntitySubscriptionBag : PooledObject<EntitySubscriptionBag>, IEntitySubscription
    {
        readonly List<IEntitySubscription> _subscriptions = new();
        public void Add(IEntitySubscription subscription) => _subscriptions.Add(subscription);
        public void Subscribe(IEntity entity)
        {
            foreach (var sub in _subscriptions)
                sub.Subscribe(entity);
        }
        public void Unsubscribe(IEntity entity)
        {
            foreach (var sub in _subscriptions)
                sub.Unsubscribe(entity);
        }
        public override void Dispose()
        {
            base.Dispose();
            _subscriptions.DisposeElementsAndClear();
        }
    }
    public abstract class SerializedEvent : ISerializedEvent
    {
        public abstract IEntitySubscription CreateSubscription(in SerializedEventSubscriptionContext context);

        protected sealed class Subscription<TEvent>
            : ProtectedPooledObject<Subscription<TEvent>>,
            IEntitySubscription,
            IEventHandler<TEvent>
        {
            IEvent<TEvent> _event;
            Action _action;
            Func<TEvent, bool> _condition;
            public static Subscription<TEvent> GetPooled(
                IEvent<TEvent> e,
                Func<TEvent, bool> condition,
                Action action)
            {
                var result = GetPooledInternal();
                result._event = e;
                result._condition = condition;
                result._action = action;
                return result;
            }
            public void Subscribe(IEntity entity)
            {
                _event.Subscribe(this);
            }
            public void Unsubscribe(IEntity entity)
            {
                _event.Unsubscribe(this);
            }
            public void OnEvent(TEvent e)
            {
                if (_condition(e))
                    _action.Invoke();
            }
        }
    }
    public abstract class SerializedEvent<TEvent> : SerializedEvent
    {
        public override IEntitySubscription CreateSubscription(in SerializedEventSubscriptionContext context)
        {
            var subscription = EntitySubscriptionBag.GetPooled();
            if (context.Entity.Has(out IEvent<TEvent> e))
                subscription.Add(Subscription<TEvent>.GetPooled(e, IsValid, context.Action));

            return subscription;
        }
        protected virtual bool IsValid(TEvent e)
        {
            return true;
        }

    }
    public abstract class SerializedEvent<TEvent1, TEvent2> : SerializedEvent
    {
        public override IEntitySubscription CreateSubscription(in SerializedEventSubscriptionContext context)
        {
            var subscription = EntitySubscriptionBag.GetPooled();
            if (context.Entity.Has(out IEvent<TEvent1> e))
                subscription.Add(Subscription<TEvent1>.GetPooled(e, IsValid, context.Action));
            if (context.Entity.Has(out IEvent<TEvent2> e2))
                subscription.Add(Subscription<TEvent2>.GetPooled(e2, IsValid2, context.Action));

            return subscription;
        }
        protected virtual bool IsValid(TEvent1 e)
        {
            return true;
        }
        protected virtual bool IsValid2(TEvent2 e)
        {
            return true;
        }
    }
}
