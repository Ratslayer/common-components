using BB.Serialized.Actions;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace BB.Serialized.States
{
    [Serializable]
    public sealed class SerializedAssetStateActions
        : BaseSerializedStateActions<ISerializedAssetStateAction, ISerializedAssetAction>
    {
    }
    [Serializable]
    public sealed class SerializedSceneStateActions
        : BaseSerializedStateActions<ISerializedSceneStateAction, ISerializedSceneAction>
    {
    }
    public abstract class BaseSerializedStateActions<TStateAction, TAction>
        where TStateAction : ISerializedStateAction
        where TAction : ISerializedAction
    {
        [SerializeReference] TStateAction[] _onEnterExit = { };
        [SerializeReference] TAction[] _onEnter = { }, _onExit = { };
        public void Enter(in SerializedActionContext context)
        {
            _onEnterExit.Invoke(context).Forget();
            _onEnter.Invoke(context).Forget();
        }
        public void Exit(in SerializedActionContext context)
        {
            _onEnterExit.Exit(context).Forget();
            _onExit.Invoke(context).Forget();
        }
    }
    public abstract class SerializedStateAction : SerializedAction, ISerializedStateAction
    {
        public abstract UniTask InvokeExit(SerializedActionContext context);
    }
}