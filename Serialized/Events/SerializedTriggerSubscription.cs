using BB.Di;
using System.Collections.Generic;

namespace BB.Serialized.Events
{
	public sealed class SerializedTriggerSubscription
		: ProtectedPooledObject<SerializedTriggerSubscription>
	{
		Entity _entity;
		readonly List<IEntitySubscription> _subscriptions = new();
		public static SerializedTriggerSubscription GetPooled(Entity e)
		{
			var result = GetPooledInternal();
			result._entity = e;
			return result;
		}
		public SerializedTriggerSubscription WithSubscription(IEntitySubscription sub)
		{
			_subscriptions.Add(sub);
			return this;
		}
		public void Subscribe()
		{
			foreach (var sub in _subscriptions)
				sub.Subscribe(_entity._ref);
		}
		public override void Dispose()
		{
            foreach (var sub in _subscriptions)
                sub.Unsubscribe(_entity._ref);
            _subscriptions.DisposeAndClear();
			base.Dispose();
		}
	}
}
