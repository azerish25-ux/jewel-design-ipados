using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("SurfaceEdges", "{CC8C4F3B-C604-4BAF-9AF7-4D1AC394B016}")]
public sealed class CSSurfaceEdges : CSBlock
{
	public CSInputChannel InBrep { get; }

	public CSInputChannel InEdgeType { get; }

	public CSOutputChannel OutEdges { get; }

	public CSSurfaceEdges(string identifier)
		: base(identifier)
	{
		InBrep = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InEdgeType = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutEdges = new CSOutputChannel(this, ChannelDataType.BrepEdge, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!CSDataHelper.GetBrep(InBrep.Data, out var value))
		{
			return UpdateError("Missing surface");
		}
		int value2 = 0;
		if (!InEdgeType.GetInteger(ref value2))
		{
			return UpdateError("Missing edge type");
		}
		CSDataArray cSDataArray = new CSDataArray();
		switch (value2)
		{
		case 0:
			foreach (BrepEdge edge in value.Edges)
			{
				if (edge.Valence == EdgeAdjacency.Naked)
				{
					cSDataArray.Append(new CSBrepEdgeData(edge));
				}
			}
			break;
		case 1:
		{
			BrepEdge brepEdge = null;
			foreach (BrepEdge edge2 in value.Edges)
			{
				if (edge2.Valence == EdgeAdjacency.Naked)
				{
					if (brepEdge == null)
					{
						brepEdge = edge2;
					}
					if (brepEdge.TangentAtEnd.IsParallelTo(edge2.TangentAtEnd) == 1 || brepEdge.TangentAtEnd.IsParallelTo(edge2.TangentAtEnd) == -1)
					{
						cSDataArray.Append(new CSBrepEdgeData(edge2));
					}
				}
			}
			break;
		}
		case 2:
		{
			BrepEdge brepEdge = null;
			foreach (BrepEdge edge3 in value.Edges)
			{
				if (edge3.Valence == EdgeAdjacency.Naked)
				{
					if (brepEdge == null)
					{
						brepEdge = edge3;
					}
					if (brepEdge.TangentAtEnd.IsParallelTo(edge3.TangentAtEnd) == 0)
					{
						cSDataArray.Append(new CSBrepEdgeData(edge3));
					}
				}
			}
			break;
		}
		default:
			return UpdateError("Unsupported edge type");
		}
		OutEdges.SetData(cSDataArray);
		return base.Update();
	}
}
