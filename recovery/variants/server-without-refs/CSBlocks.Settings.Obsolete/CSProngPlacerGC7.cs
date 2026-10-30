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
[CSBlock("ProngPlacerGC_V7", "{E25DBC76-5B8A-4970-B82C-11EE6471B51F}")]
public sealed class CSProngPlacerGC7 : CSBlock
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

	public readonly CSInputChannel InProngSizeControl;

	public readonly CSInputChannel InCornerProngAngle;

	public readonly CSInputChannel InCornerProngSize;

	public readonly CSInputChannel InCornerProngSizeOffset;

	public readonly CSOutputChannel OutProngs;

	public readonly CSOutputChannel OutProfiles;

	private ValueListItem _prongBase = ValueListItem.FollowCulet;

	private readonly CSProngCache _prongCache = new CSProngCache();

	public CSProngPlacerGC7(string identifier)
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
		InProngSizeControl = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 156);
		InCornerProngAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerProngSize = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 159);
		InCornerProngSizeOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProfiles = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Expected O, but got Unknown
		//IL_0f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdf: Expected O, but got Unknown
		//IL_0fdf: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_102f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1036: Expected O, but got Unknown
		//IL_103a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1046: Unknown result type (might be due to invalid IL or missing references)
		//IL_1057: Unknown result type (might be due to invalid IL or missing references)
		//IL_105e: Expected O, but got Unknown
		//IL_1014: Unknown result type (might be due to invalid IL or missing references)
		//IL_101e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1025: Expected O, but got Unknown
		//IL_1071: Unknown result type (might be due to invalid IL or missing references)
		//IL_107b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1082: Expected O, but got Unknown
		//IL_10bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c4: Expected O, but got Unknown
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Expected O, but got Unknown
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Expected O, but got Unknown
		//IL_10eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f2: Expected O, but got Unknown
		//IL_1101: Unknown result type (might be due to invalid IL or missing references)
		//IL_1106: Unknown result type (might be due to invalid IL or missing references)
		//IL_1110: Unknown result type (might be due to invalid IL or missing references)
		//IL_111c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1132: Expected O, but got Unknown
		//IL_1153: Unknown result type (might be due to invalid IL or missing references)
		//IL_116e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1191: Unknown result type (might be due to invalid IL or missing references)
		//IL_1198: Expected O, but got Unknown
		//IL_11a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ae: Expected O, but got Unknown
		//IL_11d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1204: Expected O, but got Unknown
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_0767: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_0822: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_126d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1274: Expected O, but got Unknown
		//IL_1296: Unknown result type (might be due to invalid IL or missing references)
		//IL_129b: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ca: Expected O, but got Unknown
		//IL_122f: Unknown result type (might be due to invalid IL or missing references)
		//IL_125d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1949: Unknown result type (might be due to invalid IL or missing references)
		//IL_1950: Unknown result type (might be due to invalid IL or missing references)
		//IL_1955: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8c: Expected O, but got Unknown
		//IL_0ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Expected O, but got Unknown
		//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Expected O, but got Unknown
		//IL_0bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Expected O, but got Unknown
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1323: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1355: Unknown result type (might be due to invalid IL or missing references)
		//IL_135c: Expected O, but got Unknown
		//IL_1377: Unknown result type (might be due to invalid IL or missing references)
		//IL_137e: Expected O, but got Unknown
		//IL_1399: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c0: Expected O, but got Unknown
		//IL_13c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1434: Unknown result type (might be due to invalid IL or missing references)
		//IL_143b: Expected O, but got Unknown
		//IL_144d: Expected O, but got Unknown
		//IL_144f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1454: Unknown result type (might be due to invalid IL or missing references)
		//IL_1459: Unknown result type (might be due to invalid IL or missing references)
		//IL_1460: Unknown result type (might be due to invalid IL or missing references)
		//IL_1467: Unknown result type (might be due to invalid IL or missing references)
		//IL_146e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1473: Unknown result type (might be due to invalid IL or missing references)
		//IL_1478: Unknown result type (might be due to invalid IL or missing references)
		//IL_147e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3a: Expected O, but got Unknown
		//IL_0c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c23: Expected O, but got Unknown
		//IL_19c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19db: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e5: Expected O, but got Unknown
		//IL_19e5: Expected O, but got Unknown
		//IL_150c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1513: Expected O, but got Unknown
		//IL_151a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1521: Expected O, but got Unknown
		//IL_0cb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Expected O, but got Unknown
		//IL_0cc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc7: Expected O, but got Unknown
		//IL_0c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Expected O, but got Unknown
		//IL_0c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c68: Expected O, but got Unknown
		//IL_1a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a3c: Expected O, but got Unknown
		//IL_1a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a64: Expected O, but got Unknown
		//IL_1a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2b: Expected O, but got Unknown
		//IL_153d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1544: Expected O, but got Unknown
		//IL_155d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1562: Unknown result type (might be due to invalid IL or missing references)
		//IL_156a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1571: Unknown result type (might be due to invalid IL or missing references)
		//IL_1578: Expected O, but got Unknown
		//IL_1591: Unknown result type (might be due to invalid IL or missing references)
		//IL_1598: Expected O, but got Unknown
		//IL_15b0: Expected O, but got Unknown
		//IL_1a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a88: Expected O, but got Unknown
		//IL_1ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aca: Expected O, but got Unknown
		//IL_15c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1605: Expected O, but got Unknown
		//IL_1610: Unknown result type (might be due to invalid IL or missing references)
		//IL_1615: Unknown result type (might be due to invalid IL or missing references)
		//IL_161a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1621: Unknown result type (might be due to invalid IL or missing references)
		//IL_1628: Unknown result type (might be due to invalid IL or missing references)
		//IL_162f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1634: Unknown result type (might be due to invalid IL or missing references)
		//IL_1639: Unknown result type (might be due to invalid IL or missing references)
		//IL_163b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1640: Unknown result type (might be due to invalid IL or missing references)
		//IL_1644: Unknown result type (might be due to invalid IL or missing references)
		//IL_164d: Unknown result type (might be due to invalid IL or missing references)
		//IL_166d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1679: Unknown result type (might be due to invalid IL or missing references)
		//IL_1690: Unknown result type (might be due to invalid IL or missing references)
		//IL_1695: Unknown result type (might be due to invalid IL or missing references)
		//IL_169b: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b00: Expected O, but got Unknown
		//IL_1b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b40: Expected O, but got Unknown
		//IL_1b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba6: Expected O, but got Unknown
		//IL_1bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbc: Expected O, but got Unknown
		//IL_1bde: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c12: Expected O, but got Unknown
		//IL_1873: Unknown result type (might be due to invalid IL or missing references)
		//IL_187a: Expected O, but got Unknown
		//IL_1885: Unknown result type (might be due to invalid IL or missing references)
		//IL_188a: Unknown result type (might be due to invalid IL or missing references)
		//IL_188f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1896: Unknown result type (might be due to invalid IL or missing references)
		//IL_189d: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_18be: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_18db: Unknown result type (might be due to invalid IL or missing references)
		//IL_18dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_16bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1b: Expected O, but got Unknown
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e29: Expected O, but got Unknown
		//IL_0e70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e77: Expected O, but got Unknown
		//IL_0e82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c82: Expected O, but got Unknown
		//IL_1ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd8: Expected O, but got Unknown
		//IL_1c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_170c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1711: Unknown result type (might be due to invalid IL or missing references)
		//IL_1716: Unknown result type (might be due to invalid IL or missing references)
		//IL_1722: Unknown result type (might be due to invalid IL or missing references)
		//IL_1729: Unknown result type (might be due to invalid IL or missing references)
		//IL_1730: Unknown result type (might be due to invalid IL or missing references)
		//IL_1735: Unknown result type (might be due to invalid IL or missing references)
		//IL_173a: Unknown result type (might be due to invalid IL or missing references)
		//IL_173e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1745: Unknown result type (might be due to invalid IL or missing references)
		//IL_174a: Unknown result type (might be due to invalid IL or missing references)
		//IL_174f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1753: Unknown result type (might be due to invalid IL or missing references)
		//IL_1758: Unknown result type (might be due to invalid IL or missing references)
		//IL_175a: Unknown result type (might be due to invalid IL or missing references)
		//IL_175f: Unknown result type (might be due to invalid IL or missing references)
		//IL_176a: Unknown result type (might be due to invalid IL or missing references)
		//IL_176c: Unknown result type (might be due to invalid IL or missing references)
		//IL_176e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Expected O, but got Unknown
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Expected O, but got Unknown
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae2: Expected O, but got Unknown
		//IL_1d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_17bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_17de: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1801: Unknown result type (might be due to invalid IL or missing references)
		//IL_1803: Unknown result type (might be due to invalid IL or missing references)
		//IL_1808: Unknown result type (might be due to invalid IL or missing references)
		//IL_1813: Unknown result type (might be due to invalid IL or missing references)
		//IL_1815: Unknown result type (might be due to invalid IL or missing references)
		//IL_1817: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6a: Expected O, but got Unknown
		//IL_1d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8c: Expected O, but got Unknown
		//IL_1da7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dce: Expected O, but got Unknown
		//IL_1dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e49: Expected O, but got Unknown
		//IL_1e5b: Expected O, but got Unknown
		//IL_1e5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e62: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e67: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f4b: Expected O, but got Unknown
		//IL_1f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f59: Expected O, but got Unknown
		//IL_1f59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f60: Expected O, but got Unknown
		//IL_1f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f94: Expected O, but got Unknown
		//IL_1fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb4: Expected O, but got Unknown
		//IL_1fcc: Expected O, but got Unknown
		//IL_1fdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1feb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff4: Unknown result type (might be due to invalid IL or missing references)
		//IL_201a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2021: Expected O, but got Unknown
		//IL_202c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2031: Unknown result type (might be due to invalid IL or missing references)
		//IL_2036: Unknown result type (might be due to invalid IL or missing references)
		//IL_203d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2044: Unknown result type (might be due to invalid IL or missing references)
		//IL_204b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2050: Unknown result type (might be due to invalid IL or missing references)
		//IL_2055: Unknown result type (might be due to invalid IL or missing references)
		//IL_2057: Unknown result type (might be due to invalid IL or missing references)
		//IL_205c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2060: Unknown result type (might be due to invalid IL or missing references)
		//IL_2069: Unknown result type (might be due to invalid IL or missing references)
		//IL_2089: Unknown result type (might be due to invalid IL or missing references)
		//IL_2095: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_20bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_228f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2296: Expected O, but got Unknown
		//IL_22a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_22da: Unknown result type (might be due to invalid IL or missing references)
		//IL_22df: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2128: Unknown result type (might be due to invalid IL or missing references)
		//IL_212d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2132: Unknown result type (might be due to invalid IL or missing references)
		//IL_213e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2145: Unknown result type (might be due to invalid IL or missing references)
		//IL_214c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2151: Unknown result type (might be due to invalid IL or missing references)
		//IL_2156: Unknown result type (might be due to invalid IL or missing references)
		//IL_215a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2161: Unknown result type (might be due to invalid IL or missing references)
		//IL_2166: Unknown result type (might be due to invalid IL or missing references)
		//IL_216b: Unknown result type (might be due to invalid IL or missing references)
		//IL_216f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2174: Unknown result type (might be due to invalid IL or missing references)
		//IL_2176: Unknown result type (might be due to invalid IL or missing references)
		//IL_217b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2186: Unknown result type (might be due to invalid IL or missing references)
		//IL_2188: Unknown result type (might be due to invalid IL or missing references)
		//IL_218a: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_21db: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2203: Unknown result type (might be due to invalid IL or missing references)
		//IL_220a: Unknown result type (might be due to invalid IL or missing references)
		//IL_220f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2214: Unknown result type (might be due to invalid IL or missing references)
		//IL_2218: Unknown result type (might be due to invalid IL or missing references)
		//IL_221d: Unknown result type (might be due to invalid IL or missing references)
		//IL_221f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2224: Unknown result type (might be due to invalid IL or missing references)
		//IL_222f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2231: Unknown result type (might be due to invalid IL or missing references)
		//IL_2233: Unknown result type (might be due to invalid IL or missing references)
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
		InCornerProngAngle.GetValue(out double value22);
		InCornerProngSize.GetValue(out int value23);
		InCornerProngSizeOffset.GetValue(out double value24);
		ValueListItem whichItem2 = ValueListItem.ProngPlacerSettingStyleProng;
		InSettingStyle.GetValueListItem(ref whichItem2);
		InProngSizeControl.GetValue(out int value25);
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
		InPercentToggle.GetValue(out bool value26);
		double value27 = 0.0;
		if (value26)
		{
			InProngPercentage.GetValue(out value27);
			value27 = 1.0 - value27;
		}
		bool flag2 = CSUtilGemRails.IsClosed(cSDataArray.Array);
		Point3d zMaxPoint;
		Circle val11 = default(Circle);
		Line val15 = default(Line);
		double num6 = default(double);
		for (int j = 0; j < cSDataArray.GetArrayCount(); j++)
		{
			int num4 = j - 1;
			int num5 = j + 1;
			if (num4 < 0)
			{
				if (!flag2)
				{
					continue;
				}
				num4 = cSDataArray.GetArrayCount() - 1;
			}
			if (num5 >= cSDataArray.GetArrayCount())
			{
				num5 = ((!flag2) ? j : 0);
			}
			CSChannelGemData channelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, num4);
			CSChannelGemData channelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			CSChannelGemData channelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, num5);
			if (channelGemData2 == null || channelGemData3 == null)
			{
				return UpdateError("Invalid gems");
			}
			CSDataHelper.SetSettingType(channelGemData3, flag ? GemSettingType.BR : GemSettingType.PR);
			Vector3d val7 = channelGemData3.Center - channelGemData2.Center;
			((Vector3d)(ref val7)).Unitize();
			Vector3d val8 = channelGemData3.Center - channelGemData4.Center;
			((Vector3d)(ref val8)).Unitize();
			if (num5 == j)
			{
				val8 = val7 * -1.0;
			}
			OnCircle circleLeft;
			OnCircle circleRight;
			double height3;
			string circles = GetCircles(channelGemData2, channelGemData3, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value26, value27, out circleLeft, out circleRight, out height3);
			if (!string.IsNullOrEmpty(circles))
			{
				return UpdateError(circles);
			}
			if (_prongBase == ValueListItem.FollowLargestGem)
			{
				height3 = (value26 ? (num3 * value27) : num3);
			}
			On3dVector val9 = new On3dVector(circleLeft.Center());
			On3dVector val10 = new On3dVector(circleRight.Center());
			On3dPoint value28 = new On3dPoint((IOn3dVector)(object)((val9 + (IOn3dVector)(object)val10) / 2.0));
			On3dVector value29 = val10 - (IOn3dVector)(object)val9;
			if (CSApplicationHelper.Application.IsCamMode)
			{
				Plane worldZX = Plane.WorldZX;
				Point3d origin = Point3d.Origin;
				zMaxPoint = channelGemData3.ZMaxPoint;
				((Circle)(ref val11))._002Ector(worldZX, origin, ((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin));
				OnXform val12 = new OnXform();
				val12.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
				Vector3d zAxis = Vector3d.ZAxis;
				((Vector3d)(ref zAxis)).Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val12));
				((Vector3d)(ref zAxis)).Unitize();
				OnXform val13 = new OnXform();
				val13.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
				Vector3d zAxis2 = Vector3d.ZAxis;
				((Vector3d)(ref zAxis2)).Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val13));
				((Vector3d)(ref zAxis2)).Unitize();
				Vector3d destination = zAxis + zAxis2;
				((Vector3d)(ref destination)).Unitize();
				Transform val14 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value28), CSConvert.On3dVectorCopyToVector3d(value29), destination);
				Point3d origin2 = Point3d.Origin;
				((Point3d)(ref origin2)).Transform(val14);
				((Line)(ref val15))._002Ector(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
				((Line)(ref val15)).Transform(val14);
				Point3d val16 = ((Line)(ref val15)).ClosestPoint(((Circle)(ref val11)).ClosestPoint(((Line)(ref val15)).PointAt(1.0)), true);
				if (((Point3d)(ref val16)).DistanceTo(origin2) > height3 * 0.8)
				{
					height3 = ((Point3d)(ref val16)).DistanceTo(origin2) * 1.2;
				}
			}
			if (!flag && RhinoMath.ToDegrees(Math.Acos(val7 * val8)) < value22)
			{
				val7 *= channelGemData3.GemXSize / 2.0;
				val8 *= channelGemData3.GemXSize / 2.0;
				Vector3d val17 = val7 + val8;
				Point3d val18 = channelGemData3.Center + val17;
				channelGemData3.GetCurve().ClosestPoint(val18, ref num6);
				OnCircle circleLeft2;
				OnCircle circleRight2;
				string circles2 = GetCircles(channelGemData2, channelGemData3, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value26, value27, out circleLeft2, out circleRight2, out height3);
				OnCircle circleLeft3;
				OnCircle circleRight3;
				string circles3 = GetCircles(channelGemData3, channelGemData4, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value26, value27, out circleLeft3, out circleRight3, out height3);
				if (!string.IsNullOrEmpty(circles2) || !string.IsNullOrEmpty(circles3))
				{
					break;
				}
				double num7 = (circleRight3.radius + circleRight2.radius + circleLeft3.radius + circleLeft2.radius) / 4.0 + value24;
				double[] array = new double[4] { circleRight3.radius, circleRight2.radius, circleLeft2.radius, circleLeft3.radius };
				if (value23 == 157)
				{
					double num8 = array[0];
					for (int k = 1; k < array.Length; k++)
					{
						if (num8 < array[k])
						{
							num8 = array[k];
						}
					}
					num7 = num8 + value24;
				}
				if (value23 == 158)
				{
					double num9 = array[0];
					for (int l = 1; l < array.Length; l++)
					{
						if (num9 > array[l])
						{
							num9 = array[l];
						}
					}
					num7 = num9 + value24;
				}
				double num10 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData3.Size) / 2.0) : channelGemData3.GemZMin);
				Point3d val19 = channelGemData3.GetCurve().PointAt(num6);
				((Point3d)(ref val19)).Transform(Transform.Translation(channelGemData3.Normal * (value9 - num10)));
				((Vector3d)(ref val17)).Unitize();
				val17 *= num7;
				val19 += val17;
				On3dPoint val20 = CSConvert.Point3dCopyToOn3dPoint(val19);
				OnCircle val21 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val20, (IOn3dVector)(object)channelGemData3.Normal.Convert()), (IOn3dPoint)(object)val20, num7);
				OnXform val22 = new OnXform();
				val22.ChangeBasis((IOnPlane)(object)val21.plane, OnUtil.On_xy_plane);
				double num11 = val21.radius * 2.0;
				if (num11 > value2 && num11 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num11, height3, value5, value6, value, out var prong, out var prongBaseRail))
					{
						return UpdateError("Create prong cache failed");
					}
					prong.Transform((IOnXform)(object)val22);
					prongBaseRail.Transform((IOnXform)(object)val22);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val22.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val22.Convert(), applyTransform: false));
				}
			}
			OnXform val23 = new OnXform();
			val23.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
			On3dVector val24 = new On3dVector(OnUtil.On_zaxis);
			val24.Transform((IOnXform)(object)val23);
			val24.Unitize();
			OnXform val25 = new OnXform();
			val25.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
			On3dVector val26 = new On3dVector(OnUtil.On_zaxis);
			val26.Transform((IOnXform)(object)val25);
			val26.Unitize();
			if (value25 == 157)
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
			if (value25 == 158)
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
			if (value25 == 159)
			{
				double radius = circleRight.radius;
				double radius2 = circleLeft.radius;
				double num12 = (radius + radius2) / 2.0;
				circleRight = new OnCircle(circleRight.Center(), num12);
				circleLeft = new OnCircle(circleLeft.Center(), num12);
			}
			if (!flag)
			{
				double num13 = circleLeft.radius * 2.0;
				if (num13 > value2 && num13 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num13, height3, value5, value6, value, out var prong2, out var prongBaseRail2))
					{
						return UpdateError("Create prong cache failed");
					}
					prong2.Transform((IOnXform)(object)val23);
					prongBaseRail2.Transform((IOnXform)(object)val23);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val23.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val23.Convert(), applyTransform: false));
				}
			}
			if (!flag)
			{
				double num14 = circleRight.radius * 2.0;
				if (num14 > value2 && num14 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num14, height3, value5, value6, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					prong3.Transform((IOnXform)(object)val25);
					prongBaseRail3.Transform((IOnXform)(object)val25);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), val25.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail3).Convert(), val25.Convert(), applyTransform: false));
				}
			}
			On3dVector val27 = val24 + (IOn3dVector)(object)val26;
			val27.Unitize();
			if (flag)
			{
				val9 = new On3dVector(circleLeft.Center());
				val10 = new On3dVector(circleRight.Center());
				Brep geometry = MakeBar((val9 - (IOn3dVector)(object)val10).Length(), circleLeft.radius, circleRight.radius, height3, value6, value18, value19, value20, value21);
				value28 = new On3dPoint((IOn3dVector)(object)((val9 + (IOn3dVector)(object)val10) / 2.0));
				value29 = val10 - (IOn3dVector)(object)val9;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value28), CSConvert.On3dVectorCopyToVector3d(value29), CSConvert.On3dVectorCopyToVector3d(val27));
				concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry, transform));
			}
			if (val5 == OnUtil.On_UNSET_VECTOR)
			{
				val5 = val27;
			}
			val6 = val27;
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
					cSChannelGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData.Normal, cSChannelGemData.Center));
				}
				double num15 = cSChannelGemData.GemXSize * value12 + value7 + value8;
				double height5 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, num15, height5, value5, value6, value, out var prong5, out var prongBaseRail5))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint obj = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
				double num16 = (cSChannelGemData.GemXSize * value12 + value7) / 2.0;
				double num17 = value9 - cSChannelGemData.GemZMin;
				On3dVector val44;
				if (val5 == OnUtil.On_UNSET_VECTOR)
				{
					val44 = new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert());
					val44.Unitize();
				}
				else
				{
					OnXform val45 = new OnXform();
					val45.Mirror((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
					val44 = new On3dVector((IOn3dVector)(object)val5);
					val44.Transform((IOnXform)(object)val45);
					val44.Unitize();
				}
				On3dVector val46 = new On3dVector((IOn3dVector)(object)cSChannelGemData.LeftVector.Convert());
				val46.Unitize();
				On3dVector val47 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val44, (IOn3dVector)(object)val46);
				val47.Unitize();
				OnPlane val48 = new OnPlane((IOn3dPoint)(object)(obj + (IOn3dVector)(object)(val47 * num16) + (IOn3dVector)(object)(val44 * num17)), (IOn3dVector)(object)val46, (IOn3dVector)(object)val47);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				{
					OnXform val64 = new OnXform();
					val64.ChangeBasis((IOnPlane)(object)val48, OnUtil.On_xy_plane);
					OnXform val65 = new OnXform();
					val65.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val66 = val65 * (IOnXform)(object)val64;
					prong5.Transform((IOnXform)(object)val66);
					prongBaseRail5.Transform((IOnXform)(object)val66);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), val66.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail5).Convert(), val66.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num18 = value14 / 2.0;
					OnXform val49 = new OnXform();
					val49.ChangeBasis((IOnPlane)(object)val48, OnUtil.On_xy_plane);
					OnXform val50 = new OnXform();
					val50.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num18), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val51 = new OnXform();
					val51.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 + num18), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val52 = val51 * (IOnXform)(object)val49 * (IOnXform)(object)val50;
					if (!flag)
					{
						OnGeometry val53 = prong5.Duplicate();
						val53.Transform((IOnXform)(object)val52);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val53).Convert(), val52.Convert(), applyTransform: false));
						OnGeometry val54 = prongBaseRail5.Duplicate();
						val54.Transform((IOnXform)(object)val52);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val54).Convert(), val52.Convert(), applyTransform: false));
					}
					OnXform val55 = new OnXform();
					val55.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num18), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val56 = new OnXform();
					val56.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 - num18), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val57 = val56 * (IOnXform)(object)val49 * (IOnXform)(object)val55;
					if (!flag)
					{
						OnGeometry val58 = prong5.Duplicate();
						val58.Transform((IOnXform)(object)val57);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val58).Convert(), val57.Convert(), applyTransform: false));
						OnGeometry val59 = prongBaseRail5.Duplicate();
						val59.Transform((IOnXform)(object)val57);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val59).Convert(), val57.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val60 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val61 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val62 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val63 = new On3dVector(0.0, 0.0, 1.0);
						val60.Transform((IOnXform)(object)val52);
						val62.Transform((IOnXform)(object)val52);
						val61.Transform((IOnXform)(object)val57);
						val63.Transform((IOnXform)(object)val57);
						Brep geometry3 = MakeBar((val60 - (IOn3dPoint)(object)val61).Length(), num15 / 2.0, num15 / 2.0, height5, value6, value18, value19, value20, value21);
						On3dPoint value36 = new On3dPoint((IOn3dPoint)(object)((val60 + (IOn3dPoint)(object)val61) / 2.0));
						On3dVector value37 = val61 - (IOn3dPoint)(object)val60;
						On3dVector value38 = val62 + (IOn3dVector)(object)val63;
						Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value36), CSConvert.On3dVectorCopyToVector3d(value37), CSConvert.On3dVectorCopyToVector3d(value38));
						concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry3, transform2));
					}
					break;
				}
				}
				break;
			}
			case ValueListItem.ProngPlacerStyleEnd10:
			{
				CSChannelGemData channelGemData5 = CSUtilGemRails.GetChannelGemData(cSDataArray, 0);
				CSChannelGemData channelGemData6 = CSUtilGemRails.GetChannelGemData(cSDataArray, 1);
				if (channelGemData5 == null || channelGemData6 == null)
				{
					return UpdateError("Invalid gems");
				}
				CSDataHelper.SetSettingType(channelGemData6, flag ? GemSettingType.BR : GemSettingType.PR);
				OnCircle circleLeft4;
				OnCircle circleRight4;
				double height4;
				string circles4 = GetCircles(channelGemData5, channelGemData6, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value26, value27, out circleLeft4, out circleRight4, out height4);
				if (!string.IsNullOrEmpty(circles4))
				{
					return UpdateError(circles4);
				}
				On3dVector val28 = new On3dVector(circleLeft4.Center());
				On3dVector val29 = new On3dVector(circleRight4.Center());
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height4 = (value26 ? (num3 * value27) : num3);
				}
				OnXform val30 = new OnXform();
				val30.ChangeBasis((IOnPlane)(object)circleLeft4.plane, OnUtil.On_xy_plane);
				On3dVector val31 = new On3dVector(OnUtil.On_zaxis);
				val31.Transform((IOnXform)(object)val30);
				val31.Unitize();
				OnXform val32 = new OnXform();
				val32.ChangeBasis((IOnPlane)(object)circleRight4.plane, OnUtil.On_xy_plane);
				On3dVector val33 = new On3dVector(OnUtil.On_zaxis);
				val33.Transform((IOnXform)(object)val32);
				val33.Unitize();
				On3dVector value30 = val31 + (IOn3dVector)(object)val33;
				if (CSApplicationHelper.Application.IsCamMode)
				{
					Plane worldZX2 = Plane.WorldZX;
					Point3d origin3 = Point3d.Origin;
					zMaxPoint = channelGemData6.ZMaxPoint;
					Circle val34 = default(Circle);
					((Circle)(ref val34))._002Ector(worldZX2, origin3, ((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin));
					On3dPoint value31 = new On3dPoint((IOn3dVector)(object)((val28 + (IOn3dVector)(object)val29) / 2.0));
					On3dVector value32 = val29 - (IOn3dVector)(object)val28;
					Transform val35 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value31), CSConvert.On3dVectorCopyToVector3d(value32), CSConvert.On3dVectorCopyToVector3d(value30));
					Point3d origin4 = Point3d.Origin;
					((Point3d)(ref origin4)).Transform(val35);
					Line val36 = default(Line);
					((Line)(ref val36))._002Ector(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
					((Line)(ref val36)).Transform(val35);
					Point3d val37 = ((Line)(ref val36)).ClosestPoint(((Circle)(ref val34)).ClosestPoint(((Line)(ref val36)).PointAt(1.0)), true);
					if (((Point3d)(ref val37)).DistanceTo(origin4) > height4 * 0.8)
					{
						height4 = ((Point3d)(ref val37)).DistanceTo(origin4) * 1.2;
					}
				}
				if (!flag)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft4.Diameter(), height4, value5, value6, value, out var prong4, out var prongBaseRail4))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value33 = val29 - (IOn3dVector)(object)val28;
					Transform val38 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val28)), CSConvert.On3dVectorCopyToVector3d(value33), CSConvert.On3dVectorCopyToVector3d(value30));
					Vector3d val39 = channelGemData5.Center - channelGemData5.XMaxPoint;
					Transform val40 = Transform.Mirror(channelGemData5.Center, val39);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val40 * val38));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight4.Diameter(), height4, value5, value6, value, out prong4, out prongBaseRail4))
					{
						return UpdateError("Create prong cache failed");
					}
					value33 = val28 - (IOn3dVector)(object)val29;
					val38 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val29)), CSConvert.On3dVectorCopyToVector3d(value33), CSConvert.On3dVectorCopyToVector3d(value30));
					val39 = channelGemData5.Center - channelGemData5.XMaxPoint;
					val40 = Transform.Mirror(channelGemData5.Center, val39);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val40 * val38));
				}
				else
				{
					Brep geometry2 = MakeBar((val28 - (IOn3dVector)(object)val29).Length(), circleLeft4.radius, circleRight4.radius, height4, value6, value18, value19, value20, value21);
					On3dPoint value34 = new On3dPoint((IOn3dVector)(object)((val28 + (IOn3dVector)(object)val29) / 2.0));
					On3dVector value35 = val29 - (IOn3dVector)(object)val28;
					Transform val41 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value34), CSConvert.On3dVectorCopyToVector3d(value35), CSConvert.On3dVectorCopyToVector3d(value30));
					Vector3d val42 = channelGemData5.Center - channelGemData5.XMaxPoint;
					Transform val43 = Transform.Mirror(channelGemData5.Center, val42);
					concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry2, val43 * val41));
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
					cSChannelGemData2.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData2.Normal, cSChannelGemData2.Center));
				}
				double num19 = cSChannelGemData2.GemXSize * value15 + value7 + value8;
				double height7 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData2.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, num19, height7, value5, value6, value, out var prong7, out var prongBaseRail7))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint obj2 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
				double num20 = (cSChannelGemData2.GemXSize * value15 + value7) / 2.0;
				double num21 = value9 - cSChannelGemData2.GemZMin;
				On3dVector val83;
				if (val6 == OnUtil.On_UNSET_VECTOR)
				{
					val83 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert());
					val83.Unitize();
				}
				else
				{
					OnXform val84 = new OnXform();
					val84.Mirror((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					val83 = new On3dVector((IOn3dVector)(object)val6);
					val83.Transform((IOnXform)(object)val84);
					val83.Unitize();
				}
				On3dVector val85 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.RightVector.Convert());
				val85.Unitize();
				On3dVector val86 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val83, (IOn3dVector)(object)val85);
				val86.Unitize();
				OnPlane val87 = new OnPlane((IOn3dPoint)(object)(obj2 + (IOn3dVector)(object)(val86 * num20) + (IOn3dVector)(object)(val83 * num21)), (IOn3dVector)(object)val85, (IOn3dVector)(object)val86);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd2:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd8:
				{
					OnXform val103 = new OnXform();
					val103.ChangeBasis((IOnPlane)(object)val87, OnUtil.On_xy_plane);
					OnXform val104 = new OnXform();
					val104.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val105 = val104 * (IOnXform)(object)val103;
					prong7.Transform((IOnXform)(object)val105);
					prongBaseRail7.Transform((IOnXform)(object)val105);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong7).Convert(), val105.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail7).Convert(), val105.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd3:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num22 = value17 / 2.0;
					OnXform val88 = new OnXform();
					val88.ChangeBasis((IOnPlane)(object)val87, OnUtil.On_xy_plane);
					OnXform val89 = new OnXform();
					val89.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num22), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val90 = new OnXform();
					val90.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 + num22), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val91 = val90 * (IOnXform)(object)val88 * (IOnXform)(object)val89;
					if (!flag)
					{
						OnGeometry val92 = prong7.Duplicate();
						val92.Transform((IOnXform)(object)val91);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val92).Convert(), val91.Convert(), applyTransform: false));
						OnGeometry val93 = prongBaseRail7.Duplicate();
						val93.Transform((IOnXform)(object)val91);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val93).Convert(), val91.Convert(), applyTransform: false));
					}
					OnXform val94 = new OnXform();
					val94.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num22), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val95 = new OnXform();
					val95.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 - num22), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val96 = val95 * (IOnXform)(object)val88 * (IOnXform)(object)val94;
					if (!flag)
					{
						OnGeometry val97 = prong7.Duplicate();
						val97.Transform((IOnXform)(object)val96);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val97).Convert(), val96.Convert(), applyTransform: false));
						OnGeometry val98 = prongBaseRail7.Duplicate();
						val98.Transform((IOnXform)(object)val96);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val98).Convert(), val96.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val99 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val100 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val101 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val102 = new On3dVector(0.0, 0.0, 1.0);
						val99.Transform((IOnXform)(object)val91);
						val101.Transform((IOnXform)(object)val91);
						val100.Transform((IOnXform)(object)val96);
						val102.Transform((IOnXform)(object)val96);
						Brep geometry5 = MakeBar((val99 - (IOn3dPoint)(object)val100).Length(), num19 / 2.0, num19 / 2.0, height7, value6, value18, value19, value20, value21);
						On3dPoint value45 = new On3dPoint((IOn3dPoint)(object)((val99 + (IOn3dPoint)(object)val100) / 2.0));
						On3dVector value46 = val100 - (IOn3dPoint)(object)val99;
						On3dVector value47 = val101 + (IOn3dVector)(object)val102;
						Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value45), CSConvert.On3dVectorCopyToVector3d(value46), CSConvert.On3dVectorCopyToVector3d(value47));
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
				OnCircle circleLeft5;
				OnCircle circleRight5;
				double height6;
				string circles5 = GetCircles(channelGemData7, channelGemData8, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value26, value27, out circleLeft5, out circleRight5, out height6);
				if (!string.IsNullOrEmpty(circles5))
				{
					return UpdateError(circles5);
				}
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height6 = (value26 ? (num3 * value27) : num3);
				}
				On3dVector val67 = new On3dVector(circleLeft5.Center());
				On3dVector val68 = new On3dVector(circleRight5.Center());
				OnXform val69 = new OnXform();
				val69.ChangeBasis((IOnPlane)(object)circleLeft5.plane, OnUtil.On_xy_plane);
				On3dVector val70 = new On3dVector(OnUtil.On_zaxis);
				val70.Transform((IOnXform)(object)val69);
				val70.Unitize();
				OnXform val71 = new OnXform();
				val71.ChangeBasis((IOnPlane)(object)circleRight5.plane, OnUtil.On_xy_plane);
				On3dVector val72 = new On3dVector(OnUtil.On_zaxis);
				val72.Transform((IOnXform)(object)val71);
				val72.Unitize();
				On3dVector value39 = val70 + (IOn3dVector)(object)val72;
				if (CSApplicationHelper.Application.IsCamMode)
				{
					Plane worldZX3 = Plane.WorldZX;
					Point3d origin5 = Point3d.Origin;
					zMaxPoint = channelGemData8.ZMaxPoint;
					Circle val73 = default(Circle);
					((Circle)(ref val73))._002Ector(worldZX3, origin5, ((Point3d)(ref zMaxPoint)).DistanceTo(Point3d.Origin));
					On3dPoint value40 = new On3dPoint((IOn3dVector)(object)((val67 + (IOn3dVector)(object)val68) / 2.0));
					On3dVector value41 = val68 - (IOn3dVector)(object)val67;
					Transform val74 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value40), CSConvert.On3dVectorCopyToVector3d(value41), CSConvert.On3dVectorCopyToVector3d(value39));
					Point3d origin6 = Point3d.Origin;
					((Point3d)(ref origin6)).Transform(val74);
					Line val75 = default(Line);
					((Line)(ref val75))._002Ector(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
					((Line)(ref val75)).Transform(val74);
					Point3d val76 = ((Line)(ref val75)).ClosestPoint(((Circle)(ref val73)).ClosestPoint(((Line)(ref val75)).PointAt(1.0)), true);
					if (((Point3d)(ref val76)).DistanceTo(origin6) > height6 * 0.8)
					{
						height6 = ((Point3d)(ref val76)).DistanceTo(origin6) * 1.2;
					}
				}
				if (!flag)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft5.Diameter(), height6, value5, value6, value, out var prong6, out var prongBaseRail6))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value42 = val68 - (IOn3dVector)(object)val67;
					Transform val77 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val67)), CSConvert.On3dVectorCopyToVector3d(value42), CSConvert.On3dVectorCopyToVector3d(value39));
					Vector3d val78 = channelGemData8.Center - channelGemData8.XMaxPoint;
					Transform val79 = Transform.Mirror(channelGemData8.Center, val78);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val79 * val77));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight5.Diameter(), height6, value5, value6, value, out prong6, out prongBaseRail6))
					{
						return UpdateError("Create prong cache failed");
					}
					value42 = val67 - (IOn3dVector)(object)val68;
					val77 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val68)), CSConvert.On3dVectorCopyToVector3d(value42), CSConvert.On3dVectorCopyToVector3d(value39));
					val78 = channelGemData8.Center - channelGemData8.XMaxPoint;
					val79 = Transform.Mirror(channelGemData8.Center, val78);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val79 * val77));
				}
				else
				{
					Brep geometry4 = MakeBar((val67 - (IOn3dVector)(object)val68).Length(), circleLeft5.radius, circleRight5.radius, height6, value6, value18, value19, value20, value21);
					On3dPoint value43 = new On3dPoint((IOn3dVector)(object)((val67 + (IOn3dVector)(object)val68) / 2.0));
					On3dVector value44 = val68 - (IOn3dVector)(object)val67;
					Transform val80 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value43), CSConvert.On3dVectorCopyToVector3d(value44), CSConvert.On3dVectorCopyToVector3d(value39));
					Vector3d val81 = channelGemData8.Center - channelGemData8.XMaxPoint;
					Transform val82 = Transform.Mirror(channelGemData8.Center, val81);
					concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry4, val82 * val80));
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
