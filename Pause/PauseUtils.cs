using BB.Di;

namespace BB
{
	public static class PauseUtils
	{
		public static void BindPauseDependencies(IDiContainer container)
		{
			container.Stack<Paused>();
			container.System<PauseOnStackChange>();
		}
	}
}
