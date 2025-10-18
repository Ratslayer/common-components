using BB.Di;
using BB.Serialized.Actions;
using BB.Serialized.Events;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

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
    public interface ISerializedSceneStateAction : ISerializedStateAction { }
    public interface ISerializedAssetStateAction : ISerializedStateAction { }
    public interface ISerializedAllStateAction : ISerializedSceneStateAction, ISerializedAssetStateAction { }
    public abstract class SerializedSceneEventState<TEvent>
        : BaseSerializedEventState<TEvent, ISerializedSceneAction>, ISerializedSceneStateAction
    { }
    public abstract class SerializedAssetEventState<TEvent>
        : BaseSerializedEventState<TEvent, ISerializedAssetAction>, ISerializedAssetStateAction
    { }

    public abstract class BaseSerializedEventState<TEvent, TSerializedAction> : SerializedStateActionSync
        where TSerializedAction : ISerializedAction
    {
        [SerializeReference]
        TSerializedAction[] _actions = { };
        IDisposable _subscription;
        protected override void InvokeSync(SerializedActionContext context)
        {
            _subscription = context.Entity.TempSubscribe<TEvent>(e => OnEvent(e, context));
        }
        protected override void InvokeExitSync(SerializedActionContext context)
        {
            _subscription?.Dispose();
        }
        protected virtual void OnEvent(TEvent e, SerializedActionContext context)
        {
            _actions.Invoke(context).Forget();
        }
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