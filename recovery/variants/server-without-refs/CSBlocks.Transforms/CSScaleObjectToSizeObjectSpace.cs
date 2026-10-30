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
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
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
		if (!((Transform)(ref value)).IsValid)
		{
			CSDataArray flattenArray = CSDataHelper.GetFlattenArray(data);
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out value);
				if (((Transform)(ref value)).IsValid)
				{
					break;
				}
			}
		}
		Transform xUp;
		Transform xDown = default(Transform);
		if (((Transform)(ref value)).IsValid)
		{
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value, out xUp, out xDown))
			{
				xUp = Transform.Identity;
				((Transform)(ref xUp)).TryGetInverse(ref xDown);
			}
		}
		else
		{
			xUp = Transform.Identity;
			((Transform)(ref xUp)).TryGetInverse(ref xDown);
		}
		CSData cSData = data.Duplicate();
		CSDataHelper.Transform(cSData, xDown);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("CSScaleObjectToSizeObjectSpace: Unable to calculate bounding box\n");
		}
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num = x - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = y - ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double z = ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num3 = z - ((Point3d)(ref val)).Z;
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
		double num4 = 1.0;
		double num5 = 1.0;
		double num6 = 1.0;
		double value2 = 0.0;
		double value3 = 0.0;
		double value4 = 0.0;
		if (iX != null && iX.GetDouble(ref value2))
		{
			num4 = value2 / num;
		}
		if (iY != null && iY.GetDouble(ref value3))
		{
			num5 = value3 / num2;
		}
		if (iZ != null && iZ.GetDouble(ref value4))
		{
			num6 = value4 / num3;
		}
		Transform val2 = Transform.Scale(Plane.WorldXY, num4, num5, num6);
		CSDataHelper.Transform(cSData, xUp * val2);
		oObj.SetData(cSData);
		return base.Update();
	}
}
