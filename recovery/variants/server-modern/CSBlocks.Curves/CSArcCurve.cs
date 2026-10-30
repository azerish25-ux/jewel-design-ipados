using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Runtime;

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
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
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
		Arc val = default(Arc);
		((Arc)(ref val))._002Ector(value, value2 - value, value3);
		if (!((Arc)(ref val)).IsValid)
		{
			return UpdateError("Error with Arc");
		}
		ArcCurve val2 = new ArcCurve(val);
		if (!((CommonObject)val2).IsValid)
		{
			return UpdateError("No arc created");
		}
		oArc.SetData(new CSOnGeometryData((GeometryBase)(object)((Curve)val2).ToNurbsCurve()));
		return base.Update();
	}
}
