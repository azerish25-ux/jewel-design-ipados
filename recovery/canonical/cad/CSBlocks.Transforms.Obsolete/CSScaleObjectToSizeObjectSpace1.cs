using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms.Obsolete;

[Obsolete]
[CSBlock("ScaleObjectToSizeObjectSpace_V1", "{EA011BB2-BF6C-4aeb-9402-6067452E3E91}")]
public sealed class CSScaleObjectToSizeObjectSpace1 : CSBlock
{
	public CSInputChannel iObj { get; }

	public CSInputChannel iX { get; }

	public CSInputChannel iY { get; }

	public CSInputChannel iZ { get; }

	public CSOutputChannel oObj { get; }

	public CSScaleObjectToSizeObjectSpace1(string identifier)
		: base(identifier)
	{
		iObj = new CSInputChannel(GetPropertyName(() => iObj), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iX = new CSInputChannel(GetPropertyName(() => iX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(GetPropertyName(() => iY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(GetPropertyName(() => iZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObj = new CSOutputChannel(GetPropertyName(() => oObj), this, ChannelDataType.Any, ChannelArrayType.Any);
	}

	public override bool Update()
	{
		if (iObj == null)
		{
			return UpdateMissing("CSScaleObjectToSizeObjectSpace: Missing Object\n");
		}
		CSData data = iObj.Data;
		if (data == null)
		{
			return UpdateMissing("CSScaleObjectToSizeObjectSpace: Missing Object Data\n");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		Transform xUp;
		Transform inverseTransform;
		if (value.IsValid)
		{
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value, out xUp, out inverseTransform))
			{
				xUp = Transform.Identity;
				xUp.TryGetInverse(out inverseTransform);
			}
		}
		else
		{
			xUp = Transform.Identity;
			xUp.TryGetInverse(out inverseTransform);
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("CSScaleObjectToSizeObjectSpace: Unable to calculate bounding box\n");
		}
		boundingBox.Transform(inverseTransform);
		double num = boundingBox.Max.X - boundingBox.Min.X;
		double num2 = boundingBox.Max.Y - boundingBox.Min.Y;
		double num3 = boundingBox.Max.Z - boundingBox.Min.Z;
		if (num3 < 0.001)
		{
			num3 = 1.0;
		}
		if (num2 < 0.001)
		{
			num2 = 1.0;
		}
		if (num < 0.001)
		{
			num = 1.0;
		}
		double xScaleFactor = 1.0;
		double yScaleFactor = 1.0;
		double zScaleFactor = 1.0;
		double value2 = 0.0;
		double value3 = 0.0;
		double value4 = 0.0;
		if (iX != null && iX.GetDouble(ref value2))
		{
			xScaleFactor = value2 / num;
		}
		if (iY != null && iY.GetDouble(ref value3))
		{
			yScaleFactor = value3 / num2;
		}
		if (iZ != null && iZ.GetDouble(ref value4))
		{
			zScaleFactor = value4 / num3;
		}
		Transform transform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, xUp * transform * inverseTransform);
		oObj.SetData(data2);
		return base.Update();
	}
}
