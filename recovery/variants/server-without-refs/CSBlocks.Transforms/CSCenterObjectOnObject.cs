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
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
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
			if (!data.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity))
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
			if (!data2.GetTightBoundingBox(ref boundingBox2, doGrow: true, Transform.Identity))
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
		if (!cSData.GetTightBoundingBox(ref boundingBox3, doGrow: true, Transform.Identity))
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
		Transform val = Transform.Translation(num, num2, num3);
		Transform transform = xUp2 * val;
		CSData data3 = data.Duplicate();
		CSDataHelper.Transform(data3, transform);
		OutObject.SetData(data3);
		return base.Update();
	}
}
