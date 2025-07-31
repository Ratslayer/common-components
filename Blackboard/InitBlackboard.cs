namespace BB
{
	public sealed record InitBlackboard(
		IBoard Board,
		IBoardValuesProvider Values) : EntitySystem
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