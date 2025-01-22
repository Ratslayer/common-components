namespace BB
{
	public sealed record BoardResourceManager(
		IBoard Board,
		BoardResources Resources,
		IEvent<BoardResourceManager> ResourcesChanged)
	{
		[OnPostSpawn]
		void AfterSpawn()
		{
			foreach (var resc in Resources)
				resc.SetToMax();
			Board.FlushChanges();
		}
	}
}