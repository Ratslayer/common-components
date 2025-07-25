using UnityEngine.UIElements;

namespace BB
{
	public interface IGameActionFailure<in ContextType> : IGameAction<ContextType>
	{
		void ExecuteFailure(ContextType context);
	}
}