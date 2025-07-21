using BB.Di;
namespace BB
{
	public sealed record UpdateBoardResourceGeneration(
		IBoard Board)
	{
		[OnUpdate]
		void OnUpdate(UpdateTime time)
		{
			Board.UpdateGeneration(time._delta);
		}
	}
	//public sealed record UpdateBoardResourceOnMaxValueChange(
	//	IBoard Board,
	//	BoardConfig Config)
	//	: BoardResourceProcessorSystem(Board)
	//{
	//	readonly Dictionary<IBoardResourceKey, double> _lastMaxValues = new();
	//	[OnDespawn]
	//	void OnDespawn()
	//	{
	//		_lastMaxValues.Clear();
	//	}
	//	public override void Process(IBoard board)
	//	{
	//		foreach (var resc in Config._resources)
	//		{
	//			if (resc.MaxValueKey is null)
	//				continue;

	//			var oldMaxValue = _lastMaxValues.TryGetValue(resc, out var v) ? v : 0;
	//			var newMaxValue = resc.MaxValueKey.Get(Board);
	//			_lastMaxValues[resc] = newMaxValue;

	//			if (!newMaxValue.IsPositive())
	//				continue;

	//			if(!oldMaxValue.IsPositive())
	//			{
	//				resc.SetToMaxValue(Board);
	//				continue;
	//			}

	//			var ratio =  newMaxValue / oldMaxValue;
	//			var value = resc.Get(Board);
	//			var additionalValue = value * (ratio - 1);
	//			resc.Add(Board, additionalValue);
	//		}
	//	}
	//}
	//public abstract record BoardResourceProcessorSystem(
	//	IBoard Board) : IBoardProcessor
	//{
	//	public abstract void Process(IBoard board);

	//	[OnSpawn]
	//	void OnSpawn() => Board.AddProcessor(this);

	//	[OnDespawn]
	//	void OnDespawn() => Board.RemoveProcessor(this);
	//}
}