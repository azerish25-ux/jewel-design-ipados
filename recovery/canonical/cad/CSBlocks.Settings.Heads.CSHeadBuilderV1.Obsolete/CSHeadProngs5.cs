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
[CSBlock("HeadProngs_V5", "{A7B36E38-6988-4CCD-A633-4F447E79572E}")]
public sealed class CSHeadProngs5 : CSBlock
{
	public CSOutputChannel oObjects { get; }

	public CSInputChannel iGems { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iProngLines { get; }

	public CSInputChannel iHeadTop { get; }

	public CSInputChannel iHeadBase { get; }

	public CSInputChannel iEnableVProngs { get; }

	public CSInputChannel iProngTopProfile { get; }

	public CSInputChannel iProngTopX { get; }

	public CSInputChannel iProngTopY { get; }

	public CSInputChannel iProngTopOffset { get; }

	public CSInputChannel iProngVTopAngle { get; }

	public CSInputChannel iEnableBase { get; }

	public CSInputChannel iProngBaseProfile { get; }

	public CSInputChannel iProngBaseX { get; }

	public CSInputChannel iProngBaseY { get; }

	public CSInputChannel iProngBaseOffset { get; }

	public CSInputChannel iProngVBaseAngle { get; }

	public CSInputChannel iObjects { get; }

	public CSInputChannel iGameChanger { get; }

	public CSInputChannel iProngDomeHeight { get; }

	public CSInputChannel iProngDomeSettingDistance { get; }

	public CSInputChannel iProngDomeSettingAngle { get; }

	public CSInputChannel iProngTopNudge { get; }

	public CSInputChannel iProngBaseNudge { get; }

	public CSInputChannel iBasetoPoint { get; }

	public CSHeadProngs5(string identifier)
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
		iProngTopOffset = new CSInputChannel(GetPropertyName(() => iProngTopOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVTopAngle = new CSInputChannel(GetPropertyName(() => iProngVTopAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableBase = new CSInputChannel(GetPropertyName(() => iEnableBase), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProngBaseProfile = new CSInputChannel(GetPropertyName(() => iProngBaseProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProngBaseX = new CSInputChannel(GetPropertyName(() => iProngBaseX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngBaseY = new CSInputChannel(GetPropertyName(() => iProngBaseY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngBaseOffset = new CSInputChannel(GetPropertyName(() => iProngBaseOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVBaseAngle = new CSInputChannel(GetPropertyName(() => iProngVBaseAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iObjects = new CSInputChannel(GetPropertyName(() => iObjects), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iGameChanger = new CSInputChannel(GetPropertyName(() => iGameChanger), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProngDomeHeight = new CSInputChannel(GetPropertyName(() => iProngDomeHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngDomeSettingDistance = new CSInputChannel(GetPropertyName(() => iProngDomeSettingDistance), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngDomeSettingAngle = new CSInputChannel(GetPropertyName(() => iProngDomeSettingAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngTopNudge = new CSInputChannel(GetPropertyName(() => iProngTopNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngBaseNudge = new CSInputChannel(GetPropertyName(() => iProngBaseNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBasetoPoint = new CSInputChannel(GetPropertyName(() => iBasetoPoint), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
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
		iProngTopOffset.GetDouble(ref value3);
		double value4 = 0.0;
		iProngVTopAngle.GetDouble(ref value4);
		double value5 = 0.0;
		iProngTopNudge.GetDouble(ref value5);
		value3 -= value2 * value5;
		int value6 = 0;
		iStyle.GetInteger(ref value6);
		bool value7 = false;
		iEnableVProngs.GetBoolean(ref value7);
		bool value8 = false;
		iEnableBase.GetBoolean(ref value8);
		bool value9 = false;
		iGameChanger.GetBoolean(ref value9);
		double value10 = 2.0;
		double value11 = 2.0;
		double value12 = 0.0;
		double value13 = 0.0;
		OnNurbsCurve onNurbsCurve;
		if (value8)
		{
			onNurbsCurve = iProngBaseProfile.GetNurbsCurve();
			if (onNurbsCurve == null)
			{
				return UpdateMissing("Prong Base Profile");
			}
			iProngBaseX.GetDouble(ref value10);
			iProngBaseY.GetDouble(ref value11);
			iProngBaseOffset.GetDouble(ref value12);
			iProngVBaseAngle.GetDouble(ref value13);
			double value14 = 0.0;
			iProngBaseNudge.GetDouble(ref value14);
			value12 -= value11 * value14;
		}
		else
		{
			onNurbsCurve = nurbsCurve.Duplicate();
			value10 = value;
			value11 = value2;
			value12 = value3;
			value13 = value4;
		}
		double value15 = 0.5;
		iProngDomeHeight.GetDouble(ref value15);
		double value16 = 0.0;
		iProngDomeSettingDistance.GetDouble(ref value16);
		double value17 = 0.0;
		iProngDomeSettingAngle.GetDouble(ref value17);
		bool value18 = false;
		iBasetoPoint.GetBoolean(ref value18);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				return UpdateMissing("GemData Missing");
			}
			OnNurbsCurve onNurbsCurve2 = iHeadTop.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve2 == null)
			{
				return UpdateMissing("HeadTop Missing");
			}
			OnNurbsCurve onNurbsCurve3 = iHeadBase.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve3 == null)
			{
				return UpdateMissing("HeadBase Missing");
			}
			if (!(iProngLines.GetArrayItem(i) is ICSArray iCSArray2))
			{
				return UpdateMissing("ProngLines Missing");
			}
			if (!cSGemData.GetGemXformRC(out var _, out var xUp))
			{
				continue;
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			if (iCSArray != null && iCSArray.GetArrayCount() > 0)
			{
				if (!(iCSArray.GetArrayItem(i) is ICSArray iCSArray3))
				{
					continue;
				}
				for (int j = 0; j < iCSArray3.GetArrayCount(); j++)
				{
					cSDataArray2.Append(iCSArray3.GetArrayItem(j));
				}
			}
			List<CSHeadStyles2.HeadProngInfo> list = (value9 ? CSHeadStyles2.GetHeadStyleGameChanger(cSGemData.GemShape, value6, cSGemData.VersionMajor) : CSHeadStyles2.GetHeadStyle(cSGemData.GemShape, value6, cSGemData.VersionMajor));
			if (list == null)
			{
				return UpdateError("Head Style Invalid");
			}
			for (int k = 0; k < list.Count; k++)
			{
				OnNurbsCurve onNurbsCurve4 = iCSArray2.GetArrayItem(k).GetNurbsCurve().Convert();
				if (onNurbsCurve4 == null)
				{
					CS.CSUtil.Print("Invalid Head Crv\n");
					continue;
				}
				On3dPoint on3dPoint = onNurbsCurve4.PointAtStart();
				double t = 0.0;
				onNurbsCurve2.GetClosestPoint(on3dPoint, ref t);
				On3dPoint on3dPoint2 = onNurbsCurve4.PointAtEnd();
				double t2 = 0.0;
				onNurbsCurve3.GetClosestPoint(on3dPoint2, ref t2);
				if (list[k].Type == CSHeadStyles2.EHeadProngType.Straight && value7)
				{
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(onNurbsCurve2, t, value4, value2, value3, out var vProng))
					{
						return UpdateError("Error Making V Prong Top");
					}
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(onNurbsCurve3, t2, value13, value11, value12, out var vProng2))
					{
						return UpdateError("Error Making V Prong Top");
					}
					OnNurbsCurve nurbs_curve = new OnNurbsCurve();
					vProng.GetNurbForm(ref nurbs_curve);
					OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
					vProng2.GetNurbForm(ref nurbs_curve2);
					On3dPoint on3dPoint3 = onNurbsCurve4.PointAtStart();
					On3dVector on3dVector = onNurbsCurve4.TangentAt(onNurbsCurve4.Domain().Min());
					on3dVector.Unitize();
					On3dPoint on3dPoint4 = on3dPoint3 - on3dVector * 0.5;
					OnLineCurve onLineCurve = new OnLineCurve(on3dPoint3, on3dPoint4);
					if (!CSUtilCurves.MergeCurves(new IOnCurve[2] { onNurbsCurve4, onLineCurve }, out OnCurve mergedCrv, 0.01))
					{
						return UpdateError("MergeCurves failed");
					}
					if ((mergedCrv.PointAtStart() - on3dPoint4).Length() > 0.01)
					{
						mergedCrv.Reverse();
					}
					if (CSApplicationHelper.Application.IsCamMode && !CreateProngExtension(cSGemData.GemZMax, ref mergedCrv, ref nurbs_curve))
					{
						return UpdateError("Unable to create CAM prong extension.");
					}
					OnXform onXform = new OnXform();
					onXform.Translation(on3dPoint4 - on3dPoint3);
					nurbs_curve.Transform(onXform);
					if (!CSUtilSweep.Sweep1(profiles: new IOnCurve[2] { nurbs_curve, nurbs_curve2 }, inputParameters: new double[2]
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
					CSData data = new CSOnGeometryData(onBrep.Convert(), xUp, applyTransform: false);
					cSDataArray2.Append(data);
					continue;
				}
				OnInterval onInterval = onNurbsCurve2.Domain();
				double num = (onInterval.Max() - onInterval.Min()) / 100.0;
				double num2 = t - num;
				double num3 = t + num;
				if (num2 < onInterval.Min())
				{
					num2 = onInterval.Max() + (num2 - onInterval.Min());
				}
				if (num3 > onInterval.Max())
				{
					num3 = onInterval.Min() + (num3 - onInterval.Max());
				}
				On3dVector on3dVector2 = onNurbsCurve2.TangentAt(num2) + onNurbsCurve2.TangentAt(num3);
				on3dVector2.Unitize();
				OnBoundingBox tight_bbox = new OnBoundingBox();
				nurbsCurve.GetTightBoundingBox(ref tight_bbox);
				OnXform onXform2 = new OnXform();
				onXform2.Scale(value / (tight_bbox.Max().x - tight_bbox.Min().x), value2 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
				OnXform onXform3 = new OnXform();
				onXform3.Translation(0.0, value3, 0.0);
				OnXform onXform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, on3dPoint, on3dVector2, OnUtil.On_zaxis);
				OnNurbsCurve onNurbsCurve5 = nurbsCurve.Duplicate();
				onNurbsCurve5.Transform(onXform4 * onXform3 * onXform2);
				On3dVector on3dVector3;
				OnXform onXform5;
				if (!value18)
				{
					OnInterval onInterval2 = onNurbsCurve3.Domain();
					double num4 = (onInterval2.Max() - onInterval2.Min()) / 100.0;
					double num5 = t2 - num4;
					double num6 = t2 + num4;
					if (num5 < onInterval2.Min())
					{
						num5 = onInterval2.Max() + (num5 - onInterval2.Min());
					}
					if (num6 > onInterval2.Max())
					{
						num6 = onInterval2.Min() + (num6 - onInterval2.Max());
					}
					on3dVector3 = onNurbsCurve3.TangentAt(num5) + onNurbsCurve3.TangentAt(num6);
					on3dVector3.Unitize();
					onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, on3dPoint2, on3dVector3, OnUtil.On_zaxis);
				}
				else
				{
					onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, on3dPoint2, on3dVector2, OnUtil.On_zaxis);
					on3dVector3 = on3dVector2;
				}
				OnXform xform = onXform5.Inverse();
				onNurbsCurve4.Duplicate().Transform(xform);
				On3dVector inputVector = onNurbsCurve4.TangentAt(onNurbsCurve4.Domain().Max());
				On3dVector inputVector2 = onNurbsCurve4.TangentAt(onNurbsCurve4.Domain().Min());
				double num7 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new On3dVector(0.0, 0.0, -1.0), inputVector2, on3dVector2);
				double value19 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new On3dVector(0.0, 0.0, -1.0), inputVector, on3dVector3);
				double num8 = 1.0 / Math.Cos(Math.Abs(value19) - Math.Abs(num7));
				OnBoundingBox tight_bbox2 = new OnBoundingBox();
				onNurbsCurve.GetTightBoundingBox(ref tight_bbox2);
				OnXform onXform6 = new OnXform();
				onXform6.Scale(value10 / (tight_bbox2.Max().x - tight_bbox2.Min().x), value11 / (tight_bbox2.Max().y - tight_bbox2.Min().y) * num8, 0.0);
				OnXform onXform7 = new OnXform();
				onXform7.Translation(0.0, value12, 0.0);
				OnNurbsCurve onNurbsCurve6 = onNurbsCurve.Duplicate();
				onNurbsCurve6.Transform(onXform5 * onXform7 * onXform6);
				if (!CSUtilSweep.Sweep1(onNurbsCurve4, new IOnCurve[2] { onNurbsCurve5, onNurbsCurve6 }, new double[2]
				{
					onNurbsCurve4.Domain().Min(),
					onNurbsCurve4.Domain().Max()
				}, out var breps2, closed: false))
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
				OnBrep dome = null;
				if (!CreateDome(nurbsCurve, value, value2, num7, value15, value16, value17, ref dome))
				{
					return UpdateError("Unable to create dome");
				}
				dome.Transform(onXform4 * onXform3);
				List<OnBrep> list2 = new List<OnBrep>();
				list2.AddRange(breps2);
				list2.Add(dome);
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
				CSData data2 = new CSOnGeometryData(onBrep2.Convert(), xUp, applyTransform: false);
				cSDataArray2.Append(data2);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oObjects.SetData(cSDataArray);
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

	private static bool CreateProngExtension(double prongExtensionHeight, ref OnCurve prongRail, ref OnNurbsCurve prongTopProfile)
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
