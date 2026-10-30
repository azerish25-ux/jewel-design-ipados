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
[CSBlock("ProngPlacerNorth_V2", "{178909AD-AB38-41ea-8D17-C37F43548819}")]
public sealed class CSProngPlacerNorth2 : CSBlock
{
	public CSInputChannel InGem { get; }

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

	public CSInputChannel InProfile { get; }

	public CSInputChannel InMeshedProngs { get; }

	public CSOutputChannel OutSurface { get; }

	public CSProngPlacerNorth2(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
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
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InMeshedProngs = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		CSDataArray flattenArray = InGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		OnNurbsCurve val = InProfile.GetNurbsCurve();
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
		bool value12 = false;
		InMeshedProngs.GetBoolean(ref value12);
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
				if (value12)
				{
					if (!CSUtilGeometry.CreateMeshProng((IOnNurbsCurve)(object)val, num4, num5, filletPer, taperPer, out var meshSurface))
					{
						return UpdateError("CreateMeshProng failed");
					}
					prong = new CSOnGeometryData(meshSurface.Convert());
				}
				else
				{
					if (!CSUtilGeometry.CreateProng((IOnNurbsCurve)(object)val, num4, num5, filletPer, taperPer, out var nurbsSurface))
					{
						return UpdateError("CreateProng failed");
					}
					prong = new CSOnGeometryData(nurbsSurface.Convert());
				}
			}
			switch (value11)
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
			case 14:
				PlaceProngs(cSGemData3, 0.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			case 15:
				PlaceProngs(cSGemData3, 180.0, 0.0, 1, num4, value9, value10, prong, xUp, ref placedProngs);
				break;
			}
		}
		OutSurface.SetData(placedProngs);
		return base.Update();
	}

	private static void PlaceProngs(CSGemData gem, double startingAngleDeg, double spacingAngleDeg, int prongCount, double prongDiameter, double nudgePer, double placement, CSData prong, OnXform xUp, ref CSDataArray placedProngs)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		if (gem == null)
		{
			return;
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
			OnXform val10 = new OnXform();
			val10.Rotation(num2, OnUtil.On_zaxis, OnUtil.On_origin);
			CSData data = prong.Duplicate();
			CSDataHelper.Transform(data, xUp.Convert() * val9.Convert() * val10.Convert());
			placedProngs.Append(data);
			((OnGeometry)val3).Rotate(CSBlockUtil.CSUtilMath.ToRadians(spacingAngleDeg), (IOn3dVector)(object)gem.Up.Convert(), (IOn3dPoint)(object)gem.Center.Convert());
		}
	}
}
