using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

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
		double value = 0.0;
		iRingRadius.GetDouble(ref value);
		string input = "[SMI Ring Rail]" + Environment.NewLine + "Radius = " + value + "mm";
		OnCircle onCircle = new OnCircle(new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis, OnUtil.On_xaxis), value);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onCircle.GetNurbForm(ref nurbs_curve);
		oRingRail.SetData(new CSOnGeometryData(nurbs_curve.Convert()));
		oString.SetData(new CSStringData(input));
		return base.Update();
	}
}
