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
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		if (!CSDataHelper.GetBrep(InBrep.Data, out var value))
		{
			return UpdateError("Missing surface");
		}
		if (!CSDataHelper.GetPoint(InPoint.Data, out var value2))
		{
			return UpdateError("Missing point");
		}
		double num = double.MaxValue;
		int num2 = 0;
		double num3 = default(double);
		foreach (BrepEdge edge in value.Edges)
		{
			((Curve)edge).ClosestPoint(value2, ref num3);
			double num4 = ((Point3d)(ref value2)).DistanceTo(((Curve)edge).PointAt(num3));
			if (num4 < num)
			{
				num = num4;
				num2 = edge.EdgeIndex;
			}
		}
		OutEdge.SetData(new CSBrepEdgeData(value.Edges[num2]));
		return base.Update();
	}
}
