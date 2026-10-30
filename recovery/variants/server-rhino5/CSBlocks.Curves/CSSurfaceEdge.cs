using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("SurfaceEdge", "{8CA33372-ECE7-4D9C-840D-CC2454E5A00B}")]
public sealed class CSSurfaceEdge : CSBlock
{
	public readonly CSInputChannel InBrep;

	public readonly CSInputChannel InPoint;

	public readonly CSOutputChannel OutEdge;

	public CSSurfaceEdge(string identifier)
		: base(identifier)
	{
		InBrep = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutEdge = new CSOutputChannel(this, ChannelDataType.BrepEdge, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!CSDataHelper.GetBrep(InBrep.Data, out var value))
		{
			return UpdateError("Missing surface");
		}
		if (!CSDataHelper.GetPoint(InPoint.Data, out var value2))
		{
			return UpdateError("Missing point");
		}
		double num = double.MaxValue;
		int index = 0;
		foreach (BrepEdge edge in value.Edges)
		{
			edge.ClosestPoint(value2, out var t);
			double num2 = value2.DistanceTo(edge.PointAt(t));
			if (num2 < num)
			{
				num = num2;
				index = edge.EdgeIndex;
			}
		}
		OutEdge.SetData(new CSBrepEdgeData(value.Edges[index]));
		return base.Update();
	}
}
