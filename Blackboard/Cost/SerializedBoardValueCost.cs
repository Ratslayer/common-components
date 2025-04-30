using System;
using UnityEngine;

namespace BB
{
	public sealed record GameMessageBuffer : ListVariable<GameMessageBuffer, TextData>;
	public sealed class AddBoardValueAction : AbstractBoardValueAction<AddBoardValueAction>,
		IGameActionCondition,
		IGameActionFailure,
		IGameActionSuccess
	{
		public bool CanExecute(GameActionContext context)
		{
			if (!_key)
				return true;
			if (!context.Entity.Has(out IBoard board))
				return false;
			return _key.CanAdd(board, GetValue(context.Entity));
		}

		public void ExecuteFailure(GameActionContext context)
		{
			if (!_key
				|| GetValue(context.Entity).GreaterThanOrEquals(0)
				|| !context.Entity.Has(out GameMessageBuffer messageBuffer, out UiConfig config))
				return;

			var data = new TextData
			{
				Text = $"Not enough {_key.Name}",
				Color = _key.ColorScheme?.TextColor ?? Color.black,
				FontSize = config._gameMessageFontSize
			};

			messageBuffer.Add(data);
		}

		public void ExecuteSuccess(GameActionContext context)
		{
			if (!_key || !context.Entity.Has(out IBoard board))
				return;

			_key.Add(board, GetValue(context.Entity));
		}
	}
	public abstract class AbstractBoardValueAction<TSelf> : ProtectedPooledObject<TSelf>, IGameAction
		where TSelf : AbstractBoardValueAction<TSelf>, new()
	{
		protected BaseBoardKey _key;
		double _value;
		EntityExpression _expression;
		public static TSelf GetPooled(BaseBoardKey key, double value, EntityExpression expression = null)
		{
			var result = GetPooledInternal();
			result._key = key;
			result._value = value;
			result._expression = expression;
			return result;
		}
		protected double GetValue(Entity entity)
			=> _value * _expression?.GetValue(entity) ?? 1;
	}
	[Serializable]
	public sealed class SerializedBoardValueCost : IFactory<IGameAction>
	{
		public BaseBoardKey _key;
		public EntityExpression _expression = new();
		public IGameAction Create()
		{
			var result = GameAction.GetPooled()
				.Add(AddBoardValueAction.GetPooled(_key, -1, _expression));
			return result;
		}
		public double GetValue(Entity entity)
			=> _expression.GetValue(entity);
	}
}