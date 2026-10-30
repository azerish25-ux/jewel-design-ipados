using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("ArcCurve", "{2CD1FE5B-AA39-4c59-936C-C6B024A019CB}")]
public sealed class CSArcCurve : CSBlock
{
	public readonly CSInputChannel iStartPoint;

	public readonly CSInputChannel iEndpoint;

	public readonly CSInputChannel iMiddlePoint;

	public readonly CSOutputChannel oArc;

	public CSArcCurve(string identifier)
		: base(identifier)
	{
		iStartPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iEndpoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iMiddlePoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oArc = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!CSDataHelper.GetPoint(iStartPoint.Data, out var value))
		{
			return UpdateMissing("Missing StartPoint");
		}
		if (!CSDataHelper.GetPoint(iMiddlePoint.Data, out var value2))
		{
			return UpdateMissing("Missing MidPoint");
		}
		if (!CSDataHelper.GetPoint(iEndpoint.Data, out var value3))
		{
			return UpdateMissing("Missing EndPoint");
		}
		Arc arc = new Arc(value, value2 - value, value3);
		if (!arc.IsValid)
		{
			return UpdateError("Error with Arc");
		}
		ArcCurve arcCurve = new ArcCurve(arc);
		if (!arcCurve.IsValid)
		{
			return UpdateError("No arc created");
		}
		oArc.SetData(new CSOnGeometryData(arcCurve.ToNurbsCurve()));
		return base.Update();
	}
}
