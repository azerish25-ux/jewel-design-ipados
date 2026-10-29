using System;

namespace CS.Annotations;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class RegexPatternAttribute : Attribute
{
}
