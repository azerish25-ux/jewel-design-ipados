using System;

namespace GV.CounterSketch.Studio.Annotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
public sealed class RazorSectionAttribute : Attribute
{
}
