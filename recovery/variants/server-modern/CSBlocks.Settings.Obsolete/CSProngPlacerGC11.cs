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
[CSBlock("ProngPlacerGC_V11", "{1C511466-5DCD-450D-92AF-244C26D2CCF8}")]
public sealed class CSProngPlacerGC11 : CSBlock
{
	private readonly CSProngCache _prongCache = new CSProngCache();

	private ValueListItem _prongBase = ValueListItem.FollowCulet;

	public CSInputChannel InStartBlendBot { get; }

	public CSInputChannel InMiddleBlendBot { get; }

	public CSInputChannel InEndBlendBot { get; }

	public CSInputChannel InStartBlendEdge { get; }

	public CSInputChannel InMiddleBlendEdge { get; }

	public CSInputChannel InEndBlendEdge { get; }

	public CSInputChannel InStartBlendTop { get; }

	public CSInputChannel InMiddleBlendTop { get; }

	public CSInputChannel InEndBlendTop { get; }

	public CSInputChannel InStartBottomArcPercentage { get; }

	public CSInputChannel InMiddleBottomArcPercentage { get; }

	public CSInputChannel InEndBottomArcPercentage { get; }

	public CSInputChannel InCornerNudgeEdge { get; }

	public CSInputChannel InCornerNudgeGem { get; }

	public CSInputChannel InCornerProngAngle { get; }

	public CSInputChannel InCornerProngSize { get; }

	public CSInputChannel InCornerProngSizeOffset { get; }

	public CSInputChannel InDoMeshedProngs { get; }

	public CSInputChannel InEnableEndProngEndControls { get; }

	public CSInputChannel InEnableEndProngRotationMirroring { get; }

	public CSInputChannel InStartNudgeEdge { get; }

	public CSInputChannel InEndNudgeEdge { get; }

	public CSInputChannel InStartNudgeGem { get; }

	public CSInputChannel InEndNudgeGem { get; }

	public CSInputChannel InEndProngStyle { get; }

	public CSInputChannel InStartFillet { get; }

	public CSInputChannel InMiddleFillet { get; }

	public CSInputChannel InEndFillet { get; }

	public CSInputChannel InStartHeight { get; }

	public CSInputChannel InMiddleHeight { get; }

	public CSInputChannel InEndHeight { get; }

	public CSInputChannel InMaxGem { get; }

	public CSInputChannel InNudgeEdge { get; }

	public CSInputChannel InNudgeGem { get; }

	public CSInputChannel InPercentToggle { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InStartProngPercentage { get; }

	public CSInputChannel InMiddleProngPercentage { get; }

	public CSInputChannel InEndProngPercentage { get; }

	public CSInputChannel InProngSizeControl { get; }

	public CSInputChannel InRotationEnd { get; }

	public CSInputChannel InRotationStart { get; }

	public CSInputChannel InSettingStyle { get; }

	public CSInputChannel InSizeMax { get; }

	public CSInputChannel InSizeMin { get; }

	public CSInputChannel InSizePercentageEnd { get; }

	public CSInputChannel InSizePercentageStart { get; }

	public CSInputChannel InSortedGems { get; }

	public CSInputChannel InSpreadEnd { get; }

	public CSInputChannel InSpreadStart { get; }

	public CSInputChannel InStartTaper { get; }

	public CSInputChannel InMiddleTaper { get; }

	public CSInputChannel InEndTaper { get; }

	public CSInputChannel InStartVerticalLocation { get; }

	public CSInputChannel InMiddleVerticalLocation { get; }

	public CSInputChannel InEndVerticalLocation { get; }

	public CSOutputChannel OutProfiles { get; }

	public CSOutputChannel OutProngs { get; }

	public CSProngPlacerGC11(string identifier)
		: base(identifier)
	{
		InSortedGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InSizeMin = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSizeMax = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 100);
		InStartHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMiddleHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InStartFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMiddleFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InStartTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMiddleTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InNudgeEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InNudgeGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InStartVerticalLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMiddleVerticalLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndVerticalLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndProngStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.ProngPlacerStyleEnd9);
		InEnableEndProngRotationMirroring = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InEnableEndProngEndControls = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InSizePercentageStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 25);
		InRotationStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpreadStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 45);
		InSizePercentageEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 25);
		InRotationEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpreadEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 45);
		InDoMeshedProngs = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InSettingStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.ProngPlacerSettingStyleProng);
		InMaxGem = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.FollowCulet);
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
		InStartBottomArcPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMiddleBottomArcPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndBottomArcPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngSizeControl = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.NormalProng);
		InCornerProngAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerProngSize = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.AverageProng);
		InCornerProngSizeOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndNudgeEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InStartNudgeEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InStartNudgeGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndNudgeGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerNudgeEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerNudgeGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProfiles = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_1859: Unknown result type (might be due to invalid IL or missing references)
		//IL_185e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1869: Unknown result type (might be due to invalid IL or missing references)
		//IL_1830: Unknown result type (might be due to invalid IL or missing references)
		//IL_1835: Unknown result type (might be due to invalid IL or missing references)
		//IL_1843: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Expected O, but got Unknown
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Expected O, but got Unknown
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Expected O, but got Unknown
		//IL_2427: Unknown result type (might be due to invalid IL or missing references)
		//IL_242e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2433: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18df: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Expected O, but got Unknown
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a14: Expected O, but got Unknown
		//IL_0a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1900: Unknown result type (might be due to invalid IL or missing references)
		//IL_1907: Unknown result type (might be due to invalid IL or missing references)
		//IL_190c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1911: Unknown result type (might be due to invalid IL or missing references)
		//IL_1034: Unknown result type (might be due to invalid IL or missing references)
		//IL_103b: Expected O, but got Unknown
		//IL_1054: Unknown result type (might be due to invalid IL or missing references)
		//IL_105b: Expected O, but got Unknown
		//IL_106c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1073: Expected O, but got Unknown
		//IL_108c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1093: Expected O, but got Unknown
		//IL_0bb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e93: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1933: Unknown result type (might be due to invalid IL or missing references)
		//IL_1935: Unknown result type (might be due to invalid IL or missing references)
		//IL_1921: Unknown result type (might be due to invalid IL or missing references)
		//IL_192c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1931: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ebf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec4: Unknown result type (might be due to invalid IL or missing references)
		//IL_194d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1960: Unknown result type (might be due to invalid IL or missing references)
		//IL_1965: Unknown result type (might be due to invalid IL or missing references)
		//IL_1967: Unknown result type (might be due to invalid IL or missing references)
		//IL_197a: Unknown result type (might be due to invalid IL or missing references)
		//IL_197f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1981: Unknown result type (might be due to invalid IL or missing references)
		//IL_1983: Unknown result type (might be due to invalid IL or missing references)
		//IL_1985: Unknown result type (might be due to invalid IL or missing references)
		//IL_198a: Unknown result type (might be due to invalid IL or missing references)
		//IL_198e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1993: Unknown result type (might be due to invalid IL or missing references)
		//IL_1995: Unknown result type (might be due to invalid IL or missing references)
		//IL_199a: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e9: Expected O, but got Unknown
		//IL_10cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d2: Expected O, but got Unknown
		//IL_0c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_260f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2619: Unknown result type (might be due to invalid IL or missing references)
		//IL_2620: Unknown result type (might be due to invalid IL or missing references)
		//IL_262a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2634: Expected O, but got Unknown
		//IL_2634: Expected O, but got Unknown
		//IL_1ee0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1efa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f18: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f21: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f36: Unknown result type (might be due to invalid IL or missing references)
		//IL_115f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1166: Expected O, but got Unknown
		//IL_116f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1176: Expected O, but got Unknown
		//IL_1127: Unknown result type (might be due to invalid IL or missing references)
		//IL_112e: Expected O, but got Unknown
		//IL_1110: Unknown result type (might be due to invalid IL or missing references)
		//IL_1117: Expected O, but got Unknown
		//IL_2684: Unknown result type (might be due to invalid IL or missing references)
		//IL_268b: Expected O, but got Unknown
		//IL_2693: Unknown result type (might be due to invalid IL or missing references)
		//IL_269f: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b7: Expected O, but got Unknown
		//IL_2669: Unknown result type (might be due to invalid IL or missing references)
		//IL_2673: Unknown result type (might be due to invalid IL or missing references)
		//IL_267a: Expected O, but got Unknown
		//IL_26cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_26d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_26dd: Expected O, but got Unknown
		//IL_2718: Unknown result type (might be due to invalid IL or missing references)
		//IL_271f: Expected O, but got Unknown
		//IL_2746: Unknown result type (might be due to invalid IL or missing references)
		//IL_274f: Expected O, but got Unknown
		//IL_2764: Unknown result type (might be due to invalid IL or missing references)
		//IL_2769: Unknown result type (might be due to invalid IL or missing references)
		//IL_2773: Unknown result type (might be due to invalid IL or missing references)
		//IL_277f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2799: Expected O, but got Unknown
		//IL_27ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2810: Unknown result type (might be due to invalid IL or missing references)
		//IL_2819: Expected O, but got Unknown
		//IL_282e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2837: Expected O, but got Unknown
		//IL_2863: Unknown result type (might be due to invalid IL or missing references)
		//IL_2868: Unknown result type (might be due to invalid IL or missing references)
		//IL_2879: Unknown result type (might be due to invalid IL or missing references)
		//IL_2885: Unknown result type (might be due to invalid IL or missing references)
		//IL_289f: Expected O, but got Unknown
		//IL_32b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1523: Unknown result type (might be due to invalid IL or missing references)
		//IL_152a: Expected O, but got Unknown
		//IL_1531: Unknown result type (might be due to invalid IL or missing references)
		//IL_1538: Expected O, but got Unknown
		//IL_293b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2944: Expected O, but got Unknown
		//IL_2970: Unknown result type (might be due to invalid IL or missing references)
		//IL_2975: Unknown result type (might be due to invalid IL or missing references)
		//IL_2986: Unknown result type (might be due to invalid IL or missing references)
		//IL_2992: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ac: Expected O, but got Unknown
		//IL_28e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_292b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2521: Unknown result type (might be due to invalid IL or missing references)
		//IL_2528: Expected O, but got Unknown
		//IL_250a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2511: Expected O, but got Unknown
		//IL_29f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_25bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c3: Expected O, but got Unknown
		//IL_25ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d1: Expected O, but got Unknown
		//IL_259e: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a5: Expected O, but got Unknown
		//IL_25ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b5: Expected O, but got Unknown
		//IL_2566: Unknown result type (might be due to invalid IL or missing references)
		//IL_256d: Expected O, but got Unknown
		//IL_254f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2556: Expected O, but got Unknown
		//IL_1764: Unknown result type (might be due to invalid IL or missing references)
		//IL_176b: Expected O, but got Unknown
		//IL_1776: Unknown result type (might be due to invalid IL or missing references)
		//IL_177b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1780: Unknown result type (might be due to invalid IL or missing references)
		//IL_1787: Unknown result type (might be due to invalid IL or missing references)
		//IL_178e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Unknown result type (might be due to invalid IL or missing references)
		//IL_179a: Unknown result type (might be due to invalid IL or missing references)
		//IL_179f: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e58: Expected O, but got Unknown
		//IL_0e53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5a: Expected O, but got Unknown
		//IL_0e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e61: Expected O, but got Unknown
		//IL_2a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a73: Expected O, but got Unknown
		//IL_2a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a99: Expected O, but got Unknown
		//IL_2ab6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2adf: Expected O, but got Unknown
		//IL_2af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b8e: Expected O, but got Unknown
		//IL_2bb2: Expected O, but got Unknown
		//IL_2bb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2be3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2be8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd2: Expected O, but got Unknown
		//IL_2cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce8: Expected O, but got Unknown
		//IL_1bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c36: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c62: Expected O, but got Unknown
		//IL_1c5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c64: Expected O, but got Unknown
		//IL_1c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6b: Expected O, but got Unknown
		//IL_1311: Unknown result type (might be due to invalid IL or missing references)
		//IL_132c: Unknown result type (might be due to invalid IL or missing references)
		//IL_358b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3595: Unknown result type (might be due to invalid IL or missing references)
		//IL_35a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_35aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b4: Expected O, but got Unknown
		//IL_35b4: Expected O, but got Unknown
		//IL_218b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2190: Unknown result type (might be due to invalid IL or missing references)
		//IL_2196: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21be: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_21dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f5: Expected O, but got Unknown
		//IL_21f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f7: Expected O, but got Unknown
		//IL_21f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21fe: Expected O, but got Unknown
		//IL_14d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3620: Unknown result type (might be due to invalid IL or missing references)
		//IL_3629: Expected O, but got Unknown
		//IL_3637: Unknown result type (might be due to invalid IL or missing references)
		//IL_3647: Unknown result type (might be due to invalid IL or missing references)
		//IL_3658: Unknown result type (might be due to invalid IL or missing references)
		//IL_3661: Expected O, but got Unknown
		//IL_35fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3607: Unknown result type (might be due to invalid IL or missing references)
		//IL_3610: Expected O, but got Unknown
		//IL_2d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d4d: Expected O, but got Unknown
		//IL_2d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d2a: Expected O, but got Unknown
		//IL_3686: Unknown result type (might be due to invalid IL or missing references)
		//IL_3690: Unknown result type (might be due to invalid IL or missing references)
		//IL_3699: Expected O, but got Unknown
		//IL_3702: Unknown result type (might be due to invalid IL or missing references)
		//IL_370b: Expected O, but got Unknown
		//IL_2e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e0a: Expected O, but got Unknown
		//IL_2e1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e26: Expected O, but got Unknown
		//IL_2da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db2: Expected O, but got Unknown
		//IL_2d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d8f: Expected O, but got Unknown
		//IL_373c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3745: Expected O, but got Unknown
		//IL_375e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3763: Unknown result type (might be due to invalid IL or missing references)
		//IL_3771: Unknown result type (might be due to invalid IL or missing references)
		//IL_3781: Unknown result type (might be due to invalid IL or missing references)
		//IL_379b: Expected O, but got Unknown
		//IL_37d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_37fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3822: Unknown result type (might be due to invalid IL or missing references)
		//IL_382b: Expected O, but got Unknown
		//IL_3844: Unknown result type (might be due to invalid IL or missing references)
		//IL_384d: Expected O, but got Unknown
		//IL_3879: Unknown result type (might be due to invalid IL or missing references)
		//IL_387e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3893: Unknown result type (might be due to invalid IL or missing references)
		//IL_38a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_38bd: Expected O, but got Unknown
		//IL_2e48: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e51: Expected O, but got Unknown
		//IL_2e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e95: Expected O, but got Unknown
		//IL_2eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ec1: Expected O, but got Unknown
		//IL_2eeb: Expected O, but got Unknown
		//IL_1009: Unknown result type (might be due to invalid IL or missing references)
		//IL_1024: Unknown result type (might be due to invalid IL or missing references)
		//IL_3961: Unknown result type (might be due to invalid IL or missing references)
		//IL_396a: Expected O, but got Unknown
		//IL_3996: Unknown result type (might be due to invalid IL or missing references)
		//IL_399b: Unknown result type (might be due to invalid IL or missing references)
		//IL_39b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_39c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_39da: Expected O, but got Unknown
		//IL_390b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3951: Unknown result type (might be due to invalid IL or missing references)
		//IL_340d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3416: Expected O, but got Unknown
		//IL_33ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_33f3: Expected O, but got Unknown
		//IL_1e13: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_34fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3505: Expected O, but got Unknown
		//IL_3512: Unknown result type (might be due to invalid IL or missing references)
		//IL_351b: Expected O, but got Unknown
		//IL_34ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_34d3: Expected O, but got Unknown
		//IL_34e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ef: Expected O, but got Unknown
		//IL_3472: Unknown result type (might be due to invalid IL or missing references)
		//IL_347b: Expected O, but got Unknown
		//IL_344f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3458: Expected O, but got Unknown
		//IL_3195: Unknown result type (might be due to invalid IL or missing references)
		//IL_319e: Expected O, but got Unknown
		//IL_31b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_31bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_31cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_31d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_31e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_31e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_31ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3203: Unknown result type (might be due to invalid IL or missing references)
		//IL_3208: Unknown result type (might be due to invalid IL or missing references)
		//IL_320d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3219: Unknown result type (might be due to invalid IL or missing references)
		//IL_321e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3224: Unknown result type (might be due to invalid IL or missing references)
		//IL_3229: Unknown result type (might be due to invalid IL or missing references)
		//IL_3237: Unknown result type (might be due to invalid IL or missing references)
		//IL_323d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3243: Unknown result type (might be due to invalid IL or missing references)
		//IL_23a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_23c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aa9: Expected O, but got Unknown
		//IL_3ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3acf: Expected O, but got Unknown
		//IL_3aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b15: Expected O, but got Unknown
		//IL_3b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bd0: Expected O, but got Unknown
		//IL_3bf4: Expected O, but got Unknown
		//IL_3bfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c25: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f88: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f92: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fce: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fde: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fe3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ff4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3012: Unknown result type (might be due to invalid IL or missing references)
		//IL_3018: Unknown result type (might be due to invalid IL or missing references)
		//IL_301e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3083: Unknown result type (might be due to invalid IL or missing references)
		//IL_3088: Unknown result type (might be due to invalid IL or missing references)
		//IL_308d: Unknown result type (might be due to invalid IL or missing references)
		//IL_309d: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_30bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_30c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_30d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_30d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_30de: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_30fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_310d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3113: Unknown result type (might be due to invalid IL or missing references)
		//IL_3119: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d90: Expected O, but got Unknown
		//IL_3d64: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d6d: Expected O, but got Unknown
		//IL_3e76: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e7f: Expected O, but got Unknown
		//IL_3e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e95: Expected O, but got Unknown
		//IL_3e97: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ea0: Expected O, but got Unknown
		//IL_3ec3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ec8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ed4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3edb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ee4: Expected O, but got Unknown
		//IL_3f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f10: Expected O, but got Unknown
		//IL_3f3a: Expected O, but got Unknown
		//IL_3e44: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e4d: Expected O, but got Unknown
		//IL_3e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e69: Expected O, but got Unknown
		//IL_3dec: Unknown result type (might be due to invalid IL or missing references)
		//IL_3df5: Expected O, but got Unknown
		//IL_3dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dd2: Expected O, but got Unknown
		//IL_41e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ed: Expected O, but got Unknown
		//IL_4206: Unknown result type (might be due to invalid IL or missing references)
		//IL_420b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4210: Unknown result type (might be due to invalid IL or missing references)
		//IL_421b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4226: Unknown result type (might be due to invalid IL or missing references)
		//IL_4231: Unknown result type (might be due to invalid IL or missing references)
		//IL_4236: Unknown result type (might be due to invalid IL or missing references)
		//IL_423b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4247: Unknown result type (might be due to invalid IL or missing references)
		//IL_4252: Unknown result type (might be due to invalid IL or missing references)
		//IL_4257: Unknown result type (might be due to invalid IL or missing references)
		//IL_425c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4268: Unknown result type (might be due to invalid IL or missing references)
		//IL_426d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4273: Unknown result type (might be due to invalid IL or missing references)
		//IL_4278: Unknown result type (might be due to invalid IL or missing references)
		//IL_4286: Unknown result type (might be due to invalid IL or missing references)
		//IL_428c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4292: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ff1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4007: Unknown result type (might be due to invalid IL or missing references)
		//IL_400c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4011: Unknown result type (might be due to invalid IL or missing references)
		//IL_401d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4028: Unknown result type (might be due to invalid IL or missing references)
		//IL_402d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4032: Unknown result type (might be due to invalid IL or missing references)
		//IL_403e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4043: Unknown result type (might be due to invalid IL or missing references)
		//IL_4049: Unknown result type (might be due to invalid IL or missing references)
		//IL_404e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4061: Unknown result type (might be due to invalid IL or missing references)
		//IL_4067: Unknown result type (might be due to invalid IL or missing references)
		//IL_406d: Unknown result type (might be due to invalid IL or missing references)
		//IL_40d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_40d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_40dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4102: Unknown result type (might be due to invalid IL or missing references)
		//IL_4107: Unknown result type (might be due to invalid IL or missing references)
		//IL_410c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4118: Unknown result type (might be due to invalid IL or missing references)
		//IL_4123: Unknown result type (might be due to invalid IL or missing references)
		//IL_4128: Unknown result type (might be due to invalid IL or missing references)
		//IL_412d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4139: Unknown result type (might be due to invalid IL or missing references)
		//IL_413e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4144: Unknown result type (might be due to invalid IL or missing references)
		//IL_4149: Unknown result type (might be due to invalid IL or missing references)
		//IL_415c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4162: Unknown result type (might be due to invalid IL or missing references)
		//IL_4168: Unknown result type (might be due to invalid IL or missing references)
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
		OnNurbsCurve val = InProfile.NurbsCurve.Convert();
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
		bool doMesh = InDoMeshedProngs.Boolean;
		if (CSApplicationHelper.Application.IsCamMode)
		{
			doMesh = false;
		}
		double num = InSizeMin.Double;
		double num2 = InSizeMax.Double;
		double num3 = InStartHeight.Double;
		double num4 = InMiddleHeight.Double;
		double num5 = InEndHeight.Double;
		double percentage = InStartFillet.Percentage;
		double percentage2 = InMiddleFillet.Percentage;
		double percentage3 = InEndFillet.Percentage;
		double percentage4 = InStartTaper.Percentage;
		double percentage5 = InMiddleTaper.Percentage;
		double percentage6 = InEndTaper.Percentage;
		double vNudgeEdge = InNudgeEdge.Double;
		double vNudgeGem = InNudgeGem.Double;
		double num6 = InStartNudgeEdge.Double;
		double num7 = InEndNudgeEdge.Double;
		double num8 = InStartNudgeGem.Double;
		double num9 = InEndNudgeGem.Double;
		double vNudgeEdge2 = InCornerNudgeEdge.Double;
		double num10 = InCornerNudgeGem.Double;
		double num11 = InStartVerticalLocation.Double;
		double num12 = InMiddleVerticalLocation.Double;
		double num13 = InEndVerticalLocation.Double;
		ValueListItem valueListItem = InEndProngStyle.ValueListItem;
		bool boolean = InEnableEndProngRotationMirroring.Boolean;
		bool boolean2 = InEnableEndProngEndControls.Boolean;
		double percentage7 = InSizePercentageStart.Percentage;
		double num14 = InRotationStart.Double;
		double num15 = InSpreadStart.Double;
		double num16 = InSizePercentageEnd.Percentage;
		double num17 = InRotationEnd.Double;
		double num18 = InSpreadEnd.Double;
		double num19 = InStartBlendTop.Double;
		double num20 = InMiddleBlendTop.Double;
		double num21 = InEndBlendTop.Double;
		double num22 = InStartBlendBot.Double;
		double num23 = InMiddleBlendBot.Double;
		double num24 = InEndBlendBot.Double;
		double num25 = InStartBlendEdge.Double;
		double num26 = InMiddleBlendEdge.Double;
		double num27 = InEndBlendEdge.Double;
		double num28 = InStartBottomArcPercentage.Double;
		double num29 = InMiddleBottomArcPercentage.Double;
		double num30 = InEndBottomArcPercentage.Double;
		double num31 = InCornerProngAngle.Double;
		int integer = InCornerProngSize.Integer;
		double num32 = InCornerProngSizeOffset.Double;
		ValueListItem valueListItem2 = InSettingStyle.ValueListItem;
		int integer2 = InProngSizeControl.Integer;
		bool flag = false;
		switch (valueListItem2)
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
		if (!boolean2)
		{
			num16 = percentage7;
			num17 = num14;
			if (boolean)
			{
				num17 *= -1.0;
			}
			num18 = num15;
		}
		ConcurrentBag<CSData> concurrentBag = new ConcurrentBag<CSData>();
		ConcurrentBag<CSData> concurrentBag2 = new ConcurrentBag<CSData>();
		OnCurve rail = CSUtilGemRails.Rail(cSDataArray);
		OnCurve val3 = CSUtilGemRails.Left(cSDataArray, rail).DuplicateCurve();
		if (!val3.IsClosed())
		{
			double num33 = -1.23432101234321E+308;
			if (!val3.GetLength(ref num33))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref val3, (Type)2, 0, num33 * 0.5);
			RhUtil.RhinoExtendCurve(ref val3, (Type)2, 1, num33 * 0.5);
		}
		OnCurve val4 = CSUtilGemRails.Right(cSDataArray, rail).DuplicateCurve();
		if (!val4.IsClosed())
		{
			double num34 = -1.23432101234321E+308;
			if (!val4.GetLength(ref num34))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref val4, (Type)2, 0, num34 * 0.5);
			RhUtil.RhinoExtendCurve(ref val4, (Type)2, 1, num34 * 0.5);
		}
		On3dVector val5 = new On3dVector(OnUtil.On_UNSET_VECTOR);
		On3dVector val6 = new On3dVector(OnUtil.On_UNSET_VECTOR);
		_prongCache.Clear();
		CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, 0);
		double gemZSize = channelGemData.GemZSize;
		double num35 = 0.0;
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
				double num36 = cSDataArray.GetArrayCount();
				double vHeight;
				double vVerticalLocation;
				if (i < cSDataArray.GetArrayCount() / 2)
				{
					vHeight = num3 * ((num36 / 2.0 - (double)i) / (num36 / 2.0)) + num4 * ((double)i / (num36 / 2.0));
					vVerticalLocation = num11 * ((num36 / 2.0 - (double)i) / (num36 / 2.0)) + num12 * ((double)i / (num36 / 2.0));
				}
				else
				{
					vHeight = num4 * (num36 - (double)i) / (double)i + num5 * (1.0 - (num36 - (double)i) / (double)i);
					vVerticalLocation = num12 * (num36 - (double)i) / (double)i + num13 * (1.0 - (num36 - (double)i) / (double)i);
				}
				double height2;
				string height = GetHeight(CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1), CSUtilGemRails.GetChannelGemData(cSDataArray, i), channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, vNudgeEdge, vNudgeGem, vHeight, vVerticalLocation, out height2);
				if (!string.IsNullOrEmpty(height))
				{
					return UpdateError(height);
				}
				if (height2 > num35)
				{
					num35 = height2;
				}
			}
		}
		InPercentToggle.GetValue(out bool value);
		double value2 = 0.0;
		double value3 = 0.0;
		double value4 = 0.0;
		if (value)
		{
			InStartProngPercentage.GetValue(out value2);
			value2 = 1.0 - value2;
			InMiddleProngPercentage.GetValue(out value3);
			value3 = 1.0 - value3;
			InEndProngPercentage.GetValue(out value4);
			value4 = 1.0 - value4;
		}
		bool flag2 = CSUtilGemRails.IsClosed(cSDataArray.Array);
		Point3d center;
		Line val12 = default(Line);
		double num44 = default(double);
		double num45 = default(double);
		double num46 = default(double);
		for (int j = 0; j < cSDataArray.GetArrayCount(); j++)
		{
			int num37 = j - 1;
			int num38 = j + 1;
			if (num37 < 0)
			{
				if (!flag2)
				{
					continue;
				}
				num37 = cSDataArray.GetArrayCount() - 1;
			}
			if (num38 >= cSDataArray.GetArrayCount())
			{
				num38 = ((!flag2) ? j : 0);
			}
			center = CSUtilGemRails.GetChannelGemData(cSDataArray, j).Center;
			double num39 = ((Point3d)(ref center)).DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, num37).Center);
			double num40 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).GemXSize * 2.0;
			if (num39 > num40)
			{
				continue;
			}
			CSChannelGemData channelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, num37);
			CSChannelGemData channelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			CSChannelGemData channelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, num38);
			if (channelGemData2 == null || channelGemData3 == null)
			{
				return UpdateError("Invalid gems");
			}
			CSDataHelper.SetSettingType(channelGemData3, flag ? GemSettingType.BR : GemSettingType.PR);
			double num41 = cSDataArray.GetArrayCount();
			double vHeight2;
			double num42;
			double num43;
			if (j < cSDataArray.GetArrayCount() / 2)
			{
				vHeight2 = num3 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + num4 * ((double)j / (num41 / 2.0));
				num42 = num11 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + num12 * ((double)j / (num41 / 2.0));
				num43 = value2 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + value3 * ((double)j / (num41 / 2.0));
			}
			else
			{
				vHeight2 = num4 * (num41 - (double)j) / (double)j + num5 * (1.0 - (num41 - (double)j) / (double)j);
				num42 = num12 * (num41 - (double)j) / (double)j + num13 * (1.0 - (num41 - (double)j) / (double)j);
				num43 = value3 * (num41 - (double)j) / (double)j + value4 * (1.0 - (num41 - (double)j) / (double)j);
			}
			OnCircle circleLeft;
			OnCircle circleRight;
			double height3;
			string circles = GetCircles(channelGemData2, channelGemData3, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, vNudgeEdge, vNudgeGem, vHeight2, num42, value, num43, out circleLeft, out circleRight, out height3);
			if (!string.IsNullOrEmpty(circles))
			{
				return UpdateError(circles);
			}
			if (_prongBase == ValueListItem.FollowLargestGem)
			{
				height3 = (value ? (num35 * num43) : num35);
			}
			On3dVector val7 = new On3dVector(circleLeft.Center());
			On3dVector val8 = new On3dVector(circleRight.Center());
			On3dPoint value5 = new On3dPoint((IOn3dVector)(object)((val7 + (IOn3dVector)(object)val8) / 2.0));
			On3dVector value6 = val8 - (IOn3dVector)(object)val7;
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
				Transform val11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value5), CSConvert.On3dVectorCopyToVector3d(value6), destination);
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
				((Surface)obj2).ClosestPoint(((Line)(ref val12)).PointAt(1.0), ref num44, ref num45);
				Point3d val16 = ((Surface)obj2).PointAt(num44, num45);
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
				if (num38 == j)
				{
					val18 = val17 * -1.0;
				}
				if (RhinoMath.ToDegrees(Math.Acos(val17 * val18)) < num31)
				{
					val17 *= channelGemData3.GemXSize / 2.0;
					val18 *= channelGemData3.GemXSize / 2.0;
					Vector3d val19 = val17 + val18;
					Point3d val20 = channelGemData3.Center + val19;
					channelGemData3.GetCurve().ClosestPoint(val20, ref num46);
					OnCircle circleLeft2;
					OnCircle circleRight2;
					double height4;
					string circles2 = GetCircles(channelGemData2, channelGemData3, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, vNudgeEdge2, num10, vHeight2, num42, value, num43, out circleLeft2, out circleRight2, out height4);
					OnCircle circleLeft3;
					OnCircle circleRight3;
					double height5;
					string circles3 = GetCircles(channelGemData3, channelGemData4, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, vNudgeEdge2, num10, vHeight2, num42, value, num43, out circleLeft3, out circleRight3, out height5);
					if (!string.IsNullOrEmpty(circles2) || !string.IsNullOrEmpty(circles3))
					{
						break;
					}
					double num47 = (circleRight3.radius + circleRight2.radius + circleLeft3.radius + circleLeft2.radius) / 4.0 + num32;
					double[] array = new double[4] { circleRight3.radius, circleRight2.radius, circleLeft2.radius, circleLeft3.radius };
					if (integer == 157)
					{
						double num48 = array[0];
						for (int k = 1; k < array.Length; k++)
						{
							if (num48 < array[k])
							{
								num48 = array[k];
							}
						}
						num47 = num48 + num32;
					}
					if (integer == 158)
					{
						double num49 = array[0];
						for (int l = 1; l < array.Length; l++)
						{
							if (num49 > array[l])
							{
								num49 = array[l];
							}
						}
						num47 = num49 + num32;
					}
					double num50 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData3.Size) / 2.0) : channelGemData3.GemZMin);
					Point3d val21 = channelGemData3.GetCurve().PointAt(num46);
					((Point3d)(ref val21)).Transform(Transform.Translation(channelGemData3.Normal * (num42 - num50)));
					((Vector3d)(ref val19)).Unitize();
					val19 *= num47 - num10;
					val21 += val19;
					On3dPoint val22 = CSConvert.Point3dCopyToOn3dPoint(val21);
					OnCircle val23 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val22, (IOn3dVector)(object)channelGemData3.Normal.Convert()), (IOn3dPoint)(object)val22, num47);
					OnXform val24 = new OnXform();
					val24.ChangeBasis((IOnPlane)(object)val23.plane, OnUtil.On_xy_plane);
					double num51 = val23.radius * 2.0;
					if (num51 > num && num51 < num2)
					{
						double bottomPercentage;
						double taperPer;
						double filletPer;
						if (j < cSDataArray.GetArrayCount() / 2)
						{
							bottomPercentage = num28 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + num29 * ((double)j / (num41 / 2.0));
							taperPer = percentage4 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + percentage5 * ((double)j / (num41 / 2.0));
							filletPer = percentage * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + percentage2 * ((double)j / (num41 / 2.0));
						}
						else
						{
							bottomPercentage = num29 * (num41 - (double)j) / (double)j + num30 * (1.0 - (num41 - (double)j) / (double)j);
							taperPer = percentage5 * (num41 - (double)j) / (double)j + percentage6 * (1.0 - (num41 - (double)j) / (double)j);
							filletPer = percentage2 * (num41 - (double)j) / (double)j + percentage3 * (1.0 - (num41 - (double)j) / (double)j);
						}
						if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num51, height3, filletPer, taperPer, doMesh, out var prong, out var prongBaseRail, bottomPercentage))
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
			if (integer2 == 157)
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
			if (integer2 == 158)
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
			if (integer2 == 159)
			{
				double radius = circleRight.radius;
				double radius2 = circleLeft.radius;
				double num52 = (radius + radius2) / 2.0;
				circleRight = new OnCircle(circleRight.Center(), num52);
				circleLeft = new OnCircle(circleLeft.Center(), num52);
			}
			if (!flag)
			{
				double num53 = circleLeft.radius * 2.0;
				if (num53 > num && num53 < num2)
				{
					double bottomPercentage2;
					double taperPer2;
					double filletPer2;
					if (j < cSDataArray.GetArrayCount() / 2)
					{
						bottomPercentage2 = num28 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + num29 * ((double)j / (num41 / 2.0));
						taperPer2 = percentage4 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + percentage5 * ((double)j / (num41 / 2.0));
						filletPer2 = percentage * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + percentage2 * ((double)j / (num41 / 2.0));
					}
					else
					{
						bottomPercentage2 = num29 * (num41 - (double)j) / (double)j + num30 * (1.0 - (num41 - (double)j) / (double)j);
						taperPer2 = percentage5 * (num41 - (double)j) / (double)j + percentage6 * (1.0 - (num41 - (double)j) / (double)j);
						filletPer2 = percentage2 * (num41 - (double)j) / (double)j + percentage3 * (1.0 - (num41 - (double)j) / (double)j);
					}
					if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num53, height3, filletPer2, taperPer2, doMesh, out var prong2, out var prongBaseRail2, bottomPercentage2))
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
				double num54 = circleRight.radius * 2.0;
				if (num54 > num && num54 < num2)
				{
					double bottomPercentage3;
					double taperPer3;
					double filletPer3;
					if (j < cSDataArray.GetArrayCount() / 2)
					{
						bottomPercentage3 = num28 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + num29 * ((double)j / (num41 / 2.0));
						taperPer3 = percentage4 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + percentage5 * ((double)j / (num41 / 2.0));
						filletPer3 = percentage * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + percentage2 * ((double)j / (num41 / 2.0));
					}
					else
					{
						bottomPercentage3 = num29 * (num41 - (double)j) / (double)j + num30 * (1.0 - (num41 - (double)j) / (double)j);
						taperPer3 = percentage5 * (num41 - (double)j) / (double)j + percentage6 * (1.0 - (num41 - (double)j) / (double)j);
						filletPer3 = percentage2 * (num41 - (double)j) / (double)j + percentage3 * (1.0 - (num41 - (double)j) / (double)j);
					}
					if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num54, height3, filletPer3, taperPer3, doMesh, out var prong3, out var prongBaseRail3, bottomPercentage3))
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
					baseDomeRadius = num22 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + num23 * ((double)j / (num41 / 2.0));
					blendEdge = num25 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + num26 * ((double)j / (num41 / 2.0));
					domeRadius = num19 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + num20 * ((double)j / (num41 / 2.0));
					bottomArcPercent = num28 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + num29 * ((double)j / (num41 / 2.0));
					filletPer4 = percentage4 * ((num41 / 2.0 - (double)j) / (num41 / 2.0)) + percentage5 * ((double)j / (num41 / 2.0));
				}
				else
				{
					baseDomeRadius = num23 * (num41 - (double)j) / (double)j + num24 * (1.0 - (num41 - (double)j) / (double)j);
					blendEdge = num26 * (num41 - (double)j) / (double)j + num27 * (1.0 - (num41 - (double)j) / (double)j);
					domeRadius = num20 * (num41 - (double)j) / (double)j + num21 * (1.0 - (num41 - (double)j) / (double)j);
					bottomArcPercent = num29 * (num41 - (double)j) / (double)j + num30 * (1.0 - (num41 - (double)j) / (double)j);
					filletPer4 = percentage5 * (num41 - (double)j) / (double)j + percentage6 * (1.0 - (num41 - (double)j) / (double)j);
				}
				Brep geometry = MakeBar((val7 - (IOn3dVector)(object)val8).Length(), circleLeft.radius, circleRight.radius, height3, filletPer4, domeRadius, baseDomeRadius, blendEdge, bottomArcPercent);
				value5 = new On3dPoint((IOn3dVector)(object)((val7 + (IOn3dVector)(object)val8) / 2.0));
				value6 = val8 - (IOn3dVector)(object)val7;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value5), CSConvert.On3dVectorCopyToVector3d(value6), CSConvert.On3dVectorCopyToVector3d(val29));
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
			double num58 = default(double);
			double num67 = default(double);
			for (int m = 0; m < cSDataArray.GetArrayCount(); m++)
			{
				int num55 = ((cSDataArray.GetArrayCount() > 1) ? (m + 1) : 0);
				int index = ((m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1));
				if (cSDataArray.GetArrayCount() > 1)
				{
					double num56;
					if (m != cSDataArray.GetArrayCount() - 1)
					{
						center = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center;
						num56 = ((Point3d)(ref center)).DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, m + 1).Center);
					}
					else
					{
						center = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center;
						num56 = ((Point3d)(ref center)).DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Center);
						num55 = 0;
					}
					double num57 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).GemXSize * 2.0;
					if (num56 <= num57)
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
						CSChannelGemData channelGemData7 = CSUtilGemRails.GetChannelGemData(cSDataArray, num55);
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
						if (num55 == m)
						{
							val31 = val30 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(val30 * val31)) < num31)
						{
							val30 *= channelGemData6.GemXSize / 2.0;
							val31 *= channelGemData6.GemXSize / 2.0;
							Vector3d val32 = val30 + val31;
							Point3d val33 = channelGemData6.Center + val32;
							channelGemData6.GetCurve().ClosestPoint(val33, ref num58);
							double num59 = cSDataArray.GetArrayCount();
							double vHeight3;
							double num60;
							double percent;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight3 = num3 * ((num59 / 2.0 - (double)m) / (num59 / 2.0)) + num4 * ((double)m / (num59 / 2.0));
								num60 = num11 * ((num59 / 2.0 - (double)m) / (num59 / 2.0)) + num12 * ((double)m / (num59 / 2.0));
								percent = value2 * ((num59 / 2.0 - (double)m) / (num59 / 2.0)) + value3 * ((double)m / (num59 / 2.0));
							}
							else
							{
								vHeight3 = num4 * (num59 - (double)m) / (double)m + num5 * (1.0 - (num59 - (double)m) / (double)m);
								num60 = num12 * (num59 - (double)m) / (double)m + num13 * (1.0 - (num59 - (double)m) / (double)m);
								percent = value3 * (num59 - (double)m) / (double)m + value4 * (1.0 - (num59 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData5, channelGemData6, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, vNudgeEdge2, num10, vHeight3, num60, value, percent, out var circleLeft4, out var circleRight4, out var height6)))
							{
								break;
							}
							double num61 = (circleRight4.radius + circleLeft4.radius) / 2.0 + num32;
							double[] array2 = new double[2] { circleRight4.radius, circleLeft4.radius };
							if (integer == 157)
							{
								double num62 = array2[0];
								for (int n = 1; n < array2.Length; n++)
								{
									if (num62 < array2[n])
									{
										num62 = array2[n];
									}
								}
								num61 = num62 + num32;
							}
							if (integer == 158)
							{
								double num63 = array2[0];
								for (int num64 = 1; num64 < array2.Length; num64++)
								{
									if (num63 > array2[num64])
									{
										num63 = array2[num64];
									}
								}
								num61 = num63 + num32;
							}
							double num65 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData6.Size) / 2.0) : channelGemData6.GemZMin);
							Point3d val34 = channelGemData6.GetCurve().PointAt(num58);
							((Point3d)(ref val34)).Transform(Transform.Translation(channelGemData6.Normal * (num60 - num65)));
							((Vector3d)(ref val32)).Unitize();
							val32 *= num61 - num10;
							val34 += val32;
							On3dPoint val35 = CSConvert.Point3dCopyToOn3dPoint(val34);
							OnCircle val36 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val35, (IOn3dVector)(object)channelGemData6.Normal.Convert()), (IOn3dPoint)(object)val35, num61);
							OnXform val37 = new OnXform();
							val37.ChangeBasis((IOnPlane)(object)val36.plane, OnUtil.On_xy_plane);
							double num66 = val36.radius * 2.0;
							if (num66 > num && num66 < num2)
							{
								double bottomPercentage4;
								double taperPer4;
								double filletPer5;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									bottomPercentage4 = num28 * ((num59 / 2.0 - (double)m) / (num59 / 2.0)) + num29 * ((double)m / (num59 / 2.0));
									taperPer4 = percentage4 * ((num59 / 2.0 - (double)m) / (num59 / 2.0)) + percentage5 * ((double)m / (num59 / 2.0));
									filletPer5 = percentage * ((num59 / 2.0 - (double)m) / (num59 / 2.0)) + percentage2 * ((double)m / (num59 / 2.0));
								}
								else
								{
									bottomPercentage4 = num29 * (num59 - (double)m) / (double)m + num30 * (1.0 - (num59 - (double)m) / (double)m);
									taperPer4 = percentage5 * (num59 - (double)m) / (double)m + percentage6 * (1.0 - (num59 - (double)m) / (double)m);
									filletPer5 = percentage2 * (num59 - (double)m) / (double)m + percentage3 * (1.0 - (num59 - (double)m) / (double)m);
								}
								if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num66, height6, filletPer5, taperPer4, doMesh, out var prong4, out var prongBaseRail4, bottomPercentage4))
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
						CSChannelGemData channelGemData9 = CSUtilGemRails.GetChannelGemData(cSDataArray, num55);
						CSChannelGemData channelGemData10 = CSUtilGemRails.GetChannelGemData(cSDataArray, (num55 + 1 != cSDataArray.GetArrayCount()) ? (num55 + 1) : 0);
						Vector3d val38 = channelGemData9.Center - channelGemData8.Center;
						((Vector3d)(ref val38)).Unitize();
						Vector3d val39 = channelGemData9.Center - channelGemData10.Center;
						((Vector3d)(ref val39)).Unitize();
						if (num55 == m)
						{
							val39 = val38 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(val38 * val39)) < num31)
						{
							val38 *= channelGemData9.GemXSize / 2.0;
							val39 *= channelGemData9.GemXSize / 2.0;
							Vector3d val40 = val38 + val39;
							Point3d val41 = channelGemData9.Center + val40;
							channelGemData9.GetCurve().ClosestPoint(val41, ref num67);
							double num68 = cSDataArray.GetArrayCount();
							double vHeight4;
							double num69;
							double percent2;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight4 = num3 * ((num68 / 2.0 - (double)m) / (num68 / 2.0)) + num4 * ((double)m / (num68 / 2.0));
								num69 = num11 * ((num68 / 2.0 - (double)m) / (num68 / 2.0)) + num12 * ((double)m / (num68 / 2.0));
								percent2 = value2 * ((num68 / 2.0 - (double)m) / (num68 / 2.0)) + value3 * ((double)m / (num68 / 2.0));
							}
							else
							{
								vHeight4 = num4 * (num68 - (double)m) / (double)m + num5 * (1.0 - (num68 - (double)m) / (double)m);
								num69 = num12 * (num68 - (double)m) / (double)m + num13 * (1.0 - (num68 - (double)m) / (double)m);
								percent2 = value3 * (num68 - (double)m) / (double)m + value4 * (1.0 - (num68 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData9, channelGemData10, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, vNudgeEdge2, num10, vHeight4, num69, value, percent2, out var circleLeft5, out var circleRight5, out var height7)))
							{
								break;
							}
							double num70 = (circleRight5.radius + circleLeft5.radius) / 2.0 + num32;
							double[] array3 = new double[2] { circleRight5.radius, circleLeft5.radius };
							if (integer == 157)
							{
								double num71 = array3[0];
								for (int num72 = 1; num72 < array3.Length; num72++)
								{
									if (num71 < array3[num72])
									{
										num71 = array3[num72];
									}
								}
								num70 = num71 + num32;
							}
							if (integer == 158)
							{
								double num73 = array3[0];
								for (int num74 = 1; num74 < array3.Length; num74++)
								{
									if (num73 > array3[num74])
									{
										num73 = array3[num74];
									}
								}
								num70 = num73 + num32;
							}
							double num75 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData9.Size) / 2.0) : channelGemData9.GemZMin);
							Point3d val42 = channelGemData9.GetCurve().PointAt(num67);
							((Point3d)(ref val42)).Transform(Transform.Translation(channelGemData9.Normal * (num69 - num75)));
							((Vector3d)(ref val40)).Unitize();
							val40 *= num70 - num10;
							val42 += val40;
							On3dPoint val43 = CSConvert.Point3dCopyToOn3dPoint(val42);
							OnCircle val44 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val43, (IOn3dVector)(object)channelGemData9.Normal.Convert()), (IOn3dPoint)(object)val43, num70);
							OnXform val45 = new OnXform();
							val45.ChangeBasis((IOnPlane)(object)val44.plane, OnUtil.On_xy_plane);
							double num76 = val44.radius * 2.0;
							if (num76 > num && num76 < num2)
							{
								double bottomPercentage5;
								double taperPer5;
								double filletPer6;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									bottomPercentage5 = num28 * ((num68 / 2.0 - (double)m) / (num68 / 2.0)) + num29 * ((double)m / (num68 / 2.0));
									taperPer5 = percentage4 * ((num68 / 2.0 - (double)m) / (num68 / 2.0)) + percentage5 * ((double)m / (num68 / 2.0));
									filletPer6 = percentage * ((num68 / 2.0 - (double)m) / (num68 / 2.0)) + percentage2 * ((double)m / (num68 / 2.0));
								}
								else
								{
									bottomPercentage5 = num29 * (num68 - (double)m) / (double)m + num30 * (1.0 - (num68 - (double)m) / (double)m);
									taperPer5 = percentage5 * (num68 - (double)m) / (double)m + percentage6 * (1.0 - (num68 - (double)m) / (double)m);
									filletPer6 = percentage2 * (num68 - (double)m) / (double)m + percentage3 * (1.0 - (num68 - (double)m) / (double)m);
								}
								if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num76, height7, filletPer6, taperPer5, doMesh, out var prong5, out var prongBaseRail5, bottomPercentage5))
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
				switch (valueListItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					CSChannelGemData cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, num55).Duplicate() as CSChannelGemData;
					if (cSDataArray.GetArrayCount() == 1)
					{
						cSChannelGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData.Normal, cSChannelGemData.Center));
					}
					double num78 = cSChannelGemData.GemXSize * percentage7 + num6 + num8;
					double height9 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + num3 - num11) : channelGemData.GemZSize);
					if (CSApplicationHelper.Application.IsCamMode)
					{
						CSChannelGemData gemPrev = cSChannelGemData;
						CSChannelGemData gemCurrent = CSUtilGemRails.GetChannelGemData(cSDataArray, (num55 + 1 != cSDataArray.GetArrayCount()) ? (num55 + 1) : 0).Duplicate() as CSChannelGemData;
						OnCircle circleLeft7;
						OnCircle circleRight7;
						double height10;
						string circles5 = GetCircles(gemPrev, gemCurrent, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, num6, num8, num3, num11, value, value2, out circleLeft7, out circleRight7, out height10);
						if (!string.IsNullOrEmpty(circles5))
						{
							return UpdateError(circles5);
						}
						if (integer2 == 157)
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
						if (integer2 == 158)
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
						if (integer2 == 159)
						{
							double radius5 = circleRight7.radius;
							double radius6 = circleLeft7.radius;
							double num79 = (radius5 + radius6) / 2.0;
							circleRight7 = new OnCircle(circleRight7.Center(), num79);
							circleLeft7 = new OnCircle(circleLeft7.Center(), num79);
						}
						On3dVector vLeft = new On3dVector(circleLeft7.Center());
						On3dVector vRight = new On3dVector(circleRight7.Center());
						height9 = GetCamHeightEnd(gemPrev, gemCurrent, circleLeft7, circleRight7, vLeft, vRight);
					}
					if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num78, height9, percentage, percentage4, doMesh, out var prong7, out var prongBaseRail7, num28))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj3 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
					double num80 = (cSChannelGemData.GemXSize * percentage7 + num6) / 2.0;
					double num81 = num11 - cSChannelGemData.GemZMin;
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
					OnPlane val62 = new OnPlane((IOn3dPoint)(object)(obj3 + (IOn3dVector)(object)(val61 * num80) + (IOn3dVector)(object)(val58 * num81)), (IOn3dVector)(object)val60, (IOn3dVector)(object)val61);
					switch (valueListItem)
					{
					case ValueListItem.ProngPlacerStyleEnd4:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd7:
					{
						OnXform val78 = new OnXform();
						val78.ChangeBasis((IOnPlane)(object)val62, OnUtil.On_xy_plane);
						OnXform val79 = new OnXform();
						val79.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num14), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
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
						double num82 = num15 / 2.0;
						OnXform val63 = new OnXform();
						val63.ChangeBasis((IOnPlane)(object)val62, OnUtil.On_xy_plane);
						OnXform val64 = new OnXform();
						val64.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num82), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val65 = new OnXform();
						val65.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num14 + num82), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
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
						val69.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num82), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val70 = new OnXform();
						val70.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num14 - num82), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
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
							Brep geometry3 = MakeBar((val74 - (IOn3dPoint)(object)val75).Length(), num78 / 2.0, num78 / 2.0, height9, percentage4, num19, num22, num25, num28);
							On3dPoint value11 = new On3dPoint((IOn3dPoint)(object)((val74 + (IOn3dPoint)(object)val75) / 2.0));
							On3dVector value12 = val75 - (IOn3dPoint)(object)val74;
							On3dVector value13 = val76 + (IOn3dVector)(object)val77;
							Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value11), CSConvert.On3dVectorCopyToVector3d(value12), CSConvert.On3dVectorCopyToVector3d(value13));
							concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry3, transform2));
						}
						break;
					}
					}
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd10:
				{
					CSChannelGemData channelGemData11 = CSUtilGemRails.GetChannelGemData(cSDataArray, num55);
					CSChannelGemData channelGemData12 = CSUtilGemRails.GetChannelGemData(cSDataArray, num55 + 1);
					if (channelGemData11 == null || channelGemData12 == null)
					{
						return UpdateError("Invalid gems");
					}
					CSDataHelper.SetSettingType(channelGemData12, flag ? GemSettingType.BR : GemSettingType.PR);
					OnCircle circleLeft6;
					OnCircle circleRight6;
					double height8;
					string circles4 = GetCircles(channelGemData11, channelGemData12, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, num6, num8, num3, num11, value, value2, out circleLeft6, out circleRight6, out height8);
					if (!string.IsNullOrEmpty(circles4))
					{
						return UpdateError(circles4);
					}
					On3dVector val46 = new On3dVector(circleLeft6.Center());
					On3dVector val47 = new On3dVector(circleRight6.Center());
					if (integer2 == 157)
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
					if (integer2 == 158)
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
					if (integer2 == 159)
					{
						double radius3 = circleRight6.radius;
						double radius4 = circleLeft6.radius;
						double num77 = (radius3 + radius4) / 2.0;
						circleRight6 = new OnCircle(circleRight6.Center(), num77);
						circleLeft6 = new OnCircle(circleLeft6.Center(), num77);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height8 = (value ? (num35 * value2) : num35);
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
					On3dVector value7 = val49 + (IOn3dVector)(object)val51;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height8 = GetCamHeightEnd(channelGemData11, channelGemData12, circleLeft6, circleRight6, val46, val47);
					}
					if (!flag)
					{
						if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, circleLeft6.Diameter(), height8, percentage, percentage4, doMesh, out var prong6, out var prongBaseRail6, num28))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value8 = val47 - (IOn3dVector)(object)val46;
						Transform val52 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val46)), CSConvert.On3dVectorCopyToVector3d(value8), CSConvert.On3dVectorCopyToVector3d(value7));
						Vector3d val53 = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform val54 = Transform.Mirror(channelGemData11.Center, val53);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val54 * val52));
						if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, circleRight6.Diameter(), height8, percentage, percentage4, doMesh, out prong6, out prongBaseRail6, num28))
						{
							return UpdateError("Create prong cache failed");
						}
						value8 = val46 - (IOn3dVector)(object)val47;
						val52 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val47)), CSConvert.On3dVectorCopyToVector3d(value8), CSConvert.On3dVectorCopyToVector3d(value7));
						val53 = channelGemData11.Center - channelGemData11.XMaxPoint;
						val54 = Transform.Mirror(channelGemData11.Center, val53);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val54 * val52));
					}
					else
					{
						Brep geometry2 = MakeBar((val46 - (IOn3dVector)(object)val47).Length(), circleLeft6.radius, circleRight6.radius, height8, percentage4, num19, num22, num25, num28);
						On3dPoint value9 = new On3dPoint((IOn3dVector)(object)((val46 + (IOn3dVector)(object)val47) / 2.0));
						On3dVector value10 = val47 - (IOn3dVector)(object)val46;
						Transform val55 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value9), CSConvert.On3dVectorCopyToVector3d(value10), CSConvert.On3dVectorCopyToVector3d(value7));
						Vector3d val56 = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform val57 = Transform.Mirror(channelGemData11.Center, val56);
						concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry2, val57 * val55));
					}
					break;
				}
				}
				switch (valueListItem)
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
					double num84 = cSChannelGemData2.GemXSize * num16 + num7 + num9;
					double height12 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData2.GemZSize + num5 - num13) : channelGemData.GemZSize);
					if (CSApplicationHelper.Application.IsCamMode)
					{
						CSChannelGemData gemCurrent2 = cSChannelGemData2;
						CSChannelGemData gemPrev2 = CSUtilGemRails.GetChannelGemData(cSDataArray, (m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1)).Duplicate() as CSChannelGemData;
						OnCircle circleLeft9;
						OnCircle circleRight9;
						double height13;
						string circles7 = GetCircles(gemPrev2, gemCurrent2, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, num7, num9, num5, num13, value, value4, out circleLeft9, out circleRight9, out height13);
						if (!string.IsNullOrEmpty(circles7))
						{
							return UpdateError(circles7);
						}
						if (integer2 == 157)
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
						if (integer2 == 158)
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
						if (integer2 == 159)
						{
							double radius9 = circleRight9.radius;
							double radius10 = circleLeft9.radius;
							double num85 = (radius9 + radius10) / 2.0;
							circleRight9 = new OnCircle(circleRight9.Center(), num85);
							circleLeft9 = new OnCircle(circleLeft9.Center(), num85);
						}
						On3dVector vLeft2 = new On3dVector(circleLeft9.Center());
						On3dVector vRight2 = new On3dVector(circleRight9.Center());
						height12 = GetCamHeightEnd(gemPrev2, gemCurrent2, circleLeft9, circleRight9, vLeft2, vRight2);
					}
					if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num84, height12, percentage3, percentage6, doMesh, out var prong9, out var prongBaseRail9, num30))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj4 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					double num86 = (cSChannelGemData2.GemXSize * num16 + num7) / 2.0;
					double num87 = num13 - cSChannelGemData2.GemZMin;
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
					OnPlane val97 = new OnPlane((IOn3dPoint)(object)(obj4 + (IOn3dVector)(object)(val96 * num86) + (IOn3dVector)(object)(val93 * num87)), (IOn3dVector)(object)val95, (IOn3dVector)(object)val96);
					switch (valueListItem)
					{
					case ValueListItem.ProngPlacerStyleEnd2:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd8:
					{
						OnXform val113 = new OnXform();
						val113.ChangeBasis((IOnPlane)(object)val97, OnUtil.On_xy_plane);
						OnXform val114 = new OnXform();
						val114.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num17), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
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
						double num88 = num18 / 2.0;
						OnXform val98 = new OnXform();
						val98.ChangeBasis((IOnPlane)(object)val97, OnUtil.On_xy_plane);
						OnXform val99 = new OnXform();
						val99.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num88), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val100 = new OnXform();
						val100.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num17 + num88), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
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
						val104.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num88), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val105 = new OnXform();
						val105.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num17 - num88), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
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
							Brep geometry5 = MakeBar((val109 - (IOn3dPoint)(object)val110).Length(), num84 / 2.0, num84 / 2.0, height12, percentage6, num21, num24, num27, num30);
							On3dPoint value18 = new On3dPoint((IOn3dPoint)(object)((val109 + (IOn3dPoint)(object)val110) / 2.0));
							On3dVector value19 = val110 - (IOn3dPoint)(object)val109;
							On3dVector value20 = val111 + (IOn3dVector)(object)val112;
							Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value18), CSConvert.On3dVectorCopyToVector3d(value19), CSConvert.On3dVectorCopyToVector3d(value20));
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
					string circles6 = GetCircles(channelGemData13, channelGemData14, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, num7, num9, num5, num13, value, value4, out circleLeft8, out circleRight8, out height11);
					if (!string.IsNullOrEmpty(circles6))
					{
						return UpdateError(circles6);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height11 = (value ? (num35 * value4) : num35);
					}
					if (integer2 == 157)
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
					if (integer2 == 158)
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
					if (integer2 == 159)
					{
						double radius7 = circleRight8.radius;
						double radius8 = circleLeft8.radius;
						double num83 = (radius7 + radius8) / 2.0;
						circleRight8 = new OnCircle(circleRight8.Center(), num83);
						circleLeft8 = new OnCircle(circleLeft8.Center(), num83);
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
					On3dVector value14 = val84 + (IOn3dVector)(object)val86;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height11 = GetCamHeightEnd(channelGemData13, channelGemData14, circleLeft8, circleRight8, val81, val82);
					}
					if (!flag)
					{
						if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, circleLeft8.Diameter(), height11, percentage3, percentage6, doMesh, out var prong8, out var prongBaseRail8, num30))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value15 = val82 - (IOn3dVector)(object)val81;
						Transform val87 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val81)), CSConvert.On3dVectorCopyToVector3d(value15), CSConvert.On3dVectorCopyToVector3d(value14));
						Vector3d val88 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform val89 = Transform.Mirror(channelGemData14.Center, val88);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), val89 * val87));
						if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, circleRight8.Diameter(), height11, percentage3, percentage6, doMesh, out prong8, out prongBaseRail8, num30))
						{
							return UpdateError("Create prong cache failed");
						}
						value15 = val81 - (IOn3dVector)(object)val82;
						val87 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val82)), CSConvert.On3dVectorCopyToVector3d(value15), CSConvert.On3dVectorCopyToVector3d(value14));
						val88 = channelGemData14.Center - channelGemData14.XMaxPoint;
						val89 = Transform.Mirror(channelGemData14.Center, val88);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), val89 * val87));
					}
					else
					{
						Brep geometry4 = MakeBar((val81 - (IOn3dVector)(object)val82).Length(), circleLeft8.radius, circleRight8.radius, height11, percentage6, num21, num24, num27, num30);
						On3dPoint value16 = new On3dPoint((IOn3dVector)(object)((val81 + (IOn3dVector)(object)val82) / 2.0));
						On3dVector value17 = val82 - (IOn3dVector)(object)val81;
						Transform val90 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value16), CSConvert.On3dVectorCopyToVector3d(value17), CSConvert.On3dVectorCopyToVector3d(value14));
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

	private static NurbsCurve BuildProngProfile(double radius, double height, double radiusTopDome, double radiusBottomDome, double taperPer)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Expected O, but got Unknown
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

	private static string GetHeight(CSChannelGemData gemPrev, CSChannelGemData gemCurrent, CSChannelGemData gemMax, IOnCurve railLeft, IOnCurve railRight, double vNudgeEdge, double vNudgeGem, double vHeight, double vVerticalLocation, out double height)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
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
