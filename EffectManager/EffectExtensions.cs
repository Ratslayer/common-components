using BB.Di;
namespace BB
{
	public static class EffectExtensions
	{
		public static void BindEffectManagers(
			this IDiContainer container)
		{
			container.System<IMaterialEffectManager, MaterialEffectManager>();
		}
	}
}
