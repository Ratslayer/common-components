namespace BB
{
    public sealed class InitBlackboard : EntitySystem
    {
        [Inject] IBoard _board;
        [Inject] IBoardValuesProvider _values;
        [OnEvent]
        void OnSpawn(EntitySpawnedEvent _)
        {
            Board.Add(_board, _values);
        }
    }
    public sealed record InitResources()
    {
        [Inject]
        IBoard Board;
        [OnEvent(typeof(PostEntitySpawnedEvent),typeof(BeforeGameStartEvent))]
        void OnPostSpawn()
        {
            foreach (var key in Board.Keys)
                if (key is IResourceBoardKey resource
                    && resource.SetToMaxOnSpawn)
                    resource.SetToMax(Board);
        }
    }
}