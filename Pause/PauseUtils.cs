using BB.Di;

namespace BB
{
	public static class PauseUtils
	{
		public static void BindPauseDependencies(IDiContainer container)
		{
			container.Var<Paused>();
			container.System<PauseOnStackChange>();
		}
	}
}
