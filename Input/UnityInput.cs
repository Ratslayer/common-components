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
    public sealed class UnityInput : IDisposable
    {
        [Inject] IInputConfig _inputConfig;
        [Inject] IEvent<MouseMovedEvent> _mouseMoved;
        [Inject] IEvent<PlayerInputEvent> _inputPublisher;

        readonly List<InputActionSubscription> _actionSubscriptions = new();
        public string GetName(InputActionWrapperAsset e)
            => _actionSubscriptions.TryGetValue(s => s.Event == e, out var sub)
            ? sub.InputName : "[NO INPUT]";

        [OnEvent(typeof(EntityCreatedEvent))]
        private void InitInputAsset()
        {
            var actions = _inputConfig.GetAllActions();
            foreach (var action in actions)
                if (action)
                    _actionSubscriptions.Add(
                        new(this, _inputConfig.InputAsset[action._inputActionName], action, _inputPublisher));
            foreach (var subscription in _actionSubscriptions)
                subscription.Subscribe();

            _inputConfig.InputAsset["Mouse"].performed += MouseMove;
        }
        public void Dispose()
        {
            foreach (var subscription in _actionSubscriptions)
                subscription.Unsubscribe();
            _inputConfig.InputAsset["Mouse"].performed -= MouseMove;
        }
        [OnEvent(typeof(EntitySpawnedEvent))]
        void Enable() => _inputConfig.InputAsset.Enable();

        [OnEvent(typeof(EntityDespawnedEvent))]
        void Disable() => _inputConfig.InputAsset.Disable();
        void MouseMove(CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>();
            _mouseMoved.Publish(new() { Delta = delta });
        }
        sealed record InputActionSubscription(
            UnityInput Input,
            InputAction Action,
            InputActionWrapperAsset Event,
            IEvent<PlayerInputEvent> InputPublisher)
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