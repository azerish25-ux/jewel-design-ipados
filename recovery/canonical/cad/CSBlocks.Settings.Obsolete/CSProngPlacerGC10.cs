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
		OnNurbsCurve nurbs_curve = InProfile.GetNurbsCurve();
		if (nurbs_curve == null)
		{
			OnCircle onCircle = new OnCircle(OnUtil.On_xy_plane, OnUtil.On_origin, 1.0);
			nurbs_curve = new OnNurbsCurve();
			onCircle.GetNurbForm(ref nurbs_curve);
			if (nurbs_curve == null)
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
		OnCurve crv = CSUtilGemRails.Left(cSDataArray, rail).DuplicateCurve();
		if (!crv.IsClosed())
		{
			double length = -1.23432101234321E+308;
			if (!crv.GetLength(ref length))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref crv, IRhinoExtend.Type.Smooth, 0, length * 0.5);
			RhUtil.RhinoExtendCurve(ref crv, IRhinoExtend.Type.Smooth, 1, length * 0.5);
		}
		OnCurve crv2 = CSUtilGemRails.Right(cSDataArray, rail).DuplicateCurve();
		if (!crv2.IsClosed())
		{
			double length2 = -1.23432101234321E+308;
			if (!crv2.GetLength(ref length2))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref crv2, IRhinoExtend.Type.Smooth, 0, length2 * 0.5);
			RhUtil.RhinoExtendCurve(ref crv2, IRhinoExtend.Type.Smooth, 1, length2 * 0.5);
		}
		On3dVector on3dVector = new On3dVector(OnUtil.On_UNSET_VECTOR);
		On3dVector on3dVector2 = new On3dVector(OnUtil.On_UNSET_VECTOR);
		_prongCache.Clear();
		CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, 0);
		double gemZSize = channelGemData.GemZSize;
		double num = 0.0;
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
				double num2 = cSDataArray.GetArrayCount();
				double vHeight;
				double vVerticalLocation;
				if (i < cSDataArray.GetArrayCount() / 2)
				{
					vHeight = value4 * ((num2 / 2.0 - (double)i) / (num2 / 2.0)) + value5 * ((double)i / (num2 / 2.0));
					vVerticalLocation = value21 * ((num2 / 2.0 - (double)i) / (num2 / 2.0)) + value22 * ((double)i / (num2 / 2.0));
				}
				else
				{
					vHeight = value5 * (num2 - (double)i) / (double)i + value6 * (1.0 - (num2 - (double)i) / (double)i);
					vVerticalLocation = value22 * (num2 - (double)i) / (double)i + value23 * (1.0 - (num2 - (double)i) / (double)i);
				}
				double height2;
				string height = GetHeight(CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1), CSUtilGemRails.GetChannelGemData(cSDataArray, i), channelGemData, crv, crv2, value13, value14, vHeight, vVerticalLocation, out height2);
				if (!string.IsNullOrEmpty(height))
				{
					return UpdateError(height);
				}
				if (height2 > num)
				{
					num = height2;
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
			int num3 = j - 1;
			int num4 = j + 1;
			if (num3 < 0)
			{
				if (!flag2)
				{
					continue;
				}
				num3 = cSDataArray.GetArrayCount() - 1;
			}
			if (num4 >= cSDataArray.GetArrayCount())
			{
				num4 = ((!flag2) ? j : 0);
			}
			double num5 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, num3).Center);
			double num6 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).GemXSize * 2.0;
			if (num5 > num6)
			{
				continue;
			}
			CSChannelGemData channelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, num3);
			CSChannelGemData channelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			CSChannelGemData channelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, num4);
			if (channelGemData2 == null || channelGemData3 == null)
			{
				return UpdateError("Invalid gems");
			}
			CSDataHelper.SetSettingType(channelGemData3, flag ? GemSettingType.BR : GemSettingType.PR);
			double num7 = cSDataArray.GetArrayCount();
			double vHeight2;
			double num8;
			double num9;
			if (j < cSDataArray.GetArrayCount() / 2)
			{
				vHeight2 = value4 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value5 * ((double)j / (num7 / 2.0));
				num8 = value21 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value22 * ((double)j / (num7 / 2.0));
				num9 = value50 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value51 * ((double)j / (num7 / 2.0));
			}
			else
			{
				vHeight2 = value5 * (num7 - (double)j) / (double)j + value6 * (1.0 - (num7 - (double)j) / (double)j);
				num8 = value22 * (num7 - (double)j) / (double)j + value23 * (1.0 - (num7 - (double)j) / (double)j);
				num9 = value51 * (num7 - (double)j) / (double)j + value52 * (1.0 - (num7 - (double)j) / (double)j);
			}
			OnCircle circleLeft;
			OnCircle circleRight;
			double height3;
			string circles = GetCircles(channelGemData2, channelGemData3, channelGemData, crv, crv2, value13, value14, vHeight2, num8, value49, num9, out circleLeft, out circleRight, out height3);
			if (!string.IsNullOrEmpty(circles))
			{
				return UpdateError(circles);
			}
			if (_prongBase == ValueListItem.FollowLargestGem)
			{
				height3 = (value49 ? (num * num9) : num);
			}
			On3dVector on3dVector3 = new On3dVector(circleLeft.Center());
			On3dVector on3dVector4 = new On3dVector(circleRight.Center());
			On3dPoint value53 = new On3dPoint((on3dVector3 + on3dVector4) / 2.0);
			On3dVector value54 = on3dVector4 - on3dVector3;
			if (CSApplicationHelper.Application.IsCamMode)
			{
				OnXform onXform = new OnXform();
				onXform.ChangeBasis(circleLeft.plane, OnUtil.On_xy_plane);
				Vector3d zAxis = Vector3d.ZAxis;
				zAxis.Transform(CSConvert.XformCopyToTransform(onXform));
				zAxis.Unitize();
				OnXform onXform2 = new OnXform();
				onXform2.ChangeBasis(circleRight.plane, OnUtil.On_xy_plane);
				Vector3d zAxis2 = Vector3d.ZAxis;
				zAxis2.Transform(CSConvert.XformCopyToTransform(onXform2));
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
				if (num4 == j)
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
					string circles2 = GetCircles(channelGemData2, channelGemData3, channelGemData, crv, crv2, value19, value20, vHeight2, num8, value49, num9, out circleLeft2, out circleRight2, out height4);
					OnCircle circleLeft3;
					OnCircle circleRight3;
					double height5;
					string circles3 = GetCircles(channelGemData3, channelGemData4, channelGemData, crv, crv2, value19, value20, vHeight2, num8, value49, num9, out circleLeft3, out circleRight3, out height5);
					if (!string.IsNullOrEmpty(circles2) || !string.IsNullOrEmpty(circles3))
					{
						break;
					}
					double num10 = (circleRight3.radius + circleRight2.radius + circleLeft3.radius + circleLeft2.radius) / 4.0 + value46;
					double[] array = new double[4] { circleRight3.radius, circleRight2.radius, circleLeft2.radius, circleLeft3.radius };
					if (value45 == 157)
					{
						double num11 = array[0];
						for (int k = 1; k < array.Length; k++)
						{
							if (num11 < array[k])
							{
								num11 = array[k];
							}
						}
						num10 = num11 + value46;
					}
					if (value45 == 158)
					{
						double num12 = array[0];
						for (int l = 1; l < array.Length; l++)
						{
							if (num12 > array[l])
							{
								num12 = array[l];
							}
						}
						num10 = num12 + value46;
					}
					double num13 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData3.Size) / 2.0) : channelGemData3.GemZMin);
					Point3d value55 = channelGemData3.GetCurve().PointAt(t);
					value55.Transform(Transform.Translation(channelGemData3.Normal * (num8 - num13)));
					vector3d4.Unitize();
					vector3d4 *= num10 - value20;
					value55 += vector3d4;
					On3dPoint on3dPoint = CSConvert.Point3dCopyToOn3dPoint(value55);
					OnCircle onCircle2 = new OnCircle(new OnPlane(on3dPoint, channelGemData3.Normal.Convert()), on3dPoint, num10);
					OnXform onXform3 = new OnXform();
					onXform3.ChangeBasis(onCircle2.plane, OnUtil.On_xy_plane);
					double num14 = onCircle2.radius * 2.0;
					if (num14 > value2 && num14 < value3)
					{
						double taperPer;
						double filletPer;
						if (j < cSDataArray.GetArrayCount() / 2)
						{
							_ = (num7 / 2.0 - (double)j) / (num7 / 2.0);
							_ = (double)j / (num7 / 2.0);
							taperPer = value10 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value11 * ((double)j / (num7 / 2.0));
							filletPer = value7 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value8 * ((double)j / (num7 / 2.0));
						}
						else
						{
							_ = value42 * (num7 - (double)j) / (double)j;
							_ = (num7 - (double)j) / (double)j;
							taperPer = value11 * (num7 - (double)j) / (double)j + value12 * (1.0 - (num7 - (double)j) / (double)j);
							filletPer = value8 * (num7 - (double)j) / (double)j + value9 * (1.0 - (num7 - (double)j) / (double)j);
						}
						if (!_prongCache.Create(nurbs_curve, num14, height3, filletPer, taperPer, value, out var prong, out var prongBaseRail))
						{
							return UpdateError("Create prong cache failed");
						}
						prong.Transform(onXform3);
						prongBaseRail.Transform(onXform3);
						concurrentBag.Add(new CSOnGeometryData(prong.Convert(), onXform3.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(prongBaseRail.Convert(), onXform3.Convert(), applyTransform: false));
					}
				}
			}
			OnXform onXform4 = new OnXform();
			onXform4.ChangeBasis(circleLeft.plane, OnUtil.On_xy_plane);
			On3dVector on3dVector5 = new On3dVector(OnUtil.On_zaxis);
			on3dVector5.Transform(onXform4);
			on3dVector5.Unitize();
			OnXform onXform5 = new OnXform();
			onXform5.ChangeBasis(circleRight.plane, OnUtil.On_xy_plane);
			On3dVector on3dVector6 = new On3dVector(OnUtil.On_zaxis);
			on3dVector6.Transform(onXform5);
			on3dVector6.Unitize();
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
				double radius3 = (radius + radius2) / 2.0;
				circleRight = new OnCircle(circleRight.Center(), radius3);
				circleLeft = new OnCircle(circleLeft.Center(), radius3);
			}
			if (!flag)
			{
				double num15 = circleLeft.radius * 2.0;
				if (num15 > value2 && num15 < value3)
				{
					double taperPer2;
					double filletPer2;
					if (j < cSDataArray.GetArrayCount() / 2)
					{
						_ = (num7 / 2.0 - (double)j) / (num7 / 2.0);
						_ = (double)j / (num7 / 2.0);
						taperPer2 = value10 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value11 * ((double)j / (num7 / 2.0));
						filletPer2 = value7 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value8 * ((double)j / (num7 / 2.0));
					}
					else
					{
						_ = value42 * (num7 - (double)j) / (double)j;
						_ = (num7 - (double)j) / (double)j;
						taperPer2 = value11 * (num7 - (double)j) / (double)j + value12 * (1.0 - (num7 - (double)j) / (double)j);
						filletPer2 = value8 * (num7 - (double)j) / (double)j + value9 * (1.0 - (num7 - (double)j) / (double)j);
					}
					if (!_prongCache.Create(nurbs_curve, num15, height3, filletPer2, taperPer2, value, out var prong2, out var prongBaseRail2))
					{
						return UpdateError("Create prong cache failed");
					}
					prong2.Transform(onXform4);
					prongBaseRail2.Transform(onXform4);
					concurrentBag.Add(new CSOnGeometryData(prong2.Convert(), onXform4.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(prongBaseRail2.Convert(), onXform4.Convert(), applyTransform: false));
				}
			}
			if (!flag)
			{
				double num16 = circleRight.radius * 2.0;
				if (num16 > value2 && num16 < value3)
				{
					double taperPer3;
					double filletPer3;
					if (j < cSDataArray.GetArrayCount() / 2)
					{
						_ = (num7 / 2.0 - (double)j) / (num7 / 2.0);
						_ = (double)j / (num7 / 2.0);
						taperPer3 = value10 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value11 * ((double)j / (num7 / 2.0));
						filletPer3 = value7 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value8 * ((double)j / (num7 / 2.0));
					}
					else
					{
						_ = value42 * (num7 - (double)j) / (double)j;
						_ = (num7 - (double)j) / (double)j;
						taperPer3 = value11 * (num7 - (double)j) / (double)j + value12 * (1.0 - (num7 - (double)j) / (double)j);
						filletPer3 = value8 * (num7 - (double)j) / (double)j + value9 * (1.0 - (num7 - (double)j) / (double)j);
					}
					if (!_prongCache.Create(nurbs_curve, num16, height3, filletPer3, taperPer3, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					prong3.Transform(onXform5);
					prongBaseRail3.Transform(onXform5);
					concurrentBag.Add(new CSOnGeometryData(prong3.Convert(), onXform5.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(prongBaseRail3.Convert(), onXform5.Convert(), applyTransform: false));
				}
			}
			On3dVector on3dVector7 = on3dVector5 + on3dVector6;
			on3dVector7.Unitize();
			if (flag)
			{
				on3dVector3 = new On3dVector(circleLeft.Center());
				on3dVector4 = new On3dVector(circleRight.Center());
				double baseDomeRadius;
				double blendEdge;
				double domeRadius;
				double bottomArcPercent;
				double filletPer4;
				if (j < cSDataArray.GetArrayCount() / 2)
				{
					baseDomeRadius = value35 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value36 * ((double)j / (num7 / 2.0));
					blendEdge = value38 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value39 * ((double)j / (num7 / 2.0));
					domeRadius = value32 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value33 * ((double)j / (num7 / 2.0));
					bottomArcPercent = value41 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value42 * ((double)j / (num7 / 2.0));
					filletPer4 = value10 * ((num7 / 2.0 - (double)j) / (num7 / 2.0)) + value11 * ((double)j / (num7 / 2.0));
				}
				else
				{
					baseDomeRadius = value36 * (num7 - (double)j) / (double)j + value37 * (1.0 - (num7 - (double)j) / (double)j);
					blendEdge = value39 * (num7 - (double)j) / (double)j + value40 * (1.0 - (num7 - (double)j) / (double)j);
					domeRadius = value33 * (num7 - (double)j) / (double)j + value34 * (1.0 - (num7 - (double)j) / (double)j);
					bottomArcPercent = value42 * (num7 - (double)j) / (double)j + value43 * (1.0 - (num7 - (double)j) / (double)j);
					filletPer4 = value11 * (num7 - (double)j) / (double)j + value12 * (1.0 - (num7 - (double)j) / (double)j);
				}
				Brep geometry = MakeBar((on3dVector3 - on3dVector4).Length(), circleLeft.radius, circleRight.radius, height3, filletPer4, domeRadius, baseDomeRadius, blendEdge, bottomArcPercent);
				value53 = new On3dPoint((on3dVector3 + on3dVector4) / 2.0);
				value54 = on3dVector4 - on3dVector3;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value53), CSConvert.On3dVectorCopyToVector3d(value54), CSConvert.On3dVectorCopyToVector3d(on3dVector7));
				concurrentBag.Add(new CSOnGeometryData(geometry, transform));
			}
			if (on3dVector == OnUtil.On_UNSET_VECTOR)
			{
				on3dVector = on3dVector7;
			}
			on3dVector2 = on3dVector7;
		}
		if (!flag2)
		{
			for (int m = 0; m < cSDataArray.GetArrayCount(); m++)
			{
				int num17 = ((cSDataArray.GetArrayCount() > 1) ? (m + 1) : 0);
				int index = ((m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1));
				if (cSDataArray.GetArrayCount() > 1)
				{
					double num18;
					if (m != cSDataArray.GetArrayCount() - 1)
					{
						num18 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, m + 1).Center);
					}
					else
					{
						num18 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Center);
						num17 = 0;
					}
					double num19 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).GemXSize * 2.0;
					if (num18 <= num19)
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
						CSChannelGemData channelGemData7 = CSUtilGemRails.GetChannelGemData(cSDataArray, num17);
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
						if (num17 == m)
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
							double num20 = cSDataArray.GetArrayCount();
							double vHeight3;
							double num21;
							double percent;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight3 = value4 * ((num20 / 2.0 - (double)m) / (num20 / 2.0)) + value5 * ((double)m / (num20 / 2.0));
								num21 = value21 * ((num20 / 2.0 - (double)m) / (num20 / 2.0)) + value22 * ((double)m / (num20 / 2.0));
								percent = value50 * ((num20 / 2.0 - (double)m) / (num20 / 2.0)) + value51 * ((double)m / (num20 / 2.0));
							}
							else
							{
								vHeight3 = value5 * (num20 - (double)m) / (double)m + value6 * (1.0 - (num20 - (double)m) / (double)m);
								num21 = value22 * (num20 - (double)m) / (double)m + value23 * (1.0 - (num20 - (double)m) / (double)m);
								percent = value51 * (num20 - (double)m) / (double)m + value52 * (1.0 - (num20 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData5, channelGemData6, channelGemData, crv, crv2, value19, value20, vHeight3, num21, value49, percent, out var circleLeft4, out var circleRight4, out var height6)))
							{
								break;
							}
							double num22 = (circleRight4.radius + circleLeft4.radius) / 2.0 + value46;
							double[] array2 = new double[2] { circleRight4.radius, circleLeft4.radius };
							if (value45 == 157)
							{
								double num23 = array2[0];
								for (int n = 1; n < array2.Length; n++)
								{
									if (num23 < array2[n])
									{
										num23 = array2[n];
									}
								}
								num22 = num23 + value46;
							}
							if (value45 == 158)
							{
								double num24 = array2[0];
								for (int num25 = 1; num25 < array2.Length; num25++)
								{
									if (num24 > array2[num25])
									{
										num24 = array2[num25];
									}
								}
								num22 = num24 + value46;
							}
							double num26 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData6.Size) / 2.0) : channelGemData6.GemZMin);
							Point3d value56 = channelGemData6.GetCurve().PointAt(t2);
							value56.Transform(Transform.Translation(channelGemData6.Normal * (num21 - num26)));
							vector3d7.Unitize();
							vector3d7 *= num22 - value20;
							value56 += vector3d7;
							On3dPoint on3dPoint2 = CSConvert.Point3dCopyToOn3dPoint(value56);
							OnCircle onCircle3 = new OnCircle(new OnPlane(on3dPoint2, channelGemData6.Normal.Convert()), on3dPoint2, num22);
							OnXform onXform6 = new OnXform();
							onXform6.ChangeBasis(onCircle3.plane, OnUtil.On_xy_plane);
							double num27 = onCircle3.radius * 2.0;
							if (num27 > value2 && num27 < value3)
							{
								double taperPer4;
								double filletPer5;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									_ = (num20 / 2.0 - (double)m) / (num20 / 2.0);
									_ = (double)m / (num20 / 2.0);
									taperPer4 = value10 * ((num20 / 2.0 - (double)m) / (num20 / 2.0)) + value11 * ((double)m / (num20 / 2.0));
									filletPer5 = value7 * ((num20 / 2.0 - (double)m) / (num20 / 2.0)) + value8 * ((double)m / (num20 / 2.0));
								}
								else
								{
									_ = value42 * (num20 - (double)m) / (double)m;
									_ = (num20 - (double)m) / (double)m;
									taperPer4 = value11 * (num20 - (double)m) / (double)m + value12 * (1.0 - (num20 - (double)m) / (double)m);
									filletPer5 = value8 * (num20 - (double)m) / (double)m + value9 * (1.0 - (num20 - (double)m) / (double)m);
								}
								if (!_prongCache.Create(nurbs_curve, num27, height6, filletPer5, taperPer4, value, out var prong4, out var prongBaseRail4))
								{
									return UpdateError("Create prong cache failed");
								}
								prong4.Transform(onXform6);
								prongBaseRail4.Transform(onXform6);
								concurrentBag.Add(new CSOnGeometryData(prong4.Convert(), onXform6.Convert(), applyTransform: false));
								concurrentBag2.Add(new CSOnGeometryData(prongBaseRail4.Convert(), onXform6.Convert(), applyTransform: false));
							}
						}
					}
					if (!flag)
					{
						CSChannelGemData channelGemData8 = CSUtilGemRails.GetChannelGemData(cSDataArray, m);
						CSChannelGemData channelGemData9 = CSUtilGemRails.GetChannelGemData(cSDataArray, num17);
						CSChannelGemData channelGemData10 = CSUtilGemRails.GetChannelGemData(cSDataArray, (num17 + 1 != cSDataArray.GetArrayCount()) ? (num17 + 1) : 0);
						Vector3d vector3d8 = channelGemData9.Center - channelGemData8.Center;
						vector3d8.Unitize();
						Vector3d vector3d9 = channelGemData9.Center - channelGemData10.Center;
						vector3d9.Unitize();
						if (num17 == m)
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
							double num28 = cSDataArray.GetArrayCount();
							double vHeight4;
							double num29;
							double percent2;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight4 = value4 * ((num28 / 2.0 - (double)m) / (num28 / 2.0)) + value5 * ((double)m / (num28 / 2.0));
								num29 = value21 * ((num28 / 2.0 - (double)m) / (num28 / 2.0)) + value22 * ((double)m / (num28 / 2.0));
								percent2 = value50 * ((num28 / 2.0 - (double)m) / (num28 / 2.0)) + value51 * ((double)m / (num28 / 2.0));
							}
							else
							{
								vHeight4 = value5 * (num28 - (double)m) / (double)m + value6 * (1.0 - (num28 - (double)m) / (double)m);
								num29 = value22 * (num28 - (double)m) / (double)m + value23 * (1.0 - (num28 - (double)m) / (double)m);
								percent2 = value51 * (num28 - (double)m) / (double)m + value52 * (1.0 - (num28 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData9, channelGemData10, channelGemData, crv, crv2, value19, value20, vHeight4, num29, value49, percent2, out var circleLeft5, out var circleRight5, out var height7)))
							{
								break;
							}
							double num30 = (circleRight5.radius + circleLeft5.radius) / 2.0 + value46;
							double[] array3 = new double[2] { circleRight5.radius, circleLeft5.radius };
							if (value45 == 157)
							{
								double num31 = array3[0];
								for (int num32 = 1; num32 < array3.Length; num32++)
								{
									if (num31 < array3[num32])
									{
										num31 = array3[num32];
									}
								}
								num30 = num31 + value46;
							}
							if (value45 == 158)
							{
								double num33 = array3[0];
								for (int num34 = 1; num34 < array3.Length; num34++)
								{
									if (num33 > array3[num34])
									{
										num33 = array3[num34];
									}
								}
								num30 = num33 + value46;
							}
							double num35 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData9.Size) / 2.0) : channelGemData9.GemZMin);
							Point3d value57 = channelGemData9.GetCurve().PointAt(t3);
							value57.Transform(Transform.Translation(channelGemData9.Normal * (num29 - num35)));
							vector3d10.Unitize();
							vector3d10 *= num30 - value20;
							value57 += vector3d10;
							On3dPoint on3dPoint3 = CSConvert.Point3dCopyToOn3dPoint(value57);
							OnCircle onCircle4 = new OnCircle(new OnPlane(on3dPoint3, channelGemData9.Normal.Convert()), on3dPoint3, num30);
							OnXform onXform7 = new OnXform();
							onXform7.ChangeBasis(onCircle4.plane, OnUtil.On_xy_plane);
							double num36 = onCircle4.radius * 2.0;
							if (num36 > value2 && num36 < value3)
							{
								double taperPer5;
								double filletPer6;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									_ = (num28 / 2.0 - (double)m) / (num28 / 2.0);
									_ = (double)m / (num28 / 2.0);
									taperPer5 = value10 * ((num28 / 2.0 - (double)m) / (num28 / 2.0)) + value11 * ((double)m / (num28 / 2.0));
									filletPer6 = value7 * ((num28 / 2.0 - (double)m) / (num28 / 2.0)) + value8 * ((double)m / (num28 / 2.0));
								}
								else
								{
									_ = value42 * (num28 - (double)m) / (double)m;
									_ = (num28 - (double)m) / (double)m;
									taperPer5 = value11 * (num28 - (double)m) / (double)m + value12 * (1.0 - (num28 - (double)m) / (double)m);
									filletPer6 = value8 * (num28 - (double)m) / (double)m + value9 * (1.0 - (num28 - (double)m) / (double)m);
								}
								if (!_prongCache.Create(nurbs_curve, num36, height7, filletPer6, taperPer5, value, out var prong5, out var prongBaseRail5))
								{
									return UpdateError("Create prong cache failed");
								}
								prong5.Transform(onXform7);
								prongBaseRail5.Transform(onXform7);
								concurrentBag.Add(new CSOnGeometryData(prong5.Convert(), onXform7.Convert(), applyTransform: false));
								concurrentBag2.Add(new CSOnGeometryData(prongBaseRail5.Convert(), onXform7.Convert(), applyTransform: false));
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
					CSChannelGemData cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, num17).Duplicate() as CSChannelGemData;
					if (cSDataArray.GetArrayCount() == 1)
					{
						cSChannelGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData.Normal, cSChannelGemData.Center));
					}
					double num37 = cSChannelGemData.GemXSize * value26 + value15 + value17;
					double height9 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + value4 - value21) : channelGemData.GemZSize);
					if (CSApplicationHelper.Application.IsCamMode)
					{
						CSChannelGemData gemPrev = cSChannelGemData;
						CSChannelGemData gemCurrent = CSUtilGemRails.GetChannelGemData(cSDataArray, (num17 + 1 != cSDataArray.GetArrayCount()) ? (num17 + 1) : 0).Duplicate() as CSChannelGemData;
						OnCircle circleLeft7;
						OnCircle circleRight7;
						double height10;
						string circles5 = GetCircles(gemPrev, gemCurrent, channelGemData, crv, crv2, value15, value17, value4, value21, value49, value50, out circleLeft7, out circleRight7, out height10);
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
							double radius7 = circleRight7.radius;
							double radius8 = circleLeft7.radius;
							double radius9 = (radius7 + radius8) / 2.0;
							circleRight7 = new OnCircle(circleRight7.Center(), radius9);
							circleLeft7 = new OnCircle(circleLeft7.Center(), radius9);
						}
						On3dVector vLeft = new On3dVector(circleLeft7.Center());
						On3dVector vRight = new On3dVector(circleRight7.Center());
						height9 = GetCamHeightEnd(gemPrev, gemCurrent, circleLeft7, circleRight7, vLeft, vRight);
					}
					if (!_prongCache.Create(nurbs_curve, num37, height9, value7, value10, value, out var prong7, out var prongBaseRail7))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint on3dPoint4 = new On3dPoint(cSChannelGemData.Center.Convert()) - new On3dVector(cSChannelGemData.ForwardVectorInGemPlane.Convert());
					double num38 = (cSChannelGemData.GemXSize * value26 + value15) / 2.0;
					double num39 = value21 - cSChannelGemData.GemZMin;
					On3dVector on3dVector12;
					if (on3dVector == OnUtil.On_UNSET_VECTOR)
					{
						on3dVector12 = new On3dVector(cSChannelGemData.Normal.Convert());
						on3dVector12.Unitize();
					}
					else
					{
						OnXform onXform10 = new OnXform();
						onXform10.Mirror(cSChannelGemData.Center.Convert(), cSChannelGemData.ForwardVectorInGemPlane.Convert());
						on3dVector12 = new On3dVector(on3dVector);
						on3dVector12.Transform(onXform10);
						on3dVector12.Unitize();
					}
					On3dVector on3dVector13 = new On3dVector(cSChannelGemData.LeftVector.Convert());
					on3dVector13.Unitize();
					On3dVector on3dVector14 = OnUtil.ON_CrossProduct(on3dVector12, on3dVector13);
					on3dVector14.Unitize();
					OnPlane plane = new OnPlane(on3dPoint4 + on3dVector14 * num38 + on3dVector12 * num39, on3dVector13, on3dVector14);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd4:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd7:
					{
						OnXform onXform18 = new OnXform();
						onXform18.ChangeBasis(plane, OnUtil.On_xy_plane);
						OnXform onXform19 = new OnXform();
						onXform19.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value27), cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
						OnXform xform2 = onXform19 * onXform18;
						prong7.Transform(xform2);
						prongBaseRail7.Transform(xform2);
						concurrentBag.Add(new CSOnGeometryData(prong7.Convert(), xform2.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(prongBaseRail7.Convert(), xform2.Convert(), applyTransform: false));
						break;
					}
					case ValueListItem.ProngPlacerStyleEnd5:
					case ValueListItem.ProngPlacerStyleEnd8:
					case ValueListItem.ProngPlacerStyleEnd9:
					{
						double num40 = value28 / 2.0;
						OnXform onXform11 = new OnXform();
						onXform11.ChangeBasis(plane, OnUtil.On_xy_plane);
						OnXform onXform12 = new OnXform();
						onXform12.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num40), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform13 = new OnXform();
						onXform13.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value27 + num40), cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
						OnXform onXform14 = onXform13 * onXform11 * onXform12;
						if (!flag)
						{
							OnGeometry onGeometry = prong7.Duplicate();
							onGeometry.Transform(onXform14);
							concurrentBag.Add(new CSOnGeometryData(onGeometry.Convert(), onXform14.Convert(), applyTransform: false));
							OnGeometry onGeometry2 = prongBaseRail7.Duplicate();
							onGeometry2.Transform(onXform14);
							concurrentBag2.Add(new CSOnGeometryData(onGeometry2.Convert(), onXform14.Convert(), applyTransform: false));
						}
						OnXform onXform15 = new OnXform();
						onXform15.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num40), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform16 = new OnXform();
						onXform16.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value27 - num40), cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
						OnXform onXform17 = onXform16 * onXform11 * onXform15;
						if (!flag)
						{
							OnGeometry onGeometry3 = prong7.Duplicate();
							onGeometry3.Transform(onXform17);
							concurrentBag.Add(new CSOnGeometryData(onGeometry3.Convert(), onXform17.Convert(), applyTransform: false));
							OnGeometry onGeometry4 = prongBaseRail7.Duplicate();
							onGeometry4.Transform(onXform17);
							concurrentBag2.Add(new CSOnGeometryData(onGeometry4.Convert(), onXform17.Convert(), applyTransform: false));
						}
						if (flag)
						{
							On3dPoint on3dPoint5 = new On3dPoint(0.0, 0.0, 0.0);
							On3dPoint on3dPoint6 = new On3dPoint(0.0, 0.0, 0.0);
							On3dVector on3dVector15 = new On3dVector(0.0, 0.0, 1.0);
							On3dVector on3dVector16 = new On3dVector(0.0, 0.0, 1.0);
							on3dPoint5.Transform(onXform14);
							on3dVector15.Transform(onXform14);
							on3dPoint6.Transform(onXform17);
							on3dVector16.Transform(onXform17);
							Brep geometry3 = MakeBar((on3dPoint5 - on3dPoint6).Length(), num37 / 2.0, num37 / 2.0, height9, value10, value32, value35, value38, value41);
							On3dPoint value62 = new On3dPoint((on3dPoint5 + on3dPoint6) / 2.0);
							On3dVector value63 = on3dPoint6 - on3dPoint5;
							On3dVector value64 = on3dVector15 + on3dVector16;
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
					CSChannelGemData channelGemData11 = CSUtilGemRails.GetChannelGemData(cSDataArray, num17);
					CSChannelGemData channelGemData12 = CSUtilGemRails.GetChannelGemData(cSDataArray, num17 + 1);
					if (channelGemData11 == null || channelGemData12 == null)
					{
						return UpdateError("Invalid gems");
					}
					CSDataHelper.SetSettingType(channelGemData12, flag ? GemSettingType.BR : GemSettingType.PR);
					OnCircle circleLeft6;
					OnCircle circleRight6;
					double height8;
					string circles4 = GetCircles(channelGemData11, channelGemData12, channelGemData, crv, crv2, value15, value17, value4, value21, value49, value50, out circleLeft6, out circleRight6, out height8);
					if (!string.IsNullOrEmpty(circles4))
					{
						return UpdateError(circles4);
					}
					On3dVector on3dVector8 = new On3dVector(circleLeft6.Center());
					On3dVector on3dVector9 = new On3dVector(circleRight6.Center());
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
						double radius4 = circleRight6.radius;
						double radius5 = circleLeft6.radius;
						double radius6 = (radius4 + radius5) / 2.0;
						circleRight6 = new OnCircle(circleRight6.Center(), radius6);
						circleLeft6 = new OnCircle(circleLeft6.Center(), radius6);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height8 = (value49 ? (num * value50) : num);
					}
					OnXform onXform8 = new OnXform();
					onXform8.ChangeBasis(circleLeft6.plane, OnUtil.On_xy_plane);
					On3dVector on3dVector10 = new On3dVector(OnUtil.On_zaxis);
					on3dVector10.Transform(onXform8);
					on3dVector10.Unitize();
					OnXform onXform9 = new OnXform();
					onXform9.ChangeBasis(circleRight6.plane, OnUtil.On_xy_plane);
					On3dVector on3dVector11 = new On3dVector(OnUtil.On_zaxis);
					on3dVector11.Transform(onXform9);
					on3dVector11.Unitize();
					On3dVector value58 = on3dVector10 + on3dVector11;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height8 = GetCamHeightEnd(channelGemData11, channelGemData12, circleLeft6, circleRight6, on3dVector8, on3dVector9);
					}
					if (!flag)
					{
						if (!_prongCache.Create(nurbs_curve, circleLeft6.Diameter(), height8, value7, value10, value, out var prong6, out var prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value59 = on3dVector9 - on3dVector8;
						Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector8), CSConvert.On3dVectorCopyToVector3d(value59), CSConvert.On3dVectorCopyToVector3d(value58));
						Vector3d normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(prong6.Convert(), transform3 * transform2));
						if (!_prongCache.Create(nurbs_curve, circleRight6.Diameter(), height8, value7, value10, value, out prong6, out prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						value59 = on3dVector8 - on3dVector9;
						transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector9), CSConvert.On3dVectorCopyToVector3d(value59), CSConvert.On3dVectorCopyToVector3d(value58));
						normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(prong6.Convert(), transform3 * transform2));
					}
					else
					{
						Brep geometry2 = MakeBar((on3dVector8 - on3dVector9).Length(), circleLeft6.radius, circleRight6.radius, height8, value10, value32, value35, value38, value41);
						On3dPoint value60 = new On3dPoint((on3dVector8 + on3dVector9) / 2.0);
						On3dVector value61 = on3dVector9 - on3dVector8;
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
					double num41 = cSChannelGemData2.GemXSize * value29 + value16 + value18;
					double height12 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData2.GemZSize + value6 - value23) : channelGemData.GemZSize);
					if (CSApplicationHelper.Application.IsCamMode)
					{
						CSChannelGemData gemCurrent2 = cSChannelGemData2;
						CSChannelGemData gemPrev2 = CSUtilGemRails.GetChannelGemData(cSDataArray, (m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1)).Duplicate() as CSChannelGemData;
						OnCircle circleLeft9;
						OnCircle circleRight9;
						double height13;
						string circles7 = GetCircles(gemPrev2, gemCurrent2, channelGemData, crv, crv2, value16, value18, value6, value23, value49, value52, out circleLeft9, out circleRight9, out height13);
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
							double radius13 = circleRight9.radius;
							double radius14 = circleLeft9.radius;
							double radius15 = (radius13 + radius14) / 2.0;
							circleRight9 = new OnCircle(circleRight9.Center(), radius15);
							circleLeft9 = new OnCircle(circleLeft9.Center(), radius15);
						}
						On3dVector vLeft2 = new On3dVector(circleLeft9.Center());
						On3dVector vRight2 = new On3dVector(circleRight9.Center());
						height12 = GetCamHeightEnd(gemPrev2, gemCurrent2, circleLeft9, circleRight9, vLeft2, vRight2);
					}
					if (!_prongCache.Create(nurbs_curve, num41, height12, value9, value12, value, out var prong9, out var prongBaseRail9))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint on3dPoint7 = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					double num42 = (cSChannelGemData2.GemXSize * value29 + value16) / 2.0;
					double num43 = value23 - cSChannelGemData2.GemZMin;
					On3dVector on3dVector21;
					if (on3dVector2 == OnUtil.On_UNSET_VECTOR)
					{
						on3dVector21 = new On3dVector(cSChannelGemData2.Normal.Convert());
						on3dVector21.Unitize();
					}
					else
					{
						OnXform onXform22 = new OnXform();
						onXform22.Mirror(cSChannelGemData2.Center.Convert(), cSChannelGemData2.ForwardVectorInGemPlane.Convert());
						on3dVector21 = new On3dVector(on3dVector2);
						on3dVector21.Transform(onXform22);
						on3dVector21.Unitize();
					}
					On3dVector on3dVector22 = new On3dVector(cSChannelGemData2.RightVector.Convert());
					on3dVector22.Unitize();
					On3dVector on3dVector23 = OnUtil.ON_CrossProduct(on3dVector21, on3dVector22);
					on3dVector23.Unitize();
					OnPlane plane2 = new OnPlane(on3dPoint7 + on3dVector23 * num42 + on3dVector21 * num43, on3dVector22, on3dVector23);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd2:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd8:
					{
						OnXform onXform30 = new OnXform();
						onXform30.ChangeBasis(plane2, OnUtil.On_xy_plane);
						OnXform onXform31 = new OnXform();
						onXform31.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value30), cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
						OnXform xform3 = onXform31 * onXform30;
						prong9.Transform(xform3);
						prongBaseRail9.Transform(xform3);
						concurrentBag.Add(new CSOnGeometryData(prong9.Convert(), xform3.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(prongBaseRail9.Convert(), xform3.Convert(), applyTransform: false));
						break;
					}
					case ValueListItem.ProngPlacerStyleEnd3:
					case ValueListItem.ProngPlacerStyleEnd7:
					case ValueListItem.ProngPlacerStyleEnd9:
					{
						double num44 = value31 / 2.0;
						OnXform onXform23 = new OnXform();
						onXform23.ChangeBasis(plane2, OnUtil.On_xy_plane);
						OnXform onXform24 = new OnXform();
						onXform24.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num44), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform25 = new OnXform();
						onXform25.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value30 + num44), cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
						OnXform onXform26 = onXform25 * onXform23 * onXform24;
						if (!flag)
						{
							OnGeometry onGeometry5 = prong9.Duplicate();
							onGeometry5.Transform(onXform26);
							concurrentBag.Add(new CSOnGeometryData(onGeometry5.Convert(), onXform26.Convert(), applyTransform: false));
							OnGeometry onGeometry6 = prongBaseRail9.Duplicate();
							onGeometry6.Transform(onXform26);
							concurrentBag2.Add(new CSOnGeometryData(onGeometry6.Convert(), onXform26.Convert(), applyTransform: false));
						}
						OnXform onXform27 = new OnXform();
						onXform27.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num44), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform28 = new OnXform();
						onXform28.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value30 - num44), cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
						OnXform onXform29 = onXform28 * onXform23 * onXform27;
						if (!flag)
						{
							OnGeometry onGeometry7 = prong9.Duplicate();
							onGeometry7.Transform(onXform29);
							concurrentBag.Add(new CSOnGeometryData(onGeometry7.Convert(), onXform29.Convert(), applyTransform: false));
							OnGeometry onGeometry8 = prongBaseRail9.Duplicate();
							onGeometry8.Transform(onXform29);
							concurrentBag2.Add(new CSOnGeometryData(onGeometry8.Convert(), onXform29.Convert(), applyTransform: false));
						}
						if (flag)
						{
							On3dPoint on3dPoint8 = new On3dPoint(0.0, 0.0, 0.0);
							On3dPoint on3dPoint9 = new On3dPoint(0.0, 0.0, 0.0);
							On3dVector on3dVector24 = new On3dVector(0.0, 0.0, 1.0);
							On3dVector on3dVector25 = new On3dVector(0.0, 0.0, 1.0);
							on3dPoint8.Transform(onXform26);
							on3dVector24.Transform(onXform26);
							on3dPoint9.Transform(onXform29);
							on3dVector25.Transform(onXform29);
							Brep geometry5 = MakeBar((on3dPoint8 - on3dPoint9).Length(), num41 / 2.0, num41 / 2.0, height12, value12, value34, value37, value40, value43);
							On3dPoint value69 = new On3dPoint((on3dPoint8 + on3dPoint9) / 2.0);
							On3dVector value70 = on3dPoint9 - on3dPoint8;
							On3dVector value71 = on3dVector24 + on3dVector25;
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
					string circles6 = GetCircles(channelGemData13, channelGemData14, channelGemData, crv, crv2, value16, value18, value6, value23, value49, value52, out circleLeft8, out circleRight8, out height11);
					if (!string.IsNullOrEmpty(circles6))
					{
						return UpdateError(circles6);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height11 = (value49 ? (num * value52) : num);
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
						double radius10 = circleRight8.radius;
						double radius11 = circleLeft8.radius;
						double radius12 = (radius10 + radius11) / 2.0;
						circleRight8 = new OnCircle(circleRight8.Center(), radius12);
						circleLeft8 = new OnCircle(circleLeft8.Center(), radius12);
					}
					On3dVector on3dVector17 = new On3dVector(circleLeft8.Center());
					On3dVector on3dVector18 = new On3dVector(circleRight8.Center());
					OnXform onXform20 = new OnXform();
					onXform20.ChangeBasis(circleLeft8.plane, OnUtil.On_xy_plane);
					On3dVector on3dVector19 = new On3dVector(OnUtil.On_zaxis);
					on3dVector19.Transform(onXform20);
					on3dVector19.Unitize();
					OnXform onXform21 = new OnXform();
					onXform21.ChangeBasis(circleRight8.plane, OnUtil.On_xy_plane);
					On3dVector on3dVector20 = new On3dVector(OnUtil.On_zaxis);
					on3dVector20.Transform(onXform21);
					on3dVector20.Unitize();
					On3dVector value65 = on3dVector19 + on3dVector20;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height11 = GetCamHeightEnd(channelGemData13, channelGemData14, circleLeft8, circleRight8, on3dVector17, on3dVector18);
					}
					if (!flag)
					{
						if (!_prongCache.Create(nurbs_curve, circleLeft8.Diameter(), height11, value9, value12, value, out var prong8, out var prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value66 = on3dVector18 - on3dVector17;
						Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector17), CSConvert.On3dVectorCopyToVector3d(value66), CSConvert.On3dVectorCopyToVector3d(value65));
						Vector3d normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(prong8.Convert(), transform8 * transform7));
						if (!_prongCache.Create(nurbs_curve, circleRight8.Diameter(), height11, value9, value12, value, out prong8, out prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						value66 = on3dVector17 - on3dVector18;
						transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector18), CSConvert.On3dVectorCopyToVector3d(value66), CSConvert.On3dVectorCopyToVector3d(value65));
						normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(prong8.Convert(), transform8 * transform7));
					}
					else
					{
						Brep geometry4 = MakeBar((on3dVector17 - on3dVector18).Length(), circleLeft8.radius, circleRight8.radius, height11, value12, value34, value37, value40, value43);
						On3dPoint value67 = new On3dPoint((on3dVector17 + on3dVector18) / 2.0);
						On3dVector value68 = on3dVector18 - on3dVector17;
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
		double num = 0.0;
		On3dPoint value = new On3dPoint((vLeft + vRight) / 2.0);
		On3dVector value2 = vRight - vLeft;
		OnXform onXform = new OnXform();
		onXform.ChangeBasis(circleLeft.plane, OnUtil.On_xy_plane);
		Vector3d zAxis = Vector3d.ZAxis;
		zAxis.Transform(CSConvert.XformCopyToTransform(onXform));
		zAxis.Unitize();
		OnXform onXform2 = new OnXform();
		onXform2.ChangeBasis(circleRight.plane, OnUtil.On_xy_plane);
		Vector3d zAxis2 = Vector3d.ZAxis;
		zAxis2.Transform(CSConvert.XformCopyToTransform(onXform2));
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
		circleLeft = null;
		circleRight = null;
		height = 0.0;
		if (gemPrev == null || gemCurrent == null)
		{
			return "Invalid gems";
		}
		On3dPoint point = (new On3dPoint(gemCurrent.Center.Convert()) + new On3dPoint(gemPrev.Center.Convert())) / 2.0;
		if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(gemPrev, gemCurrent, railLeft, vNudgeEdge, out var circleEdgeLeft))
		{
			return "CalculateCircleEdgeLeft failed";
		}
		if (!CSUtilProngPlacer.CalculateCircleEdgeRight(gemPrev, gemCurrent, railRight, vNudgeEdge, out var circleEdgeRight))
		{
			return "CalculateCircleEdgeRight failed";
		}
		OnCircle onCircle = new OnCircle(new OnPlane(gemPrev.Center.Convert(), gemPrev.Normal.Convert()), gemPrev.SizeAverage - vNudgeGem);
		OnCircle onCircle2 = new OnCircle(new OnPlane(gemCurrent.Center.Convert(), gemCurrent.Normal.Convert()), gemCurrent.SizeAverage - vNudgeGem);
		circleLeft = CSUtilGeometry.CalculateApolloniusCircle(onCircle, onCircle2, circleEdgeLeft);
		circleRight = CSUtilGeometry.CalculateApolloniusCircle(onCircle, onCircle2, circleEdgeRight);
		On3dPoint arg = onCircle.ClosestPointTo(onCircle2.Center());
		On3dPoint arg2 = onCircle2.ClosestPointTo(onCircle.Center());
		OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
		OnLineCurve onLineCurve2 = new OnLineCurve(onCircle.Center(), onCircle2.Center());
		On3dPoint on3dPoint = onLineCurve.PointAt(onLineCurve.Domain().Mid());
		double t = -1.23432101234321E+308;
		if (!onLineCurve2.GetClosestPoint(on3dPoint, ref t))
		{
			return "GetClosestPoint failed";
		}
		On3dPoint arg3 = onLineCurve2.PointAt(t);
		double num = on3dPoint.DistanceTo(arg3);
		double num2 = ((_prongBase == ValueListItem.FollowLargestGem) ? (gemMax.GemZMin + (gemMax.Size - gemCurrent.Size) / 2.0) : ((gemPrev.GemZMin + gemCurrent.GemZMin) / 2.0));
		double num3 = ((_prongBase == ValueListItem.FollowLargestGem) ? gemMax.GemZMax : ((gemPrev.GemZMax + gemCurrent.GemZMax) / 2.0));
		double num4 = ((_prongBase == ValueListItem.FollowLargestGem) ? gemMax.GemZSize : ((gemPrev.GemZSize + gemCurrent.GemZSize) / 2.0));
		if (usePercent)
		{
			num2 = ((!(percent < 0.2)) ? ((0.0 - num3) * (1.0 - percent) + num2 * percent) : ((0.0 - num3) * 0.8 + num2 * 0.2));
			num4 *= percent;
		}
		height = num4 + num + vHeight - vVerticalLocation;
		On3dVector on3dVector = new On3dVector(gemCurrent.Normal.Convert()) + new On3dVector(gemPrev.Normal.Convert());
		on3dVector.Unitize();
		double t2 = 0.0;
		circleLeft.ClosestPointTo(point, ref t2);
		circleLeft.Rotate(t2 + OnUtil.On_PI / 2.0, circleLeft.Normal());
		circleLeft.Translate(on3dVector * (vVerticalLocation - num2));
		double t3 = 0.0;
		circleRight.ClosestPointTo(point, ref t3);
		circleRight.Rotate(t3 + OnUtil.On_PI / 2.0, circleRight.Normal());
		circleRight.Translate(on3dVector * (vVerticalLocation - num2));
		return null;
	}

	private string GetHeight(CSChannelGemData gemPrev, CSChannelGemData gemCurrent, CSChannelGemData gemMax, IOnCurve railLeft, IOnCurve railRight, double vNudgeEdge, double vNudgeGem, double vHeight, double vVerticalLocation, out double height)
	{
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
		OnCircle onCircle = new OnCircle(new OnPlane(gemPrev.Center.Convert(), gemPrev.Normal.Convert()), gemPrev.SizeAverage - vNudgeGem);
		OnCircle onCircle2 = new OnCircle(new OnPlane(gemCurrent.Center.Convert(), gemCurrent.Normal.Convert()), gemCurrent.SizeAverage - vNudgeGem);
		On3dPoint arg = onCircle.ClosestPointTo(onCircle2.Center());
		On3dPoint arg2 = onCircle2.ClosestPointTo(onCircle.Center());
		OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
		OnLineCurve onLineCurve2 = new OnLineCurve(onCircle.Center(), onCircle2.Center());
		On3dPoint on3dPoint = onLineCurve.PointAt(onLineCurve.Domain().Mid());
		double t = -1.23432101234321E+308;
		if (!onLineCurve2.GetClosestPoint(on3dPoint, ref t))
		{
			return "GetClosestPoint failed";
		}
		On3dPoint arg3 = onLineCurve2.PointAt(t);
		double num = on3dPoint.DistanceTo(arg3);
		double gemZSize = gemMax.GemZSize;
		height = gemZSize + num + vHeight - vVerticalLocation;
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
