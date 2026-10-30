using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CS;
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

namespace CSBlocks.Settings;

[CSBlock("ProngPlacerGC", "{895B4619-1C42-480F-BE3A-D8B77A2DF397}")]
public sealed class CSProngPlacerGC : CSBlock
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

	public CSProngPlacerGC(string identifier)
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
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_097f: Expected O, but got Unknown
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_098d: Expected O, but got Unknown
		//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ab: Expected O, but got Unknown
		//IL_1849: Unknown result type (might be due to invalid IL or missing references)
		//IL_184e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1859: Unknown result type (might be due to invalid IL or missing references)
		//IL_1820: Unknown result type (might be due to invalid IL or missing references)
		//IL_1825: Unknown result type (might be due to invalid IL or missing references)
		//IL_1833: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Expected O, but got Unknown
		//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a04: Expected O, but got Unknown
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1024: Unknown result type (might be due to invalid IL or missing references)
		//IL_102b: Expected O, but got Unknown
		//IL_1044: Unknown result type (might be due to invalid IL or missing references)
		//IL_104b: Expected O, but got Unknown
		//IL_105c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1063: Expected O, but got Unknown
		//IL_107c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1083: Expected O, but got Unknown
		//IL_0ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2417: Unknown result type (might be due to invalid IL or missing references)
		//IL_241e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2423: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1901: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d9: Expected O, but got Unknown
		//IL_10bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c2: Expected O, but got Unknown
		//IL_0c10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e77: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1923: Unknown result type (might be due to invalid IL or missing references)
		//IL_1925: Unknown result type (might be due to invalid IL or missing references)
		//IL_1911: Unknown result type (might be due to invalid IL or missing references)
		//IL_191c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1921: Unknown result type (might be due to invalid IL or missing references)
		//IL_114f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1156: Expected O, but got Unknown
		//IL_115f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1166: Expected O, but got Unknown
		//IL_1117: Unknown result type (might be due to invalid IL or missing references)
		//IL_111e: Expected O, but got Unknown
		//IL_1100: Unknown result type (might be due to invalid IL or missing references)
		//IL_1107: Expected O, but got Unknown
		//IL_1eb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_193d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1950: Unknown result type (might be due to invalid IL or missing references)
		//IL_1955: Unknown result type (might be due to invalid IL or missing references)
		//IL_1957: Unknown result type (might be due to invalid IL or missing references)
		//IL_196a: Unknown result type (might be due to invalid IL or missing references)
		//IL_196f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1971: Unknown result type (might be due to invalid IL or missing references)
		//IL_1973: Unknown result type (might be due to invalid IL or missing references)
		//IL_1975: Unknown result type (might be due to invalid IL or missing references)
		//IL_197a: Unknown result type (might be due to invalid IL or missing references)
		//IL_197e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1983: Unknown result type (might be due to invalid IL or missing references)
		//IL_1985: Unknown result type (might be due to invalid IL or missing references)
		//IL_198a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1993: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2609: Unknown result type (might be due to invalid IL or missing references)
		//IL_2610: Unknown result type (might be due to invalid IL or missing references)
		//IL_261a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2624: Expected O, but got Unknown
		//IL_2624: Expected O, but got Unknown
		//IL_1ed0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1efd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f02: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f04: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f18: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_2674: Unknown result type (might be due to invalid IL or missing references)
		//IL_267b: Expected O, but got Unknown
		//IL_2683: Unknown result type (might be due to invalid IL or missing references)
		//IL_268f: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a7: Expected O, but got Unknown
		//IL_2659: Unknown result type (might be due to invalid IL or missing references)
		//IL_2663: Unknown result type (might be due to invalid IL or missing references)
		//IL_266a: Expected O, but got Unknown
		//IL_1513: Unknown result type (might be due to invalid IL or missing references)
		//IL_151a: Expected O, but got Unknown
		//IL_1521: Unknown result type (might be due to invalid IL or missing references)
		//IL_1528: Expected O, but got Unknown
		//IL_26bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_26cd: Expected O, but got Unknown
		//IL_2708: Unknown result type (might be due to invalid IL or missing references)
		//IL_270f: Expected O, but got Unknown
		//IL_2736: Unknown result type (might be due to invalid IL or missing references)
		//IL_273f: Expected O, but got Unknown
		//IL_2754: Unknown result type (might be due to invalid IL or missing references)
		//IL_2759: Unknown result type (might be due to invalid IL or missing references)
		//IL_2763: Unknown result type (might be due to invalid IL or missing references)
		//IL_276f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2789: Expected O, but got Unknown
		//IL_27ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2800: Unknown result type (might be due to invalid IL or missing references)
		//IL_2809: Expected O, but got Unknown
		//IL_281e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2827: Expected O, but got Unknown
		//IL_2853: Unknown result type (might be due to invalid IL or missing references)
		//IL_2858: Unknown result type (might be due to invalid IL or missing references)
		//IL_2869: Unknown result type (might be due to invalid IL or missing references)
		//IL_2875: Unknown result type (might be due to invalid IL or missing references)
		//IL_288f: Expected O, but got Unknown
		//IL_3291: Unknown result type (might be due to invalid IL or missing references)
		//IL_329c: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1754: Unknown result type (might be due to invalid IL or missing references)
		//IL_175b: Expected O, but got Unknown
		//IL_1766: Unknown result type (might be due to invalid IL or missing references)
		//IL_176b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1770: Unknown result type (might be due to invalid IL or missing references)
		//IL_1777: Unknown result type (might be due to invalid IL or missing references)
		//IL_177e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1785: Unknown result type (might be due to invalid IL or missing references)
		//IL_178a: Unknown result type (might be due to invalid IL or missing references)
		//IL_178f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e48: Expected O, but got Unknown
		//IL_0e43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4a: Expected O, but got Unknown
		//IL_0e4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e51: Expected O, but got Unknown
		//IL_292b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2934: Expected O, but got Unknown
		//IL_2960: Unknown result type (might be due to invalid IL or missing references)
		//IL_2965: Unknown result type (might be due to invalid IL or missing references)
		//IL_2976: Unknown result type (might be due to invalid IL or missing references)
		//IL_2982: Unknown result type (might be due to invalid IL or missing references)
		//IL_299c: Expected O, but got Unknown
		//IL_28d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_291b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2511: Unknown result type (might be due to invalid IL or missing references)
		//IL_2518: Expected O, but got Unknown
		//IL_24fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2501: Expected O, but got Unknown
		//IL_2ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cae: Expected O, but got Unknown
		//IL_2cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cc4: Expected O, but got Unknown
		//IL_1301: Unknown result type (might be due to invalid IL or missing references)
		//IL_131c: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b3: Expected O, but got Unknown
		//IL_25ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c1: Expected O, but got Unknown
		//IL_258e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2595: Expected O, but got Unknown
		//IL_259e: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a5: Expected O, but got Unknown
		//IL_2556: Unknown result type (might be due to invalid IL or missing references)
		//IL_255d: Expected O, but got Unknown
		//IL_253f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2546: Expected O, but got Unknown
		//IL_14c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a63: Expected O, but got Unknown
		//IL_2a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a89: Expected O, but got Unknown
		//IL_2aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2acf: Expected O, but got Unknown
		//IL_2ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b7e: Expected O, but got Unknown
		//IL_2ba2: Expected O, but got Unknown
		//IL_2ba8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d29: Expected O, but got Unknown
		//IL_2cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d06: Expected O, but got Unknown
		//IL_1be8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c02: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c22: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c52: Expected O, but got Unknown
		//IL_1c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c54: Expected O, but got Unknown
		//IL_1c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5b: Expected O, but got Unknown
		//IL_3567: Unknown result type (might be due to invalid IL or missing references)
		//IL_3571: Unknown result type (might be due to invalid IL or missing references)
		//IL_357c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3586: Unknown result type (might be due to invalid IL or missing references)
		//IL_3590: Expected O, but got Unknown
		//IL_3590: Expected O, but got Unknown
		//IL_2ddd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2de6: Expected O, but got Unknown
		//IL_2df9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e02: Expected O, but got Unknown
		//IL_2d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d8e: Expected O, but got Unknown
		//IL_2d62: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d6b: Expected O, but got Unknown
		//IL_217b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2180: Unknown result type (might be due to invalid IL or missing references)
		//IL_2186: Unknown result type (might be due to invalid IL or missing references)
		//IL_2190: Unknown result type (might be due to invalid IL or missing references)
		//IL_2195: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_21be: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_21cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e5: Expected O, but got Unknown
		//IL_21e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e7: Expected O, but got Unknown
		//IL_21e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ee: Expected O, but got Unknown
		//IL_35fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3605: Expected O, but got Unknown
		//IL_3613: Unknown result type (might be due to invalid IL or missing references)
		//IL_3623: Unknown result type (might be due to invalid IL or missing references)
		//IL_3634: Unknown result type (might be due to invalid IL or missing references)
		//IL_363d: Expected O, but got Unknown
		//IL_35d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_35e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ec: Expected O, but got Unknown
		//IL_2e24: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e2d: Expected O, but got Unknown
		//IL_2e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e55: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e71: Expected O, but got Unknown
		//IL_2e94: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e9d: Expected O, but got Unknown
		//IL_2ec7: Expected O, but got Unknown
		//IL_0ff9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1014: Unknown result type (might be due to invalid IL or missing references)
		//IL_3662: Unknown result type (might be due to invalid IL or missing references)
		//IL_366c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3675: Expected O, but got Unknown
		//IL_36de: Unknown result type (might be due to invalid IL or missing references)
		//IL_36e7: Expected O, but got Unknown
		//IL_3718: Unknown result type (might be due to invalid IL or missing references)
		//IL_3721: Expected O, but got Unknown
		//IL_373a: Unknown result type (might be due to invalid IL or missing references)
		//IL_373f: Unknown result type (might be due to invalid IL or missing references)
		//IL_374d: Unknown result type (might be due to invalid IL or missing references)
		//IL_375d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3777: Expected O, but got Unknown
		//IL_37b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_37d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_37fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3807: Expected O, but got Unknown
		//IL_3820: Unknown result type (might be due to invalid IL or missing references)
		//IL_3829: Expected O, but got Unknown
		//IL_3855: Unknown result type (might be due to invalid IL or missing references)
		//IL_385a: Unknown result type (might be due to invalid IL or missing references)
		//IL_386f: Unknown result type (might be due to invalid IL or missing references)
		//IL_387f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3899: Expected O, but got Unknown
		//IL_3171: Unknown result type (might be due to invalid IL or missing references)
		//IL_317a: Expected O, but got Unknown
		//IL_3193: Unknown result type (might be due to invalid IL or missing references)
		//IL_3198: Unknown result type (might be due to invalid IL or missing references)
		//IL_319d: Unknown result type (might be due to invalid IL or missing references)
		//IL_31a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_31be: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_31d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_31df: Unknown result type (might be due to invalid IL or missing references)
		//IL_31e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_31e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_31fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3200: Unknown result type (might be due to invalid IL or missing references)
		//IL_3205: Unknown result type (might be due to invalid IL or missing references)
		//IL_3213: Unknown result type (might be due to invalid IL or missing references)
		//IL_3219: Unknown result type (might be due to invalid IL or missing references)
		//IL_321f: Unknown result type (might be due to invalid IL or missing references)
		//IL_393d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3946: Expected O, but got Unknown
		//IL_3972: Unknown result type (might be due to invalid IL or missing references)
		//IL_3977: Unknown result type (might be due to invalid IL or missing references)
		//IL_398c: Unknown result type (might be due to invalid IL or missing references)
		//IL_399c: Unknown result type (might be due to invalid IL or missing references)
		//IL_39b6: Expected O, but got Unknown
		//IL_38e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_392d: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_33f2: Expected O, but got Unknown
		//IL_33c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_33cf: Expected O, but got Unknown
		//IL_2f64: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f89: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fee: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ff4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_34d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e1: Expected O, but got Unknown
		//IL_34ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_34f7: Expected O, but got Unknown
		//IL_34a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_34af: Expected O, but got Unknown
		//IL_34c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_34cb: Expected O, but got Unknown
		//IL_344e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3457: Expected O, but got Unknown
		//IL_342b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3434: Expected O, but got Unknown
		//IL_305f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3064: Unknown result type (might be due to invalid IL or missing references)
		//IL_3069: Unknown result type (might be due to invalid IL or missing references)
		//IL_3079: Unknown result type (might be due to invalid IL or missing references)
		//IL_3084: Unknown result type (might be due to invalid IL or missing references)
		//IL_308f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3094: Unknown result type (might be due to invalid IL or missing references)
		//IL_3099: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_30c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_30cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_30d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_30d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_30e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2396: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a85: Expected O, but got Unknown
		//IL_3aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aab: Expected O, but got Unknown
		//IL_3ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3af1: Expected O, but got Unknown
		//IL_3b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bac: Expected O, but got Unknown
		//IL_3bd0: Expected O, but got Unknown
		//IL_3bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c01: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d58: Expected O, but got Unknown
		//IL_3d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d35: Expected O, but got Unknown
		//IL_3e3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e47: Expected O, but got Unknown
		//IL_3e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e5d: Expected O, but got Unknown
		//IL_3e5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e68: Expected O, but got Unknown
		//IL_3e8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ea3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eac: Expected O, but got Unknown
		//IL_3ecf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ed8: Expected O, but got Unknown
		//IL_3f02: Expected O, but got Unknown
		//IL_3e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e15: Expected O, but got Unknown
		//IL_3e28: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e31: Expected O, but got Unknown
		//IL_3db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dbd: Expected O, but got Unknown
		//IL_3d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d9a: Expected O, but got Unknown
		//IL_41ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_41b5: Expected O, but got Unknown
		//IL_41ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_41e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_41f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_41fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_4203: Unknown result type (might be due to invalid IL or missing references)
		//IL_420f: Unknown result type (might be due to invalid IL or missing references)
		//IL_421a: Unknown result type (might be due to invalid IL or missing references)
		//IL_421f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4224: Unknown result type (might be due to invalid IL or missing references)
		//IL_4230: Unknown result type (might be due to invalid IL or missing references)
		//IL_4235: Unknown result type (might be due to invalid IL or missing references)
		//IL_423b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4240: Unknown result type (might be due to invalid IL or missing references)
		//IL_424e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4254: Unknown result type (might be due to invalid IL or missing references)
		//IL_425a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_4006: Unknown result type (might be due to invalid IL or missing references)
		//IL_400b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4011: Unknown result type (might be due to invalid IL or missing references)
		//IL_4016: Unknown result type (might be due to invalid IL or missing references)
		//IL_4029: Unknown result type (might be due to invalid IL or missing references)
		//IL_402f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4035: Unknown result type (might be due to invalid IL or missing references)
		//IL_409a: Unknown result type (might be due to invalid IL or missing references)
		//IL_409f: Unknown result type (might be due to invalid IL or missing references)
		//IL_40a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_40b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_40bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_40cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_40d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_40e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_40eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4101: Unknown result type (might be due to invalid IL or missing references)
		//IL_4106: Unknown result type (might be due to invalid IL or missing references)
		//IL_410c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4111: Unknown result type (might be due to invalid IL or missing references)
		//IL_4124: Unknown result type (might be due to invalid IL or missing references)
		//IL_412a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4130: Unknown result type (might be due to invalid IL or missing references)
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
