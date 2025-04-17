namespace BB
{
	public sealed record PauseOnSpawn(Paused Paused)
		: PushValueOnSpawn<Paused, bool>(Paused, true);
}
