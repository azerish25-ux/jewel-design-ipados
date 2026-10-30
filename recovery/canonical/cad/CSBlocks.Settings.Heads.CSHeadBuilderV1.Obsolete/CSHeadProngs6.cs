using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadProngs_V6", "{37D61A46-44B6-49B3-AE19-13BF2DAFF72F}")]
public sealed class CSHeadProngs6 : CSBlock
{
	public CSInputChannel iGems { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iProngLines { get; }

	public CSInputChannel iHeadTop { get; }

	public CSInputChannel iHeadBase { get; }

	public CSInputChannel iEnableVProngs { get; }

	public CSInputChannel iProngTopProfile { get; }

	public CSInputChannel iProngTopX { get; }

	public CSInputChannel iProngTopY { get; }

	public CSInputChannel iProngTopNudge { get; }

	public CSInputChannel iProngVTopAngle { get; }

	public CSInputChannel iEnableBase { get; }

	public CSInputChannel iProngBaseProfile { get; }

	public CSInputChannel iProngBaseX { get; }

	public CSInputChannel iProngBaseY { get; }

	public CSInputChannel iProngBaseNudge { get; }

	public CSInputChannel iProngVBaseAngle { get; }

	public CSInputChannel iObjects { get; }

	public CSInputChannel iEnableModeGameChanger { get; }

	public CSInputChannel iProngDomeHeight { get; }

	public CSInputChannel iProngDomeSettingDistance { get; }

	public CSInputChannel iProngDomeSettingAngle { get; }

	public CSInputChannel iBaseToPoint { get; }

	public CSInputChannel iEnableModeDoItYourself { get; }

	public CSOutputChannel oObjects { get; }

	public CSOutputChannel oCurves { get; }

	public CSHeadProngs6(string identifier)
		: base(identifier)
	{
		iGems = new CSInputChannel(GetPropertyName(() => iGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngLines = new CSInputChannel(GetPropertyName(() => iProngLines), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iHeadTop = new CSInputChannel(GetPropertyName(() => iHeadTop), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iHeadBase = new CSInputChannel(GetPropertyName(() => iHeadBase), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iEnableVProngs = new CSInputChannel(GetPropertyName(() => iEnableVProngs), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProngTopProfile = new CSInputChannel(GetPropertyName(() => iProngTopProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProngTopX = new CSInputChannel(GetPropertyName(() => iProngTopX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngTopY = new CSInputChannel(GetPropertyName(() => iProngTopY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngTopNudge = new CSInputChannel(GetPropertyName(() => iProngTopNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVTopAngle = new CSInputChannel(GetPropertyName(() => iProngVTopAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableBase = new CSInputChannel(GetPropertyName(() => iEnableBase), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProngBaseProfile = new CSInputChannel(GetPropertyName(() => iProngBaseProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProngBaseX = new CSInputChannel(GetPropertyName(() => iProngBaseX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngBaseY = new CSInputChannel(GetPropertyName(() => iProngBaseY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngBaseNudge = new CSInputChannel(GetPropertyName(() => iProngBaseNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVBaseAngle = new CSInputChannel(GetPropertyName(() => iProngVBaseAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iObjects = new CSInputChannel(GetPropertyName(() => iObjects), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iEnableModeGameChanger = new CSInputChannel(GetPropertyName(() => iEnableModeGameChanger), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProngDomeHeight = new CSInputChannel(GetPropertyName(() => iProngDomeHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngDomeSettingDistance = new CSInputChannel(GetPropertyName(() => iProngDomeSettingDistance), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngDomeSettingAngle = new CSInputChannel(GetPropertyName(() => iProngDomeSettingAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBaseToPoint = new CSInputChannel(GetPropertyName(() => iBaseToPoint), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iEnableModeDoItYourself = new CSInputChannel(GetPropertyName(() => iEnableModeDoItYourself), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
		oCurves = new CSOutputChannel(GetPropertyName(() => oCurves), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		OnNurbsCurve nurbsCurve = iProngTopProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Prong Top Profile");
		}
		ICSArray iCSArray = iObjects.Data as ICSArray;
		double value = 2.0;
		iProngTopX.GetDouble(ref value);
		double value2 = 2.0;
		iProngTopY.GetDouble(ref value2);
		double value3 = 0.0;
		iProngTopNudge.GetDouble(ref value3);
		double value4 = 0.0;
		iProngVTopAngle.GetDouble(ref value4);
		double num = 0.0 - value2 * value3;
		int value5 = 0;
		iStyle.GetInteger(ref value5);
		bool value6 = false;
		iEnableVProngs.GetBoolean(ref value6);
		bool value7 = false;
		iEnableBase.GetBoolean(ref value7);
		bool value8 = false;
		iEnableModeGameChanger.GetBoolean(ref value8);
		bool value9 = false;
		iEnableModeDoItYourself.GetBoolean(ref value9);
		double value10 = 2.0;
		double value11 = 2.0;
		double value12 = 0.0;
		double value13 = 0.0;
		OnNurbsCurve onNurbsCurve;
		double num2;
		if (value7)
		{
			onNurbsCurve = iProngBaseProfile.GetNurbsCurve();
			if (onNurbsCurve == null)
			{
				return UpdateMissing("Prong Base Profile");
			}
			iProngBaseX.GetDouble(ref value10);
			iProngBaseY.GetDouble(ref value11);
			iProngBaseNudge.GetDouble(ref value12);
			iProngVBaseAngle.GetDouble(ref value13);
			num2 = 0.0 - value11 * value12;
		}
		else
		{
			onNurbsCurve = nurbsCurve.Duplicate();
			value10 = value;
			value11 = value2;
			value12 = value3;
			value13 = value4;
			num2 = num;
		}
		double value14 = 0.5;
		iProngDomeHeight.GetDouble(ref value14);
		double value15 = 0.0;
		iProngDomeSettingDistance.GetDouble(ref value15);
		double value16 = 0.0;
		iProngDomeSettingAngle.GetDouble(ref value16);
		bool value17 = false;
		iBaseToPoint.GetBoolean(ref value17);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				return UpdateMissing("Gem Missing");
			}
			if (!cSGemData.GetGemXformRC(out var _, out var xUp))
			{
				continue;
			}
			OnNurbsCurve onNurbsCurve2 = iHeadTop.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve2 == null)
			{
				return UpdateMissing("HeadTop Missing");
			}
			cSDataArray2.Append(new CSOnGeometryData(onNurbsCurve2.Convert(), xUp));
			OnNurbsCurve onNurbsCurve3 = iHeadBase.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve3 == null)
			{
				return UpdateMissing("HeadBase Missing");
			}
			cSDataArray2.Append(new CSOnGeometryData(onNurbsCurve3.Convert(), xUp));
			if (!(iProngLines.GetArrayItem(i) is ICSArray iCSArray2))
			{
				return UpdateMissing("ProngLines Missing");
			}
			CSDataArray cSDataArray3 = new CSDataArray();
			if (iCSArray != null && iCSArray.GetArrayCount() > 0)
			{
				if (!(iCSArray.GetArrayItem(i) is ICSArray iCSArray3))
				{
					continue;
				}
				for (int j = 0; j < iCSArray3.GetArrayCount(); j++)
				{
					cSDataArray3.Append(iCSArray3.GetArrayItem(j));
				}
			}
			List<CSHeadStyles2.HeadProngInfo> list = (value8 ? CSHeadStyles2.GetHeadStyleGameChanger(cSGemData.GemShape, value5, cSGemData.VersionMajor) : CSHeadStyles2.GetHeadStyle(cSGemData.GemShape, value5, cSGemData.VersionMajor));
			if (list == null)
			{
				return UpdateError("Head Style Invalid");
			}
			for (int k = 0; k < list.Count; k++)
			{
				OnNurbsCurve prongRail = iCSArray2.GetArrayItem(k).GetNurbsCurve().Convert();
				if (prongRail == null)
				{
					CS.CSUtil.PrintLine("Invalid Head Crv");
					continue;
				}
				cSDataArray2.Append(new CSOnGeometryData(prongRail.Convert(), xUp));
				On3dPoint on3dPoint = prongRail.PointAtStart();
				double t = 0.0;
				onNurbsCurve2.GetClosestPoint(on3dPoint, ref t);
				On3dPoint on3dPoint2 = prongRail.PointAtEnd();
				double t2 = 0.0;
				onNurbsCurve3.GetClosestPoint(on3dPoint2, ref t2);
				if (list[k].Type == CSHeadStyles2.EHeadProngType.Straight && value6)
				{
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(onNurbsCurve2, t, value4, value2, num, out var vProng))
					{
						return UpdateError("Error Making V Prong Top");
					}
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(onNurbsCurve3, t2, value13, value11, num2, out var vProng2))
					{
						return UpdateError("Error Making V Prong Top");
					}
					OnNurbsCurve nurbs_curve = new OnNurbsCurve();
					vProng2.GetNurbForm(ref nurbs_curve);
					On3dPoint on3dPoint3 = prongRail.PointAtStart();
					On3dVector on3dVector = prongRail.TangentAt(prongRail.Domain().Min());
					on3dVector.Unitize();
					On3dPoint on3dPoint4 = on3dPoint3 - on3dVector * 0.5;
					OnXform onXform = new OnXform();
					onXform.Translation(on3dPoint4 - on3dPoint3);
					OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
					vProng.GetNurbForm(ref nurbs_curve2);
					nurbs_curve2.Transform(onXform);
					cSDataArray2.Append(new CSOnGeometryData(nurbs_curve.Convert(), xUp));
					cSDataArray2.Append(new CSOnGeometryData(nurbs_curve2.Convert(), xUp));
					if (!value9)
					{
						OnLineCurve onLineCurve = new OnLineCurve(on3dPoint3, on3dPoint4);
						if (!CSUtilCurves.MergeCurves(new IOnCurve[2] { prongRail, onLineCurve }, out OnCurve mergedCrv, 0.01))
						{
							return UpdateError("MergeCurves failed");
						}
						if ((mergedCrv.PointAtStart() - on3dPoint4).Length() > 0.01)
						{
							mergedCrv.Reverse();
						}
						if (!CSUtilSweep.Sweep1(profiles: new IOnCurve[2] { nurbs_curve2, nurbs_curve }, inputParameters: new double[2]
						{
							mergedCrv.Domain().Min(),
							mergedCrv.Domain().Max()
						}, inputRailCurve: mergedCrv, breps: out var breps, closed: false))
						{
							return UpdateError("Failed Sweep1");
						}
						if (breps == null)
						{
							return UpdateError("Failed Sweep1: No results");
						}
						if (breps.Length == 0)
						{
							return UpdateError("Failed Sweep1: No results");
						}
						OnBrep onBrep = CSUtilRhino.CapPlanarHoles(breps[0], 0.01);
						if (onBrep == null)
						{
							return UpdateError("Failed CapPlanarHoles");
						}
						onBrep.Transform(xUp.Convert());
						cSDataArray3.Append(new CSOnGeometryData(onBrep.Convert(), xUp, applyTransform: false));
					}
					continue;
				}
				OnInterval onInterval = onNurbsCurve2.Domain();
				double num3 = (onInterval.Max() - onInterval.Min()) / 100.0;
				double num4 = t - num3;
				double num5 = t + num3;
				if (num4 < onInterval.Min())
				{
					num4 = onInterval.Max() + (num4 - onInterval.Min());
				}
				if (num5 > onInterval.Max())
				{
					num5 = onInterval.Min() + (num5 - onInterval.Max());
				}
				On3dVector on3dVector2 = onNurbsCurve2.TangentAt(num4) + onNurbsCurve2.TangentAt(num5);
				on3dVector2.Unitize();
				OnBoundingBox tight_bbox = new OnBoundingBox();
				nurbsCurve.GetTightBoundingBox(ref tight_bbox);
				OnXform onXform2 = new OnXform();
				onXform2.Scale(value / (tight_bbox.Max().x - tight_bbox.Min().x), value2 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
				OnXform onXform3 = new OnXform();
				onXform3.Translation(0.0, num, 0.0);
				OnXform onXform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, on3dPoint, on3dVector2, OnUtil.On_zaxis);
				OnNurbsCurve prongTopProfile = nurbsCurve.Duplicate();
				prongTopProfile.Transform(onXform4 * onXform3 * onXform2);
				On3dVector on3dVector3;
				OnXform onXform5;
				if (!value17)
				{
					OnInterval onInterval2 = onNurbsCurve3.Domain();
					double num6 = (onInterval2.Max() - onInterval2.Min()) / 100.0;
					double num7 = t2 - num6;
					double num8 = t2 + num6;
					if (num7 < onInterval2.Min())
					{
						num7 = onInterval2.Max() + (num7 - onInterval2.Min());
					}
					if (num8 > onInterval2.Max())
					{
						num8 = onInterval2.Min() + (num8 - onInterval2.Max());
					}
					on3dVector3 = onNurbsCurve3.TangentAt(num7) + onNurbsCurve3.TangentAt(num8);
					on3dVector3.Unitize();
					onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, on3dPoint2, on3dVector3, OnUtil.On_zaxis);
				}
				else
				{
					onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, on3dPoint2, on3dVector2, OnUtil.On_zaxis);
					on3dVector3 = on3dVector2;
				}
				On3dVector inputVector = prongRail.TangentAt(prongRail.Domain().Max());
				On3dVector inputVector2 = prongRail.TangentAt(prongRail.Domain().Min());
				double num9 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new On3dVector(0.0, 0.0, -1.0), inputVector2, on3dVector2);
				double value18 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new On3dVector(0.0, 0.0, -1.0), inputVector, on3dVector3);
				double num10 = 1.0 / Math.Cos(Math.Abs(value18) - Math.Abs(num9));
				if (num10 > 2.25)
				{
					num10 = 2.25;
				}
				OnBoundingBox tight_bbox2 = new OnBoundingBox();
				onNurbsCurve.GetTightBoundingBox(ref tight_bbox2);
				OnXform onXform6 = new OnXform();
				onXform6.Scale(value10 / (tight_bbox2.Max().x - tight_bbox2.Min().x), value11 / (tight_bbox2.Max().y - tight_bbox2.Min().y) * num10, 0.0);
				OnXform onXform7 = new OnXform();
				onXform7.Translation(0.0, num2, 0.0);
				OnNurbsCurve onNurbsCurve4 = onNurbsCurve.Duplicate();
				onNurbsCurve4.Transform(onXform5 * onXform7 * onXform6);
				cSDataArray2.Append(new CSOnGeometryData(onNurbsCurve4.Convert(), xUp));
				cSDataArray2.Append(new CSOnGeometryData(prongTopProfile.Convert(), xUp));
				OnBrep dome = null;
				if (CSApplicationHelper.Application.IsCamMode && !value9)
				{
					if (!CreateProngExtension(cSGemData.GemZMax, ref prongRail, ref prongTopProfile))
					{
						return UpdateError("Unable to create CAM prong extension.");
					}
				}
				else
				{
					if (!CreateDome(nurbsCurve, value, value2, num9, value14, value15, value16, ref dome))
					{
						return UpdateError("Unable to create dome");
					}
					dome.Transform(onXform4 * onXform3);
					if (value9)
					{
						cSDataArray3.Append(new CSOnGeometryData(dome.Convert(), xUp));
						continue;
					}
				}
				if (!CSUtilSweep.Sweep1(profiles: new IOnCurve[2] { prongTopProfile, onNurbsCurve4 }, inputParameters: new double[2]
				{
					prongRail.Domain().Min(),
					prongRail.Domain().Max()
				}, inputRailCurve: prongRail, breps: out var breps2, closed: false))
				{
					return UpdateError("Failed Sweep1");
				}
				if (breps2 == null)
				{
					return UpdateError("Failed Sweep1: No results");
				}
				if (breps2.Length == 0)
				{
					return UpdateError("Failed Sweep1: No results");
				}
				List<OnBrep> list2 = new List<OnBrep>();
				if (!CSApplicationHelper.Application.IsCamMode)
				{
					list2.Add(dome);
				}
				list2.AddRange(breps2);
				OnBrep onBrep2 = CSUtilRhino.JoinAllBreps(list2.ToArray(), 0.01);
				if (onBrep2 == null)
				{
					return UpdateError("Failed JoinBreps");
				}
				onBrep2 = CSUtilRhino.CapPlanarHoles(onBrep2, 0.01);
				if (onBrep2 == null)
				{
					return UpdateError("Failed CapPlanarHoles");
				}
				onBrep2.Transform(xUp.Convert());
				cSDataArray3.Append(new CSOnGeometryData(onBrep2.Convert(), xUp, applyTransform: false));
			}
			cSDataArray.Append(cSDataArray3);
		}
		oObjects.SetData(cSDataArray);
		oCurves.SetData(cSDataArray2);
		return base.Update();
	}

	private static bool CreateDome(OnNurbsCurve prongTopProfile, double prongTopX, double prongTopY, double angle, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, ref OnBrep dome)
	{
		OnBoundingBox tight_bbox = new OnBoundingBox();
		prongTopProfile.GetTightBoundingBox(ref tight_bbox);
		OnXform onXform = new OnXform();
		onXform.Scale(prongTopX / (tight_bbox.Max().x - tight_bbox.Min().x), prongTopY / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		OnXform onXform2 = new OnXform();
		onXform2.Translation(0.0, (0.0 - prongTopY) / 2.0, 0.0);
		OnNurbsCurve onNurbsCurve = prongTopProfile.Duplicate();
		onNurbsCurve.Transform(onXform2 * onXform);
		OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, -50.0, 0.0), new On3dPoint(0.0, 50.0, 0.0));
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		onNurbsCurve.IntersectCurve(onLineCurve, ref x);
		if (x.Count() <= 0)
		{
			return false;
		}
		double t = x[0].get_m_a(0);
		double t2 = x[1].get_m_a(0);
		onNurbsCurve.ChangeClosedCurveSeam(t);
		OnCurve left_side = null;
		OnCurve right_side = null;
		if (!onNurbsCurve.Split(t2, ref left_side, ref right_side))
		{
			return false;
		}
		if (left_side == null || right_side == null)
		{
			return false;
		}
		On3dVector on3dVector = onLineCurve.TangentAt(onLineCurve.Domain().Mid());
		on3dVector.Unitize();
		On3dVector on3dVector2 = left_side.TangentAt(left_side.Domain().Mid());
		on3dVector2.Unitize();
		if (OnUtil.ON_DotProduct(on3dVector, on3dVector2) < 0.0)
		{
			left_side.Reverse();
		}
		right_side.TangentAt(right_side.Domain().Mid()).Unitize();
		if (OnUtil.ON_DotProduct(on3dVector, right_side.TangentAt(right_side.Domain().Mid())) < 0.0)
		{
			right_side.Reverse();
		}
		On3dVector on3dVector3 = new On3dVector(OnUtil.On_zaxis);
		on3dVector3.Rotate(0.0 - angle, OnUtil.On_xaxis);
		on3dVector3.Unitize();
		On3dPoint on3dPoint = tight_bbox.Center();
		on3dPoint.Transform(onXform2 * onXform);
		on3dPoint += (IOn3dVector)(on3dVector3 * (prongDomeHeight / Math.Cos(angle)));
		on3dPoint += (IOn3dVector)(new On3dVector(OnUtil.On_yaxis) * prongDomeSettingDistance);
		On3dVector on3dVector4 = new On3dVector(OnUtil.On_yaxis) * -1.0;
		on3dVector4.Rotate(CSBlockUtil.CSUtilMath.ToRadians(prongDomeSettingAngle), OnUtil.On_xaxis);
		on3dVector4.Unitize();
		On3dVector on3dVector5 = new On3dVector(on3dVector4);
		on3dVector5.Reverse();
		OnNurbsCurve onNurbsCurve2 = CSUtilCurves.BlendCurve(left_side.PointAtStart(), on3dPoint, on3dVector3, on3dVector4);
		OnNurbsCurve onNurbsCurve3 = CSUtilCurves.BlendCurve(left_side.PointAtEnd(), on3dPoint, on3dVector3, on3dVector5);
		IOnCurve[] input_curves = new IOnCurve[2] { onNurbsCurve2, onNurbsCurve3 };
		OnCurve[] output = null;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output == null)
		{
			return false;
		}
		if (output.Length == 0)
		{
			return false;
		}
		OnLineCurve curveB = new OnLineCurve(new On3dPoint(-50.0, 0.0, 0.0), new On3dPoint(50.0, 0.0, 0.0));
		ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
		left_side.IntersectCurve(curveB, ref x2);
		if (x2.Count() <= 0)
		{
			return false;
		}
		double t3 = x2[0].get_m_a(0);
		OnCurve left_side2 = null;
		OnCurve right_side2 = null;
		if (!left_side.Split(t3, ref left_side2, ref right_side2))
		{
			return false;
		}
		if (left_side2 == null || right_side2 == null)
		{
			return false;
		}
		ArrayOnX_EVENT x3 = new ArrayOnX_EVENT();
		right_side.IntersectCurve(curveB, ref x3);
		if (x3.Count() <= 0)
		{
			return false;
		}
		double t4 = x3[0].get_m_a(0);
		OnCurve left_side3 = null;
		OnCurve right_side3 = null;
		if (!right_side.Split(t4, ref left_side3, ref right_side3))
		{
			return false;
		}
		if (left_side3 == null || right_side3 == null)
		{
			return false;
		}
		OnCurve railCurve = left_side2;
		OnCurve railCurve2 = right_side2;
		IOnCurve[] profiles = output;
		if (!CSUtilSweep.Sweep2NoParam(railCurve, railCurve2, profiles, out var breps, maintainHeight: false, firstAtStart: false, closed: false, flipProfiles: false))
		{
			return false;
		}
		OnCurve railCurve3 = left_side3;
		OnCurve railCurve4 = right_side3;
		profiles = output;
		if (!CSUtilSweep.Sweep2NoParam(railCurve3, railCurve4, profiles, out var breps2, maintainHeight: false, firstAtStart: false, closed: false, flipProfiles: false))
		{
			return false;
		}
		List<OnBrep> list = new List<OnBrep>();
		list.AddRange(breps);
		list.AddRange(breps2);
		OnBrep onBrep = CSUtilRhino.JoinAllBreps(list.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (onBrep == null)
		{
			return false;
		}
		OnXform onXform3 = new OnXform();
		onXform3.Translation(0.0, prongTopY / 2.0, 0.0);
		onBrep.Transform(onXform3);
		dome = onBrep;
		return true;
	}

	private static bool CreateProngExtension(double prongExtensionHeight, ref OnNurbsCurve prongRail, ref OnNurbsCurve prongTopProfile)
	{
		On3dVector on3dVector = prongRail.TangentAt(prongRail.Domain().Min());
		on3dVector.Unitize();
		On3dPoint on3dPoint = prongRail.PointAtStart();
		On3dPoint on3dPoint2 = on3dPoint - on3dVector * (prongExtensionHeight + CSApplicationHelper.Application.CamProngExtensionHeight);
		OnXform onXform = new OnXform();
		onXform.Translation(on3dPoint2 - on3dPoint);
		prongTopProfile.Transform(onXform);
		OnLineCurve onLineCurve = new OnLineCurve(on3dPoint, on3dPoint2);
		if (!CSUtilCurves.MergeCurves(new IOnCurve[2] { prongRail, onLineCurve }, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if ((mergedCrv.PointAtStart() - on3dPoint2).Length() > 0.01)
		{
			mergedCrv.Reverse();
		}
		return true;
	}
}
