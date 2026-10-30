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
		//IL_0ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0add: Expected O, but got Unknown
		//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aeb: Expected O, but got Unknown
		//IL_0b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Expected O, but got Unknown
		//IL_0b5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b62: Expected O, but got Unknown
		//IL_116b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1172: Expected O, but got Unknown
		//IL_118b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1192: Expected O, but got Unknown
		//IL_11a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11aa: Expected O, but got Unknown
		//IL_11c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ca: Expected O, but got Unknown
		//IL_1219: Unknown result type (might be due to invalid IL or missing references)
		//IL_1220: Expected O, but got Unknown
		//IL_1202: Unknown result type (might be due to invalid IL or missing references)
		//IL_1209: Expected O, but got Unknown
		//IL_26f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2703: Unknown result type (might be due to invalid IL or missing references)
		//IL_270d: Expected O, but got Unknown
		//IL_270d: Expected O, but got Unknown
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
		//IL_2785: Unknown result type (might be due to invalid IL or missing references)
		//IL_278c: Expected O, but got Unknown
		//IL_274c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2753: Expected O, but got Unknown
		//IL_27a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b0: Expected O, but got Unknown
		//IL_27eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_27f2: Expected O, but got Unknown
		//IL_2819: Unknown result type (might be due to invalid IL or missing references)
		//IL_2820: Expected O, but got Unknown
		//IL_282f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2834: Unknown result type (might be due to invalid IL or missing references)
		//IL_2860: Expected O, but got Unknown
		//IL_28c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ca: Expected O, but got Unknown
		//IL_28df: Unknown result type (might be due to invalid IL or missing references)
		//IL_28e8: Expected O, but got Unknown
		//IL_2912: Unknown result type (might be due to invalid IL or missing references)
		//IL_2917: Unknown result type (might be due to invalid IL or missing references)
		//IL_294c: Expected O, but got Unknown
		//IL_162c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1633: Expected O, but got Unknown
		//IL_163a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1641: Expected O, but got Unknown
		//IL_29e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f1: Expected O, but got Unknown
		//IL_2a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a55: Expected O, but got Unknown
		//IL_25fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2603: Expected O, but got Unknown
		//IL_25e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ec: Expected O, but got Unknown
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
		//IL_2d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7b: Expected O, but got Unknown
		//IL_2d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d91: Expected O, but got Unknown
		//IL_1d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6b: Expected O, but got Unknown
		//IL_1d66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6d: Expected O, but got Unknown
		//IL_1d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d74: Expected O, but got Unknown
		//IL_3638: Unknown result type (might be due to invalid IL or missing references)
		//IL_364d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3657: Expected O, but got Unknown
		//IL_3657: Expected O, but got Unknown
		//IL_22d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e7: Expected O, but got Unknown
		//IL_22e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e9: Expected O, but got Unknown
		//IL_22e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f0: Expected O, but got Unknown
		//IL_36c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_36cc: Expected O, but got Unknown
		//IL_36fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3704: Expected O, but got Unknown
		//IL_36aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_36b3: Expected O, but got Unknown
		//IL_2ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_2df6: Expected O, but got Unknown
		//IL_2dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dd3: Expected O, but got Unknown
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
		//IL_383e: Expected O, but got Unknown
		//IL_38c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_38ce: Expected O, but got Unknown
		//IL_38e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f0: Expected O, but got Unknown
		//IL_391c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3921: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_3a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a0d: Expected O, but got Unknown
		//IL_3a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a7d: Expected O, but got Unknown
		//IL_34b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_34bb: Expected O, but got Unknown
		//IL_348f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3498: Expected O, but got Unknown
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
			double num7 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, num5).Center);
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
				zAxis.Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val9));
				zAxis.Unitize();
				OnXform val10 = new OnXform();
				val10.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
				Vector3d zAxis2 = Vector3d.ZAxis;
				zAxis2.Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val10));
				zAxis2.Unitize();
				Vector3d destination = zAxis + zAxis2;
				destination.Unitize();
				Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value53), CSConvert.On3dVectorCopyToVector3d(value54), destination);
				Point3d origin = Point3d.Origin;
				origin.Transform(xform);
				Line line = new Line(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
				line.Transform(xform);
				Curve curve = Curve.CreateInterpolatedCurve(new List<Point3d> { channelGemData2.ZMaxPoint, channelGemData3.ZMaxPoint, channelGemData4.ZMaxPoint }, 3);
				Curve curve2 = curve.DuplicateCurve();
				Curve curve3 = curve.DuplicateCurve();
				Vector3d vector3d = CSConvert.On3dVectorCopyToVector3d(circleLeft.plane.yaxis);
				curve2.Transform(Transform.Translation(4.0 * vector3d));
				curve3.Transform(Transform.Translation(-4.0 * vector3d));
				NurbsSurface nurbsSurface = NurbsSurface.CreateRuledSurface(curve2, curve3);
				nurbsSurface.ClosestPoint(line.PointAt(1.0), out var u, out var v);
				Point3d point3d = nurbsSurface.PointAt(u, v);
				if (point3d.DistanceTo(origin) > height3 * 0.8)
				{
					height3 = point3d.DistanceTo(origin) * 1.2;
				}
			}
			if (!flag)
			{
				Vector3d vector3d2 = channelGemData3.Center - channelGemData2.Center;
				vector3d2.Unitize();
				Vector3d vector3d3 = channelGemData3.Center - channelGemData4.Center;
				vector3d3.Unitize();
				if (num6 == j)
				{
					vector3d3 = vector3d2 * -1.0;
				}
				if (RhinoMath.ToDegrees(Math.Acos(vector3d2 * vector3d3)) < value44)
				{
					vector3d2 *= channelGemData3.GemXSize / 2.0;
					vector3d3 *= channelGemData3.GemXSize / 2.0;
					Vector3d vector3d4 = vector3d2 + vector3d3;
					Point3d testPoint = channelGemData3.Center + vector3d4;
					channelGemData3.GetCurve().ClosestPoint(testPoint, out var t);
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
					double num12 = (circleRight3.radius + circleRight2.radius + circleLeft3.radius + circleLeft2.radius) / 4.0 + value46;
					double[] array = new double[4] { circleRight3.radius, circleRight2.radius, circleLeft2.radius, circleLeft3.radius };
					if (value45 == 157)
					{
						double num13 = array[0];
						for (int k = 1; k < array.Length; k++)
						{
							if (num13 < array[k])
							{
								num13 = array[k];
							}
						}
						num12 = num13 + value46;
					}
					if (value45 == 158)
					{
						double num14 = array[0];
						for (int l = 1; l < array.Length; l++)
						{
							if (num14 > array[l])
							{
								num14 = array[l];
							}
						}
						num12 = num14 + value46;
					}
					double num15 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData3.Size) / 2.0) : channelGemData3.GemZMin);
					Point3d value55 = channelGemData3.GetCurve().PointAt(t);
					value55.Transform(Transform.Translation(channelGemData3.Normal * (num10 - num15)));
					vector3d4.Unitize();
					vector3d4 *= num12 - value20;
					value55 += vector3d4;
					On3dPoint val11 = CSConvert.Point3dCopyToOn3dPoint(value55);
					OnCircle val12 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val11, (IOn3dVector)(object)channelGemData3.Normal.Convert()), (IOn3dPoint)(object)val11, num12);
					OnXform val13 = new OnXform();
					val13.ChangeBasis((IOnPlane)(object)val12.plane, OnUtil.On_xy_plane);
					double num16 = val12.radius * 2.0;
					if (num16 > value2 && num16 < value3)
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
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, num16, height3, filletPer, taperPer, value, out var prong, out var prongBaseRail))
						{
							return UpdateError("Create prong cache failed");
						}
						prong.Transform((IOnXform)(object)val13);
						prongBaseRail.Transform((IOnXform)(object)val13);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val13.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val13.Convert(), applyTransform: false));
					}
				}
			}
			OnXform val14 = new OnXform();
			val14.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
			On3dVector val15 = new On3dVector(OnUtil.On_zaxis);
			val15.Transform((IOnXform)(object)val14);
			val15.Unitize();
			OnXform val16 = new OnXform();
			val16.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
			On3dVector val17 = new On3dVector(OnUtil.On_zaxis);
			val17.Transform((IOnXform)(object)val16);
			val17.Unitize();
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
				double num17 = (radius + radius2) / 2.0;
				circleRight = new OnCircle(circleRight.Center(), num17);
				circleLeft = new OnCircle(circleLeft.Center(), num17);
			}
			if (!flag)
			{
				double num18 = circleLeft.radius * 2.0;
				if (num18 > value2 && num18 < value3)
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
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num18, height3, filletPer2, taperPer2, value, out var prong2, out var prongBaseRail2))
					{
						return UpdateError("Create prong cache failed");
					}
					prong2.Transform((IOnXform)(object)val14);
					prongBaseRail2.Transform((IOnXform)(object)val14);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val14.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val14.Convert(), applyTransform: false));
				}
			}
			if (!flag)
			{
				double num19 = circleRight.radius * 2.0;
				if (num19 > value2 && num19 < value3)
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
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num19, height3, filletPer3, taperPer3, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					prong3.Transform((IOnXform)(object)val16);
					prongBaseRail3.Transform((IOnXform)(object)val16);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), val16.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail3).Convert(), val16.Convert(), applyTransform: false));
				}
			}
			On3dVector val18 = val15 + (IOn3dVector)(object)val17;
			val18.Unitize();
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
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value53), CSConvert.On3dVectorCopyToVector3d(value54), CSConvert.On3dVectorCopyToVector3d(val18));
				concurrentBag.Add(new CSOnGeometryData(geometry, transform));
			}
			if (val5 == OnUtil.On_UNSET_VECTOR)
			{
				val5 = val18;
			}
			val6 = val18;
		}
		if (!flag2)
		{
			for (int m = 0; m < cSDataArray.GetArrayCount(); m++)
			{
				int num20 = ((cSDataArray.GetArrayCount() > 1) ? (m + 1) : 0);
				int index = ((m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1));
				if (cSDataArray.GetArrayCount() > 1)
				{
					double num21;
					if (m != cSDataArray.GetArrayCount() - 1)
					{
						num21 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, m + 1).Center);
					}
					else
					{
						num21 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Center);
						num20 = 0;
					}
					double num22 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).GemXSize * 2.0;
					if (num21 <= num22)
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
						CSChannelGemData channelGemData7 = CSUtilGemRails.GetChannelGemData(cSDataArray, num20);
						Vector3d vector3d5 = Vector3d.Unset;
						if (channelGemData5 != null)
						{
							vector3d5 = channelGemData6.Center - channelGemData5.Center;
						}
						vector3d5.Unitize();
						Vector3d vector3d6 = Vector3d.Unset;
						if (channelGemData7 != null)
						{
							vector3d6 = channelGemData6.Center - channelGemData7.Center;
						}
						vector3d6.Unitize();
						if (num20 == m)
						{
							vector3d6 = vector3d5 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(vector3d5 * vector3d6)) < value44)
						{
							vector3d5 *= channelGemData6.GemXSize / 2.0;
							vector3d6 *= channelGemData6.GemXSize / 2.0;
							Vector3d vector3d7 = vector3d5 + vector3d6;
							Point3d testPoint2 = channelGemData6.Center + vector3d7;
							channelGemData6.GetCurve().ClosestPoint(testPoint2, out var t2);
							double num23 = cSDataArray.GetArrayCount();
							double vHeight3;
							double num24;
							double percent;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight3 = value4 * ((num23 / 2.0 - (double)m) / (num23 / 2.0)) + value5 * ((double)m / (num23 / 2.0));
								num24 = value21 * ((num23 / 2.0 - (double)m) / (num23 / 2.0)) + value22 * ((double)m / (num23 / 2.0));
								percent = value50 * ((num23 / 2.0 - (double)m) / (num23 / 2.0)) + value51 * ((double)m / (num23 / 2.0));
							}
							else
							{
								vHeight3 = value5 * (num23 - (double)m) / (double)m + value6 * (1.0 - (num23 - (double)m) / (double)m);
								num24 = value22 * (num23 - (double)m) / (double)m + value23 * (1.0 - (num23 - (double)m) / (double)m);
								percent = value51 * (num23 - (double)m) / (double)m + value52 * (1.0 - (num23 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData5, channelGemData6, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value19, value20, vHeight3, num24, value49, percent, out var circleLeft4, out var circleRight4, out var height6)))
							{
								break;
							}
							double num25 = (circleRight4.radius + circleLeft4.radius) / 2.0 + value46;
							double[] array2 = new double[2] { circleRight4.radius, circleLeft4.radius };
							if (value45 == 157)
							{
								double num26 = array2[0];
								for (int n = 1; n < array2.Length; n++)
								{
									if (num26 < array2[n])
									{
										num26 = array2[n];
									}
								}
								num25 = num26 + value46;
							}
							if (value45 == 158)
							{
								double num27 = array2[0];
								for (int num28 = 1; num28 < array2.Length; num28++)
								{
									if (num27 > array2[num28])
									{
										num27 = array2[num28];
									}
								}
								num25 = num27 + value46;
							}
							double num29 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData6.Size) / 2.0) : channelGemData6.GemZMin);
							Point3d value56 = channelGemData6.GetCurve().PointAt(t2);
							value56.Transform(Transform.Translation(channelGemData6.Normal * (num24 - num29)));
							vector3d7.Unitize();
							vector3d7 *= num25 - value20;
							value56 += vector3d7;
							On3dPoint val19 = CSConvert.Point3dCopyToOn3dPoint(value56);
							OnCircle val20 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val19, (IOn3dVector)(object)channelGemData6.Normal.Convert()), (IOn3dPoint)(object)val19, num25);
							OnXform val21 = new OnXform();
							val21.ChangeBasis((IOnPlane)(object)val20.plane, OnUtil.On_xy_plane);
							double num30 = val20.radius * 2.0;
							if (num30 > value2 && num30 < value3)
							{
								double taperPer4;
								double filletPer5;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									_ = (num23 / 2.0 - (double)m) / (num23 / 2.0);
									_ = (double)m / (num23 / 2.0);
									taperPer4 = value10 * ((num23 / 2.0 - (double)m) / (num23 / 2.0)) + value11 * ((double)m / (num23 / 2.0));
									filletPer5 = value7 * ((num23 / 2.0 - (double)m) / (num23 / 2.0)) + value8 * ((double)m / (num23 / 2.0));
								}
								else
								{
									_ = value42 * (num23 - (double)m) / (double)m;
									_ = (num23 - (double)m) / (double)m;
									taperPer4 = value11 * (num23 - (double)m) / (double)m + value12 * (1.0 - (num23 - (double)m) / (double)m);
									filletPer5 = value8 * (num23 - (double)m) / (double)m + value9 * (1.0 - (num23 - (double)m) / (double)m);
								}
								if (!_prongCache.Create((IOnNurbsCurve)(object)val, num30, height6, filletPer5, taperPer4, value, out var prong4, out var prongBaseRail4))
								{
									return UpdateError("Create prong cache failed");
								}
								prong4.Transform((IOnXform)(object)val21);
								prongBaseRail4.Transform((IOnXform)(object)val21);
								concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val21.Convert(), applyTransform: false));
								concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail4).Convert(), val21.Convert(), applyTransform: false));
							}
						}
					}
					if (!flag)
					{
						CSChannelGemData channelGemData8 = CSUtilGemRails.GetChannelGemData(cSDataArray, m);
						CSChannelGemData channelGemData9 = CSUtilGemRails.GetChannelGemData(cSDataArray, num20);
						CSChannelGemData channelGemData10 = CSUtilGemRails.GetChannelGemData(cSDataArray, (num20 + 1 != cSDataArray.GetArrayCount()) ? (num20 + 1) : 0);
						Vector3d vector3d8 = channelGemData9.Center - channelGemData8.Center;
						vector3d8.Unitize();
						Vector3d vector3d9 = channelGemData9.Center - channelGemData10.Center;
						vector3d9.Unitize();
						if (num20 == m)
						{
							vector3d9 = vector3d8 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(vector3d8 * vector3d9)) < value44)
						{
							vector3d8 *= channelGemData9.GemXSize / 2.0;
							vector3d9 *= channelGemData9.GemXSize / 2.0;
							Vector3d vector3d10 = vector3d8 + vector3d9;
							Point3d testPoint3 = channelGemData9.Center + vector3d10;
							channelGemData9.GetCurve().ClosestPoint(testPoint3, out var t3);
							double num31 = cSDataArray.GetArrayCount();
							double vHeight4;
							double num32;
							double percent2;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight4 = value4 * ((num31 / 2.0 - (double)m) / (num31 / 2.0)) + value5 * ((double)m / (num31 / 2.0));
								num32 = value21 * ((num31 / 2.0 - (double)m) / (num31 / 2.0)) + value22 * ((double)m / (num31 / 2.0));
								percent2 = value50 * ((num31 / 2.0 - (double)m) / (num31 / 2.0)) + value51 * ((double)m / (num31 / 2.0));
							}
							else
							{
								vHeight4 = value5 * (num31 - (double)m) / (double)m + value6 * (1.0 - (num31 - (double)m) / (double)m);
								num32 = value22 * (num31 - (double)m) / (double)m + value23 * (1.0 - (num31 - (double)m) / (double)m);
								percent2 = value51 * (num31 - (double)m) / (double)m + value52 * (1.0 - (num31 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData9, channelGemData10, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value19, value20, vHeight4, num32, value49, percent2, out var circleLeft5, out var circleRight5, out var height7)))
							{
								break;
							}
							double num33 = (circleRight5.radius + circleLeft5.radius) / 2.0 + value46;
							double[] array3 = new double[2] { circleRight5.radius, circleLeft5.radius };
							if (value45 == 157)
							{
								double num34 = array3[0];
								for (int num35 = 1; num35 < array3.Length; num35++)
								{
									if (num34 < array3[num35])
									{
										num34 = array3[num35];
									}
								}
								num33 = num34 + value46;
							}
							if (value45 == 158)
							{
								double num36 = array3[0];
								for (int num37 = 1; num37 < array3.Length; num37++)
								{
									if (num36 > array3[num37])
									{
										num36 = array3[num37];
									}
								}
								num33 = num36 + value46;
							}
							double num38 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData9.Size) / 2.0) : channelGemData9.GemZMin);
							Point3d value57 = channelGemData9.GetCurve().PointAt(t3);
							value57.Transform(Transform.Translation(channelGemData9.Normal * (num32 - num38)));
							vector3d10.Unitize();
							vector3d10 *= num33 - value20;
							value57 += vector3d10;
							On3dPoint val22 = CSConvert.Point3dCopyToOn3dPoint(value57);
							OnCircle val23 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val22, (IOn3dVector)(object)channelGemData9.Normal.Convert()), (IOn3dPoint)(object)val22, num33);
							OnXform val24 = new OnXform();
							val24.ChangeBasis((IOnPlane)(object)val23.plane, OnUtil.On_xy_plane);
							double num39 = val23.radius * 2.0;
							if (num39 > value2 && num39 < value3)
							{
								double taperPer5;
								double filletPer6;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									_ = (num31 / 2.0 - (double)m) / (num31 / 2.0);
									_ = (double)m / (num31 / 2.0);
									taperPer5 = value10 * ((num31 / 2.0 - (double)m) / (num31 / 2.0)) + value11 * ((double)m / (num31 / 2.0));
									filletPer6 = value7 * ((num31 / 2.0 - (double)m) / (num31 / 2.0)) + value8 * ((double)m / (num31 / 2.0));
								}
								else
								{
									_ = value42 * (num31 - (double)m) / (double)m;
									_ = (num31 - (double)m) / (double)m;
									taperPer5 = value11 * (num31 - (double)m) / (double)m + value12 * (1.0 - (num31 - (double)m) / (double)m);
									filletPer6 = value8 * (num31 - (double)m) / (double)m + value9 * (1.0 - (num31 - (double)m) / (double)m);
								}
								if (!_prongCache.Create((IOnNurbsCurve)(object)val, num39, height7, filletPer6, taperPer5, value, out var prong5, out var prongBaseRail5))
								{
									return UpdateError("Create prong cache failed");
								}
								prong5.Transform((IOnXform)(object)val24);
								prongBaseRail5.Transform((IOnXform)(object)val24);
								concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), val24.Convert(), applyTransform: false));
								concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail5).Convert(), val24.Convert(), applyTransform: false));
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
					CSChannelGemData cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, num20).Duplicate() as CSChannelGemData;
					if (cSDataArray.GetArrayCount() == 1)
					{
						cSChannelGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData.Normal, cSChannelGemData.Center));
					}
					double num41 = cSChannelGemData.GemXSize * value26 + value15 + value17;
					double height9 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + value4 - value21) : channelGemData.GemZSize);
					if (CSApplicationHelper.Application.IsCamMode)
					{
						CSChannelGemData gemPrev = cSChannelGemData;
						CSChannelGemData gemCurrent = CSUtilGemRails.GetChannelGemData(cSDataArray, (num20 + 1 != cSDataArray.GetArrayCount()) ? (num20 + 1) : 0).Duplicate() as CSChannelGemData;
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
							double num42 = (radius5 + radius6) / 2.0;
							circleRight7 = new OnCircle(circleRight7.Center(), num42);
							circleLeft7 = new OnCircle(circleLeft7.Center(), num42);
						}
						On3dVector vLeft = new On3dVector(circleLeft7.Center());
						On3dVector vRight = new On3dVector(circleRight7.Center());
						height9 = GetCamHeightEnd(gemPrev, gemCurrent, circleLeft7, circleRight7, vLeft, vRight);
					}
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num41, height9, value7, value10, value, out var prong7, out var prongBaseRail7))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
					double num43 = (cSChannelGemData.GemXSize * value26 + value15) / 2.0;
					double num44 = value21 - cSChannelGemData.GemZMin;
					On3dVector val31;
					if (val5 == OnUtil.On_UNSET_VECTOR)
					{
						val31 = new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert());
						val31.Unitize();
					}
					else
					{
						OnXform val32 = new OnXform();
						val32.Mirror((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
						val31 = new On3dVector((IOn3dVector)(object)val5);
						val31.Transform((IOnXform)(object)val32);
						val31.Unitize();
					}
					On3dVector val33 = new On3dVector((IOn3dVector)(object)cSChannelGemData.LeftVector.Convert());
					val33.Unitize();
					On3dVector val34 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val31, (IOn3dVector)(object)val33);
					val34.Unitize();
					OnPlane val35 = new OnPlane((IOn3dPoint)(object)(obj + (IOn3dVector)(object)(val34 * num43) + (IOn3dVector)(object)(val31 * num44)), (IOn3dVector)(object)val33, (IOn3dVector)(object)val34);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd4:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd7:
					{
						OnXform val51 = new OnXform();
						val51.ChangeBasis((IOnPlane)(object)val35, OnUtil.On_xy_plane);
						OnXform val52 = new OnXform();
						val52.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value27), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
						OnXform val53 = val52 * (IOnXform)(object)val51;
						prong7.Transform((IOnXform)(object)val53);
						prongBaseRail7.Transform((IOnXform)(object)val53);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong7).Convert(), val53.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail7).Convert(), val53.Convert(), applyTransform: false));
						break;
					}
					case ValueListItem.ProngPlacerStyleEnd5:
					case ValueListItem.ProngPlacerStyleEnd8:
					case ValueListItem.ProngPlacerStyleEnd9:
					{
						double num45 = value28 / 2.0;
						OnXform val36 = new OnXform();
						val36.ChangeBasis((IOnPlane)(object)val35, OnUtil.On_xy_plane);
						OnXform val37 = new OnXform();
						val37.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num45), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val38 = new OnXform();
						val38.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value27 + num45), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
						OnXform val39 = val38 * (IOnXform)(object)val36 * (IOnXform)(object)val37;
						if (!flag)
						{
							OnGeometry val40 = prong7.Duplicate();
							val40.Transform((IOnXform)(object)val39);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val40).Convert(), val39.Convert(), applyTransform: false));
							OnGeometry val41 = prongBaseRail7.Duplicate();
							val41.Transform((IOnXform)(object)val39);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val41).Convert(), val39.Convert(), applyTransform: false));
						}
						OnXform val42 = new OnXform();
						val42.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num45), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val43 = new OnXform();
						val43.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value27 - num45), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
						OnXform val44 = val43 * (IOnXform)(object)val36 * (IOnXform)(object)val42;
						if (!flag)
						{
							OnGeometry val45 = prong7.Duplicate();
							val45.Transform((IOnXform)(object)val44);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val45).Convert(), val44.Convert(), applyTransform: false));
							OnGeometry val46 = prongBaseRail7.Duplicate();
							val46.Transform((IOnXform)(object)val44);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val46).Convert(), val44.Convert(), applyTransform: false));
						}
						if (flag)
						{
							On3dPoint val47 = new On3dPoint(0.0, 0.0, 0.0);
							On3dPoint val48 = new On3dPoint(0.0, 0.0, 0.0);
							On3dVector val49 = new On3dVector(0.0, 0.0, 1.0);
							On3dVector val50 = new On3dVector(0.0, 0.0, 1.0);
							val47.Transform((IOnXform)(object)val39);
							val49.Transform((IOnXform)(object)val39);
							val48.Transform((IOnXform)(object)val44);
							val50.Transform((IOnXform)(object)val44);
							Brep geometry3 = MakeBar((val47 - (IOn3dPoint)(object)val48).Length(), num41 / 2.0, num41 / 2.0, height9, value10, value32, value35, value38, value41);
							On3dPoint value62 = new On3dPoint((IOn3dPoint)(object)((val47 + (IOn3dPoint)(object)val48) / 2.0));
							On3dVector value63 = val48 - (IOn3dPoint)(object)val47;
							On3dVector value64 = val49 + (IOn3dVector)(object)val50;
							Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value62), CSConvert.On3dVectorCopyToVector3d(value63), CSConvert.On3dVectorCopyToVector3d(value64));
							concurrentBag.Add(new CSOnGeometryData(geometry3, transform6));
						}
						break;
					}
					}
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd10:
				{
					CSChannelGemData channelGemData11 = CSUtilGemRails.GetChannelGemData(cSDataArray, num20);
					CSChannelGemData channelGemData12 = CSUtilGemRails.GetChannelGemData(cSDataArray, num20 + 1);
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
					On3dVector val25 = new On3dVector(circleLeft6.Center());
					On3dVector val26 = new On3dVector(circleRight6.Center());
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
						double num40 = (radius3 + radius4) / 2.0;
						circleRight6 = new OnCircle(circleRight6.Center(), num40);
						circleLeft6 = new OnCircle(circleLeft6.Center(), num40);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height8 = (value49 ? (num3 * value50) : num3);
					}
					OnXform val27 = new OnXform();
					val27.ChangeBasis((IOnPlane)(object)circleLeft6.plane, OnUtil.On_xy_plane);
					On3dVector val28 = new On3dVector(OnUtil.On_zaxis);
					val28.Transform((IOnXform)(object)val27);
					val28.Unitize();
					OnXform val29 = new OnXform();
					val29.ChangeBasis((IOnPlane)(object)circleRight6.plane, OnUtil.On_xy_plane);
					On3dVector val30 = new On3dVector(OnUtil.On_zaxis);
					val30.Transform((IOnXform)(object)val29);
					val30.Unitize();
					On3dVector value58 = val28 + (IOn3dVector)(object)val30;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height8 = GetCamHeightEnd(channelGemData11, channelGemData12, circleLeft6, circleRight6, val25, val26);
					}
					if (!flag)
					{
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft6.Diameter(), height8, value7, value10, value, out var prong6, out var prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value59 = val26 - (IOn3dVector)(object)val25;
						Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val25)), CSConvert.On3dVectorCopyToVector3d(value59), CSConvert.On3dVectorCopyToVector3d(value58));
						Vector3d normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), transform3 * transform2));
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight6.Diameter(), height8, value7, value10, value, out prong6, out prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						value59 = val25 - (IOn3dVector)(object)val26;
						transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val26)), CSConvert.On3dVectorCopyToVector3d(value59), CSConvert.On3dVectorCopyToVector3d(value58));
						normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), transform3 * transform2));
					}
					else
					{
						Brep geometry2 = MakeBar((val25 - (IOn3dVector)(object)val26).Length(), circleLeft6.radius, circleRight6.radius, height8, value10, value32, value35, value38, value41);
						On3dPoint value60 = new On3dPoint((IOn3dVector)(object)((val25 + (IOn3dVector)(object)val26) / 2.0));
						On3dVector value61 = val26 - (IOn3dVector)(object)val25;
						Transform transform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value60), CSConvert.On3dVectorCopyToVector3d(value61), CSConvert.On3dVectorCopyToVector3d(value58));
						Vector3d normalToMirrorPlane2 = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform transform5 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane2);
						concurrentBag.Add(new CSOnGeometryData(geometry2, transform5 * transform4));
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
					double num47 = cSChannelGemData2.GemXSize * value29 + value16 + value18;
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
							double num48 = (radius9 + radius10) / 2.0;
							circleRight9 = new OnCircle(circleRight9.Center(), num48);
							circleLeft9 = new OnCircle(circleLeft9.Center(), num48);
						}
						On3dVector vLeft2 = new On3dVector(circleLeft9.Center());
						On3dVector vRight2 = new On3dVector(circleRight9.Center());
						height12 = GetCamHeightEnd(gemPrev2, gemCurrent2, circleLeft9, circleRight9, vLeft2, vRight2);
					}
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num47, height12, value9, value12, value, out var prong9, out var prongBaseRail9))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj2 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					double num49 = (cSChannelGemData2.GemXSize * value29 + value16) / 2.0;
					double num50 = value23 - cSChannelGemData2.GemZMin;
					On3dVector val60;
					if (val6 == OnUtil.On_UNSET_VECTOR)
					{
						val60 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert());
						val60.Unitize();
					}
					else
					{
						OnXform val61 = new OnXform();
						val61.Mirror((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
						val60 = new On3dVector((IOn3dVector)(object)val6);
						val60.Transform((IOnXform)(object)val61);
						val60.Unitize();
					}
					On3dVector val62 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.RightVector.Convert());
					val62.Unitize();
					On3dVector val63 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val60, (IOn3dVector)(object)val62);
					val63.Unitize();
					OnPlane val64 = new OnPlane((IOn3dPoint)(object)(obj2 + (IOn3dVector)(object)(val63 * num49) + (IOn3dVector)(object)(val60 * num50)), (IOn3dVector)(object)val62, (IOn3dVector)(object)val63);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd2:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd8:
					{
						OnXform val80 = new OnXform();
						val80.ChangeBasis((IOnPlane)(object)val64, OnUtil.On_xy_plane);
						OnXform val81 = new OnXform();
						val81.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value30), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
						OnXform val82 = val81 * (IOnXform)(object)val80;
						prong9.Transform((IOnXform)(object)val82);
						prongBaseRail9.Transform((IOnXform)(object)val82);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong9).Convert(), val82.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail9).Convert(), val82.Convert(), applyTransform: false));
						break;
					}
					case ValueListItem.ProngPlacerStyleEnd3:
					case ValueListItem.ProngPlacerStyleEnd7:
					case ValueListItem.ProngPlacerStyleEnd9:
					{
						double num51 = value31 / 2.0;
						OnXform val65 = new OnXform();
						val65.ChangeBasis((IOnPlane)(object)val64, OnUtil.On_xy_plane);
						OnXform val66 = new OnXform();
						val66.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num51), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val67 = new OnXform();
						val67.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value30 + num51), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
						OnXform val68 = val67 * (IOnXform)(object)val65 * (IOnXform)(object)val66;
						if (!flag)
						{
							OnGeometry val69 = prong9.Duplicate();
							val69.Transform((IOnXform)(object)val68);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val69).Convert(), val68.Convert(), applyTransform: false));
							OnGeometry val70 = prongBaseRail9.Duplicate();
							val70.Transform((IOnXform)(object)val68);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val70).Convert(), val68.Convert(), applyTransform: false));
						}
						OnXform val71 = new OnXform();
						val71.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num51), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val72 = new OnXform();
						val72.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value30 - num51), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
						OnXform val73 = val72 * (IOnXform)(object)val65 * (IOnXform)(object)val71;
						if (!flag)
						{
							OnGeometry val74 = prong9.Duplicate();
							val74.Transform((IOnXform)(object)val73);
							concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val74).Convert(), val73.Convert(), applyTransform: false));
							OnGeometry val75 = prongBaseRail9.Duplicate();
							val75.Transform((IOnXform)(object)val73);
							concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val75).Convert(), val73.Convert(), applyTransform: false));
						}
						if (flag)
						{
							On3dPoint val76 = new On3dPoint(0.0, 0.0, 0.0);
							On3dPoint val77 = new On3dPoint(0.0, 0.0, 0.0);
							On3dVector val78 = new On3dVector(0.0, 0.0, 1.0);
							On3dVector val79 = new On3dVector(0.0, 0.0, 1.0);
							val76.Transform((IOnXform)(object)val68);
							val78.Transform((IOnXform)(object)val68);
							val77.Transform((IOnXform)(object)val73);
							val79.Transform((IOnXform)(object)val73);
							Brep geometry5 = MakeBar((val76 - (IOn3dPoint)(object)val77).Length(), num47 / 2.0, num47 / 2.0, height12, value12, value34, value37, value40, value43);
							On3dPoint value69 = new On3dPoint((IOn3dPoint)(object)((val76 + (IOn3dPoint)(object)val77) / 2.0));
							On3dVector value70 = val77 - (IOn3dPoint)(object)val76;
							On3dVector value71 = val78 + (IOn3dVector)(object)val79;
							Transform transform11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value69), CSConvert.On3dVectorCopyToVector3d(value70), CSConvert.On3dVectorCopyToVector3d(value71));
							concurrentBag.Add(new CSOnGeometryData(geometry5, transform11));
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
						double num46 = (radius7 + radius8) / 2.0;
						circleRight8 = new OnCircle(circleRight8.Center(), num46);
						circleLeft8 = new OnCircle(circleLeft8.Center(), num46);
					}
					On3dVector val54 = new On3dVector(circleLeft8.Center());
					On3dVector val55 = new On3dVector(circleRight8.Center());
					OnXform val56 = new OnXform();
					val56.ChangeBasis((IOnPlane)(object)circleLeft8.plane, OnUtil.On_xy_plane);
					On3dVector val57 = new On3dVector(OnUtil.On_zaxis);
					val57.Transform((IOnXform)(object)val56);
					val57.Unitize();
					OnXform val58 = new OnXform();
					val58.ChangeBasis((IOnPlane)(object)circleRight8.plane, OnUtil.On_xy_plane);
					On3dVector val59 = new On3dVector(OnUtil.On_zaxis);
					val59.Transform((IOnXform)(object)val58);
					val59.Unitize();
					On3dVector value65 = val57 + (IOn3dVector)(object)val59;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height11 = GetCamHeightEnd(channelGemData13, channelGemData14, circleLeft8, circleRight8, val54, val55);
					}
					if (!flag)
					{
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft8.Diameter(), height11, value9, value12, value, out var prong8, out var prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value66 = val55 - (IOn3dVector)(object)val54;
						Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val54)), CSConvert.On3dVectorCopyToVector3d(value66), CSConvert.On3dVectorCopyToVector3d(value65));
						Vector3d normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), transform8 * transform7));
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight8.Diameter(), height11, value9, value12, value, out prong8, out prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						value66 = val54 - (IOn3dVector)(object)val55;
						transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val55)), CSConvert.On3dVectorCopyToVector3d(value66), CSConvert.On3dVectorCopyToVector3d(value65));
						normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), transform8 * transform7));
					}
					else
					{
						Brep geometry4 = MakeBar((val54 - (IOn3dVector)(object)val55).Length(), circleLeft8.radius, circleRight8.radius, height11, value12, value34, value37, value40, value43);
						On3dPoint value67 = new On3dPoint((IOn3dVector)(object)((val54 + (IOn3dVector)(object)val55) / 2.0));
						On3dVector value68 = val55 - (IOn3dVector)(object)val54;
						Transform transform9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value67), CSConvert.On3dVectorCopyToVector3d(value68), CSConvert.On3dVectorCopyToVector3d(value65));
						Vector3d normalToMirrorPlane4 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform transform10 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane4);
						concurrentBag.Add(new CSOnGeometryData(geometry4, transform10 * transform9));
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
		double z2;
		if (z > height / 2.0)
		{
			z2 = z;
		}
		else
		{
			double num = Math.Pow(x / maxX, 2.0) * per;
			double num2 = height / 2.0;
			double num3 = (height - z) / height;
			z2 = z + num2 * num * num3;
		}
		return new ControlPoint(x, y, z2, w);
	}

	private static NurbsCurve BuildArcEnd(Point3d start, Point3d corner, Point3d mid, double endBlendPer)
	{
		if (endBlendPer < 0.01)
		{
			endBlendPer = 0.01;
		}
		if (endBlendPer > 0.99)
		{
			endBlendPer = 0.99;
		}
		Vector3d vector3d = corner - start;
		Vector3d vector3d2 = corner - mid;
		NurbsCurve nurbsCurve = new NurbsCurve(3, rational: true, 3, 7);
		double weight = 0.7071067811;
		nurbsCurve.Points[6] = new ControlPoint(start, 1.0);
		nurbsCurve.Points[5] = new ControlPoint(start + vector3d * (1.0 - endBlendPer) / 2.0, 1.0);
		nurbsCurve.Points[4] = new ControlPoint(start + vector3d * (1.0 - endBlendPer), 1.0);
		nurbsCurve.Points[3] = new ControlPoint(corner, weight);
		nurbsCurve.Points[2] = new ControlPoint(mid + vector3d2 * (1.0 - endBlendPer), 1.0);
		nurbsCurve.Points[1] = new ControlPoint(mid + vector3d2 * (1.0 - endBlendPer) / 2.0, 1.0);
		nurbsCurve.Points[0] = new ControlPoint(mid, 1.0);
		nurbsCurve.Knots[0] = 0.0;
		nurbsCurve.Knots[1] = 0.0;
		nurbsCurve.Knots[2] = 0.2;
		nurbsCurve.Knots[3] = 0.2;
		nurbsCurve.Knots[4] = 0.4;
		nurbsCurve.Knots[5] = 0.4;
		nurbsCurve.Knots[6] = 0.6;
		nurbsCurve.Knots[7] = 0.6;
		return nurbsCurve;
	}

	private NurbsCurve BuildProngProfile(double radius, double height, double radiusTopDome, double radiusBottomDome, double taperPer)
	{
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
		double x = num + num3 * radiusBottomDome;
		double x2 = radius - num3 * radiusTopDome;
		double weight = 0.7071067811;
		NurbsCurve nurbsCurve = new NurbsCurve(3, rational: true, 3, 11);
		nurbsCurve.Points[0] = new ControlPoint(0.0, 0.0, 0.0, 1.0);
		nurbsCurve.Points[1] = new ControlPoint((num - radiusBottomDome) / 2.0, 0.0, 0.0, 1.0);
		nurbsCurve.Points[2] = new ControlPoint(num - radiusBottomDome, 0.0, 0.0, 1.0);
		nurbsCurve.Points[3] = new ControlPoint(x, 0.0, 0.0, weight);
		nurbsCurve.Points[4] = new ControlPoint(num, 0.0, radiusBottomDome, 1.0);
		nurbsCurve.Points[5] = new ControlPoint((num + radius) / 2.0, 0.0, radiusBottomDome + num2 / 2.0, 1.0);
		nurbsCurve.Points[6] = new ControlPoint(radius, 0.0, radiusBottomDome + num2, 1.0);
		nurbsCurve.Points[7] = new ControlPoint(x2, 0.0, height, weight);
		nurbsCurve.Points[8] = new ControlPoint(radius - radiusTopDome, 0.0, height, 1.0);
		nurbsCurve.Points[9] = new ControlPoint((radius - radiusTopDome) / 2.0, 0.0, height, 1.0);
		nurbsCurve.Points[10] = new ControlPoint(0.0, 0.0, height, 1.0);
		nurbsCurve.Knots[0] = 0.0;
		nurbsCurve.Knots[1] = 0.0;
		nurbsCurve.Knots[2] = 0.2;
		nurbsCurve.Knots[3] = 0.2;
		nurbsCurve.Knots[4] = 0.4;
		nurbsCurve.Knots[5] = 0.4;
		nurbsCurve.Knots[6] = 0.6;
		nurbsCurve.Knots[7] = 0.6;
		nurbsCurve.Knots[8] = 0.8;
		nurbsCurve.Knots[9] = 0.8;
		nurbsCurve.Knots[10] = 1.0;
		nurbsCurve.Knots[11] = 1.0;
		return nurbsCurve;
	}

	private static double GetCamHeightEnd(CSGemData gemPrev, CSGemData gemCurrent, OnCircle circleLeft, OnCircle circleRight, On3dVector vLeft, On3dVector vRight)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		double num = 0.0;
		On3dPoint value = new On3dPoint((IOn3dVector)(object)((vLeft + (IOn3dVector)(object)vRight) / 2.0));
		On3dVector value2 = vRight - (IOn3dVector)(object)vLeft;
		OnXform val = new OnXform();
		val.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
		Vector3d zAxis = Vector3d.ZAxis;
		zAxis.Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val));
		zAxis.Unitize();
		OnXform val2 = new OnXform();
		val2.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
		Vector3d zAxis2 = Vector3d.ZAxis;
		zAxis2.Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val2));
		zAxis2.Unitize();
		Vector3d destination = zAxis + zAxis2;
		destination.Unitize();
		Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value), CSConvert.On3dVectorCopyToVector3d(value2), destination);
		Point3d origin = Point3d.Origin;
		origin.Transform(xform);
		Line line = new Line(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
		line.Transform(xform);
		Curve curve = Curve.CreateInterpolatedCurve(new List<Point3d> { gemPrev.ZMaxPoint, gemCurrent.ZMaxPoint }, 3);
		Curve curve2 = curve.DuplicateCurve();
		Curve curve3 = curve.DuplicateCurve();
		Vector3d vector3d = CSConvert.On3dVectorCopyToVector3d(circleLeft.plane.yaxis);
		curve2.Transform(Transform.Translation(4.0 * vector3d));
		curve3.Transform(Transform.Translation(-4.0 * vector3d));
		NurbsSurface nurbsSurface = NurbsSurface.CreateRuledSurface(curve2, curve3);
		nurbsSurface.ClosestPoint(line.PointAt(1.0), out var u, out var v);
		Point3d point3d = nurbsSurface.PointAt(u, v);
		if (point3d.DistanceTo(origin) > num * 0.8)
		{
			num = point3d.DistanceTo(origin) * 1.2;
		}
		return num;
	}

	private string GetCircles(CSChannelGemData gemPrev, CSChannelGemData gemCurrent, CSChannelGemData gemMax, IOnCurve railLeft, IOnCurve railRight, double vNudgeEdge, double vNudgeGem, double vHeight, double vVerticalLocation, bool usePercent, double percent, out OnCircle circleLeft, out OnCircle circleRight, out double height)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
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
		Point3d point3d = new Point3d((0.0 - centerLength) / 2.0, 0.0, 0.0);
		Point3d point3d2 = new Point3d(centerLength / 2.0, 0.0, 0.0);
		double num = Math.Asin((radius1 - radius2) / centerLength);
		Point3d point3d3 = new Point3d((0.0 - centerLength) / 2.0, radius1, 0.0);
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(num, -Vector3d.ZAxis, point3d);
		point3d3.Transform(xform);
		Point3d point3d4 = new Point3d(centerLength / 2.0, radius2, 0.0);
		Transform xform2 = CSBlockUtil.CSUtilMath.Rotation(num, -Vector3d.ZAxis, point3d2);
		point3d4.Transform(xform2);
		Point3d point3d5 = new Point3d(point3d3);
		point3d5.Y = 0.0 - point3d5.Y;
		Point3d point3d6 = new Point3d(point3d4);
		point3d6.Y = 0.0 - point3d6.Y;
		Point3d mid = new Point3d((0.0 - centerLength) / 2.0 - radius1, 0.0, 0.0);
		Point3d mid2 = new Point3d(centerLength / 2.0 + radius2, 0.0, 0.0);
		Vector3d vector3d = point3d3 - point3d4;
		vector3d.Unitize();
		double num2 = (point3d3.X - mid.X) / Math.Cos(num);
		Point3d corner = point3d3 + vector3d * num2;
		double num3 = (point3d4.X - mid2.X) / Math.Cos(num);
		Point3d corner2 = point3d4 + vector3d * num3;
		NurbsCurve rail = BuildArcEnd(point3d3, corner, mid, blendEdge);
		NurbsCurve rail2 = BuildArcEnd(point3d4, corner2, mid2, blendEdge);
		NurbsCurve profile = BuildProngProfile(radius1, height, domeRadius, baseDomeRadius, filletPer);
		NurbsCurve profile2 = BuildProngProfile(radius2, height, domeRadius, baseDomeRadius, filletPer);
		ProngRevolve(profile, rail, point3d, radius1, profile2, rail2, point3d2, radius2, Math.Max(centerLength / 2.0 + radius1, centerLength / 2.0 + radius2), height, bottomArcPercent, out var srf, out var srf2, out var srf3);
		Transform xform3 = Transform.Mirror(Point3d.Origin, Vector3d.YAxis);
		NurbsSurface nurbsSurface = new NurbsSurface(srf);
		NurbsSurface nurbsSurface2 = new NurbsSurface(srf2);
		NurbsSurface nurbsSurface3 = new NurbsSurface(srf3);
		nurbsSurface.Transform(xform3);
		nurbsSurface2.Transform(xform3);
		nurbsSurface3.Transform(xform3);
		Brep[] array = Brep.JoinBreps(new List<Brep>
		{
			srf.ToBrep(),
			srf2.ToBrep(),
			srf3.ToBrep(),
			nurbsSurface.ToBrep(),
			nurbsSurface2.ToBrep(),
			nurbsSurface3.ToBrep()
		}, 0.01);
		if (array.Length != 1)
		{
			return null;
		}
		return array[0];
	}

	private static void ProngRevolve(NurbsCurve profile0, NurbsCurve rail0, Point3d center0, double radius0, NurbsCurve profile1, NurbsCurve rail1, Point3d center1, double radius1, double maxX, double height, double per, out NurbsSurface srf0, out NurbsSurface srf1, out NurbsSurface srf2)
	{
		srf0 = NurbsSurface.Create(3, isRational: true, profile0.Order, rail0.Order, profile0.Points.Count, rail0.Points.Count);
		srf1 = NurbsSurface.Create(3, isRational: true, profile0.Order, rail0.Order, profile0.Points.Count, 5);
		srf2 = NurbsSurface.Create(3, isRational: true, profile0.Order, rail0.Order, profile0.Points.Count, rail1.Points.Count);
		int count = rail0.Points.Count;
		int count2 = profile0.Points.Count;
		int count3 = rail1.Points.Count;
		int count4 = profile1.Points.Count;
		if (count2 != count4)
		{
			return;
		}
		for (int i = 0; i < count2; i++)
		{
			Point3d location = profile0.Points[i].Location;
			Point3d point3d = new Point3d(location);
			point3d.Z = 0.0;
			Point3d point3d2 = point3d;
			double num = point3d2.DistanceTo(Point3d.Origin);
			Point3d location2 = profile1.Points[i].Location;
			point3d = new Point3d(location2);
			point3d.Z = 0.0;
			Point3d point3d3 = point3d;
			double num2 = point3d3.DistanceTo(Point3d.Origin);
			for (int j = 0; j < count; j++)
			{
				Vector3d vector3d = rail0.Points[j].Location - center0;
				Point3d point3d4 = center0 + vector3d * num / radius0;
				point3d4.Z = location.Z;
				double w = profile0.Points[i].Weight * rail0.Points[j].Weight;
				srf0.Points.SetControlPoint(i, j, BendZ(point3d4.X, point3d4.Y, point3d4.Z, w, maxX, height, per));
			}
			for (int k = 0; k < 5; k++)
			{
				Vector3d vector3d2 = rail0.Points[count - 1].Location - center0;
				Point3d point3d5 = center0 + vector3d2 * num / radius0;
				point3d5.Z = location.Z;
				Vector3d vector3d3 = rail1.Points[count3 - 1].Location - center1;
				Point3d point3d6 = center1 + vector3d3 * num2 / radius1;
				point3d6.Z = location2.Z;
				double weight = profile0.Points[i].Weight;
				Point3d point3d7 = point3d5 * (4 - k) / 4.0 + point3d6 * k / 4.0;
				srf1.Points.SetControlPoint(i, k, BendZ(point3d7.X, point3d7.Y, point3d7.Z, weight, maxX, height, per));
			}
			for (int l = 0; l < count3; l++)
			{
				Vector3d vector3d4 = rail1.Points[l].Location - center1;
				Point3d point3d8 = center1 + vector3d4 * num2 / radius1;
				point3d8.Z = location2.Z;
				double w2 = profile1.Points[i].Weight * rail1.Points[l].Weight;
				srf2.Points.SetControlPoint(i, l, BendZ(point3d8.X, point3d8.Y, point3d8.Z, w2, maxX, height, per));
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
