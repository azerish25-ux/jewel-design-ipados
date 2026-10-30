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

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemPlacerSide_V1", "{2D647144-61EF-446a-827E-6B9EAA8F631D}")]
public sealed class CSGemPlacerSide1 : CSBlock
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

	public readonly CSOutputChannel OutGem;

	public CSGemPlacerSide1(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
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
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Expected O, but got Unknown
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Expected O, but got Unknown
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Expected O, but got Unknown
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Expected O, but got Unknown
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_0630: Expected O, but got Unknown
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Expected O, but got Unknown
		CSData data = InCurve.Data;
		if (data == null)
		{
			return UpdateMissing("Curve");
		}
		OnCurve val = (OnCurve)(object)InCurve.NurbsCurve.Convert();
		if (val == null)
		{
			return UpdateMissing("Curve");
		}
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Gems");
		}
		CSGemData cSGemData = gemList[0];
		CSGemData cSGemData2 = gemList[gemList.Count - 1];
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemPairCount.Integer;
		double num = InGemSpacing.Double;
		double num2 = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double num4 = InGemTilt.Double;
		num4 /= 100.0;
		double angleDegrees = InGemAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InGemRotation.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataHelper.GetTransformNoArray(data, out var value);
		On3dVector val2;
		On3dVector val3;
		if (!value.IsValid || data is CSOutsideRingRailData || data is CSRingRailData)
		{
			val2 = new On3dVector(OnUtil.On_zaxis);
			val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val2, (IOn3dVector)(object)val.TangentAt(val.Domain().Min()));
			val3.Unitize();
		}
		else
		{
			val2 = new On3dVector(OnUtil.On_yaxis);
			val2.Transform((IOnXform)(object)value.Convert());
			val2.Unitize();
			val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val.TangentAt(val.Domain().Min()), (IOn3dVector)(object)val2);
			val3.Unitize();
		}
		OnXform val4 = new OnXform();
		val4.Rotation(angleDegrees, OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform val5 = new OnXform();
		if (boolean)
		{
			val5.Translation(0.0, 0.0 - num3, 0.0);
		}
		else
		{
			val5.Translation(0.0, num3, 0.0);
		}
		OnXform val6 = new OnXform();
		if (boolean2)
		{
			val6.Rotation(0.0 - angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		else
		{
			val6.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		OnXform xform = val4 * (IOnXform)(object)val6;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, xform.Convert()))
		{
			return UpdateError("RHS Gem Bounding Box");
		}
		double num5 = 0.0;
		if (!val.GetClosestPoint((IOn3dPoint)(object)cSGemData.Center.Convert(), ref num5))
		{
			return UpdateError("RHS Closest Point");
		}
		On3dPoint val7 = val.PointAt(num5);
		On3dVector val8 = new On3dPoint((IOn3dPoint)(object)cSGemData.Center.Convert()) - (IOn3dPoint)(object)val7;
		On3dVector val9 = val7 - (IOn3dPoint)new On3dPoint(OnUtil.On_origin);
		val9.Unitize();
		OnXform xform2 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)cSGemData.Center.Convert(), (IOn3dVector)(object)val3, (IOn3dVector)(object)val9, OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox2, doGrow: false, xform2.Convert()))
		{
			return UpdateError("RHS Test Gem Bounding Box");
		}
		OnXform val10 = new OnXform();
		val10.Translation(0.0, num3, 0.0);
		OnXform val11 = new OnXform();
		val11.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform xform3 = val4 * (IOnXform)(object)val11;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, xform3.Convert()))
		{
			return UpdateError("LHS Gem Bounding Box");
		}
		double num6 = 0.0;
		if (!val.GetClosestPoint((IOn3dPoint)(object)cSGemData2.Center.Convert(), ref num6))
		{
			return UpdateError("LHS Closest Point");
		}
		On3dPoint val12 = val.PointAt(num6);
		_ = new On3dPoint((IOn3dPoint)(object)cSGemData2.Center.Convert()) - (IOn3dPoint)(object)val12;
		On3dVector val13 = val12 - (IOn3dPoint)new On3dPoint(OnUtil.On_origin);
		val13.Unitize();
		OnXform xform4 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)cSGemData2.Center.Convert(), (IOn3dVector)(object)val3, (IOn3dVector)(object)val13, OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis);
		BoundingBox boundingBox4 = BoundingBox.Unset;
		if (!cSGemData2.GetTightBoundingBox(ref boundingBox4, doGrow: false, xform4.Convert()))
		{
			return UpdateError("LHS Test Gem Bounding Box");
		}
		double num7 = val8.Length();
		if (OnUtil.ON_DotProduct((IOn3dVector)(object)val8, (IOn3dVector)(object)val9) < 0.0)
		{
			num7 = 0.0 - num7;
		}
		num7 += num2;
		OnCurve val14 = CSUtilGeometry.OriginOffset((IOnCurve)(object)val, (IOn3dVector)(object)(-val3), num7, closed: true);
		if (val14 == null)
		{
			return UpdateError("Offset Curve");
		}
		double num8 = 0.0;
		if (!val14.GetLength(ref num8))
		{
			return UpdateError("Offset Curve Length");
		}
		double num9 = 0.0;
		if (!val14.GetClosestPoint((IOn3dPoint)(object)cSGemData.Center.Convert(), ref num5))
		{
			return UpdateError("RHS Closest Point");
		}
		if (!val14.GetLength(ref num9, 0.01, (IOnInterval)new OnInterval(val14.Domain().Min(), num5)))
		{
			return UpdateError("RHS Closest Point Length");
		}
		double num10 = Math.Abs(boundingBox2.Max.X) + Math.Abs(boundingBox.Min.X) + num;
		for (int num11 = integer - 1; num11 >= 0; num11--)
		{
			double num12 = (num9 + (num10 + (double)num11 * (boundingBox.Max.X - boundingBox.Min.X + num))) / num8;
			double num13 = 0.0;
			if (num12 > 1.0)
			{
				num12 -= 1.0;
			}
			if (num12 < 0.0)
			{
				num12 += 1.0;
			}
			if (val14.GetNormalizedArcLengthPoint(num12, ref num13))
			{
				On3dVector obj = OnUtil.ON_CrossProduct((IOn3dVector)(object)val14.TangentAt(num13), (IOn3dVector)(object)val3);
				obj.Unitize();
				On3dVector val15 = obj * num4 + (IOn3dVector)(object)(val2 * (1.0 - num4));
				if (val15.Length() < 0.001)
				{
					val15 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val15.Unitize();
				}
				OnXform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val14.PointAt(num13), (IOn3dVector)(object)val3, (IOn3dVector)(object)val15);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform5.Convert() * val5.Convert() * xform.Convert());
				cSDataArray.Append(data2);
			}
		}
		double num14 = 0.0;
		if (!val14.GetClosestPoint((IOn3dPoint)(object)cSGemData2.Center.Convert(), ref num6))
		{
			return UpdateError("LHS Closest Point");
		}
		if (!val14.GetLength(ref num14, 0.01, (IOnInterval)new OnInterval(val14.Domain().Min(), num6)))
		{
			return UpdateError("LHS Closest Point Length");
		}
		double num15 = Math.Abs(boundingBox4.Min.X) + Math.Abs(boundingBox3.Max.X) + num;
		for (int i = 0; i < integer; i++)
		{
			double num16 = (num14 - (num15 + (double)i * (boundingBox3.Max.X - boundingBox3.Min.X + num))) / num8;
			double num17 = 0.0;
			if (num16 > 1.0)
			{
				num16 -= 1.0;
			}
			if (num16 < 0.0)
			{
				num16 += 1.0;
			}
			if (val14.GetNormalizedArcLengthPoint(num16, ref num17))
			{
				On3dVector obj2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val14.TangentAt(num17), (IOn3dVector)(object)val3);
				obj2.Unitize();
				On3dVector val16 = obj2 * num4 + (IOn3dVector)(object)(val2 * (1.0 - num4));
				if (val16.Length() < 0.001)
				{
					val16 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val16.Unitize();
				}
				OnXform xform6 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val14.PointAt(num17), (IOn3dVector)(object)val3, (IOn3dVector)(object)val16);
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, xform6.Convert() * val10.Convert() * xform3.Convert());
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
