using System;
namespace BB
{
	public readonly struct InputActionData
	{
		public readonly IInputActionDetails _details;
		public readonly Action _onBegin;
		public readonly Action _onEnd;
		public InputActionData(
			IInputActionDetails details,
			Action onBegin,
			Action onEnd = null)
		{
			_details = details;
			_onBegin = onBegin;
			_onEnd = onEnd;
		}
	}
}