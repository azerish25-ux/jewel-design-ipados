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
[CSBlock("ProngPlacerGC_V6", "{4E675360-94C4-4A00-8225-4423D76D4278}")]
public sealed class CSProngPlacerGC6 : CSBlock
{
	public readonly CSInputChannel InSortedGems;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InSizeMin;

	public readonly CSInputChannel InSizeMax;

	public readonly CSInputChannel InHeight;

	public readonly CSInputChannel InFillet;

	public readonly CSInputChannel InTaper;

	public readonly CSInputChannel InNudgeEdge;

	public readonly CSInputChannel InNudgeGem;

	public readonly CSInputChannel InVerticalLocation;

	public readonly CSInputChannel InEndProngStyle;

	public readonly CSInputChannel InEnableEndProngRotationMirroring;

	public readonly CSInputChannel InEnableEndProngEndControls;

	public readonly CSInputChannel InSizePercentageStart;

	public readonly CSInputChannel InRotationStart;

	public readonly CSInputChannel InSpreadStart;

	public readonly CSInputChannel InSizePercentageEnd;

	public readonly CSInputChannel InRotationEnd;

	public readonly CSInputChannel InSpreadEnd;

	public readonly CSInputChannel InDoMeshedProngs;

	public readonly CSInputChannel InSettingStyle;

	public readonly CSInputChannel InMaxGem;

	public readonly CSInputChannel InPercentToggle;

	public readonly CSInputChannel InProngPercentage;

	public readonly CSInputChannel InBlendTop;

	public readonly CSInputChannel InBlendBot;

	public readonly CSInputChannel InBlendEdge;

	public readonly CSInputChannel InBottomArcPercentage;

	public readonly CSOutputChannel OutProngs;

	public readonly CSOutputChannel OutProfiles;

	private ValueListItem _prongBase = ValueListItem.FollowCulet;

	private readonly CSProngCache _prongCache = new CSProngCache();

	public CSProngPlacerGC6(string identifier)
		: base(identifier)
	{
		InSortedGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InSizeMin = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSizeMax = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InNudgeEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InNudgeGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InVerticalLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		InProngPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBlendTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.2);
		InBlendBot = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.2);
		InBlendEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBottomArcPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProfiles = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Expected O, but got Unknown
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5a: Expected O, but got Unknown
		//IL_0b5a: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Expected O, but got Unknown
		//IL_0baa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb1: Expected O, but got Unknown
		//IL_0bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Expected O, but got Unknown
		//IL_0b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Expected O, but got Unknown
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Expected O, but got Unknown
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfd: Expected O, but got Unknown
		//IL_0c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3f: Expected O, but got Unknown
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Expected O, but got Unknown
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c7: Expected O, but got Unknown
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6d: Expected O, but got Unknown
		//IL_0c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cae: Expected O, but got Unknown
		//IL_0ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d14: Expected O, but got Unknown
		//IL_0d23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2a: Expected O, but got Unknown
		//IL_0d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d82: Expected O, but got Unknown
		//IL_0deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df2: Expected O, but got Unknown
		//IL_0e15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4a: Expected O, but got Unknown
		//IL_0dad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Expected O, but got Unknown
		//IL_099d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Expected O, but got Unknown
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f2: Expected O, but got Unknown
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edc: Expected O, but got Unknown
		//IL_0ef7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efe: Expected O, but got Unknown
		//IL_0f19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f40: Expected O, but got Unknown
		//IL_0f49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbb: Expected O, but got Unknown
		//IL_0fcd: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1540: Unknown result type (might be due to invalid IL or missing references)
		//IL_154a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1551: Unknown result type (might be due to invalid IL or missing references)
		//IL_155b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1565: Expected O, but got Unknown
		//IL_1565: Expected O, but got Unknown
		//IL_108c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1093: Expected O, but got Unknown
		//IL_109a: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a1: Expected O, but got Unknown
		//IL_15b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bc: Expected O, but got Unknown
		//IL_15c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_15dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e4: Expected O, but got Unknown
		//IL_159a: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ab: Expected O, but got Unknown
		//IL_10bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c4: Expected O, but got Unknown
		//IL_10dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f8: Expected O, but got Unknown
		//IL_1111: Unknown result type (might be due to invalid IL or missing references)
		//IL_1118: Expected O, but got Unknown
		//IL_1130: Expected O, but got Unknown
		//IL_15f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1601: Unknown result type (might be due to invalid IL or missing references)
		//IL_1608: Expected O, but got Unknown
		//IL_1643: Unknown result type (might be due to invalid IL or missing references)
		//IL_164a: Expected O, but got Unknown
		//IL_1143: Unknown result type (might be due to invalid IL or missing references)
		//IL_1148: Unknown result type (might be due to invalid IL or missing references)
		//IL_114f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1154: Unknown result type (might be due to invalid IL or missing references)
		//IL_1158: Unknown result type (might be due to invalid IL or missing references)
		//IL_117e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1185: Expected O, but got Unknown
		//IL_1190: Unknown result type (might be due to invalid IL or missing references)
		//IL_1195: Unknown result type (might be due to invalid IL or missing references)
		//IL_119a: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11af: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1210: Unknown result type (might be due to invalid IL or missing references)
		//IL_1215: Unknown result type (might be due to invalid IL or missing references)
		//IL_121b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1220: Unknown result type (might be due to invalid IL or missing references)
		//IL_1224: Unknown result type (might be due to invalid IL or missing references)
		//IL_1679: Unknown result type (might be due to invalid IL or missing references)
		//IL_1680: Expected O, but got Unknown
		//IL_168f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1694: Unknown result type (might be due to invalid IL or missing references)
		//IL_169f: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c1: Expected O, but got Unknown
		//IL_16e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1720: Unknown result type (might be due to invalid IL or missing references)
		//IL_1727: Expected O, but got Unknown
		//IL_1736: Unknown result type (might be due to invalid IL or missing references)
		//IL_173d: Expected O, but got Unknown
		//IL_1760: Unknown result type (might be due to invalid IL or missing references)
		//IL_1765: Unknown result type (might be due to invalid IL or missing references)
		//IL_1773: Unknown result type (might be due to invalid IL or missing references)
		//IL_177f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Expected O, but got Unknown
		//IL_13f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fa: Expected O, but got Unknown
		//IL_1405: Unknown result type (might be due to invalid IL or missing references)
		//IL_140a: Unknown result type (might be due to invalid IL or missing references)
		//IL_140f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1416: Unknown result type (might be due to invalid IL or missing references)
		//IL_141d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1424: Unknown result type (might be due to invalid IL or missing references)
		//IL_1429: Unknown result type (might be due to invalid IL or missing references)
		//IL_142e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1432: Unknown result type (might be due to invalid IL or missing references)
		//IL_1439: Unknown result type (might be due to invalid IL or missing references)
		//IL_143e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1443: Unknown result type (might be due to invalid IL or missing references)
		//IL_1447: Unknown result type (might be due to invalid IL or missing references)
		//IL_144c: Unknown result type (might be due to invalid IL or missing references)
		//IL_144e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1453: Unknown result type (might be due to invalid IL or missing references)
		//IL_1459: Unknown result type (might be due to invalid IL or missing references)
		//IL_145b: Unknown result type (might be due to invalid IL or missing references)
		//IL_145d: Unknown result type (might be due to invalid IL or missing references)
		//IL_123b: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1805: Expected O, but got Unknown
		//IL_1828: Unknown result type (might be due to invalid IL or missing references)
		//IL_182d: Unknown result type (might be due to invalid IL or missing references)
		//IL_183b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1847: Unknown result type (might be due to invalid IL or missing references)
		//IL_185d: Expected O, but got Unknown
		//IL_17c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_128c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1291: Unknown result type (might be due to invalid IL or missing references)
		//IL_1296: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_12be: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_12cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12da: Unknown result type (might be due to invalid IL or missing references)
		//IL_12df: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1888: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1335: Unknown result type (might be due to invalid IL or missing references)
		//IL_133a: Unknown result type (might be due to invalid IL or missing references)
		//IL_133f: Unknown result type (might be due to invalid IL or missing references)
		//IL_134b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1352: Unknown result type (might be due to invalid IL or missing references)
		//IL_1359: Unknown result type (might be due to invalid IL or missing references)
		//IL_135e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1363: Unknown result type (might be due to invalid IL or missing references)
		//IL_1367: Unknown result type (might be due to invalid IL or missing references)
		//IL_136e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1373: Unknown result type (might be due to invalid IL or missing references)
		//IL_1378: Unknown result type (might be due to invalid IL or missing references)
		//IL_137c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1381: Unknown result type (might be due to invalid IL or missing references)
		//IL_1383: Unknown result type (might be due to invalid IL or missing references)
		//IL_1388: Unknown result type (might be due to invalid IL or missing references)
		//IL_1393: Unknown result type (might be due to invalid IL or missing references)
		//IL_1395: Unknown result type (might be due to invalid IL or missing references)
		//IL_1397: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ef: Expected O, but got Unknown
		//IL_190a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1911: Expected O, but got Unknown
		//IL_192c: Unknown result type (might be due to invalid IL or missing references)
		//IL_194c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1953: Expected O, but got Unknown
		//IL_195c: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ce: Expected O, but got Unknown
		//IL_19e0: Expected O, but got Unknown
		//IL_19e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad0: Expected O, but got Unknown
		//IL_1ad7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ade: Expected O, but got Unknown
		//IL_1ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae5: Expected O, but got Unknown
		//IL_1afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b19: Expected O, but got Unknown
		//IL_1b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b39: Expected O, but got Unknown
		//IL_1b51: Expected O, but got Unknown
		//IL_1b64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba6: Expected O, but got Unknown
		//IL_1bb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c36: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c41: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e1b: Expected O, but got Unknown
		//IL_1e26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e37: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ceb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db8: Unknown result type (might be due to invalid IL or missing references)
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
		InHeight.GetDouble(ref value4);
		double value5 = 0.0;
		InFillet.GetDoublePercentage(ref value5);
		double value6 = 0.0;
		InTaper.GetDoublePercentage(ref value6);
		double value7 = 0.0;
		InNudgeEdge.GetDouble(ref value7);
		double value8 = 0.0;
		InNudgeGem.GetDouble(ref value8);
		double value9 = 0.0;
		InVerticalLocation.GetDouble(ref value9);
		ValueListItem whichItem = ValueListItem.ProngPlacerStyleEnd9;
		InEndProngStyle.GetValueListItem(ref whichItem);
		bool value10 = true;
		InEnableEndProngRotationMirroring.GetBoolean(ref value10);
		bool value11 = false;
		InEnableEndProngEndControls.GetBoolean(ref value11);
		double value12 = 0.25;
		InSizePercentageStart.GetDoublePercentage(ref value12);
		double value13 = 0.0;
		InRotationStart.GetDouble(ref value13);
		double value14 = 45.0;
		InSpreadStart.GetDouble(ref value14);
		double value15 = 0.25;
		InSizePercentageEnd.GetDoublePercentage(ref value15);
		double value16 = 0.0;
		InRotationEnd.GetDouble(ref value16);
		double value17 = 45.0;
		InSpreadEnd.GetDouble(ref value17);
		InBlendTop.GetValue(out double value18);
		InBlendBot.GetValue(out double value19);
		InBlendEdge.GetValue(out double value20);
		InBottomArcPercentage.GetValue(out double value21);
		ValueListItem whichItem2 = ValueListItem.ProngPlacerSettingStyleProng;
		InSettingStyle.GetValueListItem(ref whichItem2);
		bool flag = false;
		switch (whichItem2)
		{
		case ValueListItem.ProngPlacerSettingStyleBar:
			flag = true;
			break;
		case ValueListItem.ProngPlacerSettingStyleAuto:
		{
			string gemShape = (cSDataArray.GetArrayItem(0) as CSGemData).GemShape;
			if (gemShape.ToLower() == "princess" || gemShape.ToLower() == "emerald" || gemShape.ToLower() == "emerald square" || gemShape.ToLower() == "emerald rotated 90")
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
		if (!value11)
		{
			value15 = value12;
			value16 = value13;
			if (value10)
			{
				value16 *= -1.0;
			}
			value17 = value14;
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
				double height2;
				string height = GetHeight(CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1), CSUtilGemRails.GetChannelGemData(cSDataArray, i), channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, out height2);
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
		InPercentToggle.GetValue(out bool value22);
		double value23 = 0.0;
		if (value22)
		{
			InProngPercentage.GetValue(out value23);
			value23 = 1.0 - value23;
		}
		bool flag2 = CSUtilGemRails.IsClosed(cSDataArray.Array);
		Point3d zMaxPoint;
		Circle val9 = default(Circle);
		Line val13 = default(Line);
		for (int j = 0; j < cSDataArray.GetArrayCount(); j++)
		{
			int num4 = j - 1;
			if (num4 < 0)
			{
				if (!flag2)
				{
					continue;
				}
				num4 = cSDataArray.GetArrayCount() - 1;
			}
			CSChannelGemData channelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, num4);
			CSChannelGemData channelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			if (channelGemData2 == null || channelGemData3 == null)
			{
				return UpdateError("Invalid gems");
			}
			CSDataHelper.SetSettingType(channelGemData3, flag ? GemSettingType.BR : GemSettingType.PR);
			OnCircle circleLeft;
			OnCircle circleRight;
			double height3;
			string circles = GetCircles(channelGemData2, channelGemData3, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value22, value23, out circleLeft, out circleRight, out height3);
			if (!string.IsNullOrEmpty(circles))
			{
				return UpdateError(circles);
			}
			if (_prongBase == ValueListItem.FollowLargestGem)
			{
				height3 = (value22 ? (num3 * value23) : num3);
			}
			On3dVector val7 = new On3dVector(circleLeft.Center());
			On3dVector val8 = new On3dVector(circleRight.Center());
			On3dPoint value24 = new On3dPoint((IOn3dVector)(object)((val7 + (IOn3dVector)(object)val8) / 2.0));
			On3dVector value25 = val8 - (IOn3dVector)(object)val7;
			if (CSApplicationHelper.Application.IsCamMode)
			{
				Plane worldZX = Plane.WorldZX;
				Point3d origin = Point3d.Origin;
				zMaxPoint = channelGemData3.ZMaxPoint;
				((Circle)(ref val9))._002Ector(worldZX, origin, ((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin));
				OnXform val10 = new OnXform();
				val10.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
				Vector3d zAxis = Vector3d.ZAxis;
				((Vector3d)(ref zAxis)).Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val10));
				((Vector3d)(ref zAxis)).Unitize();
				OnXform val11 = new OnXform();
				val11.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
				Vector3d zAxis2 = Vector3d.ZAxis;
				((Vector3d)(ref zAxis2)).Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val11));
				((Vector3d)(ref zAxis2)).Unitize();
				Vector3d destination = zAxis + zAxis2;
				((Vector3d)(ref destination)).Unitize();
				Transform val12 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value24), CSConvert.On3dVectorCopyToVector3d(value25), destination);
				Point3d origin2 = Point3d.Origin;
				((Point3d)(ref origin2)).Transform(val12);
				((Line)(ref val13))._002Ector(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
				((Line)(ref val13)).Transform(val12);
				Point3d val14 = ((Line)(ref val13)).ClosestPoint(((Circle)(ref val9)).ClosestPoint(((Line)(ref val13)).PointAt(1.0)), true);
				if (((Point3d)(ref val14)).DistanceTo(origin2) > height3 * 0.8)
				{
					height3 = ((Point3d)(ref val14)).DistanceTo(origin2) * 1.2;
				}
			}
			OnXform val15 = new OnXform();
			val15.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
			On3dVector val16 = new On3dVector(OnUtil.On_zaxis);
			val16.Transform((IOnXform)(object)val15);
			val16.Unitize();
			if (!flag)
			{
				double num5 = circleLeft.radius * 2.0;
				if (num5 > value2 && num5 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num5, height3, value5, value6, value, out var prong, out var prongBaseRail))
					{
						return UpdateError("Create prong cache failed");
					}
					prong.Transform((IOnXform)(object)val15);
					prongBaseRail.Transform((IOnXform)(object)val15);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val15.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val15.Convert(), applyTransform: false));
				}
			}
			OnXform val17 = new OnXform();
			val17.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
			On3dVector val18 = new On3dVector(OnUtil.On_zaxis);
			val18.Transform((IOnXform)(object)val17);
			val18.Unitize();
			if (!flag)
			{
				double num6 = circleRight.radius * 2.0;
				if (num6 > value2 && num6 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num6, height3, value5, value6, value, out var prong2, out var prongBaseRail2))
					{
						return UpdateError("Create prong cache failed");
					}
					prong2.Transform((IOnXform)(object)val17);
					prongBaseRail2.Transform((IOnXform)(object)val17);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val17.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val17.Convert(), applyTransform: false));
				}
			}
			On3dVector val19 = val16 + (IOn3dVector)(object)val18;
			val19.Unitize();
			if (flag)
			{
				val7 = new On3dVector(circleLeft.Center());
				val8 = new On3dVector(circleRight.Center());
				Brep geometry = MakeBar((val7 - (IOn3dVector)(object)val8).Length(), circleLeft.radius, circleRight.radius, height3, value6, value18, value19, value20, value21);
				value24 = new On3dPoint((IOn3dVector)(object)((val7 + (IOn3dVector)(object)val8) / 2.0));
				value25 = val8 - (IOn3dVector)(object)val7;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value24), CSConvert.On3dVectorCopyToVector3d(value25), CSConvert.On3dVectorCopyToVector3d(val19));
				concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry, transform));
			}
			if (val5 == OnUtil.On_UNSET_VECTOR)
			{
				val5 = val19;
			}
			val6 = val19;
		}
		if (!flag2)
		{
			switch (whichItem)
			{
			case ValueListItem.ProngPlacerStyleEnd4:
			case ValueListItem.ProngPlacerStyleEnd5:
			case ValueListItem.ProngPlacerStyleEnd6:
			case ValueListItem.ProngPlacerStyleEnd7:
			case ValueListItem.ProngPlacerStyleEnd8:
			case ValueListItem.ProngPlacerStyleEnd9:
			{
				CSChannelGemData cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Duplicate() as CSChannelGemData;
				if (cSDataArray.GetArrayCount() == 1)
				{
					cSChannelGemData.ApplyTransform(Transform.Rotation(RhinoMath.ToRadians(90.0), cSChannelGemData.Normal, cSChannelGemData.Center));
				}
				double num7 = cSChannelGemData.GemXSize * value12 + value7 + value8;
				double height5 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, num7, height5, value5, value6, value, out var prong4, out var prongBaseRail4))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint obj = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
				double num8 = (cSChannelGemData.GemXSize * value12 + value7) / 2.0;
				double num9 = value9 - cSChannelGemData.GemZMin;
				On3dVector val36;
				if (val5 == OnUtil.On_UNSET_VECTOR)
				{
					val36 = new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert());
					val36.Unitize();
				}
				else
				{
					OnXform val37 = new OnXform();
					val37.Mirror((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
					val36 = new On3dVector((IOn3dVector)(object)val5);
					val36.Transform((IOnXform)(object)val37);
					val36.Unitize();
				}
				On3dVector val38 = new On3dVector((IOn3dVector)(object)cSChannelGemData.LeftVector.Convert());
				val38.Unitize();
				On3dVector val39 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val36, (IOn3dVector)(object)val38);
				val39.Unitize();
				OnPlane val40 = new OnPlane((IOn3dPoint)(object)(obj + (IOn3dVector)(object)(val39 * num8) + (IOn3dVector)(object)(val36 * num9)), (IOn3dVector)(object)val38, (IOn3dVector)(object)val39);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				{
					OnXform val56 = new OnXform();
					val56.ChangeBasis((IOnPlane)(object)val40, OnUtil.On_xy_plane);
					OnXform val57 = new OnXform();
					val57.Rotation(value13 * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val58 = val57 * (IOnXform)(object)val56;
					prong4.Transform((IOnXform)(object)val58);
					prongBaseRail4.Transform((IOnXform)(object)val58);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val58.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail4).Convert(), val58.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num10 = value14 / 2.0;
					OnXform val41 = new OnXform();
					val41.ChangeBasis((IOnPlane)(object)val40, OnUtil.On_xy_plane);
					OnXform val42 = new OnXform();
					val42.Rotation((90.0 - num10) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val43 = new OnXform();
					val43.Rotation((value13 + num10) * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val44 = val43 * (IOnXform)(object)val41 * (IOnXform)(object)val42;
					if (!flag)
					{
						OnGeometry val45 = prong4.Duplicate();
						val45.Transform((IOnXform)(object)val44);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val45).Convert(), val44.Convert(), applyTransform: false));
						OnGeometry val46 = prongBaseRail4.Duplicate();
						val46.Transform((IOnXform)(object)val44);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val46).Convert(), val44.Convert(), applyTransform: false));
					}
					OnXform val47 = new OnXform();
					val47.Rotation((-90.0 + num10) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val48 = new OnXform();
					val48.Rotation((value13 - num10) * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val49 = val48 * (IOnXform)(object)val41 * (IOnXform)(object)val47;
					if (!flag)
					{
						OnGeometry val50 = prong4.Duplicate();
						val50.Transform((IOnXform)(object)val49);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val50).Convert(), val49.Convert(), applyTransform: false));
						OnGeometry val51 = prongBaseRail4.Duplicate();
						val51.Transform((IOnXform)(object)val49);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val51).Convert(), val49.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val52 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val53 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val54 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val55 = new On3dVector(0.0, 0.0, 1.0);
						val52.Transform((IOnXform)(object)val44);
						val54.Transform((IOnXform)(object)val44);
						val53.Transform((IOnXform)(object)val49);
						val55.Transform((IOnXform)(object)val49);
						Brep geometry3 = MakeBar((val52 - (IOn3dPoint)(object)val53).Length(), num7 / 2.0, num7 / 2.0, height5, value6, value18, value19, value20, value21);
						On3dPoint value32 = new On3dPoint((IOn3dPoint)(object)((val52 + (IOn3dPoint)(object)val53) / 2.0));
						On3dVector value33 = val53 - (IOn3dPoint)(object)val52;
						On3dVector value34 = val54 + (IOn3dVector)(object)val55;
						Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value32), CSConvert.On3dVectorCopyToVector3d(value33), CSConvert.On3dVectorCopyToVector3d(value34));
						concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry3, transform2));
					}
					break;
				}
				}
				break;
			}
			case ValueListItem.ProngPlacerStyleEnd10:
			{
				CSChannelGemData channelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, 0);
				CSChannelGemData channelGemData5 = CSUtilGemRails.GetChannelGemData(cSDataArray, 1);
				if (channelGemData4 == null || channelGemData5 == null)
				{
					return UpdateError("Invalid gems");
				}
				CSDataHelper.SetSettingType(channelGemData5, flag ? GemSettingType.BR : GemSettingType.PR);
				OnCircle circleLeft2;
				OnCircle circleRight2;
				double height4;
				string circles2 = GetCircles(channelGemData4, channelGemData5, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value22, value23, out circleLeft2, out circleRight2, out height4);
				if (!string.IsNullOrEmpty(circles2))
				{
					return UpdateError(circles2);
				}
				On3dVector val20 = new On3dVector(circleLeft2.Center());
				On3dVector val21 = new On3dVector(circleRight2.Center());
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height4 = (value22 ? (num3 * value23) : num3);
				}
				OnXform val22 = new OnXform();
				val22.ChangeBasis((IOnPlane)(object)circleLeft2.plane, OnUtil.On_xy_plane);
				On3dVector val23 = new On3dVector(OnUtil.On_zaxis);
				val23.Transform((IOnXform)(object)val22);
				val23.Unitize();
				OnXform val24 = new OnXform();
				val24.ChangeBasis((IOnPlane)(object)circleRight2.plane, OnUtil.On_xy_plane);
				On3dVector val25 = new On3dVector(OnUtil.On_zaxis);
				val25.Transform((IOnXform)(object)val24);
				val25.Unitize();
				On3dVector value26 = val23 + (IOn3dVector)(object)val25;
				if (CSApplicationHelper.Application.IsCamMode)
				{
					Plane worldZX2 = Plane.WorldZX;
					Point3d origin3 = Point3d.Origin;
					zMaxPoint = channelGemData5.ZMaxPoint;
					Circle val26 = default(Circle);
					((Circle)(ref val26))._002Ector(worldZX2, origin3, ((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin));
					On3dPoint value27 = new On3dPoint((IOn3dVector)(object)((val20 + (IOn3dVector)(object)val21) / 2.0));
					On3dVector value28 = val21 - (IOn3dVector)(object)val20;
					Transform val27 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value27), CSConvert.On3dVectorCopyToVector3d(value28), CSConvert.On3dVectorCopyToVector3d(value26));
					Point3d origin4 = Point3d.Origin;
					((Point3d)(ref origin4)).Transform(val27);
					Line val28 = default(Line);
					((Line)(ref val28))._002Ector(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
					((Line)(ref val28)).Transform(val27);
					Point3d val29 = ((Line)(ref val28)).ClosestPoint(((Circle)(ref val26)).ClosestPoint(((Line)(ref val28)).PointAt(1.0)), true);
					if (((Point3d)(ref val29)).DistanceTo(origin4) > height4 * 0.8)
					{
						height4 = ((Point3d)(ref val29)).DistanceTo(origin4) * 1.2;
					}
				}
				if (!flag)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft2.Diameter(), height4, value5, value6, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value29 = val21 - (IOn3dVector)(object)val20;
					Transform val30 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val20)), CSConvert.On3dVectorCopyToVector3d(value29), CSConvert.On3dVectorCopyToVector3d(value26));
					Vector3d val31 = channelGemData4.Center - channelGemData4.XMaxPoint;
					Transform val32 = Transform.Mirror(channelGemData4.Center, val31);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), val32 * val30));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight2.Diameter(), height4, value5, value6, value, out prong3, out prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					value29 = val20 - (IOn3dVector)(object)val21;
					val30 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val21)), CSConvert.On3dVectorCopyToVector3d(value29), CSConvert.On3dVectorCopyToVector3d(value26));
					val31 = channelGemData4.Center - channelGemData4.XMaxPoint;
					val32 = Transform.Mirror(channelGemData4.Center, val31);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), val32 * val30));
				}
				else
				{
					Brep geometry2 = MakeBar((val20 - (IOn3dVector)(object)val21).Length(), circleLeft2.radius, circleRight2.radius, height4, value6, value18, value19, value20, value21);
					On3dPoint value30 = new On3dPoint((IOn3dVector)(object)((val20 + (IOn3dVector)(object)val21) / 2.0));
					On3dVector value31 = val21 - (IOn3dVector)(object)val20;
					Transform val33 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value30), CSConvert.On3dVectorCopyToVector3d(value31), CSConvert.On3dVectorCopyToVector3d(value26));
					Vector3d val34 = channelGemData4.Center - channelGemData4.XMaxPoint;
					Transform val35 = Transform.Mirror(channelGemData4.Center, val34);
					concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry2, val35 * val33));
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
				CSChannelGemData cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.GetArrayCount() - 1).Duplicate() as CSChannelGemData;
				if (cSDataArray.GetArrayCount() == 1)
				{
					cSChannelGemData2.ApplyTransform(Transform.Rotation(RhinoMath.ToRadians(90.0), cSChannelGemData2.Normal, cSChannelGemData2.Center));
				}
				double num11 = cSChannelGemData2.GemXSize * value15 + value7 + value8;
				double height7 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData2.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, num11, height7, value5, value6, value, out var prong6, out var prongBaseRail6))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint obj2 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
				double num12 = (cSChannelGemData2.GemXSize * value15 + value7) / 2.0;
				double num13 = value9 - cSChannelGemData2.GemZMin;
				On3dVector val75;
				if (val6 == OnUtil.On_UNSET_VECTOR)
				{
					val75 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert());
					val75.Unitize();
				}
				else
				{
					OnXform val76 = new OnXform();
					val76.Mirror((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					val75 = new On3dVector((IOn3dVector)(object)val6);
					val75.Transform((IOnXform)(object)val76);
					val75.Unitize();
				}
				On3dVector val77 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.RightVector.Convert());
				val77.Unitize();
				On3dVector val78 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val75, (IOn3dVector)(object)val77);
				val78.Unitize();
				OnPlane val79 = new OnPlane((IOn3dPoint)(object)(obj2 + (IOn3dVector)(object)(val78 * num12) + (IOn3dVector)(object)(val75 * num13)), (IOn3dVector)(object)val77, (IOn3dVector)(object)val78);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd2:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd8:
				{
					OnXform val95 = new OnXform();
					val95.ChangeBasis((IOnPlane)(object)val79, OnUtil.On_xy_plane);
					OnXform val96 = new OnXform();
					val96.Rotation(value16 * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val97 = val96 * (IOnXform)(object)val95;
					prong6.Transform((IOnXform)(object)val97);
					prongBaseRail6.Transform((IOnXform)(object)val97);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val97.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail6).Convert(), val97.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd3:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num14 = value17 / 2.0;
					OnXform val80 = new OnXform();
					val80.ChangeBasis((IOnPlane)(object)val79, OnUtil.On_xy_plane);
					OnXform val81 = new OnXform();
					val81.Rotation((90.0 + num14) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val82 = new OnXform();
					val82.Rotation((value16 + num14) * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val83 = val82 * (IOnXform)(object)val80 * (IOnXform)(object)val81;
					if (!flag)
					{
						OnGeometry val84 = prong6.Duplicate();
						val84.Transform((IOnXform)(object)val83);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val84).Convert(), val83.Convert(), applyTransform: false));
						OnGeometry val85 = prongBaseRail6.Duplicate();
						val85.Transform((IOnXform)(object)val83);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val85).Convert(), val83.Convert(), applyTransform: false));
					}
					OnXform val86 = new OnXform();
					val86.Rotation((-90.0 + num14) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val87 = new OnXform();
					val87.Rotation((value16 - num14) * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val88 = val87 * (IOnXform)(object)val80 * (IOnXform)(object)val86;
					if (!flag)
					{
						OnGeometry val89 = prong6.Duplicate();
						val89.Transform((IOnXform)(object)val88);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val89).Convert(), val88.Convert(), applyTransform: false));
						OnGeometry val90 = prongBaseRail6.Duplicate();
						val90.Transform((IOnXform)(object)val88);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val90).Convert(), val88.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val91 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val92 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val93 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val94 = new On3dVector(0.0, 0.0, 1.0);
						val91.Transform((IOnXform)(object)val83);
						val93.Transform((IOnXform)(object)val83);
						val92.Transform((IOnXform)(object)val88);
						val94.Transform((IOnXform)(object)val88);
						Brep geometry5 = MakeBar((val91 - (IOn3dPoint)(object)val92).Length(), num11 / 2.0, num11 / 2.0, height7, value6, value18, value19, value20, value21);
						On3dPoint value41 = new On3dPoint((IOn3dPoint)(object)((val91 + (IOn3dPoint)(object)val92) / 2.0));
						On3dVector value42 = val92 - (IOn3dPoint)(object)val91;
						On3dVector value43 = val93 + (IOn3dVector)(object)val94;
						Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value41), CSConvert.On3dVectorCopyToVector3d(value42), CSConvert.On3dVectorCopyToVector3d(value43));
						concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry5, transform3));
					}
					break;
				}
				}
				break;
			}
			case ValueListItem.ProngPlacerStyleEnd10:
			{
				CSChannelGemData channelGemData6 = CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.GetArrayCount() - 2);
				CSChannelGemData channelGemData7 = CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.GetArrayCount() - 1);
				if (channelGemData6 == null || channelGemData7 == null)
				{
					return UpdateError("Invalid gems");
				}
				CSDataHelper.SetSettingType(channelGemData7, flag ? GemSettingType.BR : GemSettingType.PR);
				OnCircle circleLeft3;
				OnCircle circleRight3;
				double height6;
				string circles3 = GetCircles(channelGemData6, channelGemData7, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value22, value23, out circleLeft3, out circleRight3, out height6);
				if (!string.IsNullOrEmpty(circles3))
				{
					return UpdateError(circles3);
				}
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height6 = (value22 ? (num3 * value23) : num3);
				}
				On3dVector val59 = new On3dVector(circleLeft3.Center());
				On3dVector val60 = new On3dVector(circleRight3.Center());
				OnXform val61 = new OnXform();
				val61.ChangeBasis((IOnPlane)(object)circleLeft3.plane, OnUtil.On_xy_plane);
				On3dVector val62 = new On3dVector(OnUtil.On_zaxis);
				val62.Transform((IOnXform)(object)val61);
				val62.Unitize();
				OnXform val63 = new OnXform();
				val63.ChangeBasis((IOnPlane)(object)circleRight3.plane, OnUtil.On_xy_plane);
				On3dVector val64 = new On3dVector(OnUtil.On_zaxis);
				val64.Transform((IOnXform)(object)val63);
				val64.Unitize();
				On3dVector value35 = val62 + (IOn3dVector)(object)val64;
				if (CSApplicationHelper.Application.IsCamMode)
				{
					Plane worldZX3 = Plane.WorldZX;
					Point3d origin5 = Point3d.Origin;
					zMaxPoint = channelGemData7.ZMaxPoint;
					Circle val65 = default(Circle);
					((Circle)(ref val65))._002Ector(worldZX3, origin5, ((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin));
					On3dPoint value36 = new On3dPoint((IOn3dVector)(object)((val59 + (IOn3dVector)(object)val60) / 2.0));
					On3dVector value37 = val60 - (IOn3dVector)(object)val59;
					Transform val66 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value36), CSConvert.On3dVectorCopyToVector3d(value37), CSConvert.On3dVectorCopyToVector3d(value35));
					Point3d origin6 = Point3d.Origin;
					((Point3d)(ref origin6)).Transform(val66);
					Line val67 = default(Line);
					((Line)(ref val67))._002Ector(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
					((Line)(ref val67)).Transform(val66);
					Point3d val68 = ((Line)(ref val67)).ClosestPoint(((Circle)(ref val65)).ClosestPoint(((Line)(ref val67)).PointAt(1.0)), true);
					if (((Point3d)(ref val68)).DistanceTo(origin6) > height6 * 0.8)
					{
						height6 = ((Point3d)(ref val68)).DistanceTo(origin6) * 1.2;
					}
				}
				if (!flag)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft3.Diameter(), height6, value5, value6, value, out var prong5, out var prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value38 = val60 - (IOn3dVector)(object)val59;
					Transform val69 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val59)), CSConvert.On3dVectorCopyToVector3d(value38), CSConvert.On3dVectorCopyToVector3d(value35));
					Vector3d val70 = channelGemData7.Center - channelGemData7.XMaxPoint;
					Transform val71 = Transform.Mirror(channelGemData7.Center, val70);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), val71 * val69));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight3.Diameter(), height6, value5, value6, value, out prong5, out prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					value38 = val59 - (IOn3dVector)(object)val60;
					val69 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val60)), CSConvert.On3dVectorCopyToVector3d(value38), CSConvert.On3dVectorCopyToVector3d(value35));
					val70 = channelGemData7.Center - channelGemData7.XMaxPoint;
					val71 = Transform.Mirror(channelGemData7.Center, val70);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), val71 * val69));
				}
				else
				{
					Brep geometry4 = MakeBar((val59 - (IOn3dVector)(object)val60).Length(), circleLeft3.radius, circleRight3.radius, height6, value6, value18, value19, value20, value21);
					On3dPoint value39 = new On3dPoint((IOn3dVector)(object)((val59 + (IOn3dVector)(object)val60) / 2.0));
					On3dVector value40 = val60 - (IOn3dVector)(object)val59;
					Transform val72 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value39), CSConvert.On3dVectorCopyToVector3d(value40), CSConvert.On3dVectorCopyToVector3d(value35));
					Vector3d val73 = channelGemData7.Center - channelGemData7.XMaxPoint;
					Transform val74 = Transform.Mirror(channelGemData7.Center, val73);
					concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry4, val74 * val72));
				}
				break;
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
		Transform val4 = Transform.Rotation(num, -Vector3d.ZAxis, val);
		((Point3d)(ref val3)).Transform(val4);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(centerLength / 2.0, radius2, 0.0);
		Transform val6 = Transform.Rotation(num, -Vector3d.ZAxis, val2);
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
