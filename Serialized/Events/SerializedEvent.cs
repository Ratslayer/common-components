using BB.Di;
using System;

namespace BB.Serialized.Events
{
    public abstract class SerializedEvent<TEvent> : ISerializedEvent
    {
        public IEntitySubscription CreateSubscription(in SerializedEventSubscriptionContext context)
        {
            if (!context.Entity.Has(out IEvent<TEvent> e))
                return null;
            return Subscription.GetPooled(e, this, context.Action);
        }
        protected virtual bool IsValid(TEvent e)
        {
            return true;
        }

        sealed class Subscription
            : ProtectedPooledObject<Subscription>,
            IEntitySubscription,
            IEventHandler<TEvent>
        {
            IEvent<TEvent> _event;
            SerializedEvent<TEvent> _serializedEvent;
            Action _action;
            public static Subscription GetPooled(
                IEvent<TEvent> e,
                SerializedEvent<TEvent> serializedEvent,
                Action action)
            {
                var result = GetPooledInternal();
                result._event = e;
                result._serializedEvent = serializedEvent;
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
                if (_serializedEvent.IsValid(e))
                    _action.Invoke();
            }
		}
    }
}
