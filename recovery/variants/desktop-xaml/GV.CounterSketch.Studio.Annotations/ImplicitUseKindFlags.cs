using System;

namespace GV.CounterSketch.Studio.Annotations;

[Flags]
public enum ImplicitUseKindFlags
{
	Default = 7,
	Access = 1,
	Assign = 2,
	InstantiatedWithFixedConstructorSignature = 4,
	InstantiatedNoFixedConstructorSignature = 8
}
