using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("CenterAndScaleObjectToSize", "{A19D1A06-0BBD-4CF2-863E-C0C10B94095D}")]
public sealed class CSCenterAndScaleObjectToSize : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oObj;

	public CSCenterAndScaleObjectToSize(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObj = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateMissing("Object");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity))
		{
			return UpdateError("Bounding Box");
		}
		double num = boundingBox.Max.X - boundingBox.Min.X;
		double num2 = boundingBox.Max.Y - boundingBox.Min.Y;
		double num3 = boundingBox.Max.Z - boundingBox.Min.Z;
		if (num < 0.01)
		{
			num = 1.0;
		}
		if (num2 < 0.01)
		{
			num2 = 1.0;
		}
		if (num3 < 0.01)
		{
			num3 = 1.0;
		}
		double value = num;
		if (iX != null)
		{
			iX.GetDouble(ref value);
		}
		double value2 = num2;
		if (iY != null)
		{
			iY.GetDouble(ref value2);
		}
		double value3 = num3;
		if (iZ != null)
		{
			iZ.GetDouble(ref value3);
		}
		Transform transform = Transform.Scale(Plane.WorldXY, value / num, value2 / num2, value3 / num3);
		Transform transform2 = Transform.Translation(0.0 - boundingBox.Center.X, 0.0 - boundingBox.Center.Y, 0.0 - boundingBox.Center.Z);
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, transform * transform2);
		oObj.SetData(data2);
		return base.Update();
	}
}
