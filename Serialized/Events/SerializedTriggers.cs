using System;
using UnityEngine;
using BB.Serialized.Events;

namespace BB.Serialized
{
	[Serializable]
	public sealed class SerializedTriggers
	{
		[SerializeReference]
		ISerializedEvent[] _events = { };
		public SerializedTriggerSubscription CreateSubscription(in SerializedEventSubscriptionContext context)
		{
			var result = SerializedTriggerSubscription.GetPooled(context.Entity);
			foreach (var e in _events)
			{
				var subscription = e.CreateSubscription(context);
				if (subscription is not null)
					result.WithSubscription(subscription);
			}
			return result;
		}
	}
}