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
			return _key.CanAdd(board, _value);
		}

		public void ExecuteFailure(GameActionContext context)
		{
			if (!_key
				|| _value.GreaterThanOrEquals(0)
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

			_key.Add(board, _value);
		}
	}
	public abstract class AbstractBoardValueAction<TSelf> : ProtectedPooledObject<TSelf>, IGameAction
		where TSelf : AbstractBoardValueAction<TSelf>, new()
	{
		protected BaseBoardKey _key;
		protected double _value;
		public static TSelf GetPooled(BaseBoardKey key, double value)
		{
			var result = GetPooledInternal();
			result._key = key;
			result._value = value;
			return result;
		}
	}
	[Serializable]
	public sealed class SerializedBoardValueCost : IFactory<IGameAction>
	{
		[SerializeField] SerializedBoardValue[] _values = { };
		public IGameAction Create()
		{
			var result = GameAction.GetPooled();
			foreach (var value in _values)
				result.Add(AddBoardValueAction.GetPooled(value.Key, -value.Value));
			return result;
		}
	}
}