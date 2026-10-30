using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GemvisionGeometry.Geometry;

namespace OctTree;

public sealed class OctTree
{
	internal static int Depth;

	internal GPoint3D Center;

	internal double Sizehalf;

	internal OctTree[,,] Children;

	private List<GTriangle> _facets;

	internal bool Last;

	internal bool[] Sides;

	private void Subdivide(int i)
	{
		if (i == Depth)
		{
			Last = true;
			Sides = new bool[7];
			return;
		}
		Children = new OctTree[2, 2, 2];
		if (i <= 2)
		{
			ParallelOptions parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = ((Environment.ProcessorCount <= 1) ? 1 : (Environment.ProcessorCount - 1))
			};
			Parallel.For(0, 8, parallelOptions, delegate(int l)
			{
				Testnext((l >> 2) & 1, (l >> 1) & 1, l & 1);
			});
			_facets = null;
			Parallel.For(0, 8, parallelOptions, delegate(int l)
			{
				Tgo((l >> 2) & 1, (l >> 1) & 1, l & 1, i);
			});
		}
		else
		{
			for (int num = 0; num < 8; num++)
			{
				Testnext((num >> 2) & 1, (num >> 1) & 1, num & 1);
			}
			_facets = null;
			for (int num2 = 0; num2 < 8; num2++)
			{
				Tgo((num2 >> 2) & 1, (num2 >> 1) & 1, num2 & 1, i);
			}
		}
	}

	private void Testnext(int dX, int dY, int dZ)
	{
		double num = Sizehalf / 2.0;
		int num2 = ((dX != 0) ? 1 : (-1));
		int num3 = ((dY != 0) ? 1 : (-1));
		int num4 = ((dZ != 0) ? 1 : (-1));
		GPoint3D center = new GPoint3D(Center.X + num * (double)num2, Center.Y + num * (double)num3, Center.Z + num * (double)num4);
		List<GTriangle> list = IntMesh(center, num);
		if (list.Count > 0)
		{
			OctTree octTree = new OctTree
			{
				Center = center,
				Sizehalf = num,
				_facets = list
			};
			Children[dX, dY, dZ] = octTree;
		}
	}

	private List<GTriangle> IntMesh(GPoint3D center, double size)
	{
		List<GTriangle> list = new List<GTriangle>();
		foreach (GTriangle facet in _facets)
		{
			if (TriangleBoxIntersector.TriBoxOverlap(center, size, facet))
			{
				list.Add(facet);
			}
		}
		return list;
	}

	private void Tgo(int dX, int dY, int dZ, int i)
	{
		if (Children[dX, dY, dZ] != null)
		{
			Children[dX, dY, dZ].Subdivide(i + 1);
		}
	}

	public bool GetPixel(int x, int y, int z, out bool[] pSides)
	{
		return GetPixel(x, y, z, 1 << Depth - 1, out pSides);
	}

	private bool GetPixel(int x, int y, int z, int iDepth, out bool[] pSides)
	{
		if (Last)
		{
			pSides = Sides;
			return true;
		}
		int num = (((x & iDepth) != 0) ? 1 : 0);
		int num2 = (((y & iDepth) != 0) ? 1 : 0);
		int num3 = (((z & iDepth) != 0) ? 1 : 0);
		if (Children[num, num2, num3] == null)
		{
			pSides = null;
			return false;
		}
		return Children[num, num2, num3].GetPixel(x, y, z, iDepth >> 1, out pSides);
	}

	private static OctTree SetTopLevelOctTree(List<GMesh> m)
	{
		OctTree octTree = new OctTree
		{
			_facets = new List<GTriangle>()
		};
		foreach (GMesh item in m)
		{
			foreach (GMeshFace face in item.Faces)
			{
				if (face.IsQuad)
				{
					GPoint3D vert = item.Vertices[face.A];
					GPoint3D vert2 = item.Vertices[face.B];
					GPoint3D vert3 = item.Vertices[face.C];
					octTree._facets.Add(new GTriangle(vert, vert2, vert3));
					vert = item.Vertices[face.C];
					vert2 = item.Vertices[face.D];
					vert3 = item.Vertices[face.A];
					octTree._facets.Add(new GTriangle(vert, vert2, vert3));
				}
				else
				{
					GPoint3D vert4 = item.Vertices[face.A];
					GPoint3D vert5 = item.Vertices[face.B];
					GPoint3D vert6 = item.Vertices[face.C];
					octTree._facets.Add(new GTriangle(vert4, vert5, vert6));
				}
			}
		}
		return octTree;
	}

	private static void DoTreeInternal(List<GMesh> lstMesh, int depth, bool doScaling, bool makeOutputMesh, out long volume, out int surfaceArea, out int pixelCount, out double sideLength, out double scale, out GMesh gMeshOut, out string times)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Reset();
		stopwatch.Start();
		Depth = depth;
		scale = 1.0;
		GBoundingBox gBoundingBox = default(GBoundingBox);
		if (doScaling)
		{
			gBoundingBox = new GBoundingBox(lstMesh);
			double num = 78.0 / gBoundingBox.Size.X;
			double num2 = 78.0 / gBoundingBox.Size.Y;
			double num3 = 78.0 / gBoundingBox.Size.Z;
			if (num < scale)
			{
				scale = num;
			}
			if (num2 < scale)
			{
				scale = num2;
			}
			if (num3 < scale)
			{
				scale = num3;
			}
			foreach (GMesh item in lstMesh)
			{
				item.Translate(-gBoundingBox.Center);
				item.Scale(scale);
			}
		}
		OctTree octTree = SetTopLevelOctTree(lstMesh);
		octTree.Center = new GPoint3D(0.0, 0.0, 0.0);
		octTree.Sizehalf = 40.0;
		long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
		octTree.Subdivide(0);
		long elapsedMilliseconds2 = stopwatch.ElapsedMilliseconds;
		OctShell.shell(octTree, out volume, out surfaceArea, out pixelCount);
		long elapsedMilliseconds3 = stopwatch.ElapsedMilliseconds;
		gMeshOut = (makeOutputMesh ? ExtractMesh.Shell(octTree) : null);
		if (gMeshOut != null && doScaling)
		{
			gMeshOut.Scale(1.0 / scale);
			gMeshOut.Translate(gBoundingBox.Center);
		}
		if (doScaling)
		{
			foreach (GMesh item2 in lstMesh)
			{
				item2.Scale(1.0 / scale);
				item2.Translate(gBoundingBox.Center);
			}
		}
		long elapsedMilliseconds4 = stopwatch.ElapsedMilliseconds;
		sideLength = octTree.Sizehalf * 2.0 / Math.Pow(2.0, Depth);
		times = elapsedMilliseconds + "," + (elapsedMilliseconds2 - elapsedMilliseconds) + "," + (elapsedMilliseconds3 - elapsedMilliseconds2) + "," + (elapsedMilliseconds4 - elapsedMilliseconds3) + "," + elapsedMilliseconds4;
	}

	public static void GetVolume(List<GMesh> lstMesh, bool makeOutputMesh, out double volume, out GMesh gMeshOut)
	{
		DoTreeInternal(lstMesh, 11, doScaling: true, makeOutputMesh, out var volume2, out var _, out var pixelCount, out var sideLength, out var scale, out gMeshOut, out var _);
		volume = (double)volume2 * sideLength * sideLength * sideLength;
		volume -= (double)pixelCount * 4.346E-05;
		volume = volume * (1.0 / scale) * (1.0 / scale) * (1.0 / scale);
	}

	public static void GetMesh(List<GMesh> lstMesh, int depth, out GMesh gMeshOut)
	{
		DoTreeInternal(lstMesh, depth, doScaling: true, makeOutputMesh: true, out var _, out var _, out var _, out var _, out var _, out gMeshOut, out var _);
	}
}
