using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ProngPlacerNorth_V1", "{7FFA1898-E68D-449d-B8FF-AF1CC68E8435}")]
public sealed class CSProngPlacerNorth1 : CSBlock
{
	public CSInputChannel iGem { get; }

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

	public CSOutputChannel oSurface { get; }

	public CSProngPlacerNorth1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
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
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Prong Placer:  Missing Gem\n");
		}
		double value = 0.0;
		if (!iLargeDiameter.GetDouble(ref value))
		{
			return UpdateMissing("Prong Placer:  Missing Large Diameter\n");
		}
		double value2 = 0.0;
		if (!iLargeHeight.GetDouble(ref value2))
		{
			return UpdateMissing("Prong Placer:  Missing Large Height\n");
		}
		double value3 = 0.0;
		if (!iLargeFillet.GetDouble(ref value3))
		{
			return UpdateMissing("Prong Placer:  Missing Large Fillet\n");
		}
		value3 /= 100.0;
		double value4 = 0.0;
		if (!iLargeTaper.GetDouble(ref value4))
		{
			return UpdateMissing("Prong Placer:  Missing Large Taper\n");
		}
		value4 /= 100.0;
		bool flag = false;
		double value5 = 0.0;
		if (iSmallDiameter.GetDouble(ref value5))
		{
			flag = true;
		}
		double value6 = 0.0;
		if (iSmallHeight.GetDouble(ref value6))
		{
			flag = true;
		}
		double value7 = 0.0;
		if (iSmallFillet.GetDouble(ref value7))
		{
			flag = true;
			value7 /= 100.0;
		}
		double value8 = 0.0;
		if (iSmallTaper.GetDouble(ref value8))
		{
			flag = true;
			value8 /= 100.0;
		}
		double value9 = 0.0;
		if (!iNudge.GetDouble(ref value9))
		{
			return UpdateMissing("Prong Placer:  Missing Nudge\n");
		}
		value9 = value9 / 100.0 * 2.0;
		double value10 = 0.0;
		if (!iPlacement.GetDouble(ref value10))
		{
			return UpdateMissing("Prong Placer:  Missing Placement\n");
		}
		double value11 = 0.0;
		if (!iStyle.GetDouble(ref value11))
		{
			return UpdateMissing("Prong Placer:  Missing Style\n");
		}
		CSDataArray placedProngs = new CSDataArray();
		double num = 0.0;
		double num2 = 0.0;
		if (flag)
		{
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				if (flattenArray.GetArrayItem(i) is CSGemData cSGemData)
				{
					double num3 = (cSGemData.GemXSize + cSGemData.GemYSize) / 2.0;
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
		for (int j = 0; j < flattenArray.GetArrayCount(); j++)
		{
			if (!(flattenArray.GetArrayItem(j) is CSGemData cSGemData2))
			{
				continue;
			}
			CSDataHelper.SetSettingType(cSGemData2, GemSettingType.PR);
			CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData2.Xform.Convert(), out var xUp, out var xDown);
			if (!(cSGemData2.Duplicate() is CSGemData cSGemData3))
			{
				continue;
			}
			cSGemData3.ApplyTransform(xDown.Convert());
			double num4 = value;
			double num5 = value2;
			double filletPer = value3;
			double taperPer = value4;
			if (flag)
			{
				double num6 = ((cSGemData3.GemXSize + cSGemData3.GemYSize) / 2.0 - num) / (num2 - num);
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
				if (!CSUtilGeometry.CreateProng_Old_V1((IOn3dVector)(object)cSGemData3.Up.Convert(), num4, num5, filletPer, taperPer, ref nurbsSurface))
				{
					return UpdateError("CreateProng failed");
				}
				prong = new CSOnGeometryData(nurbsSurface.Convert());
			}
			switch ((int)value11)
			{
			case 0:
				PlaceProngs(cSGemData3, 0.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 1:
				PlaceProngs(cSGemData3, 30.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 2:
				PlaceProngs(cSGemData3, 60.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 3:
				PlaceProngs(cSGemData3, 90.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 4:
				PlaceProngs(cSGemData3, 120.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 5:
				PlaceProngs(cSGemData3, 150.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 6:
				PlaceProngs(cSGemData3, 60.0, 120.0, 3, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 7:
				PlaceProngs(cSGemData3, 0.0, 120.0, 3, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 8:
				PlaceProngs(cSGemData3, 45.0, 90.0, 4, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 9:
				PlaceProngs(cSGemData3, 25.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				PlaceProngs(cSGemData3, 65.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 10:
				PlaceProngs(cSGemData3, 115.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				PlaceProngs(cSGemData3, 155.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 11:
				PlaceProngs(cSGemData3, 10.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				PlaceProngs(cSGemData3, 350.0, 180.0, 2, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 12:
				PlaceProngs(cSGemData3, 0.0, 60.0, 6, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 13:
				PlaceProngs(cSGemData3, 30.0, 60.0, 6, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			}
		}
		oSurface.SetData(placedProngs);
		return base.Update();
	}

	private bool PlaceProngs(CSGemData gem, double startingAngleDeg, double spacingAngleDeg, int prongCount, double prongDiameter, double nudgePer, double placement, CSData prong, OnXform xUp, ref CSDataArray placedProngs)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		if (gem == null)
		{
			return false;
		}
		OnNurbsCurve val = gem.GetNurbsCurve().Convert();
		On3dVector val2 = new On3dVector((IOn3dVector)(object)gem.North.Convert())
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
		return base.Update();
	}
}
