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
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Expected O, but got Unknown
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Expected O, but got Unknown
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Expected O, but got Unknown
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Expected O, but got Unknown
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a14: Expected O, but got Unknown
		//IL_1034: Unknown result type (might be due to invalid IL or missing references)
		//IL_103b: Expected O, but got Unknown
		//IL_1054: Unknown result type (might be due to invalid IL or missing references)
		//IL_105b: Expected O, but got Unknown
		//IL_106c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1073: Expected O, but got Unknown
		//IL_108c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1093: Expected O, but got Unknown
		//IL_10e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e9: Expected O, but got Unknown
		//IL_10cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d2: Expected O, but got Unknown
		//IL_2619: Unknown result type (might be due to invalid IL or missing references)
		//IL_262a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2634: Expected O, but got Unknown
		//IL_2634: Expected O, but got Unknown
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
		//IL_26b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b7: Expected O, but got Unknown
		//IL_2673: Unknown result type (might be due to invalid IL or missing references)
		//IL_267a: Expected O, but got Unknown
		//IL_26d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_26dd: Expected O, but got Unknown
		//IL_2718: Unknown result type (might be due to invalid IL or missing references)
		//IL_271f: Expected O, but got Unknown
		//IL_2746: Unknown result type (might be due to invalid IL or missing references)
		//IL_274f: Expected O, but got Unknown
		//IL_2764: Unknown result type (might be due to invalid IL or missing references)
		//IL_2769: Unknown result type (might be due to invalid IL or missing references)
		//IL_2799: Expected O, but got Unknown
		//IL_2810: Unknown result type (might be due to invalid IL or missing references)
		//IL_2819: Expected O, but got Unknown
		//IL_282e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2837: Expected O, but got Unknown
		//IL_2863: Unknown result type (might be due to invalid IL or missing references)
		//IL_2868: Unknown result type (might be due to invalid IL or missing references)
		//IL_289f: Expected O, but got Unknown
		//IL_1523: Unknown result type (might be due to invalid IL or missing references)
		//IL_152a: Expected O, but got Unknown
		//IL_1531: Unknown result type (might be due to invalid IL or missing references)
		//IL_1538: Expected O, but got Unknown
		//IL_293b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2944: Expected O, but got Unknown
		//IL_2970: Unknown result type (might be due to invalid IL or missing references)
		//IL_2975: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ac: Expected O, but got Unknown
		//IL_2521: Unknown result type (might be due to invalid IL or missing references)
		//IL_2528: Expected O, but got Unknown
		//IL_250a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2511: Expected O, but got Unknown
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
		//IL_2cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd2: Expected O, but got Unknown
		//IL_2cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce8: Expected O, but got Unknown
		//IL_1c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c62: Expected O, but got Unknown
		//IL_1c5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c64: Expected O, but got Unknown
		//IL_1c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6b: Expected O, but got Unknown
		//IL_3595: Unknown result type (might be due to invalid IL or missing references)
		//IL_35aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b4: Expected O, but got Unknown
		//IL_35b4: Expected O, but got Unknown
		//IL_21e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f5: Expected O, but got Unknown
		//IL_21f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f7: Expected O, but got Unknown
		//IL_21f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21fe: Expected O, but got Unknown
		//IL_3620: Unknown result type (might be due to invalid IL or missing references)
		//IL_3629: Expected O, but got Unknown
		//IL_3658: Unknown result type (might be due to invalid IL or missing references)
		//IL_3661: Expected O, but got Unknown
		//IL_3607: Unknown result type (might be due to invalid IL or missing references)
		//IL_3610: Expected O, but got Unknown
		//IL_2d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d4d: Expected O, but got Unknown
		//IL_2d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d2a: Expected O, but got Unknown
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
		//IL_379b: Expected O, but got Unknown
		//IL_3822: Unknown result type (might be due to invalid IL or missing references)
		//IL_382b: Expected O, but got Unknown
		//IL_3844: Unknown result type (might be due to invalid IL or missing references)
		//IL_384d: Expected O, but got Unknown
		//IL_3879: Unknown result type (might be due to invalid IL or missing references)
		//IL_387e: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_3961: Unknown result type (might be due to invalid IL or missing references)
		//IL_396a: Expected O, but got Unknown
		//IL_3996: Unknown result type (might be due to invalid IL or missing references)
		//IL_399b: Unknown result type (might be due to invalid IL or missing references)
		//IL_39da: Expected O, but got Unknown
		//IL_340d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3416: Expected O, but got Unknown
		//IL_33ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_33f3: Expected O, but got Unknown
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
			double num39 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, num37).Center);
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
				zAxis.Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val9));
				zAxis.Unitize();
				OnXform val10 = new OnXform();
				val10.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
				Vector3d zAxis2 = Vector3d.ZAxis;
				zAxis2.Transform(CSConvert.XformCopyToTransform((IOnXform)(object)val10));
				zAxis2.Unitize();
				Vector3d destination = zAxis + zAxis2;
				destination.Unitize();
				Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value5), CSConvert.On3dVectorCopyToVector3d(value6), destination);
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
				if (num38 == j)
				{
					vector3d3 = vector3d2 * -1.0;
				}
				if (RhinoMath.ToDegrees(Math.Acos(vector3d2 * vector3d3)) < num31)
				{
					vector3d2 *= channelGemData3.GemXSize / 2.0;
					vector3d3 *= channelGemData3.GemXSize / 2.0;
					Vector3d vector3d4 = vector3d2 + vector3d3;
					Point3d testPoint = channelGemData3.Center + vector3d4;
					channelGemData3.GetCurve().ClosestPoint(testPoint, out var t);
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
					double num44 = (circleRight3.radius + circleRight2.radius + circleLeft3.radius + circleLeft2.radius) / 4.0 + num32;
					double[] array = new double[4] { circleRight3.radius, circleRight2.radius, circleLeft2.radius, circleLeft3.radius };
					if (integer == 157)
					{
						double num45 = array[0];
						for (int k = 1; k < array.Length; k++)
						{
							if (num45 < array[k])
							{
								num45 = array[k];
							}
						}
						num44 = num45 + num32;
					}
					if (integer == 158)
					{
						double num46 = array[0];
						for (int l = 1; l < array.Length; l++)
						{
							if (num46 > array[l])
							{
								num46 = array[l];
							}
						}
						num44 = num46 + num32;
					}
					double num47 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData3.Size) / 2.0) : channelGemData3.GemZMin);
					Point3d value7 = channelGemData3.GetCurve().PointAt(t);
					value7.Transform(Transform.Translation(channelGemData3.Normal * (num42 - num47)));
					vector3d4.Unitize();
					vector3d4 *= num44 - num10;
					value7 += vector3d4;
					On3dPoint val11 = CSConvert.Point3dCopyToOn3dPoint(value7);
					OnCircle val12 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val11, (IOn3dVector)(object)channelGemData3.Normal.Convert()), (IOn3dPoint)(object)val11, num44);
					OnXform val13 = new OnXform();
					val13.ChangeBasis((IOnPlane)(object)val12.plane, OnUtil.On_xy_plane);
					double num48 = val12.radius * 2.0;
					if (num48 > num && num48 < num2)
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
						if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num48, height3, filletPer, taperPer, doMesh, out var prong, out var prongBaseRail, bottomPercentage))
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
				double num49 = (radius + radius2) / 2.0;
				circleRight = new OnCircle(circleRight.Center(), num49);
				circleLeft = new OnCircle(circleLeft.Center(), num49);
			}
			if (!flag)
			{
				double num50 = circleLeft.radius * 2.0;
				if (num50 > num && num50 < num2)
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
					if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num50, height3, filletPer2, taperPer2, doMesh, out var prong2, out var prongBaseRail2, bottomPercentage2))
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
				double num51 = circleRight.radius * 2.0;
				if (num51 > num && num51 < num2)
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
					if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num51, height3, filletPer3, taperPer3, doMesh, out var prong3, out var prongBaseRail3, bottomPercentage3))
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
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value5), CSConvert.On3dVectorCopyToVector3d(value6), CSConvert.On3dVectorCopyToVector3d(val18));
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
				int num52 = ((cSDataArray.GetArrayCount() > 1) ? (m + 1) : 0);
				int index = ((m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1));
				if (cSDataArray.GetArrayCount() > 1)
				{
					double num53;
					if (m != cSDataArray.GetArrayCount() - 1)
					{
						num53 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, m + 1).Center);
					}
					else
					{
						num53 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Center);
						num52 = 0;
					}
					double num54 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).GemXSize * 2.0;
					if (num53 <= num54)
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
						CSChannelGemData channelGemData7 = CSUtilGemRails.GetChannelGemData(cSDataArray, num52);
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
						if (num52 == m)
						{
							vector3d6 = vector3d5 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(vector3d5 * vector3d6)) < num31)
						{
							vector3d5 *= channelGemData6.GemXSize / 2.0;
							vector3d6 *= channelGemData6.GemXSize / 2.0;
							Vector3d vector3d7 = vector3d5 + vector3d6;
							Point3d testPoint2 = channelGemData6.Center + vector3d7;
							channelGemData6.GetCurve().ClosestPoint(testPoint2, out var t2);
							double num55 = cSDataArray.GetArrayCount();
							double vHeight3;
							double num56;
							double percent;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight3 = num3 * ((num55 / 2.0 - (double)m) / (num55 / 2.0)) + num4 * ((double)m / (num55 / 2.0));
								num56 = num11 * ((num55 / 2.0 - (double)m) / (num55 / 2.0)) + num12 * ((double)m / (num55 / 2.0));
								percent = value2 * ((num55 / 2.0 - (double)m) / (num55 / 2.0)) + value3 * ((double)m / (num55 / 2.0));
							}
							else
							{
								vHeight3 = num4 * (num55 - (double)m) / (double)m + num5 * (1.0 - (num55 - (double)m) / (double)m);
								num56 = num12 * (num55 - (double)m) / (double)m + num13 * (1.0 - (num55 - (double)m) / (double)m);
								percent = value3 * (num55 - (double)m) / (double)m + value4 * (1.0 - (num55 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData5, channelGemData6, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, vNudgeEdge2, num10, vHeight3, num56, value, percent, out var circleLeft4, out var circleRight4, out var height6)))
							{
								break;
							}
							double num57 = (circleRight4.radius + circleLeft4.radius) / 2.0 + num32;
							double[] array2 = new double[2] { circleRight4.radius, circleLeft4.radius };
							if (integer == 157)
							{
								double num58 = array2[0];
								for (int n = 1; n < array2.Length; n++)
								{
									if (num58 < array2[n])
									{
										num58 = array2[n];
									}
								}
								num57 = num58 + num32;
							}
							if (integer == 158)
							{
								double num59 = array2[0];
								for (int num60 = 1; num60 < array2.Length; num60++)
								{
									if (num59 > array2[num60])
									{
										num59 = array2[num60];
									}
								}
								num57 = num59 + num32;
							}
							double num61 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData6.Size) / 2.0) : channelGemData6.GemZMin);
							Point3d value8 = channelGemData6.GetCurve().PointAt(t2);
							value8.Transform(Transform.Translation(channelGemData6.Normal * (num56 - num61)));
							vector3d7.Unitize();
							vector3d7 *= num57 - num10;
							value8 += vector3d7;
							On3dPoint val19 = CSConvert.Point3dCopyToOn3dPoint(value8);
							OnCircle val20 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val19, (IOn3dVector)(object)channelGemData6.Normal.Convert()), (IOn3dPoint)(object)val19, num57);
							OnXform val21 = new OnXform();
							val21.ChangeBasis((IOnPlane)(object)val20.plane, OnUtil.On_xy_plane);
							double num62 = val20.radius * 2.0;
							if (num62 > num && num62 < num2)
							{
								double bottomPercentage4;
								double taperPer4;
								double filletPer5;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									bottomPercentage4 = num28 * ((num55 / 2.0 - (double)m) / (num55 / 2.0)) + num29 * ((double)m / (num55 / 2.0));
									taperPer4 = percentage4 * ((num55 / 2.0 - (double)m) / (num55 / 2.0)) + percentage5 * ((double)m / (num55 / 2.0));
									filletPer5 = percentage * ((num55 / 2.0 - (double)m) / (num55 / 2.0)) + percentage2 * ((double)m / (num55 / 2.0));
								}
								else
								{
									bottomPercentage4 = num29 * (num55 - (double)m) / (double)m + num30 * (1.0 - (num55 - (double)m) / (double)m);
									taperPer4 = percentage5 * (num55 - (double)m) / (double)m + percentage6 * (1.0 - (num55 - (double)m) / (double)m);
									filletPer5 = percentage2 * (num55 - (double)m) / (double)m + percentage3 * (1.0 - (num55 - (double)m) / (double)m);
								}
								if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num62, height6, filletPer5, taperPer4, doMesh, out var prong4, out var prongBaseRail4, bottomPercentage4))
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
						CSChannelGemData channelGemData9 = CSUtilGemRails.GetChannelGemData(cSDataArray, num52);
						CSChannelGemData channelGemData10 = CSUtilGemRails.GetChannelGemData(cSDataArray, (num52 + 1 != cSDataArray.GetArrayCount()) ? (num52 + 1) : 0);
						Vector3d vector3d8 = channelGemData9.Center - channelGemData8.Center;
						vector3d8.Unitize();
						Vector3d vector3d9 = channelGemData9.Center - channelGemData10.Center;
						vector3d9.Unitize();
						if (num52 == m)
						{
							vector3d9 = vector3d8 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(vector3d8 * vector3d9)) < num31)
						{
							vector3d8 *= channelGemData9.GemXSize / 2.0;
							vector3d9 *= channelGemData9.GemXSize / 2.0;
							Vector3d vector3d10 = vector3d8 + vector3d9;
							Point3d testPoint3 = channelGemData9.Center + vector3d10;
							channelGemData9.GetCurve().ClosestPoint(testPoint3, out var t3);
							double num63 = cSDataArray.GetArrayCount();
							double vHeight4;
							double num64;
							double percent2;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight4 = num3 * ((num63 / 2.0 - (double)m) / (num63 / 2.0)) + num4 * ((double)m / (num63 / 2.0));
								num64 = num11 * ((num63 / 2.0 - (double)m) / (num63 / 2.0)) + num12 * ((double)m / (num63 / 2.0));
								percent2 = value2 * ((num63 / 2.0 - (double)m) / (num63 / 2.0)) + value3 * ((double)m / (num63 / 2.0));
							}
							else
							{
								vHeight4 = num4 * (num63 - (double)m) / (double)m + num5 * (1.0 - (num63 - (double)m) / (double)m);
								num64 = num12 * (num63 - (double)m) / (double)m + num13 * (1.0 - (num63 - (double)m) / (double)m);
								percent2 = value3 * (num63 - (double)m) / (double)m + value4 * (1.0 - (num63 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData9, channelGemData10, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, vNudgeEdge2, num10, vHeight4, num64, value, percent2, out var circleLeft5, out var circleRight5, out var height7)))
							{
								break;
							}
							double num65 = (circleRight5.radius + circleLeft5.radius) / 2.0 + num32;
							double[] array3 = new double[2] { circleRight5.radius, circleLeft5.radius };
							if (integer == 157)
							{
								double num66 = array3[0];
								for (int num67 = 1; num67 < array3.Length; num67++)
								{
									if (num66 < array3[num67])
									{
										num66 = array3[num67];
									}
								}
								num65 = num66 + num32;
							}
							if (integer == 158)
							{
								double num68 = array3[0];
								for (int num69 = 1; num69 < array3.Length; num69++)
								{
									if (num68 > array3[num69])
									{
										num68 = array3[num69];
									}
								}
								num65 = num68 + num32;
							}
							double num70 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData9.Size) / 2.0) : channelGemData9.GemZMin);
							Point3d value9 = channelGemData9.GetCurve().PointAt(t3);
							value9.Transform(Transform.Translation(channelGemData9.Normal * (num64 - num70)));
							vector3d10.Unitize();
							vector3d10 *= num65 - num10;
							value9 += vector3d10;
							On3dPoint val22 = CSConvert.Point3dCopyToOn3dPoint(value9);
							OnCircle val23 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val22, (IOn3dVector)(object)channelGemData9.Normal.Convert()), (IOn3dPoint)(object)val22, num65);
							OnXform val24 = new OnXform();
							val24.ChangeBasis((IOnPlane)(object)val23.plane, OnUtil.On_xy_plane);
							double num71 = val23.radius * 2.0;
							if (num71 > num && num71 < num2)
							{
								double bottomPercentage5;
								double taperPer5;
								double filletPer6;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									bottomPercentage5 = num28 * ((num63 / 2.0 - (double)m) / (num63 / 2.0)) + num29 * ((double)m / (num63 / 2.0));
									taperPer5 = percentage4 * ((num63 / 2.0 - (double)m) / (num63 / 2.0)) + percentage5 * ((double)m / (num63 / 2.0));
									filletPer6 = percentage * ((num63 / 2.0 - (double)m) / (num63 / 2.0)) + percentage2 * ((double)m / (num63 / 2.0));
								}
								else
								{
									bottomPercentage5 = num29 * (num63 - (double)m) / (double)m + num30 * (1.0 - (num63 - (double)m) / (double)m);
									taperPer5 = percentage5 * (num63 - (double)m) / (double)m + percentage6 * (1.0 - (num63 - (double)m) / (double)m);
									filletPer6 = percentage2 * (num63 - (double)m) / (double)m + percentage3 * (1.0 - (num63 - (double)m) / (double)m);
								}
								if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num71, height7, filletPer6, taperPer5, doMesh, out var prong5, out var prongBaseRail5, bottomPercentage5))
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
				switch (valueListItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					CSChannelGemData cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, num52).Duplicate() as CSChannelGemData;
					if (cSDataArray.GetArrayCount() == 1)
					{
						cSChannelGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData.Normal, cSChannelGemData.Center));
					}
					double num73 = cSChannelGemData.GemXSize * percentage7 + num6 + num8;
					double height9 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + num3 - num11) : channelGemData.GemZSize);
					if (CSApplicationHelper.Application.IsCamMode)
					{
						CSChannelGemData gemPrev = cSChannelGemData;
						CSChannelGemData gemCurrent = CSUtilGemRails.GetChannelGemData(cSDataArray, (num52 + 1 != cSDataArray.GetArrayCount()) ? (num52 + 1) : 0).Duplicate() as CSChannelGemData;
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
							double num74 = (radius5 + radius6) / 2.0;
							circleRight7 = new OnCircle(circleRight7.Center(), num74);
							circleLeft7 = new OnCircle(circleLeft7.Center(), num74);
						}
						On3dVector vLeft = new On3dVector(circleLeft7.Center());
						On3dVector vRight = new On3dVector(circleRight7.Center());
						height9 = GetCamHeightEnd(gemPrev, gemCurrent, circleLeft7, circleRight7, vLeft, vRight);
					}
					if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num73, height9, percentage, percentage4, doMesh, out var prong7, out var prongBaseRail7, num28))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
					double num75 = (cSChannelGemData.GemXSize * percentage7 + num6) / 2.0;
					double num76 = num11 - cSChannelGemData.GemZMin;
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
					OnPlane val35 = new OnPlane((IOn3dPoint)(object)(obj + (IOn3dVector)(object)(val34 * num75) + (IOn3dVector)(object)(val31 * num76)), (IOn3dVector)(object)val33, (IOn3dVector)(object)val34);
					switch (valueListItem)
					{
					case ValueListItem.ProngPlacerStyleEnd4:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd7:
					{
						OnXform val51 = new OnXform();
						val51.ChangeBasis((IOnPlane)(object)val35, OnUtil.On_xy_plane);
						OnXform val52 = new OnXform();
						val52.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num14), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
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
						double num77 = num15 / 2.0;
						OnXform val36 = new OnXform();
						val36.ChangeBasis((IOnPlane)(object)val35, OnUtil.On_xy_plane);
						OnXform val37 = new OnXform();
						val37.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num77), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val38 = new OnXform();
						val38.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num14 + num77), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
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
						val42.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num77), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val43 = new OnXform();
						val43.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num14 - num77), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
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
							Brep geometry3 = MakeBar((val47 - (IOn3dPoint)(object)val48).Length(), num73 / 2.0, num73 / 2.0, height9, percentage4, num19, num22, num25, num28);
							On3dPoint value14 = new On3dPoint((IOn3dPoint)(object)((val47 + (IOn3dPoint)(object)val48) / 2.0));
							On3dVector value15 = val48 - (IOn3dPoint)(object)val47;
							On3dVector value16 = val49 + (IOn3dVector)(object)val50;
							Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value14), CSConvert.On3dVectorCopyToVector3d(value15), CSConvert.On3dVectorCopyToVector3d(value16));
							concurrentBag.Add(new CSOnGeometryData(geometry3, transform6));
						}
						break;
					}
					}
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd10:
				{
					CSChannelGemData channelGemData11 = CSUtilGemRails.GetChannelGemData(cSDataArray, num52);
					CSChannelGemData channelGemData12 = CSUtilGemRails.GetChannelGemData(cSDataArray, num52 + 1);
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
					On3dVector val25 = new On3dVector(circleLeft6.Center());
					On3dVector val26 = new On3dVector(circleRight6.Center());
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
						double num72 = (radius3 + radius4) / 2.0;
						circleRight6 = new OnCircle(circleRight6.Center(), num72);
						circleLeft6 = new OnCircle(circleLeft6.Center(), num72);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height8 = (value ? (num35 * value2) : num35);
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
					On3dVector value10 = val28 + (IOn3dVector)(object)val30;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height8 = GetCamHeightEnd(channelGemData11, channelGemData12, circleLeft6, circleRight6, val25, val26);
					}
					if (!flag)
					{
						if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, circleLeft6.Diameter(), height8, percentage, percentage4, doMesh, out var prong6, out var prongBaseRail6, num28))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value11 = val26 - (IOn3dVector)(object)val25;
						Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val25)), CSConvert.On3dVectorCopyToVector3d(value11), CSConvert.On3dVectorCopyToVector3d(value10));
						Vector3d normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), transform3 * transform2));
						if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, circleRight6.Diameter(), height8, percentage, percentage4, doMesh, out prong6, out prongBaseRail6, num28))
						{
							return UpdateError("Create prong cache failed");
						}
						value11 = val25 - (IOn3dVector)(object)val26;
						transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val26)), CSConvert.On3dVectorCopyToVector3d(value11), CSConvert.On3dVectorCopyToVector3d(value10));
						normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), transform3 * transform2));
					}
					else
					{
						Brep geometry2 = MakeBar((val25 - (IOn3dVector)(object)val26).Length(), circleLeft6.radius, circleRight6.radius, height8, percentage4, num19, num22, num25, num28);
						On3dPoint value12 = new On3dPoint((IOn3dVector)(object)((val25 + (IOn3dVector)(object)val26) / 2.0));
						On3dVector value13 = val26 - (IOn3dVector)(object)val25;
						Transform transform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value12), CSConvert.On3dVectorCopyToVector3d(value13), CSConvert.On3dVectorCopyToVector3d(value10));
						Vector3d normalToMirrorPlane2 = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform transform5 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane2);
						concurrentBag.Add(new CSOnGeometryData(geometry2, transform5 * transform4));
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
					double num79 = cSChannelGemData2.GemXSize * num16 + num7 + num9;
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
							double num80 = (radius9 + radius10) / 2.0;
							circleRight9 = new OnCircle(circleRight9.Center(), num80);
							circleLeft9 = new OnCircle(circleLeft9.Center(), num80);
						}
						On3dVector vLeft2 = new On3dVector(circleLeft9.Center());
						On3dVector vRight2 = new On3dVector(circleRight9.Center());
						height12 = GetCamHeightEnd(gemPrev2, gemCurrent2, circleLeft9, circleRight9, vLeft2, vRight2);
					}
					if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, num79, height12, percentage3, percentage6, doMesh, out var prong9, out var prongBaseRail9, num30))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj2 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					double num81 = (cSChannelGemData2.GemXSize * num16 + num7) / 2.0;
					double num82 = num13 - cSChannelGemData2.GemZMin;
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
					OnPlane val64 = new OnPlane((IOn3dPoint)(object)(obj2 + (IOn3dVector)(object)(val63 * num81) + (IOn3dVector)(object)(val60 * num82)), (IOn3dVector)(object)val62, (IOn3dVector)(object)val63);
					switch (valueListItem)
					{
					case ValueListItem.ProngPlacerStyleEnd2:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd8:
					{
						OnXform val80 = new OnXform();
						val80.ChangeBasis((IOnPlane)(object)val64, OnUtil.On_xy_plane);
						OnXform val81 = new OnXform();
						val81.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num17), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
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
						double num83 = num18 / 2.0;
						OnXform val65 = new OnXform();
						val65.ChangeBasis((IOnPlane)(object)val64, OnUtil.On_xy_plane);
						OnXform val66 = new OnXform();
						val66.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num83), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val67 = new OnXform();
						val67.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num17 + num83), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
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
						val71.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num83), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val72 = new OnXform();
						val72.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num17 - num83), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
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
							Brep geometry5 = MakeBar((val76 - (IOn3dPoint)(object)val77).Length(), num79 / 2.0, num79 / 2.0, height12, percentage6, num21, num24, num27, num30);
							On3dPoint value21 = new On3dPoint((IOn3dPoint)(object)((val76 + (IOn3dPoint)(object)val77) / 2.0));
							On3dVector value22 = val77 - (IOn3dPoint)(object)val76;
							On3dVector value23 = val78 + (IOn3dVector)(object)val79;
							Transform transform11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value21), CSConvert.On3dVectorCopyToVector3d(value22), CSConvert.On3dVectorCopyToVector3d(value23));
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
						double num78 = (radius7 + radius8) / 2.0;
						circleRight8 = new OnCircle(circleRight8.Center(), num78);
						circleLeft8 = new OnCircle(circleLeft8.Center(), num78);
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
					On3dVector value17 = val57 + (IOn3dVector)(object)val59;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height11 = GetCamHeightEnd(channelGemData13, channelGemData14, circleLeft8, circleRight8, val54, val55);
					}
					if (!flag)
					{
						if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, circleLeft8.Diameter(), height11, percentage3, percentage6, doMesh, out var prong8, out var prongBaseRail8, num30))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value18 = val55 - (IOn3dVector)(object)val54;
						Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val54)), CSConvert.On3dVectorCopyToVector3d(value18), CSConvert.On3dVectorCopyToVector3d(value17));
						Vector3d normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), transform8 * transform7));
						if (!_prongCache.CreateNew((IOnNurbsCurve)(object)val, circleRight8.Diameter(), height11, percentage3, percentage6, doMesh, out prong8, out prongBaseRail8, num30))
						{
							return UpdateError("Create prong cache failed");
						}
						value18 = val54 - (IOn3dVector)(object)val55;
						transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val55)), CSConvert.On3dVectorCopyToVector3d(value18), CSConvert.On3dVectorCopyToVector3d(value17));
						normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), transform8 * transform7));
					}
					else
					{
						Brep geometry4 = MakeBar((val54 - (IOn3dVector)(object)val55).Length(), circleLeft8.radius, circleRight8.radius, height11, percentage6, num21, num24, num27, num30);
						On3dPoint value19 = new On3dPoint((IOn3dVector)(object)((val54 + (IOn3dVector)(object)val55) / 2.0));
						On3dVector value20 = val55 - (IOn3dVector)(object)val54;
						Transform transform9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value19), CSConvert.On3dVectorCopyToVector3d(value20), CSConvert.On3dVectorCopyToVector3d(value17));
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

	private static NurbsCurve BuildProngProfile(double radius, double height, double radiusTopDome, double radiusBottomDome, double taperPer)
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

	private static string GetHeight(CSChannelGemData gemPrev, CSChannelGemData gemCurrent, CSChannelGemData gemMax, IOnCurve railLeft, IOnCurve railRight, double vNudgeEdge, double vNudgeGem, double vHeight, double vVerticalLocation, out double height)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
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
