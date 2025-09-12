using BB.Di;

namespace BB.Serialized.Events
{
	public interface ISerializedEvent
	{
		IEntitySubscription CreateSubscription(in SerializedEventSubscriptionContext context);
	}
}
