using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("BoundingBox", "{D23D023D-8DBB-49EF-9AEA-9583D38DF148}")]
public sealed class CSBoundingBox : CSBlock
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

	public CSBoundingBox(string identifier)
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
		if (!data.GetTightBoundingBoxNew(ref boundingBox))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
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
		double number = boundingBox.Max.X - boundingBox.Min.X;
		double number2 = boundingBox.Max.Y - boundingBox.Min.Y;
		double number3 = boundingBox.Max.Z - boundingBox.Min.Z;
		OutCenter.SetData(new CSPointData(boundingBox.Center));
		OutMax.SetData(new CSPointData(boundingBox.Max));
		OutMin.SetData(new CSPointData(boundingBox.Min));
		OutMaxX.SetData(new CSNumberData(boundingBox.Max.X));
		OutMinX.SetData(new CSNumberData(boundingBox.Min.X));
		OutSizeX.SetData(new CSNumberData(number));
		OutMaxY.SetData(new CSNumberData(boundingBox.Max.Y));
		OutMinY.SetData(new CSNumberData(boundingBox.Min.Y));
		OutSizeY.SetData(new CSNumberData(number2));
		OutMaxZ.SetData(new CSNumberData(boundingBox.Max.Z));
		OutMinZ.SetData(new CSNumberData(boundingBox.Min.Z));
		OutSizeZ.SetData(new CSNumberData(number3));
		return base.Update();
	}
}
