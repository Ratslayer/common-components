using System.Collections.Generic;

namespace BB
{
	public static class ICostBuilderExtensions
	{
		public static ICostBuilder Apply(this ICostBuilder builder, ICostBuilderMutator mutator)
		{
			mutator.MutateCostBuilder(builder);
			return builder;
		}
		public static ICostBuilder UseEntity(this ICostBuilder builder, in Entity entity)
		{
			builder.Context._entity = entity;
			return builder;
		}
		public static ICostBuilder UseAttack(this ICostBuilder builder, IAttack attack)
		{
			builder.Context._attack = attack;
			return builder;
		}
		public static ICostBuilder UseTarget(this ICostBuilder builder, in Entity target)
		{
			builder.Context._target = target;
			return builder;
		}
		public static ICostBuilder UseHighlightedAttackTarget(this ICostBuilder builder)
		{
			var highlightedTarget = World.Require<HighlightedAttackTarget>();
			builder.UseTarget(highlightedTarget.Value?.Entity ?? default);
			return builder;
		}
		public static ICostBuilder UseWeaponAttack(this ICostBuilder builder, bool mainHand)
		{
			var mutator = EquipmentAttackCostBuilderMutator.GetPooled();
			mutator._mainHand = mainHand;
			builder.Components.Add(mutator);
			return builder;
		}
		public static ICostBuilder CheckReach(this ICostBuilder builder)
		{
			var reach = ReachCost.GetPooled();
			builder.Components.Add(reach);
			return builder;
		}
		public static ICostBuilder Spend(this ICostBuilder builder, IEnumerable<IBoardValue> values)
		{
			foreach (var cost in values)
				if (cost.Key is IBoardResourceKey key)
					builder.SpendResource(key, cost.Value);
			return builder;
		}
		public static ICostBuilder SpendResource(this ICostBuilder builder, IBoardResourceKey key, double value)
		{
			var cost = ResourceCost.GetPooled();
			cost._key = key;
			cost._value = value;
			builder.Components.Add(cost);
			return builder;
		}
		public static ICostBuilder DisplayErrorsAsMouseHints(this ICostBuilder costBuilder)
		{
			var errors = DisplayErrorsAtMouse.GetPooled();
			costBuilder.Components.Add(errors);
			return costBuilder;
		}
	}
}