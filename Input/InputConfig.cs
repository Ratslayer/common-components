using System.Collections.Generic;
namespace BB
{
	public sealed class InputConfig : BaseScriptableObject
	{
		public UnityEngine.InputSystem.InputActionAsset _inputSystemAsset;
		public List<InputActionWrapperAsset> _actions = new();
		public InputActionWrapperAsset _tap, _showTree;
	}
}