using System;

namespace GV.CounterSketch.Shared.Annotations;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class AspMvcAreaAttribute : PathReferenceAttribute
{
	[NotNull]
	public string AnonymousProperty { get; private set; }

	public AspMvcAreaAttribute()
	{
	}

	public AspMvcAreaAttribute([NotNull] string anonymousProperty)
	{
		AnonymousProperty = anonymousProperty;
	}
}
