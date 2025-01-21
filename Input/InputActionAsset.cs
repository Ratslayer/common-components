using UnityEngine;
namespace BB
{
	public sealed class InputActionAsset : BaseScriptableObject, IInputActionDetails
	{
		[SerializeField] string _name;
		[SerializeField] InputEventAsset _event;
		[SerializeField] InputActionPosition _position;
		public string Name => _name;
		public IInputEvent Event => _event;
		public InputActionPosition Position => _position;
		public string InputName
		{
			get
			{
				if (World.Has(out UnityInput input))
					return input.GetName(_event);
				return "[No Input Name]";
			}
		}
	}
}