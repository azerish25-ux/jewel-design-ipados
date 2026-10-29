using System;

namespace GV.CounterSketch.Shared.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class AspMvcActionSelectorAttribute : Attribute
{
}
