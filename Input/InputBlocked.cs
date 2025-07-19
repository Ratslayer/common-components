using BB.Di;
using System;
public sealed record InputBlocked : FlagStackValue<InputBlocked, InputBlockedFlags>;
[Flags]
public enum InputBlockedFlags
{
	None = 0,
	Movement = 1,
	Shooting = 1 << 1,
	Rotation = 1 << 2,
	All = ~0
}

public abstract record FlagStackValue<TSelf, TEnum> : StackValue<TSelf, TEnum>
	where TSelf : FlagStackValue<TSelf, TEnum>
	where TEnum : Enum
{
	public bool HasFlag(TEnum e) => Value.HasFlag(e);
}