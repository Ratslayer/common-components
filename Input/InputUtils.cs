using BB.Di;
namespace BB
{
	public static class InputUtils
	{
		public static void InstallInput(IDiContainer container, IInputConfig config)
		{
			container.Instance(config);
			container.System<IPointer, MousePointer>();
			container.System<UnityInput>();
			container.Event<InputEvent>();
			container.System<ProcessInputActions>();
			container.Stack<InputActions>();
			container.Stack<PlayerInputBlocked>();
			container.Event<InputActionEvent>();
		}
	}
}