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
[CSBlock("GemPlacerSide_V2", "{27B9B79D-F165-4e90-BB2E-8308960B25FA}")]
public sealed class CSGemPlacerSide2 : CSBlock
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

	public CSGemPlacerSide2(string identifier)
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
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Expected O, but got Unknown
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Expected O, but got Unknown
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Expected O, but got Unknown
		//IL_085c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Unknown result type (might be due to invalid IL or missing references)
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
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!((Transform)(ref value)).IsValid)
		{
			value = Transform.Identity;
		}
		On3dPoint val2 = new On3dPoint(OnUtil.On_origin);
		val2.Transform((IOnXform)(object)value.Convert());
		On3dVector val3 = val.TangentAt(val.Domain().Min());
		On3dVector val4;
		On3dVector val5;
		if (data is CSOutsideRingRailData || data is CSRingRailData)
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
		OnXform val8 = new OnXform();
		if (boolean)
		{
			val8.Translation(0.0, 0.0 - num3, 0.0);
		}
		else
		{
			val8.Translation(0.0, num3, 0.0);
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, val6.Convert() * val7.Convert()))
		{
			return UpdateError("Rhs Gem Bounding Box");
		}
		double num6 = 0.0;
		if (!val.GetClosestPoint((IOn3dPoint)(object)cSGemData.Center.Convert(), ref num6))
		{
			return UpdateError("Rhs Closest Point");
		}
		On3dPoint val9 = val.PointAt(num6);
		On3dVector val10 = new On3dPoint((IOn3dPoint)(object)cSGemData.Center.Convert()) - (IOn3dPoint)(object)val9;
		On3dVector val11 = val9 - (IOn3dPoint)new On3dPoint(OnUtil.On_origin);
		val11.Unitize();
		OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)cSGemData.Center.Convert(), (IOn3dVector)(object)val5, (IOn3dVector)(object)val11, OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox2, doGrow: false, xform.Convert()))
		{
			return UpdateError("Rhs Test Gem Bounding Box");
		}
		OnXform val12 = new OnXform();
		val12.Translation(0.0, num3, 0.0);
		OnXform val13 = new OnXform();
		val13.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform xform2 = val6 * (IOnXform)(object)val13;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, xform2.Convert()))
		{
			return UpdateError("Lhs Gem Bounding Box");
		}
		double num7 = 0.0;
		if (!val.GetClosestPoint((IOn3dPoint)(object)cSGemData2.Center.Convert(), ref num7))
		{
			return UpdateError("Lhs Closest Point");
		}
		On3dPoint val14 = val.PointAt(num7);
		_ = new On3dPoint((IOn3dPoint)(object)cSGemData2.Center.Convert()) - (IOn3dPoint)(object)val14;
		On3dVector val15 = val14 - (IOn3dPoint)new On3dPoint(OnUtil.On_origin);
		val15.Unitize();
		OnXform xform3 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)cSGemData2.Center.Convert(), (IOn3dVector)(object)val5, (IOn3dVector)(object)val15, OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis);
		BoundingBox boundingBox4 = BoundingBox.Unset;
		if (!cSGemData2.GetTightBoundingBox(ref boundingBox4, doGrow: false, xform3.Convert()))
		{
			return UpdateError("Lhs Test Gem Bounding Box");
		}
		double num8 = val10.Length();
		if (OnUtil.ON_DotProduct((IOn3dVector)(object)val10, (IOn3dVector)(object)val11) < 0.0)
		{
			num8 = 0.0 - num8;
		}
		num8 += num2;
		OnCurve val16 = CSUtilGeometry.OffsetOld((IOnCurve)(object)val, (IOn3dVector)(object)(-val5), (IOn3dPoint)(object)val2, num8, closed: true);
		if (val16 == null)
		{
			return UpdateError("Offset Curve");
		}
		double num9 = 0.0;
		if (!val16.GetLength(ref num9))
		{
			return UpdateError("Offset Curve Length");
		}
		double num10 = 0.0;
		if (!val16.GetClosestPoint((IOn3dPoint)(object)cSGemData.Center.Convert(), ref num6))
		{
			return UpdateError("Rhs Closest Point");
		}
		if (!val16.GetLength(ref num10, 0.01, (IOnInterval)new OnInterval(val16.Domain().Min(), num6)))
		{
			return UpdateError("Rhs Closest Point Length");
		}
		Point3d val17 = ((BoundingBox)(ref boundingBox2)).Max;
		double num11 = Math.Abs(((Point3d)(ref val17)).X);
		val17 = ((BoundingBox)(ref boundingBox)).Min;
		double num12 = num11 + Math.Abs(((Point3d)(ref val17)).X) + num;
		for (int num13 = integer - 1; num13 >= 0; num13--)
		{
			double num14 = num10;
			double num15 = num13;
			val17 = ((BoundingBox)(ref boundingBox)).Max;
			double x = ((Point3d)(ref val17)).X;
			val17 = ((BoundingBox)(ref boundingBox)).Min;
			double num16 = (num14 + (num12 + num15 * (x - ((Point3d)(ref val17)).X + num))) / num9;
			double num17 = 0.0;
			if (num16 > 1.0)
			{
				num16 -= 1.0;
			}
			if (num16 < 0.0)
			{
				num16 += 1.0;
			}
			if (val16.GetNormalizedArcLengthPoint(num16, ref num17))
			{
				On3dVector obj = OnUtil.ON_CrossProduct((IOn3dVector)(object)val16.TangentAt(num17), (IOn3dVector)(object)val5);
				obj.Unitize();
				On3dVector val18 = obj * num4 + (IOn3dVector)(object)(val4 * (1.0 - num4));
				if (val18.Length() < 0.001)
				{
					val18 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val18.Unitize();
				}
				OnXform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val16.PointAt(num17), (IOn3dVector)(object)val5, (IOn3dVector)(object)val18);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform4.Convert() * val8.Convert() * val6.Convert() * val7.Convert());
				cSDataArray.Append(data2);
			}
		}
		double num18 = 0.0;
		if (!val16.GetClosestPoint((IOn3dPoint)(object)cSGemData2.Center.Convert(), ref num7))
		{
			return UpdateError("Lhs Closest Point");
		}
		if (!val16.GetLength(ref num18, 0.01, (IOnInterval)new OnInterval(val16.Domain().Min(), num7)))
		{
			return UpdateError("Lhs Closest Point Length");
		}
		val17 = ((BoundingBox)(ref boundingBox4)).Min;
		double num19 = Math.Abs(((Point3d)(ref val17)).X);
		val17 = ((BoundingBox)(ref boundingBox3)).Max;
		double num20 = num19 + Math.Abs(((Point3d)(ref val17)).X) + num;
		for (int i = 0; i < integer; i++)
		{
			double num21 = num18;
			double num22 = i;
			val17 = ((BoundingBox)(ref boundingBox3)).Max;
			double x2 = ((Point3d)(ref val17)).X;
			val17 = ((BoundingBox)(ref boundingBox3)).Min;
			double num23 = (num21 - (num20 + num22 * (x2 - ((Point3d)(ref val17)).X + num))) / num9;
			double num24 = 0.0;
			if (num23 > 1.0)
			{
				num23 -= 1.0;
			}
			if (num23 < 0.0)
			{
				num23 += 1.0;
			}
			if (val16.GetNormalizedArcLengthPoint(num23, ref num24))
			{
				On3dVector obj2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val16.TangentAt(num24), (IOn3dVector)(object)val5);
				obj2.Unitize();
				On3dVector val19 = obj2 * num4 + (IOn3dVector)(object)(val4 * (1.0 - num4));
				if (val19.Length() < 0.001)
				{
					val19 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val19.Unitize();
				}
				OnXform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val16.PointAt(num24), (IOn3dVector)(object)val5, (IOn3dVector)(object)val19);
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, xform5.Convert() * val12.Convert() * xform2.Convert());
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
