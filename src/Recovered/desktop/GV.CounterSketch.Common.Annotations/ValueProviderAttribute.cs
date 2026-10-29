using System;

namespace GV.CounterSketch.Common.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ValueProviderAttribute : Attribute
{
	[NotNull]
	public string Name { get; private set; }

	public ValueProviderAttribute([NotNull] string name)
	{
		Name = name;
	}
}
