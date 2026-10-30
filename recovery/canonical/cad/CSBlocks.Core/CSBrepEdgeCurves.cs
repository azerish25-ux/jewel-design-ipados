using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("BrepEdgeCurves", "{56B7540D-671C-42a2-A44B-4F859BF38652}")]
public sealed class CSBrepEdgeCurves : CSBlock
{
	public readonly CSInputChannel iBrep;

	public readonly CSOutputChannel oArrayOfCurves;

	public CSBrepEdgeCurves(string identifier)
		: base(identifier)
	{
		iBrep = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		oArrayOfCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iBrep.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetBrep(flattenArray.GetArrayItem(i), out var value);
			if (value == null)
			{
				NurbsSurface nurbsSurface = iBrep.Data.GetNurbsSurface();
				if (nurbsSurface != null)
				{
					value = nurbsSurface.ToBrep();
				}
			}
			if (value != null)
			{
				for (int j = 0; j < value.Edges.Count; j++)
				{
					NurbsCurve geometry = value.Edges[j].ToNurbsCurve();
					cSDataArray.Append(new CSOnGeometryData(geometry));
				}
			}
		}
		oArrayOfCurves.SetData(cSDataArray);
		return base.Update();
	}
}
