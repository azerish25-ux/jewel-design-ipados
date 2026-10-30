using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Tools;

[CSBlock("IntersectCrvSrf", "{3DB596E4-05D4-407A-AD17-606D0CA8E4F4}")]
public sealed class CSIntersectCrvSrf : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InIntersector;

	public readonly CSOutputChannel OutCurves;

	public readonly CSOutputChannel OutPoints;

	public CSIntersectCrvSrf(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InIntersector = new CSInputChannel(this, ChannelDataType.Transform | ChannelDataType.Curve | ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		OutCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutPoints = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(InCurve.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing Curve");
		}
		if (!CSUtilIntersect.GetIntersectSurface(InIntersector.Data, out var brep))
		{
			return UpdateMissing("Missing Intersector");
		}
		if (!Intersection.CurveBrep(value, brep, CSApplicationHelper.Application.Tolerance, out var overlapCurves, out var intersectionPoints))
		{
			return UpdateError("Intersection failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		Curve[] array = overlapCurves;
		foreach (Curve geometry in array)
		{
			cSDataArray.Append(new CSOnGeometryData(geometry));
		}
		OutCurves.SetData(cSDataArray);
		CSDataArray cSDataArray2 = new CSDataArray();
		Point3d[] array2 = intersectionPoints;
		foreach (Point3d pointStore in array2)
		{
			cSDataArray2.Append(new CSPointData(pointStore));
		}
		OutPoints.SetData(cSDataArray2);
		return base.Update();
	}
}
