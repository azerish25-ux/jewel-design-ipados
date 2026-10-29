using System;

namespace GV.CounterSketch.Shared.Annotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
public sealed class AspMvcViewAttribute : PathReferenceAttribute
{
}
