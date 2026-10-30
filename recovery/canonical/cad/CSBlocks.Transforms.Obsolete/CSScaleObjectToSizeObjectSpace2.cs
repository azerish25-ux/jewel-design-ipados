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
[CSBlock("ScaleObjectToSizeObjectSpace_V2", "{FDA448B8-955E-4567-9B8D-592FC26DE18F}")]
public sealed class CSScaleObjectToSizeObjectSpace2 : CSBlock
{
	public CSInputChannel iObj { get; }

	public CSInputChannel iX { get; }

	public CSInputChannel iY { get; }

	public CSInputChannel iZ { get; }

	public CSOutputChannel oObj { get; }

	public CSScaleObjectToSizeObjectSpace2(string identifier)
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
			return UpdateMissing("CSScaleObjectToSizeObjectSpace: Missing Object");
		}
		CSData data = iObj.Data;
		if (data == null)
		{
			return UpdateMissing("CSScaleObjectToSizeObjectSpace: Missing Object Data");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!value.IsValid)
		{
			CSDataArray flattenArray = CSDataHelper.GetFlattenArray(data);
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out var value2);
				if (value2.IsValid)
				{
					value = value2;
					break;
				}
			}
		}
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
			return UpdateError("CSScaleObjectToSizeObjectSpace: Unable to calculate bounding box");
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
		double value3 = 0.0;
		double value4 = 0.0;
		double value5 = 0.0;
		if (iX != null && iX.GetDouble(ref value3))
		{
			xScaleFactor = value3 / num;
		}
		if (iY != null && iY.GetDouble(ref value4))
		{
			yScaleFactor = value4 / num2;
		}
		if (iZ != null && iZ.GetDouble(ref value5))
		{
			zScaleFactor = value5 / num3;
		}
		Transform transform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, xUp * transform * inverseTransform);
		oObj.SetData(data2);
		return base.Update();
	}
}
