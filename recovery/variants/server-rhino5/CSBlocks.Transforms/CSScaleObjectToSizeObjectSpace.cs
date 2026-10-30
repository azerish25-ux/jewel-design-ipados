using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[Obsolete]
[CSBlock("ScaleObjectToSizeObjectSpace", "{C3E66EBA-4810-4303-9EEE-B061A30A279A}")]
public sealed class CSScaleObjectToSizeObjectSpace : CSBlock
{
	public readonly CSInputChannel iObj;

	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oObj;

	public CSScaleObjectToSizeObjectSpace(string identifier)
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
			return UpdateMissing("CSScaleObjectToSizeObjectSpace: Missing Object\n");
		}
		CSData data = iObj.Data;
		if (data == null)
		{
			return UpdateMissing("CSScaleObjectToSizeObjectSpace: Missing Object Data\n");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!value.IsValid)
		{
			CSDataArray flattenArray = CSDataHelper.GetFlattenArray(data);
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out value);
				if (value.IsValid)
				{
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
		CSData cSData = data.Duplicate();
		CSDataHelper.Transform(cSData, inverseTransform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("CSScaleObjectToSizeObjectSpace: Unable to calculate bounding box\n");
		}
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
		CSDataHelper.Transform(cSData, xUp * transform);
		oObj.SetData(cSData);
		return base.Update();
	}
}
