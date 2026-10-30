using System;

namespace GV.CounterSketch.Shared.Annotations;

[AttributeUsage(AttributeTargets.All)]
public sealed class LocalizationRequiredAttribute : Attribute
{
	public bool Required { get; private set; }

	public LocalizationRequiredAttribute()
		: this(required: true)
	{
	}

	public LocalizationRequiredAttribute(bool required)
	{
		Required = required;
	}
}
