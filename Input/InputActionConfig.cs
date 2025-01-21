using System.Collections.Generic;
using UnityEngine;
namespace BB
{
	public sealed class InputActionConfig : BaseScriptableObject
	{
		public UnityEngine.InputSystem.InputActionAsset _inputSystemAsset;
		public List<InputEventAsset> _events = new();
		[Header("Actions")]
		public InputActionAsset _interact;
		public InputActionAsset _gatherAction;
		public InputActionAsset _closeUi;
		public InputActionAsset _rest;
		public InputActionAsset _attack;
		[Header("New Actions")]
		public InputActionAsset _click;
		public InputActionAsset _rightClick, _action1, _action2, _action3, _action4;
	}
}