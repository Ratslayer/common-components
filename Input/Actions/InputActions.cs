using System.Collections.Generic;
using BB.Di;
namespace BB
{
	public sealed record InputActions : StackValue<InputActions, List<InputActionData>>;
}