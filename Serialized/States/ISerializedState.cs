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
        UniTask InvokeExit(SerializedActionContext context);
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
        public static async UniTask Exit<TAction>(this TAction[] actions, SerializedActionContext context)
            where TAction : ISerializedStateAction
        {
            foreach (var action in actions)
            {
                var task = action.InvokeExit(context);
                if (action.WaitForExecution)
                    await task;
                else task.Forget();
            }
        }
    }
    public abstract class SerializedStateAction : SerializedAction, ISerializedStateAction
    {
        public abstract UniTask InvokeExit(SerializedActionContext context);
    }
    public abstract class SerializedStateActionSync : SerializedStateAction
    {
		public override bool WaitForExecution => false;
		public override UniTask Invoke(SerializedActionContext context)
		{
            if (IsValid(context))
                InvokeSync(context);
            return UniTask.CompletedTask;
        }
		public override UniTask InvokeExit(SerializedActionContext context)
		{
            if (IsValid(context))
                InvokeExitSync(context);
            return UniTask.CompletedTask;
        }
        protected abstract void InvokeSync(SerializedActionContext context);
        protected abstract void InvokeExitSync(SerializedActionContext context);
    }
    public abstract class SerializedStateActionAsync : SerializedStateAction
    {
        public bool _waitForExecution;
        public override bool WaitForExecution => _waitForExecution;
        public override UniTask Invoke(SerializedActionContext context)
        {
            if (!IsValid(context))
                return UniTask.CompletedTask;

            var task = InvokeAsync(context);
            if (WaitForExecution)
                return task;

            task.Forget();
            return UniTask.CompletedTask;
        }
        public override UniTask InvokeExit(SerializedActionContext context)
        {
            if (!IsValid(context))
                return UniTask.CompletedTask;

            var task = InvokeExitAsync(context);
            if (WaitForExecution)
                return task;

            task.Forget();
            return UniTask.CompletedTask;
        }
        protected abstract UniTask InvokeAsync(SerializedActionContext context);
        protected abstract UniTask InvokeExitAsync(SerializedActionContext context);
    }
}
namespace BB.Serialized.States
{
}