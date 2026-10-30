using System;

namespace GV.CounterSketch.Common.Annotations;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class AspMvcPartialViewLocationFormatAttribute : Attribute
{
	[NotNull]
	public string Format { get; private set; }

	public AspMvcPartialViewLocationFormatAttribute([NotNull] string format)
	{
		Format = format;
	}
}
