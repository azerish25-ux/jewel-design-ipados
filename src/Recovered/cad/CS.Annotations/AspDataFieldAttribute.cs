using System;

namespace CS.Annotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
public sealed class AspDataFieldAttribute : Attribute
{
}
