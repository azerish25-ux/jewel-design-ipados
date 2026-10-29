using System;

namespace GV.CounterSketch.Common.Annotations;

[Flags]
public enum ImplicitUseTargetFlags
{
	Default = 1,
	Itself = 1,
	Members = 2,
	WithMembers = 3
}
