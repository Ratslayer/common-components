using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using static UnityEngine.InputSystem.InputAction;
namespace BB
{
	public sealed record UnityInput(
		IInputConfig Config,
		IEvent<InputEvent> InputPublisher) : IDisposable
	{
		readonly List<InputActionSubscription> _actionSubscriptions = new();
		public string GetName(InputActionWrapperAsset e)
			=> _actionSubscriptions.TryGetValue(s => s.Event == e, out var sub) 
			? sub.InputName : "[NO INPUT]";
		[OnCreate]
		private void InitInputAsset()
		{
			foreach (var action in Config.Actions)
				if (action)
					_actionSubscriptions.Add(
						new(this, Config.InputAsset[action._inputActionName], action, InputPublisher));
			foreach (var subscription in _actionSubscriptions)
				subscription.Subscribe();
		}
		public void Dispose()
		{
			foreach (var subscription in _actionSubscriptions)
				subscription.Unsubscribe();
		}
		[OnSpawn]
		void Enable() => Config.InputAsset.Enable();
		[OnDespawn]
		void Disable() => Config.InputAsset.Disable();

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