using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Solids;

[CSBlock("JoinAllBrep", "{B5269356-28F2-49F6-A20E-C6BEEDB01347}")]
public sealed class CSJoinAllBrep : CSBlockExpandableInput
{
	public readonly CSOutputChannel OutJoined;

	protected override string ExpandableInputChannelIdentifier => "InSurface";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Brep;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Array | ChannelArrayType.Individual;

	public CSJoinAllBrep(string identifier)
		: base(identifier)
	{
		CreateExpansionInputChannel();
		OutJoined = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
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
				CSDataHelper.GetBrep(arrayItem, out var value);
				if (value != null)
				{
					CSUtilGeometry.FixBrepSolidOrientation(ref value);
					list.Add(value);
				}
			}
		}
		if (list.Count > 0)
		{
			Brep[] array = Brep.JoinBreps(list.ToArray(), CSApplicationHelper.Application.Tolerance);
			if (array.Length == 0)
			{
				return base.Update();
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			for (int j = 0; j < array.Length; j++)
			{
				Brep brep = array[j];
				CSUtilGeometry.FixBrepSolidOrientation(ref brep);
				cSDataArray2.Append(new CSOnGeometryData(brep));
			}
			if (cSDataArray2.GetArrayCount() != 1)
			{
				return UpdateError("Join failed to join into one surface.");
			}
			OutJoined.SetData(cSDataArray2.GetArrayItem(0));
		}
		return base.Update();
	}
}
