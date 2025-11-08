using System;

namespace BB.Serialized.Events
{
	[Serializable]
	public sealed class BeforeGameStart : SerializedEvent<BeforeGameStartEvent> { }
}
