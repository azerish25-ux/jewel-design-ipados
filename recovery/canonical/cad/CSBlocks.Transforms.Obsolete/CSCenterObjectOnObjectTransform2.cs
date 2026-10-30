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
[CSBlock("CenterObjectOnObjectTransform_V2", "{77CC3D9D-CBBE-4EE5-8D94-6B1621AE770B}")]
public sealed class CSCenterObjectOnObjectTransform2 : CSBlock
{
	public readonly CSOutputChannel OutTransform;

	public readonly CSInputChannel InObjectDestination;

	public readonly CSInputChannel InObjectSource;

	public readonly CSInputChannel InDoCenterOnX;

	public readonly CSInputChannel InDoCenterOnY;

	public readonly CSInputChannel InDoCenterOnZ;

	public CSCenterObjectOnObjectTransform2(string identifier)
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
			return UpdateMissing("Missing Destination Object");
		}
		CSData data2 = InObjectSource.Data;
		if (data2 == null)
		{
			return UpdateMissing("Missing Source Object");
		}
		bool value = true;
		InDoCenterOnX.GetBoolean(ref value);
		bool value2 = true;
		InDoCenterOnY.GetBoolean(ref value2);
		bool value3 = false;
		InDoCenterOnZ.GetBoolean(ref value3);
		CSDataHelper.GetTransformNoArray(data, out var value4);
		Transform xUp;
		if (value4.IsValid)
		{
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXform(value4, out xUp))
			{
				return UpdateError("GetUnscaledXform failed");
			}
		}
		else
		{
			xUp = Transform.Identity;
		}
		CSDataHelper.GetTransformNoArray(data2, out var value5);
		Transform xUp2;
		if (value5.IsValid)
		{
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXform(value5, out xUp2))
			{
				return UpdateError("GetUnscaledXform failed");
			}
		}
		else
		{
			xUp2 = Transform.Identity;
		}
		xUp.TryGetInverse(out var inverseTransform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox, doGrow: true, inverseTransform))
		{
			return UpdateMissing("GetTightBoundingBox failed (Destination)");
		}
		xUp2.TryGetInverse(out inverseTransform);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!data2.GetTightBoundingBox(ref boundingBox2, doGrow: true, inverseTransform))
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
