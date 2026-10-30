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
				double height2;
				string height = GetHeight(CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1), CSUtilGemRails.GetChannelGemData(cSDataArray, i), channelGemData, crv, crv2, value7, value8, value4, value9, out height2);
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
			int num2 = j - 1;
			if (num2 < 0)
			{
				if (!flag2)
				{
					continue;
				}
				num2 = cSDataArray.GetArrayCount() - 1;
			}
			CSChannelGemData channelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, num2);
			CSChannelGemData channelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			if (channelGemData2 == null || channelGemData3 == null)
			{
				return UpdateError("Invalid gems");
			}
			CSDataHelper.SetSettingType(channelGemData3, flag ? GemSettingType.BR : GemSettingType.PR);
			OnCircle circleLeft;
			OnCircle circleRight;
			double height3;
			string circles = GetCircles(channelGemData2, channelGemData3, channelGemData, crv, crv2, value7, value8, value4, value9, value22, value23, out circleLeft, out circleRight, out height3);
			if (!string.IsNullOrEmpty(circles))
			{
				return UpdateError(circles);
			}
			if (_prongBase == ValueListItem.FollowLargestGem)
			{
				height3 = (value22 ? (num * value23) : num);
			}
			On3dVector on3dVector3 = new On3dVector(circleLeft.Center());
			On3dVector on3dVector4 = new On3dVector(circleRight.Center());
			On3dPoint value24 = new On3dPoint((on3dVector3 + on3dVector4) / 2.0);
			On3dVector value25 = on3dVector4 - on3dVector3;
			if (CSApplicationHelper.Application.IsCamMode)
			{
				Circle circle = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData3.ZMaxPoint.DistanceTo(Point3d.Origin));
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
			OnXform onXform3 = new OnXform();
			onXform3.ChangeBasis(circleLeft.plane, OnUtil.On_xy_plane);
			On3dVector on3dVector5 = new On3dVector(OnUtil.On_zaxis);
			on3dVector5.Transform(onXform3);
			on3dVector5.Unitize();
			if (!flag)
			{
				double num3 = circleLeft.radius * 2.0;
				if (num3 > value2 && num3 < value3)
				{
					if (!_prongCache.Create(nurbs_curve, num3, height3, value5, value6, value, out var prong, out var prongBaseRail))
					{
						return UpdateError("Create prong cache failed");
					}
					prong.Transform(onXform3);
					prongBaseRail.Transform(onXform3);
					concurrentBag.Add(new CSOnGeometryData(prong.Convert(), onXform3.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(prongBaseRail.Convert(), onXform3.Convert(), applyTransform: false));
				}
			}
			OnXform onXform4 = new OnXform();
			onXform4.ChangeBasis(circleRight.plane, OnUtil.On_xy_plane);
			On3dVector on3dVector6 = new On3dVector(OnUtil.On_zaxis);
			on3dVector6.Transform(onXform4);
			on3dVector6.Unitize();
			if (!flag)
			{
				double num4 = circleRight.radius * 2.0;
				if (num4 > value2 && num4 < value3)
				{
					if (!_prongCache.Create(nurbs_curve, num4, height3, value5, value6, value, out var prong2, out var prongBaseRail2))
					{
						return UpdateError("Create prong cache failed");
					}
					prong2.Transform(onXform4);
					prongBaseRail2.Transform(onXform4);
					concurrentBag.Add(new CSOnGeometryData(prong2.Convert(), onXform4.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(prongBaseRail2.Convert(), onXform4.Convert(), applyTransform: false));
				}
			}
			On3dVector on3dVector7 = on3dVector5 + on3dVector6;
			on3dVector7.Unitize();
			if (flag)
			{
				on3dVector3 = new On3dVector(circleLeft.Center());
				on3dVector4 = new On3dVector(circleRight.Center());
				Brep geometry = MakeBar((on3dVector3 - on3dVector4).Length(), circleLeft.radius, circleRight.radius, height3, value6, value18, value19, value20, value21);
				value24 = new On3dPoint((on3dVector3 + on3dVector4) / 2.0);
				value25 = on3dVector4 - on3dVector3;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value24), CSConvert.On3dVectorCopyToVector3d(value25), CSConvert.On3dVectorCopyToVector3d(on3dVector7));
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
				double num5 = cSChannelGemData.GemXSize * value12 + value7 + value8;
				double height5 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create(nurbs_curve, num5, height5, value5, value6, value, out var prong4, out var prongBaseRail4))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint on3dPoint = new On3dPoint(cSChannelGemData.Center.Convert()) - new On3dVector(cSChannelGemData.ForwardVectorInGemPlane.Convert());
				double num6 = (cSChannelGemData.GemXSize * value12 + value7) / 2.0;
				double num7 = value9 - cSChannelGemData.GemZMin;
				On3dVector on3dVector12;
				if (on3dVector == OnUtil.On_UNSET_VECTOR)
				{
					on3dVector12 = new On3dVector(cSChannelGemData.Normal.Convert());
					on3dVector12.Unitize();
				}
				else
				{
					OnXform onXform7 = new OnXform();
					onXform7.Mirror(cSChannelGemData.Center.Convert(), cSChannelGemData.ForwardVectorInGemPlane.Convert());
					on3dVector12 = new On3dVector(on3dVector);
					on3dVector12.Transform(onXform7);
					on3dVector12.Unitize();
				}
				On3dVector on3dVector13 = new On3dVector(cSChannelGemData.LeftVector.Convert());
				on3dVector13.Unitize();
				On3dVector on3dVector14 = OnUtil.ON_CrossProduct(on3dVector12, on3dVector13);
				on3dVector14.Unitize();
				OnPlane plane = new OnPlane(on3dPoint + on3dVector14 * num6 + on3dVector12 * num7, on3dVector13, on3dVector14);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				{
					OnXform onXform15 = new OnXform();
					onXform15.ChangeBasis(plane, OnUtil.On_xy_plane);
					OnXform onXform16 = new OnXform();
					onXform16.Rotation(value13 * OnUtil.On_DEGREES_TO_RADIANS, cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
					OnXform xform3 = onXform16 * onXform15;
					prong4.Transform(xform3);
					prongBaseRail4.Transform(xform3);
					concurrentBag.Add(new CSOnGeometryData(prong4.Convert(), xform3.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(prongBaseRail4.Convert(), xform3.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num8 = value14 / 2.0;
					OnXform onXform8 = new OnXform();
					onXform8.ChangeBasis(plane, OnUtil.On_xy_plane);
					OnXform onXform9 = new OnXform();
					onXform9.Rotation((90.0 - num8) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform onXform10 = new OnXform();
					onXform10.Rotation((value13 + num8) * OnUtil.On_DEGREES_TO_RADIANS, cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
					OnXform onXform11 = onXform10 * onXform8 * onXform9;
					if (!flag)
					{
						OnGeometry onGeometry = prong4.Duplicate();
						onGeometry.Transform(onXform11);
						concurrentBag.Add(new CSOnGeometryData(onGeometry.Convert(), onXform11.Convert(), applyTransform: false));
						OnGeometry onGeometry2 = prongBaseRail4.Duplicate();
						onGeometry2.Transform(onXform11);
						concurrentBag2.Add(new CSOnGeometryData(onGeometry2.Convert(), onXform11.Convert(), applyTransform: false));
					}
					OnXform onXform12 = new OnXform();
					onXform12.Rotation((-90.0 + num8) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform onXform13 = new OnXform();
					onXform13.Rotation((value13 - num8) * OnUtil.On_DEGREES_TO_RADIANS, cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
					OnXform onXform14 = onXform13 * onXform8 * onXform12;
					if (!flag)
					{
						OnGeometry onGeometry3 = prong4.Duplicate();
						onGeometry3.Transform(onXform14);
						concurrentBag.Add(new CSOnGeometryData(onGeometry3.Convert(), onXform14.Convert(), applyTransform: false));
						OnGeometry onGeometry4 = prongBaseRail4.Duplicate();
						onGeometry4.Transform(onXform14);
						concurrentBag2.Add(new CSOnGeometryData(onGeometry4.Convert(), onXform14.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint on3dPoint2 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint on3dPoint3 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector on3dVector15 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector on3dVector16 = new On3dVector(0.0, 0.0, 1.0);
						on3dPoint2.Transform(onXform11);
						on3dVector15.Transform(onXform11);
						on3dPoint3.Transform(onXform14);
						on3dVector16.Transform(onXform14);
						Brep geometry3 = MakeBar((on3dPoint2 - on3dPoint3).Length(), num5 / 2.0, num5 / 2.0, height5, value6, value18, value19, value20, value21);
						On3dPoint value32 = new On3dPoint((on3dPoint2 + on3dPoint3) / 2.0);
						On3dVector value33 = on3dPoint3 - on3dPoint2;
						On3dVector value34 = on3dVector15 + on3dVector16;
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
				string circles2 = GetCircles(channelGemData4, channelGemData5, channelGemData, crv, crv2, value7, value8, value4, value9, value22, value23, out circleLeft2, out circleRight2, out height4);
				if (!string.IsNullOrEmpty(circles2))
				{
					return UpdateError(circles2);
				}
				On3dVector on3dVector8 = new On3dVector(circleLeft2.Center());
				On3dVector on3dVector9 = new On3dVector(circleRight2.Center());
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height4 = (value22 ? (num * value23) : num);
				}
				OnXform onXform5 = new OnXform();
				onXform5.ChangeBasis(circleLeft2.plane, OnUtil.On_xy_plane);
				On3dVector on3dVector10 = new On3dVector(OnUtil.On_zaxis);
				on3dVector10.Transform(onXform5);
				on3dVector10.Unitize();
				OnXform onXform6 = new OnXform();
				onXform6.ChangeBasis(circleRight2.plane, OnUtil.On_xy_plane);
				On3dVector on3dVector11 = new On3dVector(OnUtil.On_zaxis);
				on3dVector11.Transform(onXform6);
				on3dVector11.Unitize();
				On3dVector value26 = on3dVector10 + on3dVector11;
				if (CSApplicationHelper.Application.IsCamMode)
				{
					Circle circle2 = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData5.ZMaxPoint.DistanceTo(Point3d.Origin));
					On3dPoint value27 = new On3dPoint((on3dVector8 + on3dVector9) / 2.0);
					On3dVector value28 = on3dVector9 - on3dVector8;
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
					if (!_prongCache.Create(nurbs_curve, circleLeft2.Diameter(), height4, value5, value6, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value29 = on3dVector9 - on3dVector8;
					Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector8), CSConvert.On3dVectorCopyToVector3d(value29), CSConvert.On3dVectorCopyToVector3d(value26));
					Vector3d normalToMirrorPlane = channelGemData4.Center - channelGemData4.XMaxPoint;
					Transform transform3 = Transform.Mirror(channelGemData4.Center, normalToMirrorPlane);
					concurrentBag.Add(new CSOnGeometryData(prong3.Convert(), transform3 * transform2));
					if (!_prongCache.Create(nurbs_curve, circleRight2.Diameter(), height4, value5, value6, value, out prong3, out prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					value29 = on3dVector8 - on3dVector9;
					transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector9), CSConvert.On3dVectorCopyToVector3d(value29), CSConvert.On3dVectorCopyToVector3d(value26));
					normalToMirrorPlane = channelGemData4.Center - channelGemData4.XMaxPoint;
					transform3 = Transform.Mirror(channelGemData4.Center, normalToMirrorPlane);
					concurrentBag.Add(new CSOnGeometryData(prong3.Convert(), transform3 * transform2));
				}
				else
				{
					Brep geometry2 = MakeBar((on3dVector8 - on3dVector9).Length(), circleLeft2.radius, circleRight2.radius, height4, value6, value18, value19, value20, value21);
					On3dPoint value30 = new On3dPoint((on3dVector8 + on3dVector9) / 2.0);
					On3dVector value31 = on3dVector9 - on3dVector8;
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
				double num9 = cSChannelGemData2.GemXSize * value15 + value7 + value8;
				double height7 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData2.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create(nurbs_curve, num9, height7, value5, value6, value, out var prong6, out var prongBaseRail6))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint on3dPoint4 = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.ForwardVectorInGemPlane.Convert());
				double num10 = (cSChannelGemData2.GemXSize * value15 + value7) / 2.0;
				double num11 = value9 - cSChannelGemData2.GemZMin;
				On3dVector on3dVector21;
				if (on3dVector2 == OnUtil.On_UNSET_VECTOR)
				{
					on3dVector21 = new On3dVector(cSChannelGemData2.Normal.Convert());
					on3dVector21.Unitize();
				}
				else
				{
					OnXform onXform19 = new OnXform();
					onXform19.Mirror(cSChannelGemData2.Center.Convert(), cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					on3dVector21 = new On3dVector(on3dVector2);
					on3dVector21.Transform(onXform19);
					on3dVector21.Unitize();
				}
				On3dVector on3dVector22 = new On3dVector(cSChannelGemData2.RightVector.Convert());
				on3dVector22.Unitize();
				On3dVector on3dVector23 = OnUtil.ON_CrossProduct(on3dVector21, on3dVector22);
				on3dVector23.Unitize();
				OnPlane plane2 = new OnPlane(on3dPoint4 + on3dVector23 * num10 + on3dVector21 * num11, on3dVector22, on3dVector23);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd2:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd8:
				{
					OnXform onXform27 = new OnXform();
					onXform27.ChangeBasis(plane2, OnUtil.On_xy_plane);
					OnXform onXform28 = new OnXform();
					onXform28.Rotation(value16 * OnUtil.On_DEGREES_TO_RADIANS, cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
					OnXform xform5 = onXform28 * onXform27;
					prong6.Transform(xform5);
					prongBaseRail6.Transform(xform5);
					concurrentBag.Add(new CSOnGeometryData(prong6.Convert(), xform5.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(prongBaseRail6.Convert(), xform5.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd3:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num12 = value17 / 2.0;
					OnXform onXform20 = new OnXform();
					onXform20.ChangeBasis(plane2, OnUtil.On_xy_plane);
					OnXform onXform21 = new OnXform();
					onXform21.Rotation((90.0 + num12) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform onXform22 = new OnXform();
					onXform22.Rotation((value16 + num12) * OnUtil.On_DEGREES_TO_RADIANS, cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
					OnXform onXform23 = onXform22 * onXform20 * onXform21;
					if (!flag)
					{
						OnGeometry onGeometry5 = prong6.Duplicate();
						onGeometry5.Transform(onXform23);
						concurrentBag.Add(new CSOnGeometryData(onGeometry5.Convert(), onXform23.Convert(), applyTransform: false));
						OnGeometry onGeometry6 = prongBaseRail6.Duplicate();
						onGeometry6.Transform(onXform23);
						concurrentBag2.Add(new CSOnGeometryData(onGeometry6.Convert(), onXform23.Convert(), applyTransform: false));
					}
					OnXform onXform24 = new OnXform();
					onXform24.Rotation((-90.0 + num12) * OnUtil.On_DEGREES_TO_RADIANS, OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform onXform25 = new OnXform();
					onXform25.Rotation((value16 - num12) * OnUtil.On_DEGREES_TO_RADIANS, cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
					OnXform onXform26 = onXform25 * onXform20 * onXform24;
					if (!flag)
					{
						OnGeometry onGeometry7 = prong6.Duplicate();
						onGeometry7.Transform(onXform26);
						concurrentBag.Add(new CSOnGeometryData(onGeometry7.Convert(), onXform26.Convert(), applyTransform: false));
						OnGeometry onGeometry8 = prongBaseRail6.Duplicate();
						onGeometry8.Transform(onXform26);
						concurrentBag2.Add(new CSOnGeometryData(onGeometry8.Convert(), onXform26.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint on3dPoint5 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint on3dPoint6 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector on3dVector24 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector on3dVector25 = new On3dVector(0.0, 0.0, 1.0);
						on3dPoint5.Transform(onXform23);
						on3dVector24.Transform(onXform23);
						on3dPoint6.Transform(onXform26);
						on3dVector25.Transform(onXform26);
						Brep geometry5 = MakeBar((on3dPoint5 - on3dPoint6).Length(), num9 / 2.0, num9 / 2.0, height7, value6, value18, value19, value20, value21);
						On3dPoint value41 = new On3dPoint((on3dPoint5 + on3dPoint6) / 2.0);
						On3dVector value42 = on3dPoint6 - on3dPoint5;
						On3dVector value43 = on3dVector24 + on3dVector25;
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
				string circles3 = GetCircles(channelGemData6, channelGemData7, channelGemData, crv, crv2, value7, value8, value4, value9, value22, value23, out circleLeft3, out circleRight3, out height6);
				if (!string.IsNullOrEmpty(circles3))
				{
					return UpdateError(circles3);
				}
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height6 = (value22 ? (num * value23) : num);
				}
				On3dVector on3dVector17 = new On3dVector(circleLeft3.Center());
				On3dVector on3dVector18 = new On3dVector(circleRight3.Center());
				OnXform onXform17 = new OnXform();
				onXform17.ChangeBasis(circleLeft3.plane, OnUtil.On_xy_plane);
				On3dVector on3dVector19 = new On3dVector(OnUtil.On_zaxis);
				on3dVector19.Transform(onXform17);
				on3dVector19.Unitize();
				OnXform onXform18 = new OnXform();
				onXform18.ChangeBasis(circleRight3.plane, OnUtil.On_xy_plane);
				On3dVector on3dVector20 = new On3dVector(OnUtil.On_zaxis);
				on3dVector20.Transform(onXform18);
				on3dVector20.Unitize();
				On3dVector value35 = on3dVector19 + on3dVector20;
				if (CSApplicationHelper.Application.IsCamMode)
				{
					Circle circle3 = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData7.ZMaxPoint.DistanceTo(Point3d.Origin));
					On3dPoint value36 = new On3dPoint((on3dVector17 + on3dVector18) / 2.0);
					On3dVector value37 = on3dVector18 - on3dVector17;
					Transform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value36), CSConvert.On3dVectorCopyToVector3d(value37), CSConvert.On3dVectorCopyToVector3d(value35));
					Point3d origin3 = Point3d.Origin;
					origin3.Transform(xform4);
					Line line3 = new Line(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
					line3.Transform(xform4);
					Point3d point3d3 = line3.ClosestPoint(circle3.ClosestPoint(line3.PointAt(1.0)), limitToFiniteSegment: true);
					if (point3d3.DistanceTo(origin3) > height6 * 0.8)
					{
						height6 = point3d3.DistanceTo(origin3) * 1.2;
					}
				}
				if (!flag)
				{
					if (!_prongCache.Create(nurbs_curve, circleLeft3.Diameter(), height6, value5, value6, value, out var prong5, out var prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value38 = on3dVector18 - on3dVector17;
					Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector17), CSConvert.On3dVectorCopyToVector3d(value38), CSConvert.On3dVectorCopyToVector3d(value35));
					Vector3d normalToMirrorPlane3 = channelGemData7.Center - channelGemData7.XMaxPoint;
					Transform transform8 = Transform.Mirror(channelGemData7.Center, normalToMirrorPlane3);
					concurrentBag.Add(new CSOnGeometryData(prong5.Convert(), transform8 * transform7));
					if (!_prongCache.Create(nurbs_curve, circleRight3.Diameter(), height6, value5, value6, value, out prong5, out prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					value38 = on3dVector17 - on3dVector18;
					transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector18), CSConvert.On3dVectorCopyToVector3d(value38), CSConvert.On3dVectorCopyToVector3d(value35));
					normalToMirrorPlane3 = channelGemData7.Center - channelGemData7.XMaxPoint;
					transform8 = Transform.Mirror(channelGemData7.Center, normalToMirrorPlane3);
					concurrentBag.Add(new CSOnGeometryData(prong5.Convert(), transform8 * transform7));
				}
				else
				{
					Brep geometry4 = MakeBar((on3dVector17 - on3dVector18).Length(), circleLeft3.radius, circleRight3.radius, height6, value6, value18, value19, value20, value21);
					On3dPoint value39 = new On3dPoint((on3dVector17 + on3dVector18) / 2.0);
					On3dVector value40 = on3dVector18 - on3dVector17;
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
