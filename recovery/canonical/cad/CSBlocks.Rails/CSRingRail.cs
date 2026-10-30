using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Rails;

[CSBlock("RingRail", "{3578ED28-0CA1-4B69-BD57-A88DACBDDACD}")]
public sealed class CSRingRail : CSBlock
{
	public readonly CSInputChannel InRingRailRadius;

	public readonly CSOutputChannel OutRingRail;

	public CSRingRail(string identifier)
		: base(identifier)
	{
		InRingRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		double value = 8.667196228921199;
		InRingRailRadius.GetDouble(ref value);
		if (!CSUtilRingRail.CreateRingRail(value, out NurbsCurve ringRail))
		{
			return UpdateError("CreateRingRail failed");
		}
		OutRingRail.SetData(new CSRingRailData(ringRail, value));
		return base.Update();
	}
}
