using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Solids;

[CSBlock("BooleanDifference", "{D8022CC9-8AC7-4416-9195-551F462E4150}")]
public sealed class CSBooleanDifference : CSBlock
{
	public readonly CSInputChannel iSurfaceLhs;

	public readonly CSInputChannel iSurfaceRhs;

	public readonly CSInputChannel iFlip;

	public readonly CSOutputChannel oSurface;

	public CSBooleanDifference(string identifier)
		: base(identifier)
	{
		iSurfaceLhs = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		iSurfaceRhs = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		iFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iSurfaceLhs.GetFlattenArray();
		CSDataArray flattenArray2 = iSurfaceRhs.GetFlattenArray();
		bool value = false;
		iFlip.GetBoolean(ref value);
		List<Brep> cleanBrepInput = CSUtilDataHelper.GetCleanBrepInput(flattenArray);
		List<Brep> cleanBrepInput2 = CSUtilDataHelper.GetCleanBrepInput(flattenArray2);
		Brep[] array = (value ? Brep.CreateBooleanIntersection(cleanBrepInput, cleanBrepInput2, 0.01) : Brep.CreateBooleanDifference(cleanBrepInput, cleanBrepInput2, 0.01));
		if (array == null)
		{
			oSurface.SetData(iSurfaceLhs.Data.Duplicate());
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
			oSurface.SetData(iSurfaceLhs.Data.Duplicate());
		}
		return base.Update();
	}
}
