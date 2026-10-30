using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Profiles;

[CSBlock("ProfilePlacerNextToGem", "{9F12CB8B-1E05-499a-ADC5-A3174585B071}")]
public sealed class CSProfilePlacerNextToGem : CSBlock
{
	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iProfile;

	public readonly CSInputChannel iPosition;

	public readonly CSInputChannel iScaleX;

	public readonly CSInputChannel iScaleY;

	public readonly CSInputChannel iOffsetScaledY;

	public readonly CSInputChannel iOffsetScaledZ;

	public readonly CSOutputChannel oProfile;

	public CSProfilePlacerNextToGem(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iScaleX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iScaleY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOffsetScaledY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOffsetScaledZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iGem.GetFlattenArray();
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
		double value2 = 0.0;
		iPosition.GetDouble(ref value2);
		double value3 = 100.0;
		iScaleX.GetDouble(ref value3);
		value3 /= 100.0;
		double value4 = 100.0;
		iScaleY.GetDouble(ref value4);
		value4 /= 100.0;
		double value5 = 0.0;
		iOffsetScaledY.GetDouble(ref value5);
		double value6 = 0.0;
		iOffsetScaledZ.GetDouble(ref value6);
		CSDataArray cSDataArray = new CSDataArray();
		BoundingBox boundingBox = ((GeometryBase)value).GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox)).IsValid)
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (flattenArray.GetArrayItem(i) is CSGemData cSGemData)
			{
				double num = cSGemData.GemXSize * value3;
				Point3d val = ((BoundingBox)(ref boundingBox)).Max;
				double num2 = Math.Abs(((Point3d)(ref val)).X);
				val = ((BoundingBox)(ref boundingBox)).Min;
				double num3 = num / (num2 + Math.Abs(((Point3d)(ref val)).X));
				double num4 = cSGemData.GemZSize * value4;
				val = ((BoundingBox)(ref boundingBox)).Max;
				double num5 = Math.Abs(((Point3d)(ref val)).Y);
				val = ((BoundingBox)(ref boundingBox)).Min;
				double num6 = num4 / (num5 - Math.Abs(((Point3d)(ref val)).Y));
				Transform val2 = Transform.Scale(Plane.WorldXY, num3, num6, 1.0);
				double num7 = cSGemData.GemZSize * value5;
				double num8 = cSGemData.GemYSize * value6;
				Transform val3 = Transform.Translation(0.0, num7, num8);
				CSDataHelper.GetNurbsCurve(cSGemData, out var value7);
				double num9 = 0.0;
				if (!((Curve)value7).NormalizedLengthParameter(value2, ref num9))
				{
					return UpdateError("GetNormalizedArcLengthPoint failed");
				}
				Point3d unset = Point3d.Unset;
				Vector3d unset2 = Vector3d.Unset;
				unset2 = ((Curve)value7).TangentAt(num9);
				unset = ((Curve)value7).PointAt(num9);
				if (unset == Point3d.Unset || unset2 == Vector3d.Unset)
				{
					return UpdateError("Ev1Der failed");
				}
				Vector3d destination = Vector3d.CrossProduct(cSGemData.Up, unset2);
				Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.YAxis, Vector3d.ZAxis, unset, cSGemData.Up, destination);
				CSData data2 = data.Duplicate();
				CSDataHelper.Transform(data2, val4 * val3 * val2);
				cSDataArray.Append(data2);
			}
		}
		oProfile.SetData(cSDataArray);
		return base.Update();
	}
}
