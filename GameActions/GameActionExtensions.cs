using System;
namespace BB
{
	public static class GameActionExtensions
	{
		public static bool TryExecute(
			this IGameAction action,
			GameActionContext context,
			Action<GameActionContext> onEnd = null)
		{
			if (action is null)
				return false;

			if (action is IGameActionCondition condition
				&& !condition.CanExecute(context))
			{
				if (action is IGameActionFailure failure)
					failure.ExecuteFailure(context);
				onEnd?.Invoke(context);
				return false;
			}
			if (action is IGameActionSuccess success)
				success.ExecuteSuccess(context);
			onEnd?.Invoke(context);
			return true;
		}

		public static void Execute(
			this IGameAction action,
			GameActionContext context, 
			Action<GameActionContext> onEnd = null)
		{
			if (action is not IGameActionSuccess success)
				return;
		
			success.ExecuteSuccess(context);
			onEnd?.Invoke(context);
		}
	}

}
