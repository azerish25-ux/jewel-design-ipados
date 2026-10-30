using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

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
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(iCurve.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Curve1");
		}
		oPoint.SetData(new CSPointData(((Curve)value).PointAtStart));
		CSOutputChannel cSOutputChannel = oParam;
		Interval domain = ((Curve)value).Domain;
		cSOutputChannel.SetData(new CSNumberData(((Interval)(ref domain)).Min));
		return base.Update();
	}
}
