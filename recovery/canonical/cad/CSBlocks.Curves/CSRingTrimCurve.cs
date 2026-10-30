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
			BoundingBox boundingBox = value3.GetBoundingBox(accurate: true);
			PlaneSurface planeSurface = new PlaneSurface(new Plane(Point3d.Origin, Vector3d.XAxis), new Interval(0.0, 1.0), new Interval(0.0, 1.0));
			planeSurface.Translate(Point3d.Origin - planeSurface.PointAt(0.5, 0.0));
			double num = Math.Max(boundingBox.Max.X, 0.0 - boundingBox.Min.X) + Math.Max(boundingBox.Max.Z, 0.0 - boundingBox.Min.Z) + 1.0;
			double num2 = Math.Max(boundingBox.Max.Y, 0.0 - boundingBox.Min.Y) + 1.0;
			Transform xform = Transform.Scale(Plane.WorldXY, 1.0, num2 * 2.1, num * 2.1);
			planeSurface.Transform(xform);
			PlaneSurface planeSurface2 = (PlaneSurface)planeSurface.Duplicate();
			planeSurface.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value), Vector3d.YAxis, Point3d.Origin);
			planeSurface2.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value2), Vector3d.YAxis, Point3d.Origin);
			CurveIntersections curveIntersections = Intersection.CurveSurface(value3, planeSurface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count > 0)
			{
				if (value3.IsClosed)
				{
					if (!value3.ChangeClosedCurveSeam(curveIntersections[0].ParameterA))
					{
						return UpdateError("ChangeClosedCurveSeam failed");
					}
					CurveIntersections curveIntersections2 = Intersection.CurveSurface(value3, planeSurface2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					if (curveIntersections2.Count <= 0)
					{
						return UpdateError("IntersectSurface failed");
					}
					value3 = value3.Trim(new Interval(value3.Domain.Min, curveIntersections2[0].ParameterA)).ToNurbsCurve();
					if (!value3.IsValid)
					{
						return UpdateError("Trim failed");
					}
				}
				else
				{
					CurveIntersections curveIntersections3 = Intersection.CurveSurface(value3, planeSurface2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					if (curveIntersections3.Count <= 0)
					{
						return UpdateError("Trim failed");
					}
					value3 = value3.Trim(new Interval(curveIntersections3[0].ParameterA, curveIntersections3[0].ParameterA)).ToNurbsCurve();
					if (!value3.IsValid)
					{
						return UpdateError("Trim failed");
					}
				}
			}
			else
			{
				CurveIntersections curveIntersections4 = Intersection.CurveSurface(value3, planeSurface2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				if (curveIntersections4.Count <= 0)
				{
					return UpdateError("IntersectSurface failed");
				}
				value3 = value3.Trim(new Interval(value3.Domain.Min, curveIntersections4[0].ParameterA)).ToNurbsCurve();
				if (!value3.IsValid)
				{
					return UpdateError("Trim failed");
				}
			}
			cSDataArray.Append(new CSOnGeometryData(value3));
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
