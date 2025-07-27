namespace BB
{
	public interface IGameActionSuccess : IGameAction
	{
		void ExecuteSuccess(IGameActionContext context);
	}
}