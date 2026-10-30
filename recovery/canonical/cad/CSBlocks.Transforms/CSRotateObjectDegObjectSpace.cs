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
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value4);
			if (!value4.IsValid)
			{
				for (int j = 0; j < CSDataHelper.GetArrayCount(arrayItem); j++)
				{
					CSDataHelper.GetTransformNoArray(CSDataHelper.GetArrayItem(arrayItem, i), out value4);
					if (value4.IsValid)
					{
						break;
					}
				}
			}
			Transform xUp;
			Transform inverseTransform;
			if (value4.IsValid)
			{
				if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value4, out xUp, out inverseTransform))
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
			Transform transform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value), Vector3d.XAxis, Point3d.Origin);
			Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value2), Vector3d.YAxis, Point3d.Origin);
			Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value3), Vector3d.ZAxis, Point3d.Origin);
			Transform transform4 = xUp * transform * transform2 * transform3 * inverseTransform;
			CSData cSData = arrayItem.Duplicate();
			if (cSData != null)
			{
				CSDataHelper.Transform(cSData, transform4);
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
