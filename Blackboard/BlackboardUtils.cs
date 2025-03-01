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
		container.Event<IBoard>();

		container.List<BoardTags>();

		container.List<BoardResources>();
		container.Event<ResourceChangedEvent>();
		container.System<UpdateBoardResourceGeneration>();
		container.System<UpdateBoardResourceOnMaxValueChange>();

		if (initialValues is not null)
			container.System<InitBlackboard>(initialValues);
	}
}