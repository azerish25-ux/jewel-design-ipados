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
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Expected O, but got Unknown
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09af: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_097a: Unknown result type (might be due to invalid IL or missing references)
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Expected O, but got Unknown
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f3: Expected O, but got Unknown
		//IL_0a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Unknown result type (might be due to invalid IL or missing references)
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Unknown result type (might be due to invalid IL or missing references)
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b10: Expected O, but got Unknown
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
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
		if (!((Transform)(ref value)).IsValid || data is CSOutsideRingRailData || data is CSRingRailData)
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
		double num7 = num6;
		Point3d val14 = ((BoundingBox)(ref boundingBox2)).Max;
		double num8 = Math.Abs(((Point3d)(ref val14)).X);
		val14 = ((BoundingBox)(ref boundingBox2)).Min;
		int num9 = (int)(num7 / (num8 + Math.Abs(((Point3d)(ref val14)).X) + num));
		double num10 = num6;
		val14 = ((BoundingBox)(ref boundingBox2)).Max;
		double num11 = Math.Abs(((Point3d)(ref val14)).X);
		val14 = ((BoundingBox)(ref boundingBox2)).Min;
		double num12 = num10 % (num11 + Math.Abs(((Point3d)(ref val14)).X) + num);
		double num13 = num6;
		val14 = ((BoundingBox)(ref boundingBox3)).Max;
		double num14 = Math.Abs(((Point3d)(ref val14)).X);
		val14 = ((BoundingBox)(ref boundingBox3)).Min;
		int num15 = (int)(num13 / (num14 + Math.Abs(((Point3d)(ref val14)).X) + num));
		double num16 = num6;
		val14 = ((BoundingBox)(ref boundingBox3)).Max;
		double num17 = Math.Abs(((Point3d)(ref val14)).X);
		val14 = ((BoundingBox)(ref boundingBox3)).Min;
		double num18 = num16 % (num17 + Math.Abs(((Point3d)(ref val14)).X) + num);
		double num19 = num12 + num18;
		val14 = ((BoundingBox)(ref boundingBox)).Max;
		double num20 = Math.Abs(((Point3d)(ref val14)).X);
		val14 = ((BoundingBox)(ref boundingBox)).Min;
		int num21 = (int)(num19 / (num20 + Math.Abs(((Point3d)(ref val14)).X)));
		int num22 = (num9 + num15) / 2;
		if (num21 > 0)
		{
			double num23 = num4;
			val14 = ((BoundingBox)(ref boundingBox)).Max;
			double num24 = Math.Abs(((Point3d)(ref val14)).X);
			val14 = ((BoundingBox)(ref boundingBox)).Min;
			num4 = num23 - (num24 - Math.Abs(((Point3d)(ref val14)).X)) / num5 / 2.0;
		}
		else
		{
			double num25 = num4;
			val14 = ((BoundingBox)(ref boundingBox)).Max;
			double num26 = Math.Abs(((Point3d)(ref val14)).X);
			val14 = ((BoundingBox)(ref boundingBox)).Min;
			num4 = num25 - (num26 - Math.Abs(((Point3d)(ref val14)).X)) / num5;
		}
		double num27 = 0.0;
		if (num21 > 0)
		{
			if (boolean2)
			{
				val14 = ((BoundingBox)(ref boundingBox)).Max;
				double num28 = Math.Abs(((Point3d)(ref val14)).X);
				val14 = ((BoundingBox)(ref boundingBox2)).Min;
				num27 = num28 + Math.Abs(((Point3d)(ref val14)).X) + num;
			}
			else
			{
				val14 = ((BoundingBox)(ref boundingBox)).Max;
				double num29 = Math.Abs(((Point3d)(ref val14)).X);
				val14 = ((BoundingBox)(ref boundingBox2)).Min;
				num27 = num29 + Math.Abs(((Point3d)(ref val14)).X) + num;
			}
		}
		else if (boolean2)
		{
			val14 = ((BoundingBox)(ref boundingBox2)).Min;
			num27 = Math.Abs(((Point3d)(ref val14)).X) + num / 2.0;
		}
		else
		{
			val14 = ((BoundingBox)(ref boundingBox2)).Max;
			num27 = Math.Abs(((Point3d)(ref val14)).X) + num / 2.0;
		}
		for (int num30 = num22 - 1; num30 >= 0; num30--)
		{
			double num31 = num4;
			double num32 = num27;
			double num33 = num30;
			val14 = ((BoundingBox)(ref boundingBox2)).Max;
			double x = ((Point3d)(ref val14)).X;
			val14 = ((BoundingBox)(ref boundingBox2)).Min;
			double num34 = num31 + (num32 + num33 * (x - ((Point3d)(ref val14)).X + num)) / num5;
			if (num34 > 1.0)
			{
				num34 -= 1.0;
			}
			if (num34 < 0.0)
			{
				num34 += 1.0;
			}
			double num35 = 0.0;
			if (val4.GetNormalizedArcLengthPoint(num34, ref num35))
			{
				On3dVector obj = OnUtil.ON_CrossProduct((IOn3dVector)(object)val4.TangentAt(num35), (IOn3dVector)(object)val3);
				obj.Unitize();
				On3dVector val15 = obj * num3 + (IOn3dVector)(object)(val2 * (1.0 - num3));
				if (val15.Length() < 0.001)
				{
					val15 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val15.Unitize();
				}
				OnXform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(num35), (IOn3dVector)(object)val3, (IOn3dVector)(object)val15);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform4.Convert() * val8.Convert() * xform2.Convert());
				cSDataArray.Append(data2);
			}
		}
		if (num21 > 0)
		{
			double num36 = num4;
			if (num36 > 1.0)
			{
				num36 -= 1.0;
			}
			if (num36 < 0.0)
			{
				num36 += 1.0;
			}
			double num37 = 0.0;
			if (!val4.GetNormalizedArcLengthPoint(num36, ref num37))
			{
				return UpdateError("Normalized Arc Length Point");
			}
			On3dVector obj2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val4.TangentAt(num37), (IOn3dVector)(object)val3);
			obj2.Unitize();
			On3dVector val16 = obj2 * num3 + (IOn3dVector)(object)(val2 * (1.0 - num3));
			if (val16.Length() < 0.001)
			{
				val16 = new On3dVector(OnUtil.On_zaxis);
			}
			else
			{
				val16.Unitize();
			}
			OnXform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(num37), (IOn3dVector)(object)val3, (IOn3dVector)(object)val16);
			CSData data3 = gem.Duplicate();
			CSDataHelper.Transform(data3, xform5.Convert() * val6.Convert() * xform.Convert());
			cSDataArray.Append(data3);
		}
		double num38 = 0.0;
		if (num21 > 0)
		{
			if (boolean2)
			{
				val14 = ((BoundingBox)(ref boundingBox)).Min;
				double num39 = Math.Abs(((Point3d)(ref val14)).X);
				val14 = ((BoundingBox)(ref boundingBox3)).Max;
				num38 = num39 + Math.Abs(((Point3d)(ref val14)).X) + num;
			}
			else
			{
				val14 = ((BoundingBox)(ref boundingBox)).Min;
				double num40 = Math.Abs(((Point3d)(ref val14)).X);
				val14 = ((BoundingBox)(ref boundingBox3)).Max;
				num38 = num40 + Math.Abs(((Point3d)(ref val14)).X) + num;
			}
		}
		else if (boolean2)
		{
			val14 = ((BoundingBox)(ref boundingBox3)).Max;
			num38 = Math.Abs(((Point3d)(ref val14)).X) + num / 2.0;
		}
		else
		{
			val14 = ((BoundingBox)(ref boundingBox3)).Min;
			num38 = Math.Abs(((Point3d)(ref val14)).X) + num / 2.0;
		}
		for (int i = 0; i < num22; i++)
		{
			double num41 = num4;
			double num42 = num38;
			double num43 = i;
			val14 = ((BoundingBox)(ref boundingBox3)).Max;
			double x2 = ((Point3d)(ref val14)).X;
			val14 = ((BoundingBox)(ref boundingBox3)).Min;
			double num44 = num41 + (0.0 - (num42 + num43 * (x2 - ((Point3d)(ref val14)).X + num)) / num5);
			if (num44 > 1.0)
			{
				num44 -= 1.0;
			}
			if (num44 < 0.0)
			{
				num44 += 1.0;
			}
			double num45 = 0.0;
			if (val4.GetNormalizedArcLengthPoint(num44, ref num45))
			{
				On3dVector obj3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val4.TangentAt(num45), (IOn3dVector)(object)val3);
				obj3.Unitize();
				On3dVector val17 = obj3 * num3 + (IOn3dVector)(object)(val2 * (1.0 - num3));
				if (val17.Length() < 0.001)
				{
					val17 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val17.Unitize();
				}
				OnXform xform6 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(num45), (IOn3dVector)(object)val3, (IOn3dVector)(object)val17);
				CSData data4 = gem.Duplicate();
				CSDataHelper.Transform(data4, xform6.Convert() * val10.Convert() * xform3.Convert());
				cSDataArray.Append(data4);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
