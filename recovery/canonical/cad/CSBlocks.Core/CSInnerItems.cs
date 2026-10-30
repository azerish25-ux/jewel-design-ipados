using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("InnerItems", "{831D6CD2-8B62-46dd-A029-947D7CC5F9E5}")]
public sealed class CSInnerItems : CSBlock
{
	public CSInputChannel iObject;

	public CSOutputChannel oMinPoint;

	public CSOutputChannel oMaxPoint;

	public CSOutputChannel oSizePoint;

	public CSOutputChannel oSizeX;

	public CSOutputChannel oSizeY;

	public CSOutputChannel oSizeZ;

	public CSOutputChannel oMaxZ;

	public CSOutputChannel oObjects;

	public CSInnerItems(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		oMinPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oMaxPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oSizePoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oSizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMaxZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateMissing("Bounding Box Null Data");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Bound Box failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < CSDataHelper.GetArrayCount(data); i++)
		{
			CSData arrayItem = CSDataHelper.GetArrayItem(data, i);
			BoundingBox boundingBox2 = BoundingBox.Unset;
			arrayItem.GetTightBoundingBox(ref boundingBox2);
			double num = 0.0;
			double num2 = 0.0;
			if (boundingBox.Max.X != boundingBox.Min.X)
			{
				num2 += 1.0;
				if (boundingBox2.Max.X == boundingBox2.Min.X)
				{
					if (boundingBox.Max.X == boundingBox2.Max.X)
					{
						num += 1.0;
					}
					else if (boundingBox.Min.X == boundingBox2.Min.X)
					{
						num += 1.0;
					}
				}
				else if (boundingBox.Max.X == boundingBox2.Max.X && boundingBox.Min.X == boundingBox2.Min.X)
				{
					num += 1.0;
				}
			}
			if (boundingBox.Max.Y != boundingBox.Min.Y)
			{
				num2 += 1.0;
				if (boundingBox2.Max.Y == boundingBox2.Min.Y)
				{
					if (boundingBox.Max.Y == boundingBox2.Max.Y)
					{
						num += 1.0;
					}
					else if (boundingBox.Min.Y == boundingBox2.Min.Y)
					{
						num += 1.0;
					}
				}
				else if (boundingBox.Max.Y == boundingBox2.Max.Y && boundingBox.Min.Y == boundingBox2.Min.Y)
				{
					num += 1.0;
				}
			}
			if (boundingBox.Max.Z != boundingBox.Min.Z)
			{
				num2 += 1.0;
				if (boundingBox2.Max.Z == boundingBox2.Min.Z)
				{
					if (boundingBox.Max.Z == boundingBox2.Max.Z)
					{
						num += 1.0;
					}
					else if (boundingBox.Min.Z == boundingBox2.Min.Z)
					{
						num += 1.0;
					}
				}
				else if (boundingBox.Max.Z == boundingBox2.Max.Z && boundingBox.Min.Z == boundingBox2.Min.Z)
				{
					num += 1.0;
				}
			}
			if (num2 > 0.0 && num / num2 < 0.5)
			{
				cSDataArray.Append(arrayItem);
			}
		}
		boundingBox = BoundingBox.Unset;
		cSDataArray.GetTightBoundingBox(ref boundingBox);
		oMinPoint.SetData(new CSPointData(boundingBox.Min));
		oMaxPoint.SetData(new CSPointData(boundingBox.Max));
		oSizePoint.SetData(new CSPointData(new Point3d(boundingBox.Max - boundingBox.Min)));
		oSizeX.SetData(new CSNumberData(boundingBox.Max.X - boundingBox.Min.X));
		oSizeY.SetData(new CSNumberData(boundingBox.Max.Y - boundingBox.Min.Y));
		oSizeZ.SetData(new CSNumberData(boundingBox.Max.Z - boundingBox.Min.Z));
		oMaxZ.SetData(new CSNumberData(boundingBox.Max.Z));
		oObjects.SetData(cSDataArray);
		return base.Update();
	}
}
