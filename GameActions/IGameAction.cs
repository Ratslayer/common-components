using System;
namespace BB
{
	public interface IGameAction<in ContextType> : IDisposable
	{
	}
}