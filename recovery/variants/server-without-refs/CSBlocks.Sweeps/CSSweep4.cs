using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Sweeps;

[CSBlock("Sweep4", "{29CCCEC3-01C1-45D4-B441-EF9F2C75BD45}")]
public sealed class CSSweep4 : CSBlock
{
	public CSInputChannel InRail1 { get; }

	public CSInputChannel InRail2 { get; }

	public CSInputChannel InRail3 { get; }

	public CSInputChannel InRail4 { get; }

	public CSInputChannel InProfiles { get; }

	public CSInputChannel InClosed { get; }

	public CSInputChannel InMethod { get; }

	public CSInputChannel InTryMatchCurveDirectionRails { get; }

	public CSInputChannel InTryMatchCurveDirectionProfiles { get; }

	public CSOutputChannel OutSurface { get; }

	public CSSweep4(string identifier)
		: base(identifier)
	{
		InRail1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InRail2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InRail3 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InRail4 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfiles = new CSInputChannel(this, ChannelDataType.Curve | ChannelDataType.Profile, ChannelArrayType.Array);
		InClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InTryMatchCurveDirectionRails = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InTryMatchCurveDirectionProfiles = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		Curve curve = InRail1.Curve;
		Curve curve2 = InRail2.Curve;
		Curve curve3 = InRail3.Curve;
		Curve curve4 = InRail4.Curve;
		CSDataArray flattenArray = CSDataHelper.GetFlattenArray(InProfiles.Data);
		bool boolean = InClosed.Boolean;
		ValueListItem valueListItem = InMethod.ValueListItem;
		bool boolean2 = InTryMatchCurveDirectionRails.Boolean;
		bool boolean3 = InTryMatchCurveDirectionProfiles.Boolean;
		List<CSUtilSweepRC.Sweep4Profile> list = new List<CSUtilSweepRC.Sweep4Profile>();
		foreach (CSData item in flattenArray.Array)
		{
			CSProfileData cSProfileData = item as CSProfileData;
			if (cSProfileData?.GetCurveParameterCount() == 4)
			{
				Curve curve5 = cSProfileData.GetCurve();
				double curveParameter = cSProfileData.GetCurveParameter(0);
				double curveParameter2 = cSProfileData.GetCurveParameter(1);
				double curveParameter3 = cSProfileData.GetCurveParameter(2);
				double curveParameter4 = cSProfileData.GetCurveParameter(3);
				list.Add(new CSUtilSweepRC.Sweep4Profile(curve5, curveParameter, curveParameter2, curveParameter3, curveParameter4));
			}
			else
			{
				Curve val = (item as ICSCurve)?.GetCurve();
				if (val != null)
				{
					list.Add(new CSUtilSweepRC.Sweep4Profile(val));
				}
			}
		}
		if (list.Count <= 0)
		{
			return UpdateMissing("Profiles");
		}
		bool doSweepSides;
		switch (valueListItem)
		{
		case ValueListItem.Sweep4MethodLeftAndRight:
			doSweepSides = true;
			break;
		case ValueListItem.Sweep4MethodTopAndBottom:
			doSweepSides = false;
			break;
		default:
			return UpdateError("Invalid method");
		}
		if (!CSUtilSweepRC.Sweep4NoParam(curve, curve2, curve3, curve4, list, boolean, doSweepSides, boolean2, boolean3, out var sweep, out var msg))
		{
			return UpdateError(msg);
		}
		OutSurface.SetData(new CSOnGeometryData((GeometryBase)(object)sweep));
		return base.Update();
	}
}
