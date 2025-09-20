using BB.Serialized;
using BB.Serialized.Actions;
using BB.Serialized.States;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BB
{
    public readonly struct StateContext
    {
        public StateMachineBehaviour Machine { get; init; }
        public Entity Entity { get; init; }
    }
    public sealed class StateBehaviour : BaseBehaviour
    {
        [SerializeReference]
        ISerializedStateAction[] _stateActions = { };
        [SerializeField] SerializedActions _onEnter = new(), _onExit = new();
        [SerializeField] SerializedActionsWithTriggers[] _subscriptions = { };
        readonly List<IDisposable> _disposables = new();
        public void Enter(in StateContext context)
        {
            DisposeAll();
            foreach (var action in _subscriptions)
                _disposables.Add(action.Subscribe(context.Entity));
            _stateActions.Invoke(new() { Entity = context.Entity }).Forget();
            _onEnter.Invoke(new() { Entity = context.Entity });
        }
        public void Exit(in StateContext context)
        {
            DisposeAll();
            _stateActions.Exit(new() { Entity = context.Entity });
            _onExit.Invoke(new() { Entity = context.Entity });
        }
        void DisposeAll()
        {
            _disposables.DisposeAndClear();
        }
    }
}