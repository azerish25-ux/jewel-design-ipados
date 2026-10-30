using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Rails;

[CSBlock("RingRailBypass", "{6C082A4B-B104-4f71-B2A4-C9BF0FFC8E1B}")]
public sealed class CSRingRailBypass : CSBlock
{
	public readonly CSInputChannel iRingRadius;

	public readonly CSInputChannel iRailTube;

	public readonly CSInputChannel iX0;

	public readonly CSInputChannel iY0;

	public readonly CSInputChannel iX1;

	public readonly CSInputChannel iY1;

	public readonly CSOutputChannel oRingRail;

	public CSRingRailBypass(string identifier)
		: base(identifier)
	{
		iRingRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailTube = new CSInputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		iX0 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY0 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iX1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		double value = 0.0;
		iRingRadius.GetDouble(ref value);
		double value2 = 0.0;
		iX0.GetDouble(ref value2);
		double value3 = 0.0;
		iY0.GetDouble(ref value3);
		double value4 = 0.0;
		iX1.GetDouble(ref value4);
		double value5 = 0.0;
		iY1.GetDouble(ref value5);
		NurbsSurface nurbsSurface = iRailTube.Data.GetNurbsSurface();
		if (nurbsSurface == null)
		{
			return UpdateMissing("Missing ring rail tube");
		}
		Point3d item = new Point3d(0.0 - value, 0.0, 0.0);
		Point3d item2 = new Point3d(0.0 - value + value2, value3, 0.0);
		Point3d item3 = new Point3d(0.0 - value + value2 + value4, value3 + value5, 0.0);
		NurbsCurve nurbsCurve = Curve.CreateInterpolatedCurve(startTangent: new Vector3d(1.0, 0.0, 0.0), points: new Point3dList { item, item2, item3 }, degree: 3, knots: CurveKnotStyle.Chord, endTangent: Vector3d.Unset).ToNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateError("RhinoInterpCurve failed");
		}
		Curve curve = nurbsCurve.DuplicateCurve();
		curve.Translate(new Vector3d(0.0, 0.0, 100.0));
		Brep brepA = NurbsSurface.CreateRuledSurface(nurbsCurve, curve).ToBrep();
		Brep brepB = nurbsSurface.ToBrep();
		Intersection.BrepBrep(brepA, brepB, 0.01, out var intersectionCurves, out var _);
		Curve curve2 = null;
		if (intersectionCurves.Length == 1)
		{
			curve2 = intersectionCurves[0];
			if (curve2 == null)
			{
				return UpdateError("RhinoIntersectBreps failed");
			}
		}
		else if (intersectionCurves.Length > 1)
		{
			Curve[] array = Curve.JoinCurves(intersectionCurves);
			if (array.Length >= 0)
			{
				curve2 = array[0];
			}
			if (curve2 == null)
			{
				return UpdateError("RhinoMergeCurves failed");
			}
		}
		if (curve2 == null)
		{
			return UpdateError("Unknown failed");
		}
		Curve curve3 = curve2.DuplicateCurve();
		curve3.Rotate(Math.PI, Vector3d.ZAxis, Point3d.Origin);
		ArcCurve arcCurve = new ArcCurve(new Arc(new Point3d(0.0 - value, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0 - value), new Point3d(value, 0.0, 0.0)));
		Curve[] array2 = Curve.JoinCurves(new Curve[3] { curve2, arcCurve, curve3 });
		if (array2.Length == 0)
		{
			return UpdateError("RhinoMergeCurves failed");
		}
		Curve curve4 = array2[0];
		curve4.Reverse();
		oRingRail.SetData(new CSRingRailData(curve4.ToNurbsCurve(), value));
		return base.Update();
	}
}
