using System.Collections.Generic;

namespace BB
{
	public sealed class CostBuilder : ProtectedPooledObject<CostBuilder>, ICostBuilder, ICost
	{
		public ICostContext Context { get; set; }
		readonly List<ICostComponent> _components = new();

		public void AddComponent(ICostComponent component)
			=> _components.Add(component);

		public ICost Build()
			=> this;

		public bool CanSpend()
		{
			var result = true;
			foreach (var component in _components)
				if (component is ICanSpendCostComponent c
					&& !c.CanSpend(Context))
					result = false;

			Context.ProcessErrors();
			return result;
		}

		public void Spend()
		{
			foreach (var component in _components)
				if (component is ISpendCostComponent c)
					c.Spend(Context);
		}
		public static ICostBuilder Begin()
		{
			var result = GetPooledInternal();
			result.Context = CostContext.GetPooled();
			result.Context.Multiplier = 1;
			return result;
		}
	}
}