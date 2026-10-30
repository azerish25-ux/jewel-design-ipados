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
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		GTop = null;
		GSide = new List<GeometryBase>();
		GBase = null;
		ECrv = null;
		GeometryBase val = null;
		List<GeometryBase> list = new List<GeometryBase>();
		GeometryBase val2 = null;
		foreach (GeometryBase inObject in inObjects)
		{
			string obj = inObject.GetUserString("CSName")?.ToLower();
			if (obj == part + "top")
			{
				GTop = inObject;
			}
			if (obj == part + "topcam")
			{
				val = inObject;
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
				val2 = inObject;
			}
		}
		if (isCam)
		{
			if (val != null)
			{
				GTop = val;
			}
			if (list.Count != 0)
			{
				GSide = list;
			}
			if (val2 != null)
			{
				GBase = val2;
			}
		}
		if (isMesh)
		{
			Mesh val3 = new Mesh();
			GeometryBase gTop = GTop;
			val3.Append((Mesh)(object)((gTop is Mesh) ? gTop : null));
			GeometryBase gBase = GBase;
			val3.Append((Mesh)(object)((gBase is Mesh) ? gBase : null));
			foreach (Mesh item in GSide.Select((GeometryBase s) => (Mesh)(object)((s is Mesh) ? s : null)))
			{
				val3.Append(item);
			}
			GAll = (GeometryBase)(object)val3;
		}
		else
		{
			List<Brep> list2 = new List<Brep>();
			if (GTop != null)
			{
				GeometryBase gTop2 = GTop;
				list2.Add((Brep)(object)((gTop2 is Brep) ? gTop2 : null));
			}
			if (GBase != null)
			{
				GeometryBase gBase2 = GBase;
				list2.Add((Brep)(object)((gBase2 is Brep) ? gBase2 : null));
			}
			list2.AddRange(GSide.Select((GeometryBase s) => (Brep)(object)((s is Brep) ? s : null)));
			Brep[] array = Brep.JoinBreps((IEnumerable<Brep>)list2, 0.001);
			if (array != null && array.Length == 1)
			{
				GAll = (GeometryBase)(object)array[0];
			}
		}
		if (crv == null)
		{
			return;
		}
		foreach (Curve item2 in crv)
		{
			if (((GeometryBase)item2).GetUserString("CSName").ToLower() == part + "crv")
			{
				ECrv = item2;
			}
		}
	}

	public bool SetUpGeometry(bool halo, out double bottomSize, out double midSize, out double topSize, out double xSize, out double ySize)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox boundingBox = GAll.GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox)).IsValid)
		{
			bottomSize = 0.0;
			midSize = 0.0;
			topSize = 0.0;
			xSize = 0.0;
			ySize = 0.0;
			return false;
		}
		BoundingBox val = SidesBB();
		Point3d val2 = ((BoundingBox)(ref boundingBox)).Min;
		double z = ((Point3d)(ref val2)).Z;
		val2 = ((BoundingBox)(ref val)).Min;
		bottomSize = 0.0 - (z - ((Point3d)(ref val2)).Z);
		Vector3d diagonal = ((BoundingBox)(ref val)).Diagonal;
		midSize = ((Vector3d)(ref diagonal)).Z;
		val2 = ((BoundingBox)(ref val)).Max;
		topSize = 0.0 - ((Point3d)(ref val2)).Z;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		xSize = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		ySize = ((Vector3d)(ref diagonal)).Y;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		double num = 0.0 - ((Point3d)(ref val2)).X;
		double num2;
		if (!halo)
		{
			num2 = 0.0;
		}
		else
		{
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			num2 = 0.0 - ((Point3d)(ref val2)).Y;
		}
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		Transform val3 = Transform.Translation(num, num2, 0.0 - ((Point3d)(ref val2)).Z);
		Plane worldXY = Plane.WorldXY;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num3 = 1.0 / ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num4 = 1.0 / ((Vector3d)(ref diagonal)).Y;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		Transform t = Transform.Scale(worldXY, num3, num4, 1.0 / (0.0 - ((Point3d)(ref val2)).Z)) * val3;
		Trans(t);
		return true;
	}

	public void Trans(Transform t)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		GeometryBase gTop = GTop;
		if (gTop != null)
		{
			gTop.Transform(t);
		}
		foreach (GeometryBase item in GSide)
		{
			item.Transform(t);
		}
		GeometryBase gBase = GBase;
		if (gBase != null)
		{
			gBase.Transform(t);
		}
		GAll.Transform(t);
		Curve eCrv = ECrv;
		if (eCrv != null)
		{
			((GeometryBase)eCrv).Transform(t);
		}
	}

	private BoundingBox SidesBB()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox unset = BoundingBox.Unset;
		foreach (GeometryBase item in GSide)
		{
			BoundingBox boundingBox = item.GetBoundingBox(true);
			((BoundingBox)(ref unset)).Union(boundingBox);
		}
		return unset;
	}
}
