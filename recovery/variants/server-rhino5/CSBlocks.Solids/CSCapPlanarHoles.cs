using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Solids;

[CSBlock("CapPlanarHoles", "{BED891FD-E917-4968-B3D1-418D293E0E92}")]
public sealed class CSCapPlanarHoles : CSBlock
{
	public CSInputChannel InSurface { get; }

	public CSOutputChannel OutSurface { get; }

	public CSCapPlanarHoles(string identifier)
		: base(identifier)
	{
		InSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InSurface.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			OnBrep val = flattenArray.GetArrayItem(i)?.GetBrep().Convert();
			if (val == null || val.m_S.Count() == 0)
			{
				break;
			}
			if (val.IsSolid())
			{
				cSDataArray.Append(new CSOnGeometryData(val.Convert()));
				continue;
			}
			OnBrep brep = CSUtilRhino.CapPlanarHoles(val, CSApplicationHelper.Application.Tolerance);
			if (brep == null)
			{
				break;
			}
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			cSDataArray.Append(new CSOnGeometryData(brep.Convert()));
		}
		OutSurface.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
