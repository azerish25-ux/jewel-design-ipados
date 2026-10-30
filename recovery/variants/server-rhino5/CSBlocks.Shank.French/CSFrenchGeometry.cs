using System.Collections.Generic;
using System.Linq;
using Rhino.Geometry;

namespace CSBlocks.Shank.French;

public sealed class CSFrenchGeometry
{
	public readonly GeometryBase GTop;

	public readonly List<GeometryBase> GSide;

	public readonly GeometryBase GBase;

	public readonly GeometryBase GAll;

	public readonly Curve ECrv;

	public CSFrenchGeometry(IEnumerable<GeometryBase> inObjects, List<Curve> crv, string part, bool isMesh, bool isCam)
	{
		GTop = null;
		GSide = new List<GeometryBase>();
		GBase = null;
		ECrv = null;
		GeometryBase geometryBase = null;
		List<GeometryBase> list = new List<GeometryBase>();
		GeometryBase geometryBase2 = null;
		foreach (GeometryBase inObject in inObjects)
		{
			string obj = inObject.GetUserString("CSName")?.ToLower();
			if (obj == part + "top")
			{
				GTop = inObject;
			}
			if (obj == part + "topcam")
			{
				geometryBase = inObject;
			}
			if (obj == part + "side")
			{
				GSide.Add(inObject);
			}
			if (obj == part + "sidecam")
			{
				list.Add(inObject);
			}
			if (obj == part + "base")
			{
				GBase = inObject;
			}
			if (obj == part + "basecam")
			{
				geometryBase2 = inObject;
			}
		}
		if (isCam)
		{
			if (geometryBase != null)
			{
				GTop = geometryBase;
			}
			if (list.Count != 0)
			{
				GSide = list;
			}
			if (geometryBase2 != null)
			{
				GBase = geometryBase2;
			}
		}
		if (isMesh)
		{
			Mesh mesh = new Mesh();
			mesh.Append(GTop as Mesh);
			mesh.Append(GBase as Mesh);
			foreach (Mesh item in GSide.Select((GeometryBase s) => s as Mesh))
			{
				mesh.Append(item);
			}
			GAll = mesh;
		}
		else
		{
			List<Brep> list2 = new List<Brep>();
			if (GTop != null)
			{
				list2.Add(GTop as Brep);
			}
			if (GBase != null)
			{
				list2.Add(GBase as Brep);
			}
			list2.AddRange(GSide.Select((GeometryBase s) => s as Brep));
			Brep[] array = Brep.JoinBreps(list2, 0.001);
			if (array != null && array.Length == 1)
			{
				GAll = array[0];
			}
		}
		if (crv == null)
		{
			return;
		}
		foreach (Curve item2 in crv)
		{
			if (item2.GetUserString("CSName").ToLower() == part + "crv")
			{
				ECrv = item2;
			}
		}
	}

	public bool SetUpGeometry(bool halo, out double bottomSize, out double midSize, out double topSize, out double xSize, out double ySize)
	{
		BoundingBox boundingBox = GAll.GetBoundingBox(accurate: true);
		if (!boundingBox.IsValid)
		{
			bottomSize = 0.0;
			midSize = 0.0;
			topSize = 0.0;
			xSize = 0.0;
			ySize = 0.0;
			return false;
		}
		BoundingBox boundingBox2 = SidesBB();
		bottomSize = 0.0 - (boundingBox.Min.Z - boundingBox2.Min.Z);
		midSize = boundingBox2.Diagonal.Z;
		topSize = 0.0 - boundingBox2.Max.Z;
		xSize = boundingBox.Diagonal.X;
		ySize = boundingBox.Diagonal.Y;
		Transform transform = Transform.Translation(0.0 - boundingBox.Min.X, halo ? (0.0 - boundingBox.Min.Y) : 0.0, 0.0 - boundingBox.Min.Z);
		Transform t = Transform.Scale(Plane.WorldXY, 1.0 / boundingBox.Diagonal.X, 1.0 / boundingBox.Diagonal.Y, 1.0 / (0.0 - boundingBox.Min.Z)) * transform;
		Trans(t);
		return true;
	}

	public void Trans(Transform t)
	{
		GTop?.Transform(t);
		foreach (GeometryBase item in GSide)
		{
			item.Transform(t);
		}
		GBase?.Transform(t);
		GAll.Transform(t);
		ECrv?.Transform(t);
	}

	private BoundingBox SidesBB()
	{
		BoundingBox unset = BoundingBox.Unset;
		foreach (GeometryBase item in GSide)
		{
			BoundingBox boundingBox = item.GetBoundingBox(accurate: true);
			unset.Union(boundingBox);
		}
		return unset;
	}
}
