using System;
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
[CSBlock("GemPlacer_V1", "{5B2B4460-789C-4139-8F87-9CC33AD10BBA}")]
public sealed class CSGemPlacer1 : CSBlock
{
	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemYOffsetMirror;

	public readonly CSInputChannel InGemTilt;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSInputChannel InGemPosition;

	public readonly CSOutputChannel OutGem;

	public CSInputChannel iCurve;

	public CSGemPlacer1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffsetMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Expected O, but got Unknown
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Expected O, but got Unknown
		//IL_0861: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0934: Unknown result type (might be due to invalid IL or missing references)
		//IL_093b: Expected O, but got Unknown
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0956: Unknown result type (might be due to invalid IL or missing references)
		//IL_0980: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Unknown result type (might be due to invalid IL or missing references)
		//IL_098c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_0998: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Missing Curve Input");
		}
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		double num = InGemSpacing.Double;
		double distance = InGemZOffset.Double;
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
		OnXform val = new OnXform();
		val.Rotation(angleDegrees, OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform val2 = new OnXform();
		if (boolean)
		{
			val2.Translation(0.0, 0.0, 0.0);
		}
		else
		{
			val2.Translation(0.0, num2, 0.0);
		}
		OnXform val3 = new OnXform();
		if (boolean2)
		{
			val3.Rotation(0.0, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		else
		{
			val3.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		OnXform xform = val * (IOnXform)(object)val3;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, xform.Convert()))
		{
			return UpdateError("Bounding Box Failed");
		}
		OnXform val4 = new OnXform();
		if (boolean)
		{
			val4.Translation(0.0, 0.0 - num2, 0.0);
		}
		else
		{
			val4.Translation(0.0, num2, 0.0);
		}
		OnXform val5 = new OnXform();
		if (boolean2)
		{
			val5.Rotation(0.0 - angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		else
		{
			val5.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		OnXform xform2 = val * (IOnXform)(object)val5;
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, xform2.Convert()))
		{
			return UpdateError("Bounding Box Failed");
		}
		OnXform val6 = new OnXform();
		val6.Translation(0.0, num2, 0.0);
		OnXform val7 = new OnXform();
		val7.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform xform3 = val * (IOnXform)(object)val7;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, xform3.Convert()))
		{
			return UpdateError("Bounding Box Failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				return UpdateError("Missing curve Data");
			}
			OnCurve val8 = (OnCurve)(object)arrayItem.GetNurbsCurve().Convert();
			CSDataHelper.GetTransformNoArray(arrayItem, out var value);
			On3dVector val9;
			On3dVector val10;
			if (!((Transform)(ref value)).IsValid || arrayItem is CSOutsideRingRailData || arrayItem is CSRingRailData)
			{
				val9 = new On3dVector(OnUtil.On_zaxis);
				val10 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val9, (IOn3dVector)(object)val8.TangentAt(val8.Domain().Min()));
				val10.Unitize();
			}
			else
			{
				val9 = new On3dVector(OnUtil.On_yaxis);
				val9.Transform((IOnXform)(object)value.Convert());
				val9.Unitize();
				val10 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val8.TangentAt(val8.Domain().Min()), (IOn3dVector)(object)val9);
				val10.Unitize();
			}
			OnCurve val11 = CSUtilGeometry.OriginOffset((IOnCurve)(object)val8, (IOn3dVector)(object)(-val10), distance, closed: true);
			double num5 = 0.0;
			if (!val11.GetLength(ref num5))
			{
				return UpdateError("offset failed");
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
				if (!val11.GetNormalizedArcLengthPoint(num19, ref num20))
				{
					return UpdateError("Arc length failed");
				}
				On3dVector obj = OnUtil.ON_CrossProduct((IOn3dVector)(object)val11.TangentAt(num20), (IOn3dVector)(object)val10);
				obj.Unitize();
				On3dVector val13 = obj * num3 + (IOn3dVector)(object)(val9 * (1.0 - num3));
				if (val13.Length() < 0.001)
				{
					val13 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val13.Unitize();
				}
				OnXform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val11.PointAt(num20), (IOn3dVector)(object)val10, (IOn3dVector)(object)val13);
				CSData data = gem.Duplicate();
				CSDataHelper.Transform(data, xform4.Convert() * val4.Convert() * xform2.Convert());
				cSDataArray.Append(data);
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
				if (!val11.GetNormalizedArcLengthPoint(num21, ref num22))
				{
					return UpdateError("Arc length failed");
				}
				On3dVector obj2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val11.TangentAt(num22), (IOn3dVector)(object)val10);
				obj2.Unitize();
				On3dVector val14 = obj2 * num3 + (IOn3dVector)(object)(val9 * (1.0 - num3));
				if (val14.Length() < 0.001)
				{
					val14 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val14.Unitize();
				}
				OnXform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val11.PointAt(num22), (IOn3dVector)(object)val10, (IOn3dVector)(object)val14);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform5.Convert() * val2.Convert() * xform.Convert());
				cSDataArray.Append(data2);
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
			for (int j = 0; j < num7; j++)
			{
				double num26 = num4;
				double num27 = num23;
				double num28 = j;
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
				if (!val11.GetNormalizedArcLengthPoint(num29, ref num30))
				{
					return UpdateError("Arc Length Failed");
				}
				On3dVector obj3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val11.TangentAt(num30), (IOn3dVector)(object)val10);
				obj3.Unitize();
				On3dVector val15 = obj3 * num3 + (IOn3dVector)(object)(val9 * (1.0 - num3));
				if (val15.Length() < 0.001)
				{
					val15 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val15.Unitize();
				}
				OnXform xform6 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val11.PointAt(num30), (IOn3dVector)(object)val10, (IOn3dVector)(object)val15);
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, xform6.Convert() * val6.Convert() * xform3.Convert());
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
