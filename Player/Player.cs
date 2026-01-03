using BB.Di;
using UnityEngine;
namespace BB
{
    public sealed class Player : EntityVariable<Player>
    {
        public Vector3 Position => Has(out Root root) ? root.Position : Vector3.zero;
    }
    public abstract class SubscribeToEntityVariableEventSystem<TVariable, TEvent>
        : EntitySystem, IEventHandler<TEvent>
        where TVariable : EntityVariable<TVariable>
    {
        [Inject] TVariable _var;
        [Inject] IEvent<TVariable> _event;
        public TVariable Var => _var;
        readonly EventHandler _handler = new();
        [OnEvent(typeof(EntitySpawnedEvent))]
        void OnSpawn()
        {
            _handler._subscription = this;
            OnEvent(Var.Get<TEvent>());
            _event.Subscribe(_handler);
        }
        [OnEvent(typeof(EntityDespawnedEvent))]
        void OnDespawn()
        {
            if (_var.Value.Has(out IEvent<TEvent> e))
                e.Unsubscribe(this);
            _event.Unsubscribe(_handler);
        }

        public void OnEvent(TVariable msg)
        {
            if (_var.PreviousValue.Has(out IEvent<TEvent> e))
                e.Unsubscribe(this);
            if (_var.Value.Has(out e))
                e.Subscribe(this);
            //для тех событий которые являеются своими же источниками
            if (_var.Value.Has(out TEvent eventSource))
                OnEvent(eventSource);
        }

        public abstract void OnEvent(TEvent msg);

		sealed class EventHandler
			: IEventHandler<TVariable>
		{
            public SubscribeToEntityVariableEventSystem<TVariable, TEvent> _subscription;

            public void OnEvent(TVariable msg)
			{
                if (_subscription._var.PreviousValue.Has(out IEvent<TEvent> e))
                    e.Unsubscribe(_subscription);
                if (_subscription._var.Value.Has(out e))
                    e.Subscribe(_subscription);
                //для тех событий которые являеются своими же источниками
                if (_subscription._var.Value.Has(out TEvent eventSource))
                    _subscription.OnEvent(eventSource);
            }
		}
	}
}
