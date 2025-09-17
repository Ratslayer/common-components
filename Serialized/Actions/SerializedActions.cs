using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace BB.Serialized.Actions
{
	[Serializable]
    public sealed class SerializedActions
    {
        [SerializeReference]
        ISerializedActionCondition[] _conditions = { };
        [SerializeReference]
        ISerializedAction[] _successfulActions = { };
        [SerializeReference]
        ISerializedAction[] _failedActions = { };
        public void Invoke(SerializedActionContext context)
        {
            InvokeAsync().Forget();

            async UniTaskVoid InvokeAsync()
            {
                var success = true;
                foreach (var condition in _conditions)
                    if (condition?.CanInvoke(context) is false)
                    {
                        success = false;
                        break;
                    }

                var actions = success ? _successfulActions : _failedActions;
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
        }
    }
}
