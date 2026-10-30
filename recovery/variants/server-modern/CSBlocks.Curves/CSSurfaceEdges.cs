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
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Invalid comparison between Unknown and I4
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Invalid comparison between Unknown and I4
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Invalid comparison between Unknown and I4
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
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
		Vector3d tangentAtEnd;
		switch (value2)
		{
		case 0:
			foreach (BrepEdge edge in value.Edges)
			{
				if ((int)edge.Valence == 1)
				{
					cSDataArray.Append(new CSBrepEdgeData(edge));
				}
			}
			break;
		case 1:
		{
			BrepEdge val = null;
			foreach (BrepEdge edge2 in value.Edges)
			{
				if ((int)edge2.Valence != 1)
				{
					continue;
				}
				if (val == null)
				{
					val = edge2;
				}
				tangentAtEnd = ((Curve)val).TangentAtEnd;
				if (((Vector3d)(ref tangentAtEnd)).IsParallelTo(((Curve)edge2).TangentAtEnd) != 1)
				{
					tangentAtEnd = ((Curve)val).TangentAtEnd;
					if (((Vector3d)(ref tangentAtEnd)).IsParallelTo(((Curve)edge2).TangentAtEnd) != -1)
					{
						continue;
					}
				}
				cSDataArray.Append(new CSBrepEdgeData(edge2));
			}
			break;
		}
		case 2:
		{
			BrepEdge val = null;
			foreach (BrepEdge edge3 in value.Edges)
			{
				if ((int)edge3.Valence == 1)
				{
					if (val == null)
					{
						val = edge3;
					}
					tangentAtEnd = ((Curve)val).TangentAtEnd;
					if (((Vector3d)(ref tangentAtEnd)).IsParallelTo(((Curve)edge3).TangentAtEnd) == 0)
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
