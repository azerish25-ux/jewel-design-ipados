using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("RotateObjectDegObjectSpace", "{B037DF22-B467-44A7-9F06-4CB930D98D0B}")]
public sealed class CSRotateObjectDegObjectSpace : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oObject;

	public CSRotateObjectDegObjectSpace(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		iX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateMissing("RotateObjectDegObjectSpace: Missing Object to translate");
		}
		double value = 0.0;
		iX.GetDouble(ref value);
		double value2 = 0.0;
		iY.GetDouble(ref value2);
		double value3 = 0.0;
		iZ.GetDouble(ref value3);
		CSDataArray flattenArray = CSDataHelper.GetFlattenArray(data);
		CSDataArray cSDataArray = new CSDataArray();
		Transform xDown = default(Transform);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value4);
			if (!((Transform)(ref value4)).IsValid)
			{
				for (int j = 0; j < CSDataHelper.GetArrayCount(arrayItem); j++)
				{
					CSDataHelper.GetTransformNoArray(CSDataHelper.GetArrayItem(arrayItem, i), out value4);
					if (((Transform)(ref value4)).IsValid)
					{
						break;
					}
				}
			}
			Transform xUp;
			if (((Transform)(ref value4)).IsValid)
			{
				if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value4, out xUp, out xDown))
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
			Transform val = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value), Vector3d.XAxis, Point3d.Origin);
			Transform val2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value2), Vector3d.YAxis, Point3d.Origin);
			Transform val3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value3), Vector3d.ZAxis, Point3d.Origin);
			Transform transform = xUp * val * val2 * val3 * xDown;
			CSData cSData = arrayItem.Duplicate();
			if (cSData != null)
			{
				CSDataHelper.Transform(cSData, transform);
				cSDataArray.Append(cSData);
			}
		}
		if (cSDataArray.GetArrayCount() > 0)
		{
			oObject.SetData(cSDataArray);
		}
		return base.Update();
	}
}
