using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Solids.Obsolete;

[Obsolete]
[CSBlock("JoinBrep_V2", "{43DE53C7-817A-4242-B26C-707780AFF511}")]
public sealed class CSJoinBrep2 : CSBlockExpandableInput
{
	public readonly CSOutputChannel OutJoined;

	protected override string ExpandableInputChannelIdentifier => "InSurface";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Brep;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Array | ChannelArrayType.Individual;

	public CSJoinBrep2(string identifier)
		: base(identifier)
	{
		CreateExpansionInputChannel();
		OutJoined = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
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
		List<Brep> list = new List<Brep>();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem != null)
			{
				Brep brep = arrayItem.GetBrep();
				if (brep != null)
				{
					CSUtilGeometry.FixBrepSolidOrientation(ref brep);
					list.Add(brep);
				}
			}
		}
		if (list.Count <= 0)
		{
			return base.Update();
		}
		Brep[] array = CSUtilRhino.JoinBreps(list.ToArray(), 0.01);
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int j = 0; j < array.Length; j++)
		{
			Brep brep2 = array[j];
			CSUtilGeometry.FixBrepSolidOrientation(ref brep2);
			cSDataArray2.Append(new CSOnGeometryData(brep2));
		}
		OutJoined.SetData((cSDataArray2.GetArrayCount() == 1) ? cSDataArray2.GetArrayItem(0) : cSDataArray2);
		return base.Update();
	}
}
