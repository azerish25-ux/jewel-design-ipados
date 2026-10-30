namespace GemvisionGeometry.Geometry;

public struct GTriangle
{
	public GPoint3D Vert0;

	public GPoint3D Vert1;

	public GPoint3D Vert2;

	public GTriangle(GPoint3D vert0, GPoint3D vert1, GPoint3D vert2)
	{
		Vert0 = vert0;
		Vert1 = vert1;
		Vert2 = vert2;
	}
}
