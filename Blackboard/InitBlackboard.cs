namespace BB
{
    public sealed class InitBlackboard : EntitySystem
    {
        [Inject] IBoard _board;
        [Inject] BoardValue[] _values;

        [OnEvent]
        void OnSpawn(EntitySpawnedEvent _)
        {
            _board.Add(_values, this);
        }
    }

    public sealed class InitResources
    {
        [Inject] IBoard _board;

        [OnEvent(typeof(PostEntitySpawnedEvent), typeof(InitGameEvent))]
        void InitializeResources()
        {
            foreach (var key in _board.Keys)
                if (key is IResourceBoardKey resource
                    && resource.SetToMaxOnSpawn)
                    resource.SetToMax(_board, "InitResources");
        }
    }
}