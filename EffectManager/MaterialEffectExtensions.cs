using UnityEngine;

namespace BB
{
	public static class MaterialEffectExtensions
	{
		public static TEffect WithPart<TEffect>(
			this TEffect effect,
			IEnemyPart part)
			where TEffect : BasePooledMaterialEffect<TEffect>, new()
		{
			foreach (var renderer in part.Renderers)
				effect.WithRenderer(renderer);
			return effect;
		}
		public static SetColorMaterialEffect WithColor(
			this SetColorMaterialEffect effect,
			Color color)
		{
			effect.Color = color;
			return effect;
		}
	}
}
