using BB;
using BB.Di;
namespace BB
{
	public sealed class BuffDisposable : ProtectedPooledObject<BuffDisposable>
	{
		IBoardValuesProvider _provider;
		IBoard _board;
		public static BuffDisposable GetPooled(IBoard board, IBoardValuesProvider provider)
		{
			var result = GetPooledInternal();
			result._provider = provider;
			result._board = board;
			return result;
		}
		public override void Dispose()
		{
			base.Dispose();
			_provider.Add(new(_board, null, -1));
		}
	}

	public static class BlackboardUtils
	{
		public static void BindBlackboard(
			this IDiContainer container,
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
}