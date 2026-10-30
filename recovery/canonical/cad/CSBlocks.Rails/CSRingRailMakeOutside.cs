using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Rails;

[CSBlock("RingRailMakeOutside", "{620DCD47-00EF-4e0d-9A17-BCBB79B396C5}")]
public sealed class CSRingRailMakeOutside : CSBlock
{
	public readonly CSInputChannel iRingRail;

	public readonly CSInputChannel iOutsideRailTube;

	public readonly CSInputChannel iOutsideRailLine;

	public readonly CSInputChannel iScale;

	public readonly CSOutputChannel oOutsideRingRail;

	public CSRingRailMakeOutside(string identifier)
		: base(identifier)
	{
		iRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRailTube = new CSInputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		iOutsideRailLine = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iScale = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oOutsideRingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iRingRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Ring Rail");
		}
		NurbsSurface nurbsSurface = iOutsideRailTube.Data.GetNurbsSurface();
		if (nurbsSurface == null)
		{
			return UpdateMissing("Outside Ring Rail Tube");
		}
		CSDataHelper.GetNurbsCurve(iOutsideRailLine.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Outside Ring Rail Line");
		}
		double value3 = 1.0;
		iScale.GetDouble(ref value3);
		Transform xform = Transform.Scale(Plane.WorldXY, 3.0, 1.0 + value3, 3.0);
		NurbsCurve nurbsCurve = new NurbsCurve(value);
		nurbsCurve.Transform(xform);
		NurbsSurface nurbsSurface2 = NurbsSurface.CreateRuledSurface(value, nurbsCurve);
		if (!nurbsSurface2.IsValid)
		{
			return UpdateError("CreateRuledSurface failed");
		}
		if (!Intersection.SurfaceSurface(nurbsSurface2, nurbsSurface, CSApplicationHelper.Application.Tolerance, out var intersectionCurves, out var _))
		{
			return UpdateError("IntersectionSurfaces failed");
		}
		intersectionCurves = Curve.JoinCurves(intersectionCurves);
		NurbsCurve nurbsCurve2 = intersectionCurves[0].ToNurbsCurve();
		if (nurbsCurve2.GetLength() <= 0.0)
		{
			return UpdateError("GetNurbForm failed");
		}
		if (nurbsCurve2.IsClosed)
		{
			CurveIntersections curveIntersections = Intersection.CurveCurve(nurbsCurve2, value2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count > 0)
			{
				double num = double.NaN;
				int index = 0;
				for (int i = 0; i < curveIntersections.Count; i++)
				{
					double num2 = nurbsCurve2.PointAt(curveIntersections[i].ParameterA).DistanceTo(Point3d.Origin);
					if (double.IsNaN(num) || !(num2 >= num))
					{
						num = num2;
						index = i;
					}
				}
				if (!nurbsCurve2.ChangeClosedCurveSeam(curveIntersections[index].ParameterA))
				{
					return UpdateError("ChangeClosedCurveSeam failed");
				}
			}
		}
		if (value.IsClosed && nurbsCurve2.IsClosed)
		{
			CSUtilSweepRC.MatchCurveDirection2(value, nurbsCurve2, 16);
		}
		else
		{
			CSUtilSweepRC.MatchCurveDirection(value, nurbsCurve2, 16);
		}
		oOutsideRingRail.SetData(new CSOutsideRingRailData(nurbsCurve2, value3));
		return base.Update();
	}
}
