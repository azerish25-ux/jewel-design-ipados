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
		Point3d min = boundingBox.Min;
		min.X -= value;
		min.Y -= value2;
		min.Z -= value3;
		boundingBox.Min = min;
		Point3d max = boundingBox.Max;
		max.X += value;
		max.Y += value2;
		max.Z += value3;
		boundingBox.Max = max;
		oMinPoint.SetData(new CSPointData(boundingBox.Min));
		oMaxPoint.SetData(new CSPointData(boundingBox.Max));
		oSizePoint.SetData(new CSPointData(new Point3d(boundingBox.Max - boundingBox.Min)));
		oSizeX.SetData(new CSNumberData(boundingBox.Max.X - boundingBox.Min.X));
		oSizeY.SetData(new CSNumberData(boundingBox.Max.Y - boundingBox.Min.Y));
		oSizeZ.SetData(new CSNumberData(boundingBox.Max.Z - boundingBox.Min.Z));
		oMaxZ.SetData(new CSNumberData(boundingBox.Max.Z));
		oLowerLeft.SetData(new CSPointData(new Point3d(boundingBox.Min.X, boundingBox.Min.Y, boundingBox.Min.Z)));
		oLowerRight.SetData(new CSPointData(new Point3d(boundingBox.Max.X, boundingBox.Min.Y, boundingBox.Min.Z)));
		oUpperLeft.SetData(new CSPointData(new Point3d(boundingBox.Min.X, boundingBox.Max.Y, boundingBox.Min.Z)));
		oUpperRight.SetData(new CSPointData(new Point3d(boundingBox.Max.X, boundingBox.Max.Y, boundingBox.Min.Z)));
		oMinZ.SetData(new CSNumberData(boundingBox.Min.Z));
		oMaxY.SetData(new CSNumberData(boundingBox.Max.Y));
		oMinY.SetData(new CSNumberData(boundingBox.Min.Y));
		oMaxX.SetData(new CSNumberData(boundingBox.Max.X));
		oMinX.SetData(new CSNumberData(boundingBox.Min.X));
		oCenter.SetData(new CSPointData(boundingBox.Center));
		return base.Update();
	}
}
