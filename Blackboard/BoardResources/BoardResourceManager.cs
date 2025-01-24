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
				resc.SetToMax();
			Board.FlushChanges();
		}
		[OnUpdate]
		void OnUpdate(UpdateTime time)
		{
			foreach (var resc in Resources)
				if (Board.Has(resc.ResourceKey.GenRateKey, out var genRate))
					resc.Add(genRate.GetValue(new(Board)) * time._delta, new(Board));
		}
	}
}