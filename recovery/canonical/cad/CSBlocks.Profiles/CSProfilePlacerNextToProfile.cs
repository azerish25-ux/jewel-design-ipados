using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Profiles;

[CSBlock("ProfilePlacerNextToProfile", "{260FBFC4-E931-4911-9499-8043B7826E05}")]
public sealed class CSProfilePlacerNextToProfile : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iProfile;

	public readonly CSInputChannel iProfileMethod;

	public readonly CSInputChannel iXRotate;

	public readonly CSInputChannel iYRotate;

	public readonly CSInputChannel iZRotate;

	public readonly CSInputChannel iXOffset;

	public readonly CSInputChannel iYOffset;

	public readonly CSInputChannel iZOffset;

	public readonly CSOutputChannel oProfile;

	public CSProfilePlacerNextToProfile(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iProfile = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfileMethod = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iXRotate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iYRotate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZRotate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iXOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing curve");
		}
		CSData data = iProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Missing profile data");
		}
		CSDataHelper.GetNurbsCurve(data, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing profile curve");
		}
		int value2 = 0;
		iProfileMethod.GetInteger(ref value2);
		double value3 = 0.0;
		iXRotate.GetDouble(ref value3);
		double value4 = 0.0;
		iYRotate.GetDouble(ref value4);
		double value5 = 0.0;
		iZRotate.GetDouble(ref value5);
		double value6 = 0.0;
		iXOffset.GetDouble(ref value6);
		double value7 = 0.0;
		iYOffset.GetDouble(ref value7);
		double value8 = 0.0;
		iZOffset.GetDouble(ref value8);
		CSDataArray cSDataArray = new CSDataArray();
		BoundingBox boundingBox = value.GetBoundingBox(accurate: true);
		if (!value.IsValid)
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value3), Vector3d.XAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value4), Vector3d.YAxis, Point3d.Origin);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value5), Vector3d.ZAxis, Point3d.Origin);
		Transform transform4 = transform * transform2 * transform3;
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value9);
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(value9, out var xUp, out var xDown);
			CSDataHelper.GetNurbsCurve(arrayItem, out var value10);
			if (value10 == null)
			{
				return UpdateError("Invalid curve");
			}
			value10.Transform(xDown);
			BoundingBox boundingBox2 = value10.GetBoundingBox(accurate: true);
			if (!boundingBox2.IsValid)
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			Transform transform5 = ((value2 == 0) ? Transform.Translation(Math.Abs(boundingBox2.Max.X) + Math.Abs(boundingBox.Min.X) + value6, value7, value8) : Transform.Translation(0.0 - (Math.Abs(boundingBox2.Min.X) + Math.Abs(boundingBox.Max.X) + value6), value7, value8));
			Curve curve = value.DuplicateCurve();
			curve.Transform(xUp * transform5 * transform4);
			cSDataArray.Append(new CSOnGeometryData(curve, xUp * transform5 * transform4, applyTransform: false));
		}
		oProfile.SetData(cSDataArray);
		return base.Update();
	}
}
