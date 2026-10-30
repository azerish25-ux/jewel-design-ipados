using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("InterpCurveThroughPoints", "{D9FC6EA6-5B85-4304-B05F-27DF0AF4718A}")]
public sealed class CSInterpCurveThroughPoints : CSBlock
{
	public readonly CSInputChannel InPoints;

	public readonly CSOutputChannel OutCurve;

	public CSInterpCurveThroughPoints(string identifier)
		: base(identifier)
	{
		InPoints = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		List<Point3d> list = new List<Point3d>();
		if (!(InPoints.Data is CSDataArray cSDataArray))
		{
			return UpdateError("Error getting point array");
		}
		CSDataArray flattenArray = cSDataArray.GetFlattenArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetPoint(flattenArray.GetArrayItem(i), out var value);
			list.Add(value);
		}
		Curve geometry = Curve.CreateInterpolatedCurve(list, 3);
		OutCurve.SetData(new CSOnGeometryData(geometry));
		return base.Update();
	}
}
