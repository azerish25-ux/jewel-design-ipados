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

[CSBlock("JoinBrep", "{355B8692-E60E-4FAC-B28A-8F974E29F315}")]
public sealed class CSJoinBrep : CSBlockExpandableInput
{
	public readonly CSOutputChannel OutJoined;

	protected override string ExpandableInputChannelIdentifier => "InSurface";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Brep;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Array | ChannelArrayType.Individual;

	public CSJoinBrep(string identifier)
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
			Brep[] array = Brep.JoinBreps((IEnumerable<Brep>)list.ToArray(), CSApplicationHelper.Application.Tolerance);
			if (array.Length == 0)
			{
				return base.Update();
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			for (int j = 0; j < array.Length; j++)
			{
				Brep brep = array[j];
				CSUtilGeometry.FixBrepSolidOrientation(ref brep);
				cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)brep));
			}
			OutJoined.SetData((cSDataArray2.GetArrayCount() == 1) ? cSDataArray2.GetArrayItem(0) : cSDataArray2);
		}
		return base.Update();
	}
}
