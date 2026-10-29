using System;

namespace CS.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class AspMvcActionSelectorAttribute : Attribute
{
}
