using System;

namespace CS.Parametric.Channels;

[Flags]
public enum ChannelFlag
{
	None = 0,
	NotRequired = 1,
	NotLoopChecked = 2,
	Locked = 4
}
