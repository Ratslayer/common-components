using System;

namespace BB
{
	public interface ICost : IDisposable
	{
		bool CanSpend();
		void Spend();
	}
}