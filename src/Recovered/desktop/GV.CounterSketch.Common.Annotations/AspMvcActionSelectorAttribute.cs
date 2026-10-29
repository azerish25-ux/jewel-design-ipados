using System;

namespace GV.CounterSketch.Common.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class AspMvcActionSelectorAttribute : Attribute
{
}
