using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Sweeps;

[CSBlock("SimpleSweep2", "{F19EA1FF-57E7-4922-9FF7-87535B49C1C6}")]
public sealed class CSSimpleSweep2 : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InRail2;

	public readonly CSInputChannel InCurves;

	public readonly CSInputChannel InParams;

	public readonly CSInputChannel InParams2;

	public readonly CSInputChannel InClosed;

	public readonly CSInputChannel InMaintainHeight;

	public readonly CSInputChannel InChangeRailCurveSeamToMatchFirstProfile;

	public readonly CSOutputChannel OutBrep;

	public CSSimpleSweep2(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InRail2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InParams = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InParams2 = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InMaintainHeight = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InChangeRailCurveSeamToMatchFirstProfile = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		bool value = false;
		InChangeRailCurveSeamToMatchFirstProfile.GetBoolean(ref value);
		CSDataArray flattenArray = InRail.GetFlattenArray();
		CSDataArray flattenArray2 = InRail2.GetFlattenArray();
		if (flattenArray.GetArrayCount() < 1 && flattenArray2.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Rails");
		}
		OnNurbsCurve onNurbsCurve = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		OnNurbsCurve onNurbsCurve2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onNurbsCurve == null || onNurbsCurve2 == null)
		{
			return UpdateMissing("Missing Rail");
		}
		List<IOnCurve> list = new List<IOnCurve>();
		CSDataArray flattenArray3 = InCurves.GetFlattenArray();
		if (flattenArray3 == null)
		{
			return UpdateMissing("Missing Curve Array");
		}
		for (int i = 0; i < flattenArray3.GetArrayCount(); i++)
		{
			if (flattenArray3.GetArrayItem(i) == null)
			{
				return UpdateMissing("Missing Curve Array ( No Object)");
			}
			list.Add(flattenArray3.GetArrayItem(i).GetNurbsCurve().Convert());
			if (list[i] == null)
			{
				return UpdateMissing("Missing Curve Array ( No Object)");
			}
		}
		List<double> list2 = new List<double>();
		CSData cSData = InParams?.Data;
		if (cSData != null)
		{
			for (int j = 0; j < CSDataHelper.GetArrayCount(cSData); j++)
			{
				if (CSDataHelper.GetArrayItem(cSData, j) == null)
				{
					return UpdateMissing("Missing Param Array ( No Object)");
				}
				double value2 = 0.0;
				if (!CSDataHelper.GetArrayItem(cSData, j).GetDouble(ref value2))
				{
					return UpdateMissing("Missing Param Array ( No Object)");
				}
				list2.Add(value2);
			}
		}
		List<double> list3 = new List<double>();
		CSData cSData2 = InParams2?.Data;
		if (cSData2 != null)
		{
			for (int k = 0; k < CSDataHelper.GetArrayCount(cSData2); k++)
			{
				if (CSDataHelper.GetArrayItem(cSData2, k) == null)
				{
					return UpdateMissing("Missing Param Array 2 ( No Object)");
				}
				double value3 = 0.0;
				if (!CSDataHelper.GetArrayItem(cSData2, k).GetDouble(ref value3))
				{
					return UpdateError("Missing Param Array (Are your 2 rail sweeping a 1 rail profile?)");
				}
				list3.Add(value3);
			}
		}
		bool value4 = false;
		InClosed?.GetBoolean(ref value4);
		bool value5 = false;
		InMaintainHeight?.GetBoolean(ref value5);
		OnBrep[] breps;
		if (list2.Count == 0 && list3.Count == 0)
		{
			if (!CSUtilSweep.Sweep2NoParam(onNurbsCurve, onNurbsCurve2, list.ToArray(), out breps, value5, value, value4, flipProfiles: true) || breps.Length == 0)
			{
				return UpdateError("Sweep 2 No Parameters Failed");
			}
		}
		else if (!CSUtilSweep.Sweep2(onNurbsCurve, onNurbsCurve2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, value4, value5) || breps.Length == 0)
		{
			return UpdateError("Sweep 2 Failed");
		}
		base.OutputChannels.Clear();
		for (int l = 0; l < breps.Length; l++)
		{
			OnBrep onBrep = breps[l];
			if (onBrep != null)
			{
				new CSOutputChannel($"{base.Identifier}_{l}", this, ChannelDataType.Any, ChannelArrayType.Individual).SetData(new CSOnGeometryData(onBrep.Convert()));
			}
		}
		return base.Update();
	}
}
