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
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Expected O, but got Unknown
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Expected O, but got Unknown
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Expected O, but got Unknown
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Expected O, but got Unknown
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Expected O, but got Unknown
		//IL_0934: Unknown result type (might be due to invalid IL or missing references)
		//IL_093b: Expected O, but got Unknown
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
			if (!value.IsValid || arrayItem is CSOutsideRingRailData || arrayItem is CSRingRailData)
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
			num4 = ((num6 <= 0) ? (num4 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / num5) : (num4 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / num5 / 2.0));
			double num8 = 0.0;
			num8 = ((num6 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num) : (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox2.Max.X) + num / 2.0) : (Math.Abs(boundingBox2.Min.X) + num / 2.0)));
			for (int num9 = num7 - 1; num9 >= 0; num9--)
			{
				double num10 = num4 + (num8 + (double)num9 * (boundingBox2.Max.X - boundingBox2.Min.X + num)) / num5;
				double num11 = 0.0;
				if (num10 > 1.0)
				{
					num10 -= 1.0;
				}
				if (num10 < 0.0)
				{
					num10 += 1.0;
				}
				if (!val11.GetNormalizedArcLengthPoint(num10, ref num11))
				{
					return UpdateError("Arc length failed");
				}
				On3dVector obj = OnUtil.ON_CrossProduct((IOn3dVector)(object)val11.TangentAt(num11), (IOn3dVector)(object)val10);
				obj.Unitize();
				On3dVector val12 = obj * num3 + (IOn3dVector)(object)(val9 * (1.0 - num3));
				if (val12.Length() < 0.001)
				{
					val12 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val12.Unitize();
				}
				OnXform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val11.PointAt(num11), (IOn3dVector)(object)val10, (IOn3dVector)(object)val12);
				CSData data = gem.Duplicate();
				CSDataHelper.Transform(data, xform4.Convert() * val4.Convert() * xform2.Convert());
				cSDataArray.Append(data);
			}
			if (num6 > 0)
			{
				double num12 = num4;
				double num13 = 0.0;
				if (num12 > 1.0)
				{
					num12 -= 1.0;
				}
				if (num12 < 0.0)
				{
					num12 += 1.0;
				}
				if (!val11.GetNormalizedArcLengthPoint(num12, ref num13))
				{
					return UpdateError("Arc length failed");
				}
				On3dVector obj2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val11.TangentAt(num13), (IOn3dVector)(object)val10);
				obj2.Unitize();
				On3dVector val13 = obj2 * num3 + (IOn3dVector)(object)(val9 * (1.0 - num3));
				if (val13.Length() < 0.001)
				{
					val13 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val13.Unitize();
				}
				OnXform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val11.PointAt(num13), (IOn3dVector)(object)val10, (IOn3dVector)(object)val13);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform5.Convert() * val2.Convert() * xform.Convert());
				cSDataArray.Append(data2);
			}
			double num14 = 0.0;
			num14 = ((num6 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num) : (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox3.Min.X) + num / 2.0) : (Math.Abs(boundingBox3.Max.X) + num / 2.0)));
			for (int j = 0; j < num7; j++)
			{
				double num15 = num4 + (0.0 - (num14 + (double)j * (boundingBox3.Max.X - boundingBox3.Min.X + num)) / num5);
				double num16 = 0.0;
				if (num15 > 1.0)
				{
					num15 -= 1.0;
				}
				if (num15 < 0.0)
				{
					num15 += 1.0;
				}
				if (!val11.GetNormalizedArcLengthPoint(num15, ref num16))
				{
					return UpdateError("Arc Length Failed");
				}
				On3dVector obj3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val11.TangentAt(num16), (IOn3dVector)(object)val10);
				obj3.Unitize();
				On3dVector val14 = obj3 * num3 + (IOn3dVector)(object)(val9 * (1.0 - num3));
				if (val14.Length() < 0.001)
				{
					val14 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					val14.Unitize();
				}
				OnXform xform6 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)gem.North.Convert(), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)val11.PointAt(num16), (IOn3dVector)(object)val10, (IOn3dVector)(object)val14);
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, xform6.Convert() * val6.Convert() * xform3.Convert());
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
