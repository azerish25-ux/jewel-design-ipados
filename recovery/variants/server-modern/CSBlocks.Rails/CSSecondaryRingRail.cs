using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Rails;

[CSBlock("SecondaryRingRail", "{17205A3E-4F5E-4da3-8282-6F3F35DD79C9}")]
public sealed class CSSecondaryRingRail : CSBlock
{
	public readonly CSInputChannel InRingRail;

	public readonly CSOutputChannel OutSecondaryRingRail;

	public CSSecondaryRingRail(string identifier)
		: base(identifier)
	{
		InRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutSecondaryRingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InRingRail.Data;
		if (data == null)
		{
			return UpdateMissing("Ring Rail");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!((Transform)(ref value)).IsValid)
		{
			value = Transform.Identity;
		}
		if (!(data is CSRingRailData cSRingRailData))
		{
			return UpdateMissing("Ring Rail");
		}
		CSDataHelper.GetNurbsCurve(cSRingRailData, out var value2);
		if (value2 == null)
		{
			return UpdateError("GetNurbsCurve failed");
		}
		double radius = cSRingRailData.Radius;
		((GeometryBase)value2).Transform(value);
		OutSecondaryRingRail.SetData(new CSRingRailData((Curve)(object)value2, radius));
		return base.Update();
	}
}
