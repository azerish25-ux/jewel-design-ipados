using System;

namespace GV.CounterSketch.Studio.Annotations;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
public sealed class StringFormatMethodAttribute : Attribute
{
	public string FormatParameterName { get; private set; }

	public StringFormatMethodAttribute(string formatParameterName)
	{
		FormatParameterName = formatParameterName;
	}
}
