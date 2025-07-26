using BB.Di;

namespace BB
{
	public sealed record AttachToPlayerOnEnable(Player Player) : EntitySystem
	{
		[OnEnable, OnEvent(typeof(Player))]
		void OnEnable() => Entity.AttachTo(Player);
	}
	public sealed record SpawnAndAttachToPlayer(
		IEntityInstaller Installer,
		Player Player) : EntitySystem
	{
		[OnSpawn, OnEvent(typeof(Player))]
		void Attach() => Installer.SpawnAndAttachTo(Player);
	}
}