using System.Collections.Generic;
namespace BB
{
	public interface IInputConfig
	{
		UnityEngine.InputSystem.InputActionAsset InputAsset { get; }
		IReadOnlyCollection<InputActionWrapperAsset> Actions { get; }
	}
}