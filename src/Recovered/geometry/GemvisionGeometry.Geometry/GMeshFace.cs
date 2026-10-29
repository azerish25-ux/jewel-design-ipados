namespace GemvisionGeometry.Geometry;

public class GMeshFace
{
	public int A;

	public int B;

	public int C;

	public int D;

	public bool IsTriangle => !IsQuad;

	public bool IsQuad { get; }

	public GMeshFace(int a, int b, int c)
	{
		A = a;
		B = b;
		C = c;
		IsQuad = false;
	}

	public GMeshFace(int a, int b, int c, int d)
	{
		A = a;
		B = b;
		C = c;
		D = d;
		IsQuad = true;
	}
}
