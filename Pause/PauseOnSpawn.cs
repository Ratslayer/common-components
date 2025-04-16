namespace BB
{
	public sealed record PauseOnSpawn(Paused Paused)
	{
		[OnSpawn]
		void OnSpawn() => Paused.Push(true);
		[OnDespawn]
		void OnDespawn() => Paused.Pop();
	}
}
