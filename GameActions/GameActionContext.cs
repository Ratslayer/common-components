using System.Collections.Generic;
namespace BB
{
	public interface IGameActionContext
	{
		Entity Entity { get; }
		Entity TargetEntity { get; }
		List<TextData> Messages { get; }
	}
	public sealed class GameActionContext : IGameActionContext
	{
		public Entity Entity { get; init; }
		public Entity TargetEntity { get; init; }
		public List<TextData> Messages { get; init; } = new();
	}
}