using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemPlacerSpan_V1", "{5807DFC4-8F07-441c-A793-48229FCC20D4}")]
public sealed class CSGemPlacerSpan1 : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemSpan;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemYOffsetMirror;

	public readonly CSInputChannel InGemTilt;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSInputChannel InGemPosition;

	public readonly CSInputChannel InGemPlacementMethod;

	public readonly CSOutputChannel OutGem;

	public CSGemPlacerSpan1(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemSpan = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffsetMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Expected O, but got Unknown
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Expected O, but got Unknown
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Expected O, but got Unknown
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Expected O, but got Unknown
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f3: Expected O, but got Unknown
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b10: Expected O, but got Unknown
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
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemPlacementMethod.Integer;
		double angleDegrees = InGemSpan.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num = InGemSpacing.Double;
		double zOffset = InGemZOffset.Double;
		double num2 = InGemYOffset.Double;
		double num3 = InGemTilt.Double;
		num3 /= 100.0;
		double angleDegrees2 = InGemAngle.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double angleDegrees3 = InGemRotation.Double;
		angleDegrees3 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees3);
		double num4 = InGemPosition.Double;
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
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		OnCurve val4 = CSUtilGeometry.OriginOffset((IOnCurve)(object)val, (IOn3dVector)(object)(-val3), zOffset, closed: true);
		if (val4 == null)
		{
			return UpdateError("Offset Curve");
		}
		double num5 = 0.0;
		if (!val4.GetLength(ref num5))
		{
			return UpdateError("Offset Curve Length");
		}
		OnXform val5 = new OnXform();
		val5.Rotation(angleDegrees2, OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform val6 = new OnXform();
		if (boolean)
		{
			val6.Translation(0.0, 0.0, 0.0);
		}
		else
		{
			val6.Translation(0.0, num2, 0.0);
		}
		OnXform val7 = new OnXform();
		if (boolean2)
		{
			val7.Rotation(0.0, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		else
		{
			val7.Rotation(angleDegrees3, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		OnXform xform = val5 * (IOnXform)(object)val7;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, xform.Convert()))
		{
			return UpdateError("Center Gem Bounding Box");
		}
		OnXform val8 = new OnXform();
		if (boolean)
		{
			val8.Translation(0.0, 0.0 - num2, 0.0);
		}
		else
		{
			val8.Translation(0.0, num2, 0.0);
		}
		OnXform val9 = new OnXform();
		if (boolean2)
		{
			val9.Rotation(0.0 - angleDegrees3, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		else
		{
			val9.Rotation(angleDegrees3, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		OnXform xform2 = val5 * (IOnXform)(object)val9;
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, xform2.Convert()))
		{
			return UpdateError("RHS Gem Bounding Box");
		}
		OnXform val10 = new OnXform();
		val10.Translation(0.0, num2, 0.0);
		OnXform val11 = new OnXform();
		val11.Rotation(angleDegrees3, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform xform3 = val5 * (IOnXform)(object)val11;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, xform3.Convert()))
		{
			return UpdateError("LHS Gem Bounding Box");
		}
		OnLineCurve val12 = new OnLineCurve(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(0.0, 0.0, num5));
		((OnGeometry)val12).Rotate(angleDegrees / 2.0, OnUtil.On_yaxis, OnUtil.On_origin);
		ArrayOnX_EVENT val13 = new ArrayOnX_EVENT();
		val4.IntersectCurve((IOnCurve)(object)val12, ref val13, CSApplicationHelper.Application.Tolerance);
		if (val13.Count() < 1)
		{
			return UpdateError("Intersection");
		}
		double num6 = 0.0;
		if (!val4.GetLength(ref num6, CSApplicationHelper.Application.Tolerance, (IOnInterval)new OnInterval(val4.Domain().Min(), val13[0][0])))
		{
			return UpdateError("Intersection Length");
		}
		int num7 = (int)(num6 / (Math.Abs(boundingBox2.Max.X) + Math.Abs(boundingBox2.Min.X) + num));
		double num8 = num6 % (Math.Abs(boundingBox2.Max.X) + Math.Abs(boundingBox2.Min.X) + num);
		int num9 = (int)(num6 / (Math.Abs(boundingBox3.Max.X) + Math.Abs(boundingBox3.Min.X) + num));
		double num10 = num6 % (Math.Abs(boundingBox3.Max.X) + Math.Abs(boundingBox3.Min.X) + num);
		int num11 = (int)((num8 + num10) / (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox.Min.X)));
		int num12 = (num7 + num9) / 2;
		num4 = ((num11 <= 0) ? (num4 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / num5) : (num4 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / num5 / 2.0));
		double num13 = 0.0;
		num13 = ((num11 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num) : (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox2.Max.X) + num / 2.0) : (Math.Abs(boundingBox2.Min.X) + num / 2.0)));
		for (int num14 = num12 - 1; num14 >= 0; num14--)
		{
			double num15 = num4 + (num13 + (double)num14 * (boundingBox2.Max.X - boundingBox2.Min.X + num)) / num5;
			if (num15 > 1.0)
			{
				num15 -= 1.0;
			}
			if (num15 < 0.0)
			{
				num15 += 1.0;
			}
			double num16 = 0.0;
			if (val4.GetNormalizedArcLengthPoint(num15, ref num16))
			{
				On3dVector obj = OnUtil.ON_CrossProduct((IOn3dVector)(object)val4.TangentAt(num16), (IOn3dVector)(object)val3);
				obj.Unitize();
				On3dVector val14 = obj * num3 + (IOn3dVector)(object)(val2 * (1.0 - num3));
				if (val14.Length() < 0.001)
				{
					val14 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val14.Unitize();
				}
				OnXform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(num16), (IOn3dVector)(object)val3, (IOn3dVector)(object)val14);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform4.Convert() * val8.Convert() * xform2.Convert());
				cSDataArray.Append(data2);
			}
		}
		if (num11 > 0)
		{
			double num17 = num4;
			if (num17 > 1.0)
			{
				num17 -= 1.0;
			}
			if (num17 < 0.0)
			{
				num17 += 1.0;
			}
			double num18 = 0.0;
			if (!val4.GetNormalizedArcLengthPoint(num17, ref num18))
			{
				return UpdateError("Normalized Arc Length Point");
			}
			On3dVector obj2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val4.TangentAt(num18), (IOn3dVector)(object)val3);
			obj2.Unitize();
			On3dVector val15 = obj2 * num3 + (IOn3dVector)(object)(val2 * (1.0 - num3));
			if (val15.Length() < 0.001)
			{
				val15 = new On3dVector(OnUtil.On_zaxis);
			}
			else
			{
				val15.Unitize();
			}
			OnXform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(num18), (IOn3dVector)(object)val3, (IOn3dVector)(object)val15);
			CSData data3 = gem.Duplicate();
			CSDataHelper.Transform(data3, xform5.Convert() * val6.Convert() * xform.Convert());
			cSDataArray.Append(data3);
		}
		double num19 = 0.0;
		num19 = ((num11 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num) : (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox3.Min.X) + num / 2.0) : (Math.Abs(boundingBox3.Max.X) + num / 2.0)));
		for (int i = 0; i < num12; i++)
		{
			double num20 = num4 + (0.0 - (num19 + (double)i * (boundingBox3.Max.X - boundingBox3.Min.X + num)) / num5);
			if (num20 > 1.0)
			{
				num20 -= 1.0;
			}
			if (num20 < 0.0)
			{
				num20 += 1.0;
			}
			double num21 = 0.0;
			if (val4.GetNormalizedArcLengthPoint(num20, ref num21))
			{
				On3dVector obj3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val4.TangentAt(num21), (IOn3dVector)(object)val3);
				obj3.Unitize();
				On3dVector val16 = obj3 * num3 + (IOn3dVector)(object)(val2 * (1.0 - num3));
				if (val16.Length() < 0.001)
				{
					val16 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val16.Unitize();
				}
				OnXform xform6 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(num21), (IOn3dVector)(object)val3, (IOn3dVector)(object)val16);
				CSData data4 = gem.Duplicate();
				CSDataHelper.Transform(data4, xform6.Convert() * val10.Convert() * xform3.Convert());
				cSDataArray.Append(data4);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
