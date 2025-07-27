using UnityEngine.UIElements;

namespace BB
{
	public interface IGameActionFailure : IGameAction
	{
		void ExecuteFailure(IGameActionContext context);
	}
}