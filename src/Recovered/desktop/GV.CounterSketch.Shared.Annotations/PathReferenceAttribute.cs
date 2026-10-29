using System;

namespace GV.CounterSketch.Shared.Annotations;

[AttributeUsage(AttributeTargets.Parameter)]
public class PathReferenceAttribute : Attribute
{
	[NotNull]
	public string BasePath { get; private set; }

	public PathReferenceAttribute()
	{
	}

	public PathReferenceAttribute([PathReference] string basePath)
	{
		BasePath = basePath;
	}
}
