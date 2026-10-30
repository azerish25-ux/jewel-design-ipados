using System.Collections.Generic;
using System.Linq;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Solids;

[CSBlock("CapPlanarHolesFast", "{CF53D6B9-1AE7-4ca4-9B36-E6E8035EEA1F}")]
public sealed class CSCapPlanarHolesFast : CSBlock
{
	public readonly CSInputChannel iSurface;

	public readonly CSOutputChannel oSurfaceCapped;

	public CSCapPlanarHolesFast(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array);
		oSurfaceCapped = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iSurface.GetFlattenArray();
		List<Brep> list = new List<Brep>();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem != null)
			{
				CSDataHelper.GetBrep(arrayItem, out var value);
				if (value != null)
				{
					list.Add(value);
				}
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		foreach (Brep item in list.Where((Brep t) => t?.IsValid ?? false))
		{
			if (item.IsSolid)
			{
				cSDataArray.Append(new CSOnGeometryData(item));
				continue;
			}
			Brep brep = CSUtilRhino.CapPlanarHoles(item, 0.01);
			if (brep != null)
			{
				CSUtilGeometry.FixBrepSolidOrientation(ref brep);
				cSDataArray.Append(new CSOnGeometryData(brep));
			}
		}
		oSurfaceCapped.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
