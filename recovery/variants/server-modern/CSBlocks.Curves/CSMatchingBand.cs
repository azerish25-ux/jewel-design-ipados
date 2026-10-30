using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlocks.Curves;

[CSBlock("MatchingBand", "{ECF32893-E33A-4967-8044-45D5043C27B1}")]
public sealed class CSMatchingBand : CSBlock
{
	public readonly CSInputChannel InRingRail;

	public readonly CSInputChannel InObjects;

	public readonly CSInputChannel InTopSampling;

	public readonly CSInputChannel InBotSampling;

	public readonly CSInputChannel InSampleDistance;

	public readonly CSInputChannel InPosition;

	public readonly CSInputChannel InOffset;

	public readonly CSInputChannel InOutsideRingRail;

	public readonly CSInputChannel InTaperingToggle;

	public readonly CSInputChannel InSimpleToggle;

	public readonly CSInputChannel InSimpleHeight;

	public readonly CSInputChannel InStraightBandToggle;

	public readonly CSInputChannel InMirror;

	public readonly CSInputChannel InMirrorSide;

	public readonly CSInputChannel InMinRadius;

	public readonly CSInputChannel InPullBackPercent;

	public readonly CSOutputChannel OutMatchingRail;

	public readonly CSOutputChannel OutMatchingOutsideRail;

	public CSMatchingBand(string identifier)
		: base(identifier)
	{
		InRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		InTopSampling = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InBotSampling = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InSampleDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InPosition = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InOutsideRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InTaperingToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InSimpleToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InSimpleHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InStraightBandToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirrorSide = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InMinRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 5);
		InPullBackPercent = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		OutMatchingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		OutMatchingOutsideRail = new CSOutputChannel(this, ChannelDataType.OutsideRingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_109a: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_110b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1110: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_117c: Unknown result type (might be due to invalid IL or missing references)
		//IL_119c: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1310: Unknown result type (might be due to invalid IL or missing references)
		//IL_1315: Unknown result type (might be due to invalid IL or missing references)
		//IL_1324: Unknown result type (might be due to invalid IL or missing references)
		//IL_134e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1360: Unknown result type (might be due to invalid IL or missing references)
		//IL_1372: Unknown result type (might be due to invalid IL or missing references)
		//IL_138b: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1255: Unknown result type (might be due to invalid IL or missing references)
		//IL_125a: Unknown result type (might be due to invalid IL or missing references)
		//IL_126c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1271: Unknown result type (might be due to invalid IL or missing references)
		//IL_1293: Unknown result type (might be due to invalid IL or missing references)
		//IL_1295: Unknown result type (might be due to invalid IL or missing references)
		//IL_1297: Unknown result type (might be due to invalid IL or missing references)
		//IL_129c: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e62: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e67: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e91: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2020: Unknown result type (might be due to invalid IL or missing references)
		//IL_2025: Unknown result type (might be due to invalid IL or missing references)
		//IL_2029: Unknown result type (might be due to invalid IL or missing references)
		//IL_2037: Unknown result type (might be due to invalid IL or missing references)
		//IL_203c: Unknown result type (might be due to invalid IL or missing references)
		//IL_140c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1411: Unknown result type (might be due to invalid IL or missing references)
		//IL_1415: Unknown result type (might be due to invalid IL or missing references)
		//IL_1423: Unknown result type (might be due to invalid IL or missing references)
		//IL_1428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_105d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f20: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_238e: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2108: Unknown result type (might be due to invalid IL or missing references)
		//IL_2116: Unknown result type (might be due to invalid IL or missing references)
		//IL_211d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2124: Unknown result type (might be due to invalid IL or missing references)
		//IL_2129: Unknown result type (might be due to invalid IL or missing references)
		//IL_2130: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1502: Unknown result type (might be due to invalid IL or missing references)
		//IL_1509: Unknown result type (might be due to invalid IL or missing references)
		//IL_1510: Unknown result type (might be due to invalid IL or missing references)
		//IL_1515: Unknown result type (might be due to invalid IL or missing references)
		//IL_151c: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0914: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2733: Unknown result type (might be due to invalid IL or missing references)
		//IL_1750: Unknown result type (might be due to invalid IL or missing references)
		//IL_1755: Unknown result type (might be due to invalid IL or missing references)
		//IL_1759: Unknown result type (might be due to invalid IL or missing references)
		//IL_1767: Unknown result type (might be due to invalid IL or missing references)
		//IL_176c: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_190f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1921: Unknown result type (might be due to invalid IL or missing references)
		//IL_193a: Unknown result type (might be due to invalid IL or missing references)
		//IL_196f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_226e: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_22dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_23eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_23fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2411: Unknown result type (might be due to invalid IL or missing references)
		//IL_242a: Unknown result type (might be due to invalid IL or missing references)
		//IL_245f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2484: Unknown result type (might be due to invalid IL or missing references)
		//IL_2489: Unknown result type (might be due to invalid IL or missing references)
		//IL_2496: Unknown result type (might be due to invalid IL or missing references)
		//IL_249d: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_24df: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1650: Unknown result type (might be due to invalid IL or missing references)
		//IL_1683: Unknown result type (might be due to invalid IL or missing references)
		//IL_1696: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_2703: Unknown result type (might be due to invalid IL or missing references)
		//IL_2567: Unknown result type (might be due to invalid IL or missing references)
		//IL_257a: Unknown result type (might be due to invalid IL or missing references)
		//IL_258d: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_25db: Unknown result type (might be due to invalid IL or missing references)
		//IL_2613: Unknown result type (might be due to invalid IL or missing references)
		//IL_2618: Unknown result type (might be due to invalid IL or missing references)
		//IL_2625: Unknown result type (might be due to invalid IL or missing references)
		//IL_262c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2633: Unknown result type (might be due to invalid IL or missing references)
		//IL_2638: Unknown result type (might be due to invalid IL or missing references)
		//IL_263f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2644: Unknown result type (might be due to invalid IL or missing references)
		//IL_266e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2675: Unknown result type (might be due to invalid IL or missing references)
		//IL_267c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2683: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f83: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1825: Unknown result type (might be due to invalid IL or missing references)
		//IL_1838: Unknown result type (might be due to invalid IL or missing references)
		//IL_1846: Unknown result type (might be due to invalid IL or missing references)
		//IL_184d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1854: Unknown result type (might be due to invalid IL or missing references)
		//IL_1859: Unknown result type (might be due to invalid IL or missing references)
		//IL_1860: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_19df: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a96: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea7: Unknown result type (might be due to invalid IL or missing references)
		//IL_104c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1041: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f42: Unknown result type (might be due to invalid IL or missing references)
		if (!CSDataHelper.GetCurve(InRingRail, out var value))
		{
			return UpdateError("Curve");
		}
		bool flag = false;
		if (CSDataHelper.GetCurve(InOutsideRingRail, out var value2))
		{
			flag = true;
		}
		InMirror.GetValue(out bool value3);
		InMirrorSide.GetValue(out int value4);
		InMinRadius.GetValue(out double value5);
		InPullBackPercent.GetValue(out double value6);
		CSData data = InObjects.Data;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetBoundingBox(ref boundingBox))
		{
			return UpdateError("Error getting BoundingBox");
		}
		Point3d val = ((BoundingBox)(ref boundingBox)).Min;
		double num = ((Point3d)(ref val)).Y - 1.0;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double num2 = ((Point3d)(ref val)).Y + 1.0;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double num3 = Math.Pow(((Point3d)(ref val)).X, 2.0);
		val = ((BoundingBox)(ref boundingBox)).Max;
		double val2 = Math.Sqrt(num3 + Math.Pow(((Point3d)(ref val)).Z, 2.0));
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num4 = Math.Pow(((Point3d)(ref val)).X, 2.0);
		val = ((BoundingBox)(ref boundingBox)).Max;
		double val3 = Math.Sqrt(num4 + Math.Pow(((Point3d)(ref val)).Z, 2.0));
		double num5 = Math.Max(val2, val3);
		InSampleDistance.GetValue(out double value7);
		value7 = num5 + value7;
		Surface val4 = (Surface)(object)NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(value7, num2, 0.0), new Point3d(value7, num, 0.0), new Point3d(0.0, num, 0.0));
		int value8 = 0;
		InTopSampling.GetInteger(ref value8);
		switch (value8)
		{
		case 1:
			value8 = 60;
			break;
		case 2:
			value8 = 45;
			break;
		case 3:
			value8 = 30;
			break;
		case 4:
			value8 = 18;
			break;
		case 5:
			value8 = 10;
			break;
		case 6:
			value8 = 5;
			break;
		case 7:
			value8 = 4;
			break;
		case 8:
			value8 = 2;
			break;
		case 9:
			value8 = 1;
			break;
		default:
			return UpdateError("Invalid Angle");
		}
		Transform val5 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value8), Vector3d.YAxis, Point3d.Origin);
		int value9 = 0;
		InBotSampling.GetInteger(ref value9);
		switch (value9)
		{
		case 1:
			value9 = 60;
			break;
		case 2:
			value9 = 45;
			break;
		case 3:
			value9 = 30;
			break;
		case 4:
			value9 = 18;
			break;
		case 5:
			value9 = 10;
			break;
		case 6:
			value9 = 5;
			break;
		case 7:
			value9 = 4;
			break;
		case 8:
			value9 = 2;
			break;
		case 9:
			value9 = 1;
			break;
		default:
			return UpdateError("Invalid Angle");
		}
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value9), Vector3d.YAxis, Point3d.Origin);
		InPosition.GetValue(out bool value10);
		InSimpleToggle.GetValue(out bool value11);
		double value12 = 0.0;
		InOffset.GetDouble(ref value12);
		double num6 = 0.0;
		CSDataArray flattenArray = InObjects.GetFlattenArray();
		bool value13 = false;
		InTaperingToggle.GetValue(out value13);
		if (value13)
		{
			num6 = ((!value10) ? double.MaxValue : double.MinValue);
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				if (!flattenArray.GetArrayItem(i).GetBoundingBox(ref boundingBox))
				{
					continue;
				}
				if (value10)
				{
					val = ((BoundingBox)(ref boundingBox)).Min;
					if (((Point3d)(ref val)).Y > num6)
					{
						val = ((BoundingBox)(ref boundingBox)).Min;
						if (((Point3d)(ref val)).Y < 0.0)
						{
							val = ((BoundingBox)(ref boundingBox)).Min;
							num6 = ((Point3d)(ref val)).Y;
						}
					}
					continue;
				}
				val = ((BoundingBox)(ref boundingBox)).Max;
				if (((Point3d)(ref val)).Y < num6)
				{
					val = ((BoundingBox)(ref boundingBox)).Max;
					if (((Point3d)(ref val)).Y > 0.0)
					{
						val = ((BoundingBox)(ref boundingBox)).Max;
						num6 = ((Point3d)(ref val)).Y;
					}
				}
			}
		}
		Surface val7 = null;
		Surface val8;
		if (value10)
		{
			val8 = Surface.CreateExtrusion(value, Vector3d.YAxis * (0.0 - num5));
			((GeometryBase)val8).Translate(0.0, num2, 0.0);
			if (flag)
			{
				val7 = Surface.CreateExtrusion(value2, Vector3d.YAxis * (0.0 - num5));
				((GeometryBase)val7).Translate(0.0, num2, 0.0);
			}
		}
		else
		{
			val8 = Surface.CreateExtrusion(value, Vector3d.YAxis * num5);
			((GeometryBase)val8).Translate(0.0, 0.0 - num2, 0.0);
			if (flag)
			{
				val7 = Surface.CreateExtrusion(value2, Vector3d.YAxis * num5);
				((GeometryBase)val7).Translate(0.0, 0.0 - num2, 0.0);
			}
		}
		Point3dList val9 = new Point3dList();
		Point3dList val10 = new Point3dList();
		Point3dList val11 = new Point3dList();
		double num7 = 0.0;
		InStraightBandToggle.GetValue(out bool value14);
		bool flag2 = true;
		Curve[] array = default(Curve[]);
		Point3d[] array2 = default(Point3d[]);
		Point3d val15 = default(Point3d);
		Curve[] array5 = default(Curve[]);
		Point3d[] array6 = default(Point3d[]);
		double num12 = default(double);
		double num13 = default(double);
		while (flag2)
		{
			while (num7 <= 180.0)
			{
				CurveIntersections val12 = Intersection.CurveSurface(value, val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				Point3d val13 = Point3d.Unset;
				double num8 = double.MaxValue;
				for (int j = 0; j < val12.Count; j++)
				{
					Point3d pointA = val12[j].PointA;
					double num9 = Math.Sqrt(Math.Pow(((Point3d)(ref pointA)).X, 2.0) + Math.Pow(((Point3d)(ref pointA)).Y, 2.0));
					if (num9 < num8)
					{
						num8 = num9;
						val13 = pointA;
					}
				}
				CSDataArray flattenArray2 = InObjects.GetFlattenArray();
				if (flattenArray2.GetArrayCount() <= 0)
				{
					return UpdateMissing("Mising Object");
				}
				BoundingBox unset = BoundingBox.Unset;
				for (int k = 0; k < flattenArray2.GetArrayCount(); k++)
				{
					CSData arrayItem = flattenArray2.GetArrayItem(k);
					if (arrayItem == null)
					{
						continue;
					}
					CSDataHelper.GetBrep(arrayItem, out var value15);
					if (value15 != null && Intersection.BrepSurface(value15, val4, CSApplicationHelper.Application.Tolerance, ref array, ref array2) && (array.Length >= 1 || array2.Length >= 1))
					{
						Curve[] array3 = array;
						for (int l = 0; l < array3.Length; l++)
						{
							BoundingBox boundingBox2 = ((GeometryBase)array3[l]).GetBoundingBox(true);
							((BoundingBox)(ref unset)).Union(boundingBox2);
						}
						Point3d[] array4 = array2;
						foreach (Point3d val14 in array4)
						{
							((BoundingBox)(ref unset)).Union(val14);
						}
					}
				}
				val = ((BoundingBox)(ref unset)).Min;
				double y = ((Point3d)(ref val)).Y;
				val = ((BoundingBox)(ref unset)).Max;
				double y2 = ((Point3d)(ref val)).Y;
				if (value10)
				{
					((Point3d)(ref val15))._002Ector(((Point3d)(ref val13)).X, y - value12, ((Point3d)(ref val13)).Z);
					if (value13 && y > num6)
					{
						((Point3d)(ref val15)).Y = num6 - value12;
					}
					if (value14)
					{
						((Point3d)(ref val15)).Y = ((Point3d)(ref val15)).Y - y + num + 1.0;
					}
				}
				else
				{
					((Point3d)(ref val15))._002Ector(((Point3d)(ref val13)).X, y2 + value12, ((Point3d)(ref val13)).Z);
					if (value13 && y2 < num6)
					{
						((Point3d)(ref val15)).Y = num6 + value12;
					}
					if (value14)
					{
						((Point3d)(ref val15)).Y = ((Point3d)(ref val15)).Y - y2 + num2 - 1.0;
					}
				}
				((RhinoList<Point3d>)(object)val9).Add(val15);
				if (flag)
				{
					CurveIntersections val16 = Intersection.CurveSurface(value2, val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					val13 = Point3d.Unset;
					num8 = double.MaxValue;
					for (int m = 0; m < val16.Count; m++)
					{
						Point3d pointA2 = val16[m].PointA;
						double num10 = Math.Sqrt(Math.Pow(((Point3d)(ref pointA2)).X, 2.0) + Math.Pow(((Point3d)(ref pointA2)).Y, 2.0));
						if (num10 < num8)
						{
							num8 = num10;
							val13 = pointA2;
						}
					}
					flattenArray2 = InObjects.GetFlattenArray();
					if (flattenArray2.GetArrayCount() <= 0)
					{
						return UpdateMissing("Mising Object");
					}
					unset = BoundingBox.Unset;
					for (int n = 0; n < flattenArray2.GetArrayCount(); n++)
					{
						CSData arrayItem2 = flattenArray2.GetArrayItem(n);
						if (arrayItem2 == null)
						{
							continue;
						}
						CSDataHelper.GetBrep(arrayItem2, out var value16);
						if (value16 != null && Intersection.BrepSurface(value16, val4, CSApplicationHelper.Application.Tolerance, ref array5, ref array6) && (array5.Length >= 1 || array6.Length >= 1))
						{
							Curve[] array3 = array5;
							for (int l = 0; l < array3.Length; l++)
							{
								BoundingBox boundingBox3 = ((GeometryBase)array3[l]).GetBoundingBox(true);
								((BoundingBox)(ref unset)).Union(boundingBox3);
							}
							Point3d[] array4 = array6;
							foreach (Point3d val17 in array4)
							{
								((BoundingBox)(ref unset)).Union(val17);
							}
						}
					}
					val = ((BoundingBox)(ref unset)).Min;
					y = ((Point3d)(ref val)).Y;
					val = ((BoundingBox)(ref unset)).Max;
					y2 = ((Point3d)(ref val)).Y;
					if (value10)
					{
						((Point3d)(ref val15))._002Ector(((Point3d)(ref val13)).X, y - value12, ((Point3d)(ref val13)).Z);
						if (value13 && y > num6)
						{
							((Point3d)(ref val15)).Y = num6 - value12;
						}
						if (value14)
						{
							((Point3d)(ref val15)).Y = ((Point3d)(ref val15)).Y - y + num + 1.0;
						}
					}
					else
					{
						((Point3d)(ref val15))._002Ector(((Point3d)(ref val13)).X, y2 + value12, ((Point3d)(ref val13)).Z);
						if (value13 && y2 < num6)
						{
							((Point3d)(ref val15)).Y = num6 + value12;
						}
						if (value14)
						{
							((Point3d)(ref val15)).Y = ((Point3d)(ref val15)).Y - y2 + num2 - 1.0;
						}
					}
					((RhinoList<Point3d>)(object)val10).Add(val15);
				}
				if (num7 == 180.0)
				{
					break;
				}
				num7 += (double)value9;
				((GeometryBase)val4).Transform(val6);
			}
			flag2 = false;
			Curve val18 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val9, 3);
			for (int num11 = 1; num11 < ((RhinoList<Point3d>)(object)val9).Count; num11++)
			{
				val18.ClosestPoint(((RhinoList<Point3d>)(object)val9)[num11], ref num12);
				val18.ClosestPoint(((RhinoList<Point3d>)(object)val9)[num11 - 1], ref num13);
				Vector3d val19 = val18.TangentAt(num12);
				((Vector3d)(ref val19)).Unitize();
				Vector3d val20 = val18.TangentAt(num13);
				((Vector3d)(ref val20)).Unitize();
				CSBlockUtil.CSUtilMath.ToDegrees(Vector3d.VectorAngle(val19, val20));
				_ = 80.0 / num2;
			}
		}
		Curve[] array7 = default(Curve[]);
		Point3d[] array8 = default(Point3d[]);
		Point3d val24 = default(Point3d);
		Curve[] array9 = default(Curve[]);
		Point3d[] array10 = default(Point3d[]);
		while (num7 <= 360.0)
		{
			CurveIntersections val21 = Intersection.CurveSurface(value, val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			Point3d val22 = Point3d.Unset;
			double num14 = double.MaxValue;
			for (int num15 = 0; num15 < val21.Count; num15++)
			{
				Point3d pointA3 = val21[num15].PointA;
				double num16 = Math.Sqrt(Math.Pow(((Point3d)(ref pointA3)).X, 2.0) + Math.Pow(((Point3d)(ref pointA3)).Y, 2.0));
				if (num16 < num14)
				{
					num14 = num16;
					val22 = pointA3;
				}
			}
			CSDataArray flattenArray3 = InObjects.GetFlattenArray();
			if (flattenArray3.GetArrayCount() <= 0)
			{
				return UpdateMissing("Mising Object");
			}
			double num17 = double.MaxValue;
			double num18 = double.MinValue;
			for (int num19 = 0; num19 < flattenArray3.GetArrayCount(); num19++)
			{
				CSData arrayItem3 = flattenArray3.GetArrayItem(num19);
				if (arrayItem3 == null)
				{
					continue;
				}
				CSDataHelper.GetBrep(arrayItem3, out var value17);
				if (value17 != null && Intersection.BrepSurface(value17, val4, CSApplicationHelper.Application.Tolerance, ref array7, ref array8) && (array7.Length >= 1 || array8.Length >= 1))
				{
					BoundingBox unset2 = BoundingBox.Unset;
					Curve[] array3 = array7;
					for (int l = 0; l < array3.Length; l++)
					{
						BoundingBox boundingBox4 = ((GeometryBase)array3[l]).GetBoundingBox(true);
						((BoundingBox)(ref unset2)).Union(boundingBox4);
					}
					Point3d[] array4 = array8;
					foreach (Point3d val23 in array4)
					{
						((BoundingBox)(ref unset2)).Union(val23);
					}
					val = ((BoundingBox)(ref unset2)).Min;
					if (((Point3d)(ref val)).Y < num17)
					{
						val = ((BoundingBox)(ref unset2)).Min;
						num17 = ((Point3d)(ref val)).Y;
					}
					val = ((BoundingBox)(ref unset2)).Max;
					if (((Point3d)(ref val)).Y > num18)
					{
						val = ((BoundingBox)(ref unset2)).Max;
						num18 = ((Point3d)(ref val)).Y;
					}
				}
			}
			if (num18 < num17)
			{
				val = ((RhinoList<Point3d>)(object)val9).Last;
				num17 = ((Point3d)(ref val)).Y + value12;
				val = ((RhinoList<Point3d>)(object)val9).Last;
				num18 = ((Point3d)(ref val)).Y - value12;
			}
			if (value10)
			{
				((Point3d)(ref val24))._002Ector(((Point3d)(ref val22)).X, num17 - value12, ((Point3d)(ref val22)).Z);
				if (value13 && num17 > num6)
				{
					((Point3d)(ref val24)).Y = num6 - value12;
				}
				if (value14)
				{
					((Point3d)(ref val24)).Y = ((Point3d)(ref val24)).Y - num17 + num + 1.0;
				}
			}
			else
			{
				((Point3d)(ref val24))._002Ector(((Point3d)(ref val22)).X, num18 + value12, ((Point3d)(ref val22)).Z);
				if (value13 && num18 < num6)
				{
					((Point3d)(ref val24)).Y = num6 + value12;
				}
				if (value14)
				{
					((Point3d)(ref val24)).Y = ((Point3d)(ref val24)).Y - num18 + num2 - 1.0;
				}
			}
			((RhinoList<Point3d>)(object)val9).Add(val24);
			if (flag)
			{
				CurveIntersections val25 = Intersection.CurveSurface(value2, val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				val22 = Point3d.Unset;
				num14 = double.MaxValue;
				for (int num20 = 0; num20 < val25.Count; num20++)
				{
					Point3d pointA4 = val25[num20].PointA;
					double num21 = Math.Sqrt(Math.Pow(((Point3d)(ref pointA4)).X, 2.0) + Math.Pow(((Point3d)(ref pointA4)).Y, 2.0));
					if (num21 < num14)
					{
						num14 = num21;
						val22 = pointA4;
					}
				}
				flattenArray3 = InObjects.GetFlattenArray();
				if (flattenArray3.GetArrayCount() <= 0)
				{
					return UpdateMissing("Mising Object");
				}
				num17 = double.MaxValue;
				num18 = double.MinValue;
				for (int num22 = 0; num22 < flattenArray3.GetArrayCount(); num22++)
				{
					CSData arrayItem4 = flattenArray3.GetArrayItem(num22);
					if (arrayItem4 == null)
					{
						continue;
					}
					CSDataHelper.GetBrep(arrayItem4, out var value18);
					if (value18 != null && Intersection.BrepSurface(value18, val4, CSApplicationHelper.Application.Tolerance, ref array9, ref array10) && (array9.Length >= 1 || array10.Length >= 1))
					{
						BoundingBox unset3 = BoundingBox.Unset;
						Curve[] array3 = array9;
						for (int l = 0; l < array3.Length; l++)
						{
							BoundingBox boundingBox5 = ((GeometryBase)array3[l]).GetBoundingBox(true);
							((BoundingBox)(ref unset3)).Union(boundingBox5);
						}
						Point3d[] array4 = array10;
						foreach (Point3d val26 in array4)
						{
							((BoundingBox)(ref unset3)).Union(val26);
						}
						val = ((BoundingBox)(ref unset3)).Min;
						if (((Point3d)(ref val)).Y < num17)
						{
							val = ((BoundingBox)(ref unset3)).Min;
							num17 = ((Point3d)(ref val)).Y;
						}
						val = ((BoundingBox)(ref unset3)).Max;
						if (((Point3d)(ref val)).Y > num18)
						{
							val = ((BoundingBox)(ref unset3)).Max;
							num18 = ((Point3d)(ref val)).Y;
						}
					}
				}
				if (num18 < num17)
				{
					val = ((RhinoList<Point3d>)(object)val9).Last;
					num17 = ((Point3d)(ref val)).Y + value12;
					val = ((RhinoList<Point3d>)(object)val9).Last;
					num18 = ((Point3d)(ref val)).Y - value12;
				}
				if (value10)
				{
					((Point3d)(ref val24))._002Ector(((Point3d)(ref val22)).X, num17 - value12, ((Point3d)(ref val22)).Z);
					if (value13 && num17 > num6)
					{
						((Point3d)(ref val24)).Y = num6 - value12;
					}
					if (value14)
					{
						((Point3d)(ref val24)).Y = ((Point3d)(ref val24)).Y - num17 + num + 1.0;
					}
				}
				else
				{
					((Point3d)(ref val24))._002Ector(((Point3d)(ref val22)).X, num18 + value12, ((Point3d)(ref val22)).Z);
					if (value13 && num18 < num6)
					{
						((Point3d)(ref val24)).Y = num6 + value12;
					}
					if (value14)
					{
						((Point3d)(ref val24)).Y = ((Point3d)(ref val24)).Y - num18 + num2 - 1.0;
					}
				}
				if (value11)
				{
					((RhinoList<Point3d>)(object)val11).Add(val24);
				}
				else
				{
					((RhinoList<Point3d>)(object)val10).Add(val24);
				}
			}
			num7 += (double)value8;
			((GeometryBase)val4).Transform(val5);
		}
		Curve val27 = Curve.CreateControlPointCurve((IEnumerable<Point3d>)val9, 3);
		if (val27 == null)
		{
			return UpdateError("Error getting control point curve");
		}
		Curve val28 = val27.DuplicateCurve();
		((GeometryBase)val28).Transform(Transform.Scale(Plane.WorldXY, 1.5, 1.0, 1.5));
		Curve val29 = val27.DuplicateCurve();
		((GeometryBase)val29).Transform(Transform.Scale(Plane.WorldXY, 0.5, 1.0, 0.5));
		Brep[] array11 = Brep.CreateFromLoft((IEnumerable<Curve>)(object)new Curve[2] { val29, val28 }, Point3d.Unset, Point3d.Unset, (LoftType)0, false);
		if (array11.Length < 1)
		{
			return UpdateError("Error getting loft surface");
		}
		Curve[] array12 = default(Curve[]);
		Point3d[] array13 = default(Point3d[]);
		Intersection.BrepSurface(array11[0], val8, CSApplicationHelper.Application.Tolerance, ref array12, ref array13);
		if (array12.Length < 1)
		{
			return UpdateError("Error getting loft intersect");
		}
		Curve val30 = array12[0];
		double num24;
		double num27 = default(double);
		double num26;
		double num25;
		if (flag)
		{
			Curve obj = Curve.CreateControlPointCurve((IEnumerable<Point3d>)val10, 3);
			Curve val31 = obj.DuplicateCurve();
			((GeometryBase)val31).Transform(Transform.Scale(Plane.WorldXY, 1.5, 1.0, 1.5));
			Curve val32 = obj.DuplicateCurve();
			((GeometryBase)val32).Transform(Transform.Scale(Plane.WorldXY, 0.5, 1.0, 0.5));
			Brep[] array14 = Brep.CreateFromLoft((IEnumerable<Curve>)(object)new Curve[2] { val32, val31 }, Point3d.Unset, Point3d.Unset, (LoftType)0, false);
			if (array14.Length < 1)
			{
				return UpdateError("Error getting loft surface");
			}
			Intersection.BrepSurface(array14[0], val7, CSApplicationHelper.Application.Tolerance, ref array12, ref array13);
			Curve val33 = array12[0];
			if (value11)
			{
				Curve val34 = Curve.CreateControlPointCurve((IEnumerable<Point3d>)val11, 3);
				((GeometryBase)val34).MakeDeformable();
				Point3d val35 = val34.PointAtNormalizedLength(0.5);
				InSimpleHeight.GetValue(out double value19);
				Point3d pointAtStart = val34.PointAtStart;
				Point3d val36 = default(Point3d);
				((Point3d)(ref val36))._002Ector(0.0, ((Point3d)(ref pointAtStart)).Y, 0.0);
				Vector3d val37 = pointAtStart - val36;
				((GeometryBase)val34).Transform(Transform.Scale(Plane.WorldXY, 1.0, 1.0, (value19 + ((Vector3d)(ref val37)).Length) / ((Point3d)(ref val35)).Z));
				Curve[] array15 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { val33, val34 }, 0.01);
				if (array15.Length < 1)
				{
					return UpdateError();
				}
				val33 = array15[0];
			}
			val = val33.PointAtStart;
			double newScale = ((Point3d)(ref val)).DistanceTo(val33.PointAtNormalizedLength(0.5)) / 2.0;
			val4 = (Surface)(object)NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(0.0, num2, value7), new Point3d(0.0, num, value7), new Point3d(0.0, num, 0.0));
			CurveIntersections val38 = Intersection.CurveSurface(val33, val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			double num23 = default(double);
			val33.ClosestPoint(val38[0].PointA, ref num23);
			val33.ChangeClosedCurveSeam(num23);
			if (!Curve.DoDirectionsMatch(val33, value2))
			{
				val33.Reverse();
			}
			num24 = 0.0;
			num25 = 0.0;
			num26 = value5;
			double num28 = default(double);
			while (num24 <= 90.0)
			{
				Point3d val39 = val33.PointAtNormalizedLength(num25);
				val33.ClosestPoint(val39, ref num27);
				Vector3d val40 = val33.CurvatureAt(num27);
				if (1.0 / ((Vector3d)(ref val40)).Length < num26)
				{
					val33.NormalizedLengthParameter(num27, ref num28);
					double num29 = num28;
					double num30 = num28;
					if (num28 == 0.0)
					{
						num29 = 1.0;
					}
					if (num28 == 1.0)
					{
						num30 = 0.0;
					}
					num25 = num29 * val33.GetLength();
					double num31 = num25 - num26 / 2.0 * value6;
					val33.ClosestPoint(val33.PointAtLength(num31), ref num31);
					num25 = num30 * val33.GetLength();
					double num32 = num25 + num26 / 2.0 * value6;
					val33.ClosestPoint(val33.PointAtLength(num32), ref num32);
					Curve val41 = val33.Trim(new Interval(num32, num31));
					NurbsCurve item = CSUtilCurves.BlendCurve(val41.PointAtStart, val41.PointAtEnd, -val41.TangentAtStart, val41.TangentAtEnd);
					val33 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve>
					{
						val41,
						(Curve)(object)item
					})[0].DuplicateCurve();
				}
				num24 += (double)value8 / 3.0;
				num25 += (double)value8 / 3.0 / 360.0;
			}
			double num33 = default(double);
			while (num24 < 270.0)
			{
				Point3d val42 = val33.PointAtNormalizedLength(num25);
				val33.ClosestPoint(val42, ref num27);
				Vector3d val43 = val33.CurvatureAt(num27);
				if (1.0 / ((Vector3d)(ref val43)).Length < num26)
				{
					val33.NormalizedLengthParameter(num27, ref num33);
					double num34 = num33;
					double num35 = num33;
					if (num33 == 0.0)
					{
						num34 = 1.0;
					}
					if (num33 == 1.0)
					{
						num35 = 0.0;
					}
					num25 = num34 * val33.GetLength();
					double num36 = num25 - num26 / 2.0 * value6;
					val33.ClosestPoint(val33.PointAtLength(num36), ref num36);
					num25 = num35 * val33.GetLength();
					double num37 = num25 + num26 / 2.0 * value6;
					val33.ClosestPoint(val33.PointAtLength(num37), ref num37);
					Curve val44 = val33.Trim(new Interval(num37, num36));
					NurbsCurve item2 = CSUtilCurves.BlendCurve(val44.PointAtStart, val44.PointAtEnd, -val44.TangentAtStart, val44.TangentAtEnd);
					val33 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve>
					{
						val44,
						(Curve)(object)item2
					})[0].DuplicateCurve();
				}
				num24 += (double)value9 / 3.0;
				num25 += (double)value9 / 3.0 / 360.0;
			}
			double num38 = default(double);
			while (num24 <= 360.0)
			{
				Point3d val45 = val33.PointAtNormalizedLength(num25);
				val33.ClosestPoint(val45, ref num27);
				Vector3d val46 = val33.CurvatureAt(num27);
				if (1.0 / ((Vector3d)(ref val46)).Length < num26)
				{
					val33.NormalizedLengthParameter(num27, ref num38);
					double num39 = num38;
					double num40 = num38;
					if (num38 == 0.0)
					{
						num39 = 1.0;
					}
					if (num38 == 1.0)
					{
						num40 = 0.0;
					}
					num25 = num39 * val33.GetLength();
					double num41 = num25 - num26 / 2.0 * value6;
					val33.ClosestPoint(val33.PointAtLength(num41), ref num41);
					num25 = num40 * val33.GetLength();
					double num42 = num25 + num26 / 2.0 * value6;
					val33.ClosestPoint(val33.PointAtLength(num42), ref num42);
					Curve val47 = val33.Trim(new Interval(num42, num41));
					NurbsCurve item3 = CSUtilCurves.BlendCurve(val47.PointAtStart, val47.PointAtEnd, -val47.TangentAtStart, val47.TangentAtEnd);
					val33 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve>
					{
						val47,
						(Curve)(object)item3
					})[0].DuplicateCurve();
				}
				num24 += (double)value8 / 3.0;
				num25 += (double)value8 / 3.0 / 360.0;
			}
			val4 = (Surface)(object)NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(0.0, num2, value7), new Point3d(0.0, num, value7), new Point3d(0.0, num, 0.0));
			val38 = Intersection.CurveSurface(val33, val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			val33.ClosestPoint(val38[0].PointA, ref num23);
			val33.ChangeClosedCurveSeam(num23);
			if (!Curve.DoDirectionsMatch(val33, value2))
			{
				val33.Reverse();
			}
			if (value3)
			{
				double num43 = default(double);
				double num44 = default(double);
				if (value4 == 0)
				{
					val33.NormalizedLengthParameter(0.01, ref num43);
					NurbsSurface val48 = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(1.0, num2, 0.0 - value7), new Point3d(1.0, num, 0.0 - value7), new Point3d(0.0, num, 0.0));
					CurveIntersections val49 = Intersection.CurveSurface(val33, (Surface)(object)val48, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					val33.ClosestPoint(val49[0].PointA, ref num44);
					val33 = val33.Trim(num43, num44);
					Curve val50 = val33.DuplicateCurve();
					((GeometryBase)val50).Transform(Transform.Mirror(Plane.WorldYZ));
					NurbsCurve val51 = CSUtilCurves.BlendCurve(val33.PointAtStart, val50.PointAtStart, -val33.TangentAtStart, -val50.TangentAtStart);
					NurbsCurve val52 = CSUtilCurves.BlendCurve(val33.PointAtEnd, val50.PointAtEnd, val33.TangentAtEnd, val50.TangentAtEnd);
					CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[4]
					{
						val33,
						(Curve)val51,
						val50,
						(Curve)val52
					}, out Curve mergedCrv, 0.01);
					val33 = mergedCrv;
				}
				if (value4 == 1)
				{
					NurbsSurface val53 = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(-1.0, num2, 0.0 - value7), new Point3d(-1.0, num, 0.0 - value7), new Point3d(0.0, num, 0.0));
					CurveIntersections val54 = Intersection.CurveSurface(val33, (Surface)(object)val53, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					val33.ClosestPoint(val54[0].PointA, ref num43);
					val33.NormalizedLengthParameter(0.99, ref num44);
					val33 = val33.Trim(num43, num44);
					Curve val55 = val33.DuplicateCurve();
					((GeometryBase)val55).Transform(Transform.Mirror(Plane.WorldYZ));
					NurbsCurve val56 = CSUtilCurves.BlendCurve(val33.PointAtStart, val55.PointAtStart, -val33.TangentAtStart, -val55.TangentAtStart);
					NurbsCurve val57 = CSUtilCurves.BlendCurve(val33.PointAtEnd, val55.PointAtEnd, val33.TangentAtEnd, val55.TangentAtEnd);
					CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[4]
					{
						val33,
						(Curve)val56,
						val55,
						(Curve)val57
					}, out Curve mergedCrv2, 0.01);
					val33 = mergedCrv2;
				}
				val38 = Intersection.CurveSurface(val33, val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				val33.ClosestPoint(val38[0].PointA, ref num23);
				val33.ChangeClosedCurveSeam(num23);
				if (!Curve.DoDirectionsMatch(val33, value2))
				{
					val33.Reverse();
				}
			}
			OnNurbsCurve nCrv = ((OnCurve)Interop.ToOnCurve(val33)).NurbsCurve();
			OutMatchingOutsideRail.SetData(new CSOutsideRingRailData((Curve)(object)nCrv.Convert(), newScale));
		}
		val4 = (Surface)(object)NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(0.0, num2, value7), new Point3d(0.0, num, value7), new Point3d(0.0, num, 0.0));
		CurveIntersections val58 = Intersection.CurveSurface(val30, val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		double num45 = default(double);
		val30.ClosestPoint(val58[0].PointA, ref num45);
		val30.ChangeClosedCurveSeam(num45);
		if (!Curve.DoDirectionsMatch(val30, value))
		{
			val30.Reverse();
		}
		val = val30.PointAtNormalizedLength(0.25);
		double radius = ((Point3d)(ref val)).DistanceTo(val30.PointAtNormalizedLength(0.75)) / 2.0;
		num24 = 0.0;
		num27 = 0.0;
		num25 = 0.0;
		num26 = value5;
		double num47 = default(double);
		while (num24 <= 90.0)
		{
			Point3d val59 = val30.PointAtNormalizedLength(num25);
			val30.ClosestPoint(val59, ref num27);
			Point3d val60 = val30.PointAt(num27);
			Vector3d val61 = val30.CurvatureAt(num27);
			double num46 = 1.0 / ((Vector3d)(ref val61)).Length;
			((Vector3d)(ref val61)).Unitize();
			Point3d val62 = val60 + val61 * num46;
			new Line(val60, val62);
			if (num46 < num26)
			{
				val30.NormalizedLengthParameter(num27, ref num47);
				double num48 = num47;
				double num49 = num47;
				if (num47 == 0.0)
				{
					num48 = 1.0;
				}
				if (num47 == 1.0)
				{
					num49 = 0.0;
				}
				num25 = num48 * val30.GetLength();
				double num50 = num25 - num26 / 2.0 * value6;
				val30.ClosestPoint(val30.PointAtLength(num50), ref num50);
				num25 = num49 * val30.GetLength();
				double num51 = num25 + num26 / 2.0 * value6;
				val30.ClosestPoint(val30.PointAtLength(num51), ref num51);
				Curve val63 = val30.Trim(new Interval(num51, num50));
				NurbsCurve item4 = CSUtilCurves.BlendCurve(val63.PointAtStart, val63.PointAtEnd, -val63.TangentAtStart, val63.TangentAtEnd);
				val30 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve>
				{
					val63,
					(Curve)(object)item4
				})[0].DuplicateCurve();
			}
			num24 += (double)value8 / 3.0;
			num25 += (double)value8 / 3.0 / 360.0;
		}
		double num52 = default(double);
		while (num24 < 270.0)
		{
			Point3d val64 = val30.PointAtNormalizedLength(num25);
			val30.ClosestPoint(val64, ref num27);
			Vector3d val65 = val30.CurvatureAt(num27);
			if (1.0 / ((Vector3d)(ref val65)).Length < num26)
			{
				val30.NormalizedLengthParameter(num27, ref num52);
				double num53 = num52;
				double num54 = num52;
				if (num52 == 0.0)
				{
					num53 = 1.0;
				}
				if (num52 == 1.0)
				{
					num54 = 0.0;
				}
				num25 = num53 * val30.GetLength();
				double num55 = num25 - num26 / 2.0 * value6;
				val30.ClosestPoint(val30.PointAtLength(num55), ref num55);
				num25 = num54 * val30.GetLength();
				double num56 = num25 + num26 / 2.0 * value6;
				val30.ClosestPoint(val30.PointAtLength(num56), ref num56);
				Curve val66 = val30.Trim(new Interval(num56, num55));
				NurbsCurve item5 = CSUtilCurves.BlendCurve(val66.PointAtStart, val66.PointAtEnd, -val66.TangentAtStart, val66.TangentAtEnd);
				val30 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve>
				{
					val66,
					(Curve)(object)item5
				})[0].DuplicateCurve();
			}
			num24 += (double)value9 / 3.0;
			num25 += (double)value9 / 3.0 / 360.0;
		}
		double num57 = default(double);
		while (num24 <= 360.0)
		{
			Point3d val67 = val30.PointAtNormalizedLength(num25);
			val30.ClosestPoint(val67, ref num27);
			val30.PointAt(num27);
			Vector3d val68 = val30.CurvatureAt(num27);
			if (1.0 / ((Vector3d)(ref val68)).Length < num26)
			{
				val30.NormalizedLengthParameter(num27, ref num57);
				double num58 = num57;
				double num59 = num57;
				if (num57 == 0.0)
				{
					num58 = 1.0;
				}
				if (num57 == 1.0)
				{
					num59 = 0.0;
				}
				num25 = num58 * val30.GetLength();
				double num60 = num25 - num26 / 2.0 * value6;
				val30.ClosestPoint(val30.PointAtLength(num60), ref num60);
				num25 = num59 * val30.GetLength();
				double num61 = num25 + num26 / 2.0 * value6;
				val30.ClosestPoint(val30.PointAtLength(num61), ref num61);
				Curve val69 = val30.Trim(new Interval(num61, num60));
				NurbsCurve item6 = CSUtilCurves.BlendCurve(val69.PointAtStart, val69.PointAtEnd, -val69.TangentAtStart, val69.TangentAtEnd);
				val30 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve>
				{
					val69,
					(Curve)(object)item6
				})[0].DuplicateCurve();
			}
			num24 += (double)value8 / 3.0;
			num25 += (double)value8 / 3.0 / 360.0;
		}
		val58 = Intersection.CurveSurface(val30, val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		val30.ClosestPoint(val58[0].PointA, ref num45);
		val30.ChangeClosedCurveSeam(num45);
		if (!Curve.DoDirectionsMatch(val30, value))
		{
			val30.Reverse();
		}
		if (value3)
		{
			double num62 = default(double);
			double num63 = default(double);
			if (value4 == 0)
			{
				val30.NormalizedLengthParameter(0.01, ref num62);
				NurbsSurface val70 = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(1.0, num2, 0.0 - value7), new Point3d(1.0, num, 0.0 - value7), new Point3d(0.0, num, 0.0));
				CurveIntersections val71 = Intersection.CurveSurface(val30, (Surface)(object)val70, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				val30.ClosestPoint(val71[0].PointA, ref num63);
				val30 = val30.Trim(num62, num63);
				Curve val72 = val30.DuplicateCurve();
				((GeometryBase)val72).Transform(Transform.Mirror(Plane.WorldYZ));
				NurbsCurve val73 = CSUtilCurves.BlendCurve(val30.PointAtStart, val72.PointAtStart, -val30.TangentAtStart, -val72.TangentAtStart);
				NurbsCurve val74 = CSUtilCurves.BlendCurve(val30.PointAtEnd, val72.PointAtEnd, val30.TangentAtEnd, val72.TangentAtEnd);
				CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[4]
				{
					val30,
					(Curve)val73,
					val72,
					(Curve)val74
				}, out Curve mergedCrv3, 0.01);
				val30 = mergedCrv3;
			}
			if (value4 == 1)
			{
				NurbsSurface val75 = NurbsSurface.CreateFromCorners(new Point3d(0.0, num2, 0.0), new Point3d(-1.0, num2, 0.0 - value7), new Point3d(-1.0, num, 0.0 - value7), new Point3d(0.0, num, 0.0));
				CurveIntersections val76 = Intersection.CurveSurface(val30, (Surface)(object)val75, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				val30.ClosestPoint(val76[0].PointA, ref num62);
				val30.NormalizedLengthParameter(0.99, ref num63);
				val30 = val30.Trim(num62, num63);
				Curve val77 = val30.DuplicateCurve();
				((GeometryBase)val77).Transform(Transform.Mirror(Plane.WorldYZ));
				NurbsCurve val78 = CSUtilCurves.BlendCurve(val30.PointAtStart, val77.PointAtStart, -val30.TangentAtStart, -val77.TangentAtStart);
				NurbsCurve val79 = CSUtilCurves.BlendCurve(val30.PointAtEnd, val77.PointAtEnd, val30.TangentAtEnd, val77.TangentAtEnd);
				CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[4]
				{
					val30,
					(Curve)val78,
					val77,
					(Curve)val79
				}, out Curve mergedCrv4, 0.01);
				val30 = mergedCrv4;
			}
			val58 = Intersection.CurveSurface(val30, val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			val30.ClosestPoint(val58[0].PointA, ref num45);
			val30.ChangeClosedCurveSeam(num45);
			if (!Curve.DoDirectionsMatch(val30, value))
			{
				val30.Reverse();
			}
		}
		OnNurbsCurve nCrv2 = ((OnCurve)Interop.ToOnCurve(val30)).NurbsCurve();
		OutMatchingRail.SetData(new CSRingRailData((Curve)(object)nCrv2.Convert(), radius));
		return base.Update();
	}
}
