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
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
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
			return UpdateError("CSScaleObjectToSizeObjectSpace: Unable to calculate bounding box\n");
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
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, xUp * val2 * xDown);
		oObj.SetData(data2);
		return base.Update();
	}
}
