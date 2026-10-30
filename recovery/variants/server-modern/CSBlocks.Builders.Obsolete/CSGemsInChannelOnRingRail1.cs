using System;
using System.Collections.Generic;
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
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Builders.Obsolete;

[Obsolete]
[CSBlock("GemsInChannelOnRingRail_V1", "{079E49C4-B1F6-4DAA-BCA1-EAF6A4BBBB66}")]
public sealed class CSGemsInChannelOnRingRail1 : CSBlock
{
	public CSInputChannel iInnerRail { get; }

	public CSInputChannel iOuterRail { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iGemStart { get; }

	public CSInputChannel iGemEnd { get; }

	public CSInputChannel iGemSpacing { get; }

	public CSInputChannel iZOffsetPercent { get; }

	public CSInputChannel iChannelSpacing { get; }

	public CSInputChannel iChannelEndSpacing { get; }

	public CSInputChannel iShowChannel { get; }

	public CSInputChannel iOrientMethod { get; }

	public CSInputChannel iGemHeightOffset { get; }

	public CSInputChannel iSpacingMethod { get; }

	public CSOutputChannel oGems { get; }

	public CSOutputChannel oParamStart { get; }

	public CSOutputChannel oParamEnd { get; }

	public CSOutputChannel oParamFirstGem { get; }

	public CSOutputChannel oParamLastGem { get; }

	public CSOutputChannel oGemSizeVector { get; }

	public CSGemsInChannelOnRingRail1(string identifier)
		: base(identifier)
	{
		iInnerRail = new CSInputChannel(GetPropertyName(() => iInnerRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOuterRail = new CSInputChannel(GetPropertyName(() => iOuterRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iGemStart = new CSInputChannel(GetPropertyName(() => iGemStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemEnd = new CSInputChannel(GetPropertyName(() => iGemEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSpacing = new CSInputChannel(GetPropertyName(() => iGemSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZOffsetPercent = new CSInputChannel(GetPropertyName(() => iZOffsetPercent), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacing = new CSInputChannel(GetPropertyName(() => iChannelSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelEndSpacing = new CSInputChannel(GetPropertyName(() => iChannelEndSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iShowChannel = new CSInputChannel(GetPropertyName(() => iShowChannel), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iOrientMethod = new CSInputChannel(GetPropertyName(() => iOrientMethod), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemHeightOffset = new CSInputChannel(GetPropertyName(() => iGemHeightOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSpacingMethod = new CSInputChannel(GetPropertyName(() => iSpacingMethod), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGems = new CSOutputChannel(GetPropertyName(() => oGems), this, ChannelDataType.Gem, ChannelArrayType.Array);
		oParamStart = new CSOutputChannel(GetPropertyName(() => oParamStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParamEnd = new CSOutputChannel(GetPropertyName(() => oParamEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParamFirstGem = new CSOutputChannel(GetPropertyName(() => oParamFirstGem), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParamLastGem = new CSOutputChannel(GetPropertyName(() => oParamLastGem), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGemSizeVector = new CSOutputChannel(GetPropertyName(() => oGemSizeVector), this, ChannelDataType.Vector, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Expected O, but got Unknown
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iInnerRail.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Curve");
		}
		OnCurve val = (OnCurve)(object)flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Curve");
		}
		CSDataArray flattenArray2 = iOuterRail.GetFlattenArray();
		if (flattenArray2 == null || flattenArray2.GetArrayCount() < 1)
		{
			return UpdateMissing("Curve");
		}
		OnCurve val2 = (OnCurve)(object)flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val2 == null)
		{
			return UpdateMissing("Curve");
		}
		if (!(iGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem");
		}
		double value = 0.0;
		iGemStart.GetDouble(ref value);
		double value2 = 0.0;
		iGemEnd.GetDouble(ref value2);
		double value3 = 0.0;
		iGemSpacing.GetDouble(ref value3);
		double value4 = 0.0;
		iZOffsetPercent.GetDouble(ref value4);
		double value5 = 0.0;
		iChannelSpacing.GetDouble(ref value5);
		double value6 = 0.0;
		iChannelEndSpacing.GetDouble(ref value6);
		bool value7 = true;
		iShowChannel.GetBoolean(ref value7);
		int value8 = 0;
		iOrientMethod.GetInteger(ref value8);
		double value9 = 0.0;
		iGemHeightOffset.GetDouble(ref value9);
		int value10 = 0;
		iSpacingMethod.GetInteger(ref value10);
		CSData cSData = cSGemData.Duplicate();
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Gem Bounding Box");
		}
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, CSUtilGem.EGemPlacementMethod.Table, ref zOffset);
		double num = 0.0;
		if (!val2.GetNormalizedArcLengthPoint(value, ref num))
		{
			return UpdateError("Error Getting Start Point");
		}
		double num2 = 0.0;
		if (!val2.GetNormalizedArcLengthPoint(value2, ref num2))
		{
			return UpdateError("Error Getting End Point");
		}
		if (!value7)
		{
			oGems.SetData(null);
			oParamStart.SetData(new CSNumberData(num));
			oParamEnd.SetData(new CSNumberData(num2));
			oParamFirstGem.SetData(null);
			oParamLastGem.SetData(null);
			oGemSizeVector.SetData(null);
			return base.Update();
		}
		OnCurve val3 = val2.Duplicate();
		val3.Trim((IOnInterval)new OnInterval(num, num2));
		double num3 = 0.0;
		val3.GetLength(ref num3);
		double num4 = 0.0;
		num4 += value6 + ((Vector3d)(ref diagonal)).Y / 2.0;
		CSDataArray cSDataArray = new CSDataArray();
		List<On3dPoint> list = new List<On3dPoint>();
		List<On3dPoint> list2 = new List<On3dPoint>();
		List<double> list3 = new List<double>();
		double num5 = ((Vector3d)(ref diagonal)).Y + value3;
		for (; num4 + ((Vector3d)(ref diagonal)).Y / 2.0 + value6 <= num3; num4 += value3 + ((Vector3d)(ref diagonal)).Y)
		{
			double num6 = 0.0;
			On3dPoint val4 = new On3dPoint(0.0, 0.0, 0.0);
			OnXform val5 = null;
			double num7 = 0.0;
			do
			{
				if (Math.Abs(num7) > 0.01)
				{
					num4 -= num7 * 0.2;
				}
				if (val3.GetNormalizedArcLengthPoint(num4 / num3, ref num6))
				{
					On3dPoint val6 = val3.PointAt(num6);
					list.Add(val6);
					ClosestPoint(val3, val, num6, value8, out var paramOnInner);
					On3dPoint val7 = val.PointAt(paramOnInner);
					double num8 = (val6 - (IOn3dPoint)(object)val7).Length();
					OnXform val8 = new OnXform();
					val8.Translation(0.0, 0.0, zOffset - num8 * (1.0 - value4) + value9);
					On3dVector val9 = val7 - (IOn3dPoint)(object)val6;
					val9.Unitize();
					val5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Down.Convert(), (IOn3dPoint)(object)val6, (IOn3dVector)(object)val3.TangentAt(num6), (IOn3dVector)(object)val9) * (IOnXform)(object)val8;
					val4 = new On3dPoint(0.0, 0.0, 0.0);
					val4.Transform((IOnXform)(object)val5);
					if (list2.Count > 0)
					{
						num7 = (val4 - (IOn3dPoint)(object)list2[list2.Count - 1]).Length() - num5;
					}
				}
			}
			while (list2.Count != 0 && Math.Abs(num7) > 0.01);
			list2.Add(val4);
			CSData data = cSGemData.Duplicate();
			CSDataHelper.Transform(data, val5.Convert());
			cSDataArray.Append(data);
			list3.Add(num4);
		}
		num4 -= value3 + ((Vector3d)(ref diagonal)).Y;
		if (value10 == 1 && list3.Count > 1)
		{
			list2.Clear();
			cSDataArray = new CSDataArray();
			num4 += ((Vector3d)(ref diagonal)).Y / 2.0;
			double num9 = num3 - value6 - num4;
			num9 /= (double)(list3.Count - 1);
			for (int i = 0; i < list3.Count; i++)
			{
				double num10 = 0.0;
				On3dPoint val10 = new On3dPoint(0.0, 0.0, 0.0);
				OnXform val11 = null;
				num4 = list3[i] + (double)(i - 1) * num9;
				if (val3.GetNormalizedArcLengthPoint(num4 / num3, ref num10))
				{
					On3dPoint val12 = val3.PointAt(num10);
					list.Add(val12);
					ClosestPoint(val3, val, num10, value8, out var paramOnInner2);
					On3dPoint val13 = val.PointAt(paramOnInner2);
					double num11 = (val12 - (IOn3dPoint)(object)val13).Length();
					OnXform val14 = new OnXform();
					val14.Translation(0.0, 0.0, zOffset - num11 * (1.0 - value4) + value9);
					On3dVector val15 = val13 - (IOn3dPoint)(object)val12;
					val15.Unitize();
					val11 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)cSGemData.North.Convert(), (IOn3dVector)(object)cSGemData.Down.Convert(), (IOn3dPoint)(object)val12, (IOn3dVector)(object)val3.TangentAt(num10), (IOn3dVector)(object)val15) * (IOnXform)(object)val14;
					val10 = new On3dPoint(0.0, 0.0, 0.0);
					val10.Transform((IOnXform)(object)val11);
					list2.Add(val10);
					CSData data2 = cSGemData.Duplicate();
					CSDataHelper.Transform(data2, val11.Convert());
					cSDataArray.Append(data2);
				}
			}
		}
		num4 += ((Vector3d)(ref diagonal)).Y / 2.0 + value6;
		if (num4 > num3)
		{
			num4 = num3;
		}
		double num12 = 0.0;
		val3.GetNormalizedArcLengthPoint(num4 / num3, ref num12);
		On3dPoint val16 = val3.PointAt(num12);
		double number = 0.0;
		val2.GetClosestPoint((IOn3dPoint)(object)val16, ref number);
		double number2 = 0.0;
		double number3 = 0.0;
		if (list.Count > 0)
		{
			val2.GetClosestPoint((IOn3dPoint)(object)list[0], ref number2);
			val2.GetClosestPoint((IOn3dPoint)(object)list[list.Count - 1], ref number3);
		}
		oGems.SetData(cSDataArray);
		oParamStart.SetData(new CSNumberData(num));
		oParamEnd.SetData(new CSNumberData(number));
		oParamFirstGem.SetData(new CSNumberData(number2));
		oParamLastGem.SetData(new CSNumberData(number3));
		oGemSizeVector.SetData(new CSVectorData(diagonal));
		return base.Update();
	}

	private bool ClosestPoint(OnCurve Outer, OnCurve Inner, double paramOnOuter, int orientMethod, out double paramOnInner)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		paramOnInner = 0.0;
		ArrayOnX_EVENT val = new ArrayOnX_EVENT();
		On3dPoint val2 = Outer.PointAt(paramOnOuter);
		On3dVector dir = Outer.TangentAt(paramOnOuter);
		OnSurface val3 = makesrf(val2, dir, orientMethod);
		Inner.IntersectSurface((IOnSurface)(object)val3, ref val, CSApplicationHelper.Application.Tolerance);
		if (val.Count() == 0)
		{
			return false;
		}
		double num = 99999.0;
		for (int i = 0; i < val.Count(); i++)
		{
			double num2 = val2.DistanceTo((IOn3dPoint)(object)val[i][0]);
			if (num2 < num)
			{
				num = num2;
				paramOnInner = val[i][0];
			}
		}
		return true;
	}

	private OnSurface makesrf(On3dPoint Center, On3dVector Dir, int orientMethod)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_012c: Expected O, but got Unknown
		On3dVector val;
		On3dVector val2;
		if (orientMethod == 0)
		{
			val = new On3dVector(Center.x, 0.0, Center.z);
			val.Unitize();
			val2 = new On3dVector(0.0, 1.0, 0.0);
		}
		else
		{
			OnPlane val3 = new OnPlane((IOn3dPoint)(object)Center, (IOn3dVector)(object)Dir);
			val2 = new On3dVector(val3.GetXaxis());
			val = new On3dVector(val3.GetYaxis());
		}
		On3dPoint val4 = new On3dPoint((IOn3dPoint)(object)(Center + (IOn3dVector)(object)(val2 * 50.0) + (IOn3dVector)(object)(val * 50.0)));
		On3dPoint val5 = new On3dPoint((IOn3dPoint)(object)(Center + (IOn3dVector)(object)(val2 * 50.0) - (IOn3dVector)(object)(val * 50.0)));
		On3dPoint val6 = new On3dPoint((IOn3dPoint)(object)(Center - (IOn3dVector)(object)(val2 * 50.0) - (IOn3dVector)(object)(val * 50.0)));
		On3dPoint val7 = new On3dPoint((IOn3dPoint)(object)(Center - (IOn3dVector)(object)(val2 * 50.0) + (IOn3dVector)(object)(val * 50.0)));
		return (OnSurface)(object)RhUtil.RhinoCreateSurfaceFromCorners((IOn3dPoint)val4, (IOn3dPoint)(object)val5, ref val6, ref val7);
	}
}
