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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InOutsideRingRail.Data;
		if (data == null)
		{
			return UpdateMissing("Outside Ring Rail");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!((Transform)(ref value)).IsValid)
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
		((GeometryBase)value2).Transform(value);
		OutSecondaryOutsideRingRail.SetData(new CSOutsideRingRailData((Curve)(object)value2, scale));
		return base.Update();
	}
}
