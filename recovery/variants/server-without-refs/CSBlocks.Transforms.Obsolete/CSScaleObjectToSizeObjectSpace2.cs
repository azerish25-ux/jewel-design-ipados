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
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
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
		if (!((Transform)(ref value)).IsValid)
		{
			CSDataArray flattenArray = CSDataHelper.GetFlattenArray(data);
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out var value2);
				if (((Transform)(ref value2)).IsValid)
				{
					value = value2;
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
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("CSScaleObjectToSizeObjectSpace: Unable to calculate bounding box");
		}
		((BoundingBox)(ref boundingBox)).Transform(xDown);
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
		double value3 = 0.0;
		double value4 = 0.0;
		double value5 = 0.0;
		if (iX != null && iX.GetDouble(ref value3))
		{
			num4 = value3 / num;
		}
		if (iY != null && iY.GetDouble(ref value4))
		{
			num5 = value4 / num2;
		}
		if (iZ != null && iZ.GetDouble(ref value5))
		{
			num6 = value5 / num3;
		}
		Transform val2 = Transform.Scale(Plane.WorldXY, num4, num5, num6);
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, xUp * val2 * xDown);
		oObj.SetData(data2);
		return base.Update();
	}
}
