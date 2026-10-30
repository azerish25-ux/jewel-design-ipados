using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Profiles;

[CSBlock("NetworkSurfaceSignetProfilePlacer", "{A68013C9-0693-452c-8872-2AF99AB43B34}")]
public sealed class CSNetworkSurfaceSignetProfilePlacer : CSBlock
{
	public readonly CSInputChannel iRingRail;

	public readonly CSInputChannel iBottomCurve;

	public readonly CSInputChannel iTopCurve;

	public readonly CSInputChannel iPosition1;

	public readonly CSInputChannel iPosition2;

	public readonly CSInputChannel iPosition3;

	public readonly CSInputChannel iSidePosition1;

	public readonly CSInputChannel iSidePosition2;

	public readonly CSInputChannel iSidePosition3;

	public readonly CSInputChannel iTopThickness;

	public readonly CSInputChannel iTopWidth;

	public readonly CSInputChannel iTopLength;

	public readonly CSInputChannel iBottomThickness;

	public readonly CSInputChannel iBottomWidth;

	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iZFollowGem;

	public readonly CSOutputChannel oCurves;

	public readonly CSOutputChannel oTop;

	public CSNetworkSurfaceSignetProfilePlacer(string identifier)
		: base(identifier)
	{
		iRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iBottomCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iTopCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iPosition1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPosition2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPosition3 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSidePosition1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSidePosition2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSidePosition3 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBottomThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBottomWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iZFollowGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		oTop = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iRingRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing ring rail");
		}
		BoundingBox boundingBox = value.GetBoundingBox(accurate: true);
		CSDataHelper.GetNurbsCurve(iTopCurve.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Missing top curve");
		}
		CSDataHelper.GetNurbsCurve(iBottomCurve.Data, out var value3);
		if (value3 == null)
		{
			return UpdateMissing("Missing bottom curve");
		}
		double num = 0.0;
		double num2 = 0.0;
		CSData data = iGem.Data;
		BoundingBox boundingBox2 = BoundingBox.Unset;
		double num3 = 0.0;
		if (data != null)
		{
			if (data.GetTightBoundingBox(ref boundingBox2))
			{
				num = Math.Max(0.0 - boundingBox2.Min.X, boundingBox2.Max.X);
				num2 = Math.Max(0.0 - boundingBox2.Min.Y, boundingBox2.Max.Y);
			}
			CSData arrayItem = CSDataHelper.GetFlattenArray(data).GetArrayItem(0);
			if (arrayItem != null)
			{
				CSDataHelper.GetTransformNoArray(arrayItem, out var value4);
				if (value4.IsValid)
				{
					Point3d point3d = new Point3d(Point3d.Origin);
					point3d.Transform(value4);
					num3 = point3d.Z;
				}
			}
		}
		BoundingBox boundingBox3 = value2.GetBoundingBox(accurate: true);
		BoundingBox boundingBox4 = value3.GetBoundingBox(accurate: true);
		double value5 = 0.0;
		iTopWidth.GetDouble(ref value5);
		double xScaleFactor = ((!(num < 0.001)) ? ((value5 + num) / boundingBox3.Max.X) : (value5 / 2.0 / boundingBox3.Max.X));
		double value6 = 0.0;
		iBottomThickness.GetDouble(ref value6);
		double xScaleFactor2 = (boundingBox.Max.X + value6) / boundingBox4.Max.X;
		double value7 = 0.0;
		iTopLength.GetDouble(ref value7);
		double yScaleFactor = ((!(num2 < 0.001)) ? ((value7 + num2) / boundingBox3.Max.Y) : (value7 / (boundingBox3.Max.Y * 2.0)));
		double value8 = 0.0;
		iBottomWidth.GetDouble(ref value8);
		double yScaleFactor2 = value8 / (boundingBox4.Max.Y * 2.0);
		double value9 = 1.0;
		iTopThickness.GetDouble(ref value9);
		Transform xform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, 0.0);
		Transform xform2 = Transform.Scale(Plane.WorldXY, xScaleFactor2, yScaleFactor2, 0.0);
		value3.Transform(xform2);
		value2.Transform(xform);
		bool value10 = false;
		if (iZFollowGem != null)
		{
			iZFollowGem.GetBoolean(ref value10);
		}
		if (num3 > 0.01 && value10)
		{
			value2.Translate(new Vector3d(0.0, 0.0, value9 + num3));
		}
		else
		{
			value2.Translate(new Vector3d(0.0, 0.0, boundingBox.Max.Z + value9));
		}
		LineCurve lineCurve = new LineCurve(Point3d.Origin, new Point3d(0.0, 40.0, 0.0));
		LineCurve lineCurve2 = new LineCurve(Point3d.Origin, new Point3d(0.0, -40.0, 0.0));
		LineCurve lineCurve3 = new LineCurve(Point3d.Origin, new Point3d(-40.0, 0.0, 0.0));
		LineCurve lineCurve4 = new LineCurve(Point3d.Origin, new Point3d(40.0, 0.0, 0.0));
		CurveIntersections curveIntersections = Intersection.CurveCurve(value3, lineCurve, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections == null)
		{
			return UpdateError("IntersectCurve failed 1");
		}
		Point3d end = new Point3d(curveIntersections[0].PointA);
		curveIntersections.Dispose();
		curveIntersections = Intersection.CurveCurve(value3, lineCurve2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections == null)
		{
			return UpdateError("IntersectCurve failed 2");
		}
		Point3d end2 = new Point3d(curveIntersections[0].PointA);
		curveIntersections.Dispose();
		if (curveIntersections == null)
		{
			return UpdateError("IntersectCurve failed 3");
		}
		curveIntersections = Intersection.CurveCurve(value3, lineCurve4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		Point3d end3 = new Point3d(curveIntersections[0].PointA);
		curveIntersections.Dispose();
		curveIntersections = Intersection.CurveCurve(value3, lineCurve3, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections == null)
		{
			return UpdateError("IntersectCurve failed 4");
		}
		Point3d end4 = new Point3d(curveIntersections[0].PointA);
		curveIntersections.Dispose();
		lineCurve.Translate(new Vector3d(0.0, 0.0, value2.GetBoundingBox(accurate: true).Center.Z));
		lineCurve2.Translate(new Vector3d(0.0, 0.0, value2.GetBoundingBox(accurate: true).Center.Z));
		lineCurve3.Translate(new Vector3d(0.0, 0.0, value2.GetBoundingBox(accurate: true).Center.Z));
		lineCurve4.Translate(new Vector3d(0.0, 0.0, value2.GetBoundingBox(accurate: true).Center.Z));
		curveIntersections = Intersection.CurveCurve(value2, lineCurve, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections == null)
		{
			return UpdateError("IntersectCurve failed 5");
		}
		Point3d start = new Point3d(curveIntersections[0].PointA);
		curveIntersections.Dispose();
		curveIntersections = Intersection.CurveCurve(value2, lineCurve2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections == null)
		{
			return UpdateError("IntersectCurve failed 6");
		}
		Point3d start2 = new Point3d(curveIntersections[0].PointA);
		curveIntersections.Dispose();
		curveIntersections = Intersection.CurveCurve(value2, lineCurve4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections == null)
		{
			return UpdateError("IntersectCurve failed 7");
		}
		Point3d start3 = new Point3d(curveIntersections[0].PointA);
		curveIntersections.Dispose();
		curveIntersections = Intersection.CurveCurve(value2, lineCurve3, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections == null)
		{
			return UpdateError("IntersectCurve failed 8");
		}
		Point3d start4 = new Point3d(curveIntersections[0].PointA);
		curveIntersections.Dispose();
		double value11 = double.NaN;
		if (iPosition1 != null)
		{
			iPosition1.GetDouble(ref value11);
		}
		double value12 = double.NaN;
		if (iPosition2 != null)
		{
			iPosition2.GetDouble(ref value12);
		}
		double value13 = double.NaN;
		if (iPosition3 != null)
		{
			iPosition3.GetDouble(ref value13);
		}
		double value14 = double.NaN;
		double value15 = double.NaN;
		double value16 = double.NaN;
		if (iSidePosition1 != null)
		{
			iSidePosition1.GetDouble(ref value14);
		}
		if (iSidePosition2 != null)
		{
			iSidePosition2.GetDouble(ref value15);
		}
		if (iSidePosition3 != null)
		{
			iSidePosition3.GetDouble(ref value16);
		}
		Curve curve = CreateSideProfile(start, end, new Vector3d(0.0, 1.0, 0.0), value14, value15, value16);
		Curve curve2 = CreateSideProfile(start2, end2, new Vector3d(0.0, -1.0, 0.0), value14, value15, value16);
		Curve curve3 = CreateSideProfile(start3, end3, new Vector3d(1.0, 0.0, 0.0), value11, value12, value13);
		Curve curve4 = CreateSideProfile(start4, end4, new Vector3d(-1.0, 0.0, 0.0), value11, value12, value13);
		CSDataArray cSDataArray = new CSDataArray();
		cSDataArray.Append(new CSOnGeometryData(value2));
		cSDataArray.Append(new CSOnGeometryData(value3));
		cSDataArray.Append(new CSOnGeometryData(curve.ToNurbsCurve()));
		cSDataArray.Append(new CSOnGeometryData(curve2.ToNurbsCurve()));
		cSDataArray.Append(new CSOnGeometryData(curve3.ToNurbsCurve()));
		cSDataArray.Append(new CSOnGeometryData(curve4.ToNurbsCurve()));
		oCurves.SetData(cSDataArray);
		oTop.SetData(new CSOnGeometryData(value2));
		return base.Update();
	}

	private static Curve CreateSideProfile(Point3d start, Point3d end, Vector3d direction, double p1, double p2, double p3)
	{
		Line line = new Line(start, end);
		if (double.IsNaN(p1))
		{
			return new LineCurve(start, end);
		}
		if (!double.IsNaN(p2))
		{
			if (!double.IsNaN(p3))
			{
				Point3dList points = new Point3dList
				{
					start,
					line.PointAt(0.25) + direction * p1,
					line.PointAt(0.5) + direction * p2,
					line.PointAt(0.75) + direction * p2,
					end
				};
				(end - start).Unitize();
				return Curve.CreateInterpolatedCurve(endTangent: new Vector3d(0.0, 0.0, -1.0), points: points, degree: 3, knots: CurveKnotStyle.Chord, startTangent: Vector3d.Unset);
			}
			return Curve.CreateInterpolatedCurve(new Point3dList
			{
				start,
				line.PointAt(0.33) + direction * p1,
				line.PointAt(0.66) + direction * p2,
				end
			}, endTangent: new Vector3d(0.0, 0.0, -1.0), degree: 3, knots: CurveKnotStyle.Chord, startTangent: Vector3d.Unset);
		}
		return Curve.CreateInterpolatedCurve(new Point3dList
		{
			start,
			line.PointAt(0.5) + direction * p1,
			end
		}, endTangent: new Vector3d(0.0, 0.0, -1.0), degree: 3, knots: CurveKnotStyle.Chord, startTangent: Vector3d.Unset);
	}
}
