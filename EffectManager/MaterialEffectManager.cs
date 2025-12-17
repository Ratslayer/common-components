using BB.Di;
using System.Collections.Generic;
using UnityEngine;

namespace BB
{
	public sealed record MaterialEffectManager
		: IMaterialEffectManager
	{
		readonly Dictionary<Renderer, MaterialEffectContainer> _effects = new();
		readonly List<IMaterialEffectContainer> _dirtyContainers = new();
		
		public void Clear(Renderer renderer)
		{
			if (_effects.TryGetValue(renderer, out var effects))
			{
				_effects[renderer] = default;
				effects.Dispose();
			}
		}

		public IMaterialEffectContainer GetContainer(Renderer renderer)
		{
			if (!_effects.TryGetValue(renderer, out var effects))
			{
				effects = MaterialEffectContainer.GetPooled();
				effects._renderer = renderer;
				effects._manager = this;

				_effects[renderer] = effects;
			}
			return effects;
		}

		public void SetDirty(IMaterialEffectContainer container)
		{
			_dirtyContainers.AddUnique(container);
		}

		[OnEvent]
		void OnLateUpdate(LateUpdateEvent _)
		{
			if (_dirtyContainers.Count == 0)
				return;
			foreach (var container in _dirtyContainers)
				container.Update();
			_dirtyContainers.Clear();
		}
	}
}
