using BB.Di;
namespace BB
{
	public static class InputUtils
	{
		public static void InstallInput(IDiContainer container, InputActionConfig config)
		{
			container.Instance(config);
			container.System<MousePointer>();
			container.System<UnityInput>();
			container.Event<InputEvent>();
			container.System<ProcessInputActions>();
			container.Stack<InputActions>();
			container.Stack<InputBlocked, bool>(false);
			container.Event<InputActionEvent>();
		}
		public static StackValuePushDisposable<bool> BlockPlayerInputTemp(bool block)
		{
			if (block && World.Has(out InputBlocked blocked))
				return blocked.Push(true);
			return default;
		}
	}
}