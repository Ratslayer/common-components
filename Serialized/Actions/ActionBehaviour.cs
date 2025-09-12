using BB.Di;
using BB.Serialized.Actions;
using System;

namespace BB
{
	public sealed class ActionBehaviour : EntityBehaviour
	{
		public SerializedActions _actions = new();
		IDisposable _disposable;
		[OnSpawn]
		void OnSpawn() 
			=> _disposable = _actions.Subscribe(Entity);
		[OnDespawn]
		void OnDespawn() => _disposable?.Dispose();
	}
}
