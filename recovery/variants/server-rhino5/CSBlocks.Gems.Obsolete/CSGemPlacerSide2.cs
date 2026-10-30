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
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Expected O, but got Unknown
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Expected O, but got Unknown
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Expected O, but got Unknown
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
		if (!value.IsValid)
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
		double num11 = Math.Abs(boundingBox2.Max.X) + Math.Abs(boundingBox.Min.X) + num;
		for (int num12 = integer - 1; num12 >= 0; num12--)
		{
			double num13 = (num10 + (num11 + (double)num12 * (boundingBox.Max.X - boundingBox.Min.X + num))) / num9;
			double num14 = 0.0;
			if (num13 > 1.0)
			{
				num13 -= 1.0;
			}
			if (num13 < 0.0)
			{
				num13 += 1.0;
			}
			if (val16.GetNormalizedArcLengthPoint(num13, ref num14))
			{
				On3dVector obj = OnUtil.ON_CrossProduct((IOn3dVector)(object)val16.TangentAt(num14), (IOn3dVector)(object)val5);
				obj.Unitize();
				On3dVector val17 = obj * num4 + (IOn3dVector)(object)(val4 * (1.0 - num4));
				if (val17.Length() < 0.001)
				{
					val17 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val17.Unitize();
				}
				OnXform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val16.PointAt(num14), (IOn3dVector)(object)val5, (IOn3dVector)(object)val17);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform4.Convert() * val8.Convert() * val6.Convert() * val7.Convert());
				cSDataArray.Append(data2);
			}
		}
		double num15 = 0.0;
		if (!val16.GetClosestPoint((IOn3dPoint)(object)cSGemData2.Center.Convert(), ref num7))
		{
			return UpdateError("Lhs Closest Point");
		}
		if (!val16.GetLength(ref num15, 0.01, (IOnInterval)new OnInterval(val16.Domain().Min(), num7)))
		{
			return UpdateError("Lhs Closest Point Length");
		}
		double num16 = Math.Abs(boundingBox4.Min.X) + Math.Abs(boundingBox3.Max.X) + num;
		for (int i = 0; i < integer; i++)
		{
			double num17 = (num15 - (num16 + (double)i * (boundingBox3.Max.X - boundingBox3.Min.X + num))) / num9;
			double num18 = 0.0;
			if (num17 > 1.0)
			{
				num17 -= 1.0;
			}
			if (num17 < 0.0)
			{
				num17 += 1.0;
			}
			if (val16.GetNormalizedArcLengthPoint(num17, ref num18))
			{
				On3dVector obj2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val16.TangentAt(num18), (IOn3dVector)(object)val5);
				obj2.Unitize();
				On3dVector val18 = obj2 * num4 + (IOn3dVector)(object)(val4 * (1.0 - num4));
				if (val18.Length() < 0.001)
				{
					val18 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val18.Unitize();
				}
				OnXform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val16.PointAt(num18), (IOn3dVector)(object)val5, (IOn3dVector)(object)val18);
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, xform5.Convert() * val12.Convert() * xform2.Convert());
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
