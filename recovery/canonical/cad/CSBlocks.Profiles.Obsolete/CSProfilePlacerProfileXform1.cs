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
		value.Transform(xDown);
		BoundingBox boundingBox = value.GetBoundingBox(accurate: true);
		if (!boundingBox.IsValid)
		{
			return UpdateError("GetTightBoundingBox failed 1");
		}
		BoundingBox boundingBox2 = value2.GetBoundingBox(accurate: true);
		if (!boundingBox2.IsValid)
		{
			return UpdateError("GetTightBoundingBox failed 2");
		}
		Transform transform = ((value3 == 0) ? Transform.Translation(Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + value4, value5, value6) : Transform.Translation(0.0 - (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox2.Max.X) + value4), value5, value6));
		oXform.SetData(new CSXformData(xUp * transform));
		return base.Update();
	}
}
