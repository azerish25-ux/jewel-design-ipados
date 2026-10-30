using System;
using Rhino.Geometry;

namespace CS;

public class CSSpaceMorph : SpaceMorph
{
	public virtual bool IsValid()
	{
		return true;
	}

	public virtual Transform MorphFixed(Transform transform)
	{
		return transform;
	}

	public bool MorphGeometry(GeometryBase geometry)
	{
		return Morph(geometry);
	}

	public override Point3d MorphPoint(Point3d point)
	{
		throw new NotImplementedException();
	}
}
