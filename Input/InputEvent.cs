public readonly struct InputEvent
{
	public readonly IInputEvent _event;
	public readonly InputActionState _state;
	public InputEvent(IInputEvent e, InputActionState s)
	{
		_event = e;
		_state = s;
	}
}
