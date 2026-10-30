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
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Expected O, but got Unknown
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Expected O, but got Unknown
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Expected O, but got Unknown
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
		if (!value.IsValid)
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
			double num15 = (num14 + (Math.Abs(boundingBox3.Max.X) + num + Math.Abs(boundingBox.Min.X))) / num12;
			if (num15 > 1.0)
			{
				num15 -= 1.0;
			}
			if (num15 < 0.0)
			{
				num15 += 1.0;
			}
			double num16 = 0.0;
			if (!val13.GetNormalizedArcLengthPoint(num15, ref num16))
			{
				return UpdateError("Rhs GetNormalizedArcLengthPoint");
			}
			On3dPoint obj2 = val13.PointAt(num16);
			On3dVector val14 = val13.TangentAt(num16);
			val14.Unitize();
			On3dVector inputVector = OnUtil.ON_CrossProduct((IOn3dVector)(object)val14, (IOn3dVector)(object)val5);
			On3dPoint destination = obj2 + (IOn3dVector)(object)(val14 * Math.Abs(boundingBox.Min.X) * -1.0);
			IOn3dPoint val15 = (IOn3dPoint)new On3dPoint(boundingBox.Min.X, 0.0, 0.0);
			double num17 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection((IOn3dVector)(object)inputVector, (IOn3dVector)(object)val4, (IOn3dVector)(object)val5) * num4;
			OnXform val16 = new OnXform();
			val16.Rotation(num17, OnUtil.On_yaxis, val15);
			OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(val15, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)destination, (IOn3dVector)(object)val5, (IOn3dVector)(object)val4);
			CSGemData cSGemData3 = new CSGemData(gem);
			cSGemData3.ApplyTransform(xform.Convert() * val12.Convert() * val16.Convert() * val6.Convert() * val7.Convert());
			cSDataArray.Append(cSGemData3);
			cSGemData = cSGemData3;
			double num18 = 0.0;
			if (!val.GetClosestPoint((IOn3dPoint)(object)cSGemData2.Center.Convert(), ref num18))
			{
				return UpdateError("Lhs Closest Point");
			}
			On3dPoint val17 = val.PointAt(num18);
			On3dVector obj3 = new On3dPoint((IOn3dPoint)(object)cSGemData2.Center.Convert()) - (IOn3dPoint)(object)val17;
			On3dVector val18 = val17 - (IOn3dPoint)(object)val2;
			OnXform val19 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)cSGemData2.Center.Convert(), (IOn3dVector)(object)val5, (IOn3dVector)(object)val18, OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis);
			val17.Transform((IOnXform)(object)val19);
			double num19 = 0.0 - val17.y;
			double num20 = 0.0 - val17.z;
			double num21 = num19 + num3;
			OnXform val20 = new OnXform();
			val20.Translation(0.0, num21, 0.0);
			double num22 = num20;
			if (OnUtil.ON_DotProduct((IOn3dVector)(object)obj3, (IOn3dVector)(object)val18) < 0.0)
			{
				num22 = 0.0 - num22;
			}
			num22 += num2;
			OnCurve val21 = CSUtilGeometry.OffsetOld((IOnCurve)(object)val, (IOn3dVector)(object)(-val5), (IOn3dPoint)(object)val2, num22, closed: true);
			if (val21 == null)
			{
				return UpdateError("Offset Curve");
			}
			double num23 = 0.0;
			if (!val21.GetLength(ref num23))
			{
				return UpdateError("Offset Curve Length");
			}
			BoundingBox boundingBox4 = BoundingBox.Unset;
			if (!cSGemData2.GetTightBoundingBox(ref boundingBox4, doGrow: false, val19.Convert()))
			{
				return UpdateError("Lhs Test Gem Bounding Box");
			}
			double num24 = 0.0;
			if (!val21.GetClosestPoint((IOn3dPoint)(object)cSGemData2.Center.Convert(), ref num24))
			{
				return UpdateError("Lhs Closest Point");
			}
			double num25 = 0.0;
			if (!val21.GetLength(ref num25, 0.01, (IOnInterval)new OnInterval(val21.Domain().Min(), num24)))
			{
				return UpdateError("Lhs Closest Point Length");
			}
			double num26 = (num25 - (Math.Abs(boundingBox4.Min.X) + num + Math.Abs(boundingBox2.Max.X))) / num23;
			if (num26 > 1.0)
			{
				num26 -= 1.0;
			}
			if (num26 < 0.0)
			{
				num26 += 1.0;
			}
			double num27 = 0.0;
			if (!val21.GetNormalizedArcLengthPoint(num26, ref num27))
			{
				return UpdateError("Lhs GetNormalizedArcLengthPoint");
			}
			On3dPoint obj4 = val21.PointAt(num27);
			On3dVector val22 = val21.TangentAt(num27);
			val22.Unitize();
			On3dVector inputVector2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val22, (IOn3dVector)(object)val5);
			On3dPoint destination2 = obj4 + (IOn3dVector)(object)(val22 * Math.Abs(boundingBox2.Max.X));
			IOn3dPoint val23 = (IOn3dPoint)new On3dPoint(boundingBox2.Max.X, 0.0, 0.0);
			double num28 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection((IOn3dVector)(object)inputVector2, (IOn3dVector)(object)val4, (IOn3dVector)(object)val5) * num4;
			OnXform val24 = new OnXform();
			val24.Rotation(num28, OnUtil.On_yaxis, val23);
			OnXform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(val23, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)destination2, (IOn3dVector)(object)val5, (IOn3dVector)(object)val4);
			CSGemData cSGemData4 = new CSGemData(gem);
			cSGemData4.ApplyTransform(xform2.Convert() * val20.Convert() * val24.Convert() * val6.Convert() * val8.Convert());
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
