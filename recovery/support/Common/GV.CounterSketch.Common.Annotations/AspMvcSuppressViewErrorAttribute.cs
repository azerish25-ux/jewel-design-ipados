using System;

namespace GV.CounterSketch.Common.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AspMvcSuppressViewErrorAttribute : Attribute
{
}
