using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("CenterAndScaleObjectToSizeXform", "{433D2D9E-B499-439f-9570-0C689657F0E4}")]
public sealed class CSCenterAndScaleObjectToSizeXform : CSBlock
{
	public readonly CSInputChannel iObj;

	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oXform;

	public CSCenterAndScaleObjectToSizeXform(string identifier)
		: base(identifier)
	{
		iObj = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = iObj.Data;
		if (data == null)
		{
			return UpdateMissing("Object to scale");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox))
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
		iX.GetDouble(ref value);
		double value2 = num2;
		iY.GetDouble(ref value2);
		double value3 = num3;
		iZ.GetDouble(ref value3);
		Transform transform = Transform.Scale(Plane.WorldXY, value / num, value2 / num2, value3 / num3);
		Transform transform2 = Transform.Translation(0.0 - boundingBox.Center.X, 0.0 - boundingBox.Center.Y, 0.0 - boundingBox.Center.Z);
		oXform.SetData(new CSXformData(transform * transform2));
		return base.Update();
	}
}
