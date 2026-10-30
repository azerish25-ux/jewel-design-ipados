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
				string height = GetHeight(CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1), CSUtilGemRails.GetChannelGemData(cSDataArray, i), channelGemData, crv, crv2, value7, value8, value4, value13, out height2);
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
			int num2 = j - 1;
			int num3 = j + 1;
			if (num2 < 0)
			{
				if (!flag2)
				{
					continue;
				}
				num2 = cSDataArray.GetArrayCount() - 1;
			}
			if (num3 >= cSDataArray.GetArrayCount())
			{
				num3 = ((!flag2) ? j : 0);
			}
			double num4 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, num2).Center);
			double num5 = CSUtilGemRails.GetChannelGemData(cSDataArray, j).GemXSize * 2.0;
			if (num4 > num5)
			{
				continue;
			}
			CSChannelGemData channelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, num2);
			CSChannelGemData channelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			CSChannelGemData channelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, num3);
			if (channelGemData2 == null || channelGemData3 == null)
			{
				return UpdateError("Invalid gems");
			}
			CSDataHelper.SetSettingType(channelGemData3, flag ? GemSettingType.BR : GemSettingType.PR);
			OnCircle circleLeft;
			OnCircle circleRight;
			double height3;
			string circles = GetCircles(channelGemData2, channelGemData3, channelGemData, crv, crv2, value7, value8, value4, value13, value30, value31, out circleLeft, out circleRight, out height3);
			if (!string.IsNullOrEmpty(circles))
			{
				return UpdateError(circles);
			}
			if (_prongBase == ValueListItem.FollowLargestGem)
			{
				height3 = (value30 ? (num * value31) : num);
			}
			On3dVector on3dVector3 = new On3dVector(circleLeft.Center());
			On3dVector on3dVector4 = new On3dVector(circleRight.Center());
			On3dPoint value32 = new On3dPoint((on3dVector3 + on3dVector4) / 2.0);
			On3dVector value33 = on3dVector4 - on3dVector3;
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
				if (num3 == j)
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
					string circles2 = GetCircles(channelGemData2, channelGemData3, channelGemData, crv, crv2, value11, value12, value4, value13, value30, value31, out circleLeft2, out circleRight2, out height4);
					OnCircle circleLeft3;
					OnCircle circleRight3;
					double height5;
					string circles3 = GetCircles(channelGemData3, channelGemData4, channelGemData, crv, crv2, value11, value12, value4, value13, value30, value31, out circleLeft3, out circleRight3, out height5);
					if (!string.IsNullOrEmpty(circles2) || !string.IsNullOrEmpty(circles3))
					{
						break;
					}
					double num6 = (circleRight3.radius + circleRight2.radius + circleLeft3.radius + circleLeft2.radius) / 4.0 + value28;
					double[] array = new double[4] { circleRight3.radius, circleRight2.radius, circleLeft2.radius, circleLeft3.radius };
					if (value27 == 157)
					{
						double num7 = array[0];
						for (int k = 1; k < array.Length; k++)
						{
							if (num7 < array[k])
							{
								num7 = array[k];
							}
						}
						num6 = num7 + value28;
					}
					if (value27 == 158)
					{
						double num8 = array[0];
						for (int l = 1; l < array.Length; l++)
						{
							if (num8 > array[l])
							{
								num8 = array[l];
							}
						}
						num6 = num8 + value28;
					}
					double num9 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData3.Size) / 2.0) : channelGemData3.GemZMin);
					Point3d value34 = channelGemData3.GetCurve().PointAt(t);
					value34.Transform(Transform.Translation(channelGemData3.Normal * (value13 - num9)));
					vector3d3.Unitize();
					vector3d3 *= num6 - value12;
					value34 += vector3d3;
					On3dPoint on3dPoint = CSConvert.Point3dCopyToOn3dPoint(value34);
					OnCircle onCircle2 = new OnCircle(new OnPlane(on3dPoint, channelGemData3.Normal.Convert()), on3dPoint, num6);
					OnXform onXform3 = new OnXform();
					onXform3.ChangeBasis(onCircle2.plane, OnUtil.On_xy_plane);
					double num10 = onCircle2.radius * 2.0;
					if (num10 > value2 && num10 < value3)
					{
						if (!_prongCache.Create(nurbs_curve, num10, height3, value5, value6, value, out var prong, out var prongBaseRail))
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
				double radius3 = (radius + radius2) / 2.0;
				circleRight = new OnCircle(circleRight.Center(), radius3);
				circleLeft = new OnCircle(circleLeft.Center(), radius3);
			}
			if (!flag)
			{
				double num11 = circleLeft.radius * 2.0;
				if (num11 > value2 && num11 < value3)
				{
					if (!_prongCache.Create(nurbs_curve, num11, height3, value5, value6, value, out var prong2, out var prongBaseRail2))
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
				double num12 = circleRight.radius * 2.0;
				if (num12 > value2 && num12 < value3)
				{
					if (!_prongCache.Create(nurbs_curve, num12, height3, value5, value6, value, out var prong3, out var prongBaseRail3))
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
				Brep geometry = MakeBar((on3dVector3 - on3dVector4).Length(), circleLeft.radius, circleRight.radius, height3, value6, value22, value23, value24, value25);
				value32 = new On3dPoint((on3dVector3 + on3dVector4) / 2.0);
				value33 = on3dVector4 - on3dVector3;
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value32), CSConvert.On3dVectorCopyToVector3d(value33), CSConvert.On3dVectorCopyToVector3d(on3dVector7));
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
				int num13 = ((cSDataArray.GetArrayCount() > 1) ? (m + 1) : 0);
				int index = ((m - 1 > 0) ? (m - 1) : (cSDataArray.GetArrayCount() - 1));
				if (cSDataArray.GetArrayCount() > 1)
				{
					double num14;
					if (m != cSDataArray.GetArrayCount() - 1)
					{
						num14 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, m + 1).Center);
					}
					else
					{
						num14 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).Center.DistanceTo(CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Center);
						num13 = 0;
					}
					double num15 = CSUtilGemRails.GetChannelGemData(cSDataArray, m).GemXSize * 2.0;
					if (num14 <= num15)
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
						CSChannelGemData channelGemData7 = CSUtilGemRails.GetChannelGemData(cSDataArray, num13);
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
						if (num13 == m)
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
							if (!string.IsNullOrEmpty(GetCircles(channelGemData5, channelGemData6, channelGemData, crv, crv2, value11, value12, value4, value13, value30, value31, out var circleLeft4, out var circleRight4, out var height6)))
							{
								break;
							}
							double num16 = (circleRight4.radius + circleLeft4.radius) / 2.0 + value28;
							double[] array2 = new double[2] { circleRight4.radius, circleLeft4.radius };
							if (value27 == 157)
							{
								double num17 = array2[0];
								for (int n = 1; n < array2.Length; n++)
								{
									if (num17 < array2[n])
									{
										num17 = array2[n];
									}
								}
								num16 = num17 + value28;
							}
							if (value27 == 158)
							{
								double num18 = array2[0];
								for (int num19 = 1; num19 < array2.Length; num19++)
								{
									if (num18 > array2[num19])
									{
										num18 = array2[num19];
									}
								}
								num16 = num18 + value28;
							}
							double num20 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData6.Size) / 2.0) : channelGemData6.GemZMin);
							Point3d value35 = channelGemData6.GetCurve().PointAt(t2);
							value35.Transform(Transform.Translation(channelGemData6.Normal * (value13 - num20)));
							vector3d6.Unitize();
							vector3d6 *= num16 - value12;
							value35 += vector3d6;
							On3dPoint on3dPoint2 = CSConvert.Point3dCopyToOn3dPoint(value35);
							OnCircle onCircle3 = new OnCircle(new OnPlane(on3dPoint2, channelGemData6.Normal.Convert()), on3dPoint2, num16);
							OnXform onXform6 = new OnXform();
							onXform6.ChangeBasis(onCircle3.plane, OnUtil.On_xy_plane);
							double num21 = onCircle3.radius * 2.0;
							if (num21 > value2 && num21 < value3)
							{
								if (!_prongCache.Create(nurbs_curve, num21, height6, value5, value6, value, out var prong4, out var prongBaseRail4))
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
						CSChannelGemData channelGemData9 = CSUtilGemRails.GetChannelGemData(cSDataArray, num13);
						CSChannelGemData channelGemData10 = CSUtilGemRails.GetChannelGemData(cSDataArray, (num13 + 1 != cSDataArray.GetArrayCount()) ? (num13 + 1) : 0);
						Vector3d vector3d7 = channelGemData9.Center - channelGemData8.Center;
						vector3d7.Unitize();
						Vector3d vector3d8 = channelGemData9.Center - channelGemData10.Center;
						vector3d8.Unitize();
						if (num13 == m)
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
							if (!string.IsNullOrEmpty(GetCircles(channelGemData9, channelGemData10, channelGemData, crv, crv2, value11, value12, value4, value13, value30, value31, out var circleLeft5, out var circleRight5, out var height7)))
							{
								break;
							}
							double num22 = (circleRight5.radius + circleLeft5.radius) / 2.0 + value28;
							double[] array3 = new double[2] { circleRight5.radius, circleLeft5.radius };
							if (value27 == 157)
							{
								double num23 = array3[0];
								for (int num24 = 1; num24 < array3.Length; num24++)
								{
									if (num23 < array3[num24])
									{
										num23 = array3[num24];
									}
								}
								num22 = num23 + value28;
							}
							if (value27 == 158)
							{
								double num25 = array3[0];
								for (int num26 = 1; num26 < array3.Length; num26++)
								{
									if (num25 > array3[num26])
									{
										num25 = array3[num26];
									}
								}
								num22 = num25 + value28;
							}
							double num27 = ((_prongBase == ValueListItem.FollowLargestGem) ? (channelGemData.GemZMin + (channelGemData.Size - channelGemData9.Size) / 2.0) : channelGemData9.GemZMin);
							Point3d value36 = channelGemData9.GetCurve().PointAt(t3);
							value36.Transform(Transform.Translation(channelGemData9.Normal * (value13 - num27)));
							vector3d9.Unitize();
							vector3d9 *= num22 - value12;
							value36 += vector3d9;
							On3dPoint on3dPoint3 = CSConvert.Point3dCopyToOn3dPoint(value36);
							OnCircle onCircle4 = new OnCircle(new OnPlane(on3dPoint3, channelGemData9.Normal.Convert()), on3dPoint3, num22);
							OnXform onXform7 = new OnXform();
							onXform7.ChangeBasis(onCircle4.plane, OnUtil.On_xy_plane);
							double num28 = onCircle4.radius * 2.0;
							if (num28 > value2 && num28 < value3)
							{
								if (!_prongCache.Create(nurbs_curve, num28, height7, value5, value6, value, out var prong5, out var prongBaseRail5))
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
					CSChannelGemData cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, num13).Duplicate() as CSChannelGemData;
					if (cSDataArray.GetArrayCount() == 1)
					{
						cSChannelGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), cSChannelGemData.Normal, cSChannelGemData.Center));
					}
					double num29 = cSChannelGemData.GemXSize * value16 + value9 + value10;
					double height9 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData.GemZSize + value4 - value13) : channelGemData.GemZSize);
					if (!_prongCache.Create(nurbs_curve, num29, height9, value5, value6, value, out var prong7, out var prongBaseRail7))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint on3dPoint4 = new On3dPoint(cSChannelGemData.Center.Convert()) - new On3dVector(cSChannelGemData.ForwardVectorInGemPlane.Convert());
					double num30 = (cSChannelGemData.GemXSize * value16 + value9) / 2.0;
					double num31 = value13 - cSChannelGemData.GemZMin;
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
					OnPlane plane = new OnPlane(on3dPoint4 + on3dVector14 * num30 + on3dVector12 * num31, on3dVector13, on3dVector14);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd4:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd7:
					{
						OnXform onXform18 = new OnXform();
						onXform18.ChangeBasis(plane, OnUtil.On_xy_plane);
						OnXform onXform19 = new OnXform();
						onXform19.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value17), cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
						OnXform xform3 = onXform19 * onXform18;
						prong7.Transform(xform3);
						prongBaseRail7.Transform(xform3);
						concurrentBag.Add(new CSOnGeometryData(prong7.Convert(), xform3.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(prongBaseRail7.Convert(), xform3.Convert(), applyTransform: false));
						break;
					}
					case ValueListItem.ProngPlacerStyleEnd5:
					case ValueListItem.ProngPlacerStyleEnd8:
					case ValueListItem.ProngPlacerStyleEnd9:
					{
						double num32 = value18 / 2.0;
						OnXform onXform11 = new OnXform();
						onXform11.ChangeBasis(plane, OnUtil.On_xy_plane);
						OnXform onXform12 = new OnXform();
						onXform12.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 - num32), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform13 = new OnXform();
						onXform13.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value17 + num32), cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
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
						onXform15.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num32), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform16 = new OnXform();
						onXform16.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value17 - num32), cSChannelGemData.Normal.Convert(), cSChannelGemData.Center.Convert());
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
							Brep geometry3 = MakeBar((on3dPoint5 - on3dPoint6).Length(), num29 / 2.0, num29 / 2.0, height9, value6, value22, value23, value24, value25);
							On3dPoint value43 = new On3dPoint((on3dPoint5 + on3dPoint6) / 2.0);
							On3dVector value44 = on3dPoint6 - on3dPoint5;
							On3dVector value45 = on3dVector15 + on3dVector16;
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
					CSChannelGemData channelGemData11 = CSUtilGemRails.GetChannelGemData(cSDataArray, num13);
					CSChannelGemData channelGemData12 = CSUtilGemRails.GetChannelGemData(cSDataArray, num13 + 1);
					if (channelGemData11 == null || channelGemData12 == null)
					{
						return UpdateError("Invalid gems");
					}
					CSDataHelper.SetSettingType(channelGemData12, flag ? GemSettingType.BR : GemSettingType.PR);
					OnCircle circleLeft6;
					OnCircle circleRight6;
					double height8;
					string circles4 = GetCircles(channelGemData11, channelGemData12, channelGemData, crv, crv2, value9, value10, value4, value13, value30, value31, out circleLeft6, out circleRight6, out height8);
					if (!string.IsNullOrEmpty(circles4))
					{
						return UpdateError(circles4);
					}
					On3dVector on3dVector8 = new On3dVector(circleLeft6.Center());
					On3dVector on3dVector9 = new On3dVector(circleRight6.Center());
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
						double radius4 = circleRight6.radius;
						double radius5 = circleLeft6.radius;
						double radius6 = (radius4 + radius5) / 2.0;
						circleRight6 = new OnCircle(circleRight6.Center(), radius6);
						circleLeft6 = new OnCircle(circleLeft6.Center(), radius6);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height8 = (value30 ? (num * value31) : num);
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
					On3dVector value37 = on3dVector10 + on3dVector11;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						Circle circle = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData12.ZMaxPoint.DistanceTo(Point3d.Origin));
						On3dPoint value38 = new On3dPoint((on3dVector8 + on3dVector9) / 2.0);
						On3dVector value39 = on3dVector9 - on3dVector8;
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
						if (!_prongCache.Create(nurbs_curve, circleLeft6.Diameter(), height8, value5, value6, value, out var prong6, out var prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value40 = on3dVector9 - on3dVector8;
						Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector8), CSConvert.On3dVectorCopyToVector3d(value40), CSConvert.On3dVectorCopyToVector3d(value37));
						Vector3d normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						Transform transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(prong6.Convert(), transform3 * transform2));
						if (!_prongCache.Create(nurbs_curve, circleRight6.Diameter(), height8, value5, value6, value, out prong6, out prongBaseRail6))
						{
							return UpdateError("Create prong cache failed");
						}
						value40 = on3dVector8 - on3dVector9;
						transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector9), CSConvert.On3dVectorCopyToVector3d(value40), CSConvert.On3dVectorCopyToVector3d(value37));
						normalToMirrorPlane = channelGemData11.Center - channelGemData11.XMaxPoint;
						transform3 = Transform.Mirror(channelGemData11.Center, normalToMirrorPlane);
						concurrentBag.Add(new CSOnGeometryData(prong6.Convert(), transform3 * transform2));
					}
					else
					{
						Brep geometry2 = MakeBar((on3dVector8 - on3dVector9).Length(), circleLeft6.radius, circleRight6.radius, height8, value6, value22, value23, value24, value25);
						On3dPoint value41 = new On3dPoint((on3dVector8 + on3dVector9) / 2.0);
						On3dVector value42 = on3dVector9 - on3dVector8;
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
					double num33 = cSChannelGemData2.GemXSize * value19 + value9 + value10;
					double height11 = ((_prongBase == ValueListItem.FollowCulet) ? (cSChannelGemData2.GemZSize + value4 - value13) : channelGemData.GemZSize);
					if (!_prongCache.Create(nurbs_curve, num33, height11, value5, value6, value, out var prong9, out var prongBaseRail9))
					{
						return UpdateError("Create prong cache failed");
					}
					On3dPoint on3dPoint7 = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.ForwardVectorInGemPlane.Convert());
					double num34 = (cSChannelGemData2.GemXSize * value19 + value9) / 2.0;
					double num35 = value13 - cSChannelGemData2.GemZMin;
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
					OnPlane plane2 = new OnPlane(on3dPoint7 + on3dVector23 * num34 + on3dVector21 * num35, on3dVector22, on3dVector23);
					switch (whichItem)
					{
					case ValueListItem.ProngPlacerStyleEnd2:
					case ValueListItem.ProngPlacerStyleEnd6:
					case ValueListItem.ProngPlacerStyleEnd8:
					{
						OnXform onXform30 = new OnXform();
						onXform30.ChangeBasis(plane2, OnUtil.On_xy_plane);
						OnXform onXform31 = new OnXform();
						onXform31.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value20), cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
						OnXform xform5 = onXform31 * onXform30;
						prong9.Transform(xform5);
						prongBaseRail9.Transform(xform5);
						concurrentBag.Add(new CSOnGeometryData(prong9.Convert(), xform5.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(prongBaseRail9.Convert(), xform5.Convert(), applyTransform: false));
						break;
					}
					case ValueListItem.ProngPlacerStyleEnd3:
					case ValueListItem.ProngPlacerStyleEnd7:
					case ValueListItem.ProngPlacerStyleEnd9:
					{
						double num36 = value21 / 2.0;
						OnXform onXform23 = new OnXform();
						onXform23.ChangeBasis(plane2, OnUtil.On_xy_plane);
						OnXform onXform24 = new OnXform();
						onXform24.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0 + num36), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform25 = new OnXform();
						onXform25.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value20 + num36), cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
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
						onXform27.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0 + num36), OnUtil.On_zaxis, OnUtil.On_origin);
						OnXform onXform28 = new OnXform();
						onXform28.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value20 - num36), cSChannelGemData2.Normal.Convert(), cSChannelGemData2.Center.Convert());
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
							Brep geometry5 = MakeBar((on3dPoint8 - on3dPoint9).Length(), num33 / 2.0, num33 / 2.0, height11, value6, value22, value23, value24, value25);
							On3dPoint value52 = new On3dPoint((on3dPoint8 + on3dPoint9) / 2.0);
							On3dVector value53 = on3dPoint9 - on3dPoint8;
							On3dVector value54 = on3dVector24 + on3dVector25;
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
					string circles5 = GetCircles(channelGemData13, channelGemData14, channelGemData, crv, crv2, value9, value10, value4, value13, value30, value31, out circleLeft7, out circleRight7, out height10);
					if (!string.IsNullOrEmpty(circles5))
					{
						return UpdateError(circles5);
					}
					if (_prongBase == ValueListItem.FollowLargestGem)
					{
						height10 = (value30 ? (num * value31) : num);
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
						double radius7 = circleRight7.radius;
						double radius8 = circleLeft7.radius;
						double radius9 = (radius7 + radius8) / 2.0;
						circleRight7 = new OnCircle(circleRight7.Center(), radius9);
						circleLeft7 = new OnCircle(circleLeft7.Center(), radius9);
					}
					On3dVector on3dVector17 = new On3dVector(circleLeft7.Center());
					On3dVector on3dVector18 = new On3dVector(circleRight7.Center());
					OnXform onXform20 = new OnXform();
					onXform20.ChangeBasis(circleLeft7.plane, OnUtil.On_xy_plane);
					On3dVector on3dVector19 = new On3dVector(OnUtil.On_zaxis);
					on3dVector19.Transform(onXform20);
					on3dVector19.Unitize();
					OnXform onXform21 = new OnXform();
					onXform21.ChangeBasis(circleRight7.plane, OnUtil.On_xy_plane);
					On3dVector on3dVector20 = new On3dVector(OnUtil.On_zaxis);
					on3dVector20.Transform(onXform21);
					on3dVector20.Unitize();
					On3dVector value46 = on3dVector19 + on3dVector20;
					if (CSApplicationHelper.Application.IsCamMode)
					{
						Circle circle2 = new Circle(Plane.WorldZX, Point3d.Origin, channelGemData14.ZMaxPoint.DistanceTo(Point3d.Origin));
						On3dPoint value47 = new On3dPoint((on3dVector17 + on3dVector18) / 2.0);
						On3dVector value48 = on3dVector18 - on3dVector17;
						Transform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(value47), CSConvert.On3dVectorCopyToVector3d(value48), CSConvert.On3dVectorCopyToVector3d(value46));
						Point3d origin3 = Point3d.Origin;
						origin3.Transform(xform4);
						Line line3 = new Line(Point3d.Origin, new Point3d(0.0, 0.0, 6.0));
						line3.Transform(xform4);
						Point3d point3d3 = line3.ClosestPoint(circle2.ClosestPoint(line3.PointAt(1.0)), limitToFiniteSegment: true);
						if (point3d3.DistanceTo(origin3) > height10 * 0.8)
						{
							height10 = point3d3.DistanceTo(origin3) * 1.2;
						}
					}
					if (!flag)
					{
						if (!_prongCache.Create(nurbs_curve, circleLeft7.Diameter(), height10, value5, value6, value, out var prong8, out var prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						On3dVector value49 = on3dVector18 - on3dVector17;
						Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector17), CSConvert.On3dVectorCopyToVector3d(value49), CSConvert.On3dVectorCopyToVector3d(value46));
						Vector3d normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						Transform transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(prong8.Convert(), transform8 * transform7));
						if (!_prongCache.Create(nurbs_curve, circleRight7.Diameter(), height10, value5, value6, value, out prong8, out prongBaseRail8))
						{
							return UpdateError("Create prong cache failed");
						}
						value49 = on3dVector17 - on3dVector18;
						transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, CSConvert.On3dPointCopyToPoint3d(on3dVector18), CSConvert.On3dVectorCopyToVector3d(value49), CSConvert.On3dVectorCopyToVector3d(value46));
						normalToMirrorPlane3 = channelGemData14.Center - channelGemData14.XMaxPoint;
						transform8 = Transform.Mirror(channelGemData14.Center, normalToMirrorPlane3);
						concurrentBag.Add(new CSOnGeometryData(prong8.Convert(), transform8 * transform7));
					}
					else
					{
						Brep geometry4 = MakeBar((on3dVector17 - on3dVector18).Length(), circleLeft7.radius, circleRight7.radius, height10, value6, value22, value23, value24, value25);
						On3dPoint value50 = new On3dPoint((on3dVector17 + on3dVector18) / 2.0);
						On3dVector value51 = on3dVector18 - on3dVector17;
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
