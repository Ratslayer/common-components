using BB.Di;
using UnityEngine;
namespace BB
{
	public sealed record Player : EntityVariable<Player>
	{
		public Vector3 Position => Has(out Root root) ? root.Position : Vector3.zero;
	}
	public abstract record SubscribeToEntityVariableEventSystem<TVariable, TEvent>(TVariable Var)
		where TVariable: EntityVariable<TVariable>
	{
		protected abstract void OnVariableEvent(TEvent e);
		[OnSpawn]
		void OnSpawn()
		{
			OnEventSubscribe(default);
			Var._event.Subscribe(OnEventSubscribe);
		}
		[OnDespawn]
		void OnDespawn()
		{
			if (Var.Value.Has(out IEvent<TEvent> e))
				e.Unsubscribe(OnVariableEvent);
			Var._event.Unsubscribe(OnEventSubscribe);
		}
		void OnEventSubscribe(TVariable _)
		{
			if (Var.PreviousValue.Has(out IEvent<TEvent> e))
				e.Unsubscribe(OnVariableEvent);
			if(Var.Value.Has(out e))
				e.Subscribe(OnVariableEvent);
		}
	}
}
