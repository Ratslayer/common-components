using BB.Di;
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
				Board.SetToMax(resc);
			Board.FlushChanges();
		}
		[OnUpdate]
		void OnUpdate(UpdateTime time)
		{
			foreach (var resc in Resources)
				Board.AdvanceGeneration(resc, time._delta);
		}
	}
}