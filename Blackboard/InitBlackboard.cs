namespace BB
{
    public sealed record InitBlackboard(
        IBoard Board,
        IBoardValuesProvider Values) : EntitySystem
    {
        [OnSpawn]
        void OnSpawn()
        {
            Values.Add(new() { Board = Board });
        }
    }
}