using System;

namespace GV.CounterSketch.Shared.Annotations;

[MeansImplicitUse]
public sealed class PublicAPIAttribute : Attribute
{
	[NotNull]
	public string Comment { get; private set; }

	public PublicAPIAttribute()
	{
	}

	public PublicAPIAttribute([NotNull] string comment)
	{
		Comment = comment;
	}
}
