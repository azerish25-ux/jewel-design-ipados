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
		//IL_0b3f: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Expected O, but got Unknown
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Expected O, but got Unknown
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfd: Expected O, but got Unknown
		//IL_0c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3f: Expected O, but got Unknown
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Expected O, but got Unknown
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c7: Expected O, but got Unknown
		//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6d: Expected O, but got Unknown
		//IL_0c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cae: Expected O, but got Unknown
		//IL_0d0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d14: Expected O, but got Unknown
		//IL_0d23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2a: Expected O, but got Unknown
		//IL_0d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d82: Expected O, but got Unknown
		//IL_0deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df2: Expected O, but got Unknown
		//IL_0e15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4a: Expected O, but got Unknown
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Expected O, but got Unknown
		//IL_099d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Expected O, but got Unknown
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f2: Expected O, but got Unknown
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
		//IL_154a: Unknown result type (might be due to invalid IL or missing references)
		//IL_155b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1565: Expected O, but got Unknown
		//IL_1565: Expected O, but got Unknown
		//IL_108c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1093: Expected O, but got Unknown
		//IL_109a: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a1: Expected O, but got Unknown
		//IL_15b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bc: Expected O, but got Unknown
		//IL_15dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e4: Expected O, but got Unknown
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
		//IL_1601: Unknown result type (might be due to invalid IL or missing references)
		//IL_1608: Expected O, but got Unknown
		//IL_1643: Unknown result type (might be due to invalid IL or missing references)
		//IL_164a: Expected O, but got Unknown
		//IL_117e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1185: Expected O, but got Unknown
		//IL_1679: Unknown result type (might be due to invalid IL or missing references)
		//IL_1680: Expected O, but got Unknown
		//IL_168f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1694: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c1: Expected O, but got Unknown
		//IL_1720: Unknown result type (might be due to invalid IL or missing references)
		//IL_1727: Expected O, but got Unknown
		//IL_1736: Unknown result type (might be due to invalid IL or missing references)
		//IL_173d: Expected O, but got Unknown
		//IL_1760: Unknown result type (might be due to invalid IL or missing references)
		//IL_1765: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Expected O, but got Unknown
		//IL_13f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fa: Expected O, but got Unknown
		//IL_17fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1805: Expected O, but got Unknown
		//IL_1828: Unknown result type (might be due to invalid IL or missing references)
		//IL_182d: Unknown result type (might be due to invalid IL or missing references)
		//IL_185d: Expected O, but got Unknown
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
		//IL_1b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba6: Expected O, but got Unknown
		//IL_1e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e1b: Expected O, but got Unknown
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
				Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value24), CSConvert.On3dVectorCopyToVector3d(value25), destination);
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
			OnXform val11 = new OnXform();
			val11.ChangeBasis((IOnPlane)(object)circleLeft.plane, OnUtil.On_xy_plane);
			On3dVector val12 = new On3dVector(OnUtil.On_zaxis);
			val12.Transform((IOnXform)(object)val11);
			val12.Unitize();
			if (!flag)
			{
				double num5 = circleLeft.radius * 2.0;
				if (num5 > value2 && num5 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num5, height3, value5, value6, value, out var prong, out var prongBaseRail))
					{
						return UpdateError("Create prong cache failed");
					}
					prong.Transform((IOnXform)(object)val11);
					prongBaseRail.Transform((IOnXform)(object)val11);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val11.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val11.Convert(), applyTransform: false));
				}
			}
			OnXform val13 = new OnXform();
			val13.ChangeBasis((IOnPlane)(object)circleRight.plane, OnUtil.On_xy_plane);
			On3dVector val14 = new On3dVector(OnUtil.On_zaxis);
			val14.Transform((IOnXform)(object)val13);
			val14.Unitize();
			if (!flag)
			{
				double num6 = circleRight.radius * 2.0;
				if (num6 > value2 && num6 < value3)
				{
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, num6, height3, value5, value6, value, out var prong2, out var prongBaseRail2))
					{
						return UpdateError("Create prong cache failed");
					}
					prong2.Transform((IOnXform)(object)val13);
					prongBaseRail2.Transform((IOnXform)(object)val13);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val13.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val13.Convert(), applyTransform: false));
				}
			}
			On3dVector val15 = val12 + (IOn3dVector)(object)val14;
			val15.Unitize();
			if (flag)
			{
				val7 = new On3dVector(circleLeft.Center());
				val8 = new On3dVector(circleRight.Center());
				Brep geometry = MakeBar((val7 - (IOn3dVector)(object)val8).Length(), circleLeft.radius, circleRight.radius, height3, value6, value18, value19, value20, value21);
				value24 = new On3dPoint((IOn3dVector)(object)((val7 + (IOn3dVector)(object)val8) / 2.0));
				value25 = val8 - (IOn3dVector)(object)val7;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value24), CSConvert.On3dVectorCopyToVector3d(value25), CSConvert.On3dVectorCopyToVector3d(val15));
				concurrentBag.Add(new CSOnGeometryData(geometry, transform));
			}
			if (val5 == OnUtil.On_UNSET_VECTOR)
			{
				val5 = val15;
			}
			val6 = val15;
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
				On3dVector val22;
				if (val5 == OnUtil.On_UNSET_VECTOR)
				{
					val22 = new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert());
					val22.Unitize();
				}
				else
				{
					OnXform val23 = new OnXform();
					val23.Mirror((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.ForwardVectorInGemPlane.Convert());
					val22 = new On3dVector((IOn3dVector)(object)val5);
					val22.Transform((IOnXform)(object)val23);
					val22.Unitize();
				}
				On3dVector val24 = new On3dVector((IOn3dVector)(object)cSChannelGemData.LeftVector.Convert());
				val24.Unitize();
				On3dVector val25 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val22, (IOn3dVector)(object)val24);
				val25.Unitize();
				OnPlane val26 = new OnPlane((IOn3dPoint)(object)(obj + (IOn3dVector)(object)(val25 * num8) + (IOn3dVector)(object)(val22 * num9)), (IOn3dVector)(object)val24, (IOn3dVector)(object)val25);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				{
					OnXform val42 = new OnXform();
					val42.ChangeBasis((IOnPlane)(object)val26, OnUtil.On_xy_plane);
					OnXform val43 = new OnXform();
					val43.Rotation(value13 * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val44 = val43 * (IOnXform)(object)val42;
					prong4.Transform((IOnXform)(object)val44);
					prongBaseRail4.Transform((IOnXform)(object)val44);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val44.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail4).Convert(), val44.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num10 = value14 / 2.0;
					OnXform val27 = new OnXform();
					val27.ChangeBasis((IOnPlane)(object)val26, OnUtil.On_xy_plane);
					OnXform val28 = new OnXform();
					val28.Rotation((90.0 - num10) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val29 = new OnXform();
					val29.Rotation((value13 + num10) * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val30 = val29 * (IOnXform)(object)val27 * (IOnXform)(object)val28;
					if (!flag)
					{
						OnGeometry val31 = prong4.Duplicate();
						val31.Transform((IOnXform)(object)val30);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val31).Convert(), val30.Convert(), applyTransform: false));
						OnGeometry val32 = prongBaseRail4.Duplicate();
						val32.Transform((IOnXform)(object)val30);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val32).Convert(), val30.Convert(), applyTransform: false));
					}
					OnXform val33 = new OnXform();
					val33.Rotation((-90.0 + num10) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val34 = new OnXform();
					val34.Rotation((value13 - num10) * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData.Center.Convert());
					OnXform val35 = val34 * (IOnXform)(object)val27 * (IOnXform)(object)val33;
					if (!flag)
					{
						OnGeometry val36 = prong4.Duplicate();
						val36.Transform((IOnXform)(object)val35);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val36).Convert(), val35.Convert(), applyTransform: false));
						OnGeometry val37 = prongBaseRail4.Duplicate();
						val37.Transform((IOnXform)(object)val35);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val37).Convert(), val35.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val38 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val39 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val40 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val41 = new On3dVector(0.0, 0.0, 1.0);
						val38.Transform((IOnXform)(object)val30);
						val40.Transform((IOnXform)(object)val30);
						val39.Transform((IOnXform)(object)val35);
						val41.Transform((IOnXform)(object)val35);
						Brep geometry3 = MakeBar((val38 - (IOn3dPoint)(object)val39).Length(), num7 / 2.0, num7 / 2.0, height5, value6, value18, value19, value20, value21);
						On3dPoint value32 = new On3dPoint((IOn3dPoint)(object)((val38 + (IOn3dPoint)(object)val39) / 2.0));
						On3dVector value33 = val39 - (IOn3dPoint)(object)val38;
						On3dVector value34 = val40 + (IOn3dVector)(object)val41;
						Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value32), CSConvert.On3dVectorCopyToVector3d(value33), CSConvert.On3dVectorCopyToVector3d(value34));
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
				string circles2 = GetCircles(channelGemData4, channelGemData5, channelGemData, (IOnCurve)(object)val3, (IOnCurve)(object)val4, value7, value8, value4, value9, value22, value23, out circleLeft2, out circleRight2, out height4);
				if (!string.IsNullOrEmpty(circles2))
				{
					return UpdateError(circles2);
				}
				On3dVector val16 = new On3dVector(circleLeft2.Center());
				On3dVector val17 = new On3dVector(circleRight2.Center());
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height4 = (value22 ? (num3 * value23) : num3);
				}
				OnXform val18 = new OnXform();
				val18.ChangeBasis((IOnPlane)(object)circleLeft2.plane, OnUtil.On_xy_plane);
				On3dVector val19 = new On3dVector(OnUtil.On_zaxis);
				val19.Transform((IOnXform)(object)val18);
				val19.Unitize();
				OnXform val20 = new OnXform();
				val20.ChangeBasis((IOnPlane)(object)circleRight2.plane, OnUtil.On_xy_plane);
				On3dVector val21 = new On3dVector(OnUtil.On_zaxis);
				val21.Transform((IOnXform)(object)val20);
				val21.Unitize();
				On3dVector value26 = val19 + (IOn3dVector)(object)val21;
				if (CSApplicationHelper.Application.IsCamMode)
				{
					Circle circle2 = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData5.ZMaxPoint.DistanceTo(Point3d.Origin));
					On3dPoint value27 = new On3dPoint((IOn3dVector)(object)((val16 + (IOn3dVector)(object)val17) / 2.0));
					On3dVector value28 = val17 - (IOn3dVector)(object)val16;
					Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value27), CSConvert.On3dVectorCopyToVector3d(value28), CSConvert.On3dVectorCopyToVector3d(value26));
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
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft2.Diameter(), height4, value5, value6, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value29 = val17 - (IOn3dVector)(object)val16;
					Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val16)), CSConvert.On3dVectorCopyToVector3d(value29), CSConvert.On3dVectorCopyToVector3d(value26));
					Vector3d normalToMirrorPlane = channelGemData4.Center - channelGemData4.XMaxPoint;
					Transform transform3 = Transform.Mirror(channelGemData4.Center, normalToMirrorPlane);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), transform3 * transform2));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight2.Diameter(), height4, value5, value6, value, out prong3, out prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					value29 = val16 - (IOn3dVector)(object)val17;
					transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val17)), CSConvert.On3dVectorCopyToVector3d(value29), CSConvert.On3dVectorCopyToVector3d(value26));
					normalToMirrorPlane = channelGemData4.Center - channelGemData4.XMaxPoint;
					transform3 = Transform.Mirror(channelGemData4.Center, normalToMirrorPlane);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), transform3 * transform2));
				}
				else
				{
					Brep geometry2 = MakeBar((val16 - (IOn3dVector)(object)val17).Length(), circleLeft2.radius, circleRight2.radius, height4, value6, value18, value19, value20, value21);
					On3dPoint value30 = new On3dPoint((IOn3dVector)(object)((val16 + (IOn3dVector)(object)val17) / 2.0));
					On3dVector value31 = val17 - (IOn3dVector)(object)val16;
					Transform transform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value30), CSConvert.On3dVectorCopyToVector3d(value31), CSConvert.On3dVectorCopyToVector3d(value26));
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
				On3dVector val51;
				if (val6 == OnUtil.On_UNSET_VECTOR)
				{
					val51 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert());
					val51.Unitize();
				}
				else
				{
					OnXform val52 = new OnXform();
					val52.Mirror((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					val51 = new On3dVector((IOn3dVector)(object)val6);
					val51.Transform((IOnXform)(object)val52);
					val51.Unitize();
				}
				On3dVector val53 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.RightVector.Convert());
				val53.Unitize();
				On3dVector val54 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val51, (IOn3dVector)(object)val53);
				val54.Unitize();
				OnPlane val55 = new OnPlane((IOn3dPoint)(object)(obj2 + (IOn3dVector)(object)(val54 * num12) + (IOn3dVector)(object)(val51 * num13)), (IOn3dVector)(object)val53, (IOn3dVector)(object)val54);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd2:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd8:
				{
					OnXform val71 = new OnXform();
					val71.ChangeBasis((IOnPlane)(object)val55, OnUtil.On_xy_plane);
					OnXform val72 = new OnXform();
					val72.Rotation(value16 * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val73 = val72 * (IOnXform)(object)val71;
					prong6.Transform((IOnXform)(object)val73);
					prongBaseRail6.Transform((IOnXform)(object)val73);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong6).Convert(), val73.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail6).Convert(), val73.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd3:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num14 = value17 / 2.0;
					OnXform val56 = new OnXform();
					val56.ChangeBasis((IOnPlane)(object)val55, OnUtil.On_xy_plane);
					OnXform val57 = new OnXform();
					val57.Rotation((90.0 + num14) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val58 = new OnXform();
					val58.Rotation((value16 + num14) * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val59 = val58 * (IOnXform)(object)val56 * (IOnXform)(object)val57;
					if (!flag)
					{
						OnGeometry val60 = prong6.Duplicate();
						val60.Transform((IOnXform)(object)val59);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val60).Convert(), val59.Convert(), applyTransform: false));
						OnGeometry val61 = prongBaseRail6.Duplicate();
						val61.Transform((IOnXform)(object)val59);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val61).Convert(), val59.Convert(), applyTransform: false));
					}
					OnXform val62 = new OnXform();
					val62.Rotation((-90.0 + num14) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform val63 = new OnXform();
					val63.Rotation((value16 - num14) * OnUtil.On_DEGREES_TO_RADIANS, (IOn3dVector)(object)cSChannelGemData2.Normal.Convert(), (IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
					OnXform val64 = val63 * (IOnXform)(object)val56 * (IOnXform)(object)val62;
					if (!flag)
					{
						OnGeometry val65 = prong6.Duplicate();
						val65.Transform((IOnXform)(object)val64);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)val65).Convert(), val64.Convert(), applyTransform: false));
						OnGeometry val66 = prongBaseRail6.Duplicate();
						val66.Transform((IOnXform)(object)val64);
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)val66).Convert(), val64.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint val67 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint val68 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector val69 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector val70 = new On3dVector(0.0, 0.0, 1.0);
						val67.Transform((IOnXform)(object)val59);
						val69.Transform((IOnXform)(object)val59);
						val68.Transform((IOnXform)(object)val64);
						val70.Transform((IOnXform)(object)val64);
						Brep geometry5 = MakeBar((val67 - (IOn3dPoint)(object)val68).Length(), num11 / 2.0, num11 / 2.0, height7, value6, value18, value19, value20, value21);
						On3dPoint value41 = new On3dPoint((IOn3dPoint)(object)((val67 + (IOn3dPoint)(object)val68) / 2.0));
						On3dVector value42 = val68 - (IOn3dPoint)(object)val67;
						On3dVector value43 = val69 + (IOn3dVector)(object)val70;
						Transform transform11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value41), CSConvert.On3dVectorCopyToVector3d(value42), CSConvert.On3dVectorCopyToVector3d(value43));
						concurrentBag.Add(new CSOnGeometryData(geometry5, transform11));
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
				On3dVector val45 = new On3dVector(circleLeft3.Center());
				On3dVector val46 = new On3dVector(circleRight3.Center());
				OnXform val47 = new OnXform();
				val47.ChangeBasis((IOnPlane)(object)circleLeft3.plane, OnUtil.On_xy_plane);
				On3dVector val48 = new On3dVector(OnUtil.On_zaxis);
				val48.Transform((IOnXform)(object)val47);
				val48.Unitize();
				OnXform val49 = new OnXform();
				val49.ChangeBasis((IOnPlane)(object)circleRight3.plane, OnUtil.On_xy_plane);
				On3dVector val50 = new On3dVector(OnUtil.On_zaxis);
				val50.Transform((IOnXform)(object)val49);
				val50.Unitize();
				On3dVector value35 = val48 + (IOn3dVector)(object)val50;
				if (CSApplicationHelper.Application.IsCamMode)
				{
					Circle circle3 = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData7.ZMaxPoint.DistanceTo(Point3d.Origin));
					On3dPoint value36 = new On3dPoint((IOn3dVector)(object)((val45 + (IOn3dVector)(object)val46) / 2.0));
					On3dVector value37 = val46 - (IOn3dVector)(object)val45;
					Transform xform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value36), CSConvert.On3dVectorCopyToVector3d(value37), CSConvert.On3dVectorCopyToVector3d(value35));
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
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleLeft3.Diameter(), height6, value5, value6, value, out var prong5, out var prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value38 = val46 - (IOn3dVector)(object)val45;
					Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val45)), CSConvert.On3dVectorCopyToVector3d(value38), CSConvert.On3dVectorCopyToVector3d(value35));
					Vector3d normalToMirrorPlane3 = channelGemData7.Center - channelGemData7.XMaxPoint;
					Transform transform8 = Transform.Mirror(channelGemData7.Center, normalToMirrorPlane3);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), transform8 * transform7));
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, circleRight3.Diameter(), height6, value5, value6, value, out prong5, out prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					value38 = val45 - (IOn3dVector)(object)val46;
					transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(On3dPoint.op_Implicit((IOn3dVector)(object)val46)), CSConvert.On3dVectorCopyToVector3d(value38), CSConvert.On3dVectorCopyToVector3d(value35));
					normalToMirrorPlane3 = channelGemData7.Center - channelGemData7.XMaxPoint;
					transform8 = Transform.Mirror(channelGemData7.Center, normalToMirrorPlane3);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong5).Convert(), transform8 * transform7));
				}
				else
				{
					Brep geometry4 = MakeBar((val45 - (IOn3dVector)(object)val46).Length(), circleLeft3.radius, circleRight3.radius, height6, value6, value18, value19, value20, value21);
					On3dPoint value39 = new On3dPoint((IOn3dVector)(object)((val45 + (IOn3dVector)(object)val46) / 2.0));
					On3dVector value40 = val46 - (IOn3dVector)(object)val45;
					Transform transform9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value39), CSConvert.On3dVectorCopyToVector3d(value40), CSConvert.On3dVectorCopyToVector3d(value35));
					Vector3d normalToMirrorPlane4 = channelGemData7.Center - channelGemData7.XMaxPoint;
					Transform transform10 = Transform.Mirror(channelGemData7.Center, normalToMirrorPlane4);
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
		Transform xform = Transform.Rotation(num, -Vector3d.ZAxis, point3d);
		point3d3.Transform(xform);
		Point3d point3d4 = new Point3d(centerLength / 2.0, radius2, 0.0);
		Transform xform2 = Transform.Rotation(num, -Vector3d.ZAxis, point3d2);
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
