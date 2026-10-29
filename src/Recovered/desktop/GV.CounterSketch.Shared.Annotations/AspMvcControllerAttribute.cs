using System;

namespace GV.CounterSketch.Shared.Annotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
public sealed class AspMvcControllerAttribute : Attribute
{
	[NotNull]
	public string AnonymousProperty { get; private set; }

	public AspMvcControllerAttribute()
	{
	}

	public AspMvcControllerAttribute([NotNull] string anonymousProperty)
	{
		AnonymousProperty = anonymousProperty;
	}
}
