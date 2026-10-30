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
[CSBlock("ProngPlacerGC_V9", "{D17D9DFD-7444-4422-BAD2-1799C7FB0024}")]
public sealed class CSProngPlacerGC9 : CSBlock
{
	public readonly CSInputChannel InBlendBot;

	public readonly CSInputChannel InBlendEdge;

	public readonly CSInputChannel InBlendTop;

	public readonly CSInputChannel InBottomArcPercentage;

	public readonly CSInputChannel InCornerNudgeEdge;

	public readonly CSInputChannel InCornerNudgeGem;

	public readonly CSInputChannel InCornerProngAngle;

	public readonly CSInputChannel InCornerProngSize;

	public readonly CSInputChannel InCornerProngSizeOffset;

	public readonly CSInputChannel InDoMeshedProngs;

	public readonly CSInputChannel InEnableEndProngEndControls;

	public readonly CSInputChannel InEnableEndProngRotationMirroring;

	public readonly CSInputChannel InEndNudgeEdge;

	public readonly CSInputChannel InEndNudgeGem;

	public readonly CSInputChannel InEndProngStyle;

	public readonly CSInputChannel InFillet;

	public readonly CSInputChannel InHeight;

	public readonly CSInputChannel InMaxGem;

	public readonly CSInputChannel InNudgeEdge;

	public readonly CSInputChannel InNudgeGem;

	public readonly CSInputChannel InPercentToggle;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InProngPercentage;

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

	public readonly CSInputChannel InTaper;

	public readonly CSInputChannel InVerticalLocation;

	public readonly CSOutputChannel OutProfiles;

	public readonly CSOutputChannel OutProngs;

	private readonly CSProngCache _prongCache = new CSProngCache();

	private ValueListItem _prongBase = ValueListItem.FollowCulet;

	public CSProngPlacerGC9(string identifier)
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
		InEndNudgeEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndNudgeGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerNudgeEdge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerNudgeGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProfiles = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Expected O, but got Unknown
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Expected O, but got Unknown
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Expected O, but got Unknown
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Expected O, but got Unknown
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Expected O, but got Unknown
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Expected O, but got Unknown
		//IL_0c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8c: Expected O, but got Unknown
		//IL_0ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cac: Expected O, but got Unknown
		//IL_0cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc4: Expected O, but got Unknown
		//IL_0cdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce4: Expected O, but got Unknown
		//IL_0d33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Expected O, but got Unknown
		//IL_0d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d23: Expected O, but got Unknown
		//IL_0db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db7: Expected O, but got Unknown
		//IL_0dc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc7: Expected O, but got Unknown
		//IL_0d78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7f: Expected O, but got Unknown
		//IL_0d61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d68: Expected O, but got Unknown
		//IL_17f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1803: Unknown result type (might be due to invalid IL or missing references)
		//IL_180d: Expected O, but got Unknown
		//IL_180d: Expected O, but got Unknown
		//IL_185d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1864: Expected O, but got Unknown
		//IL_1885: Unknown result type (might be due to invalid IL or missing references)
		//IL_188c: Expected O, but got Unknown
		//IL_184c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1853: Expected O, but got Unknown
		//IL_18a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b0: Expected O, but got Unknown
		//IL_18eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f2: Expected O, but got Unknown
		//IL_0f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1b: Expected O, but got Unknown
		//IL_0f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Expected O, but got Unknown
		//IL_0f70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f77: Expected O, but got Unknown
		//IL_1919: Unknown result type (might be due to invalid IL or missing references)
		//IL_1920: Expected O, but got Unknown
		//IL_192f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1934: Unknown result type (might be due to invalid IL or missing references)
		//IL_1960: Expected O, but got Unknown
		//IL_19bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c6: Expected O, but got Unknown
		//IL_19d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19dc: Expected O, but got Unknown
		//IL_19fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a32: Expected O, but got Unknown
		//IL_1a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa2: Expected O, but got Unknown
		//IL_1ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af8: Expected O, but got Unknown
		//IL_0bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Expected O, but got Unknown
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdb: Expected O, but got Unknown
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be2: Expected O, but got Unknown
		//IL_132f: Unknown result type (might be due to invalid IL or missing references)
		//IL_133d: Expected O, but got Unknown
		//IL_1338: Unknown result type (might be due to invalid IL or missing references)
		//IL_133f: Expected O, but got Unknown
		//IL_133f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1346: Expected O, but got Unknown
		//IL_1b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8a: Expected O, but got Unknown
		//IL_1ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bac: Expected O, but got Unknown
		//IL_1bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bee: Expected O, but got Unknown
		//IL_1bf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c69: Expected O, but got Unknown
		//IL_1c7b: Expected O, but got Unknown
		//IL_1661: Unknown result type (might be due to invalid IL or missing references)
		//IL_166f: Expected O, but got Unknown
		//IL_166a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1671: Expected O, but got Unknown
		//IL_1671: Unknown result type (might be due to invalid IL or missing references)
		//IL_1678: Expected O, but got Unknown
		//IL_22c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e3: Expected O, but got Unknown
		//IL_22e3: Expected O, but got Unknown
		//IL_1d3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d45: Expected O, but got Unknown
		//IL_1d4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d53: Expected O, but got Unknown
		//IL_2333: Unknown result type (might be due to invalid IL or missing references)
		//IL_233a: Expected O, but got Unknown
		//IL_235b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2362: Expected O, but got Unknown
		//IL_2322: Unknown result type (might be due to invalid IL or missing references)
		//IL_2329: Expected O, but got Unknown
		//IL_237f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2386: Expected O, but got Unknown
		//IL_23c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_23c8: Expected O, but got Unknown
		//IL_1d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d98: Expected O, but got Unknown
		//IL_1d7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d81: Expected O, but got Unknown
		//IL_23f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_23fe: Expected O, but got Unknown
		//IL_2411: Unknown result type (might be due to invalid IL or missing references)
		//IL_2416: Unknown result type (might be due to invalid IL or missing references)
		//IL_2444: Expected O, but got Unknown
		//IL_24bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c4: Expected O, but got Unknown
		//IL_24d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e2: Expected O, but got Unknown
		//IL_250e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2513: Unknown result type (might be due to invalid IL or missing references)
		//IL_254a: Expected O, but got Unknown
		//IL_1e0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e15: Expected O, but got Unknown
		//IL_1e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e25: Expected O, but got Unknown
		//IL_1dd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ddd: Expected O, but got Unknown
		//IL_1dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc6: Expected O, but got Unknown
		//IL_25e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ef: Expected O, but got Unknown
		//IL_261b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2620: Unknown result type (might be due to invalid IL or missing references)
		//IL_2657: Expected O, but got Unknown
		//IL_1e41: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e48: Expected O, but got Unknown
		//IL_1e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7c: Expected O, but got Unknown
		//IL_1e95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9c: Expected O, but got Unknown
		//IL_1eb4: Expected O, but got Unknown
		//IL_1f02: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f09: Expected O, but got Unknown
		//IL_2715: Unknown result type (might be due to invalid IL or missing references)
		//IL_271e: Expected O, but got Unknown
		//IL_273b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2744: Expected O, but got Unknown
		//IL_2761: Unknown result type (might be due to invalid IL or missing references)
		//IL_2781: Unknown result type (might be due to invalid IL or missing references)
		//IL_278a: Expected O, but got Unknown
		//IL_279d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2830: Unknown result type (might be due to invalid IL or missing references)
		//IL_2839: Expected O, but got Unknown
		//IL_285d: Expected O, but got Unknown
		//IL_2177: Unknown result type (might be due to invalid IL or missing references)
		//IL_217e: Expected O, but got Unknown
		//IL_29f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f9: Expected O, but got Unknown
		//IL_29cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d6: Expected O, but got Unknown
		//IL_2adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae8: Expected O, but got Unknown
		//IL_2af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2afe: Expected O, but got Unknown
		//IL_2b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b09: Expected O, but got Unknown
		//IL_2b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b31: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b4d: Expected O, but got Unknown
		//IL_2b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b79: Expected O, but got Unknown
		//IL_2ba3: Expected O, but got Unknown
		//IL_2aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ab6: Expected O, but got Unknown
		//IL_2ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad2: Expected O, but got Unknown
		//IL_2a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a5e: Expected O, but got Unknown
		//IL_2a32: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a3b: Expected O, but got Unknown
		//IL_2c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c0e: Expected O, but got Unknown
		//IL_2f9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa3: Expected O, but got Unknown
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
		InEndNudgeEdge.GetValue(out double value9);
		InEndNudgeGem.GetValue(out double value10);
		InCornerNudgeEdge.GetValue(out double value11);
		InCornerNudgeGem.GetValue(out double value12);
		double value13 = 0.0;
		InVerticalLocation.GetDouble(ref value13);
		ValueListItem whichItem = ValueListItem.ProngPlacerStyleEnd9;
		InEndProngStyle.GetValueListItem(ref whichItem);
		bool value14 = true;
		InEnableEndProngRotationMirroring.GetBoolean(ref value14);
		bool value15 = false;
		InEnableEndProngEndControls.GetBoolean(ref value15);
		double value16 = 0.25;
		InSizePercentageStart.GetDoublePercentage(ref value16);
		double value17 = 0.0;
		InRotationStart.GetDouble(ref value17);
		double value18 = 45.0;
		InSpreadStart.GetDouble(ref value18);
		double value19 = 0.25;
		InSizePercentageEnd.GetDoublePercentage(ref value19);
		double value20 = 0.0;
		InRotationEnd.GetDouble(ref value20);
		double value21 = 45.0;
		InSpreadEnd.GetDouble(ref value21);
		InBlendTop.GetValue(out double value22);
		InBlendBot.GetValue(out double value23);
		InBlendEdge.GetValue(out double value24);
		InBottomArcPercentage.GetValue(out double value25);
		InCornerProngAngle.GetValue(out double value26);
		InCornerProngSize.GetValue(out int value27);
		InCornerProngSizeOffset.GetValue(out double value28);
		ValueListItem whichItem2 = ValueListItem.ProngPlacerSettingStyleProng;
		InSettingStyle.GetValueListItem(ref whichItem2);
		InProngSizeControl.GetValue(out int value29);
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
		if (!value15)
		{
			value19 = value16;
			value20 = value17;
			if (value14)
			{
				value20 *= -1.0;
			}
			value21 = value18;
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
				string height = GetHeight(CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1), CSUtilGemRails.GetChannelGemData(cSDataArray, i), channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value13, out height2);
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
		InPercentToggle.GetValue(out bool value30);
		double value31 = 0.0;
		if (value30)
		{
			InProngPercentage.GetValue(out value31);
			value31 = 1.0 - value31;
		}
		bool flag2 = CSUtilGemRails.IsClosed(cSDataArray.Array);
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
			double num6 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, num4).Center);
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
			string circles = GetCircles(channelGemData2, channelGemData3, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value13, value30, value31, out circleLeft, out circleRight, out height3);
			if (!string.IsNullOrEmpty(circles))
			{
				return UpdateError(circles);
			}
			if (_prongBase == ValueListItem.FollowLargestGem)
			{
				height3 = (value30 ? (num3 * value31) : num3);
			}
			On3dVector val7 = new On3dVector(circleLeft.Center());
			On3dVector val8 = new On3dVector(circleRight.Center());
			On3dPoint value32 = new On3dPoint((IOn3dVector)(object)((val7 + (IOn3dVector)(object)val8) / 2.0));
			On3dVector value33 = val8 - (IOn3dVector)(object)val7;
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
				Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value32), CSConvert.On3dVectorCopyToVector3d(value33), destination);
				Point3d origin = Point3d.Origin;
				origin.Transform(xform);
				Line line = new Line(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
				line.Transform(xform);
				Curve curve = Curve.CreateInterpolatedCurve(new List<Point3d> { channelGemData2.ZMaxPoint, channelGemData3.ZMaxPoint, channelGemData4.ZMaxPoint }, 3);
				Curve curve2 = curve.DuplicateCurve();
				Curve curve3 = curve.DuplicateCurve();
				curve2.Transform(Transform.Translation(0.0, 4.0, 0.0));
				curve3.Transform(Transform.Translation(0.0, -4.0, 0.0));
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
				Vector3d vector3d = channelGemData3.Center - channelGemData2.Center;
				vector3d.Unitize();
				Vector3d vector3d2 = channelGemData3.Center - channelGemData4.Center;
				vector3d2.Unitize();
				if (num5 == j)
				{
					vector3d2 = vector3d * -1.0;
				}
				if (RhinoMath.ToDegrees(Math.Acos(vector3d * vector3d2)) < value26)
				{
					vector3d *= channelGemData3.GemXSize / 2.0;
					vector3d2 *= channelGemData3.GemXSize / 2.0;
					Vector3d vector3d3 = vector3d + vector3d2;
					Point3d testPoint = channelGemData3.Center + vector3d3;
					channelGemData3.GetCurve().ClosestPoint(testPoint, out var t);
					OnCircle circleLeft2;
					OnCircle circleRight2;
					double height4;
					string circles2 = GetCircles(channelGemData2, channelGemData3, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value11, value12, value4, value13, value30, value31, out circleLeft2, out circleRight2, out height4);
					OnCircle circleLeft3;
					OnCircle circleRight3;
					double height5;
					string circles3 = GetCircles(channelGemData3, channelGemData4, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value11, value12, value4, value13, value30, value31, out circleLeft3, out circleRight3, out height5);
					if (!string.IsNullOrEmpty(circles2) || !string.IsNullOrEmpty(circles3))
					{
						break;
					}
					double num8 = (circleRight3.radius + circleRight2.radius + circleLeft3.radius + circleLeft2.radius) / 4.0 + value28;
					double[] array = new double[4] { circleRight3.radius, circleRight2.radius, circleLeft2.radius, circleLeft3.radius };
					if (value27 == 157)
					{
						double num9 = array[0];
						for (int k = 1; k < array.Length; k++)
						{
							if (num9 < array[k])
							{
								num9 = array[k];
							}
						}
						num8 = num9 + value28;
					}
					if (value27 == 158)
					{
						double num10 = array[0];
						for (int l = 1; l < array.Length; l++)
						{
							if (num10 > array[l])
							{
								num10 = array[l];
							}
						}
						num8 = num10 + value28;
					}
					double num11 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData3.Size) / 2.0) : channelGemData3.GemZMin);
					Point3d value34 = channelGemData3.GetCurve().PointAt(t);
					value34.Transform(Transform.Translation(channelGemData3.Normal * (value13 - num11)));
					vector3d3.Unitize();
					vector3d3 *= num8 - value12;
					value34 += vector3d3;
					On3dPoint val11 = CSConvert.Point3dCopyToOn3dPoint(value34);
					OnCircle val12 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val11, (IOn3dVector)(object)channelGemData3.Normal.Convert()), (IOn3dPoint)(object)val11, num8);
					OnXform val13 = new OnXform();
					val13.ChangeBasis((IOnPlane)(object)val12.plane, OnUtil.On_xy_plane);
					double num12 = val12.radius * 2.0;
					if (num12 > value2 && num12 < value3)
					{
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, num12, height3, value5, value6, value, out var prong, out var prongBaseRail))
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
			if (value29 == 157)
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
			if (value29 == 158)
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
			if (value29 == 159)
			{
				double radius = circleRight.radius;
				double radius2 = circleLeft.radius;
				double num13 = (radius + radius2) / 2.0;
				circleRight = new OnCircle(circleRight.Center(), num13);
				circleLeft = new OnCircle(circleLeft.Center(), num13);
			}
			if (!flag)
			{
				double num14 = circleLeft.radius * 2.0;
				if (num14 > value2 && num14 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num14, height3, value5, value6, value, out var prong2, out var prongBaseRail2))
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
				double num15 = circleRight.radius * 2.0;
				if (num15 > value2 && num15 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num15, height3, value5, value6, value, out var prong3, out var prongBaseRail3))
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
				Brep geometry = MakeBar((val7 - (IOn3dVector)(object)val8).Length(), circleLeft.radius, circleRight.radius, height3, value6, value22, value23, value24, value25);
				value32 = new On3dPoint((IOn3dVector)(object)((val7 + (IOn3dVector)(object)val8) / 2.0));
				value33 = val8 - (IOn3dVector)(object)val7;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value32), CSConvert.On3dVectorCopyToVector3d(value33), CSConvert.On3dVectorCopyToVector3d(val18));
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
				int num16 = ((cSDataArray.GetArrayCount() > 1) ? (m + 1) : 0);
				int index = ((m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1));
				if (cSDataArray.GetArrayCount() > 1)
				{
					double num17;
					if (m != cSDataArray.GetArrayCount() - 1)
					{
						num17 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, m + 1).Center);
					}
					else
					{
						num17 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Center);
						num16 = 0;
					}
					double num18 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).GemXSize * 2.0;
					if (num17 <= num18)
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
						CSChannelGemData channelGemData7 = CSUtilGemRails.GetChannelGemData(cSDataArray, num16);
						Vector3d vector3d4 = Vector3d.Unset;
						if (channelGemData5 != null)
						{
							vector3d4 = channelGemData6.Center - channelGemData5.Center;
						}
						vector3d4.Unitize();
						Vector3d vector3d5 = Vector3d.Unset;
						if (channelGemData7 != null)
						{
							vector3d5 = channelGemData6.Center - channelGemData7.Center;
						}
						vector3d5.Unitize();
						if (num16 == m)
						{
							vector3d5 = vector3d4 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(vector3d4 * vector3d5)) < value26)
						{
							vector3d4 *= channelGemData6.GemXSize / 2.0;
							vector3d5 *= channelGemData6.GemXSize / 2.0;
							Vector3d vector3d6 = vector3d4 + vector3d5;
							Point3d testPoint2 = channelGemData6.Center + vector3d6;
							channelGemData6.GetCurve().ClosestPoint(testPoint2, out var t2);
							if (!string.IsNullOrEmpty(GetCircles(channelGemData5, channelGemData6, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value11, value12, value4, value13, value30, value31, out var circleLeft4, out var circleRight4, out var height6)))
							{
								break;
							}
							double num19 = (circleRight4.radius + circleLeft4.radius) / 2.0 + value28;
							double[] array2 = new double[2] { circleRight4.radius, circleLeft4.radius };
							if (value27 == 157)
							{
								double num20 = array2[0];
								for (int n = 1; n < array2.Length; n++)
								{
									if (num20 < array2[n])
									{
										num20 = array2[n];
									}
								}
								num19 = num20 + value28;
							}
							if (value27 == 158)
							{
								double num21 = array2[0];
								for (int num22 = 1; num22 < array2.Length; num22++)
								{
									if (num21 > array2[num22])
									{
										num21 = array2[num22];
									}
								}
								num19 = num21 + value28;
							}
							double num23 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData6.Size) / 2.0) : channelGemData6.GemZMin);
							Point3d value35 = channelGemData6.GetCurve().PointAt(t2);
							value35.Transform(Transform.Translation(channelGemData6.Normal * (value13 - num23)));
							vector3d6.Unitize();
							vector3d6 *= num19 - value12;
							value35 += vector3d6;
							On3dPoint val19 = CSConvert.Point3dCopyToOn3dPoint(value35);
							OnCircle val20 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val19, (IOn3dVector)(object)channelGemData6.Normal.Convert()), (IOn3dPoint)(object)val19, num19);
							OnXform val21 = new OnXform();
							val21.ChangeBasis((IOnPlane)(object)val20.plane, OnUtil.On_xy_plane);
							double num24 = val20.radius * 2.0;
							if (num24 > value2 && num24 < value3)
							{
								if (!_prongCache.Create((IOnNurbsCurve)(object)val, num24, height6, value5, value6, value, out var prong4, out var prongBaseRail4))
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
						CSChannelGemData channelGemData9 = CSUtilGemRails.GetChannelGemData(cSDataArray, num16);
						CSChannelGemData channelGemData10 = CSUtilGemRails.GetChannelGemData(cSDataArray, (num16 + 1 != cSDataArray.GetArrayCount()) ? (num16 + 1) : 0);
						Vector3d vector3d7 = channelGemData9.Center - channelGemData8.Center;
						vector3d7.Unitize();
						Vector3d vector3d8 = channelGemData9.Center - channelGemData10.Center;
						vector3d8.Unitize();
						if (num16 == m)
						{
							vector3d8 = vector3d7 * -1.0;
						}
						if (RhinoMath.ToDegrees(Math.Acos(vector3d7 * vector3d8)) < value26)
						{
							vector3d7 *= channelGemData9.GemXSize / 2.0;
							vector3d8 *= channelGemData9.GemXSize / 2.0;
							Vector3d vector3d9 = vector3d7 + vector3d8;
							Point3d testPoint3 = channelGemData9.Center + vector3d9;
							channelGemData9.GetCurve().ClosestPoint(testPoint3, out var t3);
							if (!string.IsNullOrEmpty(GetCircles(channelGemData9, channelGemData10, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value11, value12, value4, value13, value30, value31, out var circleLeft5, out var circleRight5, out var height7)))
							{
								break;
							}
							double num25 = (circleRight5.radius + circleLeft5.radius) / 2.0 + value28;
							double[] array3 = new double[2] { circleRight5.radius, circleLeft5.radius };
							if (value27 == 157)
							{
								double num26 = array3[0];
								for (int num27 = 1; num27 < array3.Length; num27++)
								{
									if (num26 < array3[num27])
									{
										num26 = array3[num27];
									}
								}
								num25 = num26 + value28;
							}
							if (value27 == 158)
							{
								double num28 = array3[0];
								for (int num29 = 1; num29 < array3.Length; num29++)
								{
									if (num28 > array3[num29])
									{
										num28 = array3[num29];
									}
								}
								num25 = num28 + value28;
							}
							double num30 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData9.Size) / 2.0) : channelGemData9.GemZMin);
							Point3d value36 = channelGemData9.GetCurve().PointAt(t3);
							value36.Transform(Transform.Translation(channelGemData9.Normal * (value13 - num30)));
							vector3d9.Unitize();
							vector3d9 *= num25 - value12;
							value36 += vector3d9;
							On3dPoint val22 = CSConvert.Point3dCopyToOn3dPoint(value36);
							OnCircle val23 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val22, (IOn3dVector)(object)channelGemData9.Normal.Convert()), (IOn3dPoint)(object)val22, num25);
							OnXform val24 = new OnXform();
							val24.ChangeBasis((IOnPlane)(object)val23.plane, OnUtil.On_xy_plane);
							double num31 = val23.radius * 2.0;
							if (num31 > value2 && num31 < value3)
							{
								if (!_prongCache.Create((IOnNurbsCurve)(object)val, num31, height7, value5, value6, value, out var prong5, out var prongBaseRail5))
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
					CSChannelGemData cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, num16).Duplicate() as CSChannelGemData;
					if (cSDataArray.GetArrayCount() == 1)
					{
						cSChannelGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData.Normal, cSChannelGemData.Center));
					}
					double num33 = cSChannelGemData.GemXSize * value16 + value9 + value10;
					double height9 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + value4 - value13) : channelGemData.GemZSize);
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num33, height9, value5, value6, value, out var prong7, out var prongBaseRail7))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
					double num34 = (cSChannelGemData.GemXSize * value16 + value9) / 2.0;
					double num35 = value13 - cSChannelGemData.GemZMin;
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
					OnPlane val35 = new OnPlane((IOn3dPoint)(object)(obj + (IOn3dVector)(object)(val34 * num34) + (IOn3dVector)(object)(val31 * num35)), (IOn3dVector)(object)val33, (IOn3dVector)(object)val34);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd4:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd7:
					{
						OnXform val51 = new OnXform();
						val51.ChangeBasis((IOnPlane)(object)val35, OnUtil.On_xy_plane);
						OnXform val52 = new OnXform();
						val52.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value17), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
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
						double num36 = value18 / 2.0;
						OnXform val36 = new OnXform();
						val36.ChangeBasis((IOnPlane)(object)val35, OnUtil.On_xy_plane);
						OnXform val37 = new OnXform();
						val37.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num36), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val38 = new OnXform();
						val38.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value17 + num36), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
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
						val42.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num36), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val43 = new OnXform();
						val43.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value17 - num36), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
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
							Brep geometry3 = MakeBar((val47 - (IOn3dPoint)(object)val48).Length(), num33 / 2.0, num33 / 2.0, height9, value6, value22, value23, value24, value25);
							On3dPoint value43 = new On3dPoint((IOn3dPoint)(object)((val47 + (IOn3dPoint)(object)val48) / 2.0));
							On3dVector value44 = val48 - (IOn3dPoint)(object)val47;
							On3dVector value45 = val49 + (IOn3dVector)(object)val50;
							Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value43), CSConvert.On3dVectorCopyToVector3d(value44), CSConvert.On3dVectorCopyToVector3d(value45));
							concurrentBag.Add(new CSOnGeometryData(geometry3, transform6));
						}
						break;
					}
					}
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd10:
				{
					CSChannelGemData channelGemData11 = CSUtilGemRails.GetChannelGemData(cSDataArray, num16);
					CSChannelGemData channelGemData12 = CSUtilGemRails.GetChannelGemData(cSDataArray, num16 + 1);
					if (channelGemData11 == null || channelGemData12 == null)
					{
						return UpdateError("Invalid gems");
					}
					CSDataHelper.SetSettingType(channelGemData12, flag ? GemSettingType.BR : GemSettingType.PR);
					OnCircle circleLeft6;
					OnCircle circleRight6;
					double height8;
					string circles4 = GetCircles(channelGemData11, channelGemData12, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value9, value10, value4, value13, value30, value31, out circleLeft6, out circleRight6, out height8);
					if (!string.IsNullOrEmpty(circles4))
					{
						return UpdateError(circles4);
					}
					On3dVector val25 = new On3dVector(circleLeft6.Center());
					On3dVector val26 = new On3dVector(circleRight6.Center());
					if (value29 == 157)
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
					if (value29 == 158)
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
					if (value29 == 159)
					{
						double radius3 = circleRight6.radius;
						double radius4 = circleLeft6.radius;
						double num32 = (radius3 + radius4) / 2.0;
						circleRight6 = new OnCircle(circleRight6.Center(), num32);
						circleLeft6 = new OnCircle(circleLeft6.Center(), num32);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height8 = (value30 ? (num3 * value31) : num3);
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
					On3dVector value37 = val28 + (IOn3dVector)(object)val30;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						Circle circle = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData12.ZMaxPoint.DistanceTo(Point3d.Origin));
						On3dPoint value38 = new On3dPoint((IOn3dVector)(object)((val25 + (IOn3dVector)(object)val26) / 2.0));
						On3dVector value39 = val26 - (IOn3dVector)(object)val25;
						Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value38), CSConvert.On3dVectorCopyToVector3d(value39), CSConvert.On3dVectorCopyToVector3d(value37));
						Point3d origin2 = Point3d.Origin;
						origin2.Transform(xform2);
						Line line2 = new Line(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
						line2.Transform(xform2);
						Point3d point3d2 = line2.ClosestPoint(circle.ClosestPoint(line2.PointAt(1.0)), limitToFiniteSegment: true);
						if (point3d2.DistanceTo(origin2) > height8 * 0.8)
						{
							height8 = point3d2.DistanceTo(origin2) * 1.2;
						}
					}
					if (!flag)
					{
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft6.Diameter(), height8, value5, value6, value, out var prong6, out var prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value40 = val26 - (IOn3dVector)(object)val25;
						Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val25)), CSConvert.On3dVectorCopyToVector3d(value40), CSConvert.On3dVectorCopyToVector3d(value37));
						Vector3d normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), transform3 * transform2));
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight6.Diameter(), height8, value5, value6, value, out prong6, out prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						value40 = val25 - (IOn3dVector)(object)val26;
						transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val26)), CSConvert.On3dVectorCopyToVector3d(value40), CSConvert.On3dVectorCopyToVector3d(value37));
						normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), transform3 * transform2));
					}
					else
					{
						Brep geometry2 = MakeBar((val25 - (IOn3dVector)(object)val26).Length(), circleLeft6.radius, circleRight6.radius, height8, value6, value22, value23, value24, value25);
						On3dPoint value41 = new On3dPoint((IOn3dVector)(object)((val25 + (IOn3dVector)(object)val26) / 2.0));
						On3dVector value42 = val26 - (IOn3dVector)(object)val25;
						Transform transform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value41), CSConvert.On3dVectorCopyToVector3d(value42), CSConvert.On3dVectorCopyToVector3d(value37));
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
					double num38 = cSChannelGemData2.GemXSize * value19 + value9 + value10;
					double height11 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData2.GemZSize + value4 - value13) : channelGemData.GemZSize);
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num38, height11, value5, value6, value, out var prong9, out var prongBaseRail9))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint obj2 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					double num39 = (cSChannelGemData2.GemXSize * value19 + value9) / 2.0;
					double num40 = value13 - cSChannelGemData2.GemZMin;
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
					OnPlane val64 = new OnPlane((IOn3dPoint)(object)(obj2 + (IOn3dVector)(object)(val63 * num39) + (IOn3dVector)(object)(val60 * num40)), (IOn3dVector)(object)val62, (IOn3dVector)(object)val63);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd2:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd8:
					{
						OnXform val80 = new OnXform();
						val80.ChangeBasis((IOnPlane)(object)val64, OnUtil.On_xy_plane);
						OnXform val81 = new OnXform();
						val81.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value20), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
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
						double num41 = value21 / 2.0;
						OnXform val65 = new OnXform();
						val65.ChangeBasis((IOnPlane)(object)val64, OnUtil.On_xy_plane);
						OnXform val66 = new OnXform();
						val66.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num41), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val67 = new OnXform();
						val67.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value20 + num41), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
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
						val71.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num41), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform val72 = new OnXform();
						val72.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value20 - num41), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
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
							Brep geometry5 = MakeBar((val76 - (IOn3dPoint)(object)val77).Length(), num38 / 2.0, num38 / 2.0, height11, value6, value22, value23, value24, value25);
							On3dPoint value52 = new On3dPoint((IOn3dPoint)(object)((val76 + (IOn3dPoint)(object)val77) / 2.0));
							On3dVector value53 = val77 - (IOn3dPoint)(object)val76;
							On3dVector value54 = val78 + (IOn3dVector)(object)val79;
							Transform transform11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value52), CSConvert.On3dVectorCopyToVector3d(value53), CSConvert.On3dVectorCopyToVector3d(value54));
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
					OnCircle circleLeft7;
					OnCircle circleRight7;
					double height10;
					string circles5 = GetCircles(channelGemData13, channelGemData14, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value9, value10, value4, value13, value30, value31, out circleLeft7, out circleRight7, out height10);
					if (!string.IsNullOrEmpty(circles5))
					{
						return UpdateError(circles5);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height10 = (value30 ? (num3 * value31) : num3);
					}
					if (value29 == 157)
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
					if (value29 == 158)
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
					if (value29 == 159)
					{
						double radius5 = circleRight7.radius;
						double radius6 = circleLeft7.radius;
						double num37 = (radius5 + radius6) / 2.0;
						circleRight7 = new OnCircle(circleRight7.Center(), num37);
						circleLeft7 = new OnCircle(circleLeft7.Center(), num37);
					}
					On3dVector val54 = new On3dVector(circleLeft7.Center());
					On3dVector val55 = new On3dVector(circleRight7.Center());
					OnXform val56 = new OnXform();
					val56.ChangeBasis((IOnPlane)(object)circleLeft7.plane, OnUtil.On_xy_plane);
					On3dVector val57 = new On3dVector(OnUtil.On_zaxis);
					val57.Transform((IOnXform)(object)val56);
					val57.Unitize();
					OnXform val58 = new OnXform();
					val58.ChangeBasis((IOnPlane)(object)circleRight7.plane, OnUtil.On_xy_plane);
					On3dVector val59 = new On3dVector(OnUtil.On_zaxis);
					val59.Transform((IOnXform)(object)val58);
					val59.Unitize();
					On3dVector value46 = val57 + (IOn3dVector)(object)val59;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						Circle circle2 = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData14.ZMaxPoint.DistanceTo(Point3d.Origin));
						On3dPoint value47 = new On3dPoint((IOn3dVector)(object)((val54 + (IOn3dVector)(object)val55) / 2.0));
						On3dVector value48 = val55 - (IOn3dVector)(object)val54;
						Transform xform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value47), CSConvert.On3dVectorCopyToVector3d(value48), CSConvert.On3dVectorCopyToVector3d(value46));
						Point3d origin3 = Point3d.Origin;
						origin3.Transform(xform3);
						Line line3 = new Line(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
						line3.Transform(xform3);
						Point3d point3d3 = line3.ClosestPoint(circle2.ClosestPoint(line3.PointAt(1.0)), limitToFiniteSegment: true);
						if (point3d3.DistanceTo(origin3) > height10 * 0.8)
						{
							height10 = point3d3.DistanceTo(origin3) * 1.2;
						}
					}
					if (!flag)
					{
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft7.Diameter(), height10, value5, value6, value, out var prong8, out var prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value49 = val55 - (IOn3dVector)(object)val54;
						Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val54)), CSConvert.On3dVectorCopyToVector3d(value49), CSConvert.On3dVectorCopyToVector3d(value46));
						Vector3d normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), transform8 * transform7));
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight7.Diameter(), height10, value5, value6, value, out prong8, out prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						value49 = val54 - (IOn3dVector)(object)val55;
						transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val55)), CSConvert.On3dVectorCopyToVector3d(value49), CSConvert.On3dVectorCopyToVector3d(value46));
						normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong8).Convert(), transform8 * transform7));
					}
					else
					{
						Brep geometry4 = MakeBar((val54 - (IOn3dVector)(object)val55).Length(), circleLeft7.radius, circleRight7.radius, height10, value6, value22, value23, value24, value25);
						On3dPoint value50 = new On3dPoint((IOn3dVector)(object)((val54 + (IOn3dVector)(object)val55) / 2.0));
						On3dVector value51 = val55 - (IOn3dVector)(object)val54;
						Transform transform9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value50), CSConvert.On3dVectorCopyToVector3d(value51), CSConvert.On3dVectorCopyToVector3d(value46));
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
