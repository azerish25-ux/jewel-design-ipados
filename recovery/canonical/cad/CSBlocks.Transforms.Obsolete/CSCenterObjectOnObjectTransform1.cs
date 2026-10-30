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
		double dx = 0.0;
		if (value)
		{
			dx = boundingBox.Center.X - boundingBox2.Center.X;
		}
		double dy = 0.0;
		if (value2)
		{
			dy = boundingBox.Center.Y - boundingBox2.Center.Y;
		}
		double dz = 0.0;
		if (value3)
		{
			dz = boundingBox.Center.Z - boundingBox2.Center.Z;
		}
		Transform transform = Transform.Translation(dx, dy, dz);
		OutTransform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
