using System;
using System.Collections.Generic;

namespace BB
{
	public sealed class CostContext : ICostContext, IDisposable
	{
		public Entity Entity { get; set; }
		public double Multiplier { get; set; } = 1;
		public Action<List<string>> ProcessErrorMessages { get; set; }
		readonly List<string> _errors = new();

		public void AddErrorMessage(string message)
			=> _errors.Add(message);
		public void ProcessErrors()
		{
			if (_errors.Count > 0)
				ProcessErrorMessages?.Invoke(_errors);
		}
		public void Dispose()
		{
			Entity = default;
			Multiplier = 1;
			ProcessErrorMessages = default;
			_errors.Clear();
		}
	}
}