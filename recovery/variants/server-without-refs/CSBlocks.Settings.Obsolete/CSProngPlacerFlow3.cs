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

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ProngPlacerFlow_V3", "{A119BF8F-DE45-4362-BB4D-2B45F59E07A7}")]
public sealed class CSProngPlacerFlow3 : CSBlock
{
	public CSOutputChannel oSurface { get; }

	public CSInputChannel iSortedGem { get; }

	public CSInputChannel iLargeDiameter { get; }

	public CSInputChannel iLargeHeight { get; }

	public CSInputChannel iLargeFillet { get; }

	public CSInputChannel iLargeTaper { get; }

	public CSInputChannel iSmallDiameter { get; }

	public CSInputChannel iSmallHeight { get; }

	public CSInputChannel iSmallFillet { get; }

	public CSInputChannel iSmallTaper { get; }

	public CSInputChannel iNudge { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iEndControls { get; }

	public CSInputChannel iEndControlsMirror { get; }

	public CSInputChannel iEndAngleA { get; }

	public CSInputChannel iEndNudgeA { get; }

	public CSInputChannel iEndSpreadA { get; }

	public CSInputChannel iEndAngleB { get; }

	public CSInputChannel iEndNudgeB { get; }

	public CSInputChannel iEndSpreadB { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iMeshedProngs { get; }

	public CSProngPlacerFlow3(string identifier)
		: base(identifier)
	{
		iSortedGem = new CSInputChannel(GetPropertyName(() => iSortedGem), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iLargeDiameter = new CSInputChannel(GetPropertyName(() => iLargeDiameter), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLargeHeight = new CSInputChannel(GetPropertyName(() => iLargeHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLargeFillet = new CSInputChannel(GetPropertyName(() => iLargeFillet), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLargeTaper = new CSInputChannel(GetPropertyName(() => iLargeTaper), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSmallDiameter = new CSInputChannel(GetPropertyName(() => iSmallDiameter), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSmallHeight = new CSInputChannel(GetPropertyName(() => iSmallHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSmallFillet = new CSInputChannel(GetPropertyName(() => iSmallFillet), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSmallTaper = new CSInputChannel(GetPropertyName(() => iSmallTaper), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iNudge = new CSInputChannel(GetPropertyName(() => iNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndControls = new CSInputChannel(GetPropertyName(() => iEndControls), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iEndControlsMirror = new CSInputChannel(GetPropertyName(() => iEndControlsMirror), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iEndAngleA = new CSInputChannel(GetPropertyName(() => iEndAngleA), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndNudgeA = new CSInputChannel(GetPropertyName(() => iEndNudgeA), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndSpreadA = new CSInputChannel(GetPropertyName(() => iEndSpreadA), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndAngleB = new CSInputChannel(GetPropertyName(() => iEndAngleB), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndNudgeB = new CSInputChannel(GetPropertyName(() => iEndNudgeB), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndSpreadB = new CSInputChannel(GetPropertyName(() => iEndSpreadB), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iMeshedProngs = new CSInputChannel(GetPropertyName(() => iMeshedProngs), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0982: Expected O, but got Unknown
		//IL_0984: Unknown result type (might be due to invalid IL or missing references)
		//IL_098e: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Expected O, but got Unknown
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Expected O, but got Unknown
		//IL_0d3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d52: Expected O, but got Unknown
		//IL_0d54: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Expected O, but got Unknown
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Expected O, but got Unknown
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7c: Expected O, but got Unknown
		//IL_0d7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d94: Expected O, but got Unknown
		//IL_0d96: Unknown result type (might be due to invalid IL or missing references)
		if (!(iSortedGem.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		OnNurbsCurve val = iProfile.GetNurbsCurve();
		if (val == null)
		{
			OnCircle val2 = new OnCircle(OnUtil.On_xy_plane, OnUtil.On_origin, 1.0);
			val = new OnNurbsCurve();
			val2.GetNurbForm(ref val);
			if (val == null)
			{
				return UpdateMissing("Profile");
			}
		}
		double value = 0.0;
		iLargeDiameter.GetDouble(ref value);
		double value2 = 0.0;
		iLargeHeight.GetDouble(ref value2);
		double value3 = 0.0;
		iLargeFillet.GetDouble(ref value3);
		value3 /= 100.0;
		double value4 = 0.0;
		iLargeTaper.GetDouble(ref value4);
		value4 /= 100.0;
		double value5 = 0.0;
		iSmallDiameter.GetDouble(ref value5);
		double value6 = 0.0;
		iSmallHeight.GetDouble(ref value6);
		double value7 = 0.0;
		iSmallFillet.GetDouble(ref value7);
		value7 /= 100.0;
		double value8 = 0.0;
		iSmallTaper.GetDouble(ref value8);
		value8 /= 100.0;
		bool flag = value5 != 0.0 || value6 != 0.0 || value7 != 0.0 || value8 != 0.0;
		double value9 = 0.0;
		iNudge.GetDouble(ref value9);
		value9 = value9 / 100.0 * 2.0;
		double value10 = 0.0;
		iPlacement.GetDouble(ref value10);
		int value11 = 0;
		iStyle.GetInteger(ref value11);
		bool value12 = true;
		iEndControls.GetBoolean(ref value12);
		bool value13 = true;
		iEndControlsMirror.GetBoolean(ref value13);
		double value14 = 0.0;
		double value15 = value9;
		double num = 100.0;
		num /= 100.0;
		double value16 = value14;
		double value17 = value15;
		double value18 = num;
		if (value12)
		{
			iEndAngleA.GetDouble(ref value14);
			iEndNudgeA.GetDouble(ref value15);
			value15 = value15 / 100.0 * 2.0;
			iEndSpreadA.GetDouble(ref num);
			num /= 100.0;
			if (value13)
			{
				iEndAngleB.GetDouble(ref value16);
				iEndNudgeB.GetDouble(ref value17);
				value17 = value17 / 100.0 * 2.0;
				iEndSpreadB.GetDouble(ref value18);
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
		iMeshedProngs.GetBoolean(ref value19);
		CSDataArray placedProngs = new CSDataArray();
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
		CSData prong = null;
		bool flag2 = CSUtilGemRails.IsClosed(cSDataArray.Array);
		for (int j = 0; j < cSDataArray.GetArrayCount() || (j == cSDataArray.GetArrayCount() && flag2); j++)
		{
			CSChannelGemData channelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			if (channelGemData2 == null)
			{
				continue;
			}
			CSDataHelper.SetSettingType(channelGemData2, GemSettingType.PR);
			CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)channelGemData2.Xform.Convert(), out var xUp, out var xDown);
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
			double num6 = value2;
			double filletPer = value3;
			double taperPer = value4;
			if (flag)
			{
				double num7 = ((cSChannelGemData.GemXSize + cSChannelGemData.GemYSize) / 2.0 - num2) / (num3 - num2);
				num5 = num7 * (value - value5) + value5;
				num6 = num7 * (value2 - value6) + value6;
				filletPer = num7 * (value3 - value7) + value7;
				taperPer = num7 * (value4 - value8) + value8;
			}
			if (CSApplicationHelper.Application.IsCamMode)
			{
				num6 += CSApplicationHelper.Application.CamProngExtensionHeight;
			}
			if (flag || j == 0)
			{
				if (value19)
				{
					if (!CSUtilGeometry.CreateMeshProng((IOnNurbsCurve)(object)val, num5, num6, filletPer, taperPer, out var meshSurface))
					{
						return UpdateError("CreateMeshProng failed");
					}
					prong = new CSOnGeometryData((GeometryBase)(object)meshSurface.Convert());
				}
				else
				{
					if (!CSUtilGeometry.CreateProng((IOnNurbsCurve)(object)val, num5, num6, filletPer, taperPer, out var nurbsSurface))
					{
						return UpdateError("CreateProng failed");
					}
					prong = new CSOnGeometryData((GeometryBase)(object)nurbsSurface.Convert());
				}
			}
			OnCurve val3 = CSUtilGemRails.Rail(cSDataArray);
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
			{
				OnCurve val5 = null;
				if (val3 != null)
				{
					val5 = val3.DuplicateCurve();
					((OnGeometry)val5).Transform((IOnXform)(object)xDown);
				}
				PlaceProngs(cSChannelGemData, cSChannelGemData3, (IOnCurve)(object)val5, num5, value9, value10, doLhs: true, doRhs: true, prong, xUp, ref placedProngs);
				break;
			}
			case 20:
			{
				OnCurve val6 = null;
				if (val3 != null)
				{
					val6 = val3.DuplicateCurve();
					((OnGeometry)val6).Transform((IOnXform)(object)xDown);
				}
				PlaceProngs(cSChannelGemData, cSChannelGemData3, (IOnCurve)(object)val6, num5, value9, value10, doLhs: false, doRhs: true, prong, xUp, ref placedProngs);
				break;
			}
			case 21:
			{
				OnCurve val4 = null;
				if (val3 != null)
				{
					val4 = val3.DuplicateCurve();
					((OnGeometry)val4).Transform((IOnXform)(object)xDown);
				}
				PlaceProngs(cSChannelGemData, cSChannelGemData3, (IOnCurve)(object)val4, num5, value9, value10, doLhs: true, doRhs: false, prong, xUp, ref placedProngs);
				break;
			}
			}
			switch (value11)
			{
			case 0:
				if (flag2)
				{
					PlaceProngs(cSChannelGemData, 0.0, 180.0, 2, num5, value9, value10, prong, xUp, ref placedProngs);
					break;
				}
				if (j != 0)
				{
					PlaceProngs(cSChannelGemData, 180.0, 0.0, 1, num5, value9, value10, prong, xUp, ref placedProngs);
				}
				if (j != cSDataArray.GetArrayCount() - 1)
				{
					PlaceProngs(cSChannelGemData, 0.0, 0.0, 1, num5, value9, value10, prong, xUp, ref placedProngs);
				}
				break;
			case 1:
				PlaceProngs(cSChannelGemData, 90.0, 180.0, 2, num5, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 2:
				PlaceProngs(cSChannelGemData, 45.0, 180.0, 2, num5, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 3:
				PlaceProngs(cSChannelGemData, 135.0, 180.0, 2, num5, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 4:
			case 5:
			case 14:
			case 15:
			case 16:
			case 17:
				if (flag2)
				{
					PlaceProngs(cSChannelGemData, 45.0, 90.0, 4, num5, value9, value10, prong, xUp, ref placedProngs);
					break;
				}
				if (j != 0)
				{
					PlaceProngs(cSChannelGemData, 135.0, 90.0, 2, num5, value9, value10, prong, xUp, ref placedProngs);
				}
				if (j != cSDataArray.GetArrayCount() - 1)
				{
					PlaceProngs(cSChannelGemData, -45.0, 90.0, 2, num5, value9, value10, prong, xUp, ref placedProngs);
				}
				break;
			case 10:
			case 18:
				PlaceProngs(cSChannelGemData, 90.0, 0.0, 1, num5, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 11:
			case 19:
				PlaceProngs(cSChannelGemData, 270.0, 0.0, 1, num5, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 12:
				PlaceProngs(cSChannelGemData, 45.0, 90.0, 2, num5, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 13:
				PlaceProngs(cSChannelGemData, 225.0, 90.0, 2, num5, value9, value10, prong, xUp, ref placedProngs);
				break;
			}
			if (j == 0 && !flag2)
			{
				double num8 = 0.0;
				if (cSChannelGemData3 != null)
				{
					On3dPoint val7 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					num8 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert()) - (IOn3dPoint)(object)val7), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
				}
				if (cSChannelGemData2 != null)
				{
					On3dPoint val8 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					num8 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dPoint)(object)val8), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
				}
				if (num8 > 0.0)
				{
					switch (value11)
					{
					case 0:
					case 4:
					case 6:
					case 7:
					case 10:
					case 11:
					case 12:
					case 13:
					{
						double startingAngleDeg2 = 180.0 + value16;
						PlaceProngs(cSChannelGemData, startingAngleDeg2, 0.0, 1, num5, value17, value10, prong, xUp, ref placedProngs);
						break;
					}
					case 5:
					case 8:
					case 9:
					case 14:
					case 15:
						if (value18 > 0.0)
						{
							double startingAngleDeg3 = 135.0 + value16 + 90.0 * (1.0 - value18) / 2.0;
							double spacingAngleDeg = 90.0 * value18;
							PlaceProngs(cSChannelGemData, startingAngleDeg3, spacingAngleDeg, 2, num5, value17, value10, prong, xUp, ref placedProngs);
						}
						else
						{
							double startingAngleDeg3 = 180.0 + value16;
							PlaceProngs(cSChannelGemData, startingAngleDeg3, 0.0, 1, num5, value17, value10, prong, xUp, ref placedProngs);
						}
						break;
					case 16:
					case 17:
					{
						double startingAngleDeg = 180.0 + value16;
						PlaceProngs(cSChannelGemData, startingAngleDeg, 0.0, 1, num5, value17, value10, prong, xUp, ref placedProngs);
						if (value18 > 0.0)
						{
							PlaceProngs(cSChannelGemData, 135.0, 90.0, 2, num5, value9, value10, prong, xUp, ref placedProngs);
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
					case 10:
					case 11:
					case 12:
					case 13:
					{
						double startingAngleDeg5 = 0.0 + value14;
						PlaceProngs(cSChannelGemData, startingAngleDeg5, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
						break;
					}
					case 5:
					case 8:
					case 9:
					case 14:
					case 15:
						if (num > 0.0)
						{
							double startingAngleDeg6 = -45.0 + value14 + 90.0 * (1.0 - num) / 2.0;
							double spacingAngleDeg2 = 90.0 * num;
							PlaceProngs(cSChannelGemData, startingAngleDeg6, spacingAngleDeg2, 2, num5, value15, value10, prong, xUp, ref placedProngs);
						}
						else
						{
							double startingAngleDeg6 = 0.0 + value14;
							PlaceProngs(cSChannelGemData, startingAngleDeg6, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
						}
						break;
					case 16:
					case 17:
					{
						double startingAngleDeg4 = 0.0 + value14;
						PlaceProngs(cSChannelGemData, startingAngleDeg4, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
						if (num > 0.0)
						{
							PlaceProngs(cSChannelGemData, -45.0, 90.0, 2, num5, value9, value10, prong, xUp, ref placedProngs);
						}
						break;
					}
					}
				}
			}
			if (j != cSDataArray.GetArrayCount() - 1 || flag2)
			{
				continue;
			}
			double num9 = 0.0;
			if (cSChannelGemData3 != null)
			{
				On3dPoint val9 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert());
				num9 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert()) - (IOn3dPoint)(object)val9), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
			}
			if (cSChannelGemData2 != null)
			{
				On3dPoint val10 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
				num9 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dPoint)(object)val10), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
			}
			if (num9 > 0.0)
			{
				switch (value11)
				{
				case 0:
				case 4:
				case 6:
				case 8:
				case 10:
				case 11:
				case 12:
				case 13:
				{
					double startingAngleDeg8 = 0.0 + value14;
					PlaceProngs(cSChannelGemData, startingAngleDeg8, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
					break;
				}
				case 5:
				case 7:
				case 9:
				case 14:
				case 16:
					if (num > 0.0)
					{
						double startingAngleDeg9 = -45.0 + value14 + 90.0 * (1.0 - num) / 2.0;
						double spacingAngleDeg3 = 90.0 * num;
						PlaceProngs(cSChannelGemData, startingAngleDeg9, spacingAngleDeg3, 2, num5, value15, value10, prong, xUp, ref placedProngs);
					}
					else
					{
						double startingAngleDeg9 = 0.0 + value14;
						PlaceProngs(cSChannelGemData, startingAngleDeg9, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
					}
					break;
				case 15:
				case 17:
				{
					double startingAngleDeg7 = 0.0 + value14;
					PlaceProngs(cSChannelGemData, startingAngleDeg7, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
					if (num > 0.0)
					{
						PlaceProngs(cSChannelGemData, -45.0, 90.0, 2, num5, value9, value10, prong, xUp, ref placedProngs);
					}
					break;
				}
				}
				continue;
			}
			switch (value11)
			{
			case 0:
			case 4:
			case 6:
			case 8:
			case 10:
			case 11:
			case 12:
			case 13:
			{
				double startingAngleDeg11 = 180.0 + value16;
				PlaceProngs(cSChannelGemData, startingAngleDeg11, 0.0, 1, num5, value17, value10, prong, xUp, ref placedProngs);
				break;
			}
			case 5:
			case 7:
			case 9:
			case 14:
			case 16:
				if (value18 > 0.0)
				{
					double startingAngleDeg12 = 135.0 + value16 + 90.0 * (1.0 - value18) / 2.0;
					double spacingAngleDeg4 = 90.0 * value18;
					PlaceProngs(cSChannelGemData, startingAngleDeg12, spacingAngleDeg4, 2, num5, value17, value10, prong, xUp, ref placedProngs);
				}
				else
				{
					double startingAngleDeg12 = 180.0 + value16;
					PlaceProngs(cSChannelGemData, startingAngleDeg12, 0.0, 1, num5, value17, value10, prong, xUp, ref placedProngs);
				}
				break;
			case 15:
			case 17:
			{
				double startingAngleDeg10 = 180.0 + value16;
				PlaceProngs(cSChannelGemData, startingAngleDeg10, 0.0, 1, num5, value17, value10, prong, xUp, ref placedProngs);
				if (value18 > 0.0)
				{
					PlaceProngs(cSChannelGemData, 135.0, 90.0, 2, num5, value17, value10, prong, xUp, ref placedProngs);
				}
				break;
			}
			}
		}
		oSurface.SetData(placedProngs);
		return base.Update();
	}

	private static void PlaceProngs(CSChannelGemData gem, double startingAngleDeg, double spacingAngleDeg, int prongCount, double prongDiameter, double nudgePer, double placement, CSData prong, OnXform xUp, ref CSDataArray placedProngs)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		if (gem == null)
		{
			return;
		}
		OnNurbsCurve val = gem.GetNurbsCurve().Convert();
		On3dVector val2 = new On3dVector((IOn3dVector)(object)gem.ForwardVector.Convert())
		{
			z = 0.0
		};
		OnLineCurve val3 = new OnLineCurve((IOn3dPoint)(object)gem.Center.Convert(), (IOn3dPoint)(object)(new On3dPoint((IOn3dPoint)(object)gem.Center.Convert()) + (IOn3dVector)(object)(val2 * 50.0)));
		((OnGeometry)val3).Rotate(CSBlockUtil.CSUtilMath.ToRadians(startingAngleDeg), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)gem.Center.Convert());
		for (int i = 0; i < prongCount; i++)
		{
			ArrayOnX_EVENT val4 = new ArrayOnX_EVENT();
			((OnCurve)val).IntersectCurve((IOnCurve)(object)val3, ref val4);
			if (val4.Count() <= 0)
			{
				break;
			}
			double num = val4[0][0];
			On3dVector val5 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)val, num);
			val5.Unitize();
			On3dVector val6 = ((OnCurve)val).PointAt(num) - (IOn3dPoint)(object)gem.Center.Convert();
			On3dVector val7 = OnUtil.ON_CrossProduct((IOn3dVector)(object)gem.Up.Convert(), (IOn3dVector)(object)val5);
			val7.Unitize();
			val7 *= prongDiameter / 2.0 * (1.0 - nudgePer);
			On3dVector val8 = new On3dVector((IOn3dVector)(object)gem.Up.Convert());
			val8.Unitize();
			val8 *= placement;
			OnXform val9 = new OnXform();
			val9.Translation((IOn3dVector)(object)(val6 + (IOn3dVector)(object)val7 + (IOn3dVector)(object)val8));
			double num2 = CSBlockUtil.CSUtilMath.ToRadians(90.0) - CSBlockUtil.CSUtilMath.AngleOfPointD(val5.x, val5.y);
			new OnXform().Rotation(num2, OnUtil.On_zaxis, OnUtil.On_origin);
			CSData data = prong.Duplicate();
			CSDataHelper.Transform(data, xUp.Convert() * val9.Convert());
			placedProngs.Append(data);
			((OnGeometry)val3).Rotate(CSBlockUtil.CSUtilMath.ToRadians(spacingAngleDeg), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)gem.Center.Convert());
		}
	}

	private static void PlaceProngs(CSChannelGemData gemLhs, CSGemData gemRhs, IOnCurve rail, double prongDiameter, double nudgePer, double placement, bool doLhs, bool doRhs, CSData prong, OnXform xUp, ref CSDataArray placedProngs)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		if (gemLhs == null || gemRhs == null || rail == null)
		{
			return;
		}
		double num = 0.0;
		OnLineCurve val = new OnLineCurve((IOn3dPoint)(object)gemLhs.Center.Convert(), (IOn3dPoint)(object)gemRhs.Center.Convert());
		On3dPoint val2 = ((OnCurve)val).PointAt(((OnCurve)val).Domain().Mid());
		rail.GetClosestPoint((IOn3dPoint)(object)val2, ref num);
		On3dPoint val3 = rail.PointAt(num);
		OnNurbsCurve obj = gemLhs.GetNurbsCurve().Convert();
		((OnCurve)obj).GetClosestPoint((IOn3dPoint)(object)val2, ref num);
		On3dPoint val4 = ((OnCurve)obj).PointAt(num);
		OnNurbsCurve obj2 = gemRhs.GetNurbsCurve().Convert();
		((OnCurve)obj2).GetClosestPoint((IOn3dPoint)(object)val2, ref num);
		On3dPoint val5 = ((OnCurve)obj2).PointAt(num);
		double num2 = val4.DistanceTo((IOn3dPoint)(object)val5);
		double num3 = num2 + 2.0 * nudgePer * (prongDiameter / 2.0);
		On3dPoint val6 = new On3dPoint(val2.x, val2.y, val3.z);
		On3dVector val7 = new On3dVector((IOn3dVector)(object)gemLhs.Up.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)gemRhs.Up.Convert());
		val7.Unitize();
		if (prongDiameter <= num3)
		{
			On3dVector val8 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val7, (IOn3dVector)(object)(((OnCurve)val).PointAtStart() - (IOn3dPoint)(object)((OnCurve)val).PointAtEnd()));
			val8.Unitize();
			On3dPoint destination = val6 + (IOn3dVector)(object)(val7 * placement);
			OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)gemLhs.Center.Convert(), (IOn3dVector)(object)gemLhs.ForwardVector.Convert(), (IOn3dVector)(object)gemLhs.Up.Convert(), (IOn3dPoint)(object)destination, (IOn3dVector)(object)val8, (IOn3dVector)(object)val7);
			CSData data = prong.Duplicate();
			CSDataHelper.Transform(data, xUp.Convert() * xform.Convert());
			placedProngs.Append(data);
		}
		else if (doLhs || doRhs)
		{
			double num4 = val4.DistanceTo((IOn3dPoint)(object)gemLhs.Center.Convert());
			double num5 = num4 + num2 / 2.0;
			double num6 = num4 + prongDiameter / 2.0 - nudgePer * (prongDiameter / 2.0);
			double num7 = Math.Sqrt(num6 * num6 - num5 * num5);
			if (doLhs)
			{
				On3dVector val9 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val7, (IOn3dVector)(object)(((OnCurve)val).PointAtStart() - (IOn3dPoint)(object)((OnCurve)val).PointAtEnd()));
				val9.Unitize();
				On3dPoint destination2 = val6 + (IOn3dVector)(object)(val9 * num7) + (IOn3dVector)(object)(val7 * placement);
				OnXform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)destination2, (IOn3dVector)(object)val9, (IOn3dVector)(object)val7);
				CSData data2 = prong.Duplicate();
				CSDataHelper.Transform(data2, xUp.Convert() * xform2.Convert());
				placedProngs.Append(data2);
			}
			if (doRhs)
			{
				On3dVector val10 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val7, (IOn3dVector)(object)(((OnCurve)val).PointAtEnd() - (IOn3dPoint)(object)((OnCurve)val).PointAtStart()));
				val10.Unitize();
				On3dPoint destination3 = val6 + (IOn3dVector)(object)(val10 * num7) + (IOn3dVector)(object)(val7 * placement);
				OnXform xform3 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)destination3, (IOn3dVector)(object)val10, (IOn3dVector)(object)val7);
				CSData data3 = prong.Duplicate();
				CSDataHelper.Transform(data3, xUp.Convert() * xform3.Convert());
				placedProngs.Append(data3);
			}
		}
	}
}
