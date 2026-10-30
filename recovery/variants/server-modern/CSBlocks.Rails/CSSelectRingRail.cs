using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Rails;

[CSBlock("SelectRingRail", "{E10A6966-719F-4ba9-9EFE-B7AD0378B5F3}")]
public sealed class CSSelectRingRail : CSBlock
{
	public readonly CSInputChannel iRingRail1;

	public readonly CSInputChannel iRingRail2;

	public readonly CSInputChannel iUseRingRail2;

	public readonly CSOutputChannel oRingRail;

	public CSSelectRingRail(string identifier)
		: base(identifier)
	{
		iRingRail1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRingRail2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iUseRingRail2 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oRingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		bool value = false;
		iUseRingRail2.GetBoolean(ref value);
		CSData data = iRingRail1.Data;
		CSData data2 = iRingRail2.Data;
		if (data == null && data2 == null)
		{
			oRingRail.SetData(null);
		}
		else if (data == null)
		{
			oRingRail.SetData(data2);
		}
		else if (data2 == null)
		{
			oRingRail.SetData(data);
		}
		else
		{
			oRingRail.SetData(value ? data2 : data);
		}
		return base.Update();
	}
}
