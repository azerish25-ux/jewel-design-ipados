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
		if (!(iSortedGem.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		OnNurbsCurve nurbs_curve = iProfile.GetNurbsCurve();
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
					if (!CSUtilGeometry.CreateMeshProng(nurbs_curve, num5, num6, filletPer, taperPer, out var meshSurface))
					{
						return UpdateError("CreateMeshProng failed");
					}
					prong = new CSOnGeometryData(meshSurface.Convert());
				}
				else
				{
					if (!CSUtilGeometry.CreateProng(nurbs_curve, num5, num6, filletPer, taperPer, out var nurbsSurface))
					{
						return UpdateError("CreateProng failed");
					}
					prong = new CSOnGeometryData(nurbsSurface.Convert());
				}
			}
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
			{
				OnCurve onCurve3 = null;
				if (onCurve != null)
				{
					onCurve3 = onCurve.DuplicateCurve();
					onCurve3.Transform(xDown);
				}
				PlaceProngs(cSChannelGemData, cSChannelGemData3, onCurve3, num5, value9, value10, doLhs: true, doRhs: true, prong, xUp, ref placedProngs);
				break;
			}
			case 20:
			{
				OnCurve onCurve4 = null;
				if (onCurve != null)
				{
					onCurve4 = onCurve.DuplicateCurve();
					onCurve4.Transform(xDown);
				}
				PlaceProngs(cSChannelGemData, cSChannelGemData3, onCurve4, num5, value9, value10, doLhs: false, doRhs: true, prong, xUp, ref placedProngs);
				break;
			}
			case 21:
			{
				OnCurve onCurve2 = null;
				if (onCurve != null)
				{
					onCurve2 = onCurve.DuplicateCurve();
					onCurve2.Transform(xDown);
				}
				PlaceProngs(cSChannelGemData, cSChannelGemData3, onCurve2, num5, value9, value10, doLhs: true, doRhs: false, prong, xUp, ref placedProngs);
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
					On3dPoint on3dPoint = new On3dPoint(cSChannelGemData.Center.Convert());
					num8 = OnUtil.ON_DotProduct(new On3dPoint(cSChannelGemData3.Center.Convert()) - on3dPoint, cSChannelGemData.ForwardVector.Convert());
				}
				if (cSChannelGemData2 != null)
				{
					On3dPoint on3dPoint2 = new On3dPoint(cSChannelGemData2.Center.Convert());
					num8 = OnUtil.ON_DotProduct(new On3dPoint(cSChannelGemData.Center.Convert()) - on3dPoint2, cSChannelGemData.ForwardVector.Convert());
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
				On3dPoint on3dPoint3 = new On3dPoint(cSChannelGemData.Center.Convert());
				num9 = OnUtil.ON_DotProduct(new On3dPoint(cSChannelGemData3.Center.Convert()) - on3dPoint3, cSChannelGemData.ForwardVector.Convert());
			}
			if (cSChannelGemData2 != null)
			{
				On3dPoint on3dPoint4 = new On3dPoint(cSChannelGemData2.Center.Convert());
				num9 = OnUtil.ON_DotProduct(new On3dPoint(cSChannelGemData.Center.Convert()) - on3dPoint4, cSChannelGemData.ForwardVector.Convert());
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
		if (gem == null)
		{
			return;
		}
		OnNurbsCurve onNurbsCurve = gem.GetNurbsCurve().Convert();
		On3dVector on3dVector = new On3dVector(gem.ForwardVector.Convert())
		{
			z = 0.0
		};
		OnLineCurve onLineCurve = new OnLineCurve(gem.Center.Convert(), new On3dPoint(gem.Center.Convert()) + on3dVector * 50.0);
		onLineCurve.Rotate(CSBlockUtil.CSUtilMath.ToRadians(startingAngleDeg), gem.Up.Convert(), gem.Center.Convert());
		for (int i = 0; i < prongCount; i++)
		{
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
			On3dVector on3dVector5 = new On3dVector(gem.Up.Convert());
			on3dVector5.Unitize();
			on3dVector5 *= placement;
			OnXform onXform = new OnXform();
			onXform.Translation(on3dVector3 + on3dVector4 + on3dVector5);
			double angle_radians = CSBlockUtil.CSUtilMath.ToRadians(90.0) - CSBlockUtil.CSUtilMath.AngleOfPointD(on3dVector2.x, on3dVector2.y);
			new OnXform().Rotation(angle_radians, OnUtil.On_zaxis, OnUtil.On_origin);
			CSData data = prong.Duplicate();
			CSDataHelper.Transform(data, xUp.Convert() * onXform.Convert());
			placedProngs.Append(data);
			onLineCurve.Rotate(CSBlockUtil.CSUtilMath.ToRadians(spacingAngleDeg), gem.Up.Convert(), gem.Center.Convert());
		}
	}

	private static void PlaceProngs(CSChannelGemData gemLhs, CSGemData gemRhs, IOnCurve rail, double prongDiameter, double nudgePer, double placement, bool doLhs, bool doRhs, CSData prong, OnXform xUp, ref CSDataArray placedProngs)
	{
		if (gemLhs == null || gemRhs == null || rail == null)
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
		On3dVector on3dVector = new On3dVector(gemLhs.Up.Convert()) + new On3dVector(gemRhs.Up.Convert());
		on3dVector.Unitize();
		if (prongDiameter <= num2)
		{
			On3dVector on3dVector2 = OnUtil.ON_CrossProduct(on3dVector, onLineCurve.PointAtStart() - onLineCurve.PointAtEnd());
			on3dVector2.Unitize();
			On3dPoint destination = on3dPoint4 + on3dVector * placement;
			OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(gemLhs.Center.Convert(), gemLhs.ForwardVector.Convert(), gemLhs.Up.Convert(), destination, on3dVector2, on3dVector);
			CSData data = prong.Duplicate();
			CSDataHelper.Transform(data, xUp.Convert() * xform.Convert());
			placedProngs.Append(data);
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
				OnXform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, destination2, on3dVector3, on3dVector);
				CSData data2 = prong.Duplicate();
				CSDataHelper.Transform(data2, xUp.Convert() * xform2.Convert());
				placedProngs.Append(data2);
			}
			if (doRhs)
			{
				On3dVector on3dVector4 = OnUtil.ON_CrossProduct(on3dVector, onLineCurve.PointAtEnd() - onLineCurve.PointAtStart());
				on3dVector4.Unitize();
				On3dPoint destination3 = on3dPoint4 + on3dVector4 * num6 + on3dVector * placement;
				OnXform xform3 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, destination3, on3dVector4, on3dVector);
				CSData data3 = prong.Duplicate();
				CSDataHelper.Transform(data3, xUp.Convert() * xform3.Convert());
				placedProngs.Append(data3);
			}
		}
	}
}
