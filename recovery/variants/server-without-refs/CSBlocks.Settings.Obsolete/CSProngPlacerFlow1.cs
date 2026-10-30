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
[CSBlock("ProngPlacerFlow_V1", "{4B8F626F-7ACF-4427-8C0B-D643272518C0}")]
public sealed class CSProngPlacerFlow1 : CSBlock
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

	public CSInputChannel iEndSpread { get; }

	public CSInputChannel iEndAngleA { get; }

	public CSInputChannel iEndAngleB { get; }

	public CSOutputChannel oSurface { get; }

	public CSProngPlacerFlow1(string identifier)
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
		iEndSpread = new CSInputChannel(GetPropertyName(() => iEndSpread), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndAngleA = new CSInputChannel(GetPropertyName(() => iEndAngleA), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndAngleB = new CSInputChannel(GetPropertyName(() => iEndAngleB), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Expected O, but got Unknown
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Expected O, but got Unknown
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a79: Expected O, but got Unknown
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a91: Expected O, but got Unknown
		//IL_0a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Expected O, but got Unknown
		//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Expected O, but got Unknown
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Expected O, but got Unknown
		//IL_0ad5: Unknown result type (might be due to invalid IL or missing references)
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
		double value12 = 0.0;
		if (!iEndSpread.GetDouble(ref value12))
		{
			value12 = 100.0;
		}
		value12 /= 100.0;
		double value13 = 0.0;
		iEndAngleA.GetDouble(ref value13);
		double value14 = 0.0;
		iEndAngleB.GetDouble(ref value14);
		CSDataArray placedProngs = new CSDataArray();
		double num = 0.0;
		double num2 = 0.0;
		if (flag)
		{
			for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
			{
				CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
				if (channelGemData != null)
				{
					double num3 = (channelGemData.GemXSize + channelGemData.GemYSize) / 2.0;
					if (num3 < num || i == 0)
					{
						num = num3;
					}
					if (num3 > num2 || i == 0)
					{
						num2 = num3;
					}
				}
			}
			if (Math.Abs(num2 - num) < 0.001)
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
			double num4 = value;
			double num5 = value2;
			double filletPer = value3;
			double taperPer = value4;
			if (flag)
			{
				double num6 = ((cSChannelGemData.GemXSize + cSChannelGemData.GemYSize) / 2.0 - num) / (num2 - num);
				num4 = num6 * (value - value5) + value5;
				num5 = num6 * (value2 - value6) + value6;
				filletPer = num6 * (value3 - value7) + value7;
				taperPer = num6 * (value4 - value8) + value8;
			}
			if (CSApplicationHelper.Application.IsCamMode)
			{
				num5 += CSApplicationHelper.Application.CamProngExtensionHeight;
			}
			if (flag || j == 0)
			{
				OnNurbsSurface nurbsSurface = new OnNurbsSurface();
				if (!CSUtilGeometry.CreateProng_Old_V1((IOn3dVector)(object)cSChannelGemData.Up.Convert(), num4, num5, filletPer, taperPer, ref nurbsSurface))
				{
					return UpdateError("CreateProng failed");
				}
				prong = new CSOnGeometryData((GeometryBase)(object)nurbsSurface.Convert());
			}
			OnCurve val = CSUtilGemRails.Rail(cSDataArray);
			if ((uint)(value11 - 11) <= 6u)
			{
				OnCurve val2 = val.DuplicateCurve();
				((OnGeometry)val2).Transform((IOnXform)(object)xDown);
				PlaceProngs(cSChannelGemData, cSChannelGemData3, val2, num4, value9, value10, prong, xUp, ref placedProngs);
			}
			switch (value11)
			{
			case 2:
			case 3:
			case 4:
				if (flag2)
				{
					PlaceProngs(cSChannelGemData, 0.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
					break;
				}
				if (j != 0)
				{
					PlaceProngs(cSChannelGemData, 180.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
				}
				if (j != cSDataArray.GetArrayCount() - 1)
				{
					PlaceProngs(cSChannelGemData, 0.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
				}
				break;
			case 5:
				PlaceProngs(cSChannelGemData, 90.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 6:
				PlaceProngs(cSChannelGemData, 45.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 7:
				PlaceProngs(cSChannelGemData, 135.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 8:
			case 9:
			case 10:
				if (flag2)
				{
					PlaceProngs(cSChannelGemData, 45.0, 90.0, 4, num4, value9, value10, prong, xUp, ref placedProngs);
					break;
				}
				if (j != 0)
				{
					PlaceProngs(cSChannelGemData, 135.0, 90.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				}
				if (j != cSDataArray.GetArrayCount() - 1)
				{
					PlaceProngs(cSChannelGemData, -45.0, 90.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				}
				break;
			case 14:
				PlaceProngs(cSChannelGemData, 90.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 15:
				PlaceProngs(cSChannelGemData, 270.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 16:
				PlaceProngs(cSChannelGemData, 45.0, 90.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 17:
				PlaceProngs(cSChannelGemData, 225.0, 90.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			}
			if (j == 0 && !flag2)
			{
				double num7 = 0.0;
				if (cSChannelGemData3 != null)
				{
					On3dPoint val3 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					num7 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert()) - (IOn3dPoint)(object)val3), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
				}
				if (cSChannelGemData2 != null)
				{
					On3dPoint val4 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					num7 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dPoint)(object)val4), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
				}
				if (num7 > 0.0)
				{
					switch (value11)
					{
					case 0:
					case 3:
					case 9:
					case 12:
					case 14:
					case 15:
					case 16:
					case 17:
						PlaceProngs(cSChannelGemData, 180.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
						break;
					case 1:
					case 4:
					case 10:
					case 13:
						if (value12 > 0.0)
						{
							double startingAngleDeg = 135.0 + value14 + 90.0 * (1.0 - value12) / 2.0;
							double spacingAngleDeg = 90.0 * value12;
							PlaceProngs(cSChannelGemData, startingAngleDeg, spacingAngleDeg, 2, num4, value9, value10, prong, xUp, ref placedProngs);
						}
						else
						{
							PlaceProngs(cSChannelGemData, 180.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
						}
						break;
					}
				}
				else
				{
					switch (value11)
					{
					case 0:
					case 3:
					case 9:
					case 12:
					case 14:
					case 15:
					case 16:
					case 17:
						PlaceProngs(cSChannelGemData, 0.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
						break;
					case 1:
					case 4:
					case 10:
					case 13:
						if (value12 > 0.0)
						{
							double startingAngleDeg2 = -45.0 + value13 + 90.0 * (1.0 - value12) / 2.0;
							double spacingAngleDeg2 = 90.0 * value12;
							PlaceProngs(cSChannelGemData, startingAngleDeg2, spacingAngleDeg2, 2, num4, value9, value10, prong, xUp, ref placedProngs);
						}
						else
						{
							PlaceProngs(cSChannelGemData, 0.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
						}
						break;
					}
				}
			}
			if (j != cSDataArray.GetArrayCount() - 1 || flag2)
			{
				continue;
			}
			double num8 = 0.0;
			if (cSChannelGemData3 != null)
			{
				On3dPoint val5 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert());
				num8 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert()) - (IOn3dPoint)(object)val5), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
			}
			if (cSChannelGemData2 != null)
			{
				On3dPoint val6 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
				num8 = OnUtil.ON_DotProduct((IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dPoint)(object)val6), (IOn3dVector)(object)cSChannelGemData.ForwardVector.Convert());
			}
			if (num8 > 0.0)
			{
				switch (value11)
				{
				case 0:
				case 3:
				case 9:
				case 12:
				case 14:
				case 15:
				case 16:
				case 17:
					PlaceProngs(cSChannelGemData, 0.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
					break;
				case 1:
				case 4:
				case 10:
				case 13:
					if (value12 > 0.0)
					{
						double startingAngleDeg3 = -45.0 + value13 + 90.0 * (1.0 - value12) / 2.0;
						double spacingAngleDeg3 = 90.0 * value12;
						PlaceProngs(cSChannelGemData, startingAngleDeg3, spacingAngleDeg3, 2, num4, value9, value10, prong, xUp, ref placedProngs);
					}
					else
					{
						PlaceProngs(cSChannelGemData, 0.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
					}
					break;
				}
				continue;
			}
			switch (value11)
			{
			case 0:
			case 3:
			case 9:
			case 12:
			case 14:
			case 15:
			case 16:
			case 17:
				PlaceProngs(cSChannelGemData, 180.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 1:
			case 4:
			case 10:
			case 13:
				if (value12 > 0.0)
				{
					double startingAngleDeg4 = 135.0 + value14 + 90.0 * (1.0 - value12) / 2.0;
					double spacingAngleDeg4 = 90.0 * value12;
					PlaceProngs(cSChannelGemData, startingAngleDeg4, spacingAngleDeg4, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				}
				else
				{
					PlaceProngs(cSChannelGemData, 180.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
				}
				break;
			}
		}
		oSurface.SetData(placedProngs);
		return base.Update();
	}

	private bool PlaceProngs(CSChannelGemData gem, double startingAngleDeg, double spacingAngleDeg, int prongCount, double prongDiameter, double nudgePer, double placement, CSData prong, OnXform xUp, ref CSDataArray placedProngs)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		if (gem == null)
		{
			return false;
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
				return false;
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
		return true;
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
