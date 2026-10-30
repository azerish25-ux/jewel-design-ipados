using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("CenterObjectOnObjectTransform", "{3A7743D5-B3A1-4131-90A1-2CDEA5EF9398}")]
public sealed class CSCenterObjectOnObjectTransform : CSBlock
{
	public CSOutputChannel OutTransform { get; }

	public CSInputChannel InObjectSource { get; }

	public CSInputChannel InObjectDestination { get; }

	public CSInputChannel InEnableCenterOnObjectX { get; }

	public CSInputChannel InEnableCenterOnObjectY { get; }

	public CSInputChannel InEnableCenterOnObjectZ { get; }

	public CSCenterObjectOnObjectTransform(string identifier)
		: base(identifier)
	{
		InObjectSource = new CSInputChannel(GetPropertyName(() => InObjectSource), this, ChannelDataType.Any, ChannelArrayType.Any);
		InObjectDestination = new CSInputChannel(GetPropertyName(() => InObjectDestination), this, ChannelDataType.Any, ChannelArrayType.Any);
		InEnableCenterOnObjectX = new CSInputChannel(GetPropertyName(() => InEnableCenterOnObjectX), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnableCenterOnObjectY = new CSInputChannel(GetPropertyName(() => InEnableCenterOnObjectY), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnableCenterOnObjectZ = new CSInputChannel(GetPropertyName(() => InEnableCenterOnObjectZ), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutTransform = new CSOutputChannel(GetPropertyName(() => OutTransform), this, ChannelDataType.Transform, ChannelArrayType.Individual);
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
			if (!data.GetTightBoundingBox(ref boundingBox))
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
			if (!data2.GetTightBoundingBox(ref boundingBox2))
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
		if (!cSData.GetTightBoundingBox(ref boundingBox3))
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
		OnXform onXform = new OnXform();
		onXform.Translation(dx, dy, dz);
		OutTransform.SetData(new CSXformData(xUp2 * onXform.Convert()));
		return base.Update();
	}
}
