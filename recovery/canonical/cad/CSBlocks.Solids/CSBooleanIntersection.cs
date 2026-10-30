using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Solids;

[CSBlock("BooleanIntersection", "{62C5F427-B471-433c-AFCA-D6A6AC0A5199}")]
public sealed class CSBooleanIntersection : CSBlock
{
	public readonly CSInputChannel iSurfaceLhs;

	public readonly CSInputChannel iSurfaceRhs;

	public readonly CSOutputChannel oSurface;

	public CSBooleanIntersection(string identifier)
		: base(identifier)
	{
		iSurfaceLhs = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		iSurfaceRhs = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iSurfaceLhs.GetFlattenArray();
		CSDataArray flattenArray2 = iSurfaceRhs.GetFlattenArray();
		List<Brep> cleanBrepInput = CSUtilDataHelper.GetCleanBrepInput(flattenArray);
		List<Brep> cleanBrepInput2 = CSUtilDataHelper.GetCleanBrepInput(flattenArray2);
		Brep[] array = null;
		array = Brep.CreateBooleanIntersection(cleanBrepInput.ToArray(), cleanBrepInput2.ToArray(), 0.01);
		if (array.Length < 1 || array == null)
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
