namespace BB
{
	public interface IGameActionCondition : IGameAction
	{

		bool CanExecute(IGameActionContext context);
	}
}