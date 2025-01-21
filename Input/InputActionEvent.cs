public readonly struct InputActionEvent
{
	public readonly InputActionState _state;
	public readonly IInputEvent _input;
	public InputActionEvent(InputActionState state, IInputEvent input)
	{
		_state = state;
		_input = input;
	}
	public bool Performed => _state is InputActionState.Performed;
}