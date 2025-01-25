using System;
using System.Collections.Generic;

namespace BB
{
	public sealed class CostContext : PooledObject<CostContext>, ICostContext
	{
		public Entity Entity { get; set; }
		public double Multiplier { get; set; }
		public Action<List<string>> ProcessErrorMessages { get; set; }
		readonly List<string> _errors = new();

		public void AddErrorMessage(string message)
			=> _errors.Add(message);
		public void ProcessErrors()
		{
			if (_errors.Count > 0)
				ProcessErrorMessages?.Invoke(_errors);
		}
		public override void Dispose()
		{
			base.Dispose();
			Entity = default;
			ProcessErrorMessages = default;
			_errors.Clear();
		}
	}
}