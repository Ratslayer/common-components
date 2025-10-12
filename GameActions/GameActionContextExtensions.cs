using UnityEngine;
namespace BB
{
    public static class GameActionContextExtensions
    {
        //public static bool TryExecuteWithMessage(this GameAction action, Entity entity, Vector3 position)
        //{
        //    return action.TryExecute(new() { Entity = entity }, OnEnd);
        //    void OnEnd(GameActionContext context)
        //        => PublishMessage(context, position);
        //}
        //public static void ExecuteWithMessage(this GameAction action, Entity entity, Vector3 position)
        //{
        //    action.Execute(new() { Entity = entity }, OnEnd);
        //    void OnEnd(GameActionContext context)
        //        => PublishMessage(context, position);
        //}
        public static GameAction AddBoardValue(this GameAction action, IBoardKey key, double value)
        => action.Add(AddConstBoardValueAction.GetPooled(key, value));
        public static GameAction AddBoardValue(this GameAction action, IBoardKey key, IBoardKey otherKey, double multiplier = 1)
            => action.Add(AddOtherBoardValueAction.GetPooled(key, otherKey, multiplier));
        public static GameAction Publish<TEvent>(this GameAction action, TEvent e, IEvent<TEvent> publisher = null)
            => action.Add(PublishEventAction<TEvent>.GetPooled(e, publisher));
        private static void PublishMessage(GameActionContext context, PositionAdapter position)
        {
            if (!context.Entity.Has(out IEvent<ShowHintEvent> showHint))
                return;
            foreach (var message in context.Messages)
                showHint.Publish(new()
                {
                    Text = message.Text,
                    Position = position
                });
        }
    }

}
