using System;

namespace CS.Annotations;

[AttributeUsage(AttributeTargets.Method)]
public sealed class CollectionAccessAttribute : Attribute
{
	public CollectionAccessType CollectionAccessType { get; private set; }

	public CollectionAccessAttribute(CollectionAccessType collectionAccessType)
	{
		CollectionAccessType = collectionAccessType;
	}
}
