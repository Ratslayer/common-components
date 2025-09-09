
using BB.Di;

namespace BB
{
	public sealed record PlayerInputBlocked : BoolStackValue<PlayerInputBlocked>;
	public sealed record PlayerMovementBlocked : BoolStackValue<PlayerMovementBlocked>;

	public sealed record BlockPlayerInputOnSpawn(PlayerInputBlocked Input)
		: PushValueOnSpawn<PlayerInputBlocked,bool>(Input, true);
}
