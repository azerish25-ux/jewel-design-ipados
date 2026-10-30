using GemvisionGeometry.Geometry;

namespace OctTree;

internal static class ExtractMesh1
{
	private static int i0;

	private static GMesh meshout;

	private static OctTree top;

	public static GMesh Shell(OctTree octT)
	{
		i0 = 0;
		top = octT;
		meshout = new GMesh();
		DrawShell(octT, 0, 0, 0);
		return meshout;
	}

	private static void DrawShell(OctTree octT, int X, int Y, int Z)
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				for (int k = 0; k < 2; k++)
				{
					OctTree octTree = octT.Children[i, j, k];
					if (octTree != null)
					{
						if (octTree.Last)
						{
							DrawShellLast(octTree, (X << 1) | i, (Y << 1) | j, (Z << 1) | k);
						}
						else
						{
							DrawShell(octTree, (X << 1) | i, (Y << 1) | j, (Z << 1) | k);
						}
					}
				}
			}
		}
	}

	private static bool GetSide(int X, int Y, int Z, int S)
	{
		top.GetPixel(X, Y, Z, out var pSides);
		if (pSides != null)
		{
			return pSides[S];
		}
		return false;
	}

	private static void ClearSide(int X, int Y, int Z, int S)
	{
		top.GetPixel(X, Y, Z, out var pSides);
		if (pSides != null)
		{
			pSides[S] = false;
		}
	}

	private static void DrawShellLast(OctTree octT, int X, int Y, int Z)
	{
		if (octT.Sides == null)
		{
			return;
		}
		if (octT.Sides[0])
		{
			int i = 1;
			int j = 1;
			for (; GetSide(X, Y - i, Z, 0); i++)
			{
			}
			i--;
			for (; GetSide(X, Y + j, Z, 0); j++)
			{
			}
			j--;
			for (int k = Y - i; k <= Y + j; k++)
			{
				ClearSide(X, k, Z, 0);
			}
			i = i * 2 + 1;
			j = j * 2 + 1;
			GPoint3D item = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y + octT.Sizehalf * (double)j, octT.Center.Z + octT.Sizehalf);
			GPoint3D item2 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y - octT.Sizehalf * (double)i, octT.Center.Z + octT.Sizehalf);
			GPoint3D item3 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y - octT.Sizehalf * (double)i, octT.Center.Z - octT.Sizehalf);
			GPoint3D item4 = new GPoint3D(octT.Center.X + octT.Sizehalf, octT.Center.Y + octT.Sizehalf * (double)j, octT.Center.Z - octT.Sizehalf);
			meshout.Vertices.Add(item);
			meshout.Vertices.Add(item2);
			meshout.Vertices.Add(item3);
			meshout.Vertices.Add(item4);
			meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
		}
		if (octT.Sides[1])
		{
			int l = 1;
			int m = 1;
			for (; GetSide(X, Y - l, Z, 1); l++)
			{
			}
			l--;
			for (; GetSide(X, Y + m, Z, 1); m++)
			{
			}
			m--;
			for (int n = Y - l; n <= Y + m; n++)
			{
				ClearSide(X, n, Z, 1);
			}
			l = l * 2 + 1;
			m = m * 2 + 1;
			GPoint3D item5 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y + octT.Sizehalf * (double)m, octT.Center.Z + octT.Sizehalf);
			GPoint3D item6 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y + octT.Sizehalf * (double)m, octT.Center.Z - octT.Sizehalf);
			GPoint3D item7 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y - octT.Sizehalf * (double)l, octT.Center.Z - octT.Sizehalf);
			GPoint3D item8 = new GPoint3D(octT.Center.X - octT.Sizehalf, octT.Center.Y - octT.Sizehalf * (double)l, octT.Center.Z + octT.Sizehalf);
			meshout.Vertices.Add(item5);
			meshout.Vertices.Add(item6);
			meshout.Vertices.Add(item7);
			meshout.Vertices.Add(item8);
			meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
		}
		if (octT.Sides[2])
		{
			int num = 1;
			int num2 = 1;
			for (; GetSide(X - num, Y, Z, 2); num++)
			{
			}
			num--;
			for (; GetSide(X + num2, Y, Z, 2); num2++)
			{
			}
			num2--;
			for (int num3 = X - num; num3 <= X + num2; num3++)
			{
				ClearSide(num3, Y, Z, 2);
			}
			num = num * 2 + 1;
			num2 = num2 * 2 + 1;
			GPoint3D item9 = new GPoint3D(octT.Center.X + octT.Sizehalf * (double)num2, octT.Center.Y + octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
			GPoint3D item10 = new GPoint3D(octT.Center.X + octT.Sizehalf * (double)num2, octT.Center.Y + octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
			GPoint3D item11 = new GPoint3D(octT.Center.X - octT.Sizehalf * (double)num, octT.Center.Y + octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
			GPoint3D item12 = new GPoint3D(octT.Center.X - octT.Sizehalf * (double)num, octT.Center.Y + octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
			meshout.Vertices.Add(item9);
			meshout.Vertices.Add(item10);
			meshout.Vertices.Add(item11);
			meshout.Vertices.Add(item12);
			meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
		}
		if (octT.Sides[3])
		{
			int num4 = 1;
			int num5 = 1;
			for (; GetSide(X - num4, Y, Z, 3); num4++)
			{
			}
			num4--;
			for (; GetSide(X + num5, Y, Z, 3); num5++)
			{
			}
			num5--;
			for (int num6 = X - num4; num6 <= X + num5; num6++)
			{
				ClearSide(num6, Y, Z, 3);
			}
			num4 = num4 * 2 + 1;
			num5 = num5 * 2 + 1;
			GPoint3D item13 = new GPoint3D(octT.Center.X + octT.Sizehalf * (double)num5, octT.Center.Y - octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
			GPoint3D item14 = new GPoint3D(octT.Center.X - octT.Sizehalf * (double)num4, octT.Center.Y - octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
			GPoint3D item15 = new GPoint3D(octT.Center.X - octT.Sizehalf * (double)num4, octT.Center.Y - octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
			GPoint3D item16 = new GPoint3D(octT.Center.X + octT.Sizehalf * (double)num5, octT.Center.Y - octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
			meshout.Vertices.Add(item13);
			meshout.Vertices.Add(item14);
			meshout.Vertices.Add(item15);
			meshout.Vertices.Add(item16);
			meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
		}
		if (octT.Sides[4])
		{
			int num7 = 1;
			int num8 = 1;
			for (; GetSide(X - num7, Y, Z, 4); num7++)
			{
			}
			num7--;
			for (; GetSide(X + num8, Y, Z, 4); num8++)
			{
			}
			num8--;
			for (int num9 = X - num7; num9 <= X + num8; num9++)
			{
				ClearSide(num9, Y, Z, 4);
			}
			num7 = num7 * 2 + 1;
			num8 = num8 * 2 + 1;
			GPoint3D item17 = new GPoint3D(octT.Center.X + octT.Sizehalf * (double)num8, octT.Center.Y + octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
			GPoint3D item18 = new GPoint3D(octT.Center.X - octT.Sizehalf * (double)num7, octT.Center.Y + octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
			GPoint3D item19 = new GPoint3D(octT.Center.X - octT.Sizehalf * (double)num7, octT.Center.Y - octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
			GPoint3D item20 = new GPoint3D(octT.Center.X + octT.Sizehalf * (double)num8, octT.Center.Y - octT.Sizehalf, octT.Center.Z + octT.Sizehalf);
			meshout.Vertices.Add(item17);
			meshout.Vertices.Add(item18);
			meshout.Vertices.Add(item19);
			meshout.Vertices.Add(item20);
			meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
		}
		if (octT.Sides[5])
		{
			int num10 = 1;
			int num11 = 1;
			for (; GetSide(X - num10, Y, Z, 5); num10++)
			{
			}
			num10--;
			for (; GetSide(X + num11, Y, Z, 5); num11++)
			{
			}
			num11--;
			for (int num12 = X - num10; num12 <= X + num11; num12++)
			{
				ClearSide(num12, Y, Z, 5);
			}
			num10 = num10 * 2 + 1;
			num11 = num11 * 2 + 1;
			GPoint3D item21 = new GPoint3D(octT.Center.X + octT.Sizehalf * (double)num11, octT.Center.Y + octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
			GPoint3D item22 = new GPoint3D(octT.Center.X + octT.Sizehalf * (double)num11, octT.Center.Y - octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
			GPoint3D item23 = new GPoint3D(octT.Center.X - octT.Sizehalf * (double)num10, octT.Center.Y - octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
			GPoint3D item24 = new GPoint3D(octT.Center.X - octT.Sizehalf * (double)num10, octT.Center.Y + octT.Sizehalf, octT.Center.Z - octT.Sizehalf);
			meshout.Vertices.Add(item21);
			meshout.Vertices.Add(item22);
			meshout.Vertices.Add(item23);
			meshout.Vertices.Add(item24);
			meshout.Faces.Add(new GMeshFace(i0++, i0++, i0++, i0++));
		}
	}
}
