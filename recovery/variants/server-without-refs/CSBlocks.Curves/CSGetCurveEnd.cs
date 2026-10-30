using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("GetCurveEnd", "{3FF34234-8E29-4aed-88F1-A224785C592A}")]
public sealed class CSGetCurveEnd : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSOutputChannel oPoint;

	public readonly CSOutputChannel oParam;

	public CSGetCurveEnd(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(iCurve.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Curve1");
		}
		oPoint.SetData(new CSPointData(((Curve)value).PointAtEnd));
		CSOutputChannel cSOutputChannel = oParam;
		Interval domain = ((Curve)value).Domain;
		cSOutputChannel.SetData(new CSNumberData(((Interval)(ref domain)).Max));
		return base.Update();
	}
}
