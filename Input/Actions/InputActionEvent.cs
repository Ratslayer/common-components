public readonly struct InputActionEvent
{
	public readonly InputActionState _state;
	public readonly InputActionWrapperAsset _input;
	public InputActionEvent(InputActionState state, InputActionWrapperAsset input)
	{
		_state = state;
		_input = input;
	}
	public bool Performed => _state is InputActionState.Performed;
}