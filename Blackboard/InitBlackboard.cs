namespace BB
{
	public sealed record InitBlackboard(
		IBoard Board,
		BoardValuesAsset Values) : EntitySystem
	{
		[OnSpawn]
		void OnSpawn()
		{
			BoardContext
				.GetPooled(Board)
				.AddAndDispose(Values);
		}
	}
}