using System;
using System.Collections.Concurrent;
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
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ProngPlacerGC_V10", "{97E822EE-4F22-4782-924C-117531D3A967}")]
public sealed class CSProngPlacerGC10 : CSBlock
{
	public readonly CSInputChannel InStartBlendBot;

	public readonly CSInputChannel InMiddleBlendBot;

	public readonly CSInputChannel InEndBlendBot;

	public readonly CSInputChannel InStartBlendEdge;

	public readonly CSInputChannel InMiddleBlendEdge;

	public readonly CSInputChannel InEndBlendEdge;

	public readonly CSInputChannel InStartBlendTop;

	public readonly CSInputChannel InMiddleBlendTop;

	public readonly CSInputChannel InEndBlendTop;

	public readonly CSInputChannel InStartBottomArcPercentage;

	public readonly CSInputChannel InMiddleBottomArcPercentage;

	public readonly CSInputChannel InEndBottomArcPercentage;

	public readonly CSInputChannel InCornerNudgeEdge;

	public readonly CSInputChannel InCornerNudgeGem;

	public readonly CSInputChannel InCornerProngAngle;

	public readonly CSInputChannel InCornerProngSize;

	public readonly CSInputChannel InCornerProngSizeOffset;

	public readonly CSInputChannel InDoMeshedProngs;

	public readonly CSInputChannel InEnableEndProngEndControls;

	public readonly CSInputChannel InEnableEndProngRotationMirroring;

	public readonly CSInputChannel InStartNudgeEdge;

	public readonly CSInputChannel InEndNudgeEdge;

	public readonly CSInputChannel InStartNudgeGem;

	public readonly CSInputChannel InEndNudgeGem;

	public readonly CSInputChannel InEndProngStyle;

	public readonly CSInputChannel InStartFillet;

	public readonly CSInputChannel InMiddleFillet;

	public readonly CSInputChannel InEndFillet;

	public readonly CSInputChannel InStartHeight;

	public readonly CSInputChannel InMiddleHeight;

	public readonly CSInputChannel InEndHeight;

	public readonly CSInputChannel InMaxGem;

	public readonly CSInputChannel InNudgeEdge;

	public readonly CSInputChannel InNudgeGem;

	public readonly CSInputChannel InPercentToggle;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InStartProngPercentage;

	public readonly CSInputChannel InMiddleProngPercentage;

	public readonly CSInputChannel InEndProngPercentage;

	public readonly CSInputChannel InProngSizeControl;

	public readonly CSInputChannel InRotationEnd;

	public readonly CSInputChannel InRotationStart;

	public readonly CSInputChannel InSettingStyle;

	public readonly CSInputChannel InSizeMax;

	public readonly CSInputChannel InSizeMin;

	public readonly CSInputChannel InSizePercentageEnd;

	public readonly CSInputChannel InSizePercentageStart;

	public readonly CSInputChannel InSortedGems;

	public readonly CSInputChannel InSpreadEnd;

	public readonly CSInputChannel InSpreadStart;

	public readonly CSInputChannel InStartTaper;

	public readonly CSInputChannel InMiddleTaper;

	public readonly CSInputChannel InEndTaper;

	public readonly CSInputChannel InStartVerticalLocation;

	public readonly CSInputChannel InMiddleVerticalLocation;

	public readonly CSInputChannel InEndVerticalLocation;

	public readonly CSInputChannel InCamModeOverride;

	public readonly CSOutputChannel OutProfiles;

	public readonly CSOutputChannel OutProngs;

	private readonly CSProngCache _prongCache = new CSProngCache();

	private ValueListItem _prongBase = ValueListItem.FollowCulet;

	public CSProngPlacerGC10(string identifier)
		: base(identifier)
	{
		InSortedGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InSizeMin = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSizeMax = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMiddleHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMiddleFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMiddleTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InNudgeEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InNudgeGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartVerticalLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMiddleVerticalLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndVerticalLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndProngStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InEnableEndProngRotationMirroring = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnableEndProngEndControls = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InSizePercentageStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRotationStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpreadStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSizePercentageEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRotationEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpreadEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InDoMeshedProngs = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InSettingStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InMaxGem = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual, ValueListItem.FollowCulet);
		InPercentToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InStartProngPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMiddleProngPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndProngPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InStartBlendTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.2);
		InMiddleBlendTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.2);
		InEndBlendTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.2);
		InStartBlendBot = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.2);
		InMiddleBlendBot = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.2);
		InEndBlendBot = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.2);
		InStartBlendEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMiddleBlendEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndBlendEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InStartBottomArcPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InMiddleBottomArcPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InEndBottomArcPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InProngSizeControl = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 156);
		InCornerProngAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerProngSize = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 159);
		InCornerProngSizeOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndNudgeEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InStartNudgeEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InStartNudgeGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndNudgeGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerNudgeEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerNudgeGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCamModeOverride = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProfiles = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Expected O, but got Unknown
		//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1962: Unknown result type (might be due to invalid IL or missing references)
		//IL_1967: Unknown result type (might be due to invalid IL or missing references)
		//IL_1972: Unknown result type (might be due to invalid IL or missing references)
		//IL_1939: Unknown result type (might be due to invalid IL or missing references)
		//IL_193e: Unknown result type (might be due to invalid IL or missing references)
		//IL_194c: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0add: Expected O, but got Unknown
		//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aeb: Expected O, but got Unknown
		//IL_0b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		//IL_2502: Unknown result type (might be due to invalid IL or missing references)
		//IL_2509: Unknown result type (might be due to invalid IL or missing references)
		//IL_250e: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Expected O, but got Unknown
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b62: Expected O, but got Unknown
		//IL_0b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_116b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1172: Expected O, but got Unknown
		//IL_118b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1192: Expected O, but got Unknown
		//IL_11a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11aa: Expected O, but got Unknown
		//IL_11c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ca: Expected O, but got Unknown
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f96: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1219: Unknown result type (might be due to invalid IL or missing references)
		//IL_1220: Expected O, but got Unknown
		//IL_1202: Unknown result type (might be due to invalid IL or missing references)
		//IL_1209: Expected O, but got Unknown
		//IL_0d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0daf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2703: Unknown result type (might be due to invalid IL or missing references)
		//IL_270d: Expected O, but got Unknown
		//IL_270d: Expected O, but got Unknown
		//IL_1fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2004: Unknown result type (might be due to invalid IL or missing references)
		//IL_2006: Unknown result type (might be due to invalid IL or missing references)
		//IL_2008: Unknown result type (might be due to invalid IL or missing references)
		//IL_200a: Unknown result type (might be due to invalid IL or missing references)
		//IL_200f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2013: Unknown result type (might be due to invalid IL or missing references)
		//IL_2018: Unknown result type (might be due to invalid IL or missing references)
		//IL_201a: Unknown result type (might be due to invalid IL or missing references)
		//IL_201f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2028: Unknown result type (might be due to invalid IL or missing references)
		//IL_1296: Unknown result type (might be due to invalid IL or missing references)
		//IL_129d: Expected O, but got Unknown
		//IL_12a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ad: Expected O, but got Unknown
		//IL_125e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1265: Expected O, but got Unknown
		//IL_1247: Unknown result type (might be due to invalid IL or missing references)
		//IL_124e: Expected O, but got Unknown
		//IL_275d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2764: Expected O, but got Unknown
		//IL_2768: Unknown result type (might be due to invalid IL or missing references)
		//IL_2774: Unknown result type (might be due to invalid IL or missing references)
		//IL_2785: Unknown result type (might be due to invalid IL or missing references)
		//IL_278c: Expected O, but got Unknown
		//IL_2742: Unknown result type (might be due to invalid IL or missing references)
		//IL_274c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2753: Expected O, but got Unknown
		//IL_279f: Unknown result type (might be due to invalid IL or missing references)
		//IL_27a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b0: Expected O, but got Unknown
		//IL_27eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_27f2: Expected O, but got Unknown
		//IL_2819: Unknown result type (might be due to invalid IL or missing references)
		//IL_2820: Expected O, but got Unknown
		//IL_282f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2834: Unknown result type (might be due to invalid IL or missing references)
		//IL_283e: Unknown result type (might be due to invalid IL or missing references)
		//IL_284a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2860: Expected O, but got Unknown
		//IL_2881: Unknown result type (might be due to invalid IL or missing references)
		//IL_289c: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ca: Expected O, but got Unknown
		//IL_28df: Unknown result type (might be due to invalid IL or missing references)
		//IL_28e8: Expected O, but got Unknown
		//IL_2912: Unknown result type (might be due to invalid IL or missing references)
		//IL_2917: Unknown result type (might be due to invalid IL or missing references)
		//IL_2926: Unknown result type (might be due to invalid IL or missing references)
		//IL_2932: Unknown result type (might be due to invalid IL or missing references)
		//IL_294c: Expected O, but got Unknown
		//IL_335a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3365: Unknown result type (might be due to invalid IL or missing references)
		//IL_336a: Unknown result type (might be due to invalid IL or missing references)
		//IL_162c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1633: Expected O, but got Unknown
		//IL_163a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1641: Expected O, but got Unknown
		//IL_29e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f1: Expected O, but got Unknown
		//IL_2a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a55: Expected O, but got Unknown
		//IL_2996: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2603: Expected O, but got Unknown
		//IL_25e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ec: Expected O, but got Unknown
		//IL_2a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2697: Unknown result type (might be due to invalid IL or missing references)
		//IL_269e: Expected O, but got Unknown
		//IL_26a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ac: Expected O, but got Unknown
		//IL_2679: Unknown result type (might be due to invalid IL or missing references)
		//IL_2680: Expected O, but got Unknown
		//IL_2689: Unknown result type (might be due to invalid IL or missing references)
		//IL_2690: Expected O, but got Unknown
		//IL_2641: Unknown result type (might be due to invalid IL or missing references)
		//IL_2648: Expected O, but got Unknown
		//IL_262a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2631: Expected O, but got Unknown
		//IL_186d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1874: Expected O, but got Unknown
		//IL_187f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1884: Unknown result type (might be due to invalid IL or missing references)
		//IL_1889: Unknown result type (might be due to invalid IL or missing references)
		//IL_1890: Unknown result type (might be due to invalid IL or missing references)
		//IL_1897: Unknown result type (might be due to invalid IL or missing references)
		//IL_189e: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa6: Expected O, but got Unknown
		//IL_0fa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa8: Expected O, but got Unknown
		//IL_0fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faf: Expected O, but got Unknown
		//IL_2b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b1c: Expected O, but got Unknown
		//IL_2b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b42: Expected O, but got Unknown
		//IL_2b5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b88: Expected O, but got Unknown
		//IL_2b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c37: Expected O, but got Unknown
		//IL_2c5b: Expected O, but got Unknown
		//IL_2c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7b: Expected O, but got Unknown
		//IL_2d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d91: Expected O, but got Unknown
		//IL_1d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6b: Expected O, but got Unknown
		//IL_1d66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6d: Expected O, but got Unknown
		//IL_1d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d74: Expected O, but got Unknown
		//IL_1431: Unknown result type (might be due to invalid IL or missing references)
		//IL_144c: Unknown result type (might be due to invalid IL or missing references)
		//IL_362e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3638: Unknown result type (might be due to invalid IL or missing references)
		//IL_3643: Unknown result type (might be due to invalid IL or missing references)
		//IL_364d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3657: Expected O, but got Unknown
		//IL_3657: Expected O, but got Unknown
		//IL_227d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2282: Unknown result type (might be due to invalid IL or missing references)
		//IL_2288: Unknown result type (might be due to invalid IL or missing references)
		//IL_2292: Unknown result type (might be due to invalid IL or missing references)
		//IL_2297: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_22cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e7: Expected O, but got Unknown
		//IL_22e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e9: Expected O, but got Unknown
		//IL_22e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f0: Expected O, but got Unknown
		//IL_15e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_36cc: Expected O, but got Unknown
		//IL_36da: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_36fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3704: Expected O, but got Unknown
		//IL_36a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_36aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_36b3: Expected O, but got Unknown
		//IL_2ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_2df6: Expected O, but got Unknown
		//IL_2dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dd3: Expected O, but got Unknown
		//IL_3729: Unknown result type (might be due to invalid IL or missing references)
		//IL_3733: Unknown result type (might be due to invalid IL or missing references)
		//IL_373c: Expected O, but got Unknown
		//IL_37a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_37ae: Expected O, but got Unknown
		//IL_2eaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb3: Expected O, but got Unknown
		//IL_2ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ecf: Expected O, but got Unknown
		//IL_2e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e5b: Expected O, but got Unknown
		//IL_2e2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e38: Expected O, but got Unknown
		//IL_37df: Unknown result type (might be due to invalid IL or missing references)
		//IL_37e8: Expected O, but got Unknown
		//IL_3801: Unknown result type (might be due to invalid IL or missing references)
		//IL_3806: Unknown result type (might be due to invalid IL or missing references)
		//IL_3814: Unknown result type (might be due to invalid IL or missing references)
		//IL_3824: Unknown result type (might be due to invalid IL or missing references)
		//IL_383e: Expected O, but got Unknown
		//IL_387b: Unknown result type (might be due to invalid IL or missing references)
		//IL_389e: Unknown result type (might be due to invalid IL or missing references)
		//IL_38c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_38ce: Expected O, but got Unknown
		//IL_38e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f0: Expected O, but got Unknown
		//IL_391c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3921: Unknown result type (might be due to invalid IL or missing references)
		//IL_3936: Unknown result type (might be due to invalid IL or missing references)
		//IL_3946: Unknown result type (might be due to invalid IL or missing references)
		//IL_3960: Expected O, but got Unknown
		//IL_2ef1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2efa: Expected O, but got Unknown
		//IL_2f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f3e: Expected O, but got Unknown
		//IL_2f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f6a: Expected O, but got Unknown
		//IL_2f94: Expected O, but got Unknown
		//IL_1140: Unknown result type (might be due to invalid IL or missing references)
		//IL_115b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a0d: Expected O, but got Unknown
		//IL_3a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a7d: Expected O, but got Unknown
		//IL_39ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_34b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_34bb: Expected O, but got Unknown
		//IL_348f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3498: Expected O, but got Unknown
		//IL_1f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f20: Unknown result type (might be due to invalid IL or missing references)
		//IL_3acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_35a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_35aa: Expected O, but got Unknown
		//IL_35b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_35c0: Expected O, but got Unknown
		//IL_356f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3578: Expected O, but got Unknown
		//IL_358b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3594: Expected O, but got Unknown
		//IL_3517: Unknown result type (might be due to invalid IL or missing references)
		//IL_3520: Expected O, but got Unknown
		//IL_34f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_34fd: Expected O, but got Unknown
		//IL_323a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3243: Expected O, but got Unknown
		//IL_325c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3261: Unknown result type (might be due to invalid IL or missing references)
		//IL_3266: Unknown result type (might be due to invalid IL or missing references)
		//IL_3271: Unknown result type (might be due to invalid IL or missing references)
		//IL_327c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3287: Unknown result type (might be due to invalid IL or missing references)
		//IL_328c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3291: Unknown result type (might be due to invalid IL or missing references)
		//IL_329d: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_32b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_32be: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_32dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2481: Unknown result type (might be due to invalid IL or missing references)
		//IL_249c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b4c: Expected O, but got Unknown
		//IL_3b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b72: Expected O, but got Unknown
		//IL_3b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bb8: Expected O, but got Unknown
		//IL_3bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c73: Expected O, but got Unknown
		//IL_3c97: Expected O, but got Unknown
		//IL_3c9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ce0: Unknown result type (might be due to invalid IL or missing references)
		//IL_302f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3034: Unknown result type (might be due to invalid IL or missing references)
		//IL_3039: Unknown result type (might be due to invalid IL or missing references)
		//IL_3049: Unknown result type (might be due to invalid IL or missing references)
		//IL_3054: Unknown result type (might be due to invalid IL or missing references)
		//IL_305f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3064: Unknown result type (might be due to invalid IL or missing references)
		//IL_3069: Unknown result type (might be due to invalid IL or missing references)
		//IL_3075: Unknown result type (might be due to invalid IL or missing references)
		//IL_3080: Unknown result type (might be due to invalid IL or missing references)
		//IL_3085: Unknown result type (might be due to invalid IL or missing references)
		//IL_308a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3096: Unknown result type (might be due to invalid IL or missing references)
		//IL_309b: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_30bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_30c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3128: Unknown result type (might be due to invalid IL or missing references)
		//IL_312d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3132: Unknown result type (might be due to invalid IL or missing references)
		//IL_3142: Unknown result type (might be due to invalid IL or missing references)
		//IL_314d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3158: Unknown result type (might be due to invalid IL or missing references)
		//IL_315d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3162: Unknown result type (might be due to invalid IL or missing references)
		//IL_316e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3179: Unknown result type (might be due to invalid IL or missing references)
		//IL_317e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3183: Unknown result type (might be due to invalid IL or missing references)
		//IL_318f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3194: Unknown result type (might be due to invalid IL or missing references)
		//IL_319a: Unknown result type (might be due to invalid IL or missing references)
		//IL_319f: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_31be: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e33: Expected O, but got Unknown
		//IL_3e07: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e10: Expected O, but got Unknown
		//IL_3f19: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f22: Expected O, but got Unknown
		//IL_3f2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f38: Expected O, but got Unknown
		//IL_3f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f43: Expected O, but got Unknown
		//IL_3f66: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f87: Expected O, but got Unknown
		//IL_3faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fb3: Expected O, but got Unknown
		//IL_3fdd: Expected O, but got Unknown
		//IL_3ee7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ef0: Expected O, but got Unknown
		//IL_3f03: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f0c: Expected O, but got Unknown
		//IL_3e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e98: Expected O, but got Unknown
		//IL_3e6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e75: Expected O, but got Unknown
		//IL_4283: Unknown result type (might be due to invalid IL or missing references)
		//IL_428c: Expected O, but got Unknown
		//IL_42a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_42aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_42af: Unknown result type (might be due to invalid IL or missing references)
		//IL_42ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_42c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_42d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_42d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_42da: Unknown result type (might be due to invalid IL or missing references)
		//IL_42e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_42f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_42f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_42fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4307: Unknown result type (might be due to invalid IL or missing references)
		//IL_430c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4312: Unknown result type (might be due to invalid IL or missing references)
		//IL_4317: Unknown result type (might be due to invalid IL or missing references)
		//IL_4325: Unknown result type (might be due to invalid IL or missing references)
		//IL_432b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4331: Unknown result type (might be due to invalid IL or missing references)
		//IL_4078: Unknown result type (might be due to invalid IL or missing references)
		//IL_407d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4082: Unknown result type (might be due to invalid IL or missing references)
		//IL_4092: Unknown result type (might be due to invalid IL or missing references)
		//IL_409d: Unknown result type (might be due to invalid IL or missing references)
		//IL_40a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_40b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_40be: Unknown result type (might be due to invalid IL or missing references)
		//IL_40c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_40d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_40df: Unknown result type (might be due to invalid IL or missing references)
		//IL_40e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_4102: Unknown result type (might be due to invalid IL or missing references)
		//IL_4108: Unknown result type (might be due to invalid IL or missing references)
		//IL_410e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4171: Unknown result type (might be due to invalid IL or missing references)
		//IL_4176: Unknown result type (might be due to invalid IL or missing references)
		//IL_417b: Unknown result type (might be due to invalid IL or missing references)
		//IL_418b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4196: Unknown result type (might be due to invalid IL or missing references)
		//IL_41a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_41a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_41b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_41c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_41c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_41cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_41dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_41e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_41e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_41fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4201: Unknown result type (might be due to invalid IL or missing references)
		//IL_4207: Unknown result type (might be due to invalid IL or missing references)
		if (!(InSortedGems.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			OutProngs.SetData(new CSDataArray());
			OutProfiles.SetData(new CSDataArray());
			return base.Update();
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
		bool value = true;
		InDoMeshedProngs.GetBoolean(ref value);
		double value2 = 0.0;
		InSizeMin.GetDouble(ref value2);
		double value3 = 100.0;
		InSizeMax.GetDouble(ref value3);
		double value4 = 0.0;
		InStartHeight.GetDouble(ref value4);
		double value5 = 0.0;
		InMiddleHeight.GetDouble(ref value5);
		double value6 = 0.0;
		InEndHeight.GetDouble(ref value6);
		double value7 = 0.0;
		InStartFillet.GetDoublePercentage(ref value7);
		double value8 = 0.0;
		InMiddleFillet.GetDoublePercentage(ref value8);
		double value9 = 0.0;
		InEndFillet.GetDoublePercentage(ref value9);
		double value10 = 0.0;
		InStartTaper.GetDoublePercentage(ref value10);
		double value11 = 0.0;
		InMiddleTaper.GetDoublePercentage(ref value11);
		double value12 = 0.0;
		InEndTaper.GetDoublePercentage(ref value12);
		double value13 = 0.0;
		InNudgeEdge.GetDouble(ref value13);
		double value14 = 0.0;
		InNudgeGem.GetDouble(ref value14);
		InStartNudgeEdge.GetValue(out double value15);
		InEndNudgeEdge.GetValue(out double value16);
		InStartNudgeGem.GetValue(out double value17);
		InEndNudgeGem.GetValue(out double value18);
		InCornerNudgeEdge.GetValue(out double value19);
		InCornerNudgeGem.GetValue(out double value20);
		double value21 = 0.0;
		InStartVerticalLocation.GetDouble(ref value21);
		double value22 = 0.0;
		InMiddleVerticalLocation.GetDouble(ref value22);
		double value23 = 0.0;
		InEndVerticalLocation.GetDouble(ref value23);
		ValueListItem whichItem = ValueListItem.ProngPlacerStyleEnd9;
		InEndProngStyle.GetValueListItem(ref whichItem);
		bool value24 = true;
		InEnableEndProngRotationMirroring.GetBoolean(ref value24);
		bool value25 = false;
		InEnableEndProngEndControls.GetBoolean(ref value25);
		double value26 = 0.25;
		InSizePercentageStart.GetDoublePercentage(ref value26);
		double value27 = 0.0;
		InRotationStart.GetDouble(ref value27);
		double value28 = 45.0;
		InSpreadStart.GetDouble(ref value28);
		double value29 = 0.25;
		InSizePercentageEnd.GetDoublePercentage(ref value29);
		double value30 = 0.0;
		InRotationEnd.GetDouble(ref value30);
		double value31 = 45.0;
		InSpreadEnd.GetDouble(ref value31);
		InStartBlendTop.GetValue(out double value32);
		InMiddleBlendTop.GetValue(out double value33);
		InEndBlendTop.GetValue(out double value34);
		InStartBlendBot.GetValue(out double value35);
		InMiddleBlendBot.GetValue(out double value36);
		InEndBlendBot.GetValue(out double value37);
		InStartBlendEdge.GetValue(out double value38);
		InMiddleBlendEdge.GetValue(out double value39);
		InEndBlendEdge.GetValue(out double value40);
		InStartBottomArcPercentage.GetValue(out double value41);
		InMiddleBottomArcPercentage.GetValue(out double value42);
		InEndBottomArcPercentage.GetValue(out double value43);
		InCornerProngAngle.GetValue(out double value44);
		InCornerProngSize.GetValue(out int value45);
		InCornerProngSizeOffset.GetValue(out double value46);
		ValueListItem whichItem2 = ValueListItem.ProngPlacerSettingStyleProng;
		InSettingStyle.GetValueListItem(ref whichItem2);
		InProngSizeControl.GetValue(out int value47);
		bool isCamMode = CSApplicationHelper.Application.IsCamMode;
		InCamModeOverride.GetValue(out bool value48);
		if (value48)
		{
			CSApplicationHelper.Application.IsCamMode = false;
		}
		bool flag = false;
		switch (whichItem2)
		{
		case ValueListItem.ProngPlacerSettingStyleBar:
			flag = true;
			break;
		case ValueListItem.ProngPlacerSettingStyleAuto:
		{
			string text = (cSDataArray.GetArrayItem(0) as CSGemData)?.GemShape;
			if (text != null && (text.ToLower() == "princess" || text.ToLower() == "emerald" || text.ToLower() == "emerald square" || text.ToLower() == "emerald rotated 90"))
			{
				flag = true;
			}
			break;
		}
		case ValueListItem.ProngPlacerSettingStyleOff:
			OutProngs.SetData(new CSDataArray());
			OutProfiles.SetData(new CSDataArray());
			return base.Update();
		}
		if (!value25)
		{
			value29 = value26;
			value30 = value27;
			if (value24)
			{
				value30 *= -1.0;
			}
			value31 = value28;
		}
		ConcurrentBag<CSData> concurrentBag = new ConcurrentBag<CSData>();
		ConcurrentBag<CSData> concurrentBag2 = new ConcurrentBag<CSData>();
		OnCurve rail = CSUtilGemRails.Rail(cSDataArray);
		OnCurve val3 = CSUtilGemRails.Left(cSDataArray, rail).DuplicateCurve();
		if (!val3.IsClosed())
		{
			double num = -1.23432101234321E+308;
			if (!val3.GetLength(ref num))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref val3, (Type)2, 0, num * 0.5);
			RhUtil.RhinoExtendCurve(ref val3, (Type)2, 1, num * 0.5);
		}
		OnCurve val4 = CSUtilGemRails.Right(cSDataArray, rail).DuplicateCurve();
		if (!val4.IsClosed())
		{
			double num2 = -1.23432101234321E+308;
			if (!val4.GetLength(ref num2))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref val4, (Type)2, 0, num2 * 0.5);
			RhUtil.RhinoExtendCurve(ref val4, (Type)2, 1, num2 * 0.5);
		}
		On3dVector val5 = new On3dVector(OnUtil.On_UNSET_VECTOR);
		On3dVector val6 = new On3dVector(OnUtil.On_UNSET_VECTOR);
		_prongCache.Clear();
		CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, 0);
		double gemZSize = channelGemData.GemZSize;
		double num3 = 0.0;
		InMaxGem.GetValueListItem(ref _prongBase);
		if (_prongBase == ValueListItem.FollowLargestGem)
		{
			for (int i = 1; i < cSDataArray.GetArrayCount(); i++)
			{
				if (gemZSize < CSUtilGemRails.GetChannelGemData(cSDataArray, i).GemZSize)
				{
					gemZSize = CSUtilGemRails.GetChannelGemData(cSDataArray, i).GemZSize;
					channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
				}
				double num4 = cSDataArray.GetArrayCount();
				double vHeight;
				double vVerticalLocation;
				if (i < cSDataArray.GetArrayCount() / 2)
				{
					vHeight = value4 * ((num4 / 2.0 - (double)i) / (num4 / 2.0)) + value5 * ((double)i / (num4 / 2.0));
					vVerticalLocation = value21 * ((num4 / 2.0 - (double)i) / (num4 / 2.0)) + value22 * ((double)i / (num4 / 2.0));
				}
				else
				{
					vHeight = value5 * (num4 - (double)i) / (double)i + value6 * (1.0 - (num4 - (double)i) / (double)i);
					vVerticalLocation = value22 * (num4 - (double)i) / (double)i + value23 * (1.0 - (num4 - (double)i) / (double)i);
				}
				double height2;
				string height = GetHeight(CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1), CSUtilGemRails.GetChannelGemData(cSDataArray, i), channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value13, value14, vHeight, vVerticalLocation, out height2);
				if (!string.IsNullOrEmpty(height))
				{
					return UpdateError(height);
				}
				if (height2 > num3)
				{
					num3 = height2;
				}
			}
		}
		InPercentToggle.GetValue(out bool value49);
		double value50 = 0.0;
		double value51 = 0.0;
		double value52 = 0.0;
		if (value49)
		{
			InStartProngPercentage.GetValue(out value50);
			value50 = 1.0 - value50;
			InMiddleProngPercentage.GetValue(out value51);
			value51 = 1.0 - value51;
			InEndProngPercentage.GetValue(out value52);
			value52 = 1.0 - value52;
		}
		bool flag2 = CSUtilGemRails.IsClosed(cSDataArray.Array);
		Point3d center;
		Line val12 = default(Line);
		double num12 = default(double);
		double num13 = default(double);
		double num14 = default(double);
		for (int j = 0; j < cSDataArray.GetArrayCount(); j++)
		{
			int num5 = j - 1;
			int num6 = j + 1;
			if (num5 < 0)
			{
				if (!flag2)
				{
					continue;
				}
				num5 = cSDataArray.GetArrayCount() - 1;
			}
			if (num6 >= cSDataArray.GetArrayCount())
			{
				num6 = ((!flag2) ? j : 0);
			}
			center = CSUtilGemRails.GetChannelGemData(cSDataArray, j).Center;
			double num7 = ((Point3d)(ref center)).DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, num5).Center);
			double num8 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).GemXSize * 2.0;
			if (num7 > num8)
			{
				continue;
			}
			CSChannelGemData channelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, num5);
			CSChannelGemData channelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			CSChannelGemData channelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, num6);
			if (channelGemData2 == null || channelGemData3 == null)
			{
				return UpdateError("Invalid gems");
			}
			CSDataHelper.SetSettingType(channelGemData3, flag ? GemSettingType.BR : GemSettingType.PR);
			double num9 = cSDataArray.GetArrayCount();
			double vHeight2;
			double num10;
			double num11;
			if (j < cSDataArray.GetArrayCount() / 2)
			{
				vHeight2 = value4 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value5 * ((double)j / (num9 / 2.0));
				num10 = value21 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value22 * ((double)j / (num9 / 2.0));
				num11 = value50 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value51 * ((double)j / (num9 / 2.0));
			}
			else
			{
				vHeight2 = value5 * (num9 - (double)j) / (double)j + value6 * (1.0 - (num9 - (double)j) / (double)j);
				num10 = value22 * (num9 - (double)j) / (double)j + value23 * (1.0 - (num9 - (double)j) / (double)j);
				num11 = value51 * (num9 - (double)j) / (double)j + value52 * (1.0 - (num9 - (double)j) / (double)j);
			}
			OnCircle circleLeft;
			OnCircle circleRight;
			double height3;
			string circles = GetCircles(channelGemData2, channelGemData3, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value13, value14, vHeight2, num10, value49, num11, out circleLeft, out circleRight, out height3);
			if (!string.IsNullOrEmpty(circles))
			{
				return UpdateError(circles);
			}
			if (_prongBase == ValueListItem.FollowLargestGem)
			{
				height3 = (value49 ? (num3 * num11) : num3);
			}
			On3dVector val7 = new On3dVector(circleLeft.Center());
			On3dVector val8 = new On3dVector(circleRight.Center());
			On3dPoint value53 = new On3dPoint((IOn3dVector)(object)((val7 + (IOn3dVector)(object)val8) / 2.0));
			On3dVector value54 = val8 - (IOn3dVector)(object)val7;
			if (CSApplicationHelper.Application.IsCamMode)
			{
				OnXform val9 = new OnXform();
				val9.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
				Vector3d zAxis = Vector3d.ZAxis;
				((Vector3d)(ref zAxis)).Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val9));
				((Vector3d)(ref zAxis)).Unitize();
				OnXform val10 = new OnXform();
				val10.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
				Vector3d zAxis2 = Vector3d.ZAxis;
				((Vector3d)(ref zAxis2)).Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val10));
				((Vector3d)(ref zAxis2)).Unitize();
				Vector3d destination = zAxis + zAxis2;
				((Vector3d)(ref destination)).Unitize();
				Transform val11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value53), CSConvert.On3dVectorCopyToVector3d(value54), destination);
				Point3d origin = Point3d.Origin;
				((Point3d)(ref origin)).Transform(val11);
				((Line)(ref val12))._002Ector(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
				((Line)(ref val12)).Transform(val11);
				Curve obj = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)new List<Point3d> { channelGemData2.ZMaxPoint, channelGemData3.ZMaxPoint, channelGemData4.ZMaxPoint }, 3);
				Curve val13 = obj.DuplicateCurve();
				Curve val14 = obj.DuplicateCurve();
				Vector3d val15 = CSConvert.On3dVectorCopyToVector3d(circleLeft.plane.yaxis);
				((GeometryBase)val13).Transform(Transform.Translation(4.0 * val15));
				((GeometryBase)val14).Transform(Transform.Translation(-4.0 * val15));
				NurbsSurface obj2 = NurbsSurface.CreateRuledSurface(val13, val14);
				((Surface)obj2).ClosestPoint(((Line)(ref val12)).PointAt(1.0), ref num12, ref num13);
				Point3d val16 = ((Surface)obj2).PointAt(num12, num13);
				if (((Point3d)(ref val16)).DistanceTo(origin) > height3 * 0.8)
				{
					height3 = ((Point3d)(ref val16)).DistanceTo(origin) * 1.2;
				}
			}
			if (!flag)
			{
				Vector3d val17 = channelGemData3.Center - channelGemData2.Center;
				((Vector3d)(ref val17)).Unitize();
				Vector3d val18 = channelGemData3.Center - channelGemData4.Center;
				((Vector3d)(ref val18)).Unitize();
				if (num6 == j)
				{
					val18 = val17 * -1.0;
				}
				if (RhinoMath.ToDegrees(Math.Acos(val17 * val18)) < value44)
				{
					val17 *= channelGemData3.GemXSize / 2.0;
					val18 *= channelGemData3.GemXSize / 2.0;
					Vector3d val19 = val17 + val18;
					Point3d val20 = channelGemData3.Center + val19;
					channelGemData3.GetCurve().ClosestPoint(val20, ref num14);
					OnCircle circleLeft2;
					OnCircle circleRight2;
					double height4;
					string circles2 = GetCircles(channelGemData2, channelGemData3, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value19, value20, vHeight2, num10, value49, num11, out circleLeft2, out circleRight2, out height4);
					OnCircle circleLeft3;
					OnCircle circleRight3;
					double height5;
					string circles3 = GetCircles(channelGemData3, channelGemData4, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value19, value20, vHeight2, num10, value49, num11, out circleLeft3, out circleRight3, out height5);
					if (!string.IsNullOrEmpty(circles2) || !string.IsNullOrEmpty(circles3))
					{
						break;
					}
					double num15 = (circleRight3.radius + circleRight2.radius + circleLeft3.radius + circleLeft2.radius) / 4.0 + value46;
					double[] array = new double[4] { circleRight3.radius, circleRight2.radius, circleLeft2.radius, circleLeft3.radius };
					if (value45 == 157)
					{
						double num16 = array[0];
						for (int k = 1; k < array.Length; k++)
						{
							if (num16 < array[k])
							{
								num16 = array[k];
							}
						}
						num15 = num16 + value46;
					}
					if (value45 == 158)
					{
						double num17 = array[0];
						for (int l = 1; l < array.Length; l++)
						{
							if (num17 > array[l])
							{
								num17 = array[l];
							}
						}
						num15 = num17 + value46;
					}
					double num18 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData3.Size) / 2.0) : channelGemData3.GemZMin);
					Point3d val21 = channelGemData3.GetCurve().PointAt(num14);
					((Point3d)(ref val21)).Transform(Transform.Translation(channelGemData3.Normal * (num10 - num18)));
					((Vector3d)(ref val19)).Unitize();
					val19 *= num15 - value20;
					val21 += val19;
					On3dPoint val22 = CSConvert.Point3dCopyToOn3dPoint(val21);
					OnCircle val23 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val22, (IOn3dVector)(object)channelGemData3.Normal.Convert()), (IOn3dPoint)(object)val22, num15);
					OnXform val24 = new OnXform();
					val24.ChangeBasis((IOnPlane)(object)val23.plane, OnUtil.On_xy_plane);
					double num19 = val23.radius * 2.0;
					if (num19 > value2 && num19 < value3)
					{
						double taperPer;
						double filletPer;
						if (j < cSDataArray.GetArrayCount() / 2)
						{
							_ = (num9 / 2.0 - (double)j) / (num9 / 2.0);
							_ = (double)j / (num9 / 2.0);
							taperPer = value10 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value11 * ((double)j / (num9 / 2.0));
							filletPer = value7 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value8 * ((double)j / (num9 / 2.0));
						}
						else
						{
							_ = value42 * (num9 - (double)j) / (double)j;
							_ = (num9 - (double)j) / (double)j;
							taperPer = value11 * (num9 - (double)j) / (double)j + value12 * (1.0 - (num9 - (double)j) / (double)j);
							filletPer = value8 * (num9 - (double)j) / (double)j + value9 * (1.0 - (num9 - (double)j) / (double)j);
						}
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, num19, height3, filletPer, taperPer, value, out var prong, out var prongBaseRail))
						{
							return UpdateError("Create prong cache failed");
						}
						prong.Transform((IOnXform)(object)val24);
						prongBaseRail.Transform((IOnXform)(object)val24);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val24.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val24.Convert(), applyTransform: false));
					}
				}
			}
			OnXform val25 = new OnXform();
			val25.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
			On3dVector val26 = new On3dVector(OnUtil.On_zaxis);
			val26.Transform((IOnXform)(object)val25);
			val26.Unitize();
			OnXform val27 = new OnXform();
			val27.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
			On3dVector val28 = new On3dVector(OnUtil.On_zaxis);
			val28.Transform((IOnXform)(object)val27);
			val28.Unitize();
			if (value47 == 157)
			{
				if (circleRight.Diameter() > circleLeft.Diameter())
				{
					circleLeft = new OnCircle(circleLeft.Center(), circleRight.radius);
				}
				else
				{
					circleRight = new OnCircle(circleRight.Center(), circleLeft.radius);
				}
			}
			if (value47 == 158)
			{
				if (circleRight.Diameter() < circleLeft.Diameter())
				{
					circleLeft = new OnCircle(circleLeft.Center(), circleRight.radius);
				}
				else
				{
					circleRight = new OnCircle(circleRight.Center(), circleLeft.radius);
				}
			}
			if (value47 == 159)
			{
				double radius = circleRight.radius;
				double radius2 = circleLeft.radius;
				double num20 = (radius + radius2) / 2.0;
				circleRight = new OnCircle(circleRight.Center(), num20);
				circleLeft = new OnCircle(circleLeft.Center(), num20);
			}
			if (!flag)
			{
				double num21 = circleLeft.radius * 2.0;
				if (num21 > value2 && num21 < value3)
				{
					double taperPer2;
					double filletPer2;
					if (j < cSDataArray.GetArrayCount() / 2)
					{
						_ = (num9 / 2.0 - (double)j) / (num9 / 2.0);
						_ = (double)j / (num9 / 2.0);
						taperPer2 = value10 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value11 * ((double)j / (num9 / 2.0));
						filletPer2 = value7 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value8 * ((double)j / (num9 / 2.0));
					}
					else
					{
						_ = value42 * (num9 - (double)j) / (double)j;
						_ = (num9 - (double)j) / (double)j;
						taperPer2 = value11 * (num9 - (double)j) / (double)j + value12 * (1.0 - (num9 - (double)j) / (double)j);
						filletPer2 = value8 * (num9 - (double)j) / (double)j + value9 * (1.0 - (num9 - (double)j) / (double)j);
					}
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num21, height3, filletPer2, taperPer2, value, out var prong2, out var prongBaseRail2))
					{
						return UpdateError("Create prong cache failed");
					}
					prong2.Transform((IOnXform)(object)val25);
					prongBaseRail2.Transform((IOnXform)(object)val25);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val25.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val25.Convert(), applyTransform: false));
				}
			}
			if (!flag)
			{
				double num22 = circleRight.radius * 2.0;
				if (num22 > value2 && num22 < value3)
				{
					double taperPer3;
					double filletPer3;
					if (j < cSDataArray.GetArrayCount() / 2)
					{
						_ = (num9 / 2.0 - (double)j) / (num9 / 2.0);
						_ = (double)j / (num9 / 2.0);
						taperPer3 = value10 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value11 * ((double)j / (num9 / 2.0));
						filletPer3 = value7 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value8 * ((double)j / (num9 / 2.0));
					}
					else
					{
						_ = value42 * (num9 - (double)j) / (double)j;
						_ = (num9 - (double)j) / (double)j;
						taperPer3 = value11 * (num9 - (double)j) / (double)j + value12 * (1.0 - (num9 - (double)j) / (double)j);
						filletPer3 = value8 * (num9 - (double)j) / (double)j + value9 * (1.0 - (num9 - (double)j) / (double)j);
					}
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num22, height3, filletPer3, taperPer3, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					prong3.Transform((IOnXform)(object)val27);
					prongBaseRail3.Transform((IOnXform)(object)val27);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), val27.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail3).Convert(), val27.Convert(), applyTransform: false));
				}
			}
			On3dVector val29 = val26 + (IOn3dVector)(object)val28;
			val29.Unitize();
			if (flag)
			{
				val7 = new On3dVector(circleLeft.Center());
				val8 = new On3dVector(circleRight.Center());
				double baseDomeRadius;
				double blendEdge;
				double domeRadius;
				double bottomArcPercent;
				double filletPer4;
				if (j < cSDataArray.GetArrayCount() / 2)
				{
					baseDomeRadius = value35 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value36 * ((double)j / (num9 / 2.0));
					blendEdge = value38 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value39 * ((double)j / (num9 / 2.0));
					domeRadius = value32 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value33 * ((double)j / (num9 / 2.0));
					bottomArcPercent = value41 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value42 * ((double)j / (num9 / 2.0));
					filletPer4 = value10 * ((num9 / 2.0 - (double)j) / (num9 / 2.0)) + value11 * ((double)j / (num9 / 2.0));
				}
				else
				{
					baseDomeRadius = value36 * (num9 - (double)j) / (double)j + value37 * (1.0 - (num9 - (double)j) / (double)j);
					blendEdge = value39 * (num9 - (double)j) / (double)j + value40 * (1.0 - (num9 - (double)j) / (double)j);
					domeRadius = value33 * (num9 - (double)j) / (double)j + value34 * (1.0 - (num9 - (double)j) / (double)j);
					bottomArcPercent = value42 * (num9 - (double)j) / (double)j + value43 * (1.0 - (num9 - (double)j) / (double)j);
					filletPer4 = value11 * (num9 - (double)j) / (double)j + value12 * (1.0 - (num9 - (double)j) / (double)j);
				}
				Brep geometry = MakeBar((val7 - (IOn3dVector)(object)val8).Length(), circleLeft.radius, circleRight.radius, height3, filletPer4, domeRadius, baseDomeRadius, blendEdge, bottomArcPercent);
				value53 = new On3dPoint((IOn3dVector)(object)((val7 + (IOn3dVector)(object)val8) / 2.0));
				value54 = val8 - (IOn3dVector)(object)val7;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value53), CSConvert.On3dVectorCopyToVector3d(value54), CSConvert.On3dVectorCopyToVector3d(val29));
				concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry, transform));
			}
			if (val5 == OnUtil.On_UNSET_VECTOR)
			{
				val5 = val29;
			}
			val6 = val29;
		}
		if (!flag2)
		{
			double num26 = default(double);
			double num35 = default(double);
			for (int m = 0; m < cSDataArray.GetArrayCount(); m++)
			{
				int num23 = ((cSDataArray.GetArrayCount() > 1) ? (m + 1) : 0);
				int index = ((m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1));
				if (cSDataArray.GetArrayCount() > 1)
				{
					double num24;
					if (m != cSDataArray.GetArrayCount() - 1)
					{
						center = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center;
						num24 = ((Point3d)(ref center)).DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, m + 1).Center);
					}
					else
					{
						center = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center;
						num24 = ((Point3d)(ref center)).DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Center);
						num23 = 0;
					}
					double num25 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).GemXSize * 2.0;
					if (num24 <= num25)
					{
						continue;
					}
				}
				if (cSDataArray.GetArrayCount() > 1)
				{
					if (!flag)
					{
						CSChannelGemData channelGemData5 = CSUtilGemRails.GetChannelGemData(cSDataArray, index);
						CSChannelGemData channelGemData6 = CSUtilGemRails.GetChannelGemData(cSDataArray, m);
						CSChannelGemData channelGemData7 = CSUtilGemRails.GetChannelGemData(cSDataArray, num23);
						Vector3d val30 = Vector3d.Unset;
						if (channelGemData5 != null)
						{
							val30 = channelGemData6.Center - channelGemData5.Center;
						}
						((Vector3d)(ref val30)).Unitize();
						Vector3d val31 = Vector3d.Unset;
						if (channelGemData7 != null)
						{
							val31 = channelGemData6.Center - channelGemData7.Center;
						}
						((Vector3d)(ref val31)).Unitize();
						if (num23 == m)
						{
							val31 = val30 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(val30 * val31)) < value44)
						{
							val30 *= channelGemData6.GemXSize / 2.0;
							val31 *= channelGemData6.GemXSize / 2.0;
							Vector3d val32 = val30 + val31;
							Point3d val33 = channelGemData6.Center + val32;
							channelGemData6.GetCurve().ClosestPoint(val33, ref num26);
							double num27 = cSDataArray.GetArrayCount();
							double vHeight3;
							double num28;
							double percent;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight3 = value4 * ((num27 / 2.0 - (double)m) / (num27 / 2.0)) + value5 * ((double)m / (num27 / 2.0));
								num28 = value21 * ((num27 / 2.0 - (double)m) / (num27 / 2.0)) + value22 * ((double)m / (num27 / 2.0));
								percent = value50 * ((num27 / 2.0 - (double)m) / (num27 / 2.0)) + value51 * ((double)m / (num27 / 2.0));
							}
							else
							{
								vHeight3 = value5 * (num27 - (double)m) / (double)m + value6 * (1.0 - (num27 - (double)m) / (double)m);
								num28 = value22 * (num27 - (double)m) / (double)m + value23 * (1.0 - (num27 - (double)m) / (double)m);
								percent = value51 * (num27 - (double)m) / (double)m + value52 * (1.0 - (num27 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData5, channelGemData6, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value19, value20, vHeight3, num28, value49, percent, out var circleLeft4, out var circleRight4, out var height6)))
							{
								break;
							}
							double num29 = (circleRight4.radius + circleLeft4.radius) / 2.0 + value46;
							double[] array2 = new double[2] { circleRight4.radius, circleLeft4.radius };
							if (value45 == 157)
							{
								double num30 = array2[0];
								for (int n = 1; n < array2.Length; n++)
								{
									if (num30 < array2[n])
									{
										num30 = array2[n];
									}
								}
								num29 = num30 + value46;
							}
							if (value45 == 158)
							{
								double num31 = array2[0];
								for (int num32 = 1; num32 < array2.Length; num32++)
								{
									if (num31 > array2[num32])
									{
										num31 = array2[num32];
									}
								}
								num29 = num31 + value46;
							}
							double num33 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData6.Size) / 2.0) : channelGemData6.GemZMin);
							Point3d val34 = channelGemData6.GetCurve().PointAt(num26);
							((Point3d)(ref val34)).Transform(Transform.Translation(channelGemData6.Normal * (num28 - num33)));
							((Vector3d)(ref val32)).Unitize();
							val32 *= num29 - value20;
							val34 += val32;
							On3dPoint val35 = CSConvert.Point3dCopyToOn3dPoint(val34);
							OnCircle val36 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val35, (IOn3dVector)(object)channelGemData6.Normal.Convert()), (IOn3dPoint)(object)val35, num29);
							OnXform val37 = new OnXform();
							val37.ChangeBasis((IOnPlane)(object)val36.plane, OnUtil.On_xy_plane);
							double num34 = val36.radius * 2.0;
							if (num34 > value2 && num34 < value3)
							{
								double taperPer4;
								double filletPer5;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									_ = (num27 / 2.0 - (double)m) / (num27 / 2.0);
									_ = (double)m / (num27 / 2.0);
									taperPer4 = value10 * ((num27 / 2.0 - (double)m) / (num27 / 2.0)) + value11 * ((double)m / (num27 / 2.0));
									filletPer5 = value7 * ((num27 / 2.0 - (double)m) / (num27 / 2.0)) + value8 * ((double)m / (num27 / 2.0));
								}
								else
								{
									_ = value42 * (num27 - (double)m) / (double)m;
									_ = (num27 - (double)m) / (double)m;
									taperPer4 = value11 * (num27 - (double)m) / (double)m + value12 * (1.0 - (num27 - (double)m) / (double)m);
									filletPer5 = value8 * (num27 - (double)m) / (double)m + value9 * (1.0 - (num27 - (double)m) / (double)m);
								}
								if (!_prongCache.Create((IOnNurbsCurve)(object)val, num34, height6, filletPer5, taperPer4, value, out var prong4, out var prongBaseRail4))
								{
									return UpdateError("Create prong cache failed");
								}
								prong4.Transform((IOnXform)(object)val37);
								prongBaseRail4.Transform((IOnXform)(object)val37);
								concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val37.Convert(), applyTransform: false));
								concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail4).Convert(), val37.Convert(), applyTransform: false));
							}
						}
					}
					if (!flag)
					{
						CSChannelGemData channelGemData8 = CSUtilGemRails.GetChannelGemData(cSDataArray, m);
						CSChannelGemData channelGemData9 = CSUtilGemRails.GetChannelGemData(cSDataArray, num23);
						CSChannelGemData channelGemData10 = CSUtilGemRails.GetChannelGemData(cSDataArray, (num23 + 1 != cSDataArray.GetArrayCount()) ? (num23 + 1) : 0);
						Vector3d val38 = channelGemData9.Center - channelGemData8.Center;
						((Vector3d)(ref val38)).Unitize();
						Vector3d val39 = channelGemData9.Center - channelGemData10.Center;
						((Vector3d)(ref val39)).Unitize();
						if (num23 == m)
						{
							val39 = val38 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(val38 * val39)) < value44)
						{
							val38 *= channelGemData9.GemXSize / 2.0;
							val39 *= channelGemData9.GemXSize / 2.0;
							Vector3d val40 = val38 + val39;
							Point3d val41 = channelGemData9.Center + val40;
							channelGemData9.GetCurve().ClosestPoint(val41, ref num35);
							double num36 = cSDataArray.GetArrayCount();
							double vHeight4;
							double num37;
							double percent2;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight4 = value4 * ((num36 / 2.0 - (double)m) / (num36 / 2.0)) + value5 * ((double)m / (num36 / 2.0));
								num37 = value21 * ((num36 / 2.0 - (double)m) / (num36 / 2.0)) + value22 * ((double)m / (num36 / 2.0));
								percent2 = value50 * ((num36 / 2.0 - (double)m) / (num36 / 2.0)) + value51 * ((double)m / (num36 / 2.0));
							}
							else
							{
								vHeight4 = value5 * (num36 - (double)m) / (double)m + value6 * (1.0 - (num36 - (double)m) / (double)m);
								num37 = value22 * (num36 - (double)m) / (double)m + value23 * (1.0 - (num36 - (double)m) / (double)m);
								percent2 = value51 * (num36 - (double)m) / (double)m + value52 * (1.0 - (num36 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData9, channelGemData10, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value19, value20, vHeight4, num37, value49, percent2, out var circleLeft5, out var circleRight5, out var height7)))
							{
								break;
							}
							double num38 = (circleRight5.radius + circleLeft5.radius) / 2.0 + value46;
							double[] array3 = new double[2] { circleRight5.radius, circleLeft5.radius };
							if (value45 == 157)
							{
								double num39 = array3[0];
								for (int num40 = 1; num40 < array3.Length; num40++)
								{
									if (num39 < array3[num40])
									{
										num39 = array3[num40];
									}
								}
								num38 = num39 + value46;
							}
							if (value45 == 158)
							{
								double num41 = array3[0];
								for (int num42 = 1; num42 < array3.Length; num42++)
								{
									if (num41 > array3[num42])
									{
										num41 = array3[num42];
									}
								}
								num38 = num41 + value46;
							}
							double num43 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData9.Size) / 2.0) : channelGemData9.GemZMin);
							Point3d val42 = channelGemData9.GetCurve().PointAt(num35);
							((Point3d)(ref val42)).Transform(Transform.Translation(channelGemData9.Normal * (num37 - num43)));
							((Vector3d)(ref val40)).Unitize();
							val40 *= num38 - value20;
							val42 += val40;
							On3dPoint val43 = CSConvert.Point3dCopyToOn3dPoint(val42);
							OnCircle val44 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val43, (IOn3dVector)(object)channelGemData9.Normal.Convert()), (IOn3dPoint)(object)val43, num38);
							OnXform val45 = new OnXform();
							val45.ChangeBasis((IOnPlane)(object)val44.plane, OnUtil.On_xy_plane);
							double num44 = val44.radius * 2.0;
							if (num44 > value2 && num44 < value3)
							{
								double taperPer5;
								double filletPer6;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									_ = (num36 / 2.0 - (double)m) / (num36 / 2.0);
									_ = (double)m / (num36 / 2.0);
									taperPer5 = value10 * ((num36 / 2.0 - (double)m) / (num36 / 2.0)) + value11 * ((double)m / (num36 / 2.0));
									filletPer6 = value7 * ((num36 / 2.0 - (double)m) / (num36 / 2.0)) + value8 * ((double)m / (num36 / 2.0));
								}
								else
								{
									_ = value42 * (num36 - (double)m) / (double)m;
									_ = (num36 - (double)m) / (double)m;
									taperPer5 = value11 * (num36 - (double)m) / (double)m + value12 * (1.0 - (num36 - (double)m) / (double)m);
									filletPer6 = value8 * (num36 - (double)m) / (double)m + value9 * (1.0 - (num36 - (double)m) / (double)m);
								}
								if (!_prongCache.Create((IOnNurbsCurve)(object)val, num44, height7, filletPer6, taperPer5, value, out var prong5, out var prongBaseRail5))
								{
									return UpdateError("Create prong cache failed");
								}
								prong5.Transform((IOnXform)(object)val45);
								prongBaseRail5.Transform((IOnXform)(object)val45);
								concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), val45.Convert(), applyTransform: false));
								concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail5).Convert(), val45.Convert(), applyTransform: false));
							}
						}
					}
				}
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					CSChannelGemData cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, num23).Duplicate() as CSChannelGemData;
					if (cSDataArray.GetArrayCount() == 1)
					{
						cSChannelGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData.Normal, cSChannelGemData.Center));
					}
					double num46 = cSChannelGemData.GemXSize * value26 + value15 + value17;
					double height9 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + value4 - value21) : channelGemData.GemZSize);
					if (CSApplicationHelper.Application.IsCamMode)
					{
						CSChannelGemData gemPrev = cSChannelGemData;
						CSChannelGemData gemCurrent = CSUtilGemRails.GetChannelGemData(cSDataArray, (num23 + 1 != cSDataArray.GetArrayCount()) ? (num23 + 1) : 0).Duplicate() as CSChannelGemData;
						OnCircle circleLeft7;
						OnCircle circleRight7;
						double height10;
						string circles5 = GetCircles(gemPrev, gemCurrent, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value15, value17, value4, value21, value49, value50, out circleLeft7, out circleRight7, out height10);
						if (!string.IsNullOrEmpty(circles5))
						{
							return UpdateError(circles5);
						}
						if (value47 == 157)
						{
							if (circleRight7.Diameter() > circleLeft7.Diameter())
							{
								circleLeft7 = new OnCircle(circleLeft7.Center(), circleRight7.radius);
							}
							else
							{
								circleRight7 = new OnCircle(circleRight7.Center(), circleLeft7.radius);
							}
						}
						if (value47 == 158)
						{
							if (circleRight7.Diameter() < circleLeft7.Diameter())
							{
								circleLeft7 = new OnCircle(circleLeft7.Center(), circleRight7.radius);
							}
							else
							{
								circleRight7 = new OnCircle(circleRight7.Center(), circleLeft7.radius);
							}
						}
						if (value47 == 159)
						{
							double radius5 = circleRight7.radius;
							double radius6 = circleLeft7.radius;
							double num47 = (radius5 + radius6) / 2.0;
							circleRight7 = new OnCircle(circleRight7.Center(), num47);
							circleLeft7 = new OnCircle(circleLeft7.Center(), num47);
						}
						On3dVector vLeft = new On3dVector(circleLeft7.Center());
						On3dVector vRight = new On3dVector(circleRight7.Center());
						height9 = GetCamHeightEnd(gemPrev, gemCurrent, circleLeft7, circleRight7, vLeft, vRight);
					}
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num46, height9, value7, value10, value, out var prong7, out var prongBaseRail7))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj3 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
					double num48 = (cSChannelGemData.GemXSize * value26 + value15) / 2.0;
					double num49 = value21 - cSChannelGemData.GemZMin;
					On3dVector val58;
					if (val5 == OnUtil.On_UNSET_VECTOR)
					{
						val58 = new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert());
						val58.Unitize();
					}
					else
					{
						OnXform val59 = new OnXform();
						val59.Mirror((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
						val58 = new On3dVector((IOn3dVector)(object)val5);
						val58.Transform((IOnXform)(object)val59);
						val58.Unitize();
					}
					On3dVector val60 = new On3dVector((IOn3dVector)(object)cSChannelGemData.LeftVector.Convert());
					val60.Unitize();
					On3dVector val61 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val58, (IOn3dVector)(object)val60);
					val61.Unitize();
					OnPlane val62 = new OnPlane((IOn3dPoint)(object)(obj3 + (IOn3dVector)(object)(val61 * num48) + (IOn3dVector)(object)(val58 * num49)), (IOn3dVector)(object)val60, (IOn3dVector)(object)val61);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd4:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd7:
					{
						OnXform val78 = new OnXform();
						val78.ChangeBasis((IOnPlane)(object)val62, OnUtil.On_xy_plane);
						OnXform val79 = new OnXform();
						val79.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value27), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
						OnXform val80 = val79 * (IOnXform)(object)val78;
						prong7.Transform((IOnXform)(object)val80);
						prongBaseRail7.Transform((IOnXform)(object)val80);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong7).Convert(), val80.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail7).Convert(), val80.Convert(), applyTransform: false));
						break;
					}
					case ValueListItem.ProngPlacerStyleEnd5:
					case ValueListItem.ProngPlacerStyleEnd8:
					case ValueListItem.ProngPlacerStyleEnd9:
					{
						double num50 = value28 / 2.0;
						OnXform val63 = new OnXform();
						val63.ChangeBasis((IOnPlane)(object)val62, OnUtil.On_xy_plane);
						OnXform val64 = new OnXform();
						val64.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num50), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val65 = new OnXform();
						val65.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value27 + num50), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
						OnXform val66 = val65 * (IOnXform)(object)val63 * (IOnXform)(object)val64;
						if (!flag)
						{
							OnGeometry val67 = prong7.Duplicate();
							val67.Transform((IOnXform)(object)val66);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val67).Convert(), val66.Convert(), applyTransform: false));
							OnGeometry val68 = prongBaseRail7.Duplicate();
							val68.Transform((IOnXform)(object)val66);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val68).Convert(), val66.Convert(), applyTransform: false));
						}
						OnXform val69 = new OnXform();
						val69.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num50), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val70 = new OnXform();
						val70.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value27 - num50), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
						OnXform val71 = val70 * (IOnXform)(object)val63 * (IOnXform)(object)val69;
						if (!flag)
						{
							OnGeometry val72 = prong7.Duplicate();
							val72.Transform((IOnXform)(object)val71);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val72).Convert(), val71.Convert(), applyTransform: false));
							OnGeometry val73 = prongBaseRail7.Duplicate();
							val73.Transform((IOnXform)(object)val71);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val73).Convert(), val71.Convert(), applyTransform: false));
						}
						if (flag)
						{
							On3dPoint val74 = new On3dPoint(0.0, 0.0, 0.0);
							On3dPoint val75 = new On3dPoint(0.0, 0.0, 0.0);
							On3dVector val76 = new On3dVector(0.0, 0.0, 1.0);
							On3dVector val77 = new On3dVector(0.0, 0.0, 1.0);
							val74.Transform((IOnXform)(object)val66);
							val76.Transform((IOnXform)(object)val66);
							val75.Transform((IOnXform)(object)val71);
							val77.Transform((IOnXform)(object)val71);
							Brep geometry3 = MakeBar((val74 - (IOn3dPoint)(object)val75).Length(), num46 / 2.0, num46 / 2.0, height9, value10, value32, value35, value38, value41);
							On3dPoint value59 = new On3dPoint((IOn3dPoint)(object)((val74 + (IOn3dPoint)(object)val75) / 2.0));
							On3dVector value60 = val75 - (IOn3dPoint)(object)val74;
							On3dVector value61 = val76 + (IOn3dVector)(object)val77;
							Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value59), CSConvert.On3dVectorCopyToVector3d(value60), CSConvert.On3dVectorCopyToVector3d(value61));
							concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry3, transform2));
						}
						break;
					}
					}
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd10:
				{
					CSChannelGemData channelGemData11 = CSUtilGemRails.GetChannelGemData(cSDataArray, num23);
					CSChannelGemData channelGemData12 = CSUtilGemRails.GetChannelGemData(cSDataArray, num23 + 1);
					if (channelGemData11 == null || channelGemData12 == null)
					{
						return UpdateError("Invalid gems");
					}
					CSDataHelper.SetSettingType(channelGemData12, flag ? GemSettingType.BR : GemSettingType.PR);
					OnCircle circleLeft6;
					OnCircle circleRight6;
					double height8;
					string circles4 = GetCircles(channelGemData11, channelGemData12, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value15, value17, value4, value21, value49, value50, out circleLeft6, out circleRight6, out height8);
					if (!string.IsNullOrEmpty(circles4))
					{
						return UpdateError(circles4);
					}
					On3dVector val46 = new On3dVector(circleLeft6.Center());
					On3dVector val47 = new On3dVector(circleRight6.Center());
					if (value47 == 157)
					{
						if (circleRight6.Diameter() > circleLeft6.Diameter())
						{
							circleLeft6 = new OnCircle(circleLeft6.Center(), circleRight6.radius);
						}
						else
						{
							circleRight6 = new OnCircle(circleRight6.Center(), circleLeft6.radius);
						}
					}
					if (value47 == 158)
					{
						if (circleRight6.Diameter() < circleLeft6.Diameter())
						{
							circleLeft6 = new OnCircle(circleLeft6.Center(), circleRight6.radius);
						}
						else
						{
							circleRight6 = new OnCircle(circleRight6.Center(), circleLeft6.radius);
						}
					}
					if (value47 == 159)
					{
						double radius3 = circleRight6.radius;
						double radius4 = circleLeft6.radius;
						double num45 = (radius3 + radius4) / 2.0;
						circleRight6 = new OnCircle(circleRight6.Center(), num45);
						circleLeft6 = new OnCircle(circleLeft6.Center(), num45);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height8 = (value49 ? (num3 * value50) : num3);
					}
					OnXform val48 = new OnXform();
					val48.ChangeBasis((IOnPlane)(object)circleLeft6.plane, OnUtil.On_xy_plane);
					On3dVector val49 = new On3dVector(OnUtil.On_zaxis);
					val49.Transform((IOnXform)(object)val48);
					val49.Unitize();
					OnXform val50 = new OnXform();
					val50.ChangeBasis((IOnPlane)(object)circleRight6.plane, OnUtil.On_xy_plane);
					On3dVector val51 = new On3dVector(OnUtil.On_zaxis);
					val51.Transform((IOnXform)(object)val50);
					val51.Unitize();
					On3dVector value55 = val49 + (IOn3dVector)(object)val51;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height8 = GetCamHeightEnd(channelGemData11, channelGemData12, circleLeft6, circleRight6, val46, val47);
					}
					if (!flag)
					{
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft6.Diameter(), height8, value7, value10, value, out var prong6, out var prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value56 = val47 - (IOn3dVector)(object)val46;
						Transform val52 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val46)), CSConvert.On3dVectorCopyToVector3d(value56), CSConvert.On3dVectorCopyToVector3d(value55));
						Vector3d val53 = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform val54 = Transform.Mirror(channelGemData11.Center, val53);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val54 * val52));
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight6.Diameter(), height8, value7, value10, value, out prong6, out prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						value56 = val46 - (IOn3dVector)(object)val47;
						val52 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val47)), CSConvert.On3dVectorCopyToVector3d(value56), CSConvert.On3dVectorCopyToVector3d(value55));
						val53 = channelGemData11.Center - channelGemData11.XMaxPoint;
						val54 = Transform.Mirror(channelGemData11.Center, val53);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val54 * val52));
					}
					else
					{
						Brep geometry2 = MakeBar((val46 - (IOn3dVector)(object)val47).Length(), circleLeft6.radius, circleRight6.radius, height8, value10, value32, value35, value38, value41);
						On3dPoint value57 = new On3dPoint((IOn3dVector)(object)((val46 + (IOn3dVector)(object)val47) / 2.0));
						On3dVector value58 = val47 - (IOn3dVector)(object)val46;
						Transform val55 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value57), CSConvert.On3dVectorCopyToVector3d(value58), CSConvert.On3dVectorCopyToVector3d(value55));
						Vector3d val56 = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform val57 = Transform.Mirror(channelGemData11.Center, val56);
						concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry2, val57 * val55));
					}
					break;
				}
				}
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd2:
				case ValueListItem.ProngPlacerStyleEnd3:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					CSChannelGemData cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Duplicate() as CSChannelGemData;
					if (cSDataArray.GetArrayCount() == 1)
					{
						cSChannelGemData2.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData2.Normal, cSChannelGemData2.Center));
					}
					double num52 = cSChannelGemData2.GemXSize * value29 + value16 + value18;
					double height12 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData2.GemZSize + value6 - value23) : channelGemData.GemZSize);
					if (CSApplicationHelper.Application.IsCamMode)
					{
						CSChannelGemData gemCurrent2 = cSChannelGemData2;
						CSChannelGemData gemPrev2 = CSUtilGemRails.GetChannelGemData(cSDataArray, (m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1)).Duplicate() as CSChannelGemData;
						OnCircle circleLeft9;
						OnCircle circleRight9;
						double height13;
						string circles7 = GetCircles(gemPrev2, gemCurrent2, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value16, value18, value6, value23, value49, value52, out circleLeft9, out circleRight9, out height13);
						if (!string.IsNullOrEmpty(circles7))
						{
							return UpdateError(circles7);
						}
						if (value47 == 157)
						{
							if (circleRight9.Diameter() > circleLeft9.Diameter())
							{
								circleLeft9 = new OnCircle(circleLeft9.Center(), circleRight9.radius);
							}
							else
							{
								circleRight9 = new OnCircle(circleRight9.Center(), circleLeft9.radius);
							}
						}
						if (value47 == 158)
						{
							if (circleRight9.Diameter() < circleLeft9.Diameter())
							{
								circleLeft9 = new OnCircle(circleLeft9.Center(), circleRight9.radius);
							}
							else
							{
								circleRight9 = new OnCircle(circleRight9.Center(), circleLeft9.radius);
							}
						}
						if (value47 == 159)
						{
							double radius9 = circleRight9.radius;
							double radius10 = circleLeft9.radius;
							double num53 = (radius9 + radius10) / 2.0;
							circleRight9 = new OnCircle(circleRight9.Center(), num53);
							circleLeft9 = new OnCircle(circleLeft9.Center(), num53);
						}
						On3dVector vLeft2 = new On3dVector(circleLeft9.Center());
						On3dVector vRight2 = new On3dVector(circleRight9.Center());
						height12 = GetCamHeightEnd(gemPrev2, gemCurrent2, circleLeft9, circleRight9, vLeft2, vRight2);
					}
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num52, height12, value9, value12, value, out var prong9, out var prongBaseRail9))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj4 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					double num54 = (cSChannelGemData2.GemXSize * value29 + value16) / 2.0;
					double num55 = value23 - cSChannelGemData2.GemZMin;
					On3dVector val93;
					if (val6 == OnUtil.On_UNSET_VECTOR)
					{
						val93 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert());
						val93.Unitize();
					}
					else
					{
						OnXform val94 = new OnXform();
						val94.Mirror((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
						val93 = new On3dVector((IOn3dVector)(object)val6);
						val93.Transform((IOnXform)(object)val94);
						val93.Unitize();
					}
					On3dVector val95 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.RightVector.Convert());
					val95.Unitize();
					On3dVector val96 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val93, (IOn3dVector)(object)val95);
					val96.Unitize();
					OnPlane val97 = new OnPlane((IOn3dPoint)(object)(obj4 + (IOn3dVector)(object)(val96 * num54) + (IOn3dVector)(object)(val93 * num55)), (IOn3dVector)(object)val95, (IOn3dVector)(object)val96);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd2:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd8:
					{
						OnXform val113 = new OnXform();
						val113.ChangeBasis((IOnPlane)(object)val97, OnUtil.On_xy_plane);
						OnXform val114 = new OnXform();
						val114.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value30), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
						OnXform val115 = val114 * (IOnXform)(object)val113;
						prong9.Transform((IOnXform)(object)val115);
						prongBaseRail9.Transform((IOnXform)(object)val115);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong9).Convert(), val115.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail9).Convert(), val115.Convert(), applyTransform: false));
						break;
					}
					case ValueListItem.ProngPlacerStyleEnd3:
					case ValueListItem.ProngPlacerStyleEnd7:
					case ValueListItem.ProngPlacerStyleEnd9:
					{
						double num56 = value31 / 2.0;
						OnXform val98 = new OnXform();
						val98.ChangeBasis((IOnPlane)(object)val97, OnUtil.On_xy_plane);
						OnXform val99 = new OnXform();
						val99.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num56), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val100 = new OnXform();
						val100.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value30 + num56), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
						OnXform val101 = val100 * (IOnXform)(object)val98 * (IOnXform)(object)val99;
						if (!flag)
						{
							OnGeometry val102 = prong9.Duplicate();
							val102.Transform((IOnXform)(object)val101);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val102).Convert(), val101.Convert(), applyTransform: false));
							OnGeometry val103 = prongBaseRail9.Duplicate();
							val103.Transform((IOnXform)(object)val101);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val103).Convert(), val101.Convert(), applyTransform: false));
						}
						OnXform val104 = new OnXform();
						val104.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num56), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val105 = new OnXform();
						val105.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value30 - num56), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
						OnXform val106 = val105 * (IOnXform)(object)val98 * (IOnXform)(object)val104;
						if (!flag)
						{
							OnGeometry val107 = prong9.Duplicate();
							val107.Transform((IOnXform)(object)val106);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val107).Convert(), val106.Convert(), applyTransform: false));
							OnGeometry val108 = prongBaseRail9.Duplicate();
							val108.Transform((IOnXform)(object)val106);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val108).Convert(), val106.Convert(), applyTransform: false));
						}
						if (flag)
						{
							On3dPoint val109 = new On3dPoint(0.0, 0.0, 0.0);
							On3dPoint val110 = new On3dPoint(0.0, 0.0, 0.0);
							On3dVector val111 = new On3dVector(0.0, 0.0, 1.0);
							On3dVector val112 = new On3dVector(0.0, 0.0, 1.0);
							val109.Transform((IOnXform)(object)val101);
							val111.Transform((IOnXform)(object)val101);
							val110.Transform((IOnXform)(object)val106);
							val112.Transform((IOnXform)(object)val106);
							Brep geometry5 = MakeBar((val109 - (IOn3dPoint)(object)val110).Length(), num52 / 2.0, num52 / 2.0, height12, value12, value34, value37, value40, value43);
							On3dPoint value66 = new On3dPoint((IOn3dPoint)(object)((val109 + (IOn3dPoint)(object)val110) / 2.0));
							On3dVector value67 = val110 - (IOn3dPoint)(object)val109;
							On3dVector value68 = val111 + (IOn3dVector)(object)val112;
							Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value66), CSConvert.On3dVectorCopyToVector3d(value67), CSConvert.On3dVectorCopyToVector3d(value68));
							concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry5, transform3));
						}
						break;
					}
					}
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd10:
				{
					CSChannelGemData channelGemData13 = CSUtilGemRails.GetChannelGemData(cSDataArray, (m - 1 > 0) ? (m - 1) : (m + 1));
					CSChannelGemData channelGemData14 = CSUtilGemRails.GetChannelGemData(cSDataArray, m);
					if (channelGemData13 == null || channelGemData14 == null)
					{
						return UpdateError("Invalid gems");
					}
					CSDataHelper.SetSettingType(channelGemData14, flag ? GemSettingType.BR : GemSettingType.PR);
					OnCircle circleLeft8;
					OnCircle circleRight8;
					double height11;
					string circles6 = GetCircles(channelGemData13, channelGemData14, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value16, value18, value6, value23, value49, value52, out circleLeft8, out circleRight8, out height11);
					if (!string.IsNullOrEmpty(circles6))
					{
						return UpdateError(circles6);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height11 = (value49 ? (num3 * value52) : num3);
					}
					if (value47 == 157)
					{
						if (circleRight8.Diameter() > circleLeft8.Diameter())
						{
							circleLeft8 = new OnCircle(circleLeft8.Center(), circleRight8.radius);
						}
						else
						{
							circleRight8 = new OnCircle(circleRight8.Center(), circleLeft8.radius);
						}
					}
					if (value47 == 158)
					{
						if (circleRight8.Diameter() < circleLeft8.Diameter())
						{
							circleLeft8 = new OnCircle(circleLeft8.Center(), circleRight8.radius);
						}
						else
						{
							circleRight8 = new OnCircle(circleRight8.Center(), circleLeft8.radius);
						}
					}
					if (value47 == 159)
					{
						double radius7 = circleRight8.radius;
						double radius8 = circleLeft8.radius;
						double num51 = (radius7 + radius8) / 2.0;
						circleRight8 = new OnCircle(circleRight8.Center(), num51);
						circleLeft8 = new OnCircle(circleLeft8.Center(), num51);
					}
					On3dVector val81 = new On3dVector(circleLeft8.Center());
					On3dVector val82 = new On3dVector(circleRight8.Center());
					OnXform val83 = new OnXform();
					val83.ChangeBasis((IOnPlane)(object)circleLeft8.plane, OnUtil.On_xy_plane);
					On3dVector val84 = new On3dVector(OnUtil.On_zaxis);
					val84.Transform((IOnXform)(object)val83);
					val84.Unitize();
					OnXform val85 = new OnXform();
					val85.ChangeBasis((IOnPlane)(object)circleRight8.plane, OnUtil.On_xy_plane);
					On3dVector val86 = new On3dVector(OnUtil.On_zaxis);
					val86.Transform((IOnXform)(object)val85);
					val86.Unitize();
					On3dVector value62 = val84 + (IOn3dVector)(object)val86;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height11 = GetCamHeightEnd(channelGemData13, channelGemData14, circleLeft8, circleRight8, val81, val82);
					}
					if (!flag)
					{
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft8.Diameter(), height11, value9, value12, value, out var prong8, out var prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value63 = val82 - (IOn3dVector)(object)val81;
						Transform val87 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val81)), CSConvert.On3dVectorCopyToVector3d(value63), CSConvert.On3dVectorCopyToVector3d(value62));
						Vector3d val88 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform val89 = Transform.Mirror(channelGemData14.Center, val88);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), val89 * val87));
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight8.Diameter(), height11, value9, value12, value, out prong8, out prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						value63 = val81 - (IOn3dVector)(object)val82;
						val87 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val82)), CSConvert.On3dVectorCopyToVector3d(value63), CSConvert.On3dVectorCopyToVector3d(value62));
						val88 = channelGemData14.Center - channelGemData14.XMaxPoint;
						val89 = Transform.Mirror(channelGemData14.Center, val88);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), val89 * val87));
					}
					else
					{
						Brep geometry4 = MakeBar((val81 - (IOn3dVector)(object)val82).Length(), circleLeft8.radius, circleRight8.radius, height11, value12, value34, value37, value40, value43);
						On3dPoint value64 = new On3dPoint((IOn3dVector)(object)((val81 + (IOn3dVector)(object)val82) / 2.0));
						On3dVector value65 = val82 - (IOn3dVector)(object)val81;
						Transform val90 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value64), CSConvert.On3dVectorCopyToVector3d(value65), CSConvert.On3dVectorCopyToVector3d(value62));
						Vector3d val91 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform val92 = Transform.Mirror(channelGemData14.Center, val91);
						concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry4, val92 * val90));
					}
					break;
				}
				}
			}
		}
		OutProngs.SetData(new CSDataArray(concurrentBag));
		OutProfiles.SetData(new CSDataArray(concurrentBag2));
		if (value48)
		{
			CSApplicationHelper.Application.IsCamMode = isCamMode;
		}
		return base.Update();
	}

	private static ControlPoint BendZ(double x, double y, double z, double w, double maxX, double height, double per)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		double num;
		if (z > height / 2.0)
		{
			num = z;
		}
		else
		{
			double num2 = Math.Pow(x / maxX, 2.0) * per;
			double num3 = height / 2.0;
			double num4 = (height - z) / height;
			num = z + num3 * num2 * num4;
		}
		return new ControlPoint(x, y, num, w);
	}

	private static NurbsCurve BuildArcEnd(Point3d start, Point3d corner, Point3d mid, double endBlendPer)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Expected O, but got Unknown
		if (endBlendPer < 0.01)
		{
			endBlendPer = 0.01;
		}
		if (endBlendPer > 0.99)
		{
			endBlendPer = 0.99;
		}
		Vector3d val = corner - start;
		Vector3d val2 = corner - mid;
		NurbsCurve val3 = new NurbsCurve(3, true, 3, 7);
		double num = 0.7071067811;
		val3.Points[6] = new ControlPoint(start, 1.0);
		val3.Points[5] = new ControlPoint(start + val * (1.0 - endBlendPer) / 2.0, 1.0);
		val3.Points[4] = new ControlPoint(start + val * (1.0 - endBlendPer), 1.0);
		val3.Points[3] = new ControlPoint(corner, num);
		val3.Points[2] = new ControlPoint(mid + val2 * (1.0 - endBlendPer), 1.0);
		val3.Points[1] = new ControlPoint(mid + val2 * (1.0 - endBlendPer) / 2.0, 1.0);
		val3.Points[0] = new ControlPoint(mid, 1.0);
		val3.Knots[0] = 0.0;
		val3.Knots[1] = 0.0;
		val3.Knots[2] = 0.2;
		val3.Knots[3] = 0.2;
		val3.Knots[4] = 0.4;
		val3.Knots[5] = 0.4;
		val3.Knots[6] = 0.6;
		val3.Knots[7] = 0.6;
		return val3;
	}

	private NurbsCurve BuildProngProfile(double radius, double height, double radiusTopDome, double radiusBottomDome, double taperPer)
	{
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		double num = radius * (1.0 + taperPer);
		if (radiusTopDome < 0.01)
		{
			radiusTopDome = 0.01;
		}
		if (radiusBottomDome < 0.01)
		{
			radiusBottomDome = 0.01;
		}
		if (radiusTopDome > radius - 0.01)
		{
			radiusTopDome = radius - 0.01;
		}
		if (radiusBottomDome > num - 0.01)
		{
			radiusBottomDome = num - 0.01;
		}
		if (radiusTopDome + radiusBottomDome > height)
		{
			height = radiusTopDome + radiusBottomDome + 0.01;
		}
		double num2 = height - radiusTopDome - radiusBottomDome;
		double num3 = (num - radius) / num2;
		double num4 = num + num3 * radiusBottomDome;
		double num5 = radius - num3 * radiusTopDome;
		double num6 = 0.7071067811;
		NurbsCurve val = new NurbsCurve(3, true, 3, 11);
		val.Points[0] = new ControlPoint(0.0, 0.0, 0.0, 1.0);
		val.Points[1] = new ControlPoint((num - radiusBottomDome) / 2.0, 0.0, 0.0, 1.0);
		val.Points[2] = new ControlPoint(num - radiusBottomDome, 0.0, 0.0, 1.0);
		val.Points[3] = new ControlPoint(num4, 0.0, 0.0, num6);
		val.Points[4] = new ControlPoint(num, 0.0, radiusBottomDome, 1.0);
		val.Points[5] = new ControlPoint((num + radius) / 2.0, 0.0, radiusBottomDome + num2 / 2.0, 1.0);
		val.Points[6] = new ControlPoint(radius, 0.0, radiusBottomDome + num2, 1.0);
		val.Points[7] = new ControlPoint(num5, 0.0, height, num6);
		val.Points[8] = new ControlPoint(radius - radiusTopDome, 0.0, height, 1.0);
		val.Points[9] = new ControlPoint((radius - radiusTopDome) / 2.0, 0.0, height, 1.0);
		val.Points[10] = new ControlPoint(0.0, 0.0, height, 1.0);
		val.Knots[0] = 0.0;
		val.Knots[1] = 0.0;
		val.Knots[2] = 0.2;
		val.Knots[3] = 0.2;
		val.Knots[4] = 0.4;
		val.Knots[5] = 0.4;
		val.Knots[6] = 0.6;
		val.Knots[7] = 0.6;
		val.Knots[8] = 0.8;
		val.Knots[9] = 0.8;
		val.Knots[10] = 1.0;
		val.Knots[11] = 1.0;
		return val;
	}

	private static double GetCamHeightEnd(CSGemData gemPrev, CSGemData gemCurrent, OnCircle circleLeft, OnCircle circleRight, On3dVector vLeft, On3dVector vRight)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		double num = 0.0;
		On3dPoint value = new On3dPoint((IOn3dVector)(object)((vLeft + (IOn3dVector)(object)vRight) / 2.0));
		On3dVector value2 = vRight - (IOn3dVector)(object)vLeft;
		OnXform val = new OnXform();
		val.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
		Vector3d zAxis = Vector3d.ZAxis;
		((Vector3d)(ref zAxis)).Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val));
		((Vector3d)(ref zAxis)).Unitize();
		OnXform val2 = new OnXform();
		val2.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
		Vector3d zAxis2 = Vector3d.ZAxis;
		((Vector3d)(ref zAxis2)).Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val2));
		((Vector3d)(ref zAxis2)).Unitize();
		Vector3d destination = zAxis + zAxis2;
		((Vector3d)(ref destination)).Unitize();
		Transform val3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value), CSConvert.On3dVectorCopyToVector3d(value2), destination);
		Point3d origin = Point3d.Origin;
		((Point3d)(ref origin)).Transform(val3);
		Line val4 = default(Line);
		((Line)(ref val4))._002Ector(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
		((Line)(ref val4)).Transform(val3);
		Curve obj = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)new List<Point3d> { gemPrev.ZMaxPoint, gemCurrent.ZMaxPoint }, 3);
		Curve val5 = obj.DuplicateCurve();
		Curve val6 = obj.DuplicateCurve();
		Vector3d val7 = CSConvert.On3dVectorCopyToVector3d(circleLeft.plane.yaxis);
		((GeometryBase)val5).Transform(Transform.Translation(4.0 * val7));
		((GeometryBase)val6).Transform(Transform.Translation(-4.0 * val7));
		NurbsSurface obj2 = NurbsSurface.CreateRuledSurface(val5, val6);
		double num2 = default(double);
		double num3 = default(double);
		((Surface)obj2).ClosestPoint(((Line)(ref val4)).PointAt(1.0), ref num2, ref num3);
		Point3d val8 = ((Surface)obj2).PointAt(num2, num3);
		if (((Point3d)(ref val8)).DistanceTo(origin) > num * 0.8)
		{
			num = ((Point3d)(ref val8)).DistanceTo(origin) * 1.2;
		}
		return num;
	}

	private string GetCircles(CSChannelGemData gemPrev, CSChannelGemData gemCurrent, CSChannelGemData gemMax, IOnCurve railLeft, IOnCurve railRight, double vNudgeEdge, double vNudgeGem, double vHeight, double vVerticalLocation, bool usePercent, double percent, out OnCircle circleLeft, out OnCircle circleRight, out double height)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_028b: Expected O, but got Unknown
		circleLeft = null;
		circleRight = null;
		height = 0.0;
		if (gemPrev == null || gemCurrent == null)
		{
			return "Invalid gems";
		}
		On3dPoint val = (new On3dPoint((IOn3dPoint)(object)gemCurrent.Center.Convert()) + (IOn3dPoint)new On3dPoint((IOn3dPoint)(object)gemPrev.Center.Convert())) / 2.0;
		if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(gemPrev, gemCurrent, railLeft, vNudgeEdge, out var circleEdgeLeft))
		{
			return "CalculateCircleEdgeLeft failed";
		}
		if (!CSUtilProngPlacer.CalculateCircleEdgeRight(gemPrev, gemCurrent, railRight, vNudgeEdge, out var circleEdgeRight))
		{
			return "CalculateCircleEdgeRight failed";
		}
		OnCircle val2 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)gemPrev.Center.Convert(), (IOn3dVector)(object)gemPrev.Normal.Convert()), gemPrev.SizeAverage - vNudgeGem);
		OnCircle val3 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)gemCurrent.Center.Convert(), (IOn3dVector)(object)gemCurrent.Normal.Convert()), gemCurrent.SizeAverage - vNudgeGem);
		circleLeft = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)(object)val2, (IOnCircle)(object)val3, (IOnCircle)(object)circleEdgeLeft);
		circleRight = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)(object)val2, (IOnCircle)(object)val3, (IOnCircle)(object)circleEdgeRight);
		On3dPoint obj = val2.ClosestPointTo(val3.Center());
		On3dPoint val4 = val3.ClosestPointTo(val2.Center());
		OnLineCurve val5 = new OnLineCurve((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val4);
		OnLineCurve val6 = new OnLineCurve(val2.Center(), val3.Center());
		On3dPoint val7 = ((OnCurve)val5).PointAt(((OnCurve)val5).Domain().Mid());
		double num = -1.23432101234321E+308;
		if (!((OnCurve)val6).GetClosestPoint((IOn3dPoint)(object)val7, ref num))
		{
			return "GetClosestPoint failed";
		}
		On3dPoint val8 = ((OnCurve)val6).PointAt(num);
		double num2 = val7.DistanceTo((IOn3dPoint)(object)val8);
		double num3 = ((_prongBase == ValueListItem.FollowLargestGem) ? (gemMax.GemZMin + (gemMax.Size - gemCurrent.Size) / 2.0) : ((gemPrev.GemZMin + gemCurrent.GemZMin) / 2.0));
		double num4 = ((_prongBase == ValueListItem.FollowLargestGem) ? gemMax.GemZMax : ((gemPrev.GemZMax + gemCurrent.GemZMax) / 2.0));
		double num5 = ((_prongBase == ValueListItem.FollowLargestGem) ? gemMax.GemZSize : ((gemPrev.GemZSize + gemCurrent.GemZSize) / 2.0));
		if (usePercent)
		{
			num3 = ((!(percent < 0.2)) ? ((0.0 - num4) * (1.0 - percent) + num3 * percent) : ((0.0 - num4) * 0.8 + num3 * 0.2));
			num5 *= percent;
		}
		height = num5 + num2 + vHeight - vVerticalLocation;
		On3dVector val9 = new On3dVector((IOn3dVector)(object)gemCurrent.Normal.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)gemPrev.Normal.Convert());
		val9.Unitize();
		double num6 = 0.0;
		circleLeft.ClosestPointTo((IOn3dPoint)(object)val, ref num6);
		circleLeft.Rotate(num6 + OnUtil.On_PI / 2.0, circleLeft.Normal());
		circleLeft.Translate((IOn3dVector)(object)(val9 * (vVerticalLocation - num3)));
		double num7 = 0.0;
		circleRight.ClosestPointTo((IOn3dPoint)(object)val, ref num7);
		circleRight.Rotate(num7 + OnUtil.On_PI / 2.0, circleRight.Normal());
		circleRight.Translate((IOn3dVector)(object)(val9 * (vVerticalLocation - num3)));
		return null;
	}

	private string GetHeight(CSChannelGemData gemPrev, CSChannelGemData gemCurrent, CSChannelGemData gemMax, IOnCurve railLeft, IOnCurve railRight, double vNudgeEdge, double vNudgeGem, double vHeight, double vVerticalLocation, out double height)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		height = 0.0;
		if (gemPrev == null || gemCurrent == null)
		{
			return "Invalid gems";
		}
		if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(gemPrev, gemCurrent, railLeft, vNudgeEdge, out var _))
		{
			return "CalculateCircleEdgeLeft failed";
		}
		if (!CSUtilProngPlacer.CalculateCircleEdgeRight(gemPrev, gemCurrent, railRight, vNudgeEdge, out var _))
		{
			return "CalculateCircleEdgeRight failed";
		}
		OnCircle val = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)gemPrev.Center.Convert(), (IOn3dVector)(object)gemPrev.Normal.Convert()), gemPrev.SizeAverage - vNudgeGem);
		OnCircle val2 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)gemCurrent.Center.Convert(), (IOn3dVector)(object)gemCurrent.Normal.Convert()), gemCurrent.SizeAverage - vNudgeGem);
		On3dPoint obj = val.ClosestPointTo(val2.Center());
		On3dPoint val3 = val2.ClosestPointTo(val.Center());
		OnLineCurve val4 = new OnLineCurve((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val3);
		OnLineCurve val5 = new OnLineCurve(val.Center(), val2.Center());
		On3dPoint val6 = ((OnCurve)val4).PointAt(((OnCurve)val4).Domain().Mid());
		double num = -1.23432101234321E+308;
		if (!((OnCurve)val5).GetClosestPoint((IOn3dPoint)(object)val6, ref num))
		{
			return "GetClosestPoint failed";
		}
		On3dPoint val7 = ((OnCurve)val5).PointAt(num);
		double num2 = val6.DistanceTo((IOn3dPoint)(object)val7);
		double gemZSize = gemMax.GemZSize;
		height = gemZSize + num2 + vHeight - vVerticalLocation;
		return null;
	}

	private Brep MakeBar(double centerLength, double radius1, double radius2, double height, double filletPer, double domeRadius, double baseDomeRadius, double blendEdge, double bottomArcPercent)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector((0.0 - centerLength) / 2.0, 0.0, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(centerLength / 2.0, 0.0, 0.0);
		double num = Math.Asin((radius1 - radius2) / centerLength);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector((0.0 - centerLength) / 2.0, radius1, 0.0);
		Transform val4 = CSBlockUtil.CSUtilMath.Rotation(num, -Vector3d.ZAxis, val);
		((Point3d)(ref val3)).Transform(val4);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(centerLength / 2.0, radius2, 0.0);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(num, -Vector3d.ZAxis, val2);
		((Point3d)(ref val5)).Transform(val6);
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(val3);
		((Point3d)(ref val7)).Y = 0.0 - ((Point3d)(ref val7)).Y;
		Point3d val8 = default(Point3d);
		((Point3d)(ref val8))._002Ector(val5);
		((Point3d)(ref val8)).Y = 0.0 - ((Point3d)(ref val8)).Y;
		Point3d mid = default(Point3d);
		((Point3d)(ref mid))._002Ector((0.0 - centerLength) / 2.0 - radius1, 0.0, 0.0);
		Point3d mid2 = default(Point3d);
		((Point3d)(ref mid2))._002Ector(centerLength / 2.0 + radius2, 0.0, 0.0);
		Vector3d val9 = val3 - val5;
		((Vector3d)(ref val9)).Unitize();
		double num2 = (((Point3d)(ref val3)).X - ((Point3d)(ref mid)).X) / Math.Cos(num);
		Point3d corner = val3 + val9 * num2;
		double num3 = (((Point3d)(ref val5)).X - ((Point3d)(ref mid2)).X) / Math.Cos(num);
		Point3d corner2 = val5 + val9 * num3;
		NurbsCurve rail = BuildArcEnd(val3, corner, mid, blendEdge);
		NurbsCurve rail2 = BuildArcEnd(val5, corner2, mid2, blendEdge);
		ProngRevolve(BuildProngProfile(radius1, height, domeRadius, baseDomeRadius, filletPer), profile1: BuildProngProfile(radius2, height, domeRadius, baseDomeRadius, filletPer), rail0: rail, center0: val, radius0: radius1, rail1: rail2, center1: val2, radius1: radius2, maxX: Math.Max(centerLength / 2.0 + radius1, centerLength / 2.0 + radius2), height: height, per: bottomArcPercent, srf0: out var srf, srf1: out var srf2, srf2: out var srf3);
		Transform val10 = Transform.Mirror(Point3d.Origin, Vector3d.YAxis);
		NurbsSurface val11 = new NurbsSurface(srf);
		NurbsSurface val12 = new NurbsSurface(srf2);
		NurbsSurface val13 = new NurbsSurface(srf3);
		((GeometryBase)val11).Transform(val10);
		((GeometryBase)val12).Transform(val10);
		((GeometryBase)val13).Transform(val10);
		Brep[] array = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep>
		{
			((Surface)srf).ToBrep(),
			((Surface)srf2).ToBrep(),
			((Surface)srf3).ToBrep(),
			((Surface)val11).ToBrep(),
			((Surface)val12).ToBrep(),
			((Surface)val13).ToBrep()
		}, 0.01);
		if (array.Length != 1)
		{
			return null;
		}
		return array[0];
	}

	private static void ProngRevolve(NurbsCurve profile0, NurbsCurve rail0, Point3d center0, double radius0, NurbsCurve profile1, NurbsCurve rail1, Point3d center1, double radius1, double maxX, double height, double per, out NurbsSurface srf0, out NurbsSurface srf1, out NurbsSurface srf2)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		srf0 = NurbsSurface.Create(3, true, profile0.Order, rail0.Order, profile0.Points.Count, rail0.Points.Count);
		srf1 = NurbsSurface.Create(3, true, profile0.Order, rail0.Order, profile0.Points.Count, 5);
		srf2 = NurbsSurface.Create(3, true, profile0.Order, rail0.Order, profile0.Points.Count, rail1.Points.Count);
		int count = rail0.Points.Count;
		int count2 = profile0.Points.Count;
		int count3 = rail1.Points.Count;
		int count4 = profile1.Points.Count;
		if (count2 != count4)
		{
			return;
		}
		Point3d val2 = default(Point3d);
		for (int i = 0; i < count2; i++)
		{
			ControlPoint val = profile0.Points[i];
			Point3d location = ((ControlPoint)(ref val)).Location;
			((Point3d)(ref val2))._002Ector(location);
			((Point3d)(ref val2)).Z = 0.0;
			Point3d val3 = val2;
			double num = ((Point3d)(ref val3)).DistanceTo(Point3d.Origin);
			val = profile1.Points[i];
			Point3d location2 = ((ControlPoint)(ref val)).Location;
			((Point3d)(ref val2))._002Ector(location2);
			((Point3d)(ref val2)).Z = 0.0;
			Point3d val4 = val2;
			double num2 = ((Point3d)(ref val4)).DistanceTo(Point3d.Origin);
			for (int j = 0; j < count; j++)
			{
				val = rail0.Points[j];
				Vector3d val5 = ((ControlPoint)(ref val)).Location - center0;
				Point3d val6 = center0 + val5 * num / radius0;
				((Point3d)(ref val6)).Z = ((Point3d)(ref location)).Z;
				val = profile0.Points[i];
				double weight = ((ControlPoint)(ref val)).Weight;
				val = rail0.Points[j];
				double w = weight * ((ControlPoint)(ref val)).Weight;
				srf0.Points.SetControlPoint(i, j, BendZ(((Point3d)(ref val6)).X, ((Point3d)(ref val6)).Y, ((Point3d)(ref val6)).Z, w, maxX, height, per));
			}
			for (int k = 0; k < 5; k++)
			{
				val = rail0.Points[count - 1];
				Vector3d val7 = ((ControlPoint)(ref val)).Location - center0;
				Point3d val8 = center0 + val7 * num / radius0;
				((Point3d)(ref val8)).Z = ((Point3d)(ref location)).Z;
				val = rail1.Points[count3 - 1];
				Vector3d val9 = ((ControlPoint)(ref val)).Location - center1;
				Point3d val10 = center1 + val9 * num2 / radius1;
				((Point3d)(ref val10)).Z = ((Point3d)(ref location2)).Z;
				val = profile0.Points[i];
				double weight2 = ((ControlPoint)(ref val)).Weight;
				Point3d val11 = val8 * (double)(4 - k) / 4.0 + val10 * (double)k / 4.0;
				srf1.Points.SetControlPoint(i, k, BendZ(((Point3d)(ref val11)).X, ((Point3d)(ref val11)).Y, ((Point3d)(ref val11)).Z, weight2, maxX, height, per));
			}
			for (int l = 0; l < count3; l++)
			{
				val = rail1.Points[l];
				Vector3d val12 = ((ControlPoint)(ref val)).Location - center1;
				Point3d val13 = center1 + val12 * num2 / radius1;
				((Point3d)(ref val13)).Z = ((Point3d)(ref location2)).Z;
				val = profile1.Points[i];
				double weight3 = ((ControlPoint)(ref val)).Weight;
				val = rail1.Points[l];
				double w2 = weight3 * ((ControlPoint)(ref val)).Weight;
				srf2.Points.SetControlPoint(i, l, BendZ(((Point3d)(ref val13)).X, ((Point3d)(ref val13)).Y, ((Point3d)(ref val13)).Z, w2, maxX, height, per));
			}
		}
		for (int m = 0; m < profile0.Knots.Count; m++)
		{
			srf0.KnotsU[m] = profile0.Knots[m];
			srf1.KnotsU[m] = profile0.Knots[m];
			srf2.KnotsU[m] = profile0.Knots[m];
		}
		for (int n = 0; n < rail0.Knots.Count; n++)
		{
			srf0.KnotsV[n] = rail0.Knots[n];
		}
		srf1.KnotsV.CreateUniformKnots(1.0);
		for (int num3 = 0; num3 < rail1.Knots.Count; num3++)
		{
			srf2.KnotsV[num3] = rail1.Knots[num3];
		}
	}
}
