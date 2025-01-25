namespace BB
{
	public sealed record InitBlackboard(
		IBoard Board,
		BoardValuesAsset Values) : EntitySystem
	{
		[OnSpawn]
		void OnSpawn()
		{
			Values.Add(new(Board, null, 1));
		}
	}
}