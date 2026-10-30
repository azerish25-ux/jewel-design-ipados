using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Gems;

[CSBlock("GemPlacerSide", "{3D234E2A-CEC6-49A0-B49E-69317913DA52}")]
public sealed class CSGemPlacerSide : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemPairCount;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemYOffsetMirror;

	public readonly CSInputChannel InGemTilt;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSInputChannel InGemOnOff;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutDisplayGem;

	public CSGemPlacerSide(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemPairCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffsetMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemOnOff = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutDisplayGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Expected O, but got Unknown
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Expected O, but got Unknown
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Expected O, but got Unknown
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Expected O, but got Unknown
		//IL_090d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0914: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Unknown result type (might be due to invalid IL or missing references)
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		OnCurve val = (OnCurve)(object)InCurve.NurbsCurve.Convert();
		List<CSGemData> gemList = InGem.GemList;
		CSGemData cSGemData = gemList[0];
		CSGemData cSGemData2 = gemList[gemList.Count - 1];
		CSGemData gem = InGemDefinition.Gem;
		double num = InGemSpacing.Double;
		double num2 = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double num4 = InGemTilt.Double;
		num4 /= 100.0;
		double angleDegrees = InGemAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InGemRotation.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		int integer = InGemPairCount.Integer;
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataHelper.GetTransformNoArray(InCurve.Data, out var value);
		if (!((Transform)(ref value)).IsValid)
		{
			value = Transform.Identity;
		}
		On3dPoint val2 = new On3dPoint(OnUtil.On_origin);
		val2.Transform((IOnXform)(object)value.Convert());
		On3dVector val3 = val.TangentAt(val.Domain().Min());
		On3dVector val4;
		On3dVector val5;
		if (InCurve.Data is CSOutsideRingRailData || InCurve.Data is CSRingRailData)
		{
			val4 = new On3dVector(OnUtil.On_zaxis);
			val4.Transform((IOnXform)(object)value.Convert());
			val4.Unitize();
			val5 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val4, (IOn3dVector)(object)val3);
			val5.Unitize();
		}
		else
		{
			val4 = new On3dVector(OnUtil.On_yaxis);
			val4.Transform((IOnXform)(object)value.Convert());
			val4.Unitize();
			val5 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			val5.Unitize();
		}
		OnXform val6 = new OnXform();
		val6.Rotation(angleDegrees, OnUtil.On_xaxis, OnUtil.On_origin);
		double num5 = (boolean2 ? (0.0 - angleDegrees2) : angleDegrees2);
		OnXform val7 = new OnXform();
		val7.Rotation(num5, OnUtil.On_zaxis, OnUtil.On_origin);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, val6.Convert() * val7.Convert()))
		{
			return UpdateError("Rhs Gem Bounding Box");
		}
		double num6 = angleDegrees2;
		OnXform val8 = new OnXform();
		val8.Rotation(num6, OnUtil.On_zaxis, OnUtil.On_origin);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, val6.Convert() * val8.Convert()))
		{
			return UpdateError("Lhs Gem Bounding Box");
		}
		for (int i = 0; i < integer; i++)
		{
			double num7 = 0.0;
			if (!val.GetClosestPoint((IOn3dPoint)(object)cSGemData.Center.Convert(), ref num7))
			{
				return UpdateError("Rhs Closest Point");
			}
			On3dPoint val9 = val.PointAt(num7);
			On3dVector obj = new On3dPoint((IOn3dPoint)(object)cSGemData.Center.Convert()) - (IOn3dPoint)(object)val9;
			On3dVector val10 = val9 - (IOn3dPoint)(object)val2;
			OnXform val11 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)cSGemData.Center.Convert(), (IOn3dVector)(object)val5, (IOn3dVector)(object)val10, OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis);
			val9.Transform((IOnXform)(object)val11);
			double num8 = 0.0 - val9.y;
			double num9 = 0.0 - val9.z;
			double num10 = num8 + (boolean ? (0.0 - num3) : num3);
			OnXform val12 = new OnXform();
			val12.Translation(0.0, num10, 0.0);
			double num11 = num9;
			if (OnUtil.ON_DotProduct((IOn3dVector)(object)obj, (IOn3dVector)(object)val10) < 0.0)
			{
				num11 = 0.0 - num11;
			}
			num11 += num2;
			OnCurve val13 = CSUtilGeometry.OffsetOld((IOnCurve)(object)val, (IOn3dVector)(object)(-val5), (IOn3dPoint)(object)val2, num11, closed: true);
			if (val13 == null)
			{
				return UpdateError("Offset Curve");
			}
			double num12 = 0.0;
			if (!val13.GetLength(ref num12))
			{
				return UpdateError("Offset Curve Length");
			}
			BoundingBox boundingBox3 = BoundingBox.Unset;
			if (!cSGemData.GetTightBoundingBox(ref boundingBox3, doGrow: false, val11.Convert()))
			{
				return UpdateError("Rhs Test Gem Bounding Box");
			}
			double num13 = 0.0;
			if (!val13.GetClosestPoint((IOn3dPoint)(object)cSGemData.Center.Convert(), ref num13))
			{
				return UpdateError("Rhs Closest Point");
			}
			double num14 = 0.0;
			if (!val13.GetLength(ref num14, 0.01, (IOnInterval)new OnInterval(val13.Domain().Min(), num13)))
			{
				return UpdateError("Rhs Closest Point Length");
			}
			double num15 = num14;
			Point3d val14 = ((BoundingBox)(ref boundingBox3)).Max;
			double num16 = Math.Abs(((Point3d)(ref val14)).X) + num;
			val14 = ((BoundingBox)(ref boundingBox)).Min;
			double num17 = (num15 + (num16 + Math.Abs(((Point3d)(ref val14)).X))) / num12;
			if (num17 > 1.0)
			{
				num17 -= 1.0;
			}
			if (num17 < 0.0)
			{
				num17 += 1.0;
			}
			double num18 = 0.0;
			if (!val13.GetNormalizedArcLengthPoint(num17, ref num18))
			{
				return UpdateError("Rhs GetNormalizedArcLengthPoint");
			}
			On3dPoint obj2 = val13.PointAt(num18);
			On3dVector val15 = val13.TangentAt(num18);
			val15.Unitize();
			On3dVector inputVector = OnUtil.ON_CrossProduct((IOn3dVector)(object)val15, (IOn3dVector)(object)val5);
			val14 = ((BoundingBox)(ref boundingBox)).Min;
			On3dPoint destination = obj2 + (IOn3dVector)(object)(val15 * Math.Abs(((Point3d)(ref val14)).X) * -1.0);
			val14 = ((BoundingBox)(ref boundingBox)).Min;
			IOn3dPoint val16 = (IOn3dPoint)new On3dPoint(((Point3d)(ref val14)).X, 0.0, 0.0);
			double num19 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection((IOn3dVector)(object)inputVector, (IOn3dVector)(object)val4, (IOn3dVector)(object)val5) * num4;
			OnXform val17 = new OnXform();
			val17.Rotation(num19, OnUtil.On_yaxis, val16);
			OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(val16, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)destination, (IOn3dVector)(object)val5, (IOn3dVector)(object)val4);
			CSGemData cSGemData3 = new CSGemData(gem);
			cSGemData3.ApplyTransform(xform.Convert() * val12.Convert() * val17.Convert() * val6.Convert() * val7.Convert());
			cSDataArray.Append(cSGemData3);
			cSGemData = cSGemData3;
			double num20 = 0.0;
			if (!val.GetClosestPoint((IOn3dPoint)(object)cSGemData2.Center.Convert(), ref num20))
			{
				return UpdateError("Lhs Closest Point");
			}
			On3dPoint val18 = val.PointAt(num20);
			On3dVector obj3 = new On3dPoint((IOn3dPoint)(object)cSGemData2.Center.Convert()) - (IOn3dPoint)(object)val18;
			On3dVector val19 = val18 - (IOn3dPoint)(object)val2;
			OnXform val20 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)cSGemData2.Center.Convert(), (IOn3dVector)(object)val5, (IOn3dVector)(object)val19, OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis);
			val18.Transform((IOnXform)(object)val20);
			double num21 = 0.0 - val18.y;
			double num22 = 0.0 - val18.z;
			double num23 = num21 + num3;
			OnXform val21 = new OnXform();
			val21.Translation(0.0, num23, 0.0);
			double num24 = num22;
			if (OnUtil.ON_DotProduct((IOn3dVector)(object)obj3, (IOn3dVector)(object)val19) < 0.0)
			{
				num24 = 0.0 - num24;
			}
			num24 += num2;
			OnCurve val22 = CSUtilGeometry.OffsetOld((IOnCurve)(object)val, (IOn3dVector)(object)(-val5), (IOn3dPoint)(object)val2, num24, closed: true);
			if (val22 == null)
			{
				return UpdateError("Offset Curve");
			}
			double num25 = 0.0;
			if (!val22.GetLength(ref num25))
			{
				return UpdateError("Offset Curve Length");
			}
			BoundingBox boundingBox4 = BoundingBox.Unset;
			if (!cSGemData2.GetTightBoundingBox(ref boundingBox4, doGrow: false, val20.Convert()))
			{
				return UpdateError("Lhs Test Gem Bounding Box");
			}
			double num26 = 0.0;
			if (!val22.GetClosestPoint((IOn3dPoint)(object)cSGemData2.Center.Convert(), ref num26))
			{
				return UpdateError("Lhs Closest Point");
			}
			double num27 = 0.0;
			if (!val22.GetLength(ref num27, 0.01, (IOnInterval)new OnInterval(val22.Domain().Min(), num26)))
			{
				return UpdateError("Lhs Closest Point Length");
			}
			double num28 = num27;
			val14 = ((BoundingBox)(ref boundingBox4)).Min;
			double num29 = Math.Abs(((Point3d)(ref val14)).X) + num;
			val14 = ((BoundingBox)(ref boundingBox2)).Max;
			double num30 = (num28 - (num29 + Math.Abs(((Point3d)(ref val14)).X))) / num25;
			if (num30 > 1.0)
			{
				num30 -= 1.0;
			}
			if (num30 < 0.0)
			{
				num30 += 1.0;
			}
			double num31 = 0.0;
			if (!val22.GetNormalizedArcLengthPoint(num30, ref num31))
			{
				return UpdateError("Lhs GetNormalizedArcLengthPoint");
			}
			On3dPoint obj4 = val22.PointAt(num31);
			On3dVector val23 = val22.TangentAt(num31);
			val23.Unitize();
			On3dVector inputVector2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val23, (IOn3dVector)(object)val5);
			val14 = ((BoundingBox)(ref boundingBox2)).Max;
			On3dPoint destination2 = obj4 + (IOn3dVector)(object)(val23 * Math.Abs(((Point3d)(ref val14)).X));
			val14 = ((BoundingBox)(ref boundingBox2)).Max;
			IOn3dPoint val24 = (IOn3dPoint)new On3dPoint(((Point3d)(ref val14)).X, 0.0, 0.0);
			double num32 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection((IOn3dVector)(object)inputVector2, (IOn3dVector)(object)val4, (IOn3dVector)(object)val5) * num4;
			OnXform val25 = new OnXform();
			val25.Rotation(num32, OnUtil.On_yaxis, val24);
			OnXform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(val24, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)destination2, (IOn3dVector)(object)val5, (IOn3dVector)(object)val4);
			CSGemData cSGemData4 = new CSGemData(gem);
			cSGemData4.ApplyTransform(xform2.Convert() * val21.Convert() * val25.Convert() * val6.Convert() * val8.Convert());
			cSDataArray.Append(cSGemData4);
			cSGemData2 = cSGemData4;
		}
		OutGem.SetData(cSDataArray);
		InGemOnOff.GetValue(out bool value2);
		if (value2)
		{
			OutDisplayGem.SetData(cSDataArray);
		}
		return base.Update();
	}
}
