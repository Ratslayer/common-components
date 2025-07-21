using BB.Di;
namespace BB
{

	public static class BlackboardUtils
	{
		public static void BindBlackboard(
			this IDiContainer container,
			IBoardValuesProvider initialValues = null)
		{
			container.System<IBoard, Blackboard>();
			container.Event<IBoard>();

			if (initialValues is not null)
				container.System<InitBlackboard>(initialValues);
		}
	}
}