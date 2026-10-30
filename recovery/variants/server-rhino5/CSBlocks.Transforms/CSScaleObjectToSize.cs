using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("ScaleObjectToSize", "{65CD6296-1BB8-4fec-A6B6-ADFB74BC8517}")]
public sealed class CSScaleObjectToSize : CSBlock
{
	public readonly CSInputChannel iObj;

	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oObj;

	public CSScaleObjectToSize(string identifier)
		: base(identifier)
	{
		iObj = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObj = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (iObj == null)
		{
			return UpdateMissing("CSScaleObjectToSize: Bad Object\n");
		}
		CSData data = iObj.Data;
		if (data == null)
		{
			return UpdateMissing("CSScaleObjectToSize: Bad Object\n");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("CSScaleObjectToSize: Unable to calculate bounding box\n");
		}
		double num = boundingBox.Max.X - boundingBox.Min.X;
		double num2 = boundingBox.Max.Y - boundingBox.Min.Y;
		double num3 = boundingBox.Max.Z - boundingBox.Min.Z;
		if (num < 0.001)
		{
			num = 1.0;
		}
		if (num2 < 0.001)
		{
			num2 = 1.0;
		}
		if (num3 < 0.001)
		{
			num3 = 1.0;
		}
		double xScaleFactor = 1.0;
		double yScaleFactor = 1.0;
		double zScaleFactor = 1.0;
		double value = 0.0;
		double value2 = 0.0;
		double value3 = 0.0;
		if (iX != null && iX.GetDouble(ref value))
		{
			xScaleFactor = value / num;
		}
		if (iY != null && iY.GetDouble(ref value2))
		{
			yScaleFactor = value2 / num2;
		}
		if (iZ != null && iZ.GetDouble(ref value3))
		{
			zScaleFactor = value3 / num3;
		}
		Transform transform = default(Transform);
		transform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, transform);
		oObj.SetData(data2);
		return base.Update();
	}
}
