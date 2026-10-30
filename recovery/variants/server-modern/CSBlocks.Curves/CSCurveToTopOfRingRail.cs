using System.Collections.Generic;
using System.Linq;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Curves;

[CSBlock("CurveToTopOfRingRail", "{5A5F445D-EC18-4A7E-A68A-DB5C4C5E81EE}")]
public sealed class CSCurveToTopOfRingRail : CSBlock
{
	public readonly CSInputChannel iRingRail;

	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iAngle;

	public readonly CSInputChannel iFixed;

	public readonly CSInputChannel iLockXYScale;

	public readonly CSOutputChannel oCurve;

	public CSCurveToTopOfRingRail(string identifier)
		: base(identifier)
	{
		iRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFixed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iLockXYScale = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0178: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_02dc: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Expected O, but got Unknown
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iRingRail.GetFlattenArray();
		CSDataArray flattenArray2 = iCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("RingRail is missing");
		}
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve is missing");
		}
		CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out var value);
		CSDataHelper.GetNurbsCurve(flattenArray2.GetArrayItem(0), out var value2);
		if (value == null)
		{
			return UpdateMissing("RingRail is missing");
		}
		if (value2 == null)
		{
			return UpdateMissing("Curve is missing");
		}
		double value3 = 45.0;
		iAngle.GetDouble(ref value3);
		bool value4 = false;
		iFixed.GetBoolean(ref value4);
		bool value5 = false;
		iLockXYScale.GetBoolean(ref value5);
		Plane val = default(Plane);
		if (!((Curve)value).TryGetPlane(ref val))
		{
			return UpdateMissing("RingRail is not planar");
		}
		Plane val2 = default(Plane);
		if (!((Curve)value2).TryGetPlane(ref val2))
		{
			return UpdateMissing("Curve is not planar");
		}
		LineCurve val3 = new LineCurve(new Point3d(0.0, -100.0, 0.0), new Point3d(100.0, -100.0, 0.0));
		LineCurve val4 = new LineCurve(new Point3d(0.0, 100.0, 0.0), new Point3d(100.0, 100.0, 0.0));
		NurbsSurface val5 = NurbsSurface.CreateRuledSurface((Curve)val3, (Curve)(object)val4);
		((GeometryBase)val5).Rotate(CSBlockUtil.CSUtilMath.ToRadians(0.0 - value3), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		CurveIntersections val6 = Intersection.CurveSurface((Curve)(object)value, (Surface)(object)val5, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val6.Count != 1)
		{
			return UpdateError("RingRail Intersect Failed");
		}
		double parameterA = val6[0].ParameterA;
		Point3d pointA = val6[0].PointA;
		NurbsCurve val7 = ((Curve)value).ToNurbsCurve();
		NurbsCurve obj = val7;
		Interval domain = ((Curve)val7).Domain;
		val7 = ((Curve)obj).Trim(new Interval(((Interval)(ref domain)).Min, parameterA)).ToNurbsCurve();
		LineCurve val8 = new LineCurve(new Point3d(0.0, -100.0, 0.0), new Point3d(-100.0, -100.0, 0.0));
		val4 = new LineCurve(new Point3d(0.0, 100.0, 0.0), new Point3d(-100.0, 100.0, 0.0));
		val5 = NurbsSurface.CreateRuledSurface((Curve)val8, (Curve)(object)val4);
		((GeometryBase)val5).Rotate(CSBlockUtil.CSUtilMath.ToRadians(value3), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		val6 = Intersection.CurveSurface((Curve)(object)value, (Surface)(object)val5, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (((IEnumerable<IntersectionEvent>)val6).Count() != 1)
		{
			return UpdateError("RingRail Intersect Failed");
		}
		double parameterA2 = val6[0].ParameterA;
		Point3d pointA2 = val6[0].PointA;
		NurbsCurve val9 = ((Curve)value).ToNurbsCurve();
		NurbsCurve obj2 = value;
		domain = ((Curve)val9).Domain;
		val9 = ((Curve)obj2).Trim(new Interval(parameterA2, ((Interval)(ref domain)).Max)).ToNurbsCurve();
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve>
		{
			(Curve)(object)val9,
			(Curve)(object)val7
		}.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (array == null)
		{
			return UpdateError("Merge Curve Failed");
		}
		if (array.Length != 1)
		{
			return UpdateError("Error Merging RingRail Parts");
		}
		value = array[0].ToNurbsCurve();
		BoundingBox boundingBox = ((GeometryBase)value2).GetBoundingBox(true);
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double y = ((Vector3d)(ref diagonal)).Y;
		Vector3d val10 = default(Vector3d);
		((Vector3d)(ref val10))._002Ector(((Plane)(ref val)).Normal);
		NurbsCurve val11 = new NurbsCurve(value);
		NurbsCurve val12 = new NurbsCurve(value);
		((GeometryBase)val11).Translate(val10 * y);
		((GeometryBase)val12).Translate(val10 * (0.0 - y));
		NurbsSurface obj3 = NurbsSurface.CreateRuledSurface((Curve)val11, (Curve)(object)val12);
		NurbsCurve val13 = new NurbsCurve(value2);
		double x = ((Point3d)(ref pointA)).X;
		Point3d val14 = ((BoundingBox)(ref boundingBox)).Max;
		double num = x / ((Point3d)(ref val14)).X;
		double x2 = ((Point3d)(ref pointA2)).X;
		val14 = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = x2 / ((Point3d)(ref val14)).X;
		double num3 = ((num > num2) ? num : num2);
		if (value4 || num3 < 1.0)
		{
			Transform val15 = Transform.Scale(Plane.WorldXY, num3, value5 ? num3 : 1.0, 1.0);
			((GeometryBase)val13).Transform(val15);
		}
		double num4 = (((Point3d)(ref pointA)).Z + ((Point3d)(ref pointA2)).Z) / 2.0;
		val14 = ((BoundingBox)(ref boundingBox)).Center;
		((GeometryBase)val13).Translate(new Vector3d(0.0, 0.0, num4 - ((Point3d)(ref val14)).Z));
		NurbsSurface val16 = NurbsSurface.CreateRuledSurface((Curve)(object)value2, (Curve)(object)val13);
		Curve[] array2 = default(Curve[]);
		Point3d[] array3 = default(Point3d[]);
		if (!Intersection.SurfaceSurface((Surface)(object)obj3, (Surface)(object)val16, CSApplicationHelper.Application.Tolerance, ref array2, ref array3))
		{
			return UpdateError("RhinoIntersectSurfaces failed");
		}
		Curve[] array4 = Curve.JoinCurves((IEnumerable<Curve>)array2);
		if (array4.Length < 1)
		{
			return UpdateError("RhinoMergeCurves failed");
		}
		base.OutputChannels.Clear();
		for (int i = 0; i < array4.Length; i++)
		{
			Curve val17 = array4[i];
			if (val17 == null)
			{
				continue;
			}
			NurbsCurve val18 = val17.ToNurbsCurve();
			if (val18 != null)
			{
				domain = ((Curve)val18).Domain;
				Vector3d val19 = ((Curve)val18).TangentAt(((Interval)(ref domain)).Min);
				if (((Vector3d)(ref val19)).X < 0.0)
				{
					((Curve)val18).Reverse();
				}
				new CSOutputChannel($"{base.Identifier}_{i}", this, ChannelDataType.Curve, ChannelArrayType.Individual).SetData(new CSOnGeometryData((GeometryBase)(object)val18));
			}
		}
		return base.Update();
	}
}
