using System;

namespace GV.CounterSketch.Shared.Annotations;

[AttributeUsage(AttributeTargets.Method)]
public sealed class PureAttribute : Attribute
{
}
