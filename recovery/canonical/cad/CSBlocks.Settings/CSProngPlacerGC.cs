using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CS;
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

namespace CSBlocks.Settings;

[CSBlock("ProngPlacerGC", "{895B4619-1C42-480F-BE3A-D8B77A2DF397}")]
public sealed class CSProngPlacerGC : CSBlock
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

	public CSProngPlacerGC(string identifier)
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
		OnNurbsCurve nurbs_curve = InProfile.NurbsCurve.Convert();
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
		double num33 = 0.0;
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
				double num34 = cSDataArray.GetArrayCount();
				double vHeight;
				double vVerticalLocation;
				if (i < cSDataArray.GetArrayCount() / 2)
				{
					vHeight = num3 * ((num34 / 2.0 - (double)i) / (num34 / 2.0)) + num4 * ((double)i / (num34 / 2.0));
					vVerticalLocation = num11 * ((num34 / 2.0 - (double)i) / (num34 / 2.0)) + num12 * ((double)i / (num34 / 2.0));
				}
				else
				{
					vHeight = num4 * (num34 - (double)i) / (double)i + num5 * (1.0 - (num34 - (double)i) / (double)i);
					vVerticalLocation = num12 * (num34 - (double)i) / (double)i + num13 * (1.0 - (num34 - (double)i) / (double)i);
				}
				double height2;
				string height = GetHeight(CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1), CSUtilGemRails.GetChannelGemData(cSDataArray, i), channelGemData, crv, crv2, vNudgeEdge, vNudgeGem, vHeight, vVerticalLocation, out height2);
				if (!string.IsNullOrEmpty(height))
				{
					return UpdateError(height);
				}
				if (height2 > num33)
				{
					num33 = height2;
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
			int num35 = j - 1;
			int num36 = j + 1;
			if (num35 < 0)
			{
				if (!flag2)
				{
					continue;
				}
				num35 = cSDataArray.GetArrayCount() - 1;
			}
			if (num36 >= cSDataArray.GetArrayCount())
			{
				num36 = ((!flag2) ? j : 0);
			}
			double num37 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, num35).Center);
			double num38 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).GemXSize * 2.0;
			if (num37 > num38)
			{
				continue;
			}
			CSChannelGemData channelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, num35);
			CSChannelGemData channelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			CSChannelGemData channelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, num36);
			if (channelGemData2 == null || channelGemData3 == null)
			{
				return UpdateError("Invalid gems");
			}
			double num39 = cSDataArray.GetArrayCount();
			double vHeight2;
			double num40;
			double num41;
			if (j < cSDataArray.GetArrayCount() / 2)
			{
				vHeight2 = num3 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + num4 * ((double)j / (num39 / 2.0));
				num40 = num11 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + num12 * ((double)j / (num39 / 2.0));
				num41 = value2 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + value3 * ((double)j / (num39 / 2.0));
			}
			else
			{
				vHeight2 = num4 * (num39 - (double)j) / (double)j + num5 * (1.0 - (num39 - (double)j) / (double)j);
				num40 = num12 * (num39 - (double)j) / (double)j + num13 * (1.0 - (num39 - (double)j) / (double)j);
				num41 = value3 * (num39 - (double)j) / (double)j + value4 * (1.0 - (num39 - (double)j) / (double)j);
			}
			OnCircle circleLeft;
			OnCircle circleRight;
			double height3;
			string circles = GetCircles(channelGemData2, channelGemData3, channelGemData, crv, crv2, vNudgeEdge, vNudgeGem, vHeight2, num40, value, num41, out circleLeft, out circleRight, out height3);
			if (!string.IsNullOrEmpty(circles))
			{
				return UpdateError(circles);
			}
			if (_prongBase == ValueListItem.FollowLargestGem)
			{
				height3 = (value ? (num33 * num41) : num33);
			}
			On3dVector on3dVector3 = new On3dVector(circleLeft.Center());
			On3dVector on3dVector4 = new On3dVector(circleRight.Center());
			On3dPoint value5 = new On3dPoint((on3dVector3 + on3dVector4) / 2.0);
			On3dVector value6 = on3dVector4 - on3dVector3;
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
				if (num36 == j)
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
					string circles2 = GetCircles(channelGemData2, channelGemData3, channelGemData, crv, crv2, vNudgeEdge2, num10, vHeight2, num40, value, num41, out circleLeft2, out circleRight2, out height4);
					OnCircle circleLeft3;
					OnCircle circleRight3;
					double height5;
					string circles3 = GetCircles(channelGemData3, channelGemData4, channelGemData, crv, crv2, vNudgeEdge2, num10, vHeight2, num40, value, num41, out circleLeft3, out circleRight3, out height5);
					if (!string.IsNullOrEmpty(circles2) || !string.IsNullOrEmpty(circles3))
					{
						break;
					}
					double num42 = (circleRight3.radius + circleRight2.radius + circleLeft3.radius + circleLeft2.radius) / 4.0 + num32;
					double[] array = new double[4] { circleRight3.radius, circleRight2.radius, circleLeft2.radius, circleLeft3.radius };
					if (integer == 157)
					{
						double num43 = array[0];
						for (int k = 1; k < array.Length; k++)
						{
							if (num43 < array[k])
							{
								num43 = array[k];
							}
						}
						num42 = num43 + num32;
					}
					if (integer == 158)
					{
						double num44 = array[0];
						for (int l = 1; l < array.Length; l++)
						{
							if (num44 > array[l])
							{
								num44 = array[l];
							}
						}
						num42 = num44 + num32;
					}
					double num45 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData3.Size) / 2.0) : channelGemData3.GemZMin);
					Point3d value7 = channelGemData3.GetCurve().PointAt(t);
					value7.Transform(Transform.Translation(channelGemData3.Normal * (num40 - num45)));
					vector3d4.Unitize();
					vector3d4 *= num42 - num10;
					value7 += vector3d4;
					On3dPoint on3dPoint = CSConvert.Point3dCopyToOn3dPoint(value7);
					OnCircle onCircle2 = new OnCircle(new OnPlane(on3dPoint, channelGemData3.Normal.Convert()), on3dPoint, num42);
					OnXform onXform3 = new OnXform();
					onXform3.ChangeBasis(onCircle2.plane, OnUtil.On_xy_plane);
					double num46 = onCircle2.radius * 2.0;
					if (num46 > num && num46 < num2)
					{
						double bottomPercentage;
						double taperPer;
						double filletPer;
						if (j < cSDataArray.GetArrayCount() / 2)
						{
							bottomPercentage = num28 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + num29 * ((double)j / (num39 / 2.0));
							taperPer = percentage4 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + percentage5 * ((double)j / (num39 / 2.0));
							filletPer = percentage * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + percentage2 * ((double)j / (num39 / 2.0));
						}
						else
						{
							bottomPercentage = num29 * (num39 - (double)j) / (double)j + num30 * (1.0 - (num39 - (double)j) / (double)j);
							taperPer = percentage5 * (num39 - (double)j) / (double)j + percentage6 * (1.0 - (num39 - (double)j) / (double)j);
							filletPer = percentage2 * (num39 - (double)j) / (double)j + percentage3 * (1.0 - (num39 - (double)j) / (double)j);
						}
						if (!_prongCache.CreateNew(nurbs_curve, num46, height3, filletPer, taperPer, doMesh, out var prong, out var prongBaseRail, bottomPercentage))
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
				double radius3 = (radius + radius2) / 2.0;
				circleRight = new OnCircle(circleRight.Center(), radius3);
				circleLeft = new OnCircle(circleLeft.Center(), radius3);
			}
			if (!flag)
			{
				double num47 = circleLeft.radius * 2.0;
				if (num47 > num && num47 < num2)
				{
					double bottomPercentage2;
					double taperPer2;
					double filletPer2;
					if (j < cSDataArray.GetArrayCount() / 2)
					{
						bottomPercentage2 = num28 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + num29 * ((double)j / (num39 / 2.0));
						taperPer2 = percentage4 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + percentage5 * ((double)j / (num39 / 2.0));
						filletPer2 = percentage * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + percentage2 * ((double)j / (num39 / 2.0));
					}
					else
					{
						bottomPercentage2 = num29 * (num39 - (double)j) / (double)j + num30 * (1.0 - (num39 - (double)j) / (double)j);
						taperPer2 = percentage5 * (num39 - (double)j) / (double)j + percentage6 * (1.0 - (num39 - (double)j) / (double)j);
						filletPer2 = percentage2 * (num39 - (double)j) / (double)j + percentage3 * (1.0 - (num39 - (double)j) / (double)j);
					}
					if (!_prongCache.CreateNew(nurbs_curve, num47, height3, filletPer2, taperPer2, doMesh, out var prong2, out var prongBaseRail2, bottomPercentage2))
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
				double num48 = circleRight.radius * 2.0;
				if (num48 > num && num48 < num2)
				{
					double bottomPercentage3;
					double taperPer3;
					double filletPer3;
					if (j < cSDataArray.GetArrayCount() / 2)
					{
						bottomPercentage3 = num28 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + num29 * ((double)j / (num39 / 2.0));
						taperPer3 = percentage4 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + percentage5 * ((double)j / (num39 / 2.0));
						filletPer3 = percentage * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + percentage2 * ((double)j / (num39 / 2.0));
					}
					else
					{
						bottomPercentage3 = num29 * (num39 - (double)j) / (double)j + num30 * (1.0 - (num39 - (double)j) / (double)j);
						taperPer3 = percentage5 * (num39 - (double)j) / (double)j + percentage6 * (1.0 - (num39 - (double)j) / (double)j);
						filletPer3 = percentage2 * (num39 - (double)j) / (double)j + percentage3 * (1.0 - (num39 - (double)j) / (double)j);
					}
					if (!_prongCache.CreateNew(nurbs_curve, num48, height3, filletPer3, taperPer3, doMesh, out var prong3, out var prongBaseRail3, bottomPercentage3))
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
					baseDomeRadius = num22 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + num23 * ((double)j / (num39 / 2.0));
					blendEdge = num25 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + num26 * ((double)j / (num39 / 2.0));
					domeRadius = num19 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + num20 * ((double)j / (num39 / 2.0));
					bottomArcPercent = num28 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + num29 * ((double)j / (num39 / 2.0));
					filletPer4 = percentage4 * ((num39 / 2.0 - (double)j) / (num39 / 2.0)) + percentage5 * ((double)j / (num39 / 2.0));
				}
				else
				{
					baseDomeRadius = num23 * (num39 - (double)j) / (double)j + num24 * (1.0 - (num39 - (double)j) / (double)j);
					blendEdge = num26 * (num39 - (double)j) / (double)j + num27 * (1.0 - (num39 - (double)j) / (double)j);
					domeRadius = num20 * (num39 - (double)j) / (double)j + num21 * (1.0 - (num39 - (double)j) / (double)j);
					bottomArcPercent = num29 * (num39 - (double)j) / (double)j + num30 * (1.0 - (num39 - (double)j) / (double)j);
					filletPer4 = percentage5 * (num39 - (double)j) / (double)j + percentage6 * (1.0 - (num39 - (double)j) / (double)j);
				}
				Brep geometry = MakeBar((on3dVector3 - on3dVector4).Length(), circleLeft.radius, circleRight.radius, height3, filletPer4, domeRadius, baseDomeRadius, blendEdge, bottomArcPercent);
				value5 = new On3dPoint((on3dVector3 + on3dVector4) / 2.0);
				value6 = on3dVector4 - on3dVector3;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value5), CSConvert.On3dVectorCopyToVector3d(value6), CSConvert.On3dVectorCopyToVector3d(on3dVector7));
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
				int num49 = ((cSDataArray.GetArrayCount() > 1) ? (m + 1) : 0);
				int index = ((m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1));
				if (cSDataArray.GetArrayCount() > 1)
				{
					double num50;
					if (m != cSDataArray.GetArrayCount() - 1)
					{
						num50 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, m + 1).Center);
					}
					else
					{
						num50 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Center);
						num49 = 0;
					}
					double num51 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).GemXSize * 2.0;
					if (num50 <= num51)
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
						CSChannelGemData channelGemData7 = CSUtilGemRails.GetChannelGemData(cSDataArray, num49);
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
						if (num49 == m)
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
							double num52 = cSDataArray.GetArrayCount();
							double vHeight3;
							double num53;
							double percent;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight3 = num3 * ((num52 / 2.0 - (double)m) / (num52 / 2.0)) + num4 * ((double)m / (num52 / 2.0));
								num53 = num11 * ((num52 / 2.0 - (double)m) / (num52 / 2.0)) + num12 * ((double)m / (num52 / 2.0));
								percent = value2 * ((num52 / 2.0 - (double)m) / (num52 / 2.0)) + value3 * ((double)m / (num52 / 2.0));
							}
							else
							{
								vHeight3 = num4 * (num52 - (double)m) / (double)m + num5 * (1.0 - (num52 - (double)m) / (double)m);
								num53 = num12 * (num52 - (double)m) / (double)m + num13 * (1.0 - (num52 - (double)m) / (double)m);
								percent = value3 * (num52 - (double)m) / (double)m + value4 * (1.0 - (num52 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData5, channelGemData6, channelGemData, crv, crv2, vNudgeEdge2, num10, vHeight3, num53, value, percent, out var circleLeft4, out var circleRight4, out var height6)))
							{
								break;
							}
							double num54 = (circleRight4.radius + circleLeft4.radius) / 2.0 + num32;
							double[] array2 = new double[2] { circleRight4.radius, circleLeft4.radius };
							if (integer == 157)
							{
								double num55 = array2[0];
								for (int n = 1; n < array2.Length; n++)
								{
									if (num55 < array2[n])
									{
										num55 = array2[n];
									}
								}
								num54 = num55 + num32;
							}
							if (integer == 158)
							{
								double num56 = array2[0];
								for (int num57 = 1; num57 < array2.Length; num57++)
								{
									if (num56 > array2[num57])
									{
										num56 = array2[num57];
									}
								}
								num54 = num56 + num32;
							}
							double num58 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData6.Size) / 2.0) : channelGemData6.GemZMin);
							Point3d value8 = channelGemData6.GetCurve().PointAt(t2);
							value8.Transform(Transform.Translation(channelGemData6.Normal * (num53 - num58)));
							vector3d7.Unitize();
							vector3d7 *= num54 - num10;
							value8 += vector3d7;
							On3dPoint on3dPoint2 = CSConvert.Point3dCopyToOn3dPoint(value8);
							OnCircle onCircle3 = new OnCircle(new OnPlane(on3dPoint2, channelGemData6.Normal.Convert()), on3dPoint2, num54);
							OnXform onXform6 = new OnXform();
							onXform6.ChangeBasis(onCircle3.plane, OnUtil.On_xy_plane);
							double num59 = onCircle3.radius * 2.0;
							if (num59 > num && num59 < num2)
							{
								double bottomPercentage4;
								double taperPer4;
								double filletPer5;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									bottomPercentage4 = num28 * ((num52 / 2.0 - (double)m) / (num52 / 2.0)) + num29 * ((double)m / (num52 / 2.0));
									taperPer4 = percentage4 * ((num52 / 2.0 - (double)m) / (num52 / 2.0)) + percentage5 * ((double)m / (num52 / 2.0));
									filletPer5 = percentage * ((num52 / 2.0 - (double)m) / (num52 / 2.0)) + percentage2 * ((double)m / (num52 / 2.0));
								}
								else
								{
									bottomPercentage4 = num29 * (num52 - (double)m) / (double)m + num30 * (1.0 - (num52 - (double)m) / (double)m);
									taperPer4 = percentage5 * (num52 - (double)m) / (double)m + percentage6 * (1.0 - (num52 - (double)m) / (double)m);
									filletPer5 = percentage2 * (num52 - (double)m) / (double)m + percentage3 * (1.0 - (num52 - (double)m) / (double)m);
								}
								if (!_prongCache.CreateNew(nurbs_curve, num59, height6, filletPer5, taperPer4, doMesh, out var prong4, out var prongBaseRail4, bottomPercentage4))
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
						CSChannelGemData channelGemData9 = CSUtilGemRails.GetChannelGemData(cSDataArray, num49);
						CSChannelGemData channelGemData10 = CSUtilGemRails.GetChannelGemData(cSDataArray, (num49 + 1 != cSDataArray.GetArrayCount()) ? (num49 + 1) : 0);
						Vector3d vector3d8 = channelGemData9.Center - channelGemData8.Center;
						vector3d8.Unitize();
						Vector3d vector3d9 = channelGemData9.Center - channelGemData10.Center;
						vector3d9.Unitize();
						if (num49 == m)
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
							double num60 = cSDataArray.GetArrayCount();
							double vHeight4;
							double num61;
							double percent2;
							if (m < cSDataArray.GetArrayCount() / 2)
							{
								vHeight4 = num3 * ((num60 / 2.0 - (double)m) / (num60 / 2.0)) + num4 * ((double)m / (num60 / 2.0));
								num61 = num11 * ((num60 / 2.0 - (double)m) / (num60 / 2.0)) + num12 * ((double)m / (num60 / 2.0));
								percent2 = value2 * ((num60 / 2.0 - (double)m) / (num60 / 2.0)) + value3 * ((double)m / (num60 / 2.0));
							}
							else
							{
								vHeight4 = num4 * (num60 - (double)m) / (double)m + num5 * (1.0 - (num60 - (double)m) / (double)m);
								num61 = num12 * (num60 - (double)m) / (double)m + num13 * (1.0 - (num60 - (double)m) / (double)m);
								percent2 = value3 * (num60 - (double)m) / (double)m + value4 * (1.0 - (num60 - (double)m) / (double)m);
							}
							if (!string.IsNullOrEmpty(GetCircles(channelGemData9, channelGemData10, channelGemData, crv, crv2, vNudgeEdge2, num10, vHeight4, num61, value, percent2, out var circleLeft5, out var circleRight5, out var height7)))
							{
								break;
							}
							double num62 = (circleRight5.radius + circleLeft5.radius) / 2.0 + num32;
							double[] array3 = new double[2] { circleRight5.radius, circleLeft5.radius };
							if (integer == 157)
							{
								double num63 = array3[0];
								for (int num64 = 1; num64 < array3.Length; num64++)
								{
									if (num63 < array3[num64])
									{
										num63 = array3[num64];
									}
								}
								num62 = num63 + num32;
							}
							if (integer == 158)
							{
								double num65 = array3[0];
								for (int num66 = 1; num66 < array3.Length; num66++)
								{
									if (num65 > array3[num66])
									{
										num65 = array3[num66];
									}
								}
								num62 = num65 + num32;
							}
							double num67 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData9.Size) / 2.0) : channelGemData9.GemZMin);
							Point3d value9 = channelGemData9.GetCurve().PointAt(t3);
							value9.Transform(Transform.Translation(channelGemData9.Normal * (num61 - num67)));
							vector3d10.Unitize();
							vector3d10 *= num62 - num10;
							value9 += vector3d10;
							On3dPoint on3dPoint3 = CSConvert.Point3dCopyToOn3dPoint(value9);
							OnCircle onCircle4 = new OnCircle(new OnPlane(on3dPoint3, channelGemData9.Normal.Convert()), on3dPoint3, num62);
							OnXform onXform7 = new OnXform();
							onXform7.ChangeBasis(onCircle4.plane, OnUtil.On_xy_plane);
							double num68 = onCircle4.radius * 2.0;
							if (num68 > num && num68 < num2)
							{
								double bottomPercentage5;
								double taperPer5;
								double filletPer6;
								if (m < cSDataArray.GetArrayCount() / 2)
								{
									bottomPercentage5 = num28 * ((num60 / 2.0 - (double)m) / (num60 / 2.0)) + num29 * ((double)m / (num60 / 2.0));
									taperPer5 = percentage4 * ((num60 / 2.0 - (double)m) / (num60 / 2.0)) + percentage5 * ((double)m / (num60 / 2.0));
									filletPer6 = percentage * ((num60 / 2.0 - (double)m) / (num60 / 2.0)) + percentage2 * ((double)m / (num60 / 2.0));
								}
								else
								{
									bottomPercentage5 = num29 * (num60 - (double)m) / (double)m + num30 * (1.0 - (num60 - (double)m) / (double)m);
									taperPer5 = percentage5 * (num60 - (double)m) / (double)m + percentage6 * (1.0 - (num60 - (double)m) / (double)m);
									filletPer6 = percentage2 * (num60 - (double)m) / (double)m + percentage3 * (1.0 - (num60 - (double)m) / (double)m);
								}
								if (!_prongCache.CreateNew(nurbs_curve, num68, height7, filletPer6, taperPer5, doMesh, out var prong5, out var prongBaseRail5, bottomPercentage5))
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
				switch (valueListItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					CSChannelGemData cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, num49).Duplicate() as CSChannelGemData;
					if (cSDataArray.GetArrayCount() == 1)
					{
						cSChannelGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData.Normal, cSChannelGemData.Center));
					}
					double num69 = cSChannelGemData.GemXSize * percentage7 + num6 + num8;
					double height9 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + num3 - num11) : channelGemData.GemZSize);
					if (CSApplicationHelper.Application.IsCamMode)
					{
						CSChannelGemData gemPrev = cSChannelGemData;
						CSChannelGemData gemCurrent = CSUtilGemRails.GetChannelGemData(cSDataArray, (num49 + 1 != cSDataArray.GetArrayCount()) ? (num49 + 1) : 0).Duplicate() as CSChannelGemData;
						OnCircle circleLeft7;
						OnCircle circleRight7;
						double height10;
						string circles5 = GetCircles(gemPrev, gemCurrent, channelGemData, crv, crv2, num6, num8, num3, num11, value, value2, out circleLeft7, out circleRight7, out height10);
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
					if (!_prongCache.CreateNew(nurbs_curve, num69, height9, percentage, percentage4, doMesh, out var prong7, out var prongBaseRail7, num28))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint on3dPoint4 = new On3dPoint(cSChannelGemData.Center.Convert()) - new On3dVector(cSChannelGemData.ForwardVectorInGemPlane.Convert());
					double num70 = (cSChannelGemData.GemXSize * percentage7 + num6) / 2.0;
					double num71 = num11 - cSChannelGemData.GemZMin;
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
					OnPlane plane = new OnPlane(on3dPoint4 + on3dVector14 * num70 + on3dVector12 * num71, on3dVector13, on3dVector14);
					switch (valueListItem)
					{
					case ValueListItem.ProngPlacerStyleEnd4:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd7:
					{
						OnXform onXform18 = new OnXform();
						onXform18.ChangeBasis(plane, OnUtil.On_xy_plane);
						OnXform onXform19 = new OnXform();
						onXform19.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num14), cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
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
						double num72 = num15 / 2.0;
						OnXform onXform11 = new OnXform();
						onXform11.ChangeBasis(plane, OnUtil.On_xy_plane);
						OnXform onXform12 = new OnXform();
						onXform12.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num72), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform13 = new OnXform();
						onXform13.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num14 + num72), cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
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
						onXform15.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num72), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform16 = new OnXform();
						onXform16.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num14 - num72), cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
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
							Brep geometry3 = MakeBar((on3dPoint5 - on3dPoint6).Length(), num69 / 2.0, num69 / 2.0, height9, percentage4, num19, num22, num25, num28);
							On3dPoint value14 = new On3dPoint((on3dPoint5 + on3dPoint6) / 2.0);
							On3dVector value15 = on3dPoint6 - on3dPoint5;
							On3dVector value16 = on3dVector15 + on3dVector16;
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
					CSChannelGemData channelGemData11 = CSUtilGemRails.GetChannelGemData(cSDataArray, num49);
					CSChannelGemData channelGemData12 = CSUtilGemRails.GetChannelGemData(cSDataArray, num49 + 1);
					if (channelGemData11 == null || channelGemData12 == null)
					{
						return UpdateError("Invalid gems");
					}
					OnCircle circleLeft6;
					OnCircle circleRight6;
					double height8;
					string circles4 = GetCircles(channelGemData11, channelGemData12, channelGemData, crv, crv2, num6, num8, num3, num11, value, value2, out circleLeft6, out circleRight6, out height8);
					if (!string.IsNullOrEmpty(circles4))
					{
						return UpdateError(circles4);
					}
					On3dVector on3dVector8 = new On3dVector(circleLeft6.Center());
					On3dVector on3dVector9 = new On3dVector(circleRight6.Center());
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
						double radius4 = circleRight6.radius;
						double radius5 = circleLeft6.radius;
						double radius6 = (radius4 + radius5) / 2.0;
						circleRight6 = new OnCircle(circleRight6.Center(), radius6);
						circleLeft6 = new OnCircle(circleLeft6.Center(), radius6);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height8 = (value ? (num33 * value2) : num33);
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
					On3dVector value10 = on3dVector10 + on3dVector11;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height8 = GetCamHeightEnd(channelGemData11, channelGemData12, circleLeft6, circleRight6, on3dVector8, on3dVector9);
					}
					if (!flag)
					{
						if (!_prongCache.CreateNew(nurbs_curve, circleLeft6.Diameter(), height8, percentage, percentage4, doMesh, out var prong6, out var prongBaseRail6, num28))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value11 = on3dVector9 - on3dVector8;
						Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector8), CSConvert.On3dVectorCopyToVector3d(value11), CSConvert.On3dVectorCopyToVector3d(value10));
						Vector3d normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(prong6.Convert(), transform3 * transform2));
						if (!_prongCache.CreateNew(nurbs_curve, circleRight6.Diameter(), height8, percentage, percentage4, doMesh, out prong6, out prongBaseRail6, num28))
						{
							return UpdateError("Create prong cache failed");
						}
						value11 = on3dVector8 - on3dVector9;
						transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector9), CSConvert.On3dVectorCopyToVector3d(value11), CSConvert.On3dVectorCopyToVector3d(value10));
						normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(prong6.Convert(), transform3 * transform2));
					}
					else
					{
						Brep geometry2 = MakeBar((on3dVector8 - on3dVector9).Length(), circleLeft6.radius, circleRight6.radius, height8, percentage4, num19, num22, num25, num28);
						On3dPoint value12 = new On3dPoint((on3dVector8 + on3dVector9) / 2.0);
						On3dVector value13 = on3dVector9 - on3dVector8;
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
					double num73 = cSChannelGemData2.GemXSize * num16 + num7 + num9;
					double height12 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData2.GemZSize + num5 - num13) : channelGemData.GemZSize);
					if (CSApplicationHelper.Application.IsCamMode)
					{
						CSChannelGemData gemCurrent2 = cSChannelGemData2;
						CSChannelGemData gemPrev2 = CSUtilGemRails.GetChannelGemData(cSDataArray, (m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1)).Duplicate() as CSChannelGemData;
						OnCircle circleLeft9;
						OnCircle circleRight9;
						double height13;
						string circles7 = GetCircles(gemPrev2, gemCurrent2, channelGemData, crv, crv2, num7, num9, num5, num13, value, value4, out circleLeft9, out circleRight9, out height13);
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
					if (!_prongCache.CreateNew(nurbs_curve, num73, height12, percentage3, percentage6, doMesh, out var prong9, out var prongBaseRail9, num30))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint on3dPoint7 = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					double num74 = (cSChannelGemData2.GemXSize * num16 + num7) / 2.0;
					double num75 = num13 - cSChannelGemData2.GemZMin;
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
					OnPlane plane2 = new OnPlane(on3dPoint7 + on3dVector23 * num74 + on3dVector21 * num75, on3dVector22, on3dVector23);
					switch (valueListItem)
					{
					case ValueListItem.ProngPlacerStyleEnd2:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd8:
					{
						OnXform onXform30 = new OnXform();
						onXform30.ChangeBasis(plane2, OnUtil.On_xy_plane);
						OnXform onXform31 = new OnXform();
						onXform31.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num17), cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
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
						double num76 = num18 / 2.0;
						OnXform onXform23 = new OnXform();
						onXform23.ChangeBasis(plane2, OnUtil.On_xy_plane);
						OnXform onXform24 = new OnXform();
						onXform24.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num76), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform25 = new OnXform();
						onXform25.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num17 + num76), cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
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
						onXform27.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num76), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform28 = new OnXform();
						onXform28.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num17 - num76), cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
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
							Brep geometry5 = MakeBar((on3dPoint8 - on3dPoint9).Length(), num73 / 2.0, num73 / 2.0, height12, percentage6, num21, num24, num27, num30);
							On3dPoint value21 = new On3dPoint((on3dPoint8 + on3dPoint9) / 2.0);
							On3dVector value22 = on3dPoint9 - on3dPoint8;
							On3dVector value23 = on3dVector24 + on3dVector25;
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
					OnCircle circleLeft8;
					OnCircle circleRight8;
					double height11;
					string circles6 = GetCircles(channelGemData13, channelGemData14, channelGemData, crv, crv2, num7, num9, num5, num13, value, value4, out circleLeft8, out circleRight8, out height11);
					if (!string.IsNullOrEmpty(circles6))
					{
						return UpdateError(circles6);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height11 = (value ? (num33 * value4) : num33);
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
					On3dVector value17 = on3dVector19 + on3dVector20;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						height11 = GetCamHeightEnd(channelGemData13, channelGemData14, circleLeft8, circleRight8, on3dVector17, on3dVector18);
					}
					if (!flag)
					{
						if (!_prongCache.CreateNew(nurbs_curve, circleLeft8.Diameter(), height11, percentage3, percentage6, doMesh, out var prong8, out var prongBaseRail8, num30))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value18 = on3dVector18 - on3dVector17;
						Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector17), CSConvert.On3dVectorCopyToVector3d(value18), CSConvert.On3dVectorCopyToVector3d(value17));
						Vector3d normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(prong8.Convert(), transform8 * transform7));
						if (!_prongCache.CreateNew(nurbs_curve, circleRight8.Diameter(), height11, percentage3, percentage6, doMesh, out prong8, out prongBaseRail8, num30))
						{
							return UpdateError("Create prong cache failed");
						}
						value18 = on3dVector17 - on3dVector18;
						transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector18), CSConvert.On3dVectorCopyToVector3d(value18), CSConvert.On3dVectorCopyToVector3d(value17));
						normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(prong8.Convert(), transform8 * transform7));
					}
					else
					{
						Brep geometry4 = MakeBar((on3dVector17 - on3dVector18).Length(), circleLeft8.radius, circleRight8.radius, height11, percentage6, num21, num24, num27, num30);
						On3dPoint value19 = new On3dPoint((on3dVector17 + on3dVector18) / 2.0);
						On3dVector value20 = on3dVector18 - on3dVector17;
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

	private static string GetHeight(CSChannelGemData gemPrev, CSChannelGemData gemCurrent, CSChannelGemData gemMax, IOnCurve railLeft, IOnCurve railRight, double vNudgeEdge, double vNudgeGem, double vHeight, double vVerticalLocation, out double height)
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
