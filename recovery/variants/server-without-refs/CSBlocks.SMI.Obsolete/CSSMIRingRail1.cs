using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.SMI.Obsolete;

[Obsolete]
[CSBlock("SMIRingRail_V1", "{9463DC0F-2206-452b-A502-BC2B5BB71C05}")]
public sealed class CSSMIRingRail1 : CSBlock
{
	public CSInputChannel iRingRadius { get; }

	public CSOutputChannel oRingRail { get; }

	public CSOutputChannel oString { get; }

	public CSSMIRingRail1(string identifier)
		: base(identifier)
	{
		iRingRadius = new CSInputChannel(GetPropertyName(() => iRingRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRingRail = new CSOutputChannel(GetPropertyName(() => oRingRail), this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		oString = new CSOutputChannel(GetPropertyName(() => oString), this, ChannelDataType.RingRail, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		double value = 0.0;
		iRingRadius.GetDouble(ref value);
		string input = "[SMI Ring Rail]" + Environment.NewLine + "Radius = " + value + "mm";
		OnCircle val = new OnCircle((IOnPlane)new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis, OnUtil.On_xaxis), value);
		OnNurbsCurve nCrv = new OnNurbsCurve();
		val.GetNurbForm(ref nCrv);
		oRingRail.SetData(new CSOnGeometryData((GeometryBase)(object)nCrv.Convert()));
		oString.SetData(new CSStringData(input));
		return base.Update();
	}
}
