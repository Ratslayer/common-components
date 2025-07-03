namespace BB
{
	public static class TimerExtensions
	{
		public static Entity DespawnAfter(this Entity entity, float seconds)
		{
			if (!entity)
				return entity;
			var timer = DespawnTimer.Start(seconds, entity);
			entity._ref.AddDespawnDisposable(timer);
			return entity;
		}
	}
}
