namespace BB
{
	public interface IGameActionCondition<in ContextType> : IGameAction<ContextType>
	{

		bool CanExecute(ContextType context);
	}
}