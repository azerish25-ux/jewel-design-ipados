using System;

namespace GV.CounterSketch.Common.Annotations;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class AspMvcAreaViewLocationFormatAttribute : Attribute
{
	[NotNull]
	public string Format { get; private set; }

	public AspMvcAreaViewLocationFormatAttribute([NotNull] string format)
	{
		Format = format;
	}
}
