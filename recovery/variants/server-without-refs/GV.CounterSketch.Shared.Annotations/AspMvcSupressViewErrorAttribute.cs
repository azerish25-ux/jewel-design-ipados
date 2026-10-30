using System;

namespace GV.CounterSketch.Shared.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AspMvcSupressViewErrorAttribute : Attribute
{
}
