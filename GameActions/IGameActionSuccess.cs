namespace BB
{
	public interface IGameActionSuccess<in ContextType> : IGameAction<ContextType>
	{
		void ExecuteSuccess(ContextType context);
	}
}