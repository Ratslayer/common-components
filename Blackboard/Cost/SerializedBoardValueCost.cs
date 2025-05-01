using Flee.PublicTypes;
using System;
using System.Linq.Expressions;
using UnityEngine;

namespace BB
{
	public sealed record GameMessageBuffer : ListVariable<GameMessageBuffer, TextData>;
	public sealed class AddConstBoardValueAction : AbstractAddBoardValueAction<AddConstBoardValueAction>
	{
		double _value;
		public static AddConstBoardValueAction GetPooled(BaseBoardKey key, double value)
		{
			var result = GetPooledInternal();
			result._key = key;
			result._value = value;
			return result;
		}
		protected override double GetValue(Entity entity)
			=> _value;
	}
	public sealed class AddOtherBoardValueAction : AbstractAddBoardValueAction<AddOtherBoardValueAction>
	{
		double _multiplier;
		BaseBoardKey _addedKey;
		public static AddOtherBoardValueAction GetPooled(BaseBoardKey key, BaseBoardKey addedKey, double multiplier = 1)
		{
			var result = GetPooledInternal();
			result._key = key;
			result._multiplier = multiplier;
			result._addedKey = addedKey;
			return result;
		}
		protected override double GetValue(Entity entity)
			=> _addedKey.Get(entity) * _multiplier;
	}
	public sealed class AddExpressionBoardValueAction : AbstractAddBoardValueAction<AddExpressionBoardValueAction>
	{
		EntityExpression _expression;
		double _multiplier;
		public static AddExpressionBoardValueAction GetPooled(
			BaseBoardKey key,
			EntityExpression expression,
			double multiplier = 1)
		{
			var result = GetPooledInternal();
			result._key = key;
			result._expression = expression;
			result._multiplier = multiplier;
			return result;
		}
		protected override double GetValue(Entity entity)
			=> _expression.GetValue(entity) * _multiplier;
	}
	public abstract class AbstractAddBoardValueAction<TSelf> : ProtectedPooledObject<TSelf>,
		IGameActionCondition,
		IGameActionFailure,
		IGameActionSuccess
		where TSelf : AbstractAddBoardValueAction<TSelf>, new()
	{
		protected BaseBoardKey _key;

		protected abstract double GetValue(Entity entity);

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
				|| !context.Entity.Has(out UiConfig config))
				return;

			var data = new TextData
			{
				Text = $"Not enough {_key.Name}",
				Color = _key.ColorScheme?.TextColor ?? Color.black,
				FontSize = config._gameMessageFontSize
			};

			context.Messages.Add(data);
		}
		public void ExecuteSuccess(GameActionContext context)
		{
			if (!_key || !context.Entity.Has(out IBoard board))
				return;

			_key.Add(board, GetValue(context.Entity));
		}
	}
	[Serializable]
	public sealed class SerializedBoardValueCost : IFactory<IGameAction>
	{
		public BaseBoardKey _key;
		public EntityExpression _expression = new();
		public IGameAction Create()
		{
			var result = GameAction.GetPooled()
				.Add(AddExpressionBoardValueAction.GetPooled(_key, _expression, -1));
			return result;
		}
		public double GetValue(Entity entity)
			=> _expression.GetValue(entity);
	}
}