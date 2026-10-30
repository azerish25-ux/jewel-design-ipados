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
		//IL_0fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdf: Expected O, but got Unknown
		//IL_0fdf: Expected O, but got Unknown
		//IL_102f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1036: Expected O, but got Unknown
		//IL_1057: Unknown result type (might be due to invalid IL or missing references)
		//IL_105e: Expected O, but got Unknown
		//IL_101e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1025: Expected O, but got Unknown
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
		//IL_1132: Expected O, but got Unknown
		//IL_1191: Unknown result type (might be due to invalid IL or missing references)
		//IL_1198: Expected O, but got Unknown
		//IL_11a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ae: Expected O, but got Unknown
		//IL_11d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1204: Expected O, but got Unknown
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Expected O, but got Unknown
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Expected O, but got Unknown
		//IL_126d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1274: Expected O, but got Unknown
		//IL_1296: Unknown result type (might be due to invalid IL or missing references)
		//IL_129b: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ca: Expected O, but got Unknown
		//IL_0b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8c: Expected O, but got Unknown
		//IL_0ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Expected O, but got Unknown
		//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Expected O, but got Unknown
		//IL_0bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Expected O, but got Unknown
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
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3a: Expected O, but got Unknown
		//IL_0c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c23: Expected O, but got Unknown
		//IL_19ca: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_1a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a64: Expected O, but got Unknown
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
		//IL_1a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a88: Expected O, but got Unknown
		//IL_1ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aca: Expected O, but got Unknown
		//IL_15fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1605: Expected O, but got Unknown
		//IL_1af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b00: Expected O, but got Unknown
		//IL_1b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b40: Expected O, but got Unknown
		//IL_1b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba6: Expected O, but got Unknown
		//IL_1bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbc: Expected O, but got Unknown
		//IL_1bde: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c12: Expected O, but got Unknown
		//IL_1873: Unknown result type (might be due to invalid IL or missing references)
		//IL_187a: Expected O, but got Unknown
		//IL_0e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1b: Expected O, but got Unknown
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e29: Expected O, but got Unknown
		//IL_0e70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e77: Expected O, but got Unknown
		//IL_1c7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c82: Expected O, but got Unknown
		//IL_1ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd8: Expected O, but got Unknown
		//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Expected O, but got Unknown
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Expected O, but got Unknown
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae2: Expected O, but got Unknown
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
		//IL_201a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2021: Expected O, but got Unknown
		//IL_228f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2296: Expected O, but got Unknown
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
			Vector3d vector3d = channelGemData3.Center - channelGemData2.Center;
			vector3d.Unitize();
			Vector3d vector3d2 = channelGemData3.Center - channelGemData4.Center;
			vector3d2.Unitize();
			if (num5 == j)
			{
				vector3d2 = vector3d * -1.0;
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
			On3dVector val7 = new On3dVector(circleLeft.Center());
			On3dVector val8 = new On3dVector(circleRight.Center());
			On3dPoint value28 = new On3dPoint((IOn3dVector)(object)((val7 + (IOn3dVector)(object)val8) / 2.0));
			On3dVector value29 = val8 - (IOn3dVector)(object)val7;
			if (CSApplicationHelper.Application.IsCamMode)
			{
				Circle circle = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData3.ZMaxPoint.DistanceTo(Point3d.Origin));
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
				Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value28), CSConvert.On3dVectorCopyToVector3d(value29), destination);
				Point3d origin = Point3d.Origin;
				origin.Transform(xform);
				Line line = new Line(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
				line.Transform(xform);
				Point3d point3d = line.ClosestPoint(circle.ClosestPoint(line.PointAt(1.0)), limitToFiniteSegment: true);
				if (point3d.DistanceTo(origin) > height3 * 0.8)
				{
					height3 = point3d.DistanceTo(origin) * 1.2;
				}
			}
			if (!flag && RhinoMath.ToDegrees(Math.Acos(vector3d * vector3d2)) < value22)
			{
				vector3d *= channelGemData3.GemXSize / 2.0;
				vector3d2 *= channelGemData3.GemXSize / 2.0;
				Vector3d vector3d3 = vector3d + vector3d2;
				Point3d testPoint = channelGemData3.Center + vector3d3;
				channelGemData3.GetCurve().ClosestPoint(testPoint, out var t);
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
				double num6 = (circleRight3.radius + circleRight2.radius + circleLeft3.radius + circleLeft2.radius) / 4.0 + value24;
				double[] array = new double[4] { circleRight3.radius, circleRight2.radius, circleLeft2.radius, circleLeft3.radius };
				if (value23 == 157)
				{
					double num7 = array[0];
					for (int k = 1; k < array.Length; k++)
					{
						if (num7 < array[k])
						{
							num7 = array[k];
						}
					}
					num6 = num7 + value24;
				}
				if (value23 == 158)
				{
					double num8 = array[0];
					for (int l = 1; l < array.Length; l++)
					{
						if (num8 > array[l])
						{
							num8 = array[l];
						}
					}
					num6 = num8 + value24;
				}
				double num9 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData3.Size) / 2.0) : channelGemData3.GemZMin);
				Point3d value30 = channelGemData3.GetCurve().PointAt(t);
				value30.Transform(Transform.Translation(channelGemData3.Normal * (value9 - num9)));
				vector3d3.Unitize();
				vector3d3 *= num6;
				value30 += vector3d3;
				On3dPoint val11 = CSConvert.Point3dCopyToOn3dPoint(value30);
				OnCircle val12 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val11, (IOn3dVector)(object)channelGemData3.Normal.Convert()), (IOn3dPoint)(object)val11, num6);
				OnXform val13 = new OnXform();
				val13.ChangeBasis((IOnPlane)(object)val12.plane, OnUtil.On_xy_plane);
				double num10 = val12.radius * 2.0;
				if (num10 > value2 && num10 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num10, height3, value5, value6, value, out var prong, out var prongBaseRail))
					{
						return UpdateError("Create prong cache failed");
					}
					prong.Transform((IOnXform)(object)val13);
					prongBaseRail.Transform((IOnXform)(object)val13);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val13.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val13.Convert(), applyTransform: false));
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
				double num11 = (radius + radius2) / 2.0;
				circleRight = new OnCircle(circleRight.Center(), num11);
				circleLeft = new OnCircle(circleLeft.Center(), num11);
			}
			if (!flag)
			{
				double num12 = circleLeft.radius * 2.0;
				if (num12 > value2 && num12 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num12, height3, value5, value6, value, out var prong2, out var prongBaseRail2))
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
				double num13 = circleRight.radius * 2.0;
				if (num13 > value2 && num13 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num13, height3, value5, value6, value, out var prong3, out var prongBaseRail3))
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
				Brep geometry = MakeBar((val7 - (IOn3dVector)(object)val8).Length(), circleLeft.radius, circleRight.radius, height3, value6, value18, value19, value20, value21);
				value28 = new On3dPoint((IOn3dVector)(object)((val7 + (IOn3dVector)(object)val8) / 2.0));
				value29 = val8 - (IOn3dVector)(object)val7;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value28), CSConvert.On3dVectorCopyToVector3d(value29), CSConvert.On3dVectorCopyToVector3d(val18));
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
				double num14 = cSChannelGemData.GemXSize * value12 + value7 + value8;
				double height5 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, num14, height5, value5, value6, value, out var prong5, out var prongBaseRail5))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint obj = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) - (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
				double num15 = (cSChannelGemData.GemXSize * value12 + value7) / 2.0;
				double num16 = value9 - cSChannelGemData.GemZMin;
				On3dVector val25;
				if (val5 == OnUtil.On_UNSET_VECTOR)
				{
					val25 = new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert());
					val25.Unitize();
				}
				else
				{
					OnXform val26 = new OnXform();
					val26.Mirror((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
					val25 = new On3dVector((IOn3dVector)(object)val5);
					val25.Transform((IOnXform)(object)val26);
					val25.Unitize();
				}
				On3dVector val27 = new On3dVector((IOn3dVector)(object)cSChannelGemData.LeftVector.Convert());
				val27.Unitize();
				On3dVector val28 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val25, (IOn3dVector)(object)val27);
				val28.Unitize();
				OnPlane val29 = new OnPlane((IOn3dPoint)(object)(obj + (IOn3dVector)(object)(val28 * num15) + (IOn3dVector)(object)(val25 * num16)), (IOn3dVector)(object)val27, (IOn3dVector)(object)val28);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				{
					OnXform val45 = new OnXform();
					val45.ChangeBasis((IOnPlane)(object)val29, OnUtil.On_xy_plane);
					OnXform val46 = new OnXform();
					val46.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val47 = val46 * (IOnXform)(object)val45;
					prong5.Transform((IOnXform)(object)val47);
					prongBaseRail5.Transform((IOnXform)(object)val47);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), val47.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail5).Convert(), val47.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num17 = value14 / 2.0;
					OnXform val30 = new OnXform();
					val30.ChangeBasis((IOnPlane)(object)val29, OnUtil.On_xy_plane);
					OnXform val31 = new OnXform();
					val31.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num17), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val32 = new OnXform();
					val32.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 + num17), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val33 = val32 * (IOnXform)(object)val30 * (IOnXform)(object)val31;
					if (!flag)
					{
						OnGeometry val34 = prong5.Duplicate();
						val34.Transform((IOnXform)(object)val33);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val34).Convert(), val33.Convert(), applyTransform: false));
						OnGeometry val35 = prongBaseRail5.Duplicate();
						val35.Transform((IOnXform)(object)val33);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val35).Convert(), val33.Convert(), applyTransform: false));
					}
					OnXform val36 = new OnXform();
					val36.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num17), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val37 = new OnXform();
					val37.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 - num17), (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val38 = val37 * (IOnXform)(object)val30 * (IOnXform)(object)val36;
					if (!flag)
					{
						OnGeometry val39 = prong5.Duplicate();
						val39.Transform((IOnXform)(object)val38);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val39).Convert(), val38.Convert(), applyTransform: false));
						OnGeometry val40 = prongBaseRail5.Duplicate();
						val40.Transform((IOnXform)(object)val38);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val40).Convert(), val38.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val41 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val42 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val43 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val44 = new On3dVector(0.0, 0.0, 1.0);
						val41.Transform((IOnXform)(object)val33);
						val43.Transform((IOnXform)(object)val33);
						val42.Transform((IOnXform)(object)val38);
						val44.Transform((IOnXform)(object)val38);
						Brep geometry3 = MakeBar((val41 - (IOn3dPoint)(object)val42).Length(), num14 / 2.0, num14 / 2.0, height5, value6, value18, value19, value20, value21);
						On3dPoint value37 = new On3dPoint((IOn3dPoint)(object)((val41 + (IOn3dPoint)(object)val42) / 2.0));
						On3dVector value38 = val42 - (IOn3dPoint)(object)val41;
						On3dVector value39 = val43 + (IOn3dVector)(object)val44;
						Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value37), CSConvert.On3dVectorCopyToVector3d(value38), CSConvert.On3dVectorCopyToVector3d(value39));
						concurrentBag.Add(new CSOnGeometryData(geometry3, transform6));
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
				On3dVector val19 = new On3dVector(circleLeft4.Center());
				On3dVector val20 = new On3dVector(circleRight4.Center());
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height4 = (value26 ? (num3 * value27) : num3);
				}
				OnXform val21 = new OnXform();
				val21.ChangeBasis((IOnPlane)(object)circleLeft4.plane, OnUtil.On_xy_plane);
				On3dVector val22 = new On3dVector(OnUtil.On_zaxis);
				val22.Transform((IOnXform)(object)val21);
				val22.Unitize();
				OnXform val23 = new OnXform();
				val23.ChangeBasis((IOnPlane)(object)circleRight4.plane, OnUtil.On_xy_plane);
				On3dVector val24 = new On3dVector(OnUtil.On_zaxis);
				val24.Transform((IOnXform)(object)val23);
				val24.Unitize();
				On3dVector value31 = val22 + (IOn3dVector)(object)val24;
				if (CSApplicationHelper.Application.IsCamMode)
				{
					Circle circle2 = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData6.ZMaxPoint.DistanceTo(Point3d.Origin));
					On3dPoint value32 = new On3dPoint((IOn3dVector)(object)((val19 + (IOn3dVector)(object)val20) / 2.0));
					On3dVector value33 = val20 - (IOn3dVector)(object)val19;
					Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value32), CSConvert.On3dVectorCopyToVector3d(value33), CSConvert.On3dVectorCopyToVector3d(value31));
					Point3d origin2 = Point3d.Origin;
					origin2.Transform(xform2);
					Line line2 = new Line(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
					line2.Transform(xform2);
					Point3d point3d2 = line2.ClosestPoint(circle2.ClosestPoint(line2.PointAt(1.0)), limitToFiniteSegment: true);
					if (point3d2.DistanceTo(origin2) > height4 * 0.8)
					{
						height4 = point3d2.DistanceTo(origin2) * 1.2;
					}
				}
				if (!flag)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft4.Diameter(), height4, value5, value6, value, out var prong4, out var prongBaseRail4))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value34 = val20 - (IOn3dVector)(object)val19;
					Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val19)), CSConvert.On3dVectorCopyToVector3d(value34), CSConvert.On3dVectorCopyToVector3d(value31));
					Vector3d normalToMirrorPlane = channelGemData5.Center - channelGemData5.XMaxPoint;
					Transform transform3 = Transform.Mirror(channelGemData5.Center, normalToMirrorPlane);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), transform3 * transform2));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight4.Diameter(), height4, value5, value6, value, out prong4, out prongBaseRail4))
					{
						return UpdateError("Create prong cache failed");
					}
					value34 = val19 - (IOn3dVector)(object)val20;
					transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val20)), CSConvert.On3dVectorCopyToVector3d(value34), CSConvert.On3dVectorCopyToVector3d(value31));
					normalToMirrorPlane = channelGemData5.Center - channelGemData5.XMaxPoint;
					transform3 = Transform.Mirror(channelGemData5.Center, normalToMirrorPlane);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), transform3 * transform2));
				}
				else
				{
					Brep geometry2 = MakeBar((val19 - (IOn3dVector)(object)val20).Length(), circleLeft4.radius, circleRight4.radius, height4, value6, value18, value19, value20, value21);
					On3dPoint value35 = new On3dPoint((IOn3dVector)(object)((val19 + (IOn3dVector)(object)val20) / 2.0));
					On3dVector value36 = val20 - (IOn3dVector)(object)val19;
					Transform transform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value35), CSConvert.On3dVectorCopyToVector3d(value36), CSConvert.On3dVectorCopyToVector3d(value31));
					Vector3d normalToMirrorPlane2 = channelGemData5.Center - channelGemData5.XMaxPoint;
					Transform transform5 = Transform.Mirror(channelGemData5.Center, normalToMirrorPlane2);
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
				CSChannelGemData cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.GetArrayCount() - 1).Duplicate() as CSChannelGemData;
				if (cSDataArray.GetArrayCount() == 1)
				{
					cSChannelGemData2.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData2.Normal, cSChannelGemData2.Center));
				}
				double num18 = cSChannelGemData2.GemXSize * value15 + value7 + value8;
				double height7 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData2.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, num18, height7, value5, value6, value, out var prong7, out var prongBaseRail7))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint obj2 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
				double num19 = (cSChannelGemData2.GemXSize * value15 + value7) / 2.0;
				double num20 = value9 - cSChannelGemData2.GemZMin;
				On3dVector val54;
				if (val6 == OnUtil.On_UNSET_VECTOR)
				{
					val54 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert());
					val54.Unitize();
				}
				else
				{
					OnXform val55 = new OnXform();
					val55.Mirror((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					val54 = new On3dVector((IOn3dVector)(object)val6);
					val54.Transform((IOnXform)(object)val55);
					val54.Unitize();
				}
				On3dVector val56 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.RightVector.Convert());
				val56.Unitize();
				On3dVector val57 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val54, (IOn3dVector)(object)val56);
				val57.Unitize();
				OnPlane val58 = new OnPlane((IOn3dPoint)(object)(obj2 + (IOn3dVector)(object)(val57 * num19) + (IOn3dVector)(object)(val54 * num20)), (IOn3dVector)(object)val56, (IOn3dVector)(object)val57);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd2:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd8:
				{
					OnXform val74 = new OnXform();
					val74.ChangeBasis((IOnPlane)(object)val58, OnUtil.On_xy_plane);
					OnXform val75 = new OnXform();
					val75.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val76 = val75 * (IOnXform)(object)val74;
					prong7.Transform((IOnXform)(object)val76);
					prongBaseRail7.Transform((IOnXform)(object)val76);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong7).Convert(), val76.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail7).Convert(), val76.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd3:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num21 = value17 / 2.0;
					OnXform val59 = new OnXform();
					val59.ChangeBasis((IOnPlane)(object)val58, OnUtil.On_xy_plane);
					OnXform val60 = new OnXform();
					val60.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num21), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val61 = new OnXform();
					val61.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 + num21), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val62 = val61 * (IOnXform)(object)val59 * (IOnXform)(object)val60;
					if (!flag)
					{
						OnGeometry val63 = prong7.Duplicate();
						val63.Transform((IOnXform)(object)val62);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val63).Convert(), val62.Convert(), applyTransform: false));
						OnGeometry val64 = prongBaseRail7.Duplicate();
						val64.Transform((IOnXform)(object)val62);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val64).Convert(), val62.Convert(), applyTransform: false));
					}
					OnXform val65 = new OnXform();
					val65.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num21), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val66 = new OnXform();
					val66.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 - num21), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val67 = val66 * (IOnXform)(object)val59 * (IOnXform)(object)val65;
					if (!flag)
					{
						OnGeometry val68 = prong7.Duplicate();
						val68.Transform((IOnXform)(object)val67);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val68).Convert(), val67.Convert(), applyTransform: false));
						OnGeometry val69 = prongBaseRail7.Duplicate();
						val69.Transform((IOnXform)(object)val67);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val69).Convert(), val67.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val70 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val71 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val72 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val73 = new On3dVector(0.0, 0.0, 1.0);
						val70.Transform((IOnXform)(object)val62);
						val72.Transform((IOnXform)(object)val62);
						val71.Transform((IOnXform)(object)val67);
						val73.Transform((IOnXform)(object)val67);
						Brep geometry5 = MakeBar((val70 - (IOn3dPoint)(object)val71).Length(), num18 / 2.0, num18 / 2.0, height7, value6, value18, value19, value20, value21);
						On3dPoint value46 = new On3dPoint((IOn3dPoint)(object)((val70 + (IOn3dPoint)(object)val71) / 2.0));
						On3dVector value47 = val71 - (IOn3dPoint)(object)val70;
						On3dVector value48 = val72 + (IOn3dVector)(object)val73;
						Transform transform11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value46), CSConvert.On3dVectorCopyToVector3d(value47), CSConvert.On3dVectorCopyToVector3d(value48));
						concurrentBag.Add(new CSOnGeometryData(geometry5, transform11));
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
				On3dVector val48 = new On3dVector(circleLeft5.Center());
				On3dVector val49 = new On3dVector(circleRight5.Center());
				OnXform val50 = new OnXform();
				val50.ChangeBasis((IOnPlane)(object)circleLeft5.plane, OnUtil.On_xy_plane);
				On3dVector val51 = new On3dVector(OnUtil.On_zaxis);
				val51.Transform((IOnXform)(object)val50);
				val51.Unitize();
				OnXform val52 = new OnXform();
				val52.ChangeBasis((IOnPlane)(object)circleRight5.plane, OnUtil.On_xy_plane);
				On3dVector val53 = new On3dVector(OnUtil.On_zaxis);
				val53.Transform((IOnXform)(object)val52);
				val53.Unitize();
				On3dVector value40 = val51 + (IOn3dVector)(object)val53;
				if (CSApplicationHelper.Application.IsCamMode)
				{
					Circle circle3 = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData8.ZMaxPoint.DistanceTo(Point3d.Origin));
					On3dPoint value41 = new On3dPoint((IOn3dVector)(object)((val48 + (IOn3dVector)(object)val49) / 2.0));
					On3dVector value42 = val49 - (IOn3dVector)(object)val48;
					Transform xform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value41), CSConvert.On3dVectorCopyToVector3d(value42), CSConvert.On3dVectorCopyToVector3d(value40));
					Point3d origin3 = Point3d.Origin;
					origin3.Transform(xform3);
					Line line3 = new Line(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
					line3.Transform(xform3);
					Point3d point3d3 = line3.ClosestPoint(circle3.ClosestPoint(line3.PointAt(1.0)), limitToFiniteSegment: true);
					if (point3d3.DistanceTo(origin3) > height6 * 0.8)
					{
						height6 = point3d3.DistanceTo(origin3) * 1.2;
					}
				}
				if (!flag)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft5.Diameter(), height6, value5, value6, value, out var prong6, out var prongBaseRail6))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value43 = val49 - (IOn3dVector)(object)val48;
					Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val48)), CSConvert.On3dVectorCopyToVector3d(value43), CSConvert.On3dVectorCopyToVector3d(value40));
					Vector3d normalToMirrorPlane3 = channelGemData8.Center - channelGemData8.XMaxPoint;
					Transform transform8 = Transform.Mirror(channelGemData8.Center, normalToMirrorPlane3);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), transform8 * transform7));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight5.Diameter(), height6, value5, value6, value, out prong6, out prongBaseRail6))
					{
						return UpdateError("Create prong cache failed");
					}
					value43 = val48 - (IOn3dVector)(object)val49;
					transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val49)), CSConvert.On3dVectorCopyToVector3d(value43), CSConvert.On3dVectorCopyToVector3d(value40));
					normalToMirrorPlane3 = channelGemData8.Center - channelGemData8.XMaxPoint;
					transform8 = Transform.Mirror(channelGemData8.Center, normalToMirrorPlane3);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), transform8 * transform7));
				}
				else
				{
					Brep geometry4 = MakeBar((val48 - (IOn3dVector)(object)val49).Length(), circleLeft5.radius, circleRight5.radius, height6, value6, value18, value19, value20, value21);
					On3dPoint value44 = new On3dPoint((IOn3dVector)(object)((val48 + (IOn3dVector)(object)val49) / 2.0));
					On3dVector value45 = val49 - (IOn3dVector)(object)val48;
					Transform transform9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value44), CSConvert.On3dVectorCopyToVector3d(value45), CSConvert.On3dVectorCopyToVector3d(value40));
					Vector3d normalToMirrorPlane4 = channelGemData8.Center - channelGemData8.XMaxPoint;
					Transform transform10 = Transform.Mirror(channelGemData8.Center, normalToMirrorPlane4);
					concurrentBag.Add(new CSOnGeometryData(geometry4, transform10 * transform9));
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
