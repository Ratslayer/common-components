using BB.Di;
using BB.Serialized.Actions;
using BB.Serialized.Events;
using Cysharp.Threading.Tasks;

namespace BB.Serialized.States
{
    public interface ISerializedStateEvent : ISerializedEvent
    {
        IEntitySubscription CreateExitSubscription(in SerializedEventSubscriptionContext context);
    }
    public interface ISerializedStateAction : ISerializedAction
    {
    }
    public interface ISerializedStateExitActionSync : ISerializedStateAction
    {
        void ExitState(in SerializedActionContext context);
    }
    public interface ISerializedStateExitActionAsync : ISerializedStateAction
    {
        UniTask ExitState(in SerializedActionContext context);
    }
    public static class SerializedActionExtensions
    {
        public static async UniTask Invoke<TAction>(this TAction[] actions, SerializedActionContext context)
            where TAction : ISerializedAction
        {
            foreach (var action in actions)
            {
                var task = action.Invoke(context);
                if (action.WaitForExecution)
                    await task;
                else task.Forget();
            }
        }
        public static void Exit(this ISerializedStateAction[] actions, SerializedActionContext context)
        {
            foreach (var action in actions)
                if (action is ISerializedStateExitActionSync sync)
                    sync.ExitState(context);
                else if (action is ISerializedStateExitActionAsync async)
                    async.ExitState(context).Forget();
        }
    }

}
namespace BB.Serialized.Actions
{
}