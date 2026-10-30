using System;
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
[CSBlock("GemPlacer_V2", "{71A62BA3-7474-43f6-B69E-C33F76E240DC}")]
public sealed class CSGemPlacer2 : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel IGemSpacing;

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

	public CSGemPlacer2(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		IGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
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
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Expected O, but got Unknown
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Expected O, but got Unknown
		//IL_0878: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_094a: Expected O, but got Unknown
		//IL_095a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a7: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InCurve.Data;
		if (data == null)
		{
			return UpdateMissing("Curve");
		}
		OnNurbsCurve val = InCurve.NurbsCurve.Convert();
		if (val == null)
		{
			return UpdateMissing("Curve");
		}
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		int integer2 = InGemPlacementMethod.Integer;
		double num = IGemSpacing.Double;
		double zOffset = InGemZOffset.Double;
		double num2 = InGemYOffset.Double;
		double num3 = InGemTilt.Double;
		num3 /= 100.0;
		double angleDegrees = InGemAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InGemRotation.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
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
			val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val2, (IOn3dVector)(object)((OnCurve)val).TangentAt(((OnCurve)val).Domain().Min()));
			val3.Unitize();
		}
		else
		{
			val2 = new On3dVector(OnUtil.On_yaxis);
			val2.Transform((IOnXform)(object)value.Convert());
			val2.Unitize();
			val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)((OnCurve)val).TangentAt(((OnCurve)val).Domain().Min()), (IOn3dVector)(object)val2);
			val3.Unitize();
		}
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer2, ref zOffset);
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
		val5.Rotation(angleDegrees, OnUtil.On_xaxis, OnUtil.On_origin);
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
			val7.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
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
			val9.Rotation(0.0 - angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		else
		{
			val9.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
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
		val11.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform xform3 = val5 * (IOnXform)(object)val11;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, xform3.Convert()))
		{
			return UpdateError("LHS Gem Bounding Box");
		}
		int num6 = integer % 2;
		int num7 = integer / 2;
		Point3d val12;
		if (num6 > 0)
		{
			double num8 = num4;
			val12 = ((BoundingBox)(ref boundingBox)).Max;
			double num9 = Math.Abs(((Point3d)(ref val12)).X);
			val12 = ((BoundingBox)(ref boundingBox)).Min;
			num4 = num8 - (num9 - Math.Abs(((Point3d)(ref val12)).X)) / num5 / 2.0;
		}
		else
		{
			double num10 = num4;
			val12 = ((BoundingBox)(ref boundingBox)).Max;
			double num11 = Math.Abs(((Point3d)(ref val12)).X);
			val12 = ((BoundingBox)(ref boundingBox)).Min;
			num4 = num10 - (num11 - Math.Abs(((Point3d)(ref val12)).X)) / num5;
		}
		double num12 = 0.0;
		if (num6 > 0)
		{
			if (boolean2)
			{
				val12 = ((BoundingBox)(ref boundingBox)).Max;
				double num13 = Math.Abs(((Point3d)(ref val12)).X);
				val12 = ((BoundingBox)(ref boundingBox2)).Min;
				num12 = num13 + Math.Abs(((Point3d)(ref val12)).X) + num;
			}
			else
			{
				val12 = ((BoundingBox)(ref boundingBox)).Max;
				double num14 = Math.Abs(((Point3d)(ref val12)).X);
				val12 = ((BoundingBox)(ref boundingBox2)).Min;
				num12 = num14 + Math.Abs(((Point3d)(ref val12)).X) + num;
			}
		}
		else if (boolean2)
		{
			val12 = ((BoundingBox)(ref boundingBox2)).Min;
			num12 = Math.Abs(((Point3d)(ref val12)).X) + num / 2.0;
		}
		else
		{
			val12 = ((BoundingBox)(ref boundingBox2)).Max;
			num12 = Math.Abs(((Point3d)(ref val12)).X) + num / 2.0;
		}
		for (int num15 = num7 - 1; num15 >= 0; num15--)
		{
			double num16 = num4;
			double num17 = num12;
			double num18 = num15;
			val12 = ((BoundingBox)(ref boundingBox2)).Max;
			double x = ((Point3d)(ref val12)).X;
			val12 = ((BoundingBox)(ref boundingBox2)).Min;
			double num19 = num16 + (num17 + num18 * (x - ((Point3d)(ref val12)).X + num)) / num5;
			double num20 = 0.0;
			if (num19 > 1.0)
			{
				num19 -= 1.0;
			}
			if (num19 < 0.0)
			{
				num19 += 1.0;
			}
			if (val4.GetNormalizedArcLengthPoint(num19, ref num20))
			{
				On3dVector obj = OnUtil.ON_CrossProduct((IOn3dVector)(object)val4.TangentAt(num20), (IOn3dVector)(object)val3);
				obj.Unitize();
				On3dVector val13 = obj * num3 + (IOn3dVector)(object)(val2 * (1.0 - num3));
				if (val13.Length() < 0.001)
				{
					val13 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val13.Unitize();
				}
				OnXform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(num20), (IOn3dVector)(object)val3, (IOn3dVector)(object)val13);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform4.Convert() * val8.Convert() * xform2.Convert());
				cSDataArray.Append(data2);
			}
		}
		if (num6 > 0)
		{
			double num21 = num4;
			double num22 = 0.0;
			if (num21 > 1.0)
			{
				num21 -= 1.0;
			}
			if (num21 < 0.0)
			{
				num21 += 1.0;
			}
			if (!val4.GetNormalizedArcLengthPoint(num21, ref num22))
			{
				return UpdateError("Normalized Arc Length Point");
			}
			On3dVector obj2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val4.TangentAt(num22), (IOn3dVector)(object)val3);
			obj2.Unitize();
			On3dVector val14 = obj2 * num3 + (IOn3dVector)(object)(val2 * (1.0 - num3));
			if (val14.Length() < 0.001)
			{
				val14 = new On3dVector(OnUtil.On_zaxis);
			}
			else
			{
				val14.Unitize();
			}
			OnXform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(num22), (IOn3dVector)(object)val3, (IOn3dVector)(object)val14);
			CSData data3 = gem.Duplicate();
			CSDataHelper.Transform(data3, xform5.Convert() * val6.Convert() * xform.Convert());
			cSDataArray.Append(data3);
		}
		double num23 = 0.0;
		if (num6 > 0)
		{
			if (boolean2)
			{
				val12 = ((BoundingBox)(ref boundingBox)).Min;
				double num24 = Math.Abs(((Point3d)(ref val12)).X);
				val12 = ((BoundingBox)(ref boundingBox3)).Max;
				num23 = num24 + Math.Abs(((Point3d)(ref val12)).X) + num;
			}
			else
			{
				val12 = ((BoundingBox)(ref boundingBox)).Min;
				double num25 = Math.Abs(((Point3d)(ref val12)).X);
				val12 = ((BoundingBox)(ref boundingBox3)).Max;
				num23 = num25 + Math.Abs(((Point3d)(ref val12)).X) + num;
			}
		}
		else if (boolean2)
		{
			val12 = ((BoundingBox)(ref boundingBox3)).Max;
			num23 = Math.Abs(((Point3d)(ref val12)).X) + num / 2.0;
		}
		else
		{
			val12 = ((BoundingBox)(ref boundingBox3)).Min;
			num23 = Math.Abs(((Point3d)(ref val12)).X) + num / 2.0;
		}
		for (int i = 0; i < num7; i++)
		{
			double num26 = num4;
			double num27 = num23;
			double num28 = i;
			val12 = ((BoundingBox)(ref boundingBox3)).Max;
			double x2 = ((Point3d)(ref val12)).X;
			val12 = ((BoundingBox)(ref boundingBox3)).Min;
			double num29 = num26 + (0.0 - (num27 + num28 * (x2 - ((Point3d)(ref val12)).X + num)) / num5);
			double num30 = 0.0;
			if (num29 > 1.0)
			{
				num29 -= 1.0;
			}
			if (num29 < 0.0)
			{
				num29 += 1.0;
			}
			if (val4.GetNormalizedArcLengthPoint(num29, ref num30))
			{
				On3dVector obj3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val4.TangentAt(num30), (IOn3dVector)(object)val3);
				obj3.Unitize();
				On3dVector val15 = obj3 * num3 + (IOn3dVector)(object)(val2 * (1.0 - num3));
				if (val15.Length() < 0.001)
				{
					val15 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val15.Unitize();
				}
				OnXform xform6 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val4.PointAt(num30), (IOn3dVector)(object)val3, (IOn3dVector)(object)val15);
				CSData data4 = gem.Duplicate();
				CSDataHelper.Transform(data4, xform6.Convert() * val10.Convert() * xform3.Convert());
				cSDataArray.Append(data4);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
