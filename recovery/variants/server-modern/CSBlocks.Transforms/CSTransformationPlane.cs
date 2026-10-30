using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("TransformationPlane", "{4E5E4DA5-9019-4066-867E-055A6265A776}")]
public sealed class CSTransformationPlane : CSBlock
{
	public readonly CSInputChannel iBaseXform;

	public readonly CSInputChannel iOffsetX;

	public readonly CSInputChannel iOffsetY;

	public readonly CSInputChannel iOffsetZ;

	public readonly CSInputChannel iRotationX;

	public readonly CSInputChannel iRotationY;

	public readonly CSInputChannel iRotationZ;

	public readonly CSOutputChannel oPlane;

	public CSTransformationPlane(string identifier)
		: base(identifier)
	{
		iBaseXform = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iOffsetX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOffsetY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOffsetZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotationX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotationY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotationZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oPlane = new CSOutputChannel(this, ChannelDataType.Plane, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Transform value = Transform.Unset;
		if (iBaseXform != null)
		{
			CSDataArray flattenArray = iBaseXform.GetFlattenArray();
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out value);
				if (((Transform)(ref value)).IsValid)
				{
					break;
				}
			}
		}
		if (!((Transform)(ref value)).IsValid)
		{
			value = Transform.Identity;
		}
		double value2 = 0.0;
		double value3 = 0.0;
		double value4 = 0.0;
		iOffsetX.GetDouble(ref value2);
		iOffsetY.GetDouble(ref value3);
		iOffsetZ.GetDouble(ref value4);
		Transform val = Transform.Translation(value2, value3, value4);
		double value5 = 0.0;
		iRotationX.GetDouble(ref value5);
		double value6 = 0.0;
		iRotationY.GetDouble(ref value6);
		double value7 = 0.0;
		iRotationZ.GetDouble(ref value7);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value5), Vector3d.XAxis, Point3d.Origin);
		Transform val3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value6), Vector3d.YAxis, Point3d.Origin);
		Transform val4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value7), Vector3d.ZAxis, Point3d.Origin);
		oPlane.SetData(new CSPlaneData(new CSXformData(value * val * val4 * val3 * val2)));
		return base.Update();
	}
}
