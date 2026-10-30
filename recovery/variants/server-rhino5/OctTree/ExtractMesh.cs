using GemvisionGeometry.Geometry;

namespace OctTree;

internal static class ExtractMesh
{
	public static GMesh Shell(OctTree octT)
	{
		int i = 0;
		GMesh gMesh = new GMesh();
		DrawShell(octT, gMesh, ref i);
		return gMesh;
	}

	private static void DrawShell(OctTree octT, GMesh meshout, ref int i0)
	{
		if (octT.Last)
		{
			if (octT.Sides != null)
			{
				if (octT.Sides[0])
				{
					GPoint3D item = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					GPoint3D item2 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					GPoint3D item3 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					GPoint3D item4 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					meshout.Vertices.Add(item);
					meshout.Vertices.Add(item2);
					meshout.Vertices.Add(item3);
					meshout.Vertices.Add(item4);
					meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
				}
				if (octT.Sides[1])
				{
					GPoint3D item5 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					GPoint3D item6 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					GPoint3D item7 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					GPoint3D item8 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					meshout.Vertices.Add(item5);
					meshout.Vertices.Add(item6);
					meshout.Vertices.Add(item7);
					meshout.Vertices.Add(item8);
					meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
				}
				if (octT.Sides[2])
				{
					GPoint3D item9 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					GPoint3D item10 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					GPoint3D item11 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					GPoint3D item12 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					meshout.Vertices.Add(item9);
					meshout.Vertices.Add(item10);
					meshout.Vertices.Add(item11);
					meshout.Vertices.Add(item12);
					meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
				}
				if (octT.Sides[3])
				{
					GPoint3D item13 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					GPoint3D item14 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					GPoint3D item15 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					GPoint3D item16 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					meshout.Vertices.Add(item13);
					meshout.Vertices.Add(item14);
					meshout.Vertices.Add(item15);
					meshout.Vertices.Add(item16);
					meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
				}
				if (octT.Sides[4])
				{
					GPoint3D item17 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					GPoint3D item18 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					GPoint3D item19 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					GPoint3D item20 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
					meshout.Vertices.Add(item17);
					meshout.Vertices.Add(item18);
					meshout.Vertices.Add(item19);
					meshout.Vertices.Add(item20);
					meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
				}
				if (octT.Sides[5])
				{
					GPoint3D item21 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					GPoint3D item22 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					GPoint3D item23 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y - octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					GPoint3D item24 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y + octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
					meshout.Vertices.Add(item21);
					meshout.Vertices.Add(item22);
					meshout.Vertices.Add(item23);
					meshout.Vertices.Add(item24);
					meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
				}
			}
			return;
		}
		for (int j = 0; j < 2; j++)
		{
			for (int k = 0; k < 2; k++)
			{
				for (int l = 0; l < 2; l++)
				{
					if (octT.Children[j, k, l] != null)
					{
						DrawShell(octT.Children[j, k, l], meshout, ref i0);
					}
				}
			}
		}
	}
}
