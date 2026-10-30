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
		if (!value.TryGetPlane(out var plane))
		{
			return UpdateMissing("RingRail is not planar");
		}
		if (!value2.TryGetPlane(out var _))
		{
			return UpdateMissing("Curve is not planar");
		}
		LineCurve curveA = new LineCurve(new Point3d(0.0, -100.0, 0.0), new Point3d(100.0, -100.0, 0.0));
		LineCurve curveB = new LineCurve(new Point3d(0.0, 100.0, 0.0), new Point3d(100.0, 100.0, 0.0));
		NurbsSurface nurbsSurface = NurbsSurface.CreateRuledSurface(curveA, curveB);
		nurbsSurface.Rotate(CSBlockUtil.CSUtilMath.ToRadians(0.0 - value3), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		CurveIntersections curveIntersections = Intersection.CurveSurface(value, nurbsSurface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections.Count != 1)
		{
			return UpdateError("RingRail Intersect Failed");
		}
		double parameterA = curveIntersections[0].ParameterA;
		Point3d pointA = curveIntersections[0].PointA;
		NurbsCurve nurbsCurve = value.ToNurbsCurve();
		nurbsCurve = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Min, parameterA)).ToNurbsCurve();
		LineCurve curveA2 = new LineCurve(new Point3d(0.0, -100.0, 0.0), new Point3d(-100.0, -100.0, 0.0));
		curveB = new LineCurve(new Point3d(0.0, 100.0, 0.0), new Point3d(-100.0, 100.0, 0.0));
		nurbsSurface = NurbsSurface.CreateRuledSurface(curveA2, curveB);
		nurbsSurface.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value3), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		curveIntersections = Intersection.CurveSurface(value, nurbsSurface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections.Count() != 1)
		{
			return UpdateError("RingRail Intersect Failed");
		}
		double parameterA2 = curveIntersections[0].ParameterA;
		Point3d pointA2 = curveIntersections[0].PointA;
		NurbsCurve nurbsCurve2 = value.ToNurbsCurve();
		nurbsCurve2 = value.Trim(new Interval(parameterA2, nurbsCurve2.Domain.Max)).ToNurbsCurve();
		Curve[] array = Curve.JoinCurves(new List<Curve> { nurbsCurve2, nurbsCurve }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (array == null)
		{
			return UpdateError("Merge Curve Failed");
		}
		if (array.Length != 1)
		{
			return UpdateError("Error Merging RingRail Parts");
		}
		value = array[0].ToNurbsCurve();
		BoundingBox boundingBox = value2.GetBoundingBox(accurate: true);
		double y = boundingBox.Diagonal.Y;
		Vector3d vector3d = new Vector3d(plane.Normal);
		NurbsCurve nurbsCurve3 = new NurbsCurve(value);
		NurbsCurve nurbsCurve4 = new NurbsCurve(value);
		nurbsCurve3.Translate(vector3d * y);
		nurbsCurve4.Translate(vector3d * (0.0 - y));
		NurbsSurface surfaceA = NurbsSurface.CreateRuledSurface(nurbsCurve3, nurbsCurve4);
		NurbsCurve nurbsCurve5 = new NurbsCurve(value2);
		double num = pointA.X / boundingBox.Max.X;
		double num2 = pointA2.X / boundingBox.Min.X;
		double num3 = ((num > num2) ? num : num2);
		if (value4 || num3 < 1.0)
		{
			Transform xform = Transform.Scale(Plane.WorldXY, num3, value5 ? num3 : 1.0, 1.0);
			nurbsCurve5.Transform(xform);
		}
		nurbsCurve5.Translate(new Vector3d(0.0, 0.0, (pointA.Z + pointA2.Z) / 2.0 - boundingBox.Center.Z));
		NurbsSurface surfaceB = NurbsSurface.CreateRuledSurface(value2, nurbsCurve5);
		if (!Intersection.SurfaceSurface(surfaceA, surfaceB, CSApplicationHelper.Application.Tolerance, out var intersectionCurves, out var _))
		{
			return UpdateError("RhinoIntersectSurfaces failed");
		}
		Curve[] array2 = Curve.JoinCurves(intersectionCurves);
		if (array2.Length < 1)
		{
			return UpdateError("RhinoMergeCurves failed");
		}
		base.OutputChannels.Clear();
		for (int i = 0; i < array2.Length; i++)
		{
			Curve curve = array2[i];
			if (curve == null)
			{
				continue;
			}
			NurbsCurve nurbsCurve6 = curve.ToNurbsCurve();
			if (nurbsCurve6 != null)
			{
				if (nurbsCurve6.TangentAt(nurbsCurve6.Domain.Min).X < 0.0)
				{
					nurbsCurve6.Reverse();
				}
				new CSOutputChannel($"{base.Identifier}_{i}", this, ChannelDataType.Curve, ChannelArrayType.Individual).SetData(new CSOnGeometryData(nurbsCurve6));
			}
		}
		return base.Update();
	}
}
