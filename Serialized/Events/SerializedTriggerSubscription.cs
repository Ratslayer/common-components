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
				_entity._ref?.AddTemporarySubscription(sub);
		}
		public override void Dispose()
		{
			foreach (var sub in _subscriptions)
				_entity._ref?.RemoveTemporarySubscription(sub);
			_subscriptions.DisposeAndClear();
			base.Dispose();
		}
	}
}
