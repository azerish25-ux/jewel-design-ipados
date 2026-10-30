using System;

namespace GV.CounterSketch.Shared.Annotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate)]
public sealed class CanBeNullAttribute : Attribute
{
}
