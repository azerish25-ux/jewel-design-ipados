using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core.Obsolete;

[Obsolete]
[CSBlock("BoundingBox_V3", "{C966C6B9-D72E-4673-B32B-1100DD040A1A}")]
public sealed class CSBoundingBox3 : CSBlock
{
	public readonly CSInputChannel InObject;

	public readonly CSInputChannel InGrowX;

	public readonly CSInputChannel InGrowY;

	public readonly CSInputChannel InGrowZ;

	public readonly CSOutputChannel OutCenter;

	public readonly CSOutputChannel OutMax;

	public readonly CSOutputChannel OutMin;

	public readonly CSOutputChannel OutMaxX;

	public readonly CSOutputChannel OutMinX;

	public readonly CSOutputChannel OutSizeX;

	public readonly CSOutputChannel OutMaxY;

	public readonly CSOutputChannel OutMinY;

	public readonly CSOutputChannel OutSizeY;

	public readonly CSOutputChannel OutMaxZ;

	public readonly CSOutputChannel OutMinZ;

	public readonly CSOutputChannel OutSizeZ;

	public CSBoundingBox3(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InGrowX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGrowY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGrowZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutCenter = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutMax = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutMin = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutMaxX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMinX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMaxY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMinY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMaxZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMinZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InObject.Data;
		if (data == null)
		{
			return UpdateMissing("Missing object");
		}
		double value = 0.0;
		InGrowX.GetDouble(ref value);
		double value2 = 0.0;
		InGrowY.GetDouble(ref value2);
		double value3 = 0.0;
		InGrowZ.GetDouble(ref value3);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		Point3d min = ((BoundingBox)(ref boundingBox)).Min;
		((Point3d)(ref min)).X = ((Point3d)(ref min)).X - value;
		((Point3d)(ref min)).Y = ((Point3d)(ref min)).Y - value2;
		((Point3d)(ref min)).Z = ((Point3d)(ref min)).Z - value3;
		((BoundingBox)(ref boundingBox)).Min = min;
		Point3d max = ((BoundingBox)(ref boundingBox)).Max;
		((Point3d)(ref max)).X = ((Point3d)(ref max)).X + value;
		((Point3d)(ref max)).Y = ((Point3d)(ref max)).Y + value2;
		((Point3d)(ref max)).Z = ((Point3d)(ref max)).Z + value3;
		((BoundingBox)(ref boundingBox)).Max = max;
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double number = x - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double number2 = y - ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double z = ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double number3 = z - ((Point3d)(ref val)).Z;
		OutCenter.SetData(new CSPointData(((BoundingBox)(ref boundingBox)).Center));
		OutMax.SetData(new CSPointData(((BoundingBox)(ref boundingBox)).Max));
		OutMin.SetData(new CSPointData(((BoundingBox)(ref boundingBox)).Min));
		CSOutputChannel outMaxX = OutMaxX;
		val = ((BoundingBox)(ref boundingBox)).Max;
		outMaxX.SetData(new CSNumberData(((Point3d)(ref val)).X));
		CSOutputChannel outMinX = OutMinX;
		val = ((BoundingBox)(ref boundingBox)).Min;
		outMinX.SetData(new CSNumberData(((Point3d)(ref val)).X));
		OutSizeX.SetData(new CSNumberData(number));
		CSOutputChannel outMaxY = OutMaxY;
		val = ((BoundingBox)(ref boundingBox)).Max;
		outMaxY.SetData(new CSNumberData(((Point3d)(ref val)).Y));
		CSOutputChannel outMinY = OutMinY;
		val = ((BoundingBox)(ref boundingBox)).Min;
		outMinY.SetData(new CSNumberData(((Point3d)(ref val)).Y));
		OutSizeY.SetData(new CSNumberData(number2));
		CSOutputChannel outMaxZ = OutMaxZ;
		val = ((BoundingBox)(ref boundingBox)).Max;
		outMaxZ.SetData(new CSNumberData(((Point3d)(ref val)).Z));
		CSOutputChannel outMinZ = OutMinZ;
		val = ((BoundingBox)(ref boundingBox)).Min;
		outMinZ.SetData(new CSNumberData(((Point3d)(ref val)).Z));
		OutSizeZ.SetData(new CSNumberData(number3));
		return base.Update();
	}
}
