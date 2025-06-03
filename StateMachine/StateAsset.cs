using UnityEngine;

namespace BB
{
	public sealed class StateAsset : EntityComponentAsset
	{
		[SerializeField] EntityComponentAsset[] _entityComponents;
		public override void Apply(Entity target, IStateData data)
		{
			foreach (var component in _entityComponents)
				component.Apply(target, data);
		}
		public override void Unapply(Entity target, IStateData data)
		{
			foreach (var component in _entityComponents)
				component.Unapply(data.Entity, data);
		}
	}
}