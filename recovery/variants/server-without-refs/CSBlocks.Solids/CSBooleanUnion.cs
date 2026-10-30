using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Solids;

[CSBlock("BooleanUnion", "{671B7AE9-DBD8-47ce-A0B6-B3DB83FBF91E}")]
public sealed class CSBooleanUnion : CSBlock
{
	public readonly CSInputChannel iSurface;

	public readonly CSOutputChannel oSurface;

	public CSBooleanUnion(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		Brep[] array = Brep.CreateBooleanUnion((IEnumerable<Brep>)CSUtilDataHelper.GetCleanBrepInput(iSurface.GetFlattenArray()).ToArray(), CSApplicationHelper.Application.Tolerance);
		if (array == null)
		{
			oSurface.SetData(iSurface.Data.Duplicate());
			return base.Update();
		}
		CSDataArray cSDataArray = CSUtilDataHelper.CleanBrepArray(array);
		if (cSDataArray.GetArrayCount() == 1)
		{
			oSurface.SetData(cSDataArray.GetArrayItem(0));
		}
		else if (cSDataArray.GetArrayCount() > 1)
		{
			oSurface.SetData(cSDataArray);
		}
		else
		{
			oSurface.SetData(iSurface.Data.Duplicate());
		}
		return base.Update();
	}
}
