using System;

namespace GV.CounterSketch.Common.Annotations;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class AspMvcAreaMasterLocationFormatAttribute : Attribute
{
	[NotNull]
	public string Format { get; private set; }

	public AspMvcAreaMasterLocationFormatAttribute([NotNull] string format)
	{
		Format = format;
	}
}
