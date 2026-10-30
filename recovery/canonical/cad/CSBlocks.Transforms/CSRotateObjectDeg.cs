using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("RotateObjectDeg", "{9D1008F4-BD55-4288-A80D-7CED7A1E83B2}")]
public sealed class CSRotateObjectDeg : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSInputChannel iObjectCenter;

	public readonly CSOutputChannel oObject;

	public CSRotateObjectDeg(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iObjectCenter = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateMissing("TranslateObject: Missing Object to translate");
		}
		double value = 0.0;
		iX.GetDouble(ref value);
		double value2 = 0.0;
		iY.GetDouble(ref value2);
		double value3 = 0.0;
		iZ.GetDouble(ref value3);
		Point3d rotationCenter = Point3d.Origin;
		bool value4 = false;
		if (iObjectCenter != null)
		{
			iObjectCenter.GetBoolean(ref value4);
		}
		if (value4)
		{
			BoundingBox boundingBox = BoundingBox.Unset;
			data.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity);
			rotationCenter = boundingBox.Center;
		}
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value), Vector3d.XAxis, rotationCenter);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value2), Vector3d.YAxis, rotationCenter);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value3), Vector3d.ZAxis, rotationCenter);
		Transform transform4 = transform * transform2 * transform3;
		CSData cSData = data.Duplicate();
		if (cSData == null)
		{
			return base.Update();
		}
		CSDataHelper.Transform(cSData, transform4);
		oObject.SetData(cSData);
		return base.Update();
	}
}
