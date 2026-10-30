using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CS.Parametric.Interfaces;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("ChainPoint", "{1A68D6BA-11B7-431c-85BB-E3A03CE5CBFF}")]
public sealed class CSChainPoint : CSBlock, IChainPoint
{
	public CSInputChannel InBail { get; }

	public CSInputChannel InAdjustX { get; }

	public CSInputChannel InAdjustY { get; }

	public CSInputChannel InAdjustZ { get; }

	public CSOutputChannel OutChainPoint { get; }

	public CSChainPoint(string identifier)
		: base(identifier)
	{
		InBail = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InAdjustX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAdjustY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAdjustZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChainPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public Point3d GetChainPoint()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (!(OutChainPoint.Data is ICSPoint iCSPoint))
		{
			return Point3d.Origin;
		}
		if (!iCSPoint.GetPoint(out var value))
		{
			return Point3d.Origin;
		}
		if (!((Point3d)(ref value)).IsValid)
		{
			return Point3d.Origin;
		}
		return value;
	}

	public override bool Update()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		double value = 0.0;
		InAdjustX.GetDouble(ref value);
		double value2 = 0.0;
		InAdjustY.GetDouble(ref value2);
		double value3 = 0.0;
		InAdjustZ.GetDouble(ref value3);
		Point3d pointStore = Point3d.Origin;
		CSData data = InBail.Data;
		if (data != null)
		{
			BoundingBox boundingBox = default(BoundingBox);
			if (data.GetTightBoundingBox(ref boundingBox))
			{
				pointStore = ((BoundingBox)(ref boundingBox)).Center;
			}
		}
		((Point3d)(ref pointStore)).X = ((Point3d)(ref pointStore)).X + value;
		((Point3d)(ref pointStore)).Y = ((Point3d)(ref pointStore)).Y + value2;
		((Point3d)(ref pointStore)).Z = ((Point3d)(ref pointStore)).Z + value3;
		OutChainPoint.SetData(new CSPointData(pointStore));
		return base.Update();
	}
}
