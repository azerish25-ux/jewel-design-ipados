using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms.Obsolete;

[Obsolete]
[CSBlock("CenterObjectOnObjectTransform_V1", "{3997C14B-6ABC-4536-A363-E9A10969BA70}")]
public sealed class CSCenterObjectOnObjectTransform1 : CSBlock
{
	public readonly CSInputChannel InObjectDestination;

	public readonly CSInputChannel InObjectSource;

	public readonly CSInputChannel InDoCenterOnX;

	public readonly CSInputChannel InDoCenterOnY;

	public readonly CSInputChannel InDoCenterOnZ;

	public readonly CSOutputChannel OutTransform;

	public CSCenterObjectOnObjectTransform1(string identifier)
		: base(identifier)
	{
		InObjectDestination = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InObjectSource = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InDoCenterOnX = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDoCenterOnY = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDoCenterOnZ = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutTransform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InObjectDestination.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Center");
		}
		CSData data2 = InObjectSource.Data;
		if (data2 == null)
		{
			return UpdateMissing("Missing Object");
		}
		bool value = true;
		InDoCenterOnX.GetBoolean(ref value);
		bool value2 = true;
		InDoCenterOnY.GetBoolean(ref value2);
		bool value3 = true;
		InDoCenterOnZ.GetBoolean(ref value3);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateMissing("GetTightBoundingBox failed (Destination)");
		}
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!data2.GetTightBoundingBox(ref boundingBox2))
		{
			return UpdateMissing("GetTightBoundingBox failed (Source)");
		}
		double num = 0.0;
		Point3d center;
		if (value)
		{
			center = ((BoundingBox)(ref boundingBox)).Center;
			double x = ((Point3d)(ref center)).X;
			center = ((BoundingBox)(ref boundingBox2)).Center;
			num = x - ((Point3d)(ref center)).X;
		}
		double num2 = 0.0;
		if (value2)
		{
			center = ((BoundingBox)(ref boundingBox)).Center;
			double y = ((Point3d)(ref center)).Y;
			center = ((BoundingBox)(ref boundingBox2)).Center;
			num2 = y - ((Point3d)(ref center)).Y;
		}
		double num3 = 0.0;
		if (value3)
		{
			center = ((BoundingBox)(ref boundingBox)).Center;
			double z = ((Point3d)(ref center)).Z;
			center = ((BoundingBox)(ref boundingBox2)).Center;
			num3 = z - ((Point3d)(ref center)).Z;
		}
		Transform transform = Transform.Translation(num, num2, num3);
		OutTransform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
