using System;

namespace GV.CounterSketch.Studio.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HtmlElementAttributesAttribute : Attribute
{
	[NotNull]
	public string Name { get; private set; }

	public HtmlElementAttributesAttribute()
	{
	}

	public HtmlElementAttributesAttribute([NotNull] string name)
	{
		Name = name;
	}
}
