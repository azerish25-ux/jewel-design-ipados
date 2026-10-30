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
[CSBlock("ProngPlacerGC_V8", "{F5DBB048-DE6E-410E-9257-0A68CE15BFCC}")]
public sealed class CSProngPlacerGC8 : CSBlock
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

	public CSProngPlacerGC8(string identifier)
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
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Expected O, but got Unknown
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Expected O, but got Unknown
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Expected O, but got Unknown
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Expected O, but got Unknown
		//IL_1034: Unknown result type (might be due to invalid IL or missing references)
		//IL_1039: Unknown result type (might be due to invalid IL or missing references)
		//IL_1044: Unknown result type (might be due to invalid IL or missing references)
		//IL_100b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Unknown result type (might be due to invalid IL or missing references)
		//IL_101e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Expected O, but got Unknown
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4d: Expected O, but got Unknown
		//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6d: Expected O, but got Unknown
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c85: Expected O, but got Unknown
		//IL_0c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca5: Expected O, but got Unknown
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_0907: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_091f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_0929: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_173a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1741: Unknown result type (might be due to invalid IL or missing references)
		//IL_1746: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Unknown result type (might be due to invalid IL or missing references)
		//IL_10db: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfb: Expected O, but got Unknown
		//IL_0cdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce4: Expected O, but got Unknown
		//IL_0965: Unknown result type (might be due to invalid IL or missing references)
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Unknown result type (might be due to invalid IL or missing references)
		//IL_0997: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_140a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1411: Unknown result type (might be due to invalid IL or missing references)
		//IL_1416: Unknown result type (might be due to invalid IL or missing references)
		//IL_141b: Unknown result type (might be due to invalid IL or missing references)
		//IL_110e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1110: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1107: Unknown result type (might be due to invalid IL or missing references)
		//IL_110c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d78: Expected O, but got Unknown
		//IL_0d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d88: Expected O, but got Unknown
		//IL_0d39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d40: Expected O, but got Unknown
		//IL_0d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d29: Expected O, but got Unknown
		//IL_17b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d6: Expected O, but got Unknown
		//IL_17d6: Expected O, but got Unknown
		//IL_143d: Unknown result type (might be due to invalid IL or missing references)
		//IL_143f: Unknown result type (might be due to invalid IL or missing references)
		//IL_142b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1436: Unknown result type (might be due to invalid IL or missing references)
		//IL_143b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1128: Unknown result type (might be due to invalid IL or missing references)
		//IL_113b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1140: Unknown result type (might be due to invalid IL or missing references)
		//IL_1142: Unknown result type (might be due to invalid IL or missing references)
		//IL_1155: Unknown result type (might be due to invalid IL or missing references)
		//IL_115a: Unknown result type (might be due to invalid IL or missing references)
		//IL_115c: Unknown result type (might be due to invalid IL or missing references)
		//IL_115e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1160: Unknown result type (might be due to invalid IL or missing references)
		//IL_1165: Unknown result type (might be due to invalid IL or missing references)
		//IL_1169: Unknown result type (might be due to invalid IL or missing references)
		//IL_116e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1170: Unknown result type (might be due to invalid IL or missing references)
		//IL_1175: Unknown result type (might be due to invalid IL or missing references)
		//IL_117e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1826: Unknown result type (might be due to invalid IL or missing references)
		//IL_182d: Expected O, but got Unknown
		//IL_1831: Unknown result type (might be due to invalid IL or missing references)
		//IL_183d: Unknown result type (might be due to invalid IL or missing references)
		//IL_184e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1855: Expected O, but got Unknown
		//IL_180b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1815: Unknown result type (might be due to invalid IL or missing references)
		//IL_181c: Expected O, but got Unknown
		//IL_1457: Unknown result type (might be due to invalid IL or missing references)
		//IL_146a: Unknown result type (might be due to invalid IL or missing references)
		//IL_146f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1471: Unknown result type (might be due to invalid IL or missing references)
		//IL_1484: Unknown result type (might be due to invalid IL or missing references)
		//IL_1489: Unknown result type (might be due to invalid IL or missing references)
		//IL_148b: Unknown result type (might be due to invalid IL or missing references)
		//IL_148d: Unknown result type (might be due to invalid IL or missing references)
		//IL_148f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1494: Unknown result type (might be due to invalid IL or missing references)
		//IL_1498: Unknown result type (might be due to invalid IL or missing references)
		//IL_149d: Unknown result type (might be due to invalid IL or missing references)
		//IL_149f: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1868: Unknown result type (might be due to invalid IL or missing references)
		//IL_1872: Unknown result type (might be due to invalid IL or missing references)
		//IL_1879: Expected O, but got Unknown
		//IL_18b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bb: Expected O, but got Unknown
		//IL_0ed5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edc: Expected O, but got Unknown
		//IL_0ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eea: Expected O, but got Unknown
		//IL_0f31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f38: Expected O, but got Unknown
		//IL_0f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e9: Expected O, but got Unknown
		//IL_18f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1907: Unknown result type (might be due to invalid IL or missing references)
		//IL_1913: Unknown result type (might be due to invalid IL or missing references)
		//IL_1929: Expected O, but got Unknown
		//IL_194a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1965: Unknown result type (might be due to invalid IL or missing references)
		//IL_1988: Unknown result type (might be due to invalid IL or missing references)
		//IL_198f: Expected O, but got Unknown
		//IL_199e: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a5: Expected O, but got Unknown
		//IL_19c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fb: Expected O, but got Unknown
		//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6b: Expected O, but got Unknown
		//IL_1a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac1: Expected O, but got Unknown
		//IL_1a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_2224: Unknown result type (might be due to invalid IL or missing references)
		//IL_222b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9a: Expected O, but got Unknown
		//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9c: Expected O, but got Unknown
		//IL_0b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba3: Expected O, but got Unknown
		//IL_1aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12db: Unknown result type (might be due to invalid IL or missing references)
		//IL_12dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1309: Expected O, but got Unknown
		//IL_1304: Unknown result type (might be due to invalid IL or missing references)
		//IL_130b: Expected O, but got Unknown
		//IL_130b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1312: Expected O, but got Unknown
		//IL_1b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b53: Expected O, but got Unknown
		//IL_1b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b75: Expected O, but got Unknown
		//IL_1b90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb7: Expected O, but got Unknown
		//IL_1bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c32: Expected O, but got Unknown
		//IL_1c44: Expected O, but got Unknown
		//IL_1c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_15dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_15eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1601: Unknown result type (might be due to invalid IL or missing references)
		//IL_1606: Unknown result type (might be due to invalid IL or missing references)
		//IL_1608: Unknown result type (might be due to invalid IL or missing references)
		//IL_160a: Unknown result type (might be due to invalid IL or missing references)
		//IL_160c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1611: Unknown result type (might be due to invalid IL or missing references)
		//IL_1613: Unknown result type (might be due to invalid IL or missing references)
		//IL_1620: Unknown result type (might be due to invalid IL or missing references)
		//IL_162a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1638: Expected O, but got Unknown
		//IL_1633: Unknown result type (might be due to invalid IL or missing references)
		//IL_163a: Expected O, but got Unknown
		//IL_163a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1641: Expected O, but got Unknown
		//IL_229b: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c0: Expected O, but got Unknown
		//IL_22c0: Expected O, but got Unknown
		//IL_1d07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0e: Expected O, but got Unknown
		//IL_1d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d1c: Expected O, but got Unknown
		//IL_0c1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c36: Unknown result type (might be due to invalid IL or missing references)
		//IL_2310: Unknown result type (might be due to invalid IL or missing references)
		//IL_2317: Expected O, but got Unknown
		//IL_231b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2327: Unknown result type (might be due to invalid IL or missing references)
		//IL_2338: Unknown result type (might be due to invalid IL or missing references)
		//IL_233f: Expected O, but got Unknown
		//IL_22f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2306: Expected O, but got Unknown
		//IL_138a: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2352: Unknown result type (might be due to invalid IL or missing references)
		//IL_235c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2363: Expected O, but got Unknown
		//IL_239e: Unknown result type (might be due to invalid IL or missing references)
		//IL_23a5: Expected O, but got Unknown
		//IL_1d5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d61: Expected O, but got Unknown
		//IL_1d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4a: Expected O, but got Unknown
		//IL_16b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_23db: Expected O, but got Unknown
		//IL_23ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2405: Unknown result type (might be due to invalid IL or missing references)
		//IL_241b: Expected O, but got Unknown
		//IL_243c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2457: Unknown result type (might be due to invalid IL or missing references)
		//IL_247a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2481: Expected O, but got Unknown
		//IL_2490: Unknown result type (might be due to invalid IL or missing references)
		//IL_2497: Expected O, but got Unknown
		//IL_24bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_24cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_24db: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f1: Expected O, but got Unknown
		//IL_1dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dde: Expected O, but got Unknown
		//IL_1de7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dee: Expected O, but got Unknown
		//IL_1d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da6: Expected O, but got Unknown
		//IL_1d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8f: Expected O, but got Unknown
		//IL_258b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2594: Expected O, but got Unknown
		//IL_25bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_25da: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f0: Expected O, but got Unknown
		//IL_2539: Unknown result type (might be due to invalid IL or missing references)
		//IL_257b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e11: Expected O, but got Unknown
		//IL_1e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e37: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e45: Expected O, but got Unknown
		//IL_1e5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e65: Expected O, but got Unknown
		//IL_1e7d: Expected O, but got Unknown
		//IL_263a: Unknown result type (might be due to invalid IL or missing references)
		//IL_267c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ecb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed2: Expected O, but got Unknown
		//IL_1edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1efc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f62: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f71: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b7: Expected O, but got Unknown
		//IL_26d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_26dd: Expected O, but got Unknown
		//IL_26fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_271a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2723: Expected O, but got Unknown
		//IL_2736: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d2: Expected O, but got Unknown
		//IL_27f6: Expected O, but got Unknown
		//IL_27fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2801: Unknown result type (might be due to invalid IL or missing references)
		//IL_2806: Unknown result type (might be due to invalid IL or missing references)
		//IL_2811: Unknown result type (might be due to invalid IL or missing references)
		//IL_281c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2827: Unknown result type (might be due to invalid IL or missing references)
		//IL_282c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2831: Unknown result type (might be due to invalid IL or missing references)
		//IL_283f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2154: Unknown result type (might be due to invalid IL or missing references)
		//IL_215b: Expected O, but got Unknown
		//IL_2166: Unknown result type (might be due to invalid IL or missing references)
		//IL_216b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2170: Unknown result type (might be due to invalid IL or missing references)
		//IL_2177: Unknown result type (might be due to invalid IL or missing references)
		//IL_217e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2185: Unknown result type (might be due to invalid IL or missing references)
		//IL_218a: Unknown result type (might be due to invalid IL or missing references)
		//IL_218f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2193: Unknown result type (might be due to invalid IL or missing references)
		//IL_219a: Unknown result type (might be due to invalid IL or missing references)
		//IL_219f: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_21af: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_21bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_21be: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fde: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ffd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2002: Unknown result type (might be due to invalid IL or missing references)
		//IL_2007: Unknown result type (might be due to invalid IL or missing references)
		//IL_200b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2012: Unknown result type (might be due to invalid IL or missing references)
		//IL_2017: Unknown result type (might be due to invalid IL or missing references)
		//IL_201c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2020: Unknown result type (might be due to invalid IL or missing references)
		//IL_202a: Unknown result type (might be due to invalid IL or missing references)
		//IL_202f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2031: Unknown result type (might be due to invalid IL or missing references)
		//IL_2036: Unknown result type (might be due to invalid IL or missing references)
		//IL_2041: Unknown result type (might be due to invalid IL or missing references)
		//IL_2043: Unknown result type (might be due to invalid IL or missing references)
		//IL_2045: Unknown result type (might be due to invalid IL or missing references)
		//IL_208c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2091: Unknown result type (might be due to invalid IL or missing references)
		//IL_2096: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_20be: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_20cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_20dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2989: Unknown result type (might be due to invalid IL or missing references)
		//IL_2992: Expected O, but got Unknown
		//IL_2966: Unknown result type (might be due to invalid IL or missing references)
		//IL_296f: Expected O, but got Unknown
		//IL_2a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a81: Expected O, but got Unknown
		//IL_2a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a97: Expected O, but got Unknown
		//IL_2a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aa2: Expected O, but got Unknown
		//IL_2ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2add: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae6: Expected O, but got Unknown
		//IL_2b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b12: Expected O, but got Unknown
		//IL_2b3c: Expected O, but got Unknown
		//IL_2a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a4f: Expected O, but got Unknown
		//IL_2a62: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a6b: Expected O, but got Unknown
		//IL_29ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f7: Expected O, but got Unknown
		//IL_29cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d4: Expected O, but got Unknown
		//IL_2b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b67: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ba7: Expected O, but got Unknown
		//IL_2bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f33: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f3c: Expected O, but got Unknown
		//IL_2f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f80: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f96: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fab: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d28: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d42: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d58: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d62: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d83: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e26: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e46: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e51: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e56: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e67: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e72: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e77: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e88: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e93: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e98: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb7: Unknown result type (might be due to invalid IL or missing references)
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
			string text = (cSDataArray.GetArrayItem(0) as CSGemData)?.GemShape;
			if (text.ToLower() == "princess" || text.ToLower() == "emerald" || text.ToLower() == "emerald square" || text.ToLower() == "emerald rotated 90")
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
		Point3d val7;
		Line val13 = default(Line);
		double num8 = default(double);
		double num9 = default(double);
		double num10 = default(double);
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
			val7 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).Center;
			double num6 = ((Point3d)(ref val7)).DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, num4).Center);
			double num7 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).GemXSize * 2.0;
			if (num6 > num7)
			{
				continue;
			}
			CSChannelGemData channelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, num4);
			CSChannelGemData channelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			CSChannelGemData channelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, num5);
			if (channelGemData2 == null || channelGemData3 == null)
			{
				return UpdateError("Invalid gems");
			}
			CSDataHelper.SetSettingType(channelGemData3, flag ? GemSettingType.BR : GemSettingType.PR);
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
			On3dVector val8 = new On3dVector(circleLeft.Center());
			On3dVector val9 = new On3dVector(circleRight.Center());
			On3dPoint value28 = new On3dPoint((IOn3dVector)(object)((val8 + (IOn3dVector)(object)val9) / 2.0));
			On3dVector value29 = val9 - (IOn3dVector)(object)val8;
			if (CSApplicationHelper.Application.IsCamMode)
			{
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
				Transform val12 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value28), CSConvert.On3dVectorCopyToVector3d(value29), destination);
				Point3d origin = Point3d.Origin;
				((Point3d)(ref origin)).Transform(val12);
				((Line)(ref val13))._002Ector(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
				((Line)(ref val13)).Transform(val12);
				Curve obj = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)new List<Point3d> { channelGemData2.ZMaxPoint, channelGemData3.ZMaxPoint, channelGemData4.ZMaxPoint }, 3);
				Curve val14 = obj.DuplicateCurve();
				Curve val15 = obj.DuplicateCurve();
				((GeometryBase)val14).Transform(Transform.Translation(0.0, 4.0, 0.0));
				((GeometryBase)val15).Transform(Transform.Translation(0.0, -4.0, 0.0));
				NurbsSurface obj2 = NurbsSurface.CreateRuledSurface(val14, val15);
				((Surface)obj2).ClosestPoint(((Line)(ref val13)).PointAt(1.0), ref num8, ref num9);
				Point3d val16 = ((Surface)obj2).PointAt(num8, num9);
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
				if (num5 == j)
				{
					val18 = val17 * -1.0;
				}
				if (RhinoMath.ToDegrees(Math.Acos(val17 * val18)) < value22)
				{
					val17 *= channelGemData3.GemXSize / 2.0;
					val18 *= channelGemData3.GemXSize / 2.0;
					Vector3d val19 = val17 + val18;
					Point3d val20 = channelGemData3.Center + val19;
					channelGemData3.GetCurve().ClosestPoint(val20, ref num10);
					OnCircle circleLeft2;
					OnCircle circleRight2;
					double height4;
					string circles2 = GetCircles(channelGemData2, channelGemData3, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value26, value27, out circleLeft2, out circleRight2, out height4);
					OnCircle circleLeft3;
					OnCircle circleRight3;
					double height5;
					string circles3 = GetCircles(channelGemData3, channelGemData4, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value26, value27, out circleLeft3, out circleRight3, out height5);
					if (!string.IsNullOrEmpty(circles2) || !string.IsNullOrEmpty(circles3))
					{
						break;
					}
					double num11 = (circleRight3.radius + circleRight2.radius + circleLeft3.radius + circleLeft2.radius) / 4.0 + value24;
					double[] array = new double[4] { circleRight3.radius, circleRight2.radius, circleLeft2.radius, circleLeft3.radius };
					if (value23 == 157)
					{
						double num12 = array[0];
						for (int k = 1; k < array.Length; k++)
						{
							if (num12 < array[k])
							{
								num12 = array[k];
							}
						}
						num11 = num12 + value24;
					}
					if (value23 == 158)
					{
						double num13 = array[0];
						for (int l = 1; l < array.Length; l++)
						{
							if (num13 > array[l])
							{
								num13 = array[l];
							}
						}
						num11 = num13 + value24;
					}
					double num14 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData3.Size) / 2.0) : channelGemData3.GemZMin);
					Point3d val21 = channelGemData3.GetCurve().PointAt(num10);
					((Point3d)(ref val21)).Transform(Transform.Translation(channelGemData3.Normal * (value9 - num14)));
					((Vector3d)(ref val19)).Unitize();
					val19 *= num11;
					val21 += val19;
					On3dPoint val22 = CSConvert.Point3dCopyToOn3dPoint(val21);
					OnCircle val23 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val22, (IOn3dVector)(object)channelGemData3.Normal.Convert()), (IOn3dPoint)(object)val22, num11);
					OnXform val24 = new OnXform();
					val24.ChangeBasis((IOnPlane)(object)val23.plane, OnUtil.On_xy_plane);
					double num15 = val23.radius * 2.0;
					if (num15 > value2 && num15 < value3)
					{
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, num15, height3, value5, value6, value, out var prong, out var prongBaseRail))
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
				double num16 = (radius + radius2) / 2.0;
				circleRight = new OnCircle(circleRight.Center(), num16);
				circleLeft = new OnCircle(circleLeft.Center(), num16);
			}
			if (!flag)
			{
				double num17 = circleLeft.radius * 2.0;
				if (num17 > value2 && num17 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num17, height3, value5, value6, value, out var prong2, out var prongBaseRail2))
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
				double num18 = circleRight.radius * 2.0;
				if (num18 > value2 && num18 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num18, height3, value5, value6, value, out var prong3, out var prongBaseRail3))
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
				val8 = new On3dVector(circleLeft.Center());
				val9 = new On3dVector(circleRight.Center());
				Brep geometry = MakeBar((val8 - (IOn3dVector)(object)val9).Length(), circleLeft.radius, circleRight.radius, height3, value6, value18, value19, value20, value21);
				value28 = new On3dPoint((IOn3dVector)(object)((val8 + (IOn3dVector)(object)val9) / 2.0));
				value29 = val9 - (IOn3dVector)(object)val8;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value28), CSConvert.On3dVectorCopyToVector3d(value29), CSConvert.On3dVectorCopyToVector3d(val29));
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
			double num22 = default(double);
			double num29 = default(double);
			Circle val52 = default(Circle);
			Line val54 = default(Line);
			Circle val91 = default(Circle);
			Line val93 = default(Line);
			for (int m = 0; m < cSDataArray.GetArrayCount(); m++)
			{
				int num19 = ((cSDataArray.GetArrayCount() > 1) ? (m + 1) : 0);
				int index = ((m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1));
				if (cSDataArray.GetArrayCount() > 1)
				{
					double num20 = 0.0;
					if (m != cSDataArray.GetArrayCount() - 1)
					{
						val7 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center;
						num20 = ((Point3d)(ref val7)).DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, m + 1).Center);
					}
					else
					{
						val7 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center;
						num20 = ((Point3d)(ref val7)).DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Center);
						num19 = 0;
					}
					double num21 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).GemXSize * 2.0;
					if (num20 <= num21)
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
						CSChannelGemData channelGemData7 = CSUtilGemRails.GetChannelGemData(cSDataArray, num19);
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
						if (num19 == m)
						{
							val31 = val30 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(val30 * val31)) < value22)
						{
							val30 *= channelGemData6.GemXSize / 2.0;
							val31 *= channelGemData6.GemXSize / 2.0;
							Vector3d val32 = val30 + val31;
							Point3d val33 = channelGemData6.Center + val32;
							channelGemData6.GetCurve().ClosestPoint(val33, ref num22);
							if (!string.IsNullOrEmpty(GetCircles(channelGemData5, channelGemData6, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value26, value27, out var circleLeft4, out var circleRight4, out var height6)))
							{
								break;
							}
							double num23 = (circleRight4.radius + circleLeft4.radius) / 2.0 + value24;
							double[] array2 = new double[2] { circleRight4.radius, circleLeft4.radius };
							if (value23 == 157)
							{
								double num24 = array2[0];
								for (int n = 1; n < array2.Length; n++)
								{
									if (num24 < array2[n])
									{
										num24 = array2[n];
									}
								}
								num23 = num24 + value24;
							}
							if (value23 == 158)
							{
								double num25 = array2[0];
								for (int num26 = 1; num26 < array2.Length; num26++)
								{
									if (num25 > array2[num26])
									{
										num25 = array2[num26];
									}
								}
								num23 = num25 + value24;
							}
							double num27 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData6.Size) / 2.0) : channelGemData6.GemZMin);
							Point3d val34 = channelGemData6.GetCurve().PointAt(num22);
							((Point3d)(ref val34)).Transform(Transform.Translation(channelGemData6.Normal * (value9 - num27)));
							((Vector3d)(ref val32)).Unitize();
							val32 *= num23;
							val34 += val32;
							On3dPoint val35 = CSConvert.Point3dCopyToOn3dPoint(val34);
							OnCircle val36 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val35, (IOn3dVector)(object)channelGemData6.Normal.Convert()), (IOn3dPoint)(object)val35, num23);
							OnXform val37 = new OnXform();
							val37.ChangeBasis((IOnPlane)(object)val36.plane, OnUtil.On_xy_plane);
							double num28 = val36.radius * 2.0;
							if (num28 > value2 && num28 < value3)
							{
								if (!_prongCache.Create((IOnNurbsCurve)(object)val, num28, height6, value5, value6, value, out var prong4, out var prongBaseRail4))
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
						CSChannelGemData channelGemData9 = CSUtilGemRails.GetChannelGemData(cSDataArray, num19);
						CSChannelGemData channelGemData10 = CSUtilGemRails.GetChannelGemData(cSDataArray, (num19 + 1 != cSDataArray.GetArrayCount()) ? (num19 + 1) : 0);
						Vector3d val38 = channelGemData9.Center - channelGemData8.Center;
						((Vector3d)(ref val38)).Unitize();
						Vector3d val39 = channelGemData9.Center - channelGemData10.Center;
						((Vector3d)(ref val39)).Unitize();
						if (num19 == m)
						{
							val39 = val38 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(val38 * val39)) < value22)
						{
							val38 *= channelGemData9.GemXSize / 2.0;
							val39 *= channelGemData9.GemXSize / 2.0;
							Vector3d val40 = val38 + val39;
							Point3d val41 = channelGemData9.Center + val40;
							channelGemData9.GetCurve().ClosestPoint(val41, ref num29);
							if (!string.IsNullOrEmpty(GetCircles(channelGemData9, channelGemData10, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value26, value27, out var circleLeft5, out var circleRight5, out var height7)))
							{
								break;
							}
							double num30 = (circleRight5.radius + circleLeft5.radius) / 2.0 + value24;
							double[] array3 = new double[2] { circleRight5.radius, circleLeft5.radius };
							if (value23 == 157)
							{
								double num31 = array3[0];
								for (int num32 = 1; num32 < array3.Length; num32++)
								{
									if (num31 < array3[num32])
									{
										num31 = array3[num32];
									}
								}
								num30 = num31 + value24;
							}
							if (value23 == 158)
							{
								double num33 = array3[0];
								for (int num34 = 1; num34 < array3.Length; num34++)
								{
									if (num33 > array3[num34])
									{
										num33 = array3[num34];
									}
								}
								num30 = num33 + value24;
							}
							double num35 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData9.Size) / 2.0) : channelGemData9.GemZMin);
							Point3d val42 = channelGemData9.GetCurve().PointAt(num29);
							((Point3d)(ref val42)).Transform(Transform.Translation(channelGemData9.Normal * (value9 - num35)));
							((Vector3d)(ref val40)).Unitize();
							val40 *= num30;
							val42 += val40;
							On3dPoint val43 = CSConvert.Point3dCopyToOn3dPoint(val42);
							OnCircle val44 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val43, (IOn3dVector)(object)channelGemData9.Normal.Convert()), (IOn3dPoint)(object)val43, num30);
							OnXform val45 = new OnXform();
							val45.ChangeBasis((IOnPlane)(object)val44.plane, OnUtil.On_xy_plane);
							double num36 = val44.radius * 2.0;
							if (num36 > value2 && num36 < value3)
							{
								if (!_prongCache.Create((IOnNurbsCurve)(object)val, num36, height7, value5, value6, value, out var prong5, out var prongBaseRail5))
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
					CSChannelGemData cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, num19).Duplicate() as CSChannelGemData;
					if (cSDataArray.GetArrayCount() == 1)
					{
						cSChannelGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData.Normal, cSChannelGemData.Center));
					}
					double num38 = cSChannelGemData.GemXSize * value12 + value7 + value8;
					double height9 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + value4 - value9) : channelGemData.GemZSize);
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num38, height9, value5, value6, value, out var prong7, out var prongBaseRail7))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj3 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
					double num39 = (cSChannelGemData.GemXSize * value12 + value7) / 2.0;
					double num40 = value9 - cSChannelGemData.GemZMin;
					On3dVector val62;
					if (val5 == OnUtil.On_UNSET_VECTOR)
					{
						val62 = new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert());
						val62.Unitize();
					}
					else
					{
						OnXform val63 = new OnXform();
						val63.Mirror((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
						val62 = new On3dVector((IOn3dVector)(object)val5);
						val62.Transform((IOnXform)(object)val63);
						val62.Unitize();
					}
					On3dVector val64 = new On3dVector((IOn3dVector)(object)cSChannelGemData.LeftVector.Convert());
					val64.Unitize();
					On3dVector val65 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val62, (IOn3dVector)(object)val64);
					val65.Unitize();
					OnPlane val66 = new OnPlane((IOn3dPoint)(object)(obj3 + (IOn3dVector)(object)(val65 * num39) + (IOn3dVector)(object)(val62 * num40)), (IOn3dVector)(object)val64, (IOn3dVector)(object)val65);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd4:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd7:
					{
						OnXform val82 = new OnXform();
						val82.ChangeBasis((IOnPlane)(object)val66, OnUtil.On_xy_plane);
						OnXform val83 = new OnXform();
						val83.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
						OnXform val84 = val83 * (IOnXform)(object)val82;
						prong7.Transform((IOnXform)(object)val84);
						prongBaseRail7.Transform((IOnXform)(object)val84);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong7).Convert(), val84.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail7).Convert(), val84.Convert(), applyTransform: false));
						break;
					}
					case ValueListItem.ProngPlacerStyleEnd5:
					case ValueListItem.ProngPlacerStyleEnd8:
					case ValueListItem.ProngPlacerStyleEnd9:
					{
						double num41 = value14 / 2.0;
						OnXform val67 = new OnXform();
						val67.ChangeBasis((IOnPlane)(object)val66, OnUtil.On_xy_plane);
						OnXform val68 = new OnXform();
						val68.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num41), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val69 = new OnXform();
						val69.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 + num41), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
						OnXform val70 = val69 * (IOnXform)(object)val67 * (IOnXform)(object)val68;
						if (!flag)
						{
							OnGeometry val71 = prong7.Duplicate();
							val71.Transform((IOnXform)(object)val70);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val71).Convert(), val70.Convert(), applyTransform: false));
							OnGeometry val72 = prongBaseRail7.Duplicate();
							val72.Transform((IOnXform)(object)val70);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val72).Convert(), val70.Convert(), applyTransform: false));
						}
						OnXform val73 = new OnXform();
						val73.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num41), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val74 = new OnXform();
						val74.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 - num41), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
						OnXform val75 = val74 * (IOnXform)(object)val67 * (IOnXform)(object)val73;
						if (!flag)
						{
							OnGeometry val76 = prong7.Duplicate();
							val76.Transform((IOnXform)(object)val75);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val76).Convert(), val75.Convert(), applyTransform: false));
							OnGeometry val77 = prongBaseRail7.Duplicate();
							val77.Transform((IOnXform)(object)val75);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val77).Convert(), val75.Convert(), applyTransform: false));
						}
						if (flag)
						{
							On3dPoint val78 = new On3dPoint(0.0, 0.0, 0.0);
							On3dPoint val79 = new On3dPoint(0.0, 0.0, 0.0);
							On3dVector val80 = new On3dVector(0.0, 0.0, 1.0);
							On3dVector val81 = new On3dVector(0.0, 0.0, 1.0);
							val78.Transform((IOnXform)(object)val70);
							val80.Transform((IOnXform)(object)val70);
							val79.Transform((IOnXform)(object)val75);
							val81.Transform((IOnXform)(object)val75);
							Brep geometry3 = MakeBar((val78 - (IOn3dPoint)(object)val79).Length(), num38 / 2.0, num38 / 2.0, height9, value6, value18, value19, value20, value21);
							On3dPoint value36 = new On3dPoint((IOn3dPoint)(object)((val78 + (IOn3dPoint)(object)val79) / 2.0));
							On3dVector value37 = val79 - (IOn3dPoint)(object)val78;
							On3dVector value38 = val80 + (IOn3dVector)(object)val81;
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
					CSChannelGemData channelGemData11 = CSUtilGemRails.GetChannelGemData(cSDataArray, num19);
					CSChannelGemData channelGemData12 = CSUtilGemRails.GetChannelGemData(cSDataArray, num19 + 1);
					if (channelGemData11 == null || channelGemData12 == null)
					{
						return UpdateError("Invalid gems");
					}
					CSDataHelper.SetSettingType(channelGemData12, flag ? GemSettingType.BR : GemSettingType.PR);
					OnCircle circleLeft6;
					OnCircle circleRight6;
					double height8;
					string circles4 = GetCircles(channelGemData11, channelGemData12, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value26, value27, out circleLeft6, out circleRight6, out height8);
					if (!string.IsNullOrEmpty(circles4))
					{
						return UpdateError(circles4);
					}
					On3dVector val46 = new On3dVector(circleLeft6.Center());
					On3dVector val47 = new On3dVector(circleRight6.Center());
					if (value25 == 157)
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
					if (value25 == 158)
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
					if (value25 == 159)
					{
						double radius3 = circleRight6.radius;
						double radius4 = circleLeft6.radius;
						double num37 = (radius3 + radius4) / 2.0;
						circleRight6 = new OnCircle(circleRight6.Center(), num37);
						circleLeft6 = new OnCircle(circleLeft6.Center(), num37);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height8 = (value26 ? (num3 * value27) : num3);
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
					On3dVector value30 = val49 + (IOn3dVector)(object)val51;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						Plane worldZX = Plane.WorldZX;
						Point3d origin2 = Point3d.Origin;
						val7 = channelGemData12.ZMaxPoint;
						((Circle)(ref val52))._002Ector(worldZX, origin2, ((Point3d)(ref val7)).DistanceTo(Point3d.Origin));
						On3dPoint value31 = new On3dPoint((IOn3dVector)(object)((val46 + (IOn3dVector)(object)val47) / 2.0));
						On3dVector value32 = val47 - (IOn3dVector)(object)val46;
						Transform val53 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value31), CSConvert.On3dVectorCopyToVector3d(value32), CSConvert.On3dVectorCopyToVector3d(value30));
						Point3d origin3 = Point3d.Origin;
						((Point3d)(ref origin3)).Transform(val53);
						((Line)(ref val54))._002Ector(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
						((Line)(ref val54)).Transform(val53);
						Point3d val55 = ((Line)(ref val54)).ClosestPoint(((Circle)(ref val52)).ClosestPoint(((Line)(ref val54)).PointAt(1.0)), true);
						if (((Point3d)(ref val55)).DistanceTo(origin3) > height8 * 0.8)
						{
							height8 = ((Point3d)(ref val55)).DistanceTo(origin3) * 1.2;
						}
					}
					if (!flag)
					{
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft6.Diameter(), height8, value5, value6, value, out var prong6, out var prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value33 = val47 - (IOn3dVector)(object)val46;
						Transform val56 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val46)), CSConvert.On3dVectorCopyToVector3d(value33), CSConvert.On3dVectorCopyToVector3d(value30));
						Vector3d val57 = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform val58 = Transform.Mirror(CSConvert.On3dPointCopyToPoint3d(channelGemData11.Center.Convert()), val57);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val58 * val56));
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight6.Diameter(), height8, value5, value6, value, out prong6, out prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						value33 = val46 - (IOn3dVector)(object)val47;
						val56 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val47)), CSConvert.On3dVectorCopyToVector3d(value33), CSConvert.On3dVectorCopyToVector3d(value30));
						val57 = channelGemData11.Center - channelGemData11.XMaxPoint;
						val58 = Transform.Mirror(CSConvert.On3dPointCopyToPoint3d(channelGemData11.Center.Convert()), val57);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val58 * val56));
					}
					else
					{
						Brep geometry2 = MakeBar((val46 - (IOn3dVector)(object)val47).Length(), circleLeft6.radius, circleRight6.radius, height8, value6, value18, value19, value20, value21);
						On3dPoint value34 = new On3dPoint((IOn3dVector)(object)((val46 + (IOn3dVector)(object)val47) / 2.0));
						On3dVector value35 = val47 - (IOn3dVector)(object)val46;
						Transform val59 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value34), CSConvert.On3dVectorCopyToVector3d(value35), CSConvert.On3dVectorCopyToVector3d(value30));
						Vector3d val60 = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform val61 = Transform.Mirror(channelGemData11.Center, val60);
						concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry2, val61 * val59));
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
					double num43 = cSChannelGemData2.GemXSize * value15 + value7 + value8;
					double height11 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData2.GemZSize + value4 - value9) : channelGemData.GemZSize);
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num43, height11, value5, value6, value, out var prong9, out var prongBaseRail9))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj4 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					double num44 = (cSChannelGemData2.GemXSize * value15 + value7) / 2.0;
					double num45 = value9 - cSChannelGemData2.GemZMin;
					On3dVector val101;
					if (val6 == OnUtil.On_UNSET_VECTOR)
					{
						val101 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert());
						val101.Unitize();
					}
					else
					{
						OnXform val102 = new OnXform();
						val102.Mirror((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
						val101 = new On3dVector((IOn3dVector)(object)val6);
						val101.Transform((IOnXform)(object)val102);
						val101.Unitize();
					}
					On3dVector val103 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.RightVector.Convert());
					val103.Unitize();
					On3dVector val104 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val101, (IOn3dVector)(object)val103);
					val104.Unitize();
					OnPlane val105 = new OnPlane((IOn3dPoint)(object)(obj4 + (IOn3dVector)(object)(val104 * num44) + (IOn3dVector)(object)(val101 * num45)), (IOn3dVector)(object)val103, (IOn3dVector)(object)val104);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd2:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd8:
					{
						OnXform val121 = new OnXform();
						val121.ChangeBasis((IOnPlane)(object)val105, OnUtil.On_xy_plane);
						OnXform val122 = new OnXform();
						val122.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
						OnXform val123 = val122 * (IOnXform)(object)val121;
						prong9.Transform((IOnXform)(object)val123);
						prongBaseRail9.Transform((IOnXform)(object)val123);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong9).Convert(), val123.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail9).Convert(), val123.Convert(), applyTransform: false));
						break;
					}
					case ValueListItem.ProngPlacerStyleEnd3:
					case ValueListItem.ProngPlacerStyleEnd7:
					case ValueListItem.ProngPlacerStyleEnd9:
					{
						double num46 = value17 / 2.0;
						OnXform val106 = new OnXform();
						val106.ChangeBasis((IOnPlane)(object)val105, OnUtil.On_xy_plane);
						OnXform val107 = new OnXform();
						val107.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num46), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val108 = new OnXform();
						val108.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 + num46), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
						OnXform val109 = val108 * (IOnXform)(object)val106 * (IOnXform)(object)val107;
						if (!flag)
						{
							OnGeometry val110 = prong9.Duplicate();
							val110.Transform((IOnXform)(object)val109);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val110).Convert(), val109.Convert(), applyTransform: false));
							OnGeometry val111 = prongBaseRail9.Duplicate();
							val111.Transform((IOnXform)(object)val109);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val111).Convert(), val109.Convert(), applyTransform: false));
						}
						OnXform val112 = new OnXform();
						val112.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num46), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val113 = new OnXform();
						val113.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 - num46), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
						OnXform val114 = val113 * (IOnXform)(object)val106 * (IOnXform)(object)val112;
						if (!flag)
						{
							OnGeometry val115 = prong9.Duplicate();
							val115.Transform((IOnXform)(object)val114);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val115).Convert(), val114.Convert(), applyTransform: false));
							OnGeometry val116 = prongBaseRail9.Duplicate();
							val116.Transform((IOnXform)(object)val114);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val116).Convert(), val114.Convert(), applyTransform: false));
						}
						if (flag)
						{
							On3dPoint val117 = new On3dPoint(0.0, 0.0, 0.0);
							On3dPoint val118 = new On3dPoint(0.0, 0.0, 0.0);
							On3dVector val119 = new On3dVector(0.0, 0.0, 1.0);
							On3dVector val120 = new On3dVector(0.0, 0.0, 1.0);
							val117.Transform((IOnXform)(object)val109);
							val119.Transform((IOnXform)(object)val109);
							val118.Transform((IOnXform)(object)val114);
							val120.Transform((IOnXform)(object)val114);
							Brep geometry5 = MakeBar((val117 - (IOn3dPoint)(object)val118).Length(), num43 / 2.0, num43 / 2.0, height11, value6, value18, value19, value20, value21);
							On3dPoint value45 = new On3dPoint((IOn3dPoint)(object)((val117 + (IOn3dPoint)(object)val118) / 2.0));
							On3dVector value46 = val118 - (IOn3dPoint)(object)val117;
							On3dVector value47 = val119 + (IOn3dVector)(object)val120;
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
					CSChannelGemData channelGemData13 = CSUtilGemRails.GetChannelGemData(cSDataArray, (m - 1 > 0) ? (m - 1) : (m + 1));
					CSChannelGemData channelGemData14 = CSUtilGemRails.GetChannelGemData(cSDataArray, m);
					if (channelGemData13 == null || channelGemData14 == null)
					{
						return UpdateError("Invalid gems");
					}
					CSDataHelper.SetSettingType(channelGemData14, flag ? GemSettingType.BR : GemSettingType.PR);
					OnCircle circleLeft7;
					OnCircle circleRight7;
					double height10;
					string circles5 = GetCircles(channelGemData13, channelGemData14, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value26, value27, out circleLeft7, out circleRight7, out height10);
					if (!string.IsNullOrEmpty(circles5))
					{
						return UpdateError(circles5);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height10 = (value26 ? (num3 * value27) : num3);
					}
					if (value25 == 157)
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
					if (value25 == 158)
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
					if (value25 == 159)
					{
						double radius5 = circleRight7.radius;
						double radius6 = circleLeft7.radius;
						double num42 = (radius5 + radius6) / 2.0;
						circleRight7 = new OnCircle(circleRight7.Center(), num42);
						circleLeft7 = new OnCircle(circleLeft7.Center(), num42);
					}
					On3dVector val85 = new On3dVector(circleLeft7.Center());
					On3dVector val86 = new On3dVector(circleRight7.Center());
					OnXform val87 = new OnXform();
					val87.ChangeBasis((IOnPlane)(object)circleLeft7.plane, OnUtil.On_xy_plane);
					On3dVector val88 = new On3dVector(OnUtil.On_zaxis);
					val88.Transform((IOnXform)(object)val87);
					val88.Unitize();
					OnXform val89 = new OnXform();
					val89.ChangeBasis((IOnPlane)(object)circleRight7.plane, OnUtil.On_xy_plane);
					On3dVector val90 = new On3dVector(OnUtil.On_zaxis);
					val90.Transform((IOnXform)(object)val89);
					val90.Unitize();
					On3dVector value39 = val88 + (IOn3dVector)(object)val90;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						Plane worldZX2 = Plane.WorldZX;
						Point3d origin4 = Point3d.Origin;
						val7 = channelGemData14.ZMaxPoint;
						((Circle)(ref val91))._002Ector(worldZX2, origin4, ((Point3d)(ref val7)).DistanceTo(Point3d.Origin));
						On3dPoint value40 = new On3dPoint((IOn3dVector)(object)((val85 + (IOn3dVector)(object)val86) / 2.0));
						On3dVector value41 = val86 - (IOn3dVector)(object)val85;
						Transform val92 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value40), CSConvert.On3dVectorCopyToVector3d(value41), CSConvert.On3dVectorCopyToVector3d(value39));
						Point3d origin5 = Point3d.Origin;
						((Point3d)(ref origin5)).Transform(val92);
						((Line)(ref val93))._002Ector(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
						((Line)(ref val93)).Transform(val92);
						Point3d val94 = ((Line)(ref val93)).ClosestPoint(((Circle)(ref val91)).ClosestPoint(((Line)(ref val93)).PointAt(1.0)), true);
						if (((Point3d)(ref val94)).DistanceTo(origin5) > height10 * 0.8)
						{
							height10 = ((Point3d)(ref val94)).DistanceTo(origin5) * 1.2;
						}
					}
					if (!flag)
					{
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft7.Diameter(), height10, value5, value6, value, out var prong8, out var prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value42 = val86 - (IOn3dVector)(object)val85;
						Transform val95 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val85)), CSConvert.On3dVectorCopyToVector3d(value42), CSConvert.On3dVectorCopyToVector3d(value39));
						Vector3d val96 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform val97 = Transform.Mirror(channelGemData14.Center, val96);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), val97 * val95));
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight7.Diameter(), height10, value5, value6, value, out prong8, out prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						value42 = val85 - (IOn3dVector)(object)val86;
						val95 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val86)), CSConvert.On3dVectorCopyToVector3d(value42), CSConvert.On3dVectorCopyToVector3d(value39));
						val96 = channelGemData14.Center - channelGemData14.XMaxPoint;
						val97 = Transform.Mirror(channelGemData14.Center, val96);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), val97 * val95));
					}
					else
					{
						Brep geometry4 = MakeBar((val85 - (IOn3dVector)(object)val86).Length(), circleLeft7.radius, circleRight7.radius, height10, value6, value18, value19, value20, value21);
						On3dPoint value43 = new On3dPoint((IOn3dVector)(object)((val85 + (IOn3dVector)(object)val86) / 2.0));
						On3dVector value44 = val86 - (IOn3dVector)(object)val85;
						Transform val98 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value43), CSConvert.On3dVectorCopyToVector3d(value44), CSConvert.On3dVectorCopyToVector3d(value39));
						Vector3d val99 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform val100 = Transform.Mirror(channelGemData14.Center, val99);
						concurrentBag.Add(new CSOnGeometryData((GeometryBase)(object)geometry4, val100 * val98));
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
