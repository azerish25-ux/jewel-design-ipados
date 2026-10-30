using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Curves;

[CSBlock("GetCurveStart", "{0DB57993-7FB4-48f4-8C4B-3779EFBC5EDE}")]
public sealed class CSGetCurveStart : CSBlock
{
	public CSInputChannel iCurve;

	public CSOutputChannel oPoint;

	public CSOutputChannel oParam;

	public CSGetCurveStart(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oPoint = new CSOutputChannel(GetPropertyName(() => oPoint), this, ChannelDataType.Point, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(GetPropertyName(() => oParam), this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iCurve.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Curve1");
		}
		oPoint.SetData(new CSPointData(value.PointAtStart));
		oParam.SetData(new CSNumberData(value.Domain.Min));
		return base.Update();
	}
}
