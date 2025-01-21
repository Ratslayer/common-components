namespace BB
{
	public sealed class ResourceCost : PooledObject<ResourceCost>, ICostCanSpendComponent, ICostSpendComponent
	{
		public IBoardResourceKey _key;
		public double _value;

		public bool CanSpend(in CostBuilderContext context)
		{
			if (_key.Get(new(context._board)) > _value)
				return true;

			context._errors.Add($"Not enough {_key.Name}");
			return false;
		}

		public void Spend(in CostBuilderContext context)
		{
			_key.Add(-_value, new(context._board));
		}
	}
	public sealed class EquipmentAttackCostBuilderMutator
		: PooledObject<EquipmentAttackCostBuilderMutator>, IBuilderContextMutator<CostBuilderContext>
	{
		public bool _mainHand;
		public void MutateContext(CostBuilderContext context)
		{
			if (context._entity.Has(out IEquipmentInventory equipment))
				context._attack = equipment.GetWeapon(_mainHand);
		}
	}
	public sealed class ReachCost : PooledObject<ReachCost>, ICostCanSpendComponent
	{
		public bool CanSpend(in CostBuilderContext context)
		{
			if (context._combatDetails is null)
				return true;

			var reachStatus = context._combatDetails.Reach.GetReachStatus();

			switch (reachStatus)
			{
				case ReachStatus.TooFar:
					context._errors.Add($"Too far!");
					return false;
				case ReachStatus.TooClose:
					context._errors.Add($"Too close!");
					return false;
				default:
					return true;
			}
		}
	}
}