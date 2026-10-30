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
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
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
		if (((Transform)(ref value4)).IsValid)
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
		if (((Transform)(ref value5)).IsValid)
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
		Transform transform = default(Transform);
		((Transform)(ref xUp)).TryGetInverse(ref transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox, doGrow: true, transform))
		{
			return UpdateMissing("GetTightBoundingBox failed (Destination)");
		}
		((Transform)(ref xUp2)).TryGetInverse(ref transform);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!data2.GetTightBoundingBox(ref boundingBox2, doGrow: true, transform))
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
		Transform transform2 = Transform.Translation(num, num2, num3);
		OutTransform.SetData(new CSXformData(transform2));
		return base.Update();
	}
}
