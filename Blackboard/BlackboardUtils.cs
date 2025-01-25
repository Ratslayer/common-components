using BB;
using BB.Di;
public static class BlackboardUtils
{
	public static void InstallBlackboard(
		IDiContainer container,
		IBoardValuesProvider initialValues = null)
	{
		container.System<IBoard, Blackboard>();
		container.System<IConditionalBoardValues, ConditionalBoardValues>();
		container.Event<BoardChangedEvent>();
		container.List<BoardTags>();
		container.List<BoardResources>();
		container.System<BoardResourceManager>();
		container.Event<BoardResourceManager>();
		container.Event<ResourceChangedEvent>();
		if (initialValues is not null)
			container.System<InitBlackboard>(initialValues);
	}
}