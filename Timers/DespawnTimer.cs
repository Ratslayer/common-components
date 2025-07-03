namespace BB
{
	public sealed class DespawnTimer : BaseSingleTimer<DespawnTimer>
	{
		Entity _entity;
		protected override void Invoke()
		{
			_entity.Despawn();
		}
		public static DespawnTimer Start(float seconds, Entity entity)
		{
			var result = GetPooledInternal();
			result._entity = entity;
			result.Start(seconds);
			return result;
		}
	}
}
