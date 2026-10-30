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
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
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
			rotationCenter = ((BoundingBox)(ref boundingBox)).Center;
		}
		Transform val = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value), Vector3d.XAxis, rotationCenter);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value2), Vector3d.YAxis, rotationCenter);
		Transform val3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value3), Vector3d.ZAxis, rotationCenter);
		Transform transform = val * val2 * val3;
		CSData cSData = data.Duplicate();
		if (cSData == null)
		{
			return base.Update();
		}
		CSDataHelper.Transform(cSData, transform);
		oObject.SetData(cSData);
		return base.Update();
	}
}
