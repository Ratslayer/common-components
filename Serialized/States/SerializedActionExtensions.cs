using BB.Serialized.Actions;
using Cysharp.Threading.Tasks;

namespace BB.Serialized.States
{
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
}