using System;

namespace CS.Annotations;

[AttributeUsage(AttributeTargets.Method)]
public sealed class PureAttribute : Attribute
{
}
