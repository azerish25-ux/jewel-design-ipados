using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("ScaleObjectToSizeXform", "{E80ED2A1-D415-4ded-B1E8-B8557AC37E9C}")]
public sealed class CSScaleObjectToSizeXform : CSBlock
{
	public readonly CSInputChannel iObj;

	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oXform;

	public CSScaleObjectToSizeXform(string identifier)
		: base(identifier)
	{
		iObj = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
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
			return UpdateMissing("Object");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity))
		{
			return UpdateMissing("Bounding Box");
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
		oXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
