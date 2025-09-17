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
        public static async UniTask Invoke(this ISerializedAction[] actions, SerializedActionContext context)
        {
            foreach (var action in actions)
                if (action is ISerializedActionSync sync)
                    sync.Invoke(context);
                else if (action is ISerializedActionAsync async)
                {
                    var task = async.Invoke(context);
                    if (async.WaitForExecution)
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
namespace BB
{
}