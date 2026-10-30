using System.Collections.Generic;

namespace GemvisionGeometry.Geometry;

public class GMesh
{
	public readonly List<GMeshFace> Faces = new List<GMeshFace>();

	public readonly List<GPoint3D> Vertices = new List<GPoint3D>();

	public void Scale(double scale)
	{
		foreach (GPoint3D vertex in Vertices)
		{
			vertex.Scale(scale);
		}
	}

	public void Translate(GPoint3D t)
	{
		foreach (GPoint3D vertex in Vertices)
		{
			vertex.Translate(t);
		}
	}
}
