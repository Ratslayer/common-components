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
        [OnPostSpawn]
        void OnPostSpawn()
        {
            foreach (var key in Board.Keys)
                if (key is IResourceBoardKey resource
                    && resource.SetToMaxOnSpawn)
                    new AddBoardContext
                    {
                        Board = Board,
                        Key = key,
                        Value = 1e100
                    }.Add();
        }
    }
}