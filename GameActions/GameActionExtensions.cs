using System;
namespace BB
{
	public static class GameActionExtensions
	{
		public static bool TryExecute<ContextType>(
			this IGameAction<ContextType> action,
			ContextType context,
			Action<ContextType> onEnd = null)
		{
			if (action is null)
				return false;

			if (action is IGameActionCondition<ContextType> condition
				&& !condition.CanExecute(context))
			{
				if (action is IGameActionFailure<ContextType> failure)
					failure.ExecuteFailure(context);
				onEnd?.Invoke(context);
				return false;
			}
			if (action is IGameActionSuccess<ContextType> success)
				success.ExecuteSuccess(context);
			onEnd?.Invoke(context);
			return true;
		}

		public static void Execute<ContextType>(
			this IGameAction<ContextType> action, 
			ContextType context, 
			Action<ContextType> onEnd = null)
		{
			if (action is not IGameActionSuccess<ContextType> success)
				return;
		
			success.ExecuteSuccess(context);
			onEnd?.Invoke(context);
		}
	}

}
