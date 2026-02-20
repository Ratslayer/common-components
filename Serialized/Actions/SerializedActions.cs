using BB.Serialized.States;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace BB.Serialized.Actions
{
    public abstract class SerializedActions<TAction> where TAction : ISerializedAction
    {
        [SerializeReference] TAction[] _actions = { };
        public void Invoke(in SerializedActionContext context)
            => InvokeAsync(context).Forget();
        public UniTask InvokeAsync(in SerializedActionContext context)
            => _actions.Invoke(context);
    }

    [Serializable]
    public sealed class SerializedActions : SerializedActions<ISerializedAction>
    {
    }
    [Serializable]
    public sealed class SerializedAssetActions : SerializedActions<ISerializedAssetAction>
    {
    }
    [Serializable]
    public sealed class SerializedSceneActions : SerializedActions<ISerializedSceneAction>
    {
    }
    public abstract class SerializedActionSync : SerializedAction
    {
        public override bool WaitForExecution => false;
        public override UniTask Invoke(SerializedActionContext context)
        {
            try
            {
                if (IsValid(context))
                    InvokeSync(context);
            }
            catch (Exception e)
            {
                context.Entity.LogError($"Exception during execution of action {GetType().Name}: {e.Message}");
            }
            return UniTask.CompletedTask;
        }
        protected abstract void InvokeSync(SerializedActionContext context);
    }
    public abstract class SerializedActionAsync : SerializedAction
    {
        public bool _waitForExecution;
        public override bool WaitForExecution => _waitForExecution;
        public override UniTask Invoke(SerializedActionContext context)
        {
            try
            {
                if (!IsValid(context))
                    return UniTask.CompletedTask;

                var task = InvokeAsync(context);
                if (WaitForExecution)
                    return task;

                task.Forget();
            }
            catch (Exception e)
            {
                context.Entity.LogError($"Exception during execution of action {GetType().Name}: {e.Message}");
            }
            return UniTask.CompletedTask;
        }
        protected abstract UniTask InvokeAsync(SerializedActionContext context);
    }
    public abstract class SerializedAction : ISerializedAction
    {
        public abstract bool WaitForExecution { get; }

        public abstract UniTask Invoke(SerializedActionContext context);

        protected bool IsValid(SerializedActionContext context)
        {
            var validator = Validator
                .GetPooled()
                .WithEntity(context.Entity);
            SetupValidator(validator, context);
            return validator.ValidateAndDispose();
        }
        protected virtual void SetupValidator(Validator validator, SerializedActionContext context)
        {
        }
    }
    //{
    //    [SerializeReference]
    //    ISerializedActionCondition[] _conditions = { };
    //    [SerializeReference]
    //    ISerializedAction[] _successfulActions = { };
    //    [SerializeReference]
    //    ISerializedAction[] _failedActions = { };
    //    public void Invoke(SerializedActionContext context)
    //    {
    //        InvokeAsync().Forget();

    //        async UniTaskVoid InvokeAsync()
    //        {
    //            var success = true;
    //            foreach (var condition in _conditions)
    //                if (condition?.CanInvoke(context) is false)
    //                {
    //                    success = false;
    //                    break;
    //                }

    //            var actions = success ? _successfulActions : _failedActions;
    //            foreach (var action in actions)
    //                if (action is ISerializedActionSync sync)
    //                    sync.Invoke(context);
    //                else if (action is ISerializedActionAsync async)
    //                {
    //                    var task = async.Invoke(context);
    //                    if (async.WaitForExecution)
    //                        await task;
    //                    else task.Forget();
    //                }
    //        }
    //    }
    //}
}
