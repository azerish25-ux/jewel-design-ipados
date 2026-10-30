using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSBlocks.Shank.Helpers;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank.French;

[CSBlock("FrenchSetProduction", "{19AB7AA6-167B-4E7B-9601-DA9EF3999C02}")]
public sealed class CSFrenchSetProduction : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InBaseCurve;

	public readonly CSInputChannel InGemDataArray;

	public readonly CSInputChannel InStartProfileLocation;

	public readonly CSInputChannel InEndProfileLocation;

	public readonly CSInputChannel InShankTopProfile;

	public readonly CSInputChannel InChannelCutter;

	public readonly CSInputChannel InGraverCutter;

	public readonly CSInputChannel InTemplateGem;

	public readonly CSInputChannel InScallopCutterProfile;

	public readonly CSInputChannel InGemCutter;

	public readonly CSInputChannel InPilotHoleCutter;

	public readonly CSInputChannel InBooleanToggle;

	public readonly CSInputChannel InProductionShow;

	public readonly CSInputChannel InUnderLyingSurfacesToggle;

	public readonly CSInputChannel InGemClosedLoop;

	public readonly CSInputChannel InUseDivotCutter;

	public readonly CSInputChannel InDivotRadius;

	public readonly CSInputChannel InMetalThicknessStart;

	public readonly CSInputChannel InMetalThicknessMiddleEnabled;

	public readonly CSInputChannel InMetalThicknessMiddle;

	public readonly CSInputChannel InMetalThicknessMiddlePosition;

	public readonly CSInputChannel InMetalThicknessEndEnabled;

	public readonly CSInputChannel InMetalThicknessEnd;

	public readonly CSInputChannel InDivotCutterTranslate;

	public readonly CSOutputChannel OutChannelProfiles;

	public readonly CSOutputChannel OutShankProfiles;

	public readonly CSOutputChannel OutBaseSurface;

	public readonly CSOutputChannel OutChannelLoft;

	public readonly CSOutputChannel OutScallopCutters;

	public readonly CSOutputChannel OutGraverCutters;

	public readonly CSOutputChannel OutFrenchShank;

	public readonly CSOutputChannel OutGemCutters;

	public readonly CSOutputChannel OutPilotHoleCutters;

	public readonly CSOutputChannel OutStartProfiles;

	public readonly CSOutputChannel OutEndProfiles;

	public CSFrenchSetProduction(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDataArray = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InStartProfileLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndProfileLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShankTopProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InChannelCutter = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InScallopCutterProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGraverCutter = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InTemplateGem = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InGemCutter = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InPilotHoleCutter = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InBooleanToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InProductionShow = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InUnderLyingSurfacesToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemClosedLoop = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InUseDivotCutter = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDivotRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
		InMetalThicknessStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InMetalThicknessMiddleEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, 0);
		InMetalThicknessMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InMetalThicknessMiddlePosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InMetalThicknessEndEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, 0);
		InMetalThicknessEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InDivotCutterTranslate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutChannelProfiles = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutShankProfiles = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutBaseSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutChannelLoft = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutScallopCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutGraverCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutFrenchShank = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutGemCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutPilotHoleCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutStartProfiles = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutEndProfiles = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Unknown result type (might be due to invalid IL or missing references)
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ede: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1144: Unknown result type (might be due to invalid IL or missing references)
		//IL_1149: Unknown result type (might be due to invalid IL or missing references)
		//IL_114e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1155: Unknown result type (might be due to invalid IL or missing references)
		//IL_115a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1160: Unknown result type (might be due to invalid IL or missing references)
		//IL_1165: Unknown result type (might be due to invalid IL or missing references)
		//IL_116a: Unknown result type (might be due to invalid IL or missing references)
		//IL_116e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1001: Unknown result type (might be due to invalid IL or missing references)
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_1032: Unknown result type (might be due to invalid IL or missing references)
		//IL_106f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1074: Unknown result type (might be due to invalid IL or missing references)
		//IL_107d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1082: Unknown result type (might be due to invalid IL or missing references)
		//IL_1087: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10eb: Invalid comparison between Unknown and I4
		//IL_1185: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1103: Unknown result type (might be due to invalid IL or missing references)
		//IL_1105: Unknown result type (might be due to invalid IL or missing references)
		//IL_110b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1110: Unknown result type (might be due to invalid IL or missing references)
		//IL_1115: Unknown result type (might be due to invalid IL or missing references)
		//IL_1119: Unknown result type (might be due to invalid IL or missing references)
		//IL_1121: Unknown result type (might be due to invalid IL or missing references)
		//IL_1125: Unknown result type (might be due to invalid IL or missing references)
		//IL_112a: Unknown result type (might be due to invalid IL or missing references)
		//IL_112f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1133: Unknown result type (might be due to invalid IL or missing references)
		//IL_1793: Unknown result type (might be due to invalid IL or missing references)
		//IL_1767: Unknown result type (might be due to invalid IL or missing references)
		//IL_176d: Invalid comparison between Unknown and I4
		//IL_1778: Unknown result type (might be due to invalid IL or missing references)
		//IL_177e: Invalid comparison between Unknown and I4
		//IL_1234: Unknown result type (might be due to invalid IL or missing references)
		//IL_1239: Unknown result type (might be due to invalid IL or missing references)
		//IL_124c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1251: Unknown result type (might be due to invalid IL or missing references)
		//IL_1271: Unknown result type (might be due to invalid IL or missing references)
		//IL_1276: Unknown result type (might be due to invalid IL or missing references)
		//IL_1289: Unknown result type (might be due to invalid IL or missing references)
		//IL_128e: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_12eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_130b: Unknown result type (might be due to invalid IL or missing references)
		//IL_131c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1379: Unknown result type (might be due to invalid IL or missing references)
		//IL_1388: Unknown result type (might be due to invalid IL or missing references)
		//IL_1391: Unknown result type (might be due to invalid IL or missing references)
		//IL_1396: Unknown result type (might be due to invalid IL or missing references)
		//IL_139d: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_13be: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1402: Unknown result type (might be due to invalid IL or missing references)
		//IL_1410: Unknown result type (might be due to invalid IL or missing references)
		//IL_1415: Unknown result type (might be due to invalid IL or missing references)
		//IL_141e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1423: Unknown result type (might be due to invalid IL or missing references)
		//IL_1428: Unknown result type (might be due to invalid IL or missing references)
		//IL_142e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1433: Unknown result type (might be due to invalid IL or missing references)
		//IL_1435: Unknown result type (might be due to invalid IL or missing references)
		//IL_143a: Unknown result type (might be due to invalid IL or missing references)
		//IL_143f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1444: Unknown result type (might be due to invalid IL or missing references)
		//IL_1446: Unknown result type (might be due to invalid IL or missing references)
		//IL_1448: Unknown result type (might be due to invalid IL or missing references)
		//IL_144a: Unknown result type (might be due to invalid IL or missing references)
		//IL_144f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1453: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1512: Unknown result type (might be due to invalid IL or missing references)
		//IL_1517: Unknown result type (might be due to invalid IL or missing references)
		//IL_1537: Unknown result type (might be due to invalid IL or missing references)
		//IL_153c: Unknown result type (might be due to invalid IL or missing references)
		//IL_154f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1554: Unknown result type (might be due to invalid IL or missing references)
		//IL_1574: Unknown result type (might be due to invalid IL or missing references)
		//IL_1579: Unknown result type (might be due to invalid IL or missing references)
		//IL_158c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1591: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_163f: Unknown result type (might be due to invalid IL or missing references)
		//IL_164e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1657: Unknown result type (might be due to invalid IL or missing references)
		//IL_165c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1663: Unknown result type (might be due to invalid IL or missing references)
		//IL_166c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1671: Unknown result type (might be due to invalid IL or missing references)
		//IL_1676: Unknown result type (might be due to invalid IL or missing references)
		//IL_1684: Unknown result type (might be due to invalid IL or missing references)
		//IL_1689: Unknown result type (might be due to invalid IL or missing references)
		//IL_168f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1696: Unknown result type (might be due to invalid IL or missing references)
		//IL_169b: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16be: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_16db: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1700: Unknown result type (might be due to invalid IL or missing references)
		//IL_1705: Unknown result type (might be due to invalid IL or missing references)
		//IL_170a: Unknown result type (might be due to invalid IL or missing references)
		//IL_170c: Unknown result type (might be due to invalid IL or missing references)
		//IL_170e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1710: Unknown result type (might be due to invalid IL or missing references)
		//IL_1715: Unknown result type (might be due to invalid IL or missing references)
		//IL_1719: Unknown result type (might be due to invalid IL or missing references)
		bool flag = InBooleanToggle.Boolean;
		bool boolean = InProductionShow.Boolean;
		bool boolean2 = InGemClosedLoop.Boolean;
		if (!InUnderLyingSurfacesToggle.Boolean && !boolean)
		{
			return base.Update();
		}
		Curve curve = InCurve.Curve;
		Curve curve2 = InBaseCurve.Curve;
		List<UltimateGemData> list = new List<UltimateGemData>();
		if (!(InGemDataArray.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Ultimate GemData Array");
		}
		for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
		{
			if (!(cSDataArray.GetArrayItem(i) is CSDataArray gemInfo))
			{
				return UpdateMissing("Ultimate GemData GemArray");
			}
			UltimateGemData item = UltimateGemData.ReadFromArray(gemInfo);
			list.Add(item);
		}
		BoundingBox boundingBox = ((GeometryBase)InTemplateGem.Brep).GetBoundingBox(true);
		Curve curve3 = InChannelCutter.Curve;
		Curve curve4 = InScallopCutterProfile.Curve;
		Brep brep = InGraverCutter.Brep;
		Brep brep2 = InGemCutter.Brep;
		Brep brep3 = InPilotHoleCutter.Brep;
		bool boolean3 = InUseDivotCutter.Boolean;
		double num = InDivotRadius.Double;
		double num2 = InMetalThicknessStart.Double;
		bool boolean4 = InMetalThicknessMiddleEnabled.Boolean;
		double num3 = InMetalThicknessMiddle.Double;
		double num4 = InMetalThicknessMiddlePosition.Double;
		bool boolean5 = InMetalThicknessEndEnabled.Boolean;
		double num5 = InMetalThicknessEnd.Double;
		if (!boolean4)
		{
			num4 = 0.0;
			num3 = num2;
		}
		if (!boolean5)
		{
			num5 = num2;
		}
		double num6 = InDivotCutterTranslate.Double;
		InShankTopProfile.GetValues(out List<Curve> value);
		Curve val = null;
		Curve val2 = null;
		List<Curve> list2 = new List<Curve>();
		foreach (Curve item4 in value)
		{
			string text = ((GeometryBase)item4).GetUserString("CSName").ToLower();
			if (text == "top")
			{
				val = item4.DuplicateCurve();
			}
			if (text == "side")
			{
				list2.Add(item4.DuplicateCurve());
			}
			if (text == "bottom")
			{
				val2 = item4.DuplicateCurve();
			}
		}
		if (val == null || list2.Count != 2 || val2 == null)
		{
			return UpdateError("Error shank profile Curves");
		}
		double num7 = InStartProfileLocation.Double;
		double num8 = InEndProfileLocation.Double;
		while (num7 > 1.0 || num7 < 0.0)
		{
			if (curve.IsClosed)
			{
				if (num7 > 1.0)
				{
					num7 -= 1.0;
				}
				if (num7 < 0.0)
				{
					num7 += 1.0;
				}
			}
			else
			{
				if (num7 > 1.0)
				{
					num7 = 1.0;
				}
				if (num7 < 0.0)
				{
					num7 = 0.0;
				}
			}
		}
		while (num8 > 1.0 || num8 < 0.0)
		{
			if (curve.IsClosed)
			{
				if (num8 > 1.0)
				{
					num8 -= 1.0;
				}
				if (num8 < 0.0)
				{
					num8 += 1.0;
				}
			}
			else
			{
				if (num8 > 1.0)
				{
					num8 = 1.0;
				}
				if (num8 < 0.0)
				{
					num8 = 0.0;
				}
			}
		}
		double num9 = double.MinValue;
		foreach (UltimateGemData item5 in list)
		{
			if (item5.SizeX > num9)
			{
				num9 = item5.SizeX;
			}
		}
		List<Curve> list3 = new List<Curve>();
		List<Curve> list4 = new List<Curve>();
		List<Brep> list5 = new List<Brep>();
		List<Brep> list6 = new List<Brep>();
		List<Brep> list7 = new List<Brep>();
		List<Curve> list8 = new List<Curve>();
		List<Curve> list9 = new List<Curve>();
		List<Curve> list10 = new List<Curve>();
		List<Curve> list11 = new List<Curve>();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		if (!curve.IsClosed && curve2.IsClosed)
		{
			Plane val3 = default(Plane);
			((Plane)(ref val3))._002Ector(curve.PointAtStart, curve.TangentAtStart);
			CurveIntersections val4 = Intersection.CurvePlane(curve2, val3, 0.01);
			if (val4 == null || val4.Count < 1)
			{
				UpdateError("Failed plane and base Crv Intersection");
			}
			double num10 = double.MaxValue;
			double num11 = 0.0;
			for (int j = 0; j < val4.Count; j++)
			{
				Point3d pointA = val4[j].PointA;
				if (((Point3d)(ref pointA)).DistanceTo(curve.PointAtStart) < num10)
				{
					pointA = val4[j].PointA;
					num10 = ((Point3d)(ref pointA)).DistanceTo(curve.PointAtStart);
					num11 = val4[j].ParameterA;
				}
			}
			curve2.ChangeClosedCurveSeam(num11);
		}
		if (!curve2.IsClosed && curve.IsClosed)
		{
			double num12 = default(double);
			curve.ClosestPoint(curve2.PointAtStart, ref num12);
			curve.ChangeClosedCurveSeam(num12);
		}
		MakeShankProfile(endGem: true, num7, num2, list[0], boundingBox, curve, curve2, val, val2, list2, curve3, out var sweepParam, out var sweepParam2, out var outTopProfile, out var outBotProfile, out var outSide1Profile, out var outSide2Profile, out var outShankProfile, out var _);
		MakeEndGravers(start: true, num7, list[0], list[1], boundingBox, curve, curve2, brep, curve3, out var endGraverBrep, out var endChannelProfile2, out var channelCap);
		MakeEndGravers(start: false, num8, list[list.Count - 1], list[list.Count - 2], boundingBox, curve, curve2, brep, curve3, out var endGraverBrep2, out var endChannelProfile3, out var channelCap2);
		if (!boolean2)
		{
			list3.Add(endChannelProfile2);
			list4.Add(outShankProfile);
			list8.Add(outTopProfile);
			list9.Add(outBotProfile);
			list10.Add(outSide1Profile);
			list11.Add(outSide2Profile);
			list5.Add(endGraverBrep);
			cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)outShankProfile.DuplicateCurve()));
		}
		MakeShankProfile(endGem: true, num8, num5, list[list.Count - 1], boundingBox, curve, curve2, val, val2, list2, curve3, out var sweepParam3, out var sweepParam4, out var outTopProfile2, out var outBotProfile2, out var outSide1Profile2, out var outSide2Profile2, out var outShankProfile2, out var _);
		List<double> list12 = new List<double>();
		foreach (UltimateGemData item6 in list)
		{
			list12.Add(item6.SizeY);
		}
		bool[] array = CSFourRailSweep.FindMinMax(list12.ToArray());
		List<double> list13 = new List<double>();
		List<double> list14 = new List<double>();
		List<double> list15 = new List<double>();
		if (!boolean2)
		{
			list14.Add(sweepParam);
			list15.Add(sweepParam2);
			list13.Add(list[0].SizeY + num2 * 2.0);
		}
		double num13 = default(double);
		curve.ClosestPoint(curve.PointAtStart, ref num13);
		double num14 = curve.GetLength(new Interval(sweepParam, sweepParam3)) / curve.GetLength();
		double num15 = num4 * num14;
		Vector3d diagonal;
		double num19 = default(double);
		double num20 = default(double);
		for (int k = 0; k < list.Count; k++)
		{
			if (array[k])
			{
				Curve val5 = curve3.DuplicateCurve();
				double sizeX = list[k].SizeX;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				double num16 = sizeX / ((Vector3d)(ref diagonal)).X;
				double sizeY = list[k].SizeY;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				double num17 = sizeY / ((Vector3d)(ref diagonal)).Y;
				double sizeZ = list[k].SizeZ;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				double num18 = sizeZ / ((Vector3d)(ref diagonal)).Z;
				curve.ClosestPoint(list[k].GemPoint, ref num19);
				Vector3d destination = curve.PointAt(num19) - list[k].GemPoint;
				Transform val6 = Transform.Scale(Plane.WorldXY, num16, num17, num18);
				Transform val7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, list[k].GemPoint, destination, curve.TangentAt(num19));
				curve.NormalizedLengthParameter(num19, ref num20);
				double normParam = list[0].NormParam;
				double normParam2 = list[list.Count - 1].NormParam;
				num19 = list[k].NormParam - normParam;
				double num22;
				if (num19 < num15)
				{
					double num21 = num19 / num15;
					num22 = num2 + (num3 - num2) * num21;
				}
				else
				{
					num19 -= num15;
					double num23 = num19 / (normParam2 - num15);
					num22 = num3 + (num5 - num3) * num23;
				}
				((GeometryBase)val5).Transform(val6);
				((GeometryBase)val5).Transform(val7);
				list3.Add(val5);
				MakeShankProfile(endGem: false, 0.0, num22, list[k], boundingBox, curve, curve2, val, val2, list2, curve3, out var sweepParam5, out var sweepParam6, out var outTopProfile3, out var outBotProfile3, out var outSide1Profile3, out var outSide2Profile3, out var outShankProfile3, out var _);
				list14.Add(sweepParam5);
				list15.Add(sweepParam6);
				list4.Add(outShankProfile3);
				list13.Add(list[k].SizeY + num22 * 2.0);
				list8.Add(outTopProfile3);
				list9.Add(outBotProfile3);
				list10.Add(outSide1Profile3);
				list11.Add(outSide2Profile3);
			}
		}
		if (!boolean2)
		{
			list8.Add(outTopProfile2);
			list9.Add(outBotProfile2);
			list10.Add(outSide1Profile2);
			list11.Add(outSide2Profile2);
			list14.Add(sweepParam3);
			list15.Add(sweepParam4);
			list13.Add(list[list.Count - 1].SizeY + num5 * 2.0);
			list4.Add(outShankProfile2);
			list3.Add(endChannelProfile3);
			list5.Add(endGraverBrep2);
			cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)outShankProfile2.DuplicateCurve()));
		}
		Brep breps;
		Brep breps2;
		Brep breps3;
		Brep breps4;
		Brep breps5;
		if (boolean2)
		{
			if (!CSUtilSweepRC.Sweep2(curve, curve2, list8, list14, list15, out breps, closed: true, maintainHeight: true))
			{
				return UpdateError("Failed Sweep Top");
			}
			if (!CSUtilSweepRC.Sweep2(curve, curve2, list9, list14, list15, out breps2, closed: true, maintainHeight: true))
			{
				return UpdateError("Failed Sweep Top");
			}
			if (!CSUtilSweepRC.Sweep2(curve, curve2, list10, list14, list15, out breps3, closed: true, maintainHeight: true))
			{
				return UpdateError("Failed Sweep Top");
			}
			if (!CSUtilSweepRC.Sweep2(curve, curve2, list11, list14, list15, out breps4, closed: true, maintainHeight: true))
			{
				return UpdateError("Failed Sweep Top");
			}
			if (!CSUtilSweepRC.Sweep2(curve, curve2, list3, list14, list15, out breps5, closed: true, maintainHeight: true))
			{
				return UpdateError("Failed Sweep Channel");
			}
		}
		else
		{
			CSFourRailSweep.Create4Rails(curve, curve2, 0.0, list13, list14, list15, startShankProfileEnabled: false, startShankProfileTangent: false, startChannelProfileTangent: false, endShankProfileEnabled: false, endShankProfileTangent: false, endChannelProfileTangent: false, out var baseCrv, out var baseCrv2, out var gemCrv, out var gemCrv2, out var baseCrv0Param, out var baseCrv1Param, out var gemCrv0Param, out var gemCrv1Param);
			if (!CSUtilSweepRC.Sweep2(gemCrv2, gemCrv, list8, gemCrv1Param, gemCrv0Param, out breps, closed: false, maintainHeight: true))
			{
				return UpdateError("Failed Sweep Top");
			}
			if (!CSUtilSweepRC.Sweep2(baseCrv, baseCrv2, list9, baseCrv0Param, baseCrv1Param, out breps2, closed: false, maintainHeight: true))
			{
				return UpdateError("Failed Sweep base");
			}
			Curve curveFromBrepEdges = CSUtilBrepCrvs.GetCurveFromBrepEdges(breps, list8[0].PointAtStart, list8[list8.Count - 1].PointAtStart, list8[0].PointAtEnd, isThirdPointMid: false);
			Curve curveFromBrepEdges2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(breps2, list9[0].PointAtEnd, list9[list9.Count - 1].PointAtEnd, list9[0].PointAtStart, isThirdPointMid: false);
			Curve curveFromBrepEdges3 = CSUtilBrepCrvs.GetCurveFromBrepEdges(breps, list8[0].PointAtEnd, list8[list8.Count - 1].PointAtEnd, list8[0].PointAtStart, isThirdPointMid: false);
			Curve curveFromBrepEdges4 = CSUtilBrepCrvs.GetCurveFromBrepEdges(breps2, list9[0].PointAtStart, list9[list9.Count - 1].PointAtStart, list9[0].PointAtEnd, isThirdPointMid: false);
			List<double> list16 = new List<double>();
			List<double> list17 = new List<double>();
			List<double> list18 = new List<double>();
			List<double> list19 = new List<double>();
			double item2 = default(double);
			for (int l = 0; l < list14.Count; l++)
			{
				list11[l].Reverse();
				curveFromBrepEdges.ClosestPoint(list10[l].PointAtEnd, ref item2);
				list16.Add(item2);
				curveFromBrepEdges2.ClosestPoint(list10[l].PointAtStart, ref item2);
				list17.Add(item2);
				curveFromBrepEdges3.ClosestPoint(list11[l].PointAtEnd, ref item2);
				list18.Add(item2);
				curveFromBrepEdges4.ClosestPoint(list11[l].PointAtStart, ref item2);
				list19.Add(item2);
			}
			if (!CSUtilSweepRC.Sweep2(curveFromBrepEdges2, curveFromBrepEdges, list10, list17, list16, out breps3, closed: false, maintainHeight: true))
			{
				return UpdateError("Failed Sweep side1");
			}
			if (!CSUtilSweepRC.Sweep2(curveFromBrepEdges4, curveFromBrepEdges3, list11, list19, list18, out breps4, closed: false, maintainHeight: true))
			{
				return UpdateError("Failed Sweep side2");
			}
			Point3d val8 = default(Point3d);
			Point3d val9 = default(Point3d);
			gemCrv2.ClosestPoints(list3[0], ref val8, ref val9);
			double value2 = default(double);
			gemCrv2.ClosestPoint(val8, ref value2);
			gemCrv1Param[0] = value2;
			Point3d val10 = default(Point3d);
			Point3d val11 = default(Point3d);
			gemCrv.ClosestPoints(list3[0], ref val10, ref val11);
			double value3 = default(double);
			gemCrv.ClosestPoint(val10, ref value3);
			gemCrv0Param[0] = value3;
			gemCrv2.ClosestPoints(list3[list3.Count - 1], ref val8, ref val9);
			gemCrv2.ClosestPoint(val8, ref value2);
			gemCrv1Param[gemCrv1Param.Count - 1] = value2;
			gemCrv.ClosestPoints(list3[list3.Count - 1], ref val10, ref val11);
			gemCrv.ClosestPoint(val10, ref value3);
			gemCrv0Param[gemCrv0Param.Count - 1] = value3;
			if (!CSUtilSweepRC.Sweep2(gemCrv2, gemCrv, list3, gemCrv1Param, gemCrv0Param, out breps5, closed: false, maintainHeight: true))
			{
				return UpdateError("Failed Sweep Channel");
			}
			Brep[] array2 = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep> { channelCap, breps5, channelCap2 }, 0.01);
			if (array2.Length != 1)
			{
				return UpdateError("Channel Sweep messed up");
			}
			breps5 = array2[0].DuplicateBrep();
		}
		Brep[] array3 = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep> { breps, breps3, breps2, breps4 }, 0.01);
		if (array3.Length != 1)
		{
			return UpdateError("Failed join of sweeps");
		}
		Brep val12 = array3[0];
		double num24 = default(double);
		double num28 = default(double);
		double num34 = default(double);
		double num35 = default(double);
		double num36 = default(double);
		double num42 = default(double);
		double num43 = default(double);
		double num44 = default(double);
		for (int m = 0; m < list.Count; m++)
		{
			UltimateGemData ultimateGemData = list[m];
			curve.ClosestPoint(ultimateGemData.GemPoint, ref num24);
			Vector3d val13 = curve.PointAt(num24) - ultimateGemData.GemPoint;
			((Vector3d)(ref val13)).Unitize();
			Brep val14 = brep2.DuplicateBrep();
			Brep val15 = brep3.DuplicateBrep();
			double sizeX2 = ultimateGemData.SizeX;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double num25 = sizeX2 / ((Vector3d)(ref diagonal)).X;
			double sizeY2 = ultimateGemData.SizeY;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double num26 = sizeY2 / ((Vector3d)(ref diagonal)).Y;
			double sizeZ2 = ultimateGemData.SizeZ;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double num27 = sizeZ2 / ((Vector3d)(ref diagonal)).Z;
			Transform val16 = Transform.Scale(Plane.WorldXY, num25, num26, num27);
			((GeometryBase)val14).Transform(val16);
			((GeometryBase)val15).Transform(val16);
			if (boolean3)
			{
				Circle val17 = new Circle(Plane.WorldXY, Point3d.Origin, num);
				Curve val18 = (Curve)(object)((Circle)(ref val17)).ToNurbsCurve();
				BoundingBox boundingBox2 = ((GeometryBase)val18).GetBoundingBox(true);
				Point3d center = ((BoundingBox)(ref boundingBox2)).Center;
				Vector3d val19 = -Vector3d.ZAxis;
				((Vector3d)(ref val19)).Unitize();
				val19 *= num;
				((Point3d)(ref center)).Transform(Transform.Translation(val19));
				NurbsCurve val20 = CSUtilCurves.BlendCurve(val18.PointAtStart, center, -Vector3d.ZAxis, Vector3d.XAxis);
				val18.ClosestPoint(val18.PointAtStart, ref num28);
				Curve[] profiles = (Curve[])(object)new Curve[1] { (Curve)val20 };
				CSUtilSweepRC.Sweep1(val18, profiles, new double[1] { num28 }, out var breps6, closed: true, globalShapeBlend: false);
				Brep item3 = breps6[0];
				Point3d destination2 = curve.PointAt(num24) - val13 * ultimateGemData.SizeZ;
				Extrusion val21 = Extrusion.Create(val18, ultimateGemData.SizeZ * 2.0, false);
				item3 = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep>
				{
					((Surface)val21).ToBrep(),
					item3
				}, CSApplicationHelper.Application.Tolerance)[0];
				item3 = item3.CapPlanarHoles(0.01);
				if ((int)item3.SolidOrientation == -1)
				{
					item3.Flip();
				}
				Transform val22 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination2, val13, curve.TangentAt(num24));
				((GeometryBase)item3).Transform(val22);
				Transform val23 = Transform.Translation(val13 * num6);
				((GeometryBase)item3).Transform(val23);
				list7.Add(item3);
			}
			Transform val24 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, ultimateGemData.GemPoint, val13, curve.TangentAt(num24));
			((GeometryBase)val14).Transform(val24);
			list6.Add(val14);
			if (!boolean3)
			{
				((GeometryBase)val15).Transform(val24);
				list7.Add(val15);
			}
			if (m < list.Count - 1)
			{
				UltimateGemData ultimateGemData2 = list[m + 1];
				double num29;
				double num30;
				if (ultimateGemData.SizeX < ultimateGemData2.SizeX)
				{
					num29 = ultimateGemData.SizeX / ultimateGemData2.SizeX;
					num30 = 1.0 - num29;
				}
				else if (ultimateGemData.SizeX > ultimateGemData2.SizeX)
				{
					num30 = ultimateGemData2.SizeX / ultimateGemData.SizeX;
					num29 = 1.0 - num30;
				}
				else
				{
					num30 = 1.0;
					num29 = 1.0;
				}
				double sizeX3 = ultimateGemData.SizeX;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				double num31 = sizeX3 / ((Vector3d)(ref diagonal)).X;
				double sizeX4 = ultimateGemData2.SizeX;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				num25 = (num31 + sizeX4 / ((Vector3d)(ref diagonal)).X) / 2.0;
				double sizeY3 = ultimateGemData.SizeY;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				double num32 = sizeY3 / ((Vector3d)(ref diagonal)).Y;
				double sizeY4 = ultimateGemData2.SizeY;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				num26 = (num32 + sizeY4 / ((Vector3d)(ref diagonal)).Y) / 2.0;
				double sizeZ3 = ultimateGemData.SizeZ;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				double num33 = sizeZ3 / ((Vector3d)(ref diagonal)).Z;
				double sizeZ4 = ultimateGemData2.SizeZ;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				num27 = (num33 + sizeZ4 / ((Vector3d)(ref diagonal)).Z) / 2.0;
				Brep val25 = brep.DuplicateBrep();
				val16 = Transform.Scale(Plane.WorldXY, num25, num26, num27);
				((GeometryBase)val25).Transform(val16);
				curve.ClosestPoint(ultimateGemData.GemPoint, ref num34);
				curve.ClosestPoint(ultimateGemData2.GemPoint, ref num35);
				Curve val26 = curve.Trim(num34, num35);
				val26 = val26.Trim((CurveEnd)1, ultimateGemData.SizeX / 2.0);
				val26 = val26.Trim((CurveEnd)2, ultimateGemData2.SizeX / 2.0);
				curve.ClosestPoint(val26.PointAtNormalizedLength(0.5), ref num36);
				Vector3d val27 = (ultimateGemData.GemPoint - curve.PointAt(num34) + (ultimateGemData2.GemPoint - curve.PointAt(num35))) / 2.0;
				Vector3d val28 = (curve.PointAt(num24) - ultimateGemData.GemPoint) * num29 + (curve.PointAt(num35) - ultimateGemData2.GemPoint) * num30;
				((Vector3d)(ref val28)).Unitize();
				Point3d destination3 = curve.PointAt(num36) - val28 * ((Vector3d)(ref val27)).Length;
				Vector3d destination4 = curve.TangentAt(num36);
				val24 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination3, val28, destination4);
				((GeometryBase)val25).Transform(val24);
				list5.Add(val25);
			}
			else if (boolean2)
			{
				UltimateGemData ultimateGemData3 = list[0];
				double num37;
				double num38;
				if (ultimateGemData.SizeX < ultimateGemData3.SizeX)
				{
					num37 = ultimateGemData.SizeX / ultimateGemData3.SizeX;
					num38 = 1.0 - num37;
				}
				else if (ultimateGemData.SizeX > ultimateGemData3.SizeX)
				{
					num38 = ultimateGemData3.SizeX / ultimateGemData.SizeX;
					num37 = 1.0 - num38;
				}
				else
				{
					num38 = 1.0;
					num37 = 1.0;
				}
				double sizeX5 = ultimateGemData.SizeX;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				double num39 = sizeX5 / ((Vector3d)(ref diagonal)).X;
				double sizeX6 = ultimateGemData3.SizeX;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				num25 = (num39 + sizeX6 / ((Vector3d)(ref diagonal)).X) / 2.0;
				double sizeY5 = ultimateGemData.SizeY;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				double num40 = sizeY5 / ((Vector3d)(ref diagonal)).Y;
				double sizeY6 = ultimateGemData3.SizeY;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				num26 = (num40 + sizeY6 / ((Vector3d)(ref diagonal)).Y) / 2.0;
				double sizeZ5 = ultimateGemData.SizeZ;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				double num41 = sizeZ5 / ((Vector3d)(ref diagonal)).Z;
				double sizeZ6 = ultimateGemData3.SizeZ;
				diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
				num27 = (num41 + sizeZ6 / ((Vector3d)(ref diagonal)).Z) / 2.0;
				Brep val29 = brep.DuplicateBrep();
				val16 = Transform.Scale(Plane.WorldXY, num25, num26, num27);
				((GeometryBase)val29).Transform(val16);
				curve.ClosestPoint(ultimateGemData.GemPoint, ref num42);
				curve.ClosestPoint(ultimateGemData3.GemPoint, ref num43);
				Curve val30 = curve.Trim(num42, num43);
				val30 = val30.Trim((CurveEnd)1, ultimateGemData.SizeX / 2.0);
				val30 = val30.Trim((CurveEnd)2, ultimateGemData3.SizeX / 2.0);
				curve.ClosestPoint(val30.PointAtNormalizedLength(0.5), ref num44);
				Vector3d val31 = (ultimateGemData.GemPoint - curve.PointAt(num42) + (ultimateGemData3.GemPoint - curve.PointAt(num43))) / 2.0;
				Vector3d val32 = (curve.PointAt(num24) - ultimateGemData.GemPoint) * num37 + (curve.PointAt(num43) - ultimateGemData3.GemPoint) * num38;
				((Vector3d)(ref val32)).Unitize();
				Point3d destination5 = curve.PointAt(num44) - val32 * ((Vector3d)(ref val31)).Length;
				Vector3d destination6 = curve.TangentAt(num44);
				val24 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination5, val32, destination6);
				((GeometryBase)val29).Transform(val24);
				list5.Add(val29);
			}
		}
		list3.Add(endChannelProfile3);
		Brep val33;
		if (val12 != null)
		{
			val33 = val12.CapPlanarHoles(0.01);
			if (val33 == null)
			{
				val33 = val12;
			}
			if ((int)val33.SolidOrientation == -1)
			{
				val33.Flip();
			}
			if ((int)breps5.SolidOrientation == -1)
			{
				breps5.Flip();
			}
		}
		else
		{
			flag = false;
			val33 = null;
		}
		MakeScallopCutters(curve, list, boundingBox, curve4, num9 + num2, out var scallopCutters);
		CSDataArray outChannelProfiles = new CSDataArray();
		CSDataArray outTopProfile4 = new CSDataArray();
		CSDataArray outScallopCutters = new CSDataArray();
		CSDataArray outGraverCutters = new CSDataArray();
		CSDataArray outGemCutters = new CSDataArray();
		CSDataArray outPilotHoleCutters = new CSDataArray();
		list3.ForEach(delegate(Curve _)
		{
			outChannelProfiles.Append(new CSOnGeometryData((GeometryBase)(object)_));
		});
		list4.ForEach(delegate(Curve _)
		{
			outTopProfile4.Append(new CSOnGeometryData((GeometryBase)(object)_));
		});
		list5.ForEach(delegate(Brep _)
		{
			outGraverCutters.Append(new CSOnGeometryData((GeometryBase)(object)_));
		});
		scallopCutters.ForEach(delegate(Brep _)
		{
			outScallopCutters.Append(new CSOnGeometryData((GeometryBase)(object)_));
		});
		list6.ForEach(delegate(Brep _)
		{
			outGemCutters.Append(new CSOnGeometryData((GeometryBase)(object)_));
		});
		list7.ForEach(delegate(Brep _)
		{
			outPilotHoleCutters.Append(new CSOnGeometryData((GeometryBase)(object)_));
		});
		if (boolean)
		{
			if (flag)
			{
				Brep[] array4 = Brep.CreateBooleanDifference(val33, breps5, 0.01);
				if (array4 == null || array4.Length != 1)
				{
					return UpdateError("Failed Channel Intersection");
				}
				Brep[] array5 = Brep.CreateBooleanDifference((IEnumerable<Brep>)array4, (IEnumerable<Brep>)scallopCutters, 0.01);
				if (array5 == null || array5.Length != 1)
				{
					return UpdateError("Failed Intersection Scallops");
				}
				Brep[] array6 = Brep.CreateBooleanDifference(array5[0], list5[0], 0.01);
				if (array6 == null || array6.Length != 1)
				{
					return UpdateError("Failed Boolean First Graver ");
				}
				for (int num45 = 1; num45 < list5.Count; num45++)
				{
					array6 = Brep.CreateBooleanDifference(array6[0], list5[num45], 0.01);
					if (array6 == null || array6.Length == 0)
					{
						return UpdateError("Failed Boolean Gravers ");
					}
				}
				array6 = Brep.CreateBooleanDifference((IEnumerable<Brep>)array6, (IEnumerable<Brep>)list6, 0.01);
				if (array6 == null || array6.Length != 1)
				{
					return UpdateError("Failed Boolean GemCutters ");
				}
				array6 = Brep.CreateBooleanDifference((IEnumerable<Brep>)array6, (IEnumerable<Brep>)list7, 0.01);
				if (array6 == null || array6.Length != 1)
				{
					return UpdateError("Failed Boolean PilotHoles ");
				}
				OutFrenchShank.SetData(new CSOnGeometryData((GeometryBase)(object)array6[0]));
			}
			else
			{
				CSDataArray cSDataArray4 = new CSDataArray();
				cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)val33));
				cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)breps5));
				cSDataArray4.Append(outScallopCutters);
				cSDataArray4.Append(outGraverCutters);
				cSDataArray4.Append(outGemCutters);
				cSDataArray4.Append(outPilotHoleCutters);
				OutFrenchShank.SetData(cSDataArray4);
			}
			OutStartProfiles.SetData(cSDataArray2);
			OutEndProfiles.SetData(cSDataArray3);
		}
		OutBaseSurface.SetData(new CSOnGeometryData((GeometryBase)(object)val33));
		OutChannelLoft.SetData(new CSOnGeometryData((GeometryBase)(object)breps5));
		OutScallopCutters.SetData(outScallopCutters);
		OutGraverCutters.SetData(outGraverCutters);
		OutGemCutters.SetData(outGemCutters);
		OutChannelProfiles.SetData(outChannelProfiles);
		OutShankProfiles.SetData(outTopProfile4);
		return base.Update();
	}

	private void MakeEndGravers(bool start, double location, UltimateGemData endGem, UltimateGemData nextGem, BoundingBox templateGemBB, Curve crv, Curve baseCrv, Brep graverBrep, Curve channelProfile, out Brep endGraverBrep, out Curve endChannelProfile, out Brep channelCap)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		Brep val = graverBrep.DuplicateBrep();
		Curve val2 = channelProfile.DuplicateCurve();
		double sizeX = endGem.SizeX;
		Vector3d diagonal = ((BoundingBox)(ref templateGemBB)).Diagonal;
		double num = sizeX / ((Vector3d)(ref diagonal)).X;
		double sizeY = endGem.SizeY;
		diagonal = ((BoundingBox)(ref templateGemBB)).Diagonal;
		double num2 = sizeY / ((Vector3d)(ref diagonal)).Y;
		double sizeZ = endGem.SizeZ;
		diagonal = ((BoundingBox)(ref templateGemBB)).Diagonal;
		double num3 = sizeZ / ((Vector3d)(ref diagonal)).Z;
		double num4 = default(double);
		crv.ClosestPoint(crv.PointAtNormalizedLength(location), ref num4);
		double parameterA = default(double);
		baseCrv.ClosestPoint(crv.PointAtNormalizedLength(location), ref parameterA);
		double num5 = default(double);
		crv.ClosestPoint(endGem.GemPoint, ref num5);
		double num6 = default(double);
		baseCrv.ClosestPoint(crv.PointAt(num5), ref num6);
		double num7 = default(double);
		crv.ClosestPoint(nextGem.GemPoint, ref num7);
		Curve val3;
		if (start)
		{
			val3 = crv.Trim(num5, num7);
			val3 = val3.Trim((CurveEnd)1, endGem.SizeX / 2.0);
			val3 = val3.Trim((CurveEnd)2, nextGem.SizeX / 2.0);
		}
		else
		{
			val3 = crv.Trim(num7, num5);
			val3 = val3.Trim((CurveEnd)1, nextGem.SizeX / 2.0);
			val3 = val3.Trim((CurveEnd)2, endGem.SizeX / 2.0);
		}
		Point3d val4 = val3.PointAtNormalizedLength(0.5);
		double num8 = default(double);
		crv.ClosestPoint(val4, ref num8);
		Interval domain;
		Point3d val5;
		if (start)
		{
			double length = crv.GetLength(new Interval(num5, num8));
			domain = crv.Domain;
			val3 = crv.Trim(((Interval)(ref domain)).Min, num5);
			val3 = val3.Trim((CurveEnd)2, length);
			val5 = val3.PointAtEnd;
		}
		else
		{
			double length2 = crv.GetLength(new Interval(num8, num5));
			double num9 = num5;
			domain = crv.Domain;
			val3 = crv.Trim(num9, ((Interval)(ref domain)).Max);
			val3 = val3.Trim((CurveEnd)1, length2);
			val5 = val3.PointAtStart;
		}
		double num10 = default(double);
		crv.ClosestPoint(val5, ref num10);
		Plane val6 = default(Plane);
		((Plane)(ref val6))._002Ector(crv.PointAt(num10), crv.TangentAt(num10));
		CurveIntersections val7 = Intersection.CurvePlane(baseCrv, val6, 0.01);
		if (val7 == null || val7.Count < 1)
		{
			UpdateError("Failed plane and base Crv Intersection");
		}
		double num11 = double.MaxValue;
		Point3d val8;
		for (int i = 0; i < val7.Count; i++)
		{
			val8 = val7[i].PointA;
			if (((Point3d)(ref val8)).DistanceTo(crv.PointAt(num10)) < num11)
			{
				val8 = val7[i].PointA;
				num11 = ((Point3d)(ref val8)).DistanceTo(crv.PointAt(num10));
				parameterA = val7[i].ParameterA;
			}
		}
		Vector3d val9 = crv.PointAt(num10) - baseCrv.PointAt(parameterA);
		((Vector3d)(ref val9)).Unitize();
		Vector3d val10 = endGem.GemPoint - crv.PointAt(num5);
		Point3d destination = crv.PointAt(num10) - val9 * ((Vector3d)(ref val10)).Length;
		Vector3d destination2 = val9;
		Vector3d destination3 = crv.TangentAt(num10);
		Transform val11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination, destination2, destination3);
		Transform val12 = Transform.Scale(Plane.WorldXY, num, num2, num3);
		((GeometryBase)val).Transform(val12);
		((GeometryBase)val).Transform(val11);
		((GeometryBase)val2).Transform(val12);
		endChannelProfile = val2.DuplicateCurve();
		((GeometryBase)endChannelProfile).Transform(val11);
		BoundingBox boundingBox = ((GeometryBase)val2).GetBoundingBox(true);
		Point3d val13 = new Point3d(-1.0, 0.0, 5.0);
		Point3d val14 = default(Point3d);
		((Point3d)(ref val14))._002Ector(1.0, 0.0, 5.0);
		Point3d val15 = default(Point3d);
		((Point3d)(ref val15))._002Ector(1.0, 0.0, -5.0);
		Point3d val16 = default(Point3d);
		((Point3d)(ref val16))._002Ector(-1.0, 0.0, -5.0);
		NurbsSurface val17 = NurbsSurface.CreateFromCorners(val13, val14, val15, val16);
		val8 = ((BoundingBox)(ref boundingBox)).Min;
		double num12 = 0.0 - ((Point3d)(ref val8)).Z;
		Transform val18 = Transform.Translation(0.0, 0.0, num12);
		((GeometryBase)val2).Transform(val18);
		NurbsCurve profile = val2.Split((Surface)(object)val17, 0.01)[0].ToNurbsCurve();
		Point3d val19 = default(Point3d);
		((Point3d)(ref val19))._002Ector(0.0, 1.0, 0.0);
		Point3d val20 = default(Point3d);
		((Point3d)(ref val20))._002Ector(-1.5, 0.0, 0.0);
		Point3d val21 = default(Point3d);
		((Point3d)(ref val21))._002Ector(0.0, -1.0, 0.0);
		Curve val22 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)(object)new Point3d[3] { val19, val20, val21 }, 3);
		NurbsSurface val23 = CSUtilRailRevolve.RailRevolveRC(profile, val22.ToNurbsCurve(), 1.0, 0);
		val18 = Transform.Translation(0.0, 0.0, 0.0 - num12);
		((GeometryBase)val23).Transform(val18);
		if (!start)
		{
			((GeometryBase)val23).Transform(Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(180.0), Point3d.Origin));
		}
		((GeometryBase)val23).Transform(val11);
		channelCap = ((Surface)val23).ToBrep().DuplicateBrep();
		endGraverBrep = val.DuplicateBrep();
	}

	private void MakeScallopCutters(Curve crv, List<UltimateGemData> gemList, BoundingBox templateGemBB, Curve scallopProfile, double scallopLength, out List<Brep> scallopCutters)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Invalid comparison between Unknown and I4
		scallopCutters = new List<Brep>();
		double num = default(double);
		for (int i = 0; i < gemList.Count; i++)
		{
			UltimateGemData ultimateGemData = gemList[i];
			Point3d gemPoint = ultimateGemData.GemPoint;
			crv.ClosestPoint(gemPoint, ref num);
			double sizeX = ultimateGemData.SizeX;
			Vector3d diagonal = ((BoundingBox)(ref templateGemBB)).Diagonal;
			double num2 = sizeX / ((Vector3d)(ref diagonal)).X;
			double sizeY = ultimateGemData.SizeY;
			diagonal = ((BoundingBox)(ref templateGemBB)).Diagonal;
			double num3 = sizeY / ((Vector3d)(ref diagonal)).Y;
			double sizeZ = ultimateGemData.SizeZ;
			diagonal = ((BoundingBox)(ref templateGemBB)).Diagonal;
			double num4 = sizeZ / ((Vector3d)(ref diagonal)).Z;
			Transform val = Transform.Scale(Plane.WorldXY, num2, num3, num4);
			Curve obj = scallopProfile.DuplicateCurve();
			((GeometryBase)obj).Transform(val);
			Vector3d val2;
			Vector3d val3;
			if (i > 0 && i < gemList.Count - 1)
			{
				Point3d gemPoint2 = gemList[i - 1].GemPoint;
				Point3d gemPoint3 = gemList[i + 1].GemPoint;
				val2 = gemPoint2 - gemPoint;
				val3 = gemPoint3 - gemPoint;
			}
			else if (i == 0)
			{
				val3 = gemList[i + 1].GemPoint - gemPoint;
				val2 = -val3;
			}
			else
			{
				val2 = gemList[i - 1].GemPoint - gemPoint;
				val3 = -val2;
			}
			Vector3d val4 = crv.PointAt(num) - ultimateGemData.GemPoint;
			((Vector3d)(ref val4)).Unitize();
			Transform val5 = CSBlockUtil.CSUtilMath.Orient3Point(gemPoint, val4, val2, Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis);
			((Vector3d)(ref val2)).Transform(val5);
			((Vector3d)(ref val3)).Transform(val5);
			((Vector3d)(ref val2)).Z = 0.0;
			((Vector3d)(ref val3)).Z = 0.0;
			double num5 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(val2, val3);
			num5 = Math.PI - num5;
			BoundingBox boundingBox = ((GeometryBase)obj).GetBoundingBox(true);
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double x = ((Vector3d)(ref diagonal)).X;
			double num6 = x - Math.Tan(num5 / 2.0) * scallopLength;
			double num7 = x + Math.Tan(num5 / 2.0) * scallopLength;
			Curve val6 = obj.DuplicateCurve();
			Curve val7 = obj.DuplicateCurve();
			((GeometryBase)val6).Transform(Transform.Scale(Plane.WorldXY, num6 / x, 1.0, 1.0));
			((GeometryBase)val7).Transform(Transform.Scale(Plane.WorldXY, num7 / x, 1.0, 1.0));
			Transform val8 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, gemPoint, val4, crv.TangentAt(num));
			((GeometryBase)val6).Transform(val8);
			((GeometryBase)val7).Transform(val8);
			Vector3d val9 = Vector3d.CrossProduct(crv.TangentAt(num), val4);
			if (((Vector3d)(ref val3)).Y > 0.0)
			{
				((GeometryBase)val6).Transform(Transform.Translation(val9 * scallopLength));
				((GeometryBase)val7).Transform(Transform.Translation(val9 * (0.0 - scallopLength)));
			}
			else
			{
				((GeometryBase)val6).Transform(Transform.Translation(val9 * (0.0 - scallopLength)));
				((GeometryBase)val7).Transform(Transform.Translation(val9 * scallopLength));
			}
			Brep val10 = CSUtilSweepRC.Loft(new List<Curve> { val6, val7 }, (LoftType)0, doCapped: true);
			if (val10 == null)
			{
				UpdateError("Scallop Loft Error");
			}
			if ((int)val10.SolidOrientation == -1)
			{
				val10.Flip();
			}
			scallopCutters.Add(val10);
		}
	}

	private void MakeShankProfile(bool endGem, double location, double shankWidthAdd, UltimateGemData gem, BoundingBox templateGemBB, Curve crv, Curve baseCrv, Curve shankTopProfile, Curve shankBotProfile, List<Curve> shankSideProfiles, Curve channelProfile, out double sweepParam1, out double sweepParam2, out Curve outTopProfile, out Curve outBotProfile, out Curve outSide1Profile, out Curve outSide2Profile, out Curve outShankProfile, out Curve endChannelProfile)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		Curve val = shankTopProfile.DuplicateCurve();
		Curve val2 = shankBotProfile.DuplicateCurve();
		Point3d pointAtStart = val.PointAtStart;
		Point3d val3 = shankSideProfiles[0].PointAtEnd;
		double num = ((Point3d)(ref val3)).DistanceTo(pointAtStart);
		val3 = shankSideProfiles[1].PointAtEnd;
		Curve val4;
		Curve val5;
		if (num < ((Point3d)(ref val3)).DistanceTo(pointAtStart))
		{
			val4 = shankSideProfiles[0].DuplicateCurve();
			val5 = shankSideProfiles[1].DuplicateCurve();
		}
		else
		{
			val4 = shankSideProfiles[1].DuplicateCurve();
			val5 = shankSideProfiles[0].DuplicateCurve();
		}
		Curve val6 = channelProfile.DuplicateCurve();
		BoundingBox boundingBox = ((GeometryBase)shankTopProfile).GetBoundingBox(true);
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double y = ((Vector3d)(ref diagonal)).Y;
		diagonal = ((BoundingBox)(ref templateGemBB)).Diagonal;
		double num2 = y - ((Vector3d)(ref diagonal)).Y;
		double sizeX = gem.SizeX;
		diagonal = ((BoundingBox)(ref templateGemBB)).Diagonal;
		double num3 = sizeX / ((Vector3d)(ref diagonal)).X;
		double num4 = gem.SizeY + shankWidthAdd * 2.0;
		diagonal = ((BoundingBox)(ref templateGemBB)).Diagonal;
		double num5 = num4 / (((Vector3d)(ref diagonal)).Y + num2);
		double sizeZ = gem.SizeZ;
		diagonal = ((BoundingBox)(ref templateGemBB)).Diagonal;
		double num6 = sizeZ / ((Vector3d)(ref diagonal)).Z;
		Transform val7 = Transform.Scale(Plane.WorldXY, num3, num5, num6);
		((GeometryBase)val6).Transform(val7);
		((GeometryBase)val).Transform(val7);
		((GeometryBase)val2).Transform(val7);
		((GeometryBase)val4).Transform(val7);
		((GeometryBase)val5).Transform(val7);
		double num7 = default(double);
		crv.ClosestPoint(gem.GemPoint, ref num7);
		double num8 = default(double);
		baseCrv.ClosestPoint(gem.GemPoint, ref num8);
		Point3d val8 = gem.GemPoint;
		Plane val9 = default(Plane);
		((Plane)(ref val9))._002Ector(val8, crv.TangentAt(num7));
		CurveIntersections val10 = Intersection.CurvePlane(baseCrv, val9, 0.01);
		if (val10 == null || val10.Count < 1)
		{
			UpdateError("Failed plane and base Crv Intersection");
		}
		double num9 = double.MaxValue;
		for (int i = 0; i < val10.Count; i++)
		{
			val3 = val10[i].PointA;
			if (((Point3d)(ref val3)).DistanceTo(val8) < num9)
			{
				val3 = val10[i].PointA;
				num9 = ((Point3d)(ref val3)).DistanceTo(val8);
				num8 = val10[i].ParameterA;
			}
		}
		Vector3d val11 = crv.PointAt(num7) - gem.GemPoint;
		((Vector3d)(ref val11)).Unitize();
		Vector3d inputVector = crv.PointAt(num7) - gem.GemPoint;
		((Vector3d)(ref inputVector)).Unitize();
		if (CSBlockUtil.CSUtilMath.AngleBetweenVectors(val11, inputVector) > Math.PI / 2.0)
		{
			((Vector3d)(ref val11)).Reverse();
		}
		if (endGem)
		{
			crv.ClosestPoint(crv.PointAtNormalizedLength(location), ref num7);
			((Plane)(ref val9))._002Ector(crv.PointAt(num7), crv.TangentAt(num7));
			val10 = Intersection.CurvePlane(baseCrv, val9, 0.01);
			if (val10 == null || val10.Count < 1)
			{
				UpdateError("Failed plane and base Crv Intersection");
			}
			num9 = double.MaxValue;
			for (int j = 0; j < val10.Count; j++)
			{
				val3 = val10[j].PointA;
				if (((Point3d)(ref val3)).DistanceTo(val8) < num9)
				{
					val3 = val10[j].PointA;
					num9 = ((Point3d)(ref val3)).DistanceTo(val8);
					num8 = val10[j].ParameterA;
				}
			}
			if (baseCrv.IsClosed && !crv.IsClosed)
			{
				double num10 = num8;
				Interval domain = baseCrv.Domain;
				if (Math.Abs(num10 - ((Interval)(ref domain)).Max) < 0.001)
				{
					domain = baseCrv.Domain;
					num8 = ((Interval)(ref domain)).Min;
				}
			}
			val11 = crv.PointAt(num7) - baseCrv.PointAt(num8);
			((Vector3d)(ref val11)).Unitize();
			if (CSBlockUtil.CSUtilMath.AngleBetweenVectors(val11, inputVector) > Math.PI / 2.0)
			{
				((Vector3d)(ref val11)).Reverse();
			}
			double num11 = default(double);
			crv.ClosestPoint(gem.GemPoint, ref num11);
			Vector3d val12 = crv.PointAt(num11) - gem.GemPoint;
			val8 = crv.PointAtNormalizedLength(location) - val11 * ((Vector3d)(ref val12)).Length;
		}
		boundingBox = ((GeometryBase)val).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double z = ((Vector3d)(ref diagonal)).Z;
		boundingBox = ((GeometryBase)val2).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num12 = z + ((Vector3d)(ref diagonal)).Z;
		BoundingBox boundingBox2 = ((GeometryBase)val2).GetBoundingBox(true);
		val3 = baseCrv.PointAt(num8);
		double num13 = ((Point3d)(ref val3)).DistanceTo(crv.PointAt(num7));
		if (num12 > num13)
		{
			Transform val13 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, num13 * 0.75 / num12);
			((GeometryBase)val).Transform(val13);
			((GeometryBase)val2).Transform(val13);
			boundingBox2 = ((GeometryBase)val2).GetBoundingBox(true);
		}
		Transform val14 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, val8, val11, crv.TangentAt(num7));
		((GeometryBase)val6).Transform(val14);
		Curve val15 = val.DuplicateCurve();
		((GeometryBase)val15).Transform(val14);
		Curve val16 = val2.DuplicateCurve();
		val3 = ((BoundingBox)(ref boundingBox2)).Min;
		((GeometryBase)val16).Transform(Transform.Translation(0.0, 0.0, 0.0 - ((Point3d)(ref val3)).Z));
		Point3d destination = crv.PointAt(num7) - val11 * num13;
		Transform val17 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination, val11, crv.TangentAt(num7));
		((GeometryBase)val16).Transform(val17);
		val3 = val16.PointAtStart;
		double num14 = ((Point3d)(ref val3)).DistanceTo(val15.PointAtEnd);
		Curve val18 = val4.DuplicateCurve();
		BoundingBox boundingBox3 = ((GeometryBase)val18).GetBoundingBox(true);
		Plane worldXY = Plane.WorldXY;
		diagonal = ((BoundingBox)(ref boundingBox3)).Diagonal;
		((GeometryBase)val18).Transform(Transform.Scale(worldXY, 1.0, 1.0, num14 / ((Vector3d)(ref diagonal)).Z));
		Transform val19 = CSBlockUtil.CSUtilMath.Orient3Point(val18.PointAtEnd, Vector3d.ZAxis, Vector3d.XAxis, val15.PointAtStart, val11, crv.TangentAt(num7));
		((GeometryBase)val18).Transform(val19);
		Curve val20 = val5.DuplicateCurve();
		BoundingBox boundingBox4 = ((GeometryBase)val20).GetBoundingBox(true);
		Plane worldXY2 = Plane.WorldXY;
		diagonal = ((BoundingBox)(ref boundingBox4)).Diagonal;
		((GeometryBase)val20).Transform(Transform.Scale(worldXY2, 1.0, 1.0, num14 / ((Vector3d)(ref diagonal)).Z));
		Transform val21 = CSBlockUtil.CSUtilMath.Orient3Point(val20.PointAtStart, Vector3d.ZAxis, Vector3d.XAxis, val15.PointAtEnd, val11, crv.TangentAt(num7));
		((GeometryBase)val20).Transform(val21);
		outTopProfile = val15;
		outBotProfile = val16;
		outSide1Profile = val18;
		outSide2Profile = val20;
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { val15, val18, val16, val20 }, out Curve mergedCrv, 0.01);
		endChannelProfile = val6.DuplicateCurve();
		if (mergedCrv == null)
		{
			UpdateError("Shank Profile Failed To Merge");
		}
		double num15 = default(double);
		mergedCrv.ClosestPoint(baseCrv.PointAt(num8), ref num15);
		mergedCrv.ChangeClosedCurveSeam(num15);
		outShankProfile = mergedCrv;
		sweepParam1 = num7;
		sweepParam2 = num8;
	}
}
