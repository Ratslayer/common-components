using UnityEngine;

namespace BB
{
	public sealed record PauseOnStackChange
	{
		[OnEvent]
		void OnPauseChange(Paused paused)
		{
			Time.timeScale = paused ? 0 : 1;
		}
	}
}
