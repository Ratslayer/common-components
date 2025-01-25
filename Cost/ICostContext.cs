using System;
using System.Collections.Generic;

namespace BB
{
	public interface ICostContext
	{
		Entity Entity { get; set; }
		double Multiplier { get; set; }	
		void AddErrorMessage(string message);
		Action<List<string>> ProcessErrorMessages { get; set; }
		void ProcessErrors();
	}
}