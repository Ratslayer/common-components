using System.Collections.Generic;
using UnityEngine;

namespace BB
{
	public abstract class BasePooledMaterialEffect<TSelf>
		: PooledObject<TSelf>,
		IMaterialEffect
		where TSelf : BasePooledMaterialEffect<TSelf>, new()
	{
		readonly List<IMaterialEffectContainer> _containers = new();
		IMaterialEffectManager _manager;
		public abstract void Apply(in ApplyMaterialEffectContext context);
		public TSelf WithContainer(IMaterialEffectContainer container)
		{
			_containers.Add(container);
			container.AddEffect(this);
			return (TSelf)this;
		}
		public TSelf WithManager(IMaterialEffectManager manager)
		{
			_manager = manager;
			return (TSelf)this;
		}
		protected void Update()
		{
			foreach (var container in _containers)
				container.Update();
		}

		public TSelf WithRenderer(Renderer renderer)
		{
			ThrowHelper.IfNull(_manager,
				"Material Manager not set before calling WithRenderer");
			var container = _manager.GetContainer(renderer);
			return WithContainer(container);
		}
		public override void Dispose()
		{
			base.Dispose();
			foreach (var container in _containers)
				container.RemoveEffect(this);
			_containers.Clear();
		}
	}
}
