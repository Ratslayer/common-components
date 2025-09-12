using System;

namespace BB.Serialized.Events
{
	public readonly struct SerializedEventSubscriptionContext
	{
		public Entity Entity { get; init; }
		public Action Action { get; init; }
	}
}
