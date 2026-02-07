using System.Collections.Generic;
namespace BB
{
	public sealed class GameAction : PooledObject<GameAction>,
		IGameActionCondition,
		IGameActionSuccess,
		IGameActionFailure
	{
		readonly List<IGameAction> _components = new();
		public bool CanExecute(IGameActionContext context)
		{
			foreach (var c in _components)
				if (c is IGameActionCondition condition
					&& !condition.CanExecute(context))
					return false;
			return true;
		}

		public void ExecuteSuccess(IGameActionContext context)
		{
			foreach (var c in _components)
				if (c is IGameActionSuccess success)
					success.ExecuteSuccess(context);
		}
		public void ExecuteFailure(IGameActionContext context)
		{
			foreach (var c in _components)
				if (c is IGameActionFailure failure)
					failure.ExecuteFailure(context);
		}

		public override void Dispose()
		{
			_components.DisposeElementsAndClear();
			base.Dispose();
		}

		public GameAction Add(IGameAction component)
		{
			if (component is not null)
				_components.Add(component);
			return this;
		}
		public GameAction Add(IFactory<IGameAction> component)
			=> Add(component?.Create());
	}
}