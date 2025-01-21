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

		[OnEvent]
		void OnGameTimeAdvance(GameTimeAdvancedEvent time)
		{

			var flush = false;
			foreach (var resc in Resources)
				flush |= resc.AdvanceGeneration(time.Delta);

			if (flush)
				Board.FlushChanges();
		}
	}
}