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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return transform;
	}

	public bool MorphGeometry(GeometryBase geometry)
	{
		return ((SpaceMorph)this).Morph(geometry);
	}

	public override Point3d MorphPoint(Point3d point)
	{
		throw new NotImplementedException();
	}
}
