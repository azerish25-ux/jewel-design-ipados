using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlocks.Curves;

[CSBlock("RingTrimCurve", "{808AD54C-4FAB-4E7C-A7EE-9E80BFEE842C}")]
public sealed class CSRingTrimCurve : CSBlock
{
	public CSOutputChannel OutCurves { get; }

	public CSInputChannel InCurve { get; }

	public CSInputChannel InAngleStart { get; }

	public CSInputChannel InAngleEnd { get; }

	public CSRingTrimCurve(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(GetPropertyName(() => InCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAngleStart = new CSInputChannel(GetPropertyName(() => InAngleStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngleEnd = new CSInputChannel(GetPropertyName(() => InAngleEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutCurves = new CSOutputChannel(GetPropertyName(() => OutCurves), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve");
		}
		double value = 10.0;
		InAngleStart.GetDouble(ref value);
		double value2 = 45.0;
		InAngleEnd.GetDouble(ref value2);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(i), out var value3);
			if (value3 == null)
			{
				continue;
			}
			BoundingBox boundingBox = ((GeometryBase)value3).GetBoundingBox(true);
			PlaneSurface val = new PlaneSurface(new Plane(Point3d.Origin, Vector3d.XAxis), new Interval(0.0, 1.0), new Interval(0.0, 1.0));
			((GeometryBase)val).Translate(Point3d.Origin - ((Surface)val).PointAt(0.5, 0.0));
			Point3d val2 = ((BoundingBox)(ref boundingBox)).Max;
			double x = ((Point3d)(ref val2)).X;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			double num = Math.Max(x, 0.0 - ((Point3d)(ref val2)).X);
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			double z = ((Point3d)(ref val2)).Z;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			double num2 = num + Math.Max(z, 0.0 - ((Point3d)(ref val2)).Z) + 1.0;
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			double y = ((Point3d)(ref val2)).Y;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			double num3 = Math.Max(y, 0.0 - ((Point3d)(ref val2)).Y) + 1.0;
			Transform val3 = Transform.Scale(Plane.WorldXY, 1.0, num3 * 2.1, num2 * 2.1);
			((GeometryBase)val).Transform(val3);
			PlaneSurface val4 = (PlaneSurface)((GeometryBase)val).Duplicate();
			((GeometryBase)val).Rotate(CSBlockUtil.CSUtilMath.ToRadians(value), Vector3d.YAxis, Point3d.Origin);
			((GeometryBase)val4).Rotate(CSBlockUtil.CSUtilMath.ToRadians(value2), Vector3d.YAxis, Point3d.Origin);
			CurveIntersections val5 = Intersection.CurveSurface((Curve)(object)value3, (Surface)(object)val, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			Interval domain;
			if (val5.Count > 0)
			{
				if (((Curve)value3).IsClosed)
				{
					if (!((Curve)value3).ChangeClosedCurveSeam(val5[0].ParameterA))
					{
						return UpdateError("ChangeClosedCurveSeam failed");
					}
					CurveIntersections val6 = Intersection.CurveSurface((Curve)(object)value3, (Surface)(object)val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					if (val6.Count <= 0)
					{
						return UpdateError("IntersectSurface failed");
					}
					NurbsCurve obj = value3;
					domain = ((Curve)value3).Domain;
					value3 = ((Curve)obj).Trim(new Interval(((Interval)(ref domain)).Min, val6[0].ParameterA)).ToNurbsCurve();
					if (!((CommonObject)value3).IsValid)
					{
						return UpdateError("Trim failed");
					}
				}
				else
				{
					CurveIntersections val7 = Intersection.CurveSurface((Curve)(object)value3, (Surface)(object)val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					if (val7.Count <= 0)
					{
						return UpdateError("Trim failed");
					}
					value3 = ((Curve)value3).Trim(new Interval(val7[0].ParameterA, val7[0].ParameterA)).ToNurbsCurve();
					if (!((CommonObject)value3).IsValid)
					{
						return UpdateError("Trim failed");
					}
				}
			}
			else
			{
				CurveIntersections val8 = Intersection.CurveSurface((Curve)(object)value3, (Surface)(object)val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				if (val8.Count <= 0)
				{
					return UpdateError("IntersectSurface failed");
				}
				NurbsCurve obj2 = value3;
				domain = ((Curve)value3).Domain;
				value3 = ((Curve)obj2).Trim(new Interval(((Interval)(ref domain)).Min, val8[0].ParameterA)).ToNurbsCurve();
				if (!((CommonObject)value3).IsValid)
				{
					return UpdateError("Trim failed");
				}
			}
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)value3));
		}
		if (flattenArray.GetArrayCount() == 1)
		{
			OutCurves.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			OutCurves.SetData(cSDataArray);
		}
		return base.Update();
	}
}
