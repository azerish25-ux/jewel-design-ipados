using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Rails;

[CSBlock("SecondaryOutsideRingRail", "{523D952C-7171-4ed4-A5A4-9CCFF916438F}")]
public sealed class CSSecondaryOutsideRingRail : CSBlock
{
	public readonly CSInputChannel InOutsideRingRail;

	public readonly CSOutputChannel OutSecondaryOutsideRingRail;

	public CSSecondaryOutsideRingRail(string identifier)
		: base(identifier)
	{
		InOutsideRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutSecondaryOutsideRingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = InOutsideRingRail.Data;
		if (data == null)
		{
			return UpdateMissing("Outside Ring Rail");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!value.IsValid)
		{
			value = Transform.Identity;
		}
		if (!(data is CSOutsideRingRailData cSOutsideRingRailData))
		{
			return UpdateMissing("Outside Ring Rail");
		}
		CSDataHelper.GetNurbsCurve(cSOutsideRingRailData, out var value2);
		if (value2 == null)
		{
			return UpdateError("GetNurbsCurve failed");
		}
		double scale = cSOutsideRingRailData.Scale;
		value2.Transform(value);
		OutSecondaryOutsideRingRail.SetData(new CSOutsideRingRailData(value2, scale));
		return base.Update();
	}
}
