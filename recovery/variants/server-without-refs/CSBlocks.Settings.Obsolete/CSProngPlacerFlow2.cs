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
[CSBlock("ProngPlacerFlow_V2", "{5F238BAE-D259-499f-8A8B-C282A18108EF}")]
public sealed class CSProngPlacerFlow2 : CSBlock
{
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

	public CSOutputChannel oSurface { get; }

	public CSProngPlacerFlow2(string identifier)
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
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Expected O, but got Unknown
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Expected O, but got Unknown
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1e: Expected O, but got Unknown
		//IL_0c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c36: Expected O, but got Unknown
		//IL_0c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Expected O, but got Unknown
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c60: Expected O, but got Unknown
		//IL_0c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c78: Expected O, but got Unknown
		//IL_0c7a: Unknown result type (might be due to invalid IL or missing references)
		if (!(iSortedGem.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Prong Placer:  Missing Gem\n");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Prong Placer:  Missing Gem\n");
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
			OnXform xDown = null;
			OnXform xUp = null;
			CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)channelGemData2.Xform.Convert(), out xUp, out xDown);
			if (!(channelGemData2.Duplicate() is CSChannelGemData cSChannelGemData))
			{
				continue;
			}
			cSChannelGemData.ApplyTransform(xDown.Convert());
			CSChannelGemData cSChannelGemData2 = null;
			if (j - 1 >= 0)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, j - 1).Duplicate() as CSChannelGemData;
				cSChannelGemData2.ApplyTransform(xDown.Convert());
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (j + 1 <= cSDataArray.GetArrayCount() - 1)
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j + 1).Duplicate() as CSChannelGemData;
				cSChannelGemData3.ApplyTransform(xDown.Convert());
			}
			else if (j + 1 == cSDataArray.GetArrayCount() && flag2)
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Duplicate() as CSChannelGemData;
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
				OnNurbsSurface nurbsSurface = new OnNurbsSurface();
				if (!CSUtilGeometry.CreateProng_Old_V1((IOn3dVector)(object)cSChannelGemData.Up.Convert(), num5, num6, filletPer, taperPer, ref nurbsSurface))
				{
					return UpdateError("CreateProng failed");
				}
				prong = new CSOnGeometryData((GeometryBase)(object)nurbsSurface.Convert());
			}
			OnCurve val = CSUtilGemRails.Rail(cSDataArray);
			if ((uint)(value11 - 6) <= 11u)
			{
				OnCurve val2 = val.DuplicateCurve();
				((OnGeometry)val2).Transform((IOnXform)(object)xDown);
				PlaceProngs(cSChannelGemData, cSChannelGemData3, val2, num5, value9, value10, prong, xUp, ref placedProngs);
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
				PlaceProngs(cSChannelGemData, 90.0, 0.0, 1, num5, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 11:
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
					On3dPoint val3 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					num8 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert()) - (IOn3dPoint)(object)val3), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
				}
				if (cSChannelGemData2 != null)
				{
					On3dPoint val4 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					num8 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dPoint)(object)val4), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
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
					{
						double num9 = 0.0;
						double num10 = 0.0;
						if (value18 > 0.0)
						{
							num9 = 135.0 + value16 + 90.0 * (1.0 - value18) / 2.0;
							num10 = 90.0 * value18;
							PlaceProngs(cSChannelGemData, num9, num10, 2, num5, value17, value10, prong, xUp, ref placedProngs);
						}
						else
						{
							num9 = 180.0 + value16;
							PlaceProngs(cSChannelGemData, num9, 0.0, 1, num5, value17, value10, prong, xUp, ref placedProngs);
						}
						break;
					}
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
						double startingAngleDeg4 = 0.0 + value14;
						PlaceProngs(cSChannelGemData, startingAngleDeg4, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
						break;
					}
					case 5:
					case 8:
					case 9:
					case 14:
					case 15:
					{
						double num11 = 0.0;
						double num12 = 0.0;
						if (num > 0.0)
						{
							num11 = -45.0 + value14 + 90.0 * (1.0 - num) / 2.0;
							num12 = 90.0 * num;
							PlaceProngs(cSChannelGemData, num11, num12, 2, num5, value15, value10, prong, xUp, ref placedProngs);
						}
						else
						{
							num11 = 0.0 + value14;
							PlaceProngs(cSChannelGemData, num11, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
						}
						break;
					}
					case 16:
					case 17:
					{
						double startingAngleDeg3 = 0.0 + value14;
						PlaceProngs(cSChannelGemData, startingAngleDeg3, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
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
			double num13 = 0.0;
			if (cSChannelGemData3 != null)
			{
				On3dPoint val5 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert());
				num13 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert()) - (IOn3dPoint)(object)val5), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
			}
			if (cSChannelGemData2 != null)
			{
				On3dPoint val6 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
				num13 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dPoint)(object)val6), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
			}
			if (num13 > 0.0)
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
					double startingAngleDeg6 = 0.0 + value14;
					PlaceProngs(cSChannelGemData, startingAngleDeg6, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
					break;
				}
				case 5:
				case 7:
				case 9:
				case 14:
				case 16:
				{
					double num14 = 0.0;
					double num15 = 0.0;
					if (num > 0.0)
					{
						num14 = -45.0 + value14 + 90.0 * (1.0 - num) / 2.0;
						num15 = 90.0 * num;
						PlaceProngs(cSChannelGemData, num14, num15, 2, num5, value15, value10, prong, xUp, ref placedProngs);
					}
					else
					{
						num14 = 0.0 + value14;
						PlaceProngs(cSChannelGemData, num14, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
					}
					break;
				}
				case 15:
				case 17:
				{
					double startingAngleDeg5 = 0.0 + value14;
					PlaceProngs(cSChannelGemData, startingAngleDeg5, 0.0, 1, num5, value15, value10, prong, xUp, ref placedProngs);
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
				double startingAngleDeg8 = 180.0 + value16;
				PlaceProngs(cSChannelGemData, startingAngleDeg8, 0.0, 1, num5, value17, value10, prong, xUp, ref placedProngs);
				break;
			}
			case 5:
			case 7:
			case 9:
			case 14:
			case 16:
			{
				double num16 = 0.0;
				double num17 = 0.0;
				if (value18 > 0.0)
				{
					num16 = 135.0 + value16 + 90.0 * (1.0 - value18) / 2.0;
					num17 = 90.0 * value18;
					PlaceProngs(cSChannelGemData, num16, num17, 2, num5, value17, value10, prong, xUp, ref placedProngs);
				}
				else
				{
					num16 = 180.0 + value16;
					PlaceProngs(cSChannelGemData, num16, 0.0, 1, num5, value17, value10, prong, xUp, ref placedProngs);
				}
				break;
			}
			case 15:
			case 17:
			{
				double startingAngleDeg7 = 180.0 + value16;
				PlaceProngs(cSChannelGemData, startingAngleDeg7, 0.0, 1, num5, value17, value10, prong, xUp, ref placedProngs);
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
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
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
			if (val4.Count() == 0)
			{
				break;
			}
			On3dVector val5 = ((OnCurve)val).PointAt(val4[0][0]) - (IOn3dPoint)(object)gem.Center.Convert();
			double num = val5.Length();
			val5.Unitize();
			On3dVector val6 = new On3dVector((IOn3dVector)(object)gem.Up.Convert());
			val6.Unitize();
			OnXform val7 = new OnXform();
			val7.Translation((IOn3dVector)(object)(val5 * (num + prongDiameter / 2.0 * (1.0 - nudgePer)) + (IOn3dVector)(object)(val6 * placement)));
			CSData data = prong.Duplicate();
			CSDataHelper.Transform(data, xUp.Convert() * val7.Convert());
			placedProngs.Append(data);
			((OnGeometry)val3).Rotate(CSBlockUtil.CSUtilMath.ToRadians(spacingAngleDeg), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)gem.Center.Convert());
		}
	}

	private bool PlaceProngs(CSChannelGemData gemLHS, CSChannelGemData gemRHS, OnCurve rail, double prongDiameter, double nudgePer, double placement, CSData prong, OnXform xUp, ref CSDataArray placedProngs)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_00f3: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		if (gemLHS == null || gemRHS == null)
		{
			return false;
		}
		double num = 0.0;
		OnLineCurve val = new OnLineCurve((IOn3dPoint)(object)gemLHS.Center.Convert(), (IOn3dPoint)(object)gemRHS.Center.Convert());
		On3dPoint val2 = ((OnCurve)val).PointAt(((OnCurve)val).Domain().Mid());
		rail.GetClosestPoint((IOn3dPoint)(object)val2, ref num);
		On3dPoint val3 = rail.PointAt(num);
		OnNurbsCurve obj = gemLHS.GetNurbsCurve().Convert();
		((OnCurve)obj).GetClosestPoint((IOn3dPoint)(object)val2, ref num);
		On3dPoint val4 = ((OnCurve)obj).PointAt(num);
		OnNurbsCurve obj2 = gemRHS.GetNurbsCurve().Convert();
		((OnCurve)obj2).GetClosestPoint((IOn3dPoint)(object)val2, ref num);
		On3dPoint val5 = ((OnCurve)obj2).PointAt(num);
		double num2 = val4.DistanceTo((IOn3dPoint)(object)val5);
		double num3 = num2 + 2.0 * nudgePer * (prongDiameter / 2.0);
		On3dPoint val6 = new On3dPoint(val2.x, val2.y, val3.z);
		On3dVector val7 = new On3dVector((IOn3dVector)(object)gemLHS.Up.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)gemRHS.Up.Convert());
		val7.Unitize();
		if (prongDiameter <= num3)
		{
			On3dVector val8 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val7, (IOn3dVector)(object)(((OnCurve)val).PointAtStart() - (IOn3dPoint)(object)((OnCurve)val).PointAtEnd()));
			val8.Unitize();
			On3dPoint destination = val6 + (IOn3dVector)(object)(val7 * placement);
			OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)gemLHS.Center.Convert(), (IOn3dVector)(object)gemLHS.ForwardVector.Convert(), (IOn3dVector)(object)gemLHS.Up.Convert(), (IOn3dPoint)(object)destination, (IOn3dVector)(object)val8, (IOn3dVector)(object)val7);
			CSData data = prong.Duplicate();
			CSDataHelper.Transform(data, xUp.Convert() * xform.Convert());
			placedProngs.Append(data);
		}
		else
		{
			double num4 = val4.DistanceTo((IOn3dPoint)(object)gemLHS.Center.Convert());
			double num5 = num4 + num2 / 2.0;
			double num6 = num4 + prongDiameter / 2.0 - nudgePer * (prongDiameter / 2.0);
			double num7 = Math.Sqrt(num6 * num6 - num5 * num5);
			On3dVector val9 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val7, (IOn3dVector)(object)(((OnCurve)val).PointAtStart() - (IOn3dPoint)(object)((OnCurve)val).PointAtEnd()));
			val9.Unitize();
			On3dPoint destination2 = val6 + (IOn3dVector)(object)(val9 * num7) + (IOn3dVector)(object)(val7 * placement);
			OnXform xform2 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)gemLHS.Center.Convert(), (IOn3dVector)(object)gemLHS.ForwardVector.Convert(), (IOn3dVector)(object)gemLHS.Up.Convert(), (IOn3dPoint)(object)destination2, (IOn3dVector)(object)val9, (IOn3dVector)(object)val7);
			CSData data2 = prong.Duplicate();
			CSDataHelper.Transform(data2, xUp.Convert() * xform2.Convert());
			placedProngs.Append(data2);
			On3dVector val10 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val7, (IOn3dVector)(object)(((OnCurve)val).PointAtEnd() - (IOn3dPoint)(object)((OnCurve)val).PointAtStart()));
			val10.Unitize();
			On3dPoint destination3 = val6 + (IOn3dVector)(object)(val10 * num7) + (IOn3dVector)(object)(val7 * placement);
			OnXform xform3 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)gemLHS.Center.Convert(), (IOn3dVector)(object)gemLHS.ForwardVector.Convert(), (IOn3dVector)(object)gemLHS.Up.Convert(), (IOn3dPoint)(object)destination3, (IOn3dVector)(object)val10, (IOn3dVector)(object)val7);
			data2 = prong.Duplicate();
			CSDataHelper.Transform(data2, xUp.Convert() * xform3.Convert());
			placedProngs.Append(data2);
		}
		return true;
	}
}
