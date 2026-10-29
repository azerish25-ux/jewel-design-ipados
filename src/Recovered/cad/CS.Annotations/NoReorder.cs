using System;

namespace CS.Annotations;

[AttributeUsage(AttributeTargets.All)]
public sealed class NoReorder : Attribute
{
}
