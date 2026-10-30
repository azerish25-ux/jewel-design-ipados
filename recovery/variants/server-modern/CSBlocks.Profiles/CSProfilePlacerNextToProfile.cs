using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Runtime;

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
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
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
		BoundingBox boundingBox = ((GeometryBase)value).GetBoundingBox(true);
		if (!((CommonObject)value).IsValid)
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		Transform val = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value3), Vector3d.XAxis, Point3d.Origin);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value4), Vector3d.YAxis, Point3d.Origin);
		Transform val3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value5), Vector3d.ZAxis, Point3d.Origin);
		Transform val4 = val * val2 * val3;
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
			((GeometryBase)value10).Transform(xDown);
			BoundingBox boundingBox2 = ((GeometryBase)value10).GetBoundingBox(true);
			if (!((BoundingBox)(ref boundingBox2)).IsValid)
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			Point3d val5;
			Transform val6;
			if (value2 != 0)
			{
				val5 = ((BoundingBox)(ref boundingBox2)).Min;
				double num = Math.Abs(((Point3d)(ref val5)).X);
				val5 = ((BoundingBox)(ref boundingBox)).Max;
				val6 = Transform.Translation(0.0 - (num + Math.Abs(((Point3d)(ref val5)).X) + value6), value7, value8);
			}
			else
			{
				val5 = ((BoundingBox)(ref boundingBox2)).Max;
				double num2 = Math.Abs(((Point3d)(ref val5)).X);
				val5 = ((BoundingBox)(ref boundingBox)).Min;
				val6 = Transform.Translation(num2 + Math.Abs(((Point3d)(ref val5)).X) + value6, value7, value8);
			}
			Transform val7 = val6;
			Curve val8 = ((Curve)value).DuplicateCurve();
			((GeometryBase)val8).Transform(xUp * val7 * val4);
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val8, xUp * val7 * val4, applyTransform: false));
		}
		oProfile.SetData(cSDataArray);
		return base.Update();
	}
}
