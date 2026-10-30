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
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Expected O, but got Unknown
		//IL_09be: Expected O, but got Unknown
		//IL_0a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Expected O, but got Unknown
		//IL_0a36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3d: Expected O, but got Unknown
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a04: Expected O, but got Unknown
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a61: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa3: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_0aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad1: Expected O, but got Unknown
		//IL_0ae0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b11: Expected O, but got Unknown
		//IL_0b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8d: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be3: Expected O, but got Unknown
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Expected O, but got Unknown
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Expected O, but got Unknown
		//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Expected O, but got Unknown
		//IL_0822: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Expected O, but got Unknown
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Expected O, but got Unknown
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
		//IL_12e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1301: Expected O, but got Unknown
		//IL_1301: Expected O, but got Unknown
		//IL_1351: Unknown result type (might be due to invalid IL or missing references)
		//IL_1358: Expected O, but got Unknown
		//IL_1379: Unknown result type (might be due to invalid IL or missing references)
		//IL_1380: Expected O, but got Unknown
		//IL_1340: Unknown result type (might be due to invalid IL or missing references)
		//IL_1347: Expected O, but got Unknown
		//IL_139d: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a4: Expected O, but got Unknown
		//IL_13df: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e6: Expected O, but got Unknown
		//IL_1415: Unknown result type (might be due to invalid IL or missing references)
		//IL_141c: Expected O, but got Unknown
		//IL_142b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1430: Unknown result type (might be due to invalid IL or missing references)
		//IL_145c: Expected O, but got Unknown
		//IL_14bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c2: Expected O, but got Unknown
		//IL_14d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d8: Expected O, but got Unknown
		//IL_14fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ff: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_1597: Unknown result type (might be due to invalid IL or missing references)
		//IL_159e: Expected O, but got Unknown
		//IL_15c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f4: Expected O, but got Unknown
		//IL_11cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d2: Expected O, but got Unknown
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
		//IL_1b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2b: Expected O, but got Unknown
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
				Point3d point3d = CSConvert.On3dPointCopyToPoint3d(circleLeft.Center());
				if (channelGemData3.ZMaxPoint.DistanceTo(Point3d.Origin) > point3d.DistanceTo(Point3d.Origin) + num5)
				{
					height3 += channelGemData3.ZMaxPoint.DistanceTo(Point3d.Origin) - (point3d.DistanceTo(Point3d.Origin) + num5);
				}
			}
			OnXform val7 = new OnXform();
			val7.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
			On3dVector val8 = new On3dVector(OnUtil.On_zaxis);
			val8.Transform((IOnXform)(object)val7);
			val8.Unitize();
			if (!flag)
			{
				double num6 = circleLeft.radius * 2.0;
				if (num6 > value2 && num6 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num6, height3, value5, value6, value, out var prong, out var prongBaseRail))
					{
						return UpdateError("Create prong cache failed");
					}
					prong.Transform((IOnXform)(object)val7);
					prongBaseRail.Transform((IOnXform)(object)val7);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val7.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val7.Convert(), applyTransform: false));
				}
			}
			OnXform val9 = new OnXform();
			val9.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
			On3dVector val10 = new On3dVector(OnUtil.On_zaxis);
			val10.Transform((IOnXform)(object)val9);
			val10.Unitize();
			if (!flag)
			{
				double num7 = circleRight.radius * 2.0;
				if (num7 > value2 && num7 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num7, height3, value5, value6, value, out var prong2, out var prongBaseRail2))
					{
						return UpdateError("Create prong cache failed");
					}
					prong2.Transform((IOnXform)(object)val9);
					prongBaseRail2.Transform((IOnXform)(object)val9);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val9.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val9.Convert(), applyTransform: false));
				}
			}
			On3dVector val11 = val8 + (IOn3dVector)(object)val10;
			val11.Unitize();
			if (flag)
			{
				On3dVector val12 = new On3dVector(circleLeft.Center());
				On3dVector val13 = new On3dVector(circleRight.Center());
				Brep geometry = MakeBar((val12 - (IOn3dVector)(object)val13).Length(), circleLeft.radius, circleRight.radius, height3, Math.Min(circleLeft.radius, circleRight.radius) * value5, value6);
				On3dPoint value20 = new On3dPoint((IOn3dVector)(object)((val12 + (IOn3dVector)(object)val13) / 2.0));
				On3dVector value21 = val13 - (IOn3dVector)(object)val12;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value20), CSConvert.On3dVectorCopyToVector3d(value21), CSConvert.On3dVectorCopyToVector3d(val11));
				concurrentBag.Add(new CSOnGeometryData(geometry, transform));
			}
			if (val5 == OnUtil.On_UNSET_VECTOR)
			{
				val5 = val11;
			}
			val6 = val11;
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
				double num9 = channelGemData6.GemXSize * value12 + value7 + value8;
				double height5 = ((_prongBase == ValueListItem.FollowCulet) ? (channelGemData6.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, num9, height5, value5, value6, value, out var prong4, out var prongBaseRail4))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint obj = new On3dPoint((IOn3dPoint)(object)channelGemData6.Center.Convert()) - (IOn3dVector)new On3dVector((IOn3dVector)(object)channelGemData6.ForwardVectorInGemPlane.Convert());
				double num10 = (channelGemData6.GemXSize * value12 + value7) / 2.0;
				double num11 = value9 - channelGemData6.GemZMin;
				On3dVector val20;
				if (val5 == OnUtil.On_UNSET_VECTOR)
				{
					val20 = new On3dVector((IOn3dVector)(object)channelGemData6.Normal.Convert());
					val20.Unitize();
				}
				else
				{
					OnXform val21 = new OnXform();
					val21.Mirror((IOn3dPoint)(object)channelGemData6.Center.Convert(), (IOn3dVector)(object)channelGemData6.ForwardVectorInGemPlane.Convert());
					val20 = new On3dVector((IOn3dVector)(object)val5);
					val20.Transform((IOnXform)(object)val21);
					val20.Unitize();
				}
				On3dVector val22 = new On3dVector((IOn3dVector)(object)channelGemData6.LeftVector.Convert());
				val22.Unitize();
				On3dVector val23 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val20, (IOn3dVector)(object)val22);
				val23.Unitize();
				OnPlane val24 = new OnPlane((IOn3dPoint)(object)(obj + (IOn3dVector)(object)(val23 * num10) + (IOn3dVector)(object)(val20 * num11)), (IOn3dVector)(object)val22, (IOn3dVector)(object)val23);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				{
					OnXform val40 = new OnXform();
					val40.ChangeBasis((IOnPlane)(object)val24, OnUtil.On_xy_plane);
					OnXform val41 = new OnXform();
					val41.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13), (IOn3dVector)(object)channelGemData6.Normal.Convert(), (IOn3dPoint)(object)channelGemData6.Center.Convert());
					OnXform val42 = val41 * (IOnXform)(object)val40;
					prong4.Transform((IOnXform)(object)val42);
					prongBaseRail4.Transform((IOnXform)(object)val42);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val42.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail4).Convert(), val42.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num12 = value14 / 2.0;
					OnXform val25 = new OnXform();
					val25.ChangeBasis((IOnPlane)(object)val24, OnUtil.On_xy_plane);
					OnXform val26 = new OnXform();
					val26.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num12), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val27 = new OnXform();
					val27.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 + num12), (IOn3dVector)(object)channelGemData6.Normal.Convert(), (IOn3dPoint)(object)channelGemData6.Center.Convert());
					OnXform val28 = val27 * (IOnXform)(object)val25 * (IOnXform)(object)val26;
					if (!flag)
					{
						OnGeometry val29 = prong4.Duplicate();
						val29.Transform((IOnXform)(object)val28);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val29).Convert(), val28.Convert(), applyTransform: false));
						OnGeometry val30 = prongBaseRail4.Duplicate();
						val30.Transform((IOnXform)(object)val28);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val30).Convert(), val28.Convert(), applyTransform: false));
					}
					OnXform val31 = new OnXform();
					val31.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num12), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val32 = new OnXform();
					val32.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 - num12), (IOn3dVector)(object)channelGemData6.Normal.Convert(), (IOn3dPoint)(object)channelGemData6.Center.Convert());
					OnXform val33 = val32 * (IOnXform)(object)val25 * (IOnXform)(object)val31;
					if (!flag)
					{
						OnGeometry val34 = prong4.Duplicate();
						val34.Transform((IOnXform)(object)val33);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val34).Convert(), val33.Convert(), applyTransform: false));
						OnGeometry val35 = prongBaseRail4.Duplicate();
						val35.Transform((IOnXform)(object)val33);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val35).Convert(), val33.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val36 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val37 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val38 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val39 = new On3dVector(0.0, 0.0, 1.0);
						val36.Transform((IOnXform)(object)val28);
						val38.Transform((IOnXform)(object)val28);
						val37.Transform((IOnXform)(object)val33);
						val39.Transform((IOnXform)(object)val33);
						Brep geometry3 = MakeBar((val36 - (IOn3dPoint)(object)val37).Length(), num9 / 2.0, num9 / 2.0, height5, num9 / 2.0 * value5, value6);
						On3dPoint value26 = new On3dPoint((IOn3dPoint)(object)((val36 + (IOn3dPoint)(object)val37) / 2.0));
						On3dVector value27 = val37 - (IOn3dPoint)(object)val36;
						On3dVector value28 = val38 + (IOn3dVector)(object)val39;
						Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value26), CSConvert.On3dVectorCopyToVector3d(value27), CSConvert.On3dVectorCopyToVector3d(value28));
						concurrentBag.Add(new CSOnGeometryData(geometry3, transform6));
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
					double num8 = 4.0 * height4 / 5.0;
					Point3d point3d2 = CSConvert.On3dPointCopyToPoint3d(circleLeft2.Center());
					if (channelGemData5.ZMaxPoint.DistanceTo(Point3d.Origin) > point3d2.DistanceTo(Point3d.Origin) + num8)
					{
						height4 += channelGemData5.ZMaxPoint.DistanceTo(Point3d.Origin) - (point3d2.DistanceTo(Point3d.Origin) + num8);
					}
				}
				OnXform val14 = new OnXform();
				val14.ChangeBasis((IOnPlane)(object)circleLeft2.plane, OnUtil.On_xy_plane);
				On3dVector val15 = new On3dVector(OnUtil.On_zaxis);
				val15.Transform((IOnXform)(object)val14);
				val15.Unitize();
				OnXform val16 = new OnXform();
				val16.ChangeBasis((IOnPlane)(object)circleRight2.plane, OnUtil.On_xy_plane);
				On3dVector val17 = new On3dVector(OnUtil.On_zaxis);
				val17.Transform((IOnXform)(object)val16);
				val17.Unitize();
				On3dVector value22 = val15 + (IOn3dVector)(object)val17;
				On3dVector val18 = new On3dVector(circleLeft2.Center());
				On3dVector val19 = new On3dVector(circleRight2.Center());
				if (!flag)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft2.Diameter(), height4, value5, value6, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value23 = val19 - (IOn3dVector)(object)val18;
					Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val18)), CSConvert.On3dVectorCopyToVector3d(value23), CSConvert.On3dVectorCopyToVector3d(value22));
					Vector3d normalToMirrorPlane = channelGemData4.Center - channelGemData4.XMaxPoint;
					Transform transform3 = Transform.Mirror(channelGemData4.Center, normalToMirrorPlane);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), transform3 * transform2));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight2.Diameter(), height4, value5, value6, value, out prong3, out prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					value23 = val18 - (IOn3dVector)(object)val19;
					transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val19)), CSConvert.On3dVectorCopyToVector3d(value23), CSConvert.On3dVectorCopyToVector3d(value22));
					normalToMirrorPlane = channelGemData4.Center - channelGemData4.XMaxPoint;
					transform3 = Transform.Mirror(channelGemData4.Center, normalToMirrorPlane);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), transform3 * transform2));
				}
				else
				{
					Brep geometry2 = MakeBar((val18 - (IOn3dVector)(object)val19).Length(), circleLeft2.radius, circleRight2.radius, height4, Math.Min(circleLeft2.radius, circleRight2.radius) * value5, value6);
					On3dPoint value24 = new On3dPoint((IOn3dVector)(object)((val18 + (IOn3dVector)(object)val19) / 2.0));
					On3dVector value25 = val19 - (IOn3dVector)(object)val18;
					Transform transform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value24), CSConvert.On3dVectorCopyToVector3d(value25), CSConvert.On3dVectorCopyToVector3d(value22));
					Vector3d normalToMirrorPlane2 = channelGemData4.Center - channelGemData4.XMaxPoint;
					Transform transform5 = Transform.Mirror(channelGemData4.Center, normalToMirrorPlane2);
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
				CSChannelGemData channelGemData9 = CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.GetArrayCount() - 1);
				double num14 = channelGemData9.GemXSize * value15 + value7 + value8;
				double height7 = ((_prongBase == ValueListItem.FollowCulet) ? (channelGemData9.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, num14, height7, value5, value6, value, out var prong6, out var prongBaseRail6))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint obj2 = new On3dPoint((IOn3dPoint)(object)channelGemData9.Center.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)channelGemData9.ForwardVectorInGemPlane.Convert());
				double num15 = (channelGemData9.GemXSize * value15 + value7) / 2.0;
				double num16 = value9 - channelGemData9.GemZMin;
				On3dVector val49;
				if (val6 == OnUtil.On_UNSET_VECTOR)
				{
					val49 = new On3dVector((IOn3dVector)(object)channelGemData9.Normal.Convert());
					val49.Unitize();
				}
				else
				{
					OnXform val50 = new OnXform();
					val50.Mirror((IOn3dPoint)(object)channelGemData9.Center.Convert(), (IOn3dVector)(object)channelGemData9.ForwardVectorInGemPlane.Convert());
					val49 = new On3dVector((IOn3dVector)(object)val6);
					val49.Transform((IOnXform)(object)val50);
					val49.Unitize();
				}
				On3dVector val51 = new On3dVector((IOn3dVector)(object)channelGemData9.RightVector.Convert());
				val51.Unitize();
				On3dVector val52 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val49, (IOn3dVector)(object)val51);
				val52.Unitize();
				OnPlane val53 = new OnPlane((IOn3dPoint)(object)(obj2 + (IOn3dVector)(object)(val52 * num15) + (IOn3dVector)(object)(val49 * num16)), (IOn3dVector)(object)val51, (IOn3dVector)(object)val52);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd2:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd8:
				{
					OnXform val69 = new OnXform();
					val69.ChangeBasis((IOnPlane)(object)val53, OnUtil.On_xy_plane);
					OnXform val70 = new OnXform();
					val70.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16), (IOn3dVector)(object)channelGemData9.Normal.Convert(), (IOn3dPoint)(object)channelGemData9.Center.Convert());
					OnXform val71 = val70 * (IOnXform)(object)val69;
					prong6.Transform((IOnXform)(object)val71);
					prongBaseRail6.Transform((IOnXform)(object)val71);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val71.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail6).Convert(), val71.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd3:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num17 = value17 / 2.0;
					OnXform val54 = new OnXform();
					val54.ChangeBasis((IOnPlane)(object)val53, OnUtil.On_xy_plane);
					OnXform val55 = new OnXform();
					val55.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num17), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val56 = new OnXform();
					val56.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 + num17), (IOn3dVector)(object)channelGemData9.Normal.Convert(), (IOn3dPoint)(object)channelGemData9.Center.Convert());
					OnXform val57 = val56 * (IOnXform)(object)val54 * (IOnXform)(object)val55;
					if (!flag)
					{
						OnGeometry val58 = prong6.Duplicate();
						val58.Transform((IOnXform)(object)val57);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val58).Convert(), val57.Convert(), applyTransform: false));
						OnGeometry val59 = prongBaseRail6.Duplicate();
						val59.Transform((IOnXform)(object)val57);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val59).Convert(), val57.Convert(), applyTransform: false));
					}
					OnXform val60 = new OnXform();
					val60.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num17), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val61 = new OnXform();
					val61.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 - num17), (IOn3dVector)(object)channelGemData9.Normal.Convert(), (IOn3dPoint)(object)channelGemData9.Center.Convert());
					OnXform val62 = val61 * (IOnXform)(object)val54 * (IOnXform)(object)val60;
					if (!flag)
					{
						OnGeometry val63 = prong6.Duplicate();
						val63.Transform((IOnXform)(object)val62);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val63).Convert(), val62.Convert(), applyTransform: false));
						OnGeometry val64 = prongBaseRail6.Duplicate();
						val64.Transform((IOnXform)(object)val62);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val64).Convert(), val62.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val65 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val66 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val67 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val68 = new On3dVector(0.0, 0.0, 1.0);
						val65.Transform((IOnXform)(object)val57);
						val67.Transform((IOnXform)(object)val57);
						val66.Transform((IOnXform)(object)val62);
						val68.Transform((IOnXform)(object)val62);
						Brep geometry5 = MakeBar((val65 - (IOn3dPoint)(object)val66).Length(), num14 / 2.0, num14 / 2.0, height7, num14 / 2.0 * value5, value6);
						On3dPoint value33 = new On3dPoint((IOn3dPoint)(object)((val65 + (IOn3dPoint)(object)val66) / 2.0));
						On3dVector value34 = val66 - (IOn3dPoint)(object)val65;
						On3dVector value35 = val67 + (IOn3dVector)(object)val68;
						Transform transform11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value33), CSConvert.On3dVectorCopyToVector3d(value34), CSConvert.On3dVectorCopyToVector3d(value35));
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
					double num13 = 4.0 * height6 / 5.0;
					Point3d point3d3 = CSConvert.On3dPointCopyToPoint3d(circleLeft3.Center());
					if (channelGemData8.ZMaxPoint.DistanceTo(Point3d.Origin) > point3d3.DistanceTo(Point3d.Origin) + num13)
					{
						height6 += channelGemData8.ZMaxPoint.DistanceTo(Point3d.Origin) - (point3d3.DistanceTo(Point3d.Origin) + num13);
					}
				}
				OnXform val43 = new OnXform();
				val43.ChangeBasis((IOnPlane)(object)circleLeft3.plane, OnUtil.On_xy_plane);
				On3dVector val44 = new On3dVector(OnUtil.On_zaxis);
				val44.Transform((IOnXform)(object)val43);
				val44.Unitize();
				OnXform val45 = new OnXform();
				val45.ChangeBasis((IOnPlane)(object)circleRight3.plane, OnUtil.On_xy_plane);
				On3dVector val46 = new On3dVector(OnUtil.On_zaxis);
				val46.Transform((IOnXform)(object)val45);
				val46.Unitize();
				On3dVector value29 = val44 + (IOn3dVector)(object)val46;
				On3dVector val47 = new On3dVector(circleLeft3.Center());
				On3dVector val48 = new On3dVector(circleRight3.Center());
				if (!flag)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft3.Diameter(), height6, value5, value6, value, out var prong5, out var prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value30 = val48 - (IOn3dVector)(object)val47;
					Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val47)), CSConvert.On3dVectorCopyToVector3d(value30), CSConvert.On3dVectorCopyToVector3d(value29));
					Vector3d normalToMirrorPlane3 = channelGemData8.Center - channelGemData8.XMaxPoint;
					Transform transform8 = Transform.Mirror(channelGemData8.Center, normalToMirrorPlane3);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), transform8 * transform7));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight3.Diameter(), height6, value5, value6, value, out prong5, out prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					value30 = val47 - (IOn3dVector)(object)val48;
					transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val48)), CSConvert.On3dVectorCopyToVector3d(value30), CSConvert.On3dVectorCopyToVector3d(value29));
					normalToMirrorPlane3 = channelGemData8.Center - channelGemData8.XMaxPoint;
					transform8 = Transform.Mirror(channelGemData8.Center, normalToMirrorPlane3);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), transform8 * transform7));
				}
				else
				{
					Brep geometry4 = MakeBar((val47 - (IOn3dVector)(object)val48).Length(), circleLeft3.radius, circleRight3.radius, height6, Math.Min(circleLeft3.radius, circleRight3.radius) * value5, value6);
					On3dPoint value31 = new On3dPoint((IOn3dVector)(object)((val47 + (IOn3dVector)(object)val48) / 2.0));
					On3dVector value32 = val48 - (IOn3dVector)(object)val47;
					Transform transform9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value31), CSConvert.On3dVectorCopyToVector3d(value32), CSConvert.On3dVectorCopyToVector3d(value29));
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

	private static NurbsCurve BuildProngProfile(double radius, double height, double filletPer, double taperPer)
	{
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
		NurbsCurve nurbsCurve = new NurbsCurve(3, rational: true, 3, 9);
		nurbsCurve.Points[0] = new ControlPoint(0.0, 0.0, 0.0, 1.0);
		nurbsCurve.Points[1] = new ControlPoint(num / 2.0, 0.0, 0.0, 1.0);
		nurbsCurve.Points[2] = new ControlPoint(num, 0.0, 0.0, 1.0);
		nurbsCurve.Points[3] = new ControlPoint((num + radius) / 2.0, 0.0, num3 / 2.0, 1.0);
		nurbsCurve.Points[4] = new ControlPoint(radius, 0.0, num3, 1.0);
		double x = num + (radius - num) / num3 * height;
		double z = height;
		nurbsCurve.Points[5] = new ControlPoint(x, 0.0, z, 0.7071067811);
		nurbsCurve.Points[6] = new ControlPoint(radius - num2, 0.0, height, 1.0);
		nurbsCurve.Points[7] = new ControlPoint((radius - num2) / 2.0, 0.0, height, 1.0);
		nurbsCurve.Points[8] = new ControlPoint(0.0, 0.0, height, 1.0);
		nurbsCurve.Knots[0] = 0.0;
		nurbsCurve.Knots[1] = 0.0;
		nurbsCurve.Knots[2] = 0.25;
		nurbsCurve.Knots[3] = 0.25;
		nurbsCurve.Knots[4] = 0.5;
		nurbsCurve.Knots[5] = 0.5;
		nurbsCurve.Knots[6] = 0.75;
		nurbsCurve.Knots[7] = 0.75;
		nurbsCurve.Knots[8] = 1.0;
		nurbsCurve.Knots[9] = 1.0;
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

	private static Brep MakeBar(double centerLength, double radius1, double radius2, double height, double domeRadius, double filletPer)
	{
		Point3d point3d = new Point3d((0.0 - centerLength) / 2.0, 0.0, 0.0);
		Point3d point3d2 = new Point3d(centerLength / 2.0, 0.0, 0.0);
		double angleRadians = Math.Asin((radius1 - radius2) / centerLength);
		Point3d point3d3 = new Point3d((0.0 - centerLength) / 2.0, radius1, 0.0);
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(angleRadians, -Vector3d.ZAxis, point3d);
		point3d3.Transform(xform);
		Point3d point3d4 = new Point3d(centerLength / 2.0, radius2, 0.0);
		Transform xform2 = CSBlockUtil.CSUtilMath.Rotation(angleRadians, -Vector3d.ZAxis, point3d2);
		point3d4.Transform(xform2);
		Point3d point3d5 = new Point3d(point3d3);
		point3d5.Y = 0.0 - point3d5.Y;
		Point3d point3d6 = new Point3d(point3d4);
		point3d6.Y = 0.0 - point3d6.Y;
		Point3d pointOnInterior = new Point3d((0.0 - centerLength) / 2.0 - radius1, 0.0, 0.0);
		Point3d pointOnInterior2 = new Point3d(centerLength / 2.0 + radius2, 0.0, 0.0);
		ArcCurve arcCurve = new ArcCurve(new Arc(point3d3, pointOnInterior, point3d5));
		ArcCurve arcCurve2 = new ArcCurve(new Arc(point3d4, pointOnInterior2, point3d6));
		NurbsCurve nurbsCurve = BuildProngProfile(radius1, height, domeRadius / radius1, filletPer);
		NurbsCurve nurbsCurve2 = BuildProngProfile(radius2, height, domeRadius / radius2, filletPer);
		NurbsSurface nurbsSurface = RailRevolve(nurbsCurve, arcCurve.ToNurbsCurve(), point3d, radius1);
		NurbsSurface nurbsSurface2 = RailRevolve(nurbsCurve2, arcCurve2.ToNurbsCurve(), point3d2, radius2);
		Transform xform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, point3d, point3d3 - point3d, Vector3d.ZAxis);
		Transform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, point3d, point3d5 - point3d, Vector3d.ZAxis);
		Transform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, point3d2, point3d4 - point3d2, Vector3d.ZAxis);
		Transform xform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, point3d2, point3d6 - point3d2, Vector3d.ZAxis);
		Curve curve = nurbsCurve.DuplicateCurve();
		curve.Transform(xform3);
		Curve curve2 = nurbsCurve.DuplicateCurve();
		curve2.Transform(xform4);
		Curve curve3 = nurbsCurve2.DuplicateCurve();
		curve3.Transform(xform5);
		Curve curve4 = nurbsCurve2.DuplicateCurve();
		curve4.Transform(xform6);
		NurbsSurface nurbsSurface3 = NurbsSurface.CreateRuledSurface(curve, curve3);
		NurbsSurface nurbsSurface4 = NurbsSurface.CreateRuledSurface(curve2, curve4);
		Brep[] array = Brep.JoinBreps(new List<Brep>
		{
			nurbsSurface.ToBrep(),
			nurbsSurface2.ToBrep(),
			nurbsSurface3.ToBrep(),
			nurbsSurface4.ToBrep()
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
		NurbsSurface nurbsSurface = NurbsSurface.Create(3, isRational: true, profile.Order, rail.Order, profile.Points.Count, rail.Points.Count);
		int count = rail.Points.Count;
		int count2 = profile.Points.Count;
		for (int i = 0; i < count2; i++)
		{
			Point3d location = profile.Points[i].Location;
			Point3d point3d = new Point3d(location);
			point3d.Z = 0.0;
			Point3d point3d2 = point3d;
			double num = point3d2.DistanceTo(Point3d.Origin);
			for (int j = 0; j < count; j++)
			{
				Vector3d vector3d = rail.Points[j].Location - center;
				Point3d point3d3 = center + vector3d * num / s1;
				point3d3.Z = location.Z;
				double weight = profile.Points[i].Weight * rail.Points[j].Weight;
				nurbsSurface.Points.SetControlPoint(i, j, new ControlPoint(point3d3.X, point3d3.Y, point3d3.Z, weight));
			}
		}
		for (int k = 0; k < profile.Knots.Count; k++)
		{
			nurbsSurface.KnotsU[k] = profile.Knots[k];
		}
		for (int l = 0; l < rail.Knots.Count; l++)
		{
			nurbsSurface.KnotsV[l] = rail.Knots[l];
		}
		return nurbsSurface;
	}
}
