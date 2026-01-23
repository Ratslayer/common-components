namespace BB
{
	public readonly struct PlayerInputEvent
	{
		public readonly InputActionWrapperAsset _action;
		public readonly InputActionState _state;
		public PlayerInputEvent(
			InputActionWrapperAsset e,
			InputActionState s)
		{
			_action = e;
			_state = s;
		}

		public bool Performed(InputActionWrapperAsset action)
			=> Is(action, InputActionState.Performed);
		public bool Is(
			InputActionWrapperAsset action,
			InputActionState state)
			=> _action == action && _state == state;
	}
}