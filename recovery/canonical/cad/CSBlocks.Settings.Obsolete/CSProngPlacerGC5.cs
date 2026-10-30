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
			string circles = GetCircles(channelGemData2, channelGemData3, channelGemData, crv, crv2, value7, value8, value4, value9, value18, value19, out circleLeft, out circleRight, out height3);
			if (!string.IsNullOrEmpty(circles))
			{
				return UpdateError(circles);
			}
			if (_prongBase == ValueListItem.FollowLargestGem)
			{
				height3 = (value18 ? (num * value19) : num);
			}
			if (CSApplicationHelper.Application.IsCamMode)
			{
				double num3 = 4.0 * height3 / 5.0;
				Point3d point3d = CSConvert.On3dPointCopyToPoint3d(circleLeft.Center());
				if (channelGemData3.ZMaxPoint.DistanceTo(Point3d.Origin) > point3d.DistanceTo(Point3d.Origin) + num3)
				{
					height3 += channelGemData3.ZMaxPoint.DistanceTo(Point3d.Origin) - (point3d.DistanceTo(Point3d.Origin) + num3);
				}
			}
			OnXform onXform = new OnXform();
			onXform.ChangeBasis(circleLeft.plane, OnUtil.On_xy_plane);
			On3dVector on3dVector3 = new On3dVector(OnUtil.On_zaxis);
			on3dVector3.Transform(onXform);
			on3dVector3.Unitize();
			if (!flag)
			{
				double num4 = circleLeft.radius * 2.0;
				if (num4 > value2 && num4 < value3)
				{
					if (!_prongCache.Create(nurbs_curve, num4, height3, value5, value6, value, out var prong, out var prongBaseRail))
					{
						return UpdateError("Create prong cache failed");
					}
					prong.Transform(onXform);
					prongBaseRail.Transform(onXform);
					concurrentBag.Add(new CSOnGeometryData(prong.Convert(), onXform.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(prongBaseRail.Convert(), onXform.Convert(), applyTransform: false));
				}
			}
			OnXform onXform2 = new OnXform();
			onXform2.ChangeBasis(circleRight.plane, OnUtil.On_xy_plane);
			On3dVector on3dVector4 = new On3dVector(OnUtil.On_zaxis);
			on3dVector4.Transform(onXform2);
			on3dVector4.Unitize();
			if (!flag)
			{
				double num5 = circleRight.radius * 2.0;
				if (num5 > value2 && num5 < value3)
				{
					if (!_prongCache.Create(nurbs_curve, num5, height3, value5, value6, value, out var prong2, out var prongBaseRail2))
					{
						return UpdateError("Create prong cache failed");
					}
					prong2.Transform(onXform2);
					prongBaseRail2.Transform(onXform2);
					concurrentBag.Add(new CSOnGeometryData(prong2.Convert(), onXform2.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(prongBaseRail2.Convert(), onXform2.Convert(), applyTransform: false));
				}
			}
			On3dVector on3dVector5 = on3dVector3 + on3dVector4;
			on3dVector5.Unitize();
			if (flag)
			{
				On3dVector on3dVector6 = new On3dVector(circleLeft.Center());
				On3dVector on3dVector7 = new On3dVector(circleRight.Center());
				Brep geometry = MakeBar((on3dVector6 - on3dVector7).Length(), circleLeft.radius, circleRight.radius, height3, Math.Min(circleLeft.radius, circleRight.radius) * value5, value6);
				On3dPoint value20 = new On3dPoint((on3dVector6 + on3dVector7) / 2.0);
				On3dVector value21 = on3dVector7 - on3dVector6;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value20), CSConvert.On3dVectorCopyToVector3d(value21), CSConvert.On3dVectorCopyToVector3d(on3dVector5));
				concurrentBag.Add(new CSOnGeometryData(geometry, transform));
			}
			if (on3dVector == OnUtil.On_UNSET_VECTOR)
			{
				on3dVector = on3dVector5;
			}
			on3dVector2 = on3dVector5;
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
				double num7 = channelGemData6.GemXSize * value12 + value7 + value8;
				double height5 = ((_prongBase == ValueListItem.FollowCulet) ? (channelGemData6.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create(nurbs_curve, num7, height5, value5, value6, value, out var prong4, out var prongBaseRail4))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint on3dPoint = new On3dPoint(channelGemData6.Center.Convert()) - new On3dVector(channelGemData6.ForwardVectorInGemPlane.Convert());
				double num8 = (channelGemData6.GemXSize * value12 + value7) / 2.0;
				double num9 = value9 - channelGemData6.GemZMin;
				On3dVector on3dVector12;
				if (on3dVector == OnUtil.On_UNSET_VECTOR)
				{
					on3dVector12 = new On3dVector(channelGemData6.Normal.Convert());
					on3dVector12.Unitize();
				}
				else
				{
					OnXform onXform5 = new OnXform();
					onXform5.Mirror(channelGemData6.Center.Convert(), channelGemData6.ForwardVectorInGemPlane.Convert());
					on3dVector12 = new On3dVector(on3dVector);
					on3dVector12.Transform(onXform5);
					on3dVector12.Unitize();
				}
				On3dVector on3dVector13 = new On3dVector(channelGemData6.LeftVector.Convert());
				on3dVector13.Unitize();
				On3dVector on3dVector14 = OnUtil.ON_CrossProduct(on3dVector12, on3dVector13);
				on3dVector14.Unitize();
				OnPlane plane = new OnPlane(on3dPoint + on3dVector14 * num8 + on3dVector12 * num9, on3dVector13, on3dVector14);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd4:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd7:
				{
					OnXform onXform13 = new OnXform();
					onXform13.ChangeBasis(plane, OnUtil.On_xy_plane);
					OnXform onXform14 = new OnXform();
					onXform14.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13), channelGemData6.Normal.Convert(), channelGemData6.Center.Convert());
					OnXform xform = onXform14 * onXform13;
					prong4.Transform(xform);
					prongBaseRail4.Transform(xform);
					concurrentBag.Add(new CSOnGeometryData(prong4.Convert(), xform.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(prongBaseRail4.Convert(), xform.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd5:
				case ValueListItem.ProngPlacerStyleEnd8:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num10 = value14 / 2.0;
					OnXform onXform6 = new OnXform();
					onXform6.ChangeBasis(plane, OnUtil.On_xy_plane);
					OnXform onXform7 = new OnXform();
					onXform7.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num10), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform onXform8 = new OnXform();
					onXform8.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 + num10), channelGemData6.Normal.Convert(), channelGemData6.Center.Convert());
					OnXform onXform9 = onXform8 * onXform6 * onXform7;
					if (!flag)
					{
						OnGeometry onGeometry = prong4.Duplicate();
						onGeometry.Transform(onXform9);
						concurrentBag.Add(new CSOnGeometryData(onGeometry.Convert(), onXform9.Convert(), applyTransform: false));
						OnGeometry onGeometry2 = prongBaseRail4.Duplicate();
						onGeometry2.Transform(onXform9);
						concurrentBag2.Add(new CSOnGeometryData(onGeometry2.Convert(), onXform9.Convert(), applyTransform: false));
					}
					OnXform onXform10 = new OnXform();
					onXform10.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num10), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform onXform11 = new OnXform();
					onXform11.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value13 - num10), channelGemData6.Normal.Convert(), channelGemData6.Center.Convert());
					OnXform onXform12 = onXform11 * onXform6 * onXform10;
					if (!flag)
					{
						OnGeometry onGeometry3 = prong4.Duplicate();
						onGeometry3.Transform(onXform12);
						concurrentBag.Add(new CSOnGeometryData(onGeometry3.Convert(), onXform12.Convert(), applyTransform: false));
						OnGeometry onGeometry4 = prongBaseRail4.Duplicate();
						onGeometry4.Transform(onXform12);
						concurrentBag2.Add(new CSOnGeometryData(onGeometry4.Convert(), onXform12.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint on3dPoint2 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint on3dPoint3 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector on3dVector15 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector on3dVector16 = new On3dVector(0.0, 0.0, 1.0);
						on3dPoint2.Transform(onXform9);
						on3dVector15.Transform(onXform9);
						on3dPoint3.Transform(onXform12);
						on3dVector16.Transform(onXform12);
						Brep geometry3 = MakeBar((on3dPoint2 - on3dPoint3).Length(), num7 / 2.0, num7 / 2.0, height5, num7 / 2.0 * value5, value6);
						On3dPoint value26 = new On3dPoint((on3dPoint2 + on3dPoint3) / 2.0);
						On3dVector value27 = on3dPoint3 - on3dPoint2;
						On3dVector value28 = on3dVector15 + on3dVector16;
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
				string circles2 = GetCircles(channelGemData4, channelGemData5, channelGemData, crv, crv2, value7, value8, value4, value9, value18, value19, out circleLeft2, out circleRight2, out height4);
				if (!string.IsNullOrEmpty(circles2))
				{
					return UpdateError(circles2);
				}
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height4 = (value18 ? (num * value19) : num);
				}
				if (CSApplicationHelper.Application.IsCamMode)
				{
					double num6 = 4.0 * height4 / 5.0;
					Point3d point3d2 = CSConvert.On3dPointCopyToPoint3d(circleLeft2.Center());
					if (channelGemData5.ZMaxPoint.DistanceTo(Point3d.Origin) > point3d2.DistanceTo(Point3d.Origin) + num6)
					{
						height4 += channelGemData5.ZMaxPoint.DistanceTo(Point3d.Origin) - (point3d2.DistanceTo(Point3d.Origin) + num6);
					}
				}
				OnXform onXform3 = new OnXform();
				onXform3.ChangeBasis(circleLeft2.plane, OnUtil.On_xy_plane);
				On3dVector on3dVector8 = new On3dVector(OnUtil.On_zaxis);
				on3dVector8.Transform(onXform3);
				on3dVector8.Unitize();
				OnXform onXform4 = new OnXform();
				onXform4.ChangeBasis(circleRight2.plane, OnUtil.On_xy_plane);
				On3dVector on3dVector9 = new On3dVector(OnUtil.On_zaxis);
				on3dVector9.Transform(onXform4);
				on3dVector9.Unitize();
				On3dVector value22 = on3dVector8 + on3dVector9;
				On3dVector on3dVector10 = new On3dVector(circleLeft2.Center());
				On3dVector on3dVector11 = new On3dVector(circleRight2.Center());
				if (!flag)
				{
					if (!_prongCache.Create(nurbs_curve, circleLeft2.Diameter(), height4, value5, value6, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value23 = on3dVector11 - on3dVector10;
					Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector10), CSConvert.On3dVectorCopyToVector3d(value23), CSConvert.On3dVectorCopyToVector3d(value22));
					Vector3d normalToMirrorPlane = channelGemData4.Center - channelGemData4.XMaxPoint;
					Transform transform3 = Transform.Mirror(channelGemData4.Center, normalToMirrorPlane);
					concurrentBag.Add(new CSOnGeometryData(prong3.Convert(), transform3 * transform2));
					if (!_prongCache.Create(nurbs_curve, circleRight2.Diameter(), height4, value5, value6, value, out prong3, out prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					value23 = on3dVector10 - on3dVector11;
					transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector11), CSConvert.On3dVectorCopyToVector3d(value23), CSConvert.On3dVectorCopyToVector3d(value22));
					normalToMirrorPlane = channelGemData4.Center - channelGemData4.XMaxPoint;
					transform3 = Transform.Mirror(channelGemData4.Center, normalToMirrorPlane);
					concurrentBag.Add(new CSOnGeometryData(prong3.Convert(), transform3 * transform2));
				}
				else
				{
					Brep geometry2 = MakeBar((on3dVector10 - on3dVector11).Length(), circleLeft2.radius, circleRight2.radius, height4, Math.Min(circleLeft2.radius, circleRight2.radius) * value5, value6);
					On3dPoint value24 = new On3dPoint((on3dVector10 + on3dVector11) / 2.0);
					On3dVector value25 = on3dVector11 - on3dVector10;
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
				double num12 = channelGemData9.GemXSize * value15 + value7 + value8;
				double height7 = ((_prongBase == ValueListItem.FollowCulet) ? (channelGemData9.GemZSize + value4 - value9) : channelGemData.GemZSize);
				if (!_prongCache.Create(nurbs_curve, num12, height7, value5, value6, value, out var prong6, out var prongBaseRail6))
				{
					return UpdateError("Create prong cache failed");
				}
				On3dPoint on3dPoint4 = new On3dPoint(channelGemData9.Center.Convert()) + new On3dVector(channelGemData9.ForwardVectorInGemPlane.Convert());
				double num13 = (channelGemData9.GemXSize * value15 + value7) / 2.0;
				double num14 = value9 - channelGemData9.GemZMin;
				On3dVector on3dVector21;
				if (on3dVector2 == OnUtil.On_UNSET_VECTOR)
				{
					on3dVector21 = new On3dVector(channelGemData9.Normal.Convert());
					on3dVector21.Unitize();
				}
				else
				{
					OnXform onXform17 = new OnXform();
					onXform17.Mirror(channelGemData9.Center.Convert(), channelGemData9.ForwardVectorInGemPlane.Convert());
					on3dVector21 = new On3dVector(on3dVector2);
					on3dVector21.Transform(onXform17);
					on3dVector21.Unitize();
				}
				On3dVector on3dVector22 = new On3dVector(channelGemData9.RightVector.Convert());
				on3dVector22.Unitize();
				On3dVector on3dVector23 = OnUtil.ON_CrossProduct(on3dVector21, on3dVector22);
				on3dVector23.Unitize();
				OnPlane plane2 = new OnPlane(on3dPoint4 + on3dVector23 * num13 + on3dVector21 * num14, on3dVector22, on3dVector23);
				switch (whichItem)
				{
				case ValueListItem.ProngPlacerStyleEnd2:
				case ValueListItem.ProngPlacerStyleEnd6:
				case ValueListItem.ProngPlacerStyleEnd8:
				{
					OnXform onXform25 = new OnXform();
					onXform25.ChangeBasis(plane2, OnUtil.On_xy_plane);
					OnXform onXform26 = new OnXform();
					onXform26.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16), channelGemData9.Normal.Convert(), channelGemData9.Center.Convert());
					OnXform xform2 = onXform26 * onXform25;
					prong6.Transform(xform2);
					prongBaseRail6.Transform(xform2);
					concurrentBag.Add(new CSOnGeometryData(prong6.Convert(), xform2.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(prongBaseRail6.Convert(), xform2.Convert(), applyTransform: false));
					break;
				}
				case ValueListItem.ProngPlacerStyleEnd3:
				case ValueListItem.ProngPlacerStyleEnd7:
				case ValueListItem.ProngPlacerStyleEnd9:
				{
					double num15 = value17 / 2.0;
					OnXform onXform18 = new OnXform();
					onXform18.ChangeBasis(plane2, OnUtil.On_xy_plane);
					OnXform onXform19 = new OnXform();
					onXform19.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num15), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform onXform20 = new OnXform();
					onXform20.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 + num15), channelGemData9.Normal.Convert(), channelGemData9.Center.Convert());
					OnXform onXform21 = onXform20 * onXform18 * onXform19;
					if (!flag)
					{
						OnGeometry onGeometry5 = prong6.Duplicate();
						onGeometry5.Transform(onXform21);
						concurrentBag.Add(new CSOnGeometryData(onGeometry5.Convert(), onXform21.Convert(), applyTransform: false));
						OnGeometry onGeometry6 = prongBaseRail6.Duplicate();
						onGeometry6.Transform(onXform21);
						concurrentBag2.Add(new CSOnGeometryData(onGeometry6.Convert(), onXform21.Convert(), applyTransform: false));
					}
					OnXform onXform22 = new OnXform();
					onXform22.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num15), OnUtil.On_zaxis, OnUtil.On_origin);
					OnXform onXform23 = new OnXform();
					onXform23.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value16 - num15), channelGemData9.Normal.Convert(), channelGemData9.Center.Convert());
					OnXform onXform24 = onXform23 * onXform18 * onXform22;
					if (!flag)
					{
						OnGeometry onGeometry7 = prong6.Duplicate();
						onGeometry7.Transform(onXform24);
						concurrentBag.Add(new CSOnGeometryData(onGeometry7.Convert(), onXform24.Convert(), applyTransform: false));
						OnGeometry onGeometry8 = prongBaseRail6.Duplicate();
						onGeometry8.Transform(onXform24);
						concurrentBag2.Add(new CSOnGeometryData(onGeometry8.Convert(), onXform24.Convert(), applyTransform: false));
					}
					if (flag)
					{
						On3dPoint on3dPoint5 = new On3dPoint(0.0, 0.0, 0.0);
						On3dPoint on3dPoint6 = new On3dPoint(0.0, 0.0, 0.0);
						On3dVector on3dVector24 = new On3dVector(0.0, 0.0, 1.0);
						On3dVector on3dVector25 = new On3dVector(0.0, 0.0, 1.0);
						on3dPoint5.Transform(onXform21);
						on3dVector24.Transform(onXform21);
						on3dPoint6.Transform(onXform24);
						on3dVector25.Transform(onXform24);
						Brep geometry5 = MakeBar((on3dPoint5 - on3dPoint6).Length(), num12 / 2.0, num12 / 2.0, height7, num12 / 2.0 * value5, value6);
						On3dPoint value33 = new On3dPoint((on3dPoint5 + on3dPoint6) / 2.0);
						On3dVector value34 = on3dPoint6 - on3dPoint5;
						On3dVector value35 = on3dVector24 + on3dVector25;
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
				string circles3 = GetCircles(channelGemData7, channelGemData8, channelGemData, crv, crv2, value7, value8, value4, value9, value18, value19, out circleLeft3, out circleRight3, out height6);
				if (!string.IsNullOrEmpty(circles3))
				{
					return UpdateError(circles3);
				}
				if (_prongBase == ValueListItem.FollowLargestGem)
				{
					height6 = (value18 ? (num * value19) : num);
				}
				if (CSApplicationHelper.Application.IsCamMode)
				{
					double num11 = 4.0 * height6 / 5.0;
					Point3d point3d3 = CSConvert.On3dPointCopyToPoint3d(circleLeft3.Center());
					if (channelGemData8.ZMaxPoint.DistanceTo(Point3d.Origin) > point3d3.DistanceTo(Point3d.Origin) + num11)
					{
						height6 += channelGemData8.ZMaxPoint.DistanceTo(Point3d.Origin) - (point3d3.DistanceTo(Point3d.Origin) + num11);
					}
				}
				OnXform onXform15 = new OnXform();
				onXform15.ChangeBasis(circleLeft3.plane, OnUtil.On_xy_plane);
				On3dVector on3dVector17 = new On3dVector(OnUtil.On_zaxis);
				on3dVector17.Transform(onXform15);
				on3dVector17.Unitize();
				OnXform onXform16 = new OnXform();
				onXform16.ChangeBasis(circleRight3.plane, OnUtil.On_xy_plane);
				On3dVector on3dVector18 = new On3dVector(OnUtil.On_zaxis);
				on3dVector18.Transform(onXform16);
				on3dVector18.Unitize();
				On3dVector value29 = on3dVector17 + on3dVector18;
				On3dVector on3dVector19 = new On3dVector(circleLeft3.Center());
				On3dVector on3dVector20 = new On3dVector(circleRight3.Center());
				if (!flag)
				{
					if (!_prongCache.Create(nurbs_curve, circleLeft3.Diameter(), height6, value5, value6, value, out var prong5, out var prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dVector value30 = on3dVector20 - on3dVector19;
					Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector19), CSConvert.On3dVectorCopyToVector3d(value30), CSConvert.On3dVectorCopyToVector3d(value29));
					Vector3d normalToMirrorPlane3 = channelGemData8.Center - channelGemData8.XMaxPoint;
					Transform transform8 = Transform.Mirror(channelGemData8.Center, normalToMirrorPlane3);
					concurrentBag.Add(new CSOnGeometryData(prong5.Convert(), transform8 * transform7));
					if (!_prongCache.Create(nurbs_curve, circleRight3.Diameter(), height6, value5, value6, value, out prong5, out prongBaseRail5))
					{
						return UpdateError("Create prong cache failed");
					}
					value30 = on3dVector19 - on3dVector20;
					transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector20), CSConvert.On3dVectorCopyToVector3d(value30), CSConvert.On3dVectorCopyToVector3d(value29));
					normalToMirrorPlane3 = channelGemData8.Center - channelGemData8.XMaxPoint;
					transform8 = Transform.Mirror(channelGemData8.Center, normalToMirrorPlane3);
					concurrentBag.Add(new CSOnGeometryData(prong5.Convert(), transform8 * transform7));
				}
				else
				{
					Brep geometry4 = MakeBar((on3dVector19 - on3dVector20).Length(), circleLeft3.radius, circleRight3.radius, height6, Math.Min(circleLeft3.radius, circleRight3.radius) * value5, value6);
					On3dPoint value31 = new On3dPoint((on3dVector19 + on3dVector20) / 2.0);
					On3dVector value32 = on3dVector20 - on3dVector19;
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
