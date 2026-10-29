using System;

namespace GV.CounterSketch.Studio.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AspMvcSupressViewErrorAttribute : Attribute
{
}
