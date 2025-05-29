using UnityEngine;

namespace BB
{
	public sealed class StateAsset : EntityComponentAsset
	{
		[SerializeField] EntityComponentAsset[] _entityComponents;
		public override void Apply(Entity target, IStateMachine machine)
		{
			foreach (var component in _entityComponents)
				component.Apply(target, machine);
		}
		public override void Unapply(Entity target, IStateMachine machine)
		{
			foreach (var component in _entityComponents)
				component.Unapply(machine.Entity, machine);
		}
	}
}