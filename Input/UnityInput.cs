using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine;
namespace BB
{
    public readonly struct MouseMovedEvent
    {
        public Vector2 Delta { get; init; }
    }
    public sealed record UnityInput(
        IInputConfig Config,
        IEvent<MouseMovedEvent> MouseMoved,
        IEvent<InputEvent> InputPublisher) : IDisposable
    {
        readonly List<InputActionSubscription> _actionSubscriptions = new();
        public string GetName(InputActionWrapperAsset e)
            => _actionSubscriptions.TryGetValue(s => s.Event == e, out var sub)
            ? sub.InputName : "[NO INPUT]";

        [OnCreate]
        private void InitInputAsset()
        {
            var actions = Config.GetAllActions();
            foreach (var action in actions)
                if (action)
                    _actionSubscriptions.Add(
                        new(this, Config.InputAsset[action._inputActionName], action, InputPublisher));
            foreach (var subscription in _actionSubscriptions)
                subscription.Subscribe();

            Config.InputAsset["Mouse"].performed += MouseMove;
        }
        public void Dispose()
        {
            foreach (var subscription in _actionSubscriptions)
                subscription.Unsubscribe();
            Config.InputAsset["Mouse"].performed -= MouseMove;
        }
        [OnSpawn]
        void Enable() => Config.InputAsset.Enable();
        [OnDespawn]
        void Disable() => Config.InputAsset.Disable();
        void MouseMove(CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>();
            MouseMoved.Publish(new() { Delta = delta });
        }
        sealed record InputActionSubscription(
            UnityInput Input,
            InputAction Action,
            InputActionWrapperAsset Event,
            IEvent<InputEvent> InputPublisher)
        {
            const string Regex = @"\[(.*)\]";
            public string InputName => Action.ToString().MatchWithRegex(Regex).Split('/')[^1].ToUpper();
            public void Subscribe()
            {
                Action.performed += Invoke;
                Action.started += Invoke;
                Action.canceled += Invoke;
            }
            public void Unsubscribe()
            {
                Action.performed -= Invoke;
                Action.started -= Invoke;
                Action.canceled -= Invoke;
            }
            void Invoke(CallbackContext context)
            {
                InputActionState state = context.performed ? InputActionState.Performed
                    : context.started ? InputActionState.Began : InputActionState.Ended;
                InputPublisher.Publish(new(Event, state));

            }
        }
    }
}