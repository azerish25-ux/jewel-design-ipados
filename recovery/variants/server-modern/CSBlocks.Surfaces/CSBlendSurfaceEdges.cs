using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Surfaces;

[CSBlock("BlendSurfaceEdges", "{B7DFCC54-69E4-47A4-9859-6B2AE2AFFF74}")]
public sealed class CSBlendSurfaceEdges : CSBlock
{
	public CSInputChannel InEdge1 { get; }

	public CSInputChannel InEdge2 { get; }

	public CSInputChannel InFlip1 { get; }

	public CSInputChannel InFlip2 { get; }

	public CSOutputChannel OutSurface { get; }

	public CSBlendSurfaceEdges(string identifier)
		: base(identifier)
	{
		InEdge1 = new CSInputChannel(this, ChannelDataType.Curve | ChannelDataType.BrepEdge, ChannelArrayType.Individual);
		InEdge2 = new CSInputChannel(this, ChannelDataType.Curve | ChannelDataType.BrepEdge, ChannelArrayType.Individual);
		InFlip1 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InFlip2 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!CSDataHelper.GetBrepEdge(InEdge1.Data, out var value))
		{
			return UpdateError("Missing First Edge");
		}
		if (!CSDataHelper.GetBrepEdge(InEdge2.Data, out var value2))
		{
			return UpdateError("Missing Second Edge");
		}
		bool value3 = true;
		InFlip1.GetBoolean(ref value3);
		bool value4 = false;
		InFlip2.GetBoolean(ref value4);
		if (!CSUtilGeometry.LoftBrepEdge(new List<Curve>
		{
			(Curve)(object)value,
			(Curve)(object)value2
		}, value3, value4, out var surface))
		{
			return UpdateError("LoftBrepEdge Failed");
		}
		if (surface != null)
		{
			OutSurface.SetData(new CSOnGeometryData((GeometryBase)(object)surface));
		}
		return base.Update();
	}
}
