namespace BB
{
	public sealed record InitBlackboard(BoardValuesAsset Values) : EntitySystem
	{
		[OnSpawn]
		void OnSpawn()
		{
			Values.Add(1, Entity);
		}
	}
}