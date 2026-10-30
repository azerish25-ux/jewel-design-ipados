using System;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSCompatibility;

public static class CSConvert
{
	public static OnBoundingBox BoundingBoxToOnBoundingBox(BoundingBox bb)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		if (bb.Min.X == 0.0 && bb.Min.Y == 0.0 && bb.Min.Z == 0.0 && bb.Max.X == 0.0 && bb.Max.Y == 0.0 && bb.Max.Z == 0.0)
		{
			return new OnBoundingBox();
		}
		return new OnBoundingBox((IOn3dPoint)(object)Point3dCopyToOn3dPoint(bb.Min), (IOn3dPoint)(object)Point3dCopyToOn3dPoint(bb.Max));
	}

	public static OnBoundingBox Convert(this BoundingBox bBox)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		if (bBox.IsValid)
		{
			On3dPoint obj = Point3dCopyToOn3dPoint(bBox.Min);
			On3dPoint val = Point3dCopyToOn3dPoint(bBox.Max);
			return new OnBoundingBox((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val);
		}
		return new OnBoundingBox();
	}

	public static BoundingBox Convert(this OnBoundingBox bBox)
	{
		if (!bBox.IsValid())
		{
			return BoundingBox.Unset;
		}
		Point3d min = On3dPointCopyToPoint3d(bBox.Min());
		Point3d max = On3dPointCopyToPoint3d(bBox.Max());
		return new BoundingBox(min, max);
	}

	public static On3dPoint Convert(this Point3d pt)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (!pt.Equals(Point3d.Unset))
		{
			return new On3dPoint(pt.X, pt.Y, pt.Z);
		}
		return new On3dPoint(OnUtil.On_UNSET_POINT);
	}

	public static Point3d Convert(this IOn3dPoint pt)
	{
		if (pt == null)
		{
			return Point3d.Unset;
		}
		if (pt.Equals(OnUtil.On_UNSET_POINT))
		{
			return Point3d.Unset;
		}
		return new Point3d(pt.x, pt.y, pt.z);
	}

	public static Point3d Convert(this On3dPoint pt)
	{
		if (pt == (IOn3dPoint)null)
		{
			return Point3d.Unset;
		}
		if (pt.Equals(OnUtil.On_UNSET_POINT))
		{
			return Point3d.Unset;
		}
		return new Point3d(pt.x, pt.y, pt.z);
	}

	public static On3dVector Convert(this Vector3d vec)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (!vec.Equals(Vector3d.Unset))
		{
			return new On3dVector(vec.X, vec.Y, vec.Z);
		}
		return new On3dVector(OnUtil.On_UNSET_VECTOR);
	}

	public static Vector3d Convert(this On3dVector vec)
	{
		if (vec == (IOn3dVector)null)
		{
			return Vector3d.Unset;
		}
		if (vec.Equals(OnUtil.On_UNSET_VECTOR))
		{
			return Vector3d.Unset;
		}
		return new Vector3d(vec.x, vec.y, vec.z);
	}

	public static OnXform Convert(this Transform transform)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		if (transform.Equals(Transform.Unset))
		{
			return null;
		}
		OnXform val = new OnXform();
		val.Identity();
		if (transform.Equals(Transform.Identity))
		{
			return val;
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				val[i, j] = transform[i, j];
			}
		}
		return val;
	}

	public static Transform Convert(this IOnXform xform)
	{
		if (xform == null)
		{
			return Transform.Unset;
		}
		Transform identity = Transform.Identity;
		if (xform.IsIdentity())
		{
			return identity;
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				identity[i, j] = xform[i, j];
			}
		}
		return identity;
	}

	public static Transform Convert(this OnXform xform)
	{
		if (xform == null)
		{
			return Transform.Unset;
		}
		Transform identity = Transform.Identity;
		if (xform.IsIdentity())
		{
			return identity;
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				identity[i, j] = xform[i, j];
			}
		}
		return identity;
	}

	public static OnCurve Convert(this Curve crv)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		if (crv == null)
		{
			return null;
		}
		return (OnCurve)Interop.ToOnCurve(crv);
	}

	public static OnNurbsCurve Convert(this NurbsCurve nCrv)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (nCrv == null)
		{
			return null;
		}
		OnCurve val = (OnCurve)Interop.ToOnCurve(nCrv);
		if ((int)val == 0)
		{
			return null;
		}
		return val.NurbsCurve();
	}

	public static Curve Convert(this OnCurve crv)
	{
		if (crv == null)
		{
			return null;
		}
		return Interop.FromOnCurve(crv);
	}

	public static InstanceReferenceGeometry Convert(this OnInstanceRef oInstRef)
	{
		if (oInstRef == null)
		{
			return null;
		}
		InstanceReferenceGeometry instanceReferenceGeometry = new InstanceReferenceGeometry(Guid.Empty, oInstRef.m_xform.Convert());
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		((OnObject)oInstRef).GetUserString("CSName", ref empty);
		((OnObject)oInstRef).GetUserString("CSLayer", ref empty2);
		((OnObject)oInstRef).GetUserString("CSType", ref empty3);
		((OnObject)oInstRef).GetUserString("CSShape", ref empty4);
		instanceReferenceGeometry.SetUserString("CSName", empty);
		instanceReferenceGeometry.SetUserString("CSLayer", empty2);
		instanceReferenceGeometry.SetUserString("CSType", empty3);
		instanceReferenceGeometry.SetUserString("CSShape", empty4);
		return instanceReferenceGeometry;
	}

	public static NurbsCurve Convert(this OnNurbsCurve nCrv)
	{
		if (nCrv == null)
		{
			return null;
		}
		return Interop.FromOnCurve(nCrv).ToNurbsCurve();
	}

	public static OnBrep Convert(this Brep brep)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		if (brep == null)
		{
			return null;
		}
		return (OnBrep)Interop.ToOnBrep(brep);
	}

	public static Brep Convert(this OnBrep brep)
	{
		if (brep == null)
		{
			return null;
		}
		return Interop.FromOnBrep(brep);
	}

	public static OnMesh Convert(this Mesh mesh)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		if (mesh == null)
		{
			return null;
		}
		return (OnMesh)Interop.ToOnMesh(mesh);
	}

	public static Mesh Convert(this OnMesh mesh)
	{
		if (mesh == null)
		{
			return null;
		}
		return Interop.FromOnMesh(mesh);
	}

	public static OnSurface Convert(this Surface srf)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		if (srf == null)
		{
			return null;
		}
		return (OnSurface)Interop.ToOnSurface(srf);
	}

	public static OnNurbsSurface Convert(this NurbsSurface nSrf)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (nSrf == null)
		{
			return null;
		}
		OnSurface val = (OnSurface)Interop.ToOnSurface(nSrf);
		if ((int)val == 0)
		{
			return null;
		}
		return val.NurbsSurface();
	}

	public static Surface Convert(this OnSurface srf)
	{
		if (srf == null)
		{
			return null;
		}
		return Interop.FromOnSurface(srf);
	}

	public static NurbsSurface Convert(this OnNurbsSurface nSrf)
	{
		if (nSrf == null)
		{
			return null;
		}
		return Interop.FromOnSurface(nSrf).ToNurbsSurface();
	}

	public static Point Convert(this OnPoint npnt)
	{
		if (npnt == null)
		{
			return null;
		}
		return new Point(new Point3d(npnt.point.x, npnt.point.y, npnt.point.z));
	}

	public static OnGeometry Convert(this GeometryBase geo)
	{
		return ToOnGeometry(geo);
	}

	public static GeometryBase Convert(this OnObject obj)
	{
		return ToGeometryBase(obj);
	}

	public static OnLine Convert(this Line line)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		if (line.IsValid)
		{
			On3dPoint obj = Point3dCopyToOn3dPoint(line.From);
			On3dPoint val = Point3dCopyToOn3dPoint(line.To);
			return new OnLine((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val);
		}
		return new OnLine();
	}

	public static Line Convert(this OnLine line)
	{
		if (line == null)
		{
			return Line.Unset;
		}
		return new Line(line.from.x, line.from.y, line.from.z, line.to.x, line.to.y, line.to.z);
	}

	public static Point3d On3dPointCopyToPoint3d(On3dPoint value)
	{
		return new Point3d(value.x, value.y, value.z);
	}

	public static Point3d On3dPointCopyToPoint3d(IOn3dPoint value)
	{
		return new Point3d(value.x, value.y, value.z);
	}

	public static Vector3d On3dVectorCopyToVector3d(On3dVector value)
	{
		return new Vector3d(value.x, value.y, value.z);
	}

	public static Vector3d On3dVectorCopyToVector3d(IOn3dVector value)
	{
		return new Vector3d(value.x, value.y, value.z);
	}

	public static BoundingBox OnBoundingBoxToBoundingBox(OnBoundingBox bb)
	{
		if (bb.Min().x != 0.0 || bb.Min().y != 0.0 || bb.Min().z != 0.0 || bb.Max().x != 0.0 || bb.Max().y != 0.0 || bb.Max().z != 0.0)
		{
			return new BoundingBox(On3dPointCopyToPoint3d(bb.Min()), On3dPointCopyToPoint3d(bb.Max()));
		}
		return BoundingBox.Unset;
	}

	public static Polyline OnPolylineToPolyline(OnPolyline pline)
	{
		Polyline polyline = new Polyline(pline.PointCount());
		for (int i = 0; i < pline.PointCount(); i++)
		{
			polyline.Add(((ArrayOn3dPoint)pline)[i].x, ((ArrayOn3dPoint)pline)[i].y, ((ArrayOn3dPoint)pline)[i].z);
		}
		return polyline;
	}

	public static On3dPoint Point3dCopyToOn3dPoint(Point3d value)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		return new On3dPoint(value.X, value.Y, value.Z);
	}

	public static Brep Repair(this Brep brep, double tol)
	{
		OnBrep obj = brep.Convert().Duplicate();
		RhUtil.RhinoRepairBrep(obj, tol);
		return obj.Convert();
	}

	public static OnGeometry ToOnGeometry(GeometryBase gb)
	{
		IntPtr intPtr = Interop.NativeGeometryNonConstPointer(gb);
		if (!(IntPtr.Zero == intPtr))
		{
			return OnGeometry.WrapNativePointer(intPtr, false, false, (object)gb);
		}
		return null;
	}

	public static OnObject ToOnObject(GeometryBase gb)
	{
		IntPtr intPtr = Interop.NativeGeometryNonConstPointer(gb);
		if (!(IntPtr.Zero == intPtr))
		{
			return OnObject.WrapNativePointer(intPtr, false, false, (object)gb);
		}
		return null;
	}

	public static On3dVector Vector3dCopyToOn3dVector(Vector3d value)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		return new On3dVector(value.X, value.Y, value.Z);
	}

	public static Transform XformCopyToTransform(IOnXform value)
	{
		Transform result = default(Transform);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				result[i, j] = value[i, j];
			}
		}
		return result;
	}

	private static GeometryBase ToGeometryBase(OnObject obj)
	{
		if (obj == null)
		{
			return null;
		}
		OnBrep val = OnBrep.Cast(obj);
		if (val != null)
		{
			return val.Convert();
		}
		OnSurface val2 = OnSurface.Cast(obj);
		if (val2 != null)
		{
			return val2.Convert();
		}
		OnMesh val3 = OnMesh.Cast(obj);
		if (val3 != null)
		{
			return val3.Convert();
		}
		OnCurve val4 = OnCurve.Cast(obj);
		if (val4 != null)
		{
			return val4.Convert();
		}
		OnPoint val5 = OnPoint.Cast(obj);
		if (val5 != null)
		{
			return val5.Convert();
		}
		return OnInstanceRef.Cast(obj)?.Convert();
	}
}
