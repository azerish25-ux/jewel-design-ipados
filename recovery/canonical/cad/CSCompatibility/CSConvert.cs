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
		if (bb.Min.X != 0.0 || bb.Min.Y != 0.0 || bb.Min.Z != 0.0 || bb.Max.X != 0.0 || bb.Max.Y != 0.0 || bb.Max.Z != 0.0)
		{
			return new OnBoundingBox(Point3dCopyToOn3dPoint(bb.Min), Point3dCopyToOn3dPoint(bb.Max));
		}
		return new OnBoundingBox();
	}

	public static OnBoundingBox Convert(this BoundingBox bBox)
	{
		if (!bBox.IsValid)
		{
			return new OnBoundingBox();
		}
		On3dPoint min = Point3dCopyToOn3dPoint(bBox.Min);
		On3dPoint max = Point3dCopyToOn3dPoint(bBox.Max);
		return new OnBoundingBox(min, max);
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
		if (pt.Equals(Point3d.Unset))
		{
			return new On3dPoint(OnUtil.On_UNSET_POINT);
		}
		return new On3dPoint(pt.X, pt.Y, pt.Z);
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

	public static On3dVector Convert(this Vector3d vec)
	{
		if (vec.Equals(Vector3d.Unset))
		{
			return new On3dVector(OnUtil.On_UNSET_VECTOR);
		}
		return new On3dVector(vec.X, vec.Y, vec.Z);
	}

	public static Vector3d Convert(this On3dVector vec)
	{
		if (vec == null)
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
		if (transform.Equals(Transform.Unset))
		{
			return null;
		}
		OnXform onXform = new OnXform();
		onXform.Identity();
		if (transform.Equals(Transform.Identity))
		{
			return onXform;
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				onXform[i, j] = transform[i, j];
			}
		}
		return onXform;
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
		if (crv == null)
		{
			return null;
		}
		return (OnCurve)Interop.ToOnCurve(crv);
	}

	public static OnNurbsCurve Convert(this NurbsCurve nCrv)
	{
		if (nCrv == null)
		{
			return null;
		}
		return ((OnCurve)Interop.ToOnCurve(nCrv))?.NurbsCurve();
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
		string string_value = string.Empty;
		string string_value2 = string.Empty;
		string string_value3 = string.Empty;
		string string_value4 = string.Empty;
		oInstRef.GetUserString("CSName", ref string_value);
		oInstRef.GetUserString("CSLayer", ref string_value2);
		oInstRef.GetUserString("CSType", ref string_value3);
		oInstRef.GetUserString("CSShape", ref string_value4);
		instanceReferenceGeometry.SetUserString("CSName", string_value);
		instanceReferenceGeometry.SetUserString("CSLayer", string_value2);
		instanceReferenceGeometry.SetUserString("CSType", string_value3);
		instanceReferenceGeometry.SetUserString("CSShape", string_value4);
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
		if (srf == null)
		{
			return null;
		}
		return (OnSurface)Interop.ToOnSurface(srf);
	}

	public static OnNurbsSurface Convert(this NurbsSurface nSrf)
	{
		if (nSrf == null)
		{
			return null;
		}
		return ((OnSurface)Interop.ToOnSurface(nSrf))?.NurbsSurface();
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
		if (!line.IsValid)
		{
			return new OnLine();
		}
		On3dPoint start = Point3dCopyToOn3dPoint(line.From);
		On3dPoint end = Point3dCopyToOn3dPoint(line.To);
		return new OnLine(start, end);
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
			polyline.Add(pline[i].x, pline[i].y, pline[i].z);
		}
		return polyline;
	}

	public static On3dPoint Point3dCopyToOn3dPoint(Point3d value)
	{
		return new On3dPoint(value.X, value.Y, value.Z);
	}

	public static Brep Repair(this Brep brep, double tol)
	{
		OnBrep onBrep = brep.Convert().Duplicate();
		RhUtil.RhinoRepairBrep(onBrep, tol);
		return onBrep.Convert();
	}

	public static OnGeometry ToOnGeometry(GeometryBase gb)
	{
		IntPtr intPtr = Interop.NativeGeometryNonConstPointer(gb);
		if (!(IntPtr.Zero == intPtr))
		{
			return OnGeometry.WrapNativePointer(intPtr, isConst: false, doDelete: false, gb);
		}
		return null;
	}

	public static OnObject ToOnObject(GeometryBase gb)
	{
		IntPtr intPtr = Interop.NativeGeometryNonConstPointer(gb);
		if (!(IntPtr.Zero == intPtr))
		{
			return OnObject.WrapNativePointer(intPtr, isConst: false, doDelete: false, gb);
		}
		return null;
	}

	public static On3dVector Vector3dCopyToOn3dVector(Vector3d value)
	{
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
		OnBrep onBrep = OnBrep.Cast(obj);
		if (onBrep != null)
		{
			return onBrep.Convert();
		}
		OnSurface onSurface = OnSurface.Cast(obj);
		if (onSurface != null)
		{
			return onSurface.Convert();
		}
		OnMesh onMesh = OnMesh.Cast(obj);
		if (onMesh != null)
		{
			return onMesh.Convert();
		}
		OnCurve onCurve = OnCurve.Cast(obj);
		if (onCurve != null)
		{
			return onCurve.Convert();
		}
		OnPoint onPoint = OnPoint.Cast(obj);
		if (onPoint != null)
		{
			return onPoint.Convert();
		}
		return OnInstanceRef.Cast(obj)?.Convert();
	}
}
