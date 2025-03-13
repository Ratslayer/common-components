using UnityEngine;

namespace BB
{
	public interface IMaterialEffectManager
	{
		IMaterialEffectContainer GetContainer(Renderer renderer);
		void SetDirty(IMaterialEffectContainer container);
	}
	public interface IMaterialEffectContainer
	{
		void AddEffect(IMaterialEffect effect);
		void RemoveEffect(IMaterialEffect effect);
		void Update();
	}
}
