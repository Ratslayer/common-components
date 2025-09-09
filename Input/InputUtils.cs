using BB.Di;
namespace BB
{
	public static class InputUtils
	{
		public static void InstallInput(IDiContainer container, IInputConfig config)
		{
			container.Instance(config);

			container.Event<InputEvent>();
			container.Event<MovePlayerInputEvent>();
			container.Event<TurnPlayerInputEvent>();
			container.Event<InputActionEvent>();

			container.System<GameInputSystem>();
			container.System<IPointer, MousePointer>();
			container.System<UnityInput>();
			container.System<ProcessInputActions>();

			container.Var<CurrentPlayerActions>();

			container.Stack<InputActions>();
			container.Stack<PlayerInputBlocked>();
			container.Stack<PlayerMovementBlocked>();
		}
	}
}