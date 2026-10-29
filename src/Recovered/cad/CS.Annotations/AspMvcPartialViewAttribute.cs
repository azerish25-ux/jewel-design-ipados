using System;

namespace CS.Annotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
public sealed class AspMvcPartialViewAttribute : Attribute
{
}
