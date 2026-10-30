using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Rails.Obsolete;

[Obsolete]
[CSBlock("RingRail_V1", "{65AA45C6-D1CD-4c45-92BB-B5A9A6A2EC47}")]
public sealed class CSRingRail1 : CSBlock
{
	public CSInputChannel iRingRadius { get; }

	public CSOutputChannel oRingRail { get; }

	public CSRingRail1(string identifier)
		: base(identifier)
	{
		iRingRadius = new CSInputChannel(GetPropertyName(() => iRingRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRingRail = new CSOutputChannel(GetPropertyName(() => oRingRail), this, ChannelDataType.RingRail, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		double value = 7.0;
		iRingRadius.GetDouble(ref value);
		OnCircle val = new OnCircle(OnUtil.On_zx_plane, value);
		OnNurbsCurve nCrv = new OnNurbsCurve();
		if (val.GetNurbForm(ref nCrv) <= 0)
		{
			return UpdateError("GetNurbForm failed");
		}
		oRingRail.SetData(new CSRingRailData((Curve)(object)nCrv.Convert(), value));
		return base.Update();
	}
}
