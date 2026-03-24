using Flee.PublicTypes;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace BB
{
    [Serializable]
    public struct ExpressionVariable
    {
        public string _name;
        public SerializedDouble _value;
    }

    [Serializable]
    public sealed class EntityExpression
    {
        public ExpressionVariable[] _variables = { };
        public string _expression;
        private IGenericExpression<double> _compiledExpression;

        public double GetValue(Entity entity)
        {
            if (_compiledExpression is null)
                Recompile();

            foreach (var variable in _variables)
            {
                if (!variable._name.IsValid())
                    continue;
                var value = variable._value.GetValue(entity);
                _compiledExpression.Context.Variables[variable._name] = value;
            }

            var result = _compiledExpression.Evaluate();
            return result;
        }

        [Button]
        void Recompile()
        {
            var context = new ExpressionContext();

            foreach (var variable in _variables)
                if (variable._name.IsValid())
                    context.Variables[variable._name] = 0d;

            _compiledExpression = context.CompileGeneric<double>(_expression);
        }

        [Button, HideInPlayMode]
        void Test() => LogTestResult(World.Entity);

        [Button, HideInEditorMode]
        void TestOnPlayer() => LogTestResult(World.Get<Player>());

        void LogTestResult(Entity entity)
        {
            var value = GetValue(entity);
            entity
                .GetLogger()
                .Info($"Expression {_expression} evaluates to {value}");
        }
    }

    public sealed class FormulaValueGetterAsset : BoardValueGetterAsset
    {
        public EntityExpression _expression = new();

        public override double GetValue(IBoard board, in GetBoardContext context)
            => _expression.GetValue(board.Entity);
    }
}