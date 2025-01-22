using System.Numerics;

public readonly struct InputEvent
{
	public readonly InputActionWrapperAsset _action;
	public readonly InputActionState _state;
	public InputEvent(
		InputActionWrapperAsset e, 
		InputActionState s)
	{
		_action = e;
		_state = s;
	}
}
