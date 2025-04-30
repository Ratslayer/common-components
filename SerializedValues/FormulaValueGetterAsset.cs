using Flee.PublicTypes;
using Sirenix.OdinInspector;
using System;
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
		public ExpressionVariable[] _variables;
		public string _expression;
		public double GetValue(Entity entity)
		{
			var context = new ExpressionContext();
			foreach (var variable in _variables)
				if (variable._name.IsValid())
					context.Variables[variable._name] = variable._value.GetValue(entity);

			var expression = context.CompileGeneric<double>(_expression);
			var result = expression.Evaluate();
			return result;
		}
		[Button, HideInPlayMode]
		void Test() => LogTestResult(World.Entity);

		[Button, HideInEditorMode]
		void TestOnPlayer() => LogTestResult(World.Get<Player>());

		void LogTestResult(Entity entity)
		{
			var value = GetValue(entity);
			Log.Logger.Info($"Expression {_expression} evaluates to {value}");
		}
	}
	public sealed class FormulaValueGetterAsset : EntityValueGetterAsset
	{
		public EntityExpression _expression = new();
		public override double GetValue(Entity entity)
			=> _expression.GetValue(entity);
	}
}