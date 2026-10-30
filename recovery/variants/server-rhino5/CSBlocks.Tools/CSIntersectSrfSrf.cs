using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Tools;

[CSBlock("IntersectSrfSrf", "{2DE49D73-0271-4FAF-BF6C-4139861BC479}")]
public sealed class CSIntersectSrfSrf : CSBlock
{
	public readonly CSInputChannel InSurface;

	public readonly CSInputChannel InIntersector;

	public readonly CSOutputChannel OutCurves;

	public readonly CSOutputChannel OutPoints;

	public CSIntersectSrfSrf(string identifier)
		: base(identifier)
	{
		InSurface = new CSInputChannel(this, ChannelDataType.Transform | ChannelDataType.Curve | ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		InIntersector = new CSInputChannel(this, ChannelDataType.Transform | ChannelDataType.Curve | ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		OutCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutPoints = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!CSUtilIntersect.GetIntersectSurface(InSurface.Data, out var brep))
		{
			return UpdateMissing("Missing Surface");
		}
		if (!CSUtilIntersect.GetIntersectSurface(InIntersector.Data, out var brep2))
		{
			return UpdateMissing("Missing Intersector");
		}
		if (!Intersection.BrepBrep(brep, brep2, CSApplicationHelper.Application.Tolerance, out var intersectionCurves, out var intersectionPoints))
		{
			return UpdateError("Intersection failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		Curve[] array = intersectionCurves;
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
