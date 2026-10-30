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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
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
		Transform val = Transform.Identity;
		Transform val2 = Transform.Identity;
		if (value != Point3d.Unset && value2 != Point3d.Unset && value3 != Point3d.Unset)
		{
			val = CSBlockUtil.CSUtilMath.Transform3PointToOrigin(value, value2, value3);
		}
		else if (value != Point3d.Unset && value2 != Point3d.Unset)
		{
			val = CSBlockUtil.CSUtilMath.Transform2PointToOrigin(value, value2);
		}
		if (value4 != Point3d.Unset && value5 != Point3d.Unset && value6 != Point3d.Unset)
		{
			val2 = CSBlockUtil.CSUtilMath.TransformOriginTo3Point(value4, value5, value6);
		}
		else if (value4 != Point3d.Unset && value5 != Point3d.Unset)
		{
			val2 = CSBlockUtil.CSUtilMath.TransformOriginTo2Point(value4, value5);
		}
		Transform val3 = Transform.Identity;
		bool value7 = false;
		InScale.GetBoolean(ref value7);
		if (value7 && value4 != Point3d.Unset && value != Point3d.Unset && value5 != Point3d.Unset && value2 != Point3d.Unset)
		{
			Vector3d val4 = value2 - value;
			Vector3d val5 = value5 - value4;
			if (((Vector3d)(ref val4)).Length > 0.0)
			{
				double num = ((Vector3d)(ref val5)).Length / ((Vector3d)(ref val4)).Length;
				val3 = Transform.Scale(Plane.WorldXY, num, num, num);
			}
		}
		Transform transform = val2 * val3 * val;
		OutXform.SetData(new CSXformData(transform));
		if (InObject != null)
		{
			CSData data = InObject.Data;
			if (data != null)
			{
				CSData data2 = data.Duplicate();
				CSDataHelper.Transform(data2, transform);
				OutObject.SetData(data2);
			}
		}
		return base.Update();
	}
}
