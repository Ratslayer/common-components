using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using BB.Di;
using static UnityEngine.InputSystem.InputAction;
namespace BB
{
	public interface IInputActionDetails
	{
		IInputEvent Event { get; }
		string Name { get; }
		string InputName { get; }
		InputActionPosition Position { get; }
	}
	public enum InputActionPosition
	{
		None,
		Bottom
	}
	public readonly struct InputActionData
	{
		public readonly IInputActionDetails _details;
		public readonly Action _onBegin;
		public readonly Action _onEnd;
		public InputActionData(
			IInputActionDetails details,
			Action onBegin,
			Action onEnd = null)
		{
			_details = details;
			_onBegin = onBegin;
			_onEnd = onEnd;
		}
	}
	public sealed record InputActions : StackValue<InputActions, List<InputActionData>>;
	public sealed record UnityInput(
		InputActionConfig Config,
		IEvent<InputEvent> InputPublisher) : IDisposable
	{
		readonly List<InputActionSubscription> _actionSubscriptions = new();
		public string GetName(IInputEvent e)
			=> _actionSubscriptions.TryGetValue(s => s.Event == e, out var sub) 
			? sub.InputName : "[NO INPUT]";
		UnityEngine.InputSystem.InputActionAsset Input => Config._inputSystemAsset;
		[OnCreate]
		private void InitInputAsset()
		{
			foreach (var action in Config._events)
				if (action)
					_actionSubscriptions.Add(
						new(this, Input[action._inputActionName], action, InputPublisher));
			foreach (var subscription in _actionSubscriptions)
				subscription.Subscribe();
		}
		public void Dispose()
		{
			foreach (var subscription in _actionSubscriptions)
				subscription.Unsubscribe();
		}
		[OnSpawn]
		void Enable() => Input.Enable();
		[OnDespawn]
		void Disable() => Input.Disable();

		sealed record InputActionSubscription(
			UnityInput Input,
			InputAction Action,
			IInputEvent Event,
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
				InputPublisher.Raise(new(Event, state));

			}
		}
	}
}