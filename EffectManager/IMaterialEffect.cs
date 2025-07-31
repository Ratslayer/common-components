using UnityEngine;

namespace BB
{
	public interface IMaterialEffect
	{
		void Apply(MaterialPropertyBlock mpb);
	}
	public interface IRendererEffect
	{
		void Apply(Renderer renderer);
	}
}
