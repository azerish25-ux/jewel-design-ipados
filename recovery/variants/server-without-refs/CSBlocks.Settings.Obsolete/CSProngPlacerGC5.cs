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
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ProngPlacerGC_V5", "{4A870A35-7F91-465A-B64B-9CE127357A06}")]
public sealed class CSProngPlacerGC5 : CSBlock
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

	public readonly CSOutputChannel OutProngs;

	public readonly CSOutputChannel OutProfiles;

	private readonly CSProngCache _prongCache = new CSProngCache();

	private ValueListItem _prongBase = ValueListItem.FollowCulet;

	public CSProngPlacerGC5(string identifier)
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
		InProngPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProfiles = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Expected O, but got Unknown
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Expected O, but got Unknown
		//IL_09be: Expected O, but got Unknown
		//IL_0a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Expected O, but got Unknown
		//IL_0a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3d: Expected O, but got Unknown
		//IL_09f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a04: Expected O, but got Unknown
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a61: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa3: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad1: Expected O, but got Unknown
		//IL_0ae0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b11: Expected O, but got Unknown
		//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8d: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be3: Expected O, but got Unknown
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Expected O, but got Unknown
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Expected O, but got Unknown
		//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0822: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Expected O, but got Unknown
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Expected O, but got Unknown
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3b: Expected O, but got Unknown
		//IL_0d56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5d: Expected O, but got Unknown
		//IL_0d78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9f: Expected O, but got Unknown
		//IL_0da8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e20: Expected O, but got Unknown
		//IL_0e32: Expected O, but got Unknown
		//IL_0e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_12dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1301: Expected O, but got Unknown
		//IL_1301: Expected O, but got Unknown
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1351: Unknown result type (might be due to invalid IL or missing references)
		//IL_1358: Expected O, but got Unknown
		//IL_135c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1368: Unknown result type (might be due to invalid IL or missing references)
		//IL_1379: Unknown result type (might be due to invalid IL or missing references)
		//IL_1380: Expected O, but got Unknown
		//IL_1336: Unknown result type (might be due to invalid IL or missing references)
		//IL_1340: Unknown result type (might be due to invalid IL or missing references)
		//IL_1347: Expected O, but got Unknown
		//IL_1393: Unknown result type (might be due to invalid IL or missing references)
		//IL_139d: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a4: Expected O, but got Unknown
		//IL_13df: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e6: Expected O, but got Unknown
		//IL_1415: Unknown result type (might be due to invalid IL or missing references)
		//IL_141c: Expected O, but got Unknown
		//IL_142b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1430: Unknown result type (might be due to invalid IL or missing references)
		//IL_143a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1446: Unknown result type (might be due to invalid IL or missing references)
		//IL_145c: Expected O, but got Unknown
		//IL_147d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1498: Unknown result type (might be due to invalid IL or missing references)
		//IL_14bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c2: Expected O, but got Unknown
		//IL_14d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d8: Expected O, but got Unknown
		//IL_14fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_150c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1518: Unknown result type (might be due to invalid IL or missing references)
		//IL_152e: Expected O, but got Unknown
		//IL_0f88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8f: Expected O, but got Unknown
		//IL_0fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc3: Expected O, but got Unknown
		//IL_0fdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe3: Expected O, but got Unknown
		//IL_0ffb: Expected O, but got Unknown
		//IL_1004: Unknown result type (might be due to invalid IL or missing references)
		//IL_100b: Expected O, but got Unknown
		//IL_1012: Unknown result type (might be due to invalid IL or missing references)
		//IL_1019: Expected O, but got Unknown
		//IL_0f31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1597: Unknown result type (might be due to invalid IL or missing references)
		//IL_159e: Expected O, but got Unknown
		//IL_15c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15de: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f4: Expected O, but got Unknown
		//IL_1559: Unknown result type (might be due to invalid IL or missing references)
		//IL_1587: Unknown result type (might be due to invalid IL or missing references)
		//IL_11cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d2: Expected O, but got Unknown
		//IL_11dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1201: Unknown result type (might be due to invalid IL or missing references)
		//IL_1206: Unknown result type (might be due to invalid IL or missing references)
		//IL_120a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1211: Unknown result type (might be due to invalid IL or missing references)
		//IL_1216: Unknown result type (might be due to invalid IL or missing references)
		//IL_121b: Unknown result type (might be due to invalid IL or missing references)
		//IL_121f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1224: Unknown result type (might be due to invalid IL or missing references)
		//IL_1226: Unknown result type (might be due to invalid IL or missing references)
		//IL_122b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1231: Unknown result type (might be due to invalid IL or missing references)
		//IL_1233: Unknown result type (might be due to invalid IL or missing references)
		//IL_1235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_161f: Unknown result type (might be due to invalid IL or missing references)
		//IL_164d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1057: Unknown result type (might be due to invalid IL or missing references)
		//IL_105c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1061: Unknown result type (might be due to invalid IL or missing references)
		//IL_106d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1074: Unknown result type (might be due to invalid IL or missing references)
		//IL_107b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1080: Unknown result type (might be due to invalid IL or missing references)
		//IL_1085: Unknown result type (might be due to invalid IL or missing references)
		//IL_1089: Unknown result type (might be due to invalid IL or missing references)
		//IL_1090: Unknown result type (might be due to invalid IL or missing references)
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_109a: Unknown result type (might be due to invalid IL or missing references)
		//IL_109e: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_167f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1686: Expected O, but got Unknown
		//IL_16a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a8: Expected O, but got Unknown
		//IL_16c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ea: Expected O, but got Unknown
		//IL_16f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1764: Unknown result type (might be due to invalid IL or missing references)
		//IL_176b: Expected O, but got Unknown
		//IL_177d: Expected O, but got Unknown
		//IL_177f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1784: Unknown result type (might be due to invalid IL or missing references)
		//IL_1789: Unknown result type (might be due to invalid IL or missing references)
		//IL_1790: Unknown result type (might be due to invalid IL or missing references)
		//IL_1797: Unknown result type (might be due to invalid IL or missing references)
		//IL_179e: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1100: Unknown result type (might be due to invalid IL or missing references)
		//IL_1105: Unknown result type (might be due to invalid IL or missing references)
		//IL_110a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1116: Unknown result type (might be due to invalid IL or missing references)
		//IL_111d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1124: Unknown result type (might be due to invalid IL or missing references)
		//IL_1129: Unknown result type (might be due to invalid IL or missing references)
		//IL_112e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1132: Unknown result type (might be due to invalid IL or missing references)
		//IL_1139: Unknown result type (might be due to invalid IL or missing references)
		//IL_113e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1143: Unknown result type (might be due to invalid IL or missing references)
		//IL_1147: Unknown result type (might be due to invalid IL or missing references)
		//IL_114c: Unknown result type (might be due to invalid IL or missing references)
		//IL_114e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1153: Unknown result type (might be due to invalid IL or missing references)
		//IL_115e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1160: Unknown result type (might be due to invalid IL or missing references)
		//IL_1162: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e8: Expected O, but got Unknown
		//IL_1901: Unknown result type (might be due to invalid IL or missing references)
		//IL_1906: Unknown result type (might be due to invalid IL or missing references)
		//IL_190e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1915: Unknown result type (might be due to invalid IL or missing references)
		//IL_191c: Expected O, but got Unknown
		//IL_1935: Unknown result type (might be due to invalid IL or missing references)
		//IL_193c: Expected O, but got Unknown
		//IL_1954: Expected O, but got Unknown
		//IL_195d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1964: Expected O, but got Unknown
		//IL_196b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1972: Expected O, but got Unknown
		//IL_188a: Unknown result type (might be due to invalid IL or missing references)
		//IL_188f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1893: Unknown result type (might be due to invalid IL or missing references)
		//IL_1898: Unknown result type (might be due to invalid IL or missing references)
		//IL_189c: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2b: Expected O, but got Unknown
		//IL_1b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b47: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19de: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abb: Unknown result type (might be due to invalid IL or missing references)
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
		InPercentToggle.GetValue(out bool value18);
		double value19 = 0.0;
		if (value18)
		{
			InProngPercentage.GetValue(out value19);
			value19 = 1.0 - value19;
		}
		bool flag2 = CSUtilGemRails.IsClosed(cSDataArray.Array);
		Point3d zMaxPoint;
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
			string circles = GetCircles(channelGemData2, channelGemData3, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value18, value19, out circleLeft, out circleRight, out height3);
			if (!string.IsNullOrEmpty(circles))
			{
				return UpdateError(circles);
			}
			if (_prongBase == ValueListItem.FollowLargestGem)
			{
				height3 = (value18 ? (num3 * value19) : num3);
			}
			if (CSApplicationHelper.Application.IsCamMode)
			{
				double num5 = 4.0 * height3 / 5.0;
				Point3d val7 = CSConvert.On3dPointCopyToPoint3d(circleLeft.Center());
				zMaxPoint = channelGemData3.ZMaxPoint;
				if (((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin) > ((Point3d)(ref val7)).DistanceTo(Point3d.Origin) + num5)
				{
					double num6 = height3;
					zMaxPoint = channelGemData3.ZMaxPoint;
					height3 = num6 + (((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin) - (((Point3d)(ref val7)).DistanceTo(Point3d.Origin) + num5));
				}
			}
			OnXform val8 = new OnXform();
			val8.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
			On3dVector val9 = new On3dVector(OnUtil.On_zaxis);
			val9.Transform((IOnXform)(object)val8);
			val9.Unitize();
			if (!flag)
			{
				double num7 = circleLeft.radius * 2.0;
				if (num7 > value2 && num7 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num7, height3, value5, value6, value, out var prong, out var prongBaseRail))
					{
						return UpdateError("Create prong cache failed");
					}
					prong.Transform((IOnXform)(object)val8);
					prongBaseRail.Transform((IOnXform)(object)val8);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val8.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val8.Convert(), applyTransform: false));
				}
			}
			OnXform val10 = new OnXform();
			val10.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
			On3dVector val11 = new On3dVector(OnUtil.On_zaxis);
			val11.Transform((IOnXform)(object)val10);
			val11.Unitize();
			if (!flag)
			{
				double num8 = circleRight.radius * 2.0;
				if (num8 > value2 && num8 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num8, height3, value5, value6, value, out var prong2, out var prongBaseRail2))
					{
						return UpdateError("Create prong cache failed");
					}
					prong2.Transform((IOnXform)(object)val10);
					prongBaseRail2.Transform((IOnXform)(object)val10);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val10.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val10.Convert(), applyTransform: false));
				}
			}
			On3dVector val12 = val9 + (IOn3dVector)(object)val11;
			val12.Unitize();
			if (flag)
			{
				On3dVector val13 = new On3dVector(circleLeft.Center());
				On3dVector val14 = new On3dVector(circleRight.Center());
				Brep geometry = MakeBar((val13 - (IOn3dVector)(object)val14).Length(), circleLeft.radius, circleRight.radius, height3, Math.Min(circleLeft.radius, circleRight.radius) * value5, value6);
				On3dPoint value20 = new On3dPoint((IOn3dVector)(object)((val13 + (IOn3dVector)(object)val14) / 2.0));
				On3dVector value21 = val14 - (IOn3dVector)(object)val13;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value20), CSConvert.On3dVectorCopyToVector3d(value21), CSConvert.On3dVectorCopyToVector3d(val12));
				concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry, transform));
			}
			if (val5 == OnUtil.On_UNSET_VECTOR)
			{
				val5 = val12;
			}
			val6 = val12;
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
				CSChannelGemData channelGemData6 = CSUtilGemRails.GetChannelGemData(cSDataArray, 0);
				double num11 = channelGemData6.GemXSize * value12 + value7 + value8;
				double height5 = ((_prongBase == ValueListItem.FollowCulet) ? (channelGemData6.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, num11, height5, value5, value6, value, out var prong4, out var prongBaseRail4))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint obj = new On3dPoint((IOn3dPoint)(object)channelGemData6.Center.Convert()) - (IOn3dVector)new On3dVector((IOn3dVector)(object)channelGemData6.ForwardVectorInGemPlane.Convert());
				double num12 = (channelGemData6.GemXSize * value12 + value7) / 2.0;
				double num13 = value9 - channelGemData6.GemZMin;
				On3dVector val28;
				if (val5 == OnUtil.On_UNSET_VECTOR)
				{
					val28 = new On3dVector((IOn3dVector)(object)channelGemData6.Normal.Convert());
					val28.Unitize();
				}
				else
				{
					OnXform val29 = new OnXform();
					val29.Mirror((IOn3dPoint)(object)channelGemData6.Center.Convert(), (IOn3dVector)(object)channelGemData6.ForwardVectorInGemPlane.Convert());
					val28 = new On3dVector((IOn3dVector)(object)val5);
					val28.Transform((IOnXform)(object)val29);
					val28.Unitize();
				}
				On3dVector val30 = new On3dVector((IOn3dVector)(object)channelGemData6.LeftVector.Convert());
				val30.Unitize();
				On3dVector val31 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val28, (IOn3dVector)(object)val30);
				val31.Unitize();
				OnPlane val32 = new OnPlane((IOn3dPoint)(object)(obj + (IOn3dVector)(object)(val31 * num12) + (IOn3dVector)(object)(val28 * num13)), (IOn3dVector)(object)val30, (IOn3dVector)(object)val31);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				{
					OnXform val48 = new OnXform();
					val48.ChangeBasis((IOnPlane)(object)val32, OnUtil.On_xy_plane);
					OnXform val49 = new OnXform();
					val49.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13), (IOn3dVector)(object)channelGemData6.Normal.Convert(), (IOn3dPoint)(object)channelGemData6.Center.Convert());
					OnXform val50 = val49 * (IOnXform)(object)val48;
					prong4.Transform((IOnXform)(object)val50);
					prongBaseRail4.Transform((IOnXform)(object)val50);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val50.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail4).Convert(), val50.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num14 = value14 / 2.0;
					OnXform val33 = new OnXform();
					val33.ChangeBasis((IOnPlane)(object)val32, OnUtil.On_xy_plane);
					OnXform val34 = new OnXform();
					val34.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num14), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val35 = new OnXform();
					val35.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 + num14), (IOn3dVector)(object)channelGemData6.Normal.Convert(), (IOn3dPoint)(object)channelGemData6.Center.Convert());
					OnXform val36 = val35 * (IOnXform)(object)val33 * (IOnXform)(object)val34;
					if (!flag)
					{
						OnGeometry val37 = prong4.Duplicate();
						val37.Transform((IOnXform)(object)val36);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val37).Convert(), val36.Convert(), applyTransform: false));
						OnGeometry val38 = prongBaseRail4.Duplicate();
						val38.Transform((IOnXform)(object)val36);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val38).Convert(), val36.Convert(), applyTransform: false));
					}
					OnXform val39 = new OnXform();
					val39.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num14), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val40 = new OnXform();
					val40.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 - num14), (IOn3dVector)(object)channelGemData6.Normal.Convert(), (IOn3dPoint)(object)channelGemData6.Center.Convert());
					OnXform val41 = val40 * (IOnXform)(object)val33 * (IOnXform)(object)val39;
					if (!flag)
					{
						OnGeometry val42 = prong4.Duplicate();
						val42.Transform((IOnXform)(object)val41);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val42).Convert(), val41.Convert(), applyTransform: false));
						OnGeometry val43 = prongBaseRail4.Duplicate();
						val43.Transform((IOnXform)(object)val41);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val43).Convert(), val41.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val44 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val45 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val46 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val47 = new On3dVector(0.0, 0.0, 1.0);
						val44.Transform((IOnXform)(object)val36);
						val46.Transform((IOnXform)(object)val36);
						val45.Transform((IOnXform)(object)val41);
						val47.Transform((IOnXform)(object)val41);
						Brep geometry3 = MakeBar((val44 - (IOn3dPoint)(object)val45).Length(), num11 / 2.0, num11 / 2.0, height5, num11 / 2.0 * value5, value6);
						On3dPoint value26 = new On3dPoint((IOn3dPoint)(object)((val44 + (IOn3dPoint)(object)val45) / 2.0));
						On3dVector value27 = val45 - (IOn3dPoint)(object)val44;
						On3dVector value28 = val46 + (IOn3dVector)(object)val47;
						Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value26), CSConvert.On3dVectorCopyToVector3d(value27), CSConvert.On3dVectorCopyToVector3d(value28));
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
				string circles2 = GetCircles(channelGemData4, channelGemData5, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value18, value19, out circleLeft2, out circleRight2, out height4);
				if (!string.IsNullOrEmpty(circles2))
				{
					return UpdateError(circles2);
				}
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height4 = (value18 ? (num3 * value19) : num3);
				}
				if (CSApplicationHelper.Application.IsCamMode)
				{
					double num9 = 4.0 * height4 / 5.0;
					Point3d val15 = CSConvert.On3dPointCopyToPoint3d(circleLeft2.Center());
					zMaxPoint = channelGemData5.ZMaxPoint;
					if (((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin) > ((Point3d)(ref val15)).DistanceTo(Point3d.Origin) + num9)
					{
						double num10 = height4;
						zMaxPoint = channelGemData5.ZMaxPoint;
						height4 = num10 + (((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin) - (((Point3d)(ref val15)).DistanceTo(Point3d.Origin) + num9));
					}
				}
				OnXform val16 = new OnXform();
				val16.ChangeBasis((IOnPlane)(object)circleLeft2.plane, OnUtil.On_xy_plane);
				On3dVector val17 = new On3dVector(OnUtil.On_zaxis);
				val17.Transform((IOnXform)(object)val16);
				val17.Unitize();
				OnXform val18 = new OnXform();
				val18.ChangeBasis((IOnPlane)(object)circleRight2.plane, OnUtil.On_xy_plane);
				On3dVector val19 = new On3dVector(OnUtil.On_zaxis);
				val19.Transform((IOnXform)(object)val18);
				val19.Unitize();
				On3dVector value22 = val17 + (IOn3dVector)(object)val19;
				On3dVector val20 = new On3dVector(circleLeft2.Center());
				On3dVector val21 = new On3dVector(circleRight2.Center());
				if (!flag)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft2.Diameter(), height4, value5, value6, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value23 = val21 - (IOn3dVector)(object)val20;
					Transform val22 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val20)), CSConvert.On3dVectorCopyToVector3d(value23), CSConvert.On3dVectorCopyToVector3d(value22));
					Vector3d val23 = channelGemData4.Center - channelGemData4.XMaxPoint;
					Transform val24 = Transform.Mirror(channelGemData4.Center, val23);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), val24 * val22));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight2.Diameter(), height4, value5, value6, value, out prong3, out prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					value23 = val20 - (IOn3dVector)(object)val21;
					val22 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val21)), CSConvert.On3dVectorCopyToVector3d(value23), CSConvert.On3dVectorCopyToVector3d(value22));
					val23 = channelGemData4.Center - channelGemData4.XMaxPoint;
					val24 = Transform.Mirror(channelGemData4.Center, val23);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), val24 * val22));
				}
				else
				{
					Brep geometry2 = MakeBar((val20 - (IOn3dVector)(object)val21).Length(), circleLeft2.radius, circleRight2.radius, height4, Math.Min(circleLeft2.radius, circleRight2.radius) * value5, value6);
					On3dPoint value24 = new On3dPoint((IOn3dVector)(object)((val20 + (IOn3dVector)(object)val21) / 2.0));
					On3dVector value25 = val21 - (IOn3dVector)(object)val20;
					Transform val25 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value24), CSConvert.On3dVectorCopyToVector3d(value25), CSConvert.On3dVectorCopyToVector3d(value22));
					Vector3d val26 = channelGemData4.Center - channelGemData4.XMaxPoint;
					Transform val27 = Transform.Mirror(channelGemData4.Center, val26);
					concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry2, val27 * val25));
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
				CSChannelGemData channelGemData9 = CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.GetArrayCount() - 1);
				double num17 = channelGemData9.GemXSize * value15 + value7 + value8;
				double height7 = ((_prongBase == ValueListItem.FollowCulet) ? (channelGemData9.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, num17, height7, value5, value6, value, out var prong6, out var prongBaseRail6))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint obj2 = new On3dPoint((IOn3dPoint)(object)channelGemData9.Center.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)channelGemData9.ForwardVectorInGemPlane.Convert());
				double num18 = (channelGemData9.GemXSize * value15 + value7) / 2.0;
				double num19 = value9 - channelGemData9.GemZMin;
				On3dVector val64;
				if (val6 == OnUtil.On_UNSET_VECTOR)
				{
					val64 = new On3dVector((IOn3dVector)(object)channelGemData9.Normal.Convert());
					val64.Unitize();
				}
				else
				{
					OnXform val65 = new OnXform();
					val65.Mirror((IOn3dPoint)(object)channelGemData9.Center.Convert(), (IOn3dVector)(object)channelGemData9.ForwardVectorInGemPlane.Convert());
					val64 = new On3dVector((IOn3dVector)(object)val6);
					val64.Transform((IOnXform)(object)val65);
					val64.Unitize();
				}
				On3dVector val66 = new On3dVector((IOn3dVector)(object)channelGemData9.RightVector.Convert());
				val66.Unitize();
				On3dVector val67 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val64, (IOn3dVector)(object)val66);
				val67.Unitize();
				OnPlane val68 = new OnPlane((IOn3dPoint)(object)(obj2 + (IOn3dVector)(object)(val67 * num18) + (IOn3dVector)(object)(val64 * num19)), (IOn3dVector)(object)val66, (IOn3dVector)(object)val67);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd2:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd8:
				{
					OnXform val84 = new OnXform();
					val84.ChangeBasis((IOnPlane)(object)val68, OnUtil.On_xy_plane);
					OnXform val85 = new OnXform();
					val85.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16), (IOn3dVector)(object)channelGemData9.Normal.Convert(), (IOn3dPoint)(object)channelGemData9.Center.Convert());
					OnXform val86 = val85 * (IOnXform)(object)val84;
					prong6.Transform((IOnXform)(object)val86);
					prongBaseRail6.Transform((IOnXform)(object)val86);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val86.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail6).Convert(), val86.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd3:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num20 = value17 / 2.0;
					OnXform val69 = new OnXform();
					val69.ChangeBasis((IOnPlane)(object)val68, OnUtil.On_xy_plane);
					OnXform val70 = new OnXform();
					val70.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num20), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val71 = new OnXform();
					val71.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 + num20), (IOn3dVector)(object)channelGemData9.Normal.Convert(), (IOn3dPoint)(object)channelGemData9.Center.Convert());
					OnXform val72 = val71 * (IOnXform)(object)val69 * (IOnXform)(object)val70;
					if (!flag)
					{
						OnGeometry val73 = prong6.Duplicate();
						val73.Transform((IOnXform)(object)val72);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val73).Convert(), val72.Convert(), applyTransform: false));
						OnGeometry val74 = prongBaseRail6.Duplicate();
						val74.Transform((IOnXform)(object)val72);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val74).Convert(), val72.Convert(), applyTransform: false));
					}
					OnXform val75 = new OnXform();
					val75.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num20), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val76 = new OnXform();
					val76.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 - num20), (IOn3dVector)(object)channelGemData9.Normal.Convert(), (IOn3dPoint)(object)channelGemData9.Center.Convert());
					OnXform val77 = val76 * (IOnXform)(object)val69 * (IOnXform)(object)val75;
					if (!flag)
					{
						OnGeometry val78 = prong6.Duplicate();
						val78.Transform((IOnXform)(object)val77);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val78).Convert(), val77.Convert(), applyTransform: false));
						OnGeometry val79 = prongBaseRail6.Duplicate();
						val79.Transform((IOnXform)(object)val77);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val79).Convert(), val77.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val80 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val81 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val82 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val83 = new On3dVector(0.0, 0.0, 1.0);
						val80.Transform((IOnXform)(object)val72);
						val82.Transform((IOnXform)(object)val72);
						val81.Transform((IOnXform)(object)val77);
						val83.Transform((IOnXform)(object)val77);
						Brep geometry5 = MakeBar((val80 - (IOn3dPoint)(object)val81).Length(), num17 / 2.0, num17 / 2.0, height7, num17 / 2.0 * value5, value6);
						On3dPoint value33 = new On3dPoint((IOn3dPoint)(object)((val80 + (IOn3dPoint)(object)val81) / 2.0));
						On3dVector value34 = val81 - (IOn3dPoint)(object)val80;
						On3dVector value35 = val82 + (IOn3dVector)(object)val83;
						Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value33), CSConvert.On3dVectorCopyToVector3d(value34), CSConvert.On3dVectorCopyToVector3d(value35));
						concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry5, transform3));
					}
					break;
				}
				}
				break;
			}
			case ValueListItem.ProngPlacerStyleEnd10:
			{
				CSChannelGemData channelGemData7 = CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.GetArrayCount() - 2);
				CSChannelGemData channelGemData8 = CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.GetArrayCount() - 1);
				if (channelGemData7 == null || channelGemData8 == null)
				{
					return UpdateError("Invalid gems");
				}
				CSDataHelper.SetSettingType(channelGemData8, flag ? GemSettingType.BR : GemSettingType.PR);
				OnCircle circleLeft3;
				OnCircle circleRight3;
				double height6;
				string circles3 = GetCircles(channelGemData7, channelGemData8, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value18, value19, out circleLeft3, out circleRight3, out height6);
				if (!string.IsNullOrEmpty(circles3))
				{
					return UpdateError(circles3);
				}
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height6 = (value18 ? (num3 * value19) : num3);
				}
				if (CSApplicationHelper.Application.IsCamMode)
				{
					double num15 = 4.0 * height6 / 5.0;
					Point3d val51 = CSConvert.On3dPointCopyToPoint3d(circleLeft3.Center());
					zMaxPoint = channelGemData8.ZMaxPoint;
					if (((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin) > ((Point3d)(ref val51)).DistanceTo(Point3d.Origin) + num15)
					{
						double num16 = height6;
						zMaxPoint = channelGemData8.ZMaxPoint;
						height6 = num16 + (((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin) - (((Point3d)(ref val51)).DistanceTo(Point3d.Origin) + num15));
					}
				}
				OnXform val52 = new OnXform();
				val52.ChangeBasis((IOnPlane)(object)circleLeft3.plane, OnUtil.On_xy_plane);
				On3dVector val53 = new On3dVector(OnUtil.On_zaxis);
				val53.Transform((IOnXform)(object)val52);
				val53.Unitize();
				OnXform val54 = new OnXform();
				val54.ChangeBasis((IOnPlane)(object)circleRight3.plane, OnUtil.On_xy_plane);
				On3dVector val55 = new On3dVector(OnUtil.On_zaxis);
				val55.Transform((IOnXform)(object)val54);
				val55.Unitize();
				On3dVector value29 = val53 + (IOn3dVector)(object)val55;
				On3dVector val56 = new On3dVector(circleLeft3.Center());
				On3dVector val57 = new On3dVector(circleRight3.Center());
				if (!flag)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft3.Diameter(), height6, value5, value6, value, out var prong5, out var prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value30 = val57 - (IOn3dVector)(object)val56;
					Transform val58 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val56)), CSConvert.On3dVectorCopyToVector3d(value30), CSConvert.On3dVectorCopyToVector3d(value29));
					Vector3d val59 = channelGemData8.Center - channelGemData8.XMaxPoint;
					Transform val60 = Transform.Mirror(channelGemData8.Center, val59);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), val60 * val58));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight3.Diameter(), height6, value5, value6, value, out prong5, out prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					value30 = val56 - (IOn3dVector)(object)val57;
					val58 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val57)), CSConvert.On3dVectorCopyToVector3d(value30), CSConvert.On3dVectorCopyToVector3d(value29));
					val59 = channelGemData8.Center - channelGemData8.XMaxPoint;
					val60 = Transform.Mirror(channelGemData8.Center, val59);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), val60 * val58));
				}
				else
				{
					Brep geometry4 = MakeBar((val56 - (IOn3dVector)(object)val57).Length(), circleLeft3.radius, circleRight3.radius, height6, Math.Min(circleLeft3.radius, circleRight3.radius) * value5, value6);
					On3dPoint value31 = new On3dPoint((IOn3dVector)(object)((val56 + (IOn3dVector)(object)val57) / 2.0));
					On3dVector value32 = val57 - (IOn3dVector)(object)val56;
					Transform val61 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value31), CSConvert.On3dVectorCopyToVector3d(value32), CSConvert.On3dVectorCopyToVector3d(value29));
					Vector3d val62 = channelGemData8.Center - channelGemData8.XMaxPoint;
					Transform val63 = Transform.Mirror(channelGemData8.Center, val62);
					concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry4, val63 * val61));
				}
				break;
			}
			}
		}
		OutProngs.SetData(new CSDataArray(concurrentBag));
		OutProfiles.SetData(new CSDataArray(concurrentBag2));
		return base.Update();
	}

	private static NurbsCurve BuildProngProfile(double radius, double height, double filletPer, double taperPer)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Expected O, but got Unknown
		if (filletPer > 0.99)
		{
			filletPer = 0.99;
		}
		if (filletPer < 0.01)
		{
			filletPer = 0.01;
		}
		double num = radius * (1.0 + taperPer);
		double num2 = radius * filletPer;
		if (num2 > height)
		{
			height = num2 + 0.01;
		}
		double num3 = height - num2;
		NurbsCurve val = new NurbsCurve(3, true, 3, 9);
		val.Points[0] = new ControlPoint(0.0, 0.0, 0.0, 1.0);
		val.Points[1] = new ControlPoint(num / 2.0, 0.0, 0.0, 1.0);
		val.Points[2] = new ControlPoint(num, 0.0, 0.0, 1.0);
		val.Points[3] = new ControlPoint((num + radius) / 2.0, 0.0, num3 / 2.0, 1.0);
		val.Points[4] = new ControlPoint(radius, 0.0, num3, 1.0);
		double num4 = num + (radius - num) / num3 * height;
		double num5 = height;
		val.Points[5] = new ControlPoint(num4, 0.0, num5, 0.7071067811);
		val.Points[6] = new ControlPoint(radius - num2, 0.0, height, 1.0);
		val.Points[7] = new ControlPoint((radius - num2) / 2.0, 0.0, height, 1.0);
		val.Points[8] = new ControlPoint(0.0, 0.0, height, 1.0);
		val.Knots[0] = 0.0;
		val.Knots[1] = 0.0;
		val.Knots[2] = 0.25;
		val.Knots[3] = 0.25;
		val.Knots[4] = 0.5;
		val.Knots[5] = 0.5;
		val.Knots[6] = 0.75;
		val.Knots[7] = 0.75;
		val.Knots[8] = 1.0;
		val.Knots[9] = 1.0;
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

	private static Brep MakeBar(double centerLength, double radius1, double radius2, double height, double domeRadius, double filletPer)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector((0.0 - centerLength) / 2.0, 0.0, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(centerLength / 2.0, 0.0, 0.0);
		double angleRadians = Math.Asin((radius1 - radius2) / centerLength);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector((0.0 - centerLength) / 2.0, radius1, 0.0);
		Transform val4 = CSBlockUtil.CSUtilMath.Rotation(angleRadians, -Vector3d.ZAxis, val);
		((Point3d)(ref val3)).Transform(val4);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(centerLength / 2.0, radius2, 0.0);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(angleRadians, -Vector3d.ZAxis, val2);
		((Point3d)(ref val5)).Transform(val6);
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(val3);
		((Point3d)(ref val7)).Y = 0.0 - ((Point3d)(ref val7)).Y;
		Point3d val8 = default(Point3d);
		((Point3d)(ref val8))._002Ector(val5);
		((Point3d)(ref val8)).Y = 0.0 - ((Point3d)(ref val8)).Y;
		Point3d val9 = default(Point3d);
		((Point3d)(ref val9))._002Ector((0.0 - centerLength) / 2.0 - radius1, 0.0, 0.0);
		Point3d val10 = default(Point3d);
		((Point3d)(ref val10))._002Ector(centerLength / 2.0 + radius2, 0.0, 0.0);
		ArcCurve val11 = new ArcCurve(new Arc(val3, val9, val7));
		ArcCurve val12 = new ArcCurve(new Arc(val5, val10, val8));
		NurbsCurve val13 = BuildProngProfile(radius1, height, domeRadius / radius1, filletPer);
		NurbsCurve val14 = BuildProngProfile(radius2, height, domeRadius / radius2, filletPer);
		NurbsSurface val15 = RailRevolve(val13, ((Curve)val11).ToNurbsCurve(), val, radius1);
		NurbsSurface val16 = RailRevolve(val14, ((Curve)val12).ToNurbsCurve(), val2, radius2);
		Transform val17 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, val, val3 - val, Vector3d.ZAxis);
		Transform val18 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, val, val7 - val, Vector3d.ZAxis);
		Transform val19 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, val2, val5 - val2, Vector3d.ZAxis);
		Transform val20 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, val2, val8 - val2, Vector3d.ZAxis);
		Curve val21 = ((Curve)val13).DuplicateCurve();
		((GeometryBase)val21).Transform(val17);
		Curve obj = ((Curve)val13).DuplicateCurve();
		((GeometryBase)obj).Transform(val18);
		Curve val22 = ((Curve)val14).DuplicateCurve();
		((GeometryBase)val22).Transform(val19);
		Curve val23 = ((Curve)val14).DuplicateCurve();
		((GeometryBase)val23).Transform(val20);
		NurbsSurface val24 = NurbsSurface.CreateRuledSurface(val21, val22);
		NurbsSurface val25 = NurbsSurface.CreateRuledSurface(obj, val23);
		Brep[] array = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep>
		{
			((Surface)val15).ToBrep(),
			((Surface)val16).ToBrep(),
			((Surface)val24).ToBrep(),
			((Surface)val25).ToBrep()
		}, 0.01);
		if (array.Length != 1)
		{
			return null;
		}
		Brep brep = array[0];
		CSUtilGeometry.FixBrepSolidOrientation(ref brep);
		return brep;
	}

	private static NurbsSurface RailRevolve(NurbsCurve profile, NurbsCurve rail, Point3d center, double s1)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		NurbsSurface val = NurbsSurface.Create(3, true, profile.Order, rail.Order, profile.Points.Count, rail.Points.Count);
		int count = rail.Points.Count;
		int count2 = profile.Points.Count;
		Point3d val3 = default(Point3d);
		for (int i = 0; i < count2; i++)
		{
			ControlPoint val2 = profile.Points[i];
			Point3d location = ((ControlPoint)(ref val2)).Location;
			((Point3d)(ref val3))._002Ector(location);
			((Point3d)(ref val3)).Z = 0.0;
			Point3d val4 = val3;
			double num = ((Point3d)(ref val4)).DistanceTo(Point3d.Origin);
			for (int j = 0; j < count; j++)
			{
				val2 = rail.Points[j];
				Vector3d val5 = ((ControlPoint)(ref val2)).Location - center;
				Point3d val6 = center + val5 * num / s1;
				((Point3d)(ref val6)).Z = ((Point3d)(ref location)).Z;
				val2 = profile.Points[i];
				double weight = ((ControlPoint)(ref val2)).Weight;
				val2 = rail.Points[j];
				double num2 = weight * ((ControlPoint)(ref val2)).Weight;
				val.Points.SetControlPoint(i, j, new ControlPoint(((Point3d)(ref val6)).X, ((Point3d)(ref val6)).Y, ((Point3d)(ref val6)).Z, num2));
			}
		}
		for (int k = 0; k < profile.Knots.Count; k++)
		{
			val.KnotsU[k] = profile.Knots[k];
		}
		for (int l = 0; l < rail.Knots.Count; l++)
		{
			val.KnotsV[l] = rail.Knots[l];
		}
		return val;
	}
}
