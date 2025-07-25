using System.Collections.Generic;
namespace BB
{
	public sealed class GameAction<ContextType> : PooledObject<GameAction<ContextType>>,
		IGameActionCondition<ContextType>,
		IGameActionSuccess<ContextType>,
		IGameActionFailure<ContextType>
	{
		readonly List<IGameAction<ContextType>> _components = new();
		public bool CanExecute(ContextType context)
		{
			foreach (var c in _components)
				if (c is IGameActionCondition<ContextType> condition
					&& !condition.CanExecute(context))
					return false;
			return true;
		}

		public void ExecuteSuccess(ContextType context)
		{
			foreach (var c in _components)
				if (c is IGameActionSuccess<ContextType> success)
					success.ExecuteSuccess(context);
		}
		public void ExecuteFailure(ContextType context)
		{
			foreach (var c in _components)
				if (c is IGameActionFailure<ContextType> failure)
					failure.ExecuteFailure(context);
		}

		public override void Dispose()
		{
			_components.DisposeAndClear();
			base.Dispose();
		}

		public GameAction<ContextType> Add(IGameAction<ContextType> component)
		{
			if (component is not null)
				_components.Add(component);
			return this;
		}
		public GameAction<ContextType> Add(IFactory<IGameAction<ContextType>> component)
			=> Add(component?.Create());
	}
}