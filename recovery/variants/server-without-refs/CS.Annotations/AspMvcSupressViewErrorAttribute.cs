using System;

namespace CS.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AspMvcSupressViewErrorAttribute : Attribute
{
}
