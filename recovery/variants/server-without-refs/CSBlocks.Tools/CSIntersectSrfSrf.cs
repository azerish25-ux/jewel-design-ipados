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
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		if (!CSUtilIntersect.GetIntersectSurface(InSurface.Data, out var brep))
		{
			return UpdateMissing("Missing Surface");
		}
		if (!CSUtilIntersect.GetIntersectSurface(InIntersector.Data, out var brep2))
		{
			return UpdateMissing("Missing Intersector");
		}
		Curve[] array = default(Curve[]);
		Point3d[] array2 = default(Point3d[]);
		if (!Intersection.BrepBrep(brep, brep2, CSApplicationHelper.Application.Tolerance, ref array, ref array2))
		{
			return UpdateError("Intersection failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		Curve[] array3 = array;
		foreach (Curve geometry in array3)
		{
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)geometry));
		}
		OutCurves.SetData(cSDataArray);
		CSDataArray cSDataArray2 = new CSDataArray();
		Point3d[] array4 = array2;
		foreach (Point3d pointStore in array4)
		{
			cSDataArray2.Append(new CSPointData(pointStore));
		}
		OutPoints.SetData(cSDataArray2);
		return base.Update();
	}
}
