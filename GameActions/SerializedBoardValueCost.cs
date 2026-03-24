using System;
using UnityEngine;

namespace BB
{
    public sealed class PublishEventAction<TEvent>
        : ProtectedPooledObject<PublishEventAction<TEvent>>,
            IGameActionSuccess
    {
        TEvent _event;
        IEvent<TEvent> _publisher;

        public void ExecuteSuccess(IGameActionContext context)
            => _publisher?.Publish(_event);

        public static PublishEventAction<TEvent> GetPooled(TEvent e, IEvent<TEvent> publisher = null)
        {
            var result = GetPooledInternal();
            result._event = e;
            result._publisher = publisher ?? World.Get<IEvent<TEvent>>();
            return result;
        }
    }

    public sealed class AddConstBoardValueAction : AbstractAddBoardValueAction<AddConstBoardValueAction>
    {
        double _value;

        public static AddConstBoardValueAction GetPooled(IBoardKey key, double value)
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
        IBoardKey _addedKey;

        public static AddOtherBoardValueAction GetPooled(IBoardKey key, IBoardKey addedKey, double multiplier = 1)
        {
            var result = GetPooledInternal();
            result._key = key;
            result._multiplier = multiplier;
            result._addedKey = addedKey;
            return result;
        }

        protected override double GetValue(Entity entity)
            => entity.Require<IBoard>().Get(_addedKey) * _multiplier;
    }

    public abstract class AbstractAddBoardValueAction<TSelf> : ProtectedPooledObject<TSelf>,
        IGameActionCondition,
        IGameActionFailure,
        IGameActionSuccess
        where TSelf : AbstractAddBoardValueAction<TSelf>, new()
    {
        protected IBoardKey _key;

        protected abstract double GetValue(Entity entity);

        public bool CanExecute(IGameActionContext context)
        {
            if (_key is null)
                return true;
            if (!context.Entity.Has(out IBoard board))
                return false;
            return _key.CanAdd(board, GetValue(context.Entity));
        }

        public void ExecuteFailure(IGameActionContext context)
        {
            if (_key is not BaseBoardKey key
                || GetValue(context.Entity).GreaterThanOrEquals(0))
                return;

            var data = new TextData
            {
                Text = $"Not enough {_key.Name}",
                Color = Color.black,
                FontSize = 12
            };

            context.Messages.Add(data);
        }

        public void ExecuteSuccess(IGameActionContext context)
        {
            if (_key is null || !context.Entity.Has(out IBoard board))
                return;

            Board.Add(board, _key, this, GetValue(context.Entity));
        }
    }

    public sealed class AddExpressionBoardValueAction
        : AbstractAddBoardValueAction<AddExpressionBoardValueAction>
    {
        EntityExpression _expression;
        double _multiplier;

        public static AddExpressionBoardValueAction GetPooled(
            IBoardKey key,
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
}