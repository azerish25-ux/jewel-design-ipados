using System;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSCompatibility;

public static class CSConvert
{
	public static OnBoundingBox BoundingBoxToOnBoundingBox(BoundingBox bb)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		Point3d val = ((BoundingBox)(ref bb)).Min;
		if (((Point3d)(ref val)).X == 0.0)
		{
			val = ((BoundingBox)(ref bb)).Min;
			if (((Point3d)(ref val)).Y == 0.0)
			{
				val = ((BoundingBox)(ref bb)).Min;
				if (((Point3d)(ref val)).Z == 0.0)
				{
					val = ((BoundingBox)(ref bb)).Max;
					if (((Point3d)(ref val)).X == 0.0)
					{
						val = ((BoundingBox)(ref bb)).Max;
						if (((Point3d)(ref val)).Y == 0.0)
						{
							val = ((BoundingBox)(ref bb)).Max;
							if (((Point3d)(ref val)).Z == 0.0)
							{
								return new OnBoundingBox();
							}
						}
					}
				}
			}
		}
		return new OnBoundingBox((IOn3dPoint)(object)Point3dCopyToOn3dPoint(((BoundingBox)(ref bb)).Min), (IOn3dPoint)(object)Point3dCopyToOn3dPoint(((BoundingBox)(ref bb)).Max));
	}

	public static OnBoundingBox Convert(this BoundingBox bBox)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		if (((BoundingBox)(ref bBox)).IsValid)
		{
			On3dPoint obj = Point3dCopyToOn3dPoint(((BoundingBox)(ref bBox)).Min);
			On3dPoint val = Point3dCopyToOn3dPoint(((BoundingBox)(ref bBox)).Max);
			return new OnBoundingBox((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val);
		}
		return new OnBoundingBox();
	}

	public static BoundingBox Convert(this OnBoundingBox bBox)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (!bBox.IsValid())
		{
			return BoundingBox.Unset;
		}
		Point3d val = On3dPointCopyToPoint3d(bBox.Min());
		Point3d val2 = On3dPointCopyToPoint3d(bBox.Max());
		return new BoundingBox(val, val2);
	}

	public static On3dPoint Convert(this Point3d pt)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (!((Point3d)(ref pt)).Equals(Point3d.Unset))
		{
			return new On3dPoint(((Point3d)(ref pt)).X, ((Point3d)(ref pt)).Y, ((Point3d)(ref pt)).Z);
		}
		return new On3dPoint(OnUtil.On_UNSET_POINT);
	}

	public static Point3d Convert(this IOn3dPoint pt)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (!((Vector3d)(ref vec)).Equals(Vector3d.Unset))
		{
			return new On3dVector(((Vector3d)(ref vec)).X, ((Vector3d)(ref vec)).Y, ((Vector3d)(ref vec)).Z);
		}
		return new On3dVector(OnUtil.On_UNSET_VECTOR);
	}

	public static Vector3d Convert(this On3dVector vec)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (((Transform)(ref transform)).Equals(Transform.Unset))
		{
			return null;
		}
		OnXform val = new OnXform();
		val.Identity();
		if (((Transform)(ref transform)).Equals(Transform.Identity))
		{
			return val;
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				val[i, j] = ((Transform)(ref transform))[i, j];
			}
		}
		return val;
	}

	public static Transform Convert(this IOnXform xform)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
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
				((Transform)(ref identity))[i, j] = xform[i, j];
			}
		}
		return identity;
	}

	public static Transform Convert(this OnXform xform)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
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
				((Transform)(ref identity))[i, j] = xform[i, j];
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
		OnCurve val = (OnCurve)Interop.ToOnCurve((Curve)(object)nCrv);
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
		return Interop.FromOnCurve((object)crv);
	}

	public static InstanceReferenceGeometry Convert(this OnInstanceRef oInstRef)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		if (oInstRef == null)
		{
			return null;
		}
		InstanceReferenceGeometry val = new InstanceReferenceGeometry(Guid.Empty, oInstRef.m_xform.Convert());
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		((OnObject)oInstRef).GetUserString("CSName", ref empty);
		((OnObject)oInstRef).GetUserString("CSLayer", ref empty2);
		((OnObject)oInstRef).GetUserString("CSType", ref empty3);
		((OnObject)oInstRef).GetUserString("CSShape", ref empty4);
		((GeometryBase)val).SetUserString("CSName", empty);
		((GeometryBase)val).SetUserString("CSLayer", empty2);
		((GeometryBase)val).SetUserString("CSType", empty3);
		((GeometryBase)val).SetUserString("CSShape", empty4);
		return val;
	}

	public static NurbsCurve Convert(this OnNurbsCurve nCrv)
	{
		if (nCrv == null)
		{
			return null;
		}
		return Interop.FromOnCurve((object)nCrv).ToNurbsCurve();
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
		return Interop.FromOnBrep((object)brep);
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
		return Interop.FromOnMesh((object)mesh);
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
		OnSurface val = (OnSurface)Interop.ToOnSurface((Surface)(object)nSrf);
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
		return Interop.FromOnSurface((object)srf);
	}

	public static NurbsSurface Convert(this OnNurbsSurface nSrf)
	{
		if (nSrf == null)
		{
			return null;
		}
		return Interop.FromOnSurface((object)nSrf).ToNurbsSurface();
	}

	public static Point Convert(this OnPoint npnt)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		if (((Line)(ref line)).IsValid)
		{
			On3dPoint obj = Point3dCopyToOn3dPoint(((Line)(ref line)).From);
			On3dPoint val = Point3dCopyToOn3dPoint(((Line)(ref line)).To);
			return new OnLine((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val);
		}
		return new OnLine();
	}

	public static Line Convert(this OnLine line)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		if (line == null)
		{
			return Line.Unset;
		}
		return new Line(line.from.x, line.from.y, line.from.z, line.to.x, line.to.y, line.to.z);
	}

	public static Point3d On3dPointCopyToPoint3d(On3dPoint value)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Point3d(value.x, value.y, value.z);
	}

	public static Point3d On3dPointCopyToPoint3d(IOn3dPoint value)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Point3d(value.x, value.y, value.z);
	}

	public static Vector3d On3dVectorCopyToVector3d(On3dVector value)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3d(value.x, value.y, value.z);
	}

	public static Vector3d On3dVectorCopyToVector3d(IOn3dVector value)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3d(value.x, value.y, value.z);
	}

	public static BoundingBox OnBoundingBoxToBoundingBox(OnBoundingBox bb)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		if (bb.Min().x != 0.0 || bb.Min().y != 0.0 || bb.Min().z != 0.0 || bb.Max().x != 0.0 || bb.Max().y != 0.0 || bb.Max().z != 0.0)
		{
			return new BoundingBox(On3dPointCopyToPoint3d(bb.Min()), On3dPointCopyToPoint3d(bb.Max()));
		}
		return BoundingBox.Unset;
	}

	public static Polyline OnPolylineToPolyline(OnPolyline pline)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Polyline val = new Polyline(pline.PointCount());
		for (int i = 0; i < pline.PointCount(); i++)
		{
			((Point3dList)val).Add(((ArrayOn3dPoint)pline)[i].x, ((ArrayOn3dPoint)pline)[i].y, ((ArrayOn3dPoint)pline)[i].z);
		}
		return val;
	}

	public static On3dPoint Point3dCopyToOn3dPoint(Point3d value)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		return new On3dPoint(((Point3d)(ref value)).X, ((Point3d)(ref value)).Y, ((Point3d)(ref value)).Z);
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
		return new On3dVector(((Vector3d)(ref value)).X, ((Vector3d)(ref value)).Y, ((Vector3d)(ref value)).Z);
	}

	public static Transform XformCopyToTransform(IOnXform value)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Transform result = default(Transform);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				((Transform)(ref result))[i, j] = value[i, j];
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
			return (GeometryBase)(object)val.Convert();
		}
		OnSurface val2 = OnSurface.Cast(obj);
		if (val2 != null)
		{
			return (GeometryBase)(object)val2.Convert();
		}
		OnMesh val3 = OnMesh.Cast(obj);
		if (val3 != null)
		{
			return (GeometryBase)(object)val3.Convert();
		}
		OnCurve val4 = OnCurve.Cast(obj);
		if (val4 != null)
		{
			return (GeometryBase)(object)val4.Convert();
		}
		OnPoint val5 = OnPoint.Cast(obj);
		if (val5 != null)
		{
			return (GeometryBase)(object)val5.Convert();
		}
		return (GeometryBase)(object)OnInstanceRef.Cast(obj)?.Convert();
	}
}
