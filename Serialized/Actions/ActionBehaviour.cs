using BB.Di;
using BB.Serialized;
using System;

namespace BB
{
	public sealed class ActionBehaviour : EntityBehaviour
	{
		public SerializedActionsWithTriggers _actions = new();
		IDisposable _disposable;
		[OnSpawn]
		void OnSpawn() 
			=> _disposable = _actions.Subscribe(Entity);
		[OnDespawn]
		void OnDespawn() => _disposable?.Dispose();
	}
}
