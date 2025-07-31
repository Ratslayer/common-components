using System;
namespace BB
{
	public readonly struct InputActionData
	{
		public readonly InputActionWrapperAsset _asset;
		public readonly Action _onBegin;
		public readonly Action _onEnd;
		public InputActionData(
			InputActionWrapperAsset asset,
			Action onBegin,
			Action onEnd = null)
		{
			_asset = asset;
			_onBegin = onBegin;
			_onEnd = onEnd;
		}
	}
}