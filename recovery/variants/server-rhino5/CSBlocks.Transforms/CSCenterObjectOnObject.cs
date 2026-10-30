using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("CenterObjectOnObject", "{7882D564-7162-4776-9FC8-67532430F6A8}")]
public sealed class CSCenterObjectOnObject : CSBlock
{
	public readonly CSOutputChannel OutObject;

	public readonly CSInputChannel InObjectSource;

	public readonly CSInputChannel InObjectDestination;

	public readonly CSInputChannel InEnableCenterOnObjectX;

	public readonly CSInputChannel InEnableCenterOnObjectY;

	public readonly CSInputChannel InEnableCenterOnObjectZ;

	public CSCenterObjectOnObject(string identifier)
		: base(identifier)
	{
		InObjectSource = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InObjectDestination = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InEnableCenterOnObjectX = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnableCenterOnObjectY = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnableCenterOnObjectZ = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = InObjectSource.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Source Object");
		}
		CSData data2 = InObjectDestination.Data;
		if (data2 == null)
		{
			return UpdateMissing("Missing Destination Object");
		}
		bool value = true;
		InEnableCenterOnObjectX.GetBoolean(ref value);
		bool value2 = true;
		InEnableCenterOnObjectY.GetBoolean(ref value2);
		bool value3 = false;
		InEnableCenterOnObjectZ.GetBoolean(ref value3);
		CSDataHelper.GetTransformNoArray(data, out var value4);
		Transform xDown;
		Transform xUp;
		if (!value4.IsValid || value4.Equals(Transform.Identity))
		{
			BoundingBox boundingBox = BoundingBox.Unset;
			if (!data.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity))
			{
				return UpdateError("GetTightBoundingBox failed (Source");
			}
			Transform.Translation(boundingBox.Center - Point3d.Origin).TryGetInverse(out xDown);
		}
		else if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value4, out xUp, out xDown))
		{
			return UpdateError("GetUnscaledXform failed");
		}
		CSDataHelper.GetTransformNoArray(data2, out var value5);
		Transform xUp2;
		Transform xDown2;
		if (!value5.IsValid || value5.Equals(Transform.Identity))
		{
			BoundingBox boundingBox2 = BoundingBox.Unset;
			if (!data2.GetTightBoundingBox(ref boundingBox2, doGrow: true, Transform.Identity))
			{
				return UpdateError("GetTightBoundingBox failed (Destination)");
			}
			xUp2 = Transform.Translation(boundingBox2.Center - Point3d.Origin);
			xUp2.TryGetInverse(out xDown2);
		}
		else if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value5, out xUp2, out xDown2))
		{
			return UpdateError("GetUnscaledXform failed");
		}
		CSData cSData = data2.Duplicate();
		CSDataHelper.Transform(cSData, xDown2);
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox3, doGrow: true, Transform.Identity))
		{
			return UpdateError("GetTightBoundingBox failed (Destination)");
		}
		double dx = 0.0;
		if (value)
		{
			dx = boundingBox3.Center.X;
		}
		double dy = 0.0;
		if (value2)
		{
			dy = boundingBox3.Center.Y;
		}
		double dz = 0.0;
		if (value3)
		{
			dz = boundingBox3.Center.Z;
		}
		Transform transform = Transform.Translation(dx, dy, dz);
		Transform transform2 = xUp2 * transform;
		CSData data3 = data.Duplicate();
		CSDataHelper.Transform(data3, transform2);
		OutObject.SetData(data3);
		return base.Update();
	}
}
