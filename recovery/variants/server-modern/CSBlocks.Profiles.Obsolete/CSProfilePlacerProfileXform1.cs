using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Profiles.Obsolete;

[Obsolete]
[CSBlock("ProfilePlacerProfileXform_V1", "{24CCD2E1-73A8-4714-8495-A3BD7210317D}")]
public sealed class CSProfilePlacerProfileXform1 : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iProfile;

	public readonly CSInputChannel iProfileMethod;

	public readonly CSInputChannel iXOffset;

	public readonly CSInputChannel iYOffset;

	public readonly CSInputChannel iZOffset;

	public readonly CSOutputChannel oXform;

	public CSProfilePlacerProfileXform1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfileMethod = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iXOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		CSData data = iCurve.Data;
		if (data == null)
		{
			return UpdateMissing("Missing curve data");
		}
		CSDataHelper.GetNurbsCurve(data, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing curve");
		}
		CSData data2 = iProfile.Data;
		if (data2 == null)
		{
			return UpdateMissing("Missing profile data");
		}
		CSDataHelper.GetNurbsCurve(data2, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Missing profile");
		}
		int value3 = 0;
		iProfileMethod.GetInteger(ref value3);
		double value4 = 0.0;
		iXOffset.GetDouble(ref value4);
		double value5 = 0.0;
		iYOffset.GetDouble(ref value5);
		double value6 = 0.0;
		iZOffset.GetDouble(ref value6);
		CSDataHelper.GetTransformNoArray(data, out var value7);
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(value7, out var xUp, out var xDown);
		value = new NurbsCurve(value);
		((GeometryBase)value).Transform(xDown);
		BoundingBox boundingBox = ((GeometryBase)value).GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox)).IsValid)
		{
			return UpdateError("GetTightBoundingBox failed 1");
		}
		BoundingBox boundingBox2 = ((GeometryBase)value2).GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox2)).IsValid)
		{
			return UpdateError("GetTightBoundingBox failed 2");
		}
		Point3d val;
		Transform val2;
		if (value3 != 0)
		{
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num = Math.Abs(((Point3d)(ref val)).X);
			val = ((BoundingBox)(ref boundingBox2)).Max;
			val2 = Transform.Translation(0.0 - (num + Math.Abs(((Point3d)(ref val)).X) + value4), value5, value6);
		}
		else
		{
			val = ((BoundingBox)(ref boundingBox)).Max;
			double num2 = Math.Abs(((Point3d)(ref val)).X);
			val = ((BoundingBox)(ref boundingBox2)).Min;
			val2 = Transform.Translation(num2 + Math.Abs(((Point3d)(ref val)).X) + value4, value5, value6);
		}
		Transform val3 = val2;
		oXform.SetData(new CSXformData(xUp * val3));
		return base.Update();
	}
}
