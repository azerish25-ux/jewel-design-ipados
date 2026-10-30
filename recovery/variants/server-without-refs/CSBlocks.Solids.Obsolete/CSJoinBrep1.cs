using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Solids.Obsolete;

[Obsolete]
[CSBlock("JoinBrep_V1", "{CE5BE8F9-4162-4438-99E4-A2606573F739}")]
public sealed class CSJoinBrep1 : CSBlockExpandableInput
{
	public CSOutputChannel OutJoined { get; }

	protected override string ExpandableInputChannelIdentifier => "InSurface";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Brep;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Array | ChannelArrayType.Individual;

	public CSJoinBrep1(string identifier)
		: base(identifier)
	{
		CreateExpansionInputChannel();
		OutJoined = new CSOutputChannel(GetPropertyName(() => OutJoined), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray cSDataArray = new CSDataArray();
		foreach (CSInputChannel nonExpandableInputChannel in base.NonExpandableInputChannels)
		{
			if (nonExpandableInputChannel != null)
			{
				CSData data = nonExpandableInputChannel.Data;
				if (data != null)
				{
					cSDataArray.Append(data);
				}
			}
		}
		CSDataArray flattenArray = cSDataArray.GetFlattenArray();
		List<OnBrep> list = new List<OnBrep>();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem != null)
			{
				OnBrep val = arrayItem.GetBrep().Convert();
				if (val != null)
				{
					list.Add(val);
				}
			}
		}
		OnBrep val2 = CSUtilRhino.JoinAllBreps(list.ToArray(), 0.01);
		if (val2 != null)
		{
			OutJoined.SetData(new CSOnGeometryData((GeometryBase)(object)val2.Convert()));
		}
		return base.Update();
	}
}
