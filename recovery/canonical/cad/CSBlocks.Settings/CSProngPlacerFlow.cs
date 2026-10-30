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
using Rhino.Geometry;

namespace CSBlocks.Settings;

[CSBlock("ProngPlacerFlow", "{F1D8CD89-26B1-44C6-9AF9-0CAB3024C779}")]
public sealed class CSProngPlacerFlow : CSBlock
{
	public CSInputChannel InSortedGem { get; }

	public CSInputChannel InLargeDiameter { get; }

	public CSInputChannel InLargeHeight { get; }

	public CSInputChannel InLargeFillet { get; }

	public CSInputChannel InLargeTaper { get; }

	public CSInputChannel InSmallDiameter { get; }

	public CSInputChannel InSmallHeight { get; }

	public CSInputChannel InSmallFillet { get; }

	public CSInputChannel InSmallTaper { get; }

	public CSInputChannel InNudge { get; }

	public CSInputChannel InPlacement { get; }

	public CSInputChannel InStyle { get; }

	public CSInputChannel InEndControls { get; }

	public CSInputChannel InEndControlsMirror { get; }

	public CSInputChannel InEndAngleA { get; }

	public CSInputChannel InEndNudgeA { get; }

	public CSInputChannel InEndSpreadA { get; }

	public CSInputChannel InEndAngleB { get; }

	public CSInputChannel InEndNudgeB { get; }

	public CSInputChannel InEndSpreadB { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InMeshedProngs { get; }

	public CSOutputChannel OutSurface { get; }

	public CSProngPlacerFlow(string identifier)
		: base(identifier)
	{
		InSortedGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InLargeDiameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLargeHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLargeFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLargeTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSmallDiameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSmallHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSmallFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSmallTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStyle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndControls = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEndControlsMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEndAngleA = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndNudgeA = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndSpreadA = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndAngleB = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndNudgeB = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndSpreadB = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InMeshedProngs = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!(InSortedGem.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		OnNurbsCurve nurbs_curve = InProfile.GetNurbsCurve();
		if (nurbs_curve == null)
		{
			OnCircle onCircle = new OnCircle(OnUtil.On_xy_plane, OnUtil.On_origin, 1.0);
			nurbs_curve = new OnNurbsCurve();
			onCircle.GetNurbForm(ref nurbs_curve);
			if (nurbs_curve == null)
			{
				return UpdateMissing("Profile");
			}
		}
		double value = 0.0;
		InLargeDiameter.GetDouble(ref value);
		double value2 = 0.0;
		InLargeHeight.GetDouble(ref value2);
		double value3 = 0.0;
		InLargeFillet.GetDouble(ref value3);
		value3 /= 100.0;
		double value4 = 0.0;
		InLargeTaper.GetDouble(ref value4);
		value4 /= 100.0;
		double value5 = 0.0;
		InSmallDiameter.GetDouble(ref value5);
		double value6 = 0.0;
		InSmallHeight.GetDouble(ref value6);
		double value7 = 0.0;
		InSmallFillet.GetDouble(ref value7);
		value7 /= 100.0;
		double value8 = 0.0;
		InSmallTaper.GetDouble(ref value8);
		value8 /= 100.0;
		bool flag = value5 != 0.0 || value6 != 0.0 || value7 != 0.0 || value8 != 0.0;
		double value9 = 0.0;
		InNudge.GetDouble(ref value9);
		value9 = value9 / 100.0 * 2.0;
		double value10 = 0.0;
		InPlacement.GetDouble(ref value10);
		int value11 = 0;
		InStyle.GetInteger(ref value11);
		bool value12 = true;
		InEndControls.GetBoolean(ref value12);
		bool value13 = true;
		InEndControlsMirror.GetBoolean(ref value13);
		double value14 = 0.0;
		double value15 = value9;
		double num = 100.0;
		num /= 100.0;
		double value16 = value14;
		double value17 = value15;
		double value18 = num;
		if (value12)
		{
			InEndAngleA.GetDouble(ref value14);
			InEndNudgeA.GetDouble(ref value15);
			value15 = value15 / 100.0 * 2.0;
			InEndSpreadA.GetDouble(ref num);
			num /= 100.0;
			if (value13)
			{
				InEndAngleB.GetDouble(ref value16);
				InEndNudgeB.GetDouble(ref value17);
				value17 = value17 / 100.0 * 2.0;
				InEndSpreadB.GetDouble(ref value18);
				value18 /= 100.0;
			}
			else
			{
				value16 = value14;
				value17 = value15;
				value18 = num;
			}
		}
		bool value19 = false;
		InMeshedProngs.GetBoolean(ref value19);
		CSDataArray cSDataArray2 = new CSDataArray();
		double num2 = 0.0;
		double num3 = 0.0;
		if (flag)
		{
			for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
			{
				CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
				if (channelGemData != null)
				{
					double num4 = (channelGemData.GemXSize + channelGemData.GemYSize) / 2.0;
					if (num4 < num2 || i == 0)
					{
						num2 = num4;
					}
					if (num4 > num3 || i == 0)
					{
						num3 = num4;
					}
				}
			}
			if (Math.Abs(num3 - num2) < 0.001)
			{
				flag = false;
			}
		}
		CSData cSData = null;
		bool flag2 = CSUtilGemRails.IsClosed(cSDataArray.Array);
		for (int j = 0; j < cSDataArray.GetArrayCount() || (j == cSDataArray.GetArrayCount() && flag2); j++)
		{
			CSChannelGemData channelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			if (channelGemData2 == null)
			{
				continue;
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(channelGemData2.Xform.Convert(), out var xUp, out var xDown);
			if (!(channelGemData2.Duplicate() is CSChannelGemData cSChannelGemData))
			{
				continue;
			}
			cSChannelGemData.ApplyTransform(xDown.Convert());
			CSChannelGemData cSChannelGemData2 = null;
			if (j - 1 >= 0)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, j - 1).Duplicate() as CSChannelGemData;
				if (cSChannelGemData2 == null)
				{
					continue;
				}
				cSChannelGemData2.ApplyTransform(xDown.Convert());
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (j + 1 <= cSDataArray.GetArrayCount() - 1)
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j + 1).Duplicate() as CSChannelGemData;
				if (cSChannelGemData3 == null)
				{
					continue;
				}
				cSChannelGemData3.ApplyTransform(xDown.Convert());
			}
			else if (j + 1 == cSDataArray.GetArrayCount() && flag2)
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Duplicate() as CSChannelGemData;
				if (cSChannelGemData3 == null)
				{
					continue;
				}
				cSChannelGemData3.ApplyTransform(xDown.Convert());
			}
			double num5 = value;
			double filletPer = value3;
			double taperPer = value4;
			if (flag)
			{
				double num6 = ((cSChannelGemData.GemXSize + cSChannelGemData.GemYSize) / 2.0 - num2) / (num3 - num2);
				num5 = num6 * (value - value5) + value5;
				filletPer = num6 * (value3 - value7) + value7;
				taperPer = num6 * (value4 - value8) + value8;
			}
			List<OnXform> prongTransform = new List<OnXform>();
			OnCurve onCurve = CSUtilGemRails.Rail(cSDataArray);
			switch (value11)
			{
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			{
				OnCurve onCurve3 = null;
				if (onCurve != null)
				{
					onCurve3 = onCurve.DuplicateCurve();
					onCurve3.Transform(xDown);
				}
				PlaceProngs(cSChannelGemData, cSChannelGemData3, onCurve3, num5, value9, value10, doLhs: true, doRhs: true, xUp, ref prongTransform);
				break;
			}
			case 23:
			{
				OnCurve onCurve4 = null;
				if (onCurve != null)
				{
					onCurve4 = onCurve.DuplicateCurve();
					onCurve4.Transform(xDown);
				}
				PlaceProngs(cSChannelGemData, cSChannelGemData3, onCurve4, num5, value9, value10, doLhs: false, doRhs: true, xUp, ref prongTransform);
				break;
			}
			case 24:
			{
				OnCurve onCurve2 = null;
				if (onCurve != null)
				{
					onCurve2 = onCurve.DuplicateCurve();
					onCurve2.Transform(xDown);
				}
				PlaceProngs(cSChannelGemData, cSChannelGemData3, onCurve2, num5, value9, value10, doLhs: true, doRhs: false, xUp, ref prongTransform);
				break;
			}
			}
			switch (value11)
			{
			case 0:
				if (flag2)
				{
					PlaceProngs(cSChannelGemData, 0.0, 180.0, 2, num5, value9, value10, xUp, ref prongTransform);
					break;
				}
				if (j != 0)
				{
					PlaceProngs(cSChannelGemData, 180.0, 0.0, 1, num5, value9, value10, xUp, ref prongTransform);
				}
				if (j != cSDataArray.GetArrayCount() - 1)
				{
					PlaceProngs(cSChannelGemData, 0.0, 0.0, 1, num5, value9, value10, xUp, ref prongTransform);
				}
				break;
			case 1:
				PlaceProngs(cSChannelGemData, 90.0, 180.0, 2, num5, value9, value10, xUp, ref prongTransform);
				break;
			case 2:
				PlaceProngs(cSChannelGemData, 45.0, 180.0, 2, num5, value9, value10, xUp, ref prongTransform);
				break;
			case 3:
				PlaceProngs(cSChannelGemData, 135.0, 180.0, 2, num5, value9, value10, xUp, ref prongTransform);
				break;
			case 4:
			case 5:
			case 17:
			case 18:
			case 19:
			case 20:
				if (flag2)
				{
					PlaceProngs(cSChannelGemData, 45.0, 90.0, 4, num5, value9, value10, xUp, ref prongTransform);
					break;
				}
				if (j != 0)
				{
					PlaceProngs(cSChannelGemData, 135.0, 90.0, 2, num5, value9, value10, xUp, ref prongTransform);
				}
				if (j != cSDataArray.GetArrayCount() - 1)
				{
					PlaceProngs(cSChannelGemData, -45.0, 90.0, 2, num5, value9, value10, xUp, ref prongTransform);
				}
				break;
			case 13:
			case 21:
				PlaceProngs(cSChannelGemData, 90.0, 0.0, 1, num5, value9, value10, xUp, ref prongTransform);
				break;
			case 14:
			case 22:
				PlaceProngs(cSChannelGemData, 270.0, 0.0, 1, num5, value9, value10, xUp, ref prongTransform);
				break;
			case 15:
				PlaceProngs(cSChannelGemData, 45.0, 90.0, 2, num5, value9, value10, xUp, ref prongTransform);
				break;
			case 16:
				PlaceProngs(cSChannelGemData, 225.0, 90.0, 2, num5, value9, value10, xUp, ref prongTransform);
				break;
			}
			if (j == 0 && !flag2)
			{
				double num7 = 0.0;
				if (cSChannelGemData3 != null)
				{
					On3dPoint on3dPoint = cSChannelGemData.Center.Convert();
					num7 = OnUtil.ON_DotProduct(cSChannelGemData3.Center.Convert() - on3dPoint, cSChannelGemData.ForwardVector.Convert());
				}
				if (cSChannelGemData2 != null)
				{
					On3dPoint on3dPoint2 = cSChannelGemData2.Center.Convert();
					num7 = OnUtil.ON_DotProduct(cSChannelGemData.Center.Convert() - on3dPoint2, cSChannelGemData.ForwardVector.Convert());
				}
				if (num7 > 0.0)
				{
					switch (value11)
					{
					case 0:
					case 4:
					case 6:
					case 7:
					case 8:
					case 13:
					case 14:
					case 15:
					case 16:
					{
						double startingAngleDeg2 = 180.0 + value16;
						PlaceProngs(cSChannelGemData, startingAngleDeg2, 0.0, 1, num5, value17, value10, xUp, ref prongTransform);
						break;
					}
					case 5:
					case 9:
					case 11:
					case 17:
					case 18:
						if (value18 > 0.0)
						{
							double startingAngleDeg5 = 135.0 + value16 + 90.0 * (1.0 - value18) / 2.0;
							double spacingAngleDeg2 = 90.0 * value18;
							PlaceProngs(cSChannelGemData, startingAngleDeg5, spacingAngleDeg2, 2, num5, value17, value10, xUp, ref prongTransform);
						}
						else
						{
							double startingAngleDeg6 = 180.0 + value16;
							PlaceProngs(cSChannelGemData, startingAngleDeg6, 0.0, 1, num5, value17, value10, xUp, ref prongTransform);
						}
						break;
					case 10:
					case 12:
						if (value18 > 0.0)
						{
							double startingAngleDeg3 = 135.0 + value16 + 90.0 * (1.0 - value18) / 2.0;
							double spacingAngleDeg = 90.0 * value18;
							PlaceProngs2(cSChannelGemData, startingAngleDeg3, spacingAngleDeg, 2, num5, value17, value10, xUp, ref prongTransform);
						}
						else
						{
							double startingAngleDeg4 = 180.0 + value16;
							PlaceProngs(cSChannelGemData, startingAngleDeg4, 0.0, 1, num5, value17, value10, xUp, ref prongTransform);
						}
						break;
					case 19:
					case 20:
					{
						double startingAngleDeg = 180.0 + value16;
						PlaceProngs(cSChannelGemData, startingAngleDeg, 0.0, 1, num5, value17, value10, xUp, ref prongTransform);
						if (value18 > 0.0)
						{
							PlaceProngs(cSChannelGemData, 135.0, 90.0, 2, num5, value9, value10, xUp, ref prongTransform);
						}
						break;
					}
					}
				}
				else
				{
					switch (value11)
					{
					case 0:
					case 4:
					case 6:
					case 7:
					case 8:
					case 13:
					case 14:
					case 15:
					case 16:
					{
						double startingAngleDeg8 = 0.0 + value14;
						PlaceProngs(cSChannelGemData, startingAngleDeg8, 0.0, 1, num5, value15, value10, xUp, ref prongTransform);
						break;
					}
					case 5:
					case 9:
					case 11:
					case 17:
					case 18:
						if (num > 0.0)
						{
							double startingAngleDeg11 = -45.0 + value14 + 90.0 * (1.0 - num) / 2.0;
							double spacingAngleDeg4 = 90.0 * num;
							PlaceProngs(cSChannelGemData, startingAngleDeg11, spacingAngleDeg4, 2, num5, value15, value10, xUp, ref prongTransform);
						}
						else
						{
							double startingAngleDeg12 = 0.0 + value14;
							PlaceProngs(cSChannelGemData, startingAngleDeg12, 0.0, 1, num5, value15, value10, xUp, ref prongTransform);
						}
						break;
					case 10:
					case 12:
						if (num > 0.0)
						{
							double startingAngleDeg9 = -45.0 + value14 + 90.0 * (1.0 - num) / 2.0;
							double spacingAngleDeg3 = 90.0 * num;
							PlaceProngs2(cSChannelGemData, startingAngleDeg9, spacingAngleDeg3, 2, num5, value15, value10, xUp, ref prongTransform);
						}
						else
						{
							double startingAngleDeg10 = 0.0 + value14;
							PlaceProngs(cSChannelGemData, startingAngleDeg10, 0.0, 1, num5, value15, value10, xUp, ref prongTransform);
						}
						break;
					case 19:
					case 20:
					{
						double startingAngleDeg7 = 0.0 + value14;
						PlaceProngs(cSChannelGemData, startingAngleDeg7, 0.0, 1, num5, value15, value10, xUp, ref prongTransform);
						if (num > 0.0)
						{
							PlaceProngs(cSChannelGemData, -45.0, 90.0, 2, num5, value9, value10, xUp, ref prongTransform);
						}
						break;
					}
					}
				}
			}
			if (j == cSDataArray.GetArrayCount() - 1 && !flag2)
			{
				double num8 = 0.0;
				if (cSChannelGemData3 != null)
				{
					On3dPoint on3dPoint3 = cSChannelGemData.Center.Convert();
					num8 = OnUtil.ON_DotProduct(cSChannelGemData3.Center.Convert() - on3dPoint3, cSChannelGemData.ForwardVector.Convert());
				}
				if (cSChannelGemData2 != null)
				{
					On3dPoint on3dPoint4 = cSChannelGemData2.Center.Convert();
					num8 = OnUtil.ON_DotProduct(cSChannelGemData.Center.Convert() - on3dPoint4, cSChannelGemData.ForwardVector.Convert());
				}
				if (num8 > 0.0)
				{
					switch (value11)
					{
					case 0:
					case 4:
					case 6:
					case 9:
					case 10:
					case 13:
					case 14:
					case 15:
					case 16:
					{
						double startingAngleDeg14 = 0.0 + value14;
						PlaceProngs(cSChannelGemData, startingAngleDeg14, 0.0, 1, num5, value15, value10, xUp, ref prongTransform);
						break;
					}
					case 5:
					case 7:
					case 11:
					case 17:
					case 19:
						if (num > 0.0)
						{
							double startingAngleDeg17 = -45.0 + value14 + 90.0 * (1.0 - num) / 2.0;
							double spacingAngleDeg6 = 90.0 * num;
							PlaceProngs(cSChannelGemData, startingAngleDeg17, spacingAngleDeg6, 2, num5, value15, value10, xUp, ref prongTransform);
						}
						else
						{
							double startingAngleDeg18 = 0.0 + value14;
							PlaceProngs(cSChannelGemData, startingAngleDeg18, 0.0, 1, num5, value15, value10, xUp, ref prongTransform);
						}
						break;
					case 8:
					case 12:
						if (num > 0.0)
						{
							double startingAngleDeg15 = -45.0 + value14 + 90.0 * (1.0 - num) / 2.0;
							double spacingAngleDeg5 = 90.0 * num;
							PlaceProngs2(cSChannelGemData, startingAngleDeg15, spacingAngleDeg5, 2, num5, value15, value10, xUp, ref prongTransform);
						}
						else
						{
							double startingAngleDeg16 = 0.0 + value14;
							PlaceProngs(cSChannelGemData, startingAngleDeg16, 0.0, 1, num5, value15, value10, xUp, ref prongTransform);
						}
						break;
					case 18:
					case 20:
					{
						double startingAngleDeg13 = 0.0 + value14;
						PlaceProngs(cSChannelGemData, startingAngleDeg13, 0.0, 1, num5, value15, value10, xUp, ref prongTransform);
						if (num > 0.0)
						{
							PlaceProngs(cSChannelGemData, -45.0, 90.0, 2, num5, value9, value10, xUp, ref prongTransform);
						}
						break;
					}
					}
				}
				else
				{
					switch (value11)
					{
					case 0:
					case 4:
					case 6:
					case 9:
					case 10:
					case 13:
					case 14:
					case 15:
					case 16:
					{
						double startingAngleDeg20 = 180.0 + value16;
						PlaceProngs(cSChannelGemData, startingAngleDeg20, 0.0, 1, num5, value17, value10, xUp, ref prongTransform);
						break;
					}
					case 5:
					case 7:
					case 11:
					case 17:
					case 19:
						if (value18 > 0.0)
						{
							double startingAngleDeg23 = 135.0 + value16 + 90.0 * (1.0 - value18) / 2.0;
							double spacingAngleDeg8 = 90.0 * value18;
							PlaceProngs(cSChannelGemData, startingAngleDeg23, spacingAngleDeg8, 2, num5, value17, value10, xUp, ref prongTransform);
						}
						else
						{
							double startingAngleDeg24 = 180.0 + value16;
							PlaceProngs(cSChannelGemData, startingAngleDeg24, 0.0, 1, num5, value17, value10, xUp, ref prongTransform);
						}
						break;
					case 8:
					case 12:
						if (value18 > 0.0)
						{
							double startingAngleDeg21 = 135.0 + value16 + 90.0 * (1.0 - value18) / 2.0;
							double spacingAngleDeg7 = 90.0 * value18;
							PlaceProngs2(cSChannelGemData, startingAngleDeg21, spacingAngleDeg7, 2, num5, value17, value10, xUp, ref prongTransform);
						}
						else
						{
							double startingAngleDeg22 = 180.0 + value16;
							PlaceProngs(cSChannelGemData, startingAngleDeg22, 0.0, 1, num5, value17, value10, xUp, ref prongTransform);
						}
						break;
					case 18:
					case 20:
					{
						double startingAngleDeg19 = 180.0 + value16;
						PlaceProngs(cSChannelGemData, startingAngleDeg19, 0.0, 1, num5, value17, value10, xUp, ref prongTransform);
						if (value18 > 0.0)
						{
							PlaceProngs(cSChannelGemData, 135.0, 90.0, 2, num5, value17, value10, xUp, ref prongTransform);
						}
						break;
					}
					}
				}
			}
			foreach (OnXform item in prongTransform)
			{
				double num9 = value2;
				if (flag)
				{
					num9 = ((cSChannelGemData.GemXSize + cSChannelGemData.GemYSize) / 2.0 - num2) / (num3 - num2) * (value2 - value6) + value6;
				}
				if (CSApplicationHelper.Application.IsCamMode)
				{
					Circle circle = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData2.ZMaxPoint.DistanceTo(Point3d.Origin));
					Transform xform = item.Convert();
					Point3d origin = Point3d.Origin;
					origin.Transform(xform);
					Line line = new Line(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
					line.Transform(xform);
					Point3d point3d = line.ClosestPoint(circle.ClosestPoint(line.PointAt(1.0)), limitToFiniteSegment: true);
					if (point3d.DistanceTo(origin) > 7.0 * num9 / 8.0)
					{
						num9 = point3d.DistanceTo(origin) * 1.125;
					}
				}
				if (flag || j == 0)
				{
					if (value19)
					{
						if (!CSUtilGeometry.CreateMeshProng(nurbs_curve, num5, num9, filletPer, taperPer, out var meshSurface))
						{
							return UpdateError("CreateMeshProng failed");
						}
						cSData = new CSOnGeometryData(meshSurface.Convert());
					}
					else
					{
						if (!CSUtilGeometry.CreateProng(nurbs_curve, num5, num9, filletPer, taperPer, out var nurbsSurface))
						{
							return UpdateError("CreateProng failed");
						}
						cSData = new CSOnGeometryData(nurbsSurface.Convert());
					}
				}
				CSData data = cSData.Duplicate();
				CSDataHelper.Transform(data, item.Convert());
				cSDataArray2.Append(data);
			}
		}
		OutSurface.SetData(cSDataArray2);
		return base.Update();
	}

	private static void PlaceProngs(CSChannelGemData gem, double startingAngleDeg, double spacingAngleDeg, int prongCount, double prongDiameter, double nudgePer, double placement, OnXform xUp, ref List<OnXform> prongTransform)
	{
		if (gem == null)
		{
			return;
		}
		OnNurbsCurve onNurbsCurve = gem.GetNurbsCurve().Convert();
		On3dVector on3dVector = new On3dVector(gem.ForwardVector.Convert())
		{
			z = 0.0
		};
		for (int i = 0; i < prongCount; i++)
		{
			double angleDegrees = startingAngleDeg + (double)i * spacingAngleDeg;
			OnLineCurve onLineCurve = new OnLineCurve(gem.Center.Convert(), gem.Center.Convert() + on3dVector * 50.0);
			onLineCurve.Rotate(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), gem.Up.Convert(), gem.Center.Convert());
			ArrayOnX_EVENT x = new ArrayOnX_EVENT();
			onNurbsCurve.IntersectCurve(onLineCurve, ref x);
			if (x.Count() <= 0)
			{
				break;
			}
			double num = x[0].get_m_a(0);
			On3dVector on3dVector2 = CSBlockUtil.CSUtilMath.AverageTangentAt(onNurbsCurve, num);
			on3dVector2.Unitize();
			On3dVector on3dVector3 = onNurbsCurve.PointAt(num) - gem.Center.Convert();
			On3dVector on3dVector4 = OnUtil.ON_CrossProduct(gem.Up.Convert(), on3dVector2);
			on3dVector4.Unitize();
			on3dVector4 *= prongDiameter / 2.0 * (1.0 - nudgePer);
			On3dVector on3dVector5 = gem.Up.Convert();
			on3dVector5.Unitize();
			on3dVector5 *= placement;
			OnXform onXform = new OnXform();
			onXform.Translation(on3dVector3 + on3dVector4 + on3dVector5);
			double angle_radians = CSBlockUtil.CSUtilMath.ToRadians(90.0) - CSBlockUtil.CSUtilMath.AngleOfPointD(on3dVector2.x, on3dVector2.y);
			OnXform onXform2 = new OnXform();
			onXform2.Rotation(angle_radians, OnUtil.On_zaxis, OnUtil.On_origin);
			prongTransform.Add(xUp * onXform * onXform2);
		}
	}

	private static void PlaceProngs(CSChannelGemData gemLhs, CSGemData gemRhs, IOnCurve rail, double prongDiameter, double nudgePer, double placement, bool doLhs, bool doRhs, OnXform xUp, ref List<OnXform> prongTransform)
	{
		if (gemLhs == null || gemRhs == null)
		{
			return;
		}
		double t = 0.0;
		OnLineCurve onLineCurve = new OnLineCurve(gemLhs.Center.Convert(), gemRhs.Center.Convert());
		On3dPoint on3dPoint = onLineCurve.PointAt(onLineCurve.Domain().Mid());
		rail.GetClosestPoint(on3dPoint, ref t);
		On3dPoint on3dPoint2 = rail.PointAt(t);
		OnNurbsCurve onNurbsCurve = gemLhs.GetNurbsCurve().Convert();
		onNurbsCurve.GetClosestPoint(on3dPoint, ref t);
		On3dPoint on3dPoint3 = onNurbsCurve.PointAt(t);
		OnNurbsCurve onNurbsCurve2 = gemRhs.GetNurbsCurve().Convert();
		onNurbsCurve2.GetClosestPoint(on3dPoint, ref t);
		On3dPoint arg = onNurbsCurve2.PointAt(t);
		double num = on3dPoint3.DistanceTo(arg);
		double num2 = num + 2.0 * nudgePer * (prongDiameter / 2.0);
		On3dPoint on3dPoint4 = new On3dPoint(on3dPoint.x, on3dPoint.y, on3dPoint2.z);
		On3dVector on3dVector = gemLhs.Up.Convert() + gemRhs.Up.Convert();
		on3dVector.Unitize();
		if (prongDiameter <= num2)
		{
			On3dVector on3dVector2 = OnUtil.ON_CrossProduct(on3dVector, onLineCurve.PointAtStart() - onLineCurve.PointAtEnd());
			on3dVector2.Unitize();
			On3dPoint destination = on3dPoint4 + on3dVector * placement;
			OnXform onXform = CSBlockUtil.CSUtilMath.Orient3Point(gemLhs.Center.Convert(), gemLhs.ForwardVector.Convert(), gemLhs.Up.Convert(), destination, on3dVector2, on3dVector);
			prongTransform.Add(xUp * onXform);
		}
		else if (doLhs || doRhs)
		{
			double num3 = on3dPoint3.DistanceTo(gemLhs.Center.Convert());
			double num4 = num3 + num / 2.0;
			double num5 = num3 + prongDiameter / 2.0 - nudgePer * (prongDiameter / 2.0);
			double num6 = Math.Sqrt(num5 * num5 - num4 * num4);
			if (doLhs)
			{
				On3dVector on3dVector3 = OnUtil.ON_CrossProduct(on3dVector, onLineCurve.PointAtStart() - onLineCurve.PointAtEnd());
				on3dVector3.Unitize();
				On3dPoint destination2 = on3dPoint4 + on3dVector3 * num6 + on3dVector * placement;
				OnXform onXform2 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, destination2, on3dVector3, on3dVector);
				prongTransform.Add(xUp * onXform2);
			}
			if (doRhs)
			{
				On3dVector on3dVector4 = OnUtil.ON_CrossProduct(on3dVector, onLineCurve.PointAtEnd() - onLineCurve.PointAtStart());
				on3dVector4.Unitize();
				On3dPoint destination3 = on3dPoint4 + on3dVector4 * num6 + on3dVector * placement;
				OnXform onXform3 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, destination3, on3dVector4, on3dVector);
				prongTransform.Add(xUp * onXform3);
			}
		}
	}

	private static void PlaceProngs2(CSChannelGemData gem, double startingAngleDeg, double spacingAngleDeg, int prongCount, double prongDiameter, double nudgePer, double placement, OnXform xUp, ref List<OnXform> prongTransform)
	{
		if (gem == null)
		{
			return;
		}
		OnNurbsCurve onNurbsCurve = gem.GetNurbsCurve().Convert();
		On3dVector on3dVector = new On3dVector(gem.ForwardVector.Convert())
		{
			z = 0.0
		};
		for (int i = 0; i < prongCount; i++)
		{
			double num = startingAngleDeg + (double)i * spacingAngleDeg;
			OnLineCurve onLineCurve = new OnLineCurve(gem.Center.Convert(), gem.Center.Convert() + on3dVector * 50.0);
			onLineCurve.Rotate(CSBlockUtil.CSUtilMath.ToRadians(num), gem.Up.Convert(), gem.Center.Convert());
			ArrayOnX_EVENT x = new ArrayOnX_EVENT();
			onNurbsCurve.IntersectCurve(onLineCurve, ref x);
			if (x.Count() <= 0)
			{
				break;
			}
			double num2 = x[0].get_m_a(0);
			On3dVector on3dVector2 = CSBlockUtil.CSUtilMath.AverageTangentAt(onNurbsCurve, num2);
			on3dVector2.Unitize();
			On3dVector on3dVector3 = onNurbsCurve.PointAt(num2) - gem.Center.Convert();
			On3dVector on3dVector4 = OnUtil.ON_CrossProduct(gem.Up.Convert(), on3dVector2);
			on3dVector4.Unitize();
			on3dVector4 *= prongDiameter / 2.0 * (1.0 - nudgePer);
			On3dVector on3dVector5 = gem.Up.Convert();
			on3dVector5.Unitize();
			on3dVector5 *= placement;
			OnXform onXform = new OnXform();
			onXform.Translation(on3dVector3 + on3dVector4 + on3dVector5);
			if (num < 0.0)
			{
				num += 360.0;
			}
			double angleDegrees = ((!(OnUtil.ON_DotProduct(gem.ForwardVector.Convert(), OnUtil.On_xaxis) > 0.0)) ? ((num != 0.0) ? ((num > 0.0 && num < 180.0) ? 180.0 : ((num != 180.0) ? 0.0 : 270.0)) : 90.0) : ((num != 0.0) ? ((num > 0.0 && num < 180.0) ? 0.0 : ((num != 180.0) ? 180.0 : 90.0)) : 270.0));
			OnXform onXform2 = new OnXform();
			onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), OnUtil.On_zaxis, OnUtil.On_origin);
			prongTransform.Add(xUp * onXform * onXform2);
		}
	}
}
