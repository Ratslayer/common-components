using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace BB
{
	public sealed record MousePointer
	{
		public Vector3 Position => Mouse.current.position.value;
	}
	public sealed record ProcessInputActions(InputActions Actions)
	{
		readonly List<InputActionData> _pressedActions = new();
		[OnEvent]
		void OnInput(InputEvent e)
		{
			if (Actions.Value is null)
				return;
			if (e._state == InputActionState.Began)
			{
				foreach (var action in Actions.Value)
					if (action._asset == e._action)
					{
						action._onBegin?.Invoke();
						if (action._onEnd is null)
							return;
						_pressedActions.RemoveAll(action);
						_pressedActions.Add(action);
						return;
					}
			}
			if (e._state == InputActionState.Ended)
			{
				foreach (var i in -_pressedActions.Count)
				{
					var action = _pressedActions[i];
					if (action._asset != e._action)
						continue;
					action._onEnd.Invoke();
					_pressedActions.RemoveAt(i);
				}
			}
		}
	}
}