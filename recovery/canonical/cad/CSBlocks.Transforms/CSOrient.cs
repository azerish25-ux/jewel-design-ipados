using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("Orient", "{BE8C4A9C-42EB-44cd-B7BB-3AABF1FE7DD8}")]
public sealed class CSOrient : CSBlock
{
	public readonly CSInputChannel InDestinationPoint1;

	public readonly CSInputChannel InDestinationPoint2;

	public readonly CSInputChannel InDestinationPoint3;

	public readonly CSInputChannel InSourcePoint1;

	public readonly CSInputChannel InSourcePoint2;

	public readonly CSInputChannel InSourcePoint3;

	public readonly CSInputChannel InObject;

	public readonly CSInputChannel InScale;

	public readonly CSOutputChannel OutXform;

	public readonly CSOutputChannel OutObject;

	public CSOrient(string identifier)
		: base(identifier)
	{
		InDestinationPoint1 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InDestinationPoint2 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InDestinationPoint3 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InSourcePoint1 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InSourcePoint2 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InSourcePoint3 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		InScale = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		Point3d value = Point3d.Unset;
		Point3d value2 = Point3d.Unset;
		Point3d value3 = Point3d.Unset;
		CSDataHelper.GetPoint(InSourcePoint1, out value);
		CSDataHelper.GetPoint(InSourcePoint2, out value2);
		CSDataHelper.GetPoint(InSourcePoint3, out value3);
		Point3d value4 = Point3d.Unset;
		Point3d value5 = Point3d.Unset;
		Point3d value6 = Point3d.Unset;
		CSDataHelper.GetPoint(InDestinationPoint1, out value4);
		CSDataHelper.GetPoint(InDestinationPoint2, out value5);
		CSDataHelper.GetPoint(InDestinationPoint3, out value6);
		Transform transform = Transform.Identity;
		Transform transform2 = Transform.Identity;
		if (value != Point3d.Unset && value2 != Point3d.Unset && value3 != Point3d.Unset)
		{
			transform = CSBlockUtil.CSUtilMath.Transform3PointToOrigin(value, value2, value3);
		}
		else if (value != Point3d.Unset && value2 != Point3d.Unset)
		{
			transform = CSBlockUtil.CSUtilMath.Transform2PointToOrigin(value, value2);
		}
		if (value4 != Point3d.Unset && value5 != Point3d.Unset && value6 != Point3d.Unset)
		{
			transform2 = CSBlockUtil.CSUtilMath.TransformOriginTo3Point(value4, value5, value6);
		}
		else if (value4 != Point3d.Unset && value5 != Point3d.Unset)
		{
			transform2 = CSBlockUtil.CSUtilMath.TransformOriginTo2Point(value4, value5);
		}
		Transform transform3 = Transform.Identity;
		bool value7 = false;
		InScale.GetBoolean(ref value7);
		if (value7 && value4 != Point3d.Unset && value != Point3d.Unset && value5 != Point3d.Unset && value2 != Point3d.Unset)
		{
			Vector3d vector3d = value2 - value;
			Vector3d vector3d2 = value5 - value4;
			if (vector3d.Length > 0.0)
			{
				double num = vector3d2.Length / vector3d.Length;
				transform3 = Transform.Scale(Plane.WorldXY, num, num, num);
			}
		}
		Transform transform4 = transform2 * transform3 * transform;
		OutXform.SetData(new CSXformData(transform4));
		if (InObject != null)
		{
			CSData data = InObject.Data;
			if (data != null)
			{
				CSData data2 = data.Duplicate();
				CSDataHelper.Transform(data2, transform4);
				OutObject.SetData(data2);
			}
		}
		return base.Update();
	}
}
