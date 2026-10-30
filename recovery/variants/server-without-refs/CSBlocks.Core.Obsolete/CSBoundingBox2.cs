using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core.Obsolete;

[Obsolete]
[CSBlock("BoundingBox_V2", "{7FCB7F8B-AE18-45ba-9214-71AB069103AF}")]
public sealed class CSBoundingBox2 : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iGrowX;

	public readonly CSInputChannel iGrowY;

	public readonly CSInputChannel iGrowZ;

	public readonly CSOutputChannel oMinPoint;

	public readonly CSOutputChannel oMaxPoint;

	public readonly CSOutputChannel oSizePoint;

	public readonly CSOutputChannel oSizeX;

	public readonly CSOutputChannel oSizeY;

	public readonly CSOutputChannel oSizeZ;

	public readonly CSOutputChannel oMaxZ;

	public readonly CSOutputChannel oLowerLeft;

	public readonly CSOutputChannel oLowerRight;

	public readonly CSOutputChannel oUpperLeft;

	public readonly CSOutputChannel oUpperRight;

	public readonly CSOutputChannel oMinZ;

	public readonly CSOutputChannel oMaxY;

	public readonly CSOutputChannel oMinY;

	public readonly CSOutputChannel oMaxX;

	public readonly CSOutputChannel oMinX;

	public readonly CSOutputChannel oCenter;

	public CSBoundingBox2(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iGrowX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGrowY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGrowZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMinPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oMaxPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oSizePoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oSizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMaxZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oLowerLeft = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oLowerRight = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oUpperLeft = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oUpperRight = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMinZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMaxY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMinY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMaxX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMinX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCenter = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		if (iObject == null)
		{
			return UpdateError("Bounding Box Null Input");
		}
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateError("Bounding Box Null Data");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateMissing("Missing geometry");
		}
		double value = 0.0;
		double value2 = 0.0;
		double value3 = 0.0;
		iGrowX?.GetDouble(ref value);
		iGrowY?.GetDouble(ref value2);
		iGrowZ?.GetDouble(ref value3);
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
		oMinPoint.SetData(new CSPointData(((BoundingBox)(ref boundingBox)).Min));
		oMaxPoint.SetData(new CSPointData(((BoundingBox)(ref boundingBox)).Max));
		oSizePoint.SetData(new CSPointData(new Point3d(((BoundingBox)(ref boundingBox)).Max - ((BoundingBox)(ref boundingBox)).Min)));
		CSOutputChannel cSOutputChannel = oSizeX;
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel.SetData(new CSNumberData(x - ((Point3d)(ref val)).X));
		CSOutputChannel cSOutputChannel2 = oSizeY;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel2.SetData(new CSNumberData(y - ((Point3d)(ref val)).Y));
		CSOutputChannel cSOutputChannel3 = oSizeZ;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double z = ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel3.SetData(new CSNumberData(z - ((Point3d)(ref val)).Z));
		CSOutputChannel cSOutputChannel4 = oMaxZ;
		val = ((BoundingBox)(ref boundingBox)).Max;
		cSOutputChannel4.SetData(new CSNumberData(((Point3d)(ref val)).Z));
		CSOutputChannel cSOutputChannel5 = oLowerLeft;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double x2 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double y2 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel5.SetData(new CSPointData(new Point3d(x2, y2, ((Point3d)(ref val)).Z)));
		CSOutputChannel cSOutputChannel6 = oLowerRight;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double x3 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double y3 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel6.SetData(new CSPointData(new Point3d(x3, y3, ((Point3d)(ref val)).Z)));
		CSOutputChannel cSOutputChannel7 = oUpperLeft;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double x4 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y4 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel7.SetData(new CSPointData(new Point3d(x4, y4, ((Point3d)(ref val)).Z)));
		CSOutputChannel cSOutputChannel8 = oUpperRight;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double x5 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y5 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel8.SetData(new CSPointData(new Point3d(x5, y5, ((Point3d)(ref val)).Z)));
		CSOutputChannel cSOutputChannel9 = oMinZ;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel9.SetData(new CSNumberData(((Point3d)(ref val)).Z));
		CSOutputChannel cSOutputChannel10 = oMaxY;
		val = ((BoundingBox)(ref boundingBox)).Max;
		cSOutputChannel10.SetData(new CSNumberData(((Point3d)(ref val)).Y));
		CSOutputChannel cSOutputChannel11 = oMinY;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel11.SetData(new CSNumberData(((Point3d)(ref val)).Y));
		CSOutputChannel cSOutputChannel12 = oMaxX;
		val = ((BoundingBox)(ref boundingBox)).Max;
		cSOutputChannel12.SetData(new CSNumberData(((Point3d)(ref val)).X));
		CSOutputChannel cSOutputChannel13 = oMinX;
		val = ((BoundingBox)(ref boundingBox)).Min;
		cSOutputChannel13.SetData(new CSNumberData(((Point3d)(ref val)).X));
		oCenter.SetData(new CSPointData(((BoundingBox)(ref boundingBox)).Center));
		return base.Update();
	}
}
