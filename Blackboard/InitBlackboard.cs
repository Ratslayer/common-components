using UnityEngine;

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
    public sealed record InitResources(IBoard Board)
    {
        [OnPostSpawn, OnEvent(typeof(BeforeGameStartEvent))]
        void OnPostSpawn()
        {
            foreach (var key in Board.Keys)
                if (key is IResourceBoardKey resource
                    && resource.SetToMaxOnSpawn)
                    resource.SetToMax(Board);
        }
    }
}