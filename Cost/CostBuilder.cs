using BB.Di;
using System;
using System.Collections.Generic;

namespace BB
{
	public sealed class CostBuilder : PooledObject<CostBuilder>, ICostBuilder, ICost
	{
		readonly List<object> _components = new();
		readonly CostBuilderContext _context = new();
		public CostBuilderContext Context => _context;
		public List<object> Components => _components;
		public ICost Build()
		{
			IBuilderUtils.MutateContext(this);
			_context.Init();
			return this;
		}
		public bool CanSpend()
		{
			var result = true;
			foreach (var component in _components)
				if (component is ICostCanSpendComponent canSpend)
					result &= canSpend.CanSpend(_context);

			foreach (var component in _components)
				if (component is ICostErrorComponent error)
					error.CostError(_context);

			return result;
		}

		public void Spend()
		{
			foreach (var component in _components)
				if (component is ICostSpendComponent canSpend)
					canSpend.Spend(_context);

			foreach (var component in _components)
				if (component is ICostErrorComponent error)
					error.CostError(_context);
		}
		public override void Dispose()
		{
			base.Dispose();
			_context.Dispose();

			foreach (var component in _components)
				if (component is IDisposable disposable)
					disposable.Dispose();
			_components.Clear();
		}
	}
}