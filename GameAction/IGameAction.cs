using System;
using System.Collections.Generic;
using UnityEngine;
namespace BB
{
	public sealed class GameActionContext
	{
		public Entity Entity { get; init; }
		public List<TextData> Messages { get; init; } = new();
	}
	public interface IGameAction : IDisposable
	{
	}
	public interface IGameActionCondition : IGameAction
	{

		bool CanExecute(GameActionContext context);
	}
	public interface IGameActionSuccess : IGameAction
	{
		void ExecuteSuccess(GameActionContext context);
	}
	public interface IGameActionFailure : IGameAction
	{
		void ExecuteFailure(GameActionContext context);
	}
	public sealed class GameAction : PooledObject<GameAction>,
		IGameActionCondition,
		IGameActionSuccess,
		IGameActionFailure
	{
		readonly List<IGameAction> _components = new();
		public bool CanExecute(GameActionContext context)
		{
			foreach (var c in _components)
				if (c is IGameActionCondition condition
					&& !condition.CanExecute(context))
					return false;
			return true;
		}

		public void ExecuteSuccess(GameActionContext context)
		{
			foreach (var c in _components)
				if (c is IGameActionSuccess success)
					success.ExecuteSuccess(context);
		}
		public void ExecuteFailure(GameActionContext context)
		{
			foreach (var c in _components)
				if (c is IGameActionFailure failure)
					failure.ExecuteFailure(context);
		}

		public override void Dispose()
		{
			_components.DisposeAndClear();
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

	public static class GameActionExtensions
	{
		public static bool TryExecute(
			this IGameAction action,
			Entity entity,
			Action<GameActionContext> onEnd = null)
		{
			if (action is null)
				return false;

			var context = new GameActionContext
			{
				Entity = entity,
			};
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
		public static bool TryExecuteWithMessage(this IGameAction action, Entity entity, Vector3 position)
		{
			return TryExecute(action, entity, OnEnd);
			void OnEnd(GameActionContext context)
				=> PublishMessage(context, position);
		}
		public static void Execute(this IGameAction action, Entity entity, Action<GameActionContext> onEnd = null)
		{
			if (action is not IGameActionSuccess success)
				return;
			var context = new GameActionContext
			{
				Entity = entity,
			};
			success.ExecuteSuccess(context);
			onEnd?.Invoke(context);
		}
		public static void ExecuteWithMessage(this IGameAction action, Entity entity, Vector3 position)
		{
			Execute(action, entity, OnEnd);
			void OnEnd(GameActionContext context)
				=> PublishMessage(context, position);
		}

		private static void PublishMessage(GameActionContext context, Vector3 position)
		{
			if (!context.Entity.Has(out IEvent<ShowMovingHintEvent> showHint))
				return;
			foreach (var message in context.Messages)
				showHint.Publish(new()
				{
					Position = position,
					Data = message
				});
		}
		public static GameAction AddBoardValue(this GameAction action, BaseBoardKey key, double value)
			=> action.Add(AddConstBoardValueAction.GetPooled(key, value));
		public static GameAction AddBoardValue(this GameAction action, BaseBoardKey key, BaseBoardKey otherKey, double multiplier = 1)
			=> action.Add(AddOtherBoardValueAction.GetPooled(key, otherKey, multiplier));
		public static GameAction Publish<TEvent>(this GameAction action, TEvent e, IEvent<TEvent> publisher = null)
			=> action.Add(PublishEventAction<TEvent>.GetPooled(e, publisher));
	}

}
