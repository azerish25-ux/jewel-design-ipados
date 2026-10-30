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
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
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
		Transform xUp;
		Transform xDown = default(Transform);
		if (!((Transform)(ref value4)).IsValid || ((Transform)(ref value4)).Equals(Transform.Identity))
		{
			BoundingBox boundingBox = BoundingBox.Unset;
			if (!data.GetTightBoundingBox(ref boundingBox))
			{
				return UpdateError("GetTightBoundingBox failed (Source");
			}
			xUp = Transform.Translation(((BoundingBox)(ref boundingBox)).Center - Point3d.Origin);
			((Transform)(ref xUp)).TryGetInverse(ref xDown);
		}
		else if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value4, out xUp, out xDown))
		{
			return UpdateError("GetUnscaledXform failed");
		}
		CSDataHelper.GetTransformNoArray(data2, out var value5);
		Transform xUp2;
		Transform xDown2 = default(Transform);
		if (!((Transform)(ref value5)).IsValid || ((Transform)(ref value5)).Equals(Transform.Identity))
		{
			BoundingBox boundingBox2 = BoundingBox.Unset;
			if (!data2.GetTightBoundingBox(ref boundingBox2))
			{
				return UpdateError("GetTightBoundingBox failed (Destination)");
			}
			xUp2 = Transform.Translation(((BoundingBox)(ref boundingBox2)).Center - Point3d.Origin);
			((Transform)(ref xUp2)).TryGetInverse(ref xDown2);
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
		double num = 0.0;
		Point3d center;
		if (value)
		{
			center = ((BoundingBox)(ref boundingBox3)).Center;
			num = ((Point3d)(ref center)).X;
		}
		double num2 = 0.0;
		if (value2)
		{
			center = ((BoundingBox)(ref boundingBox3)).Center;
			num2 = ((Point3d)(ref center)).Y;
		}
		double num3 = 0.0;
		if (value3)
		{
			center = ((BoundingBox)(ref boundingBox3)).Center;
			num3 = ((Point3d)(ref center)).Z;
		}
		OnXform val = new OnXform();
		val.Translation(num, num2, num3);
		OutTransform.SetData(new CSXformData(xUp2 * val.Convert()));
		return base.Update();
	}
}
