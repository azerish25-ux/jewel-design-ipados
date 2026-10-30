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

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadProngs_V4", "{4F1D12DB-9A3A-4702-B865-BE11C4E468A8}")]
public sealed class CSHeadProngs4 : CSBlock
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

	public CSInputChannel iProngTopNudge { get; }

	public CSInputChannel iProngVTopAngle { get; }

	public CSInputChannel iEnableBase { get; }

	public CSInputChannel iProngBaseProfile { get; }

	public CSInputChannel iProngBaseX { get; }

	public CSInputChannel iProngBaseY { get; }

	public CSInputChannel iProngBaseNudge { get; }

	public CSInputChannel iProngVBaseAngle { get; }

	public CSInputChannel iObjects { get; }

	public CSInputChannel iGameChanger { get; }

	public CSHeadProngs4(string identifier)
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
		iGameChanger = new CSInputChannel(GetPropertyName(() => iGameChanger), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Gems");
		}
		int value = 0;
		iStyle.GetInteger(ref value);
		OnNurbsCurve nurbsCurve = iProngTopProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Prong Top Profile");
		}
		bool value2 = false;
		iEnableVProngs.GetBoolean(ref value2);
		double value3 = 2.0;
		iProngTopX.GetDouble(ref value3);
		double value4 = 2.0;
		iProngTopY.GetDouble(ref value4);
		double value5 = 0.0;
		iProngTopNudge.GetDouble(ref value5);
		double value6 = 0.0;
		iProngVTopAngle.GetDouble(ref value6);
		bool value7 = false;
		iEnableBase.GetBoolean(ref value7);
		bool value8 = false;
		iGameChanger.GetBoolean(ref value8);
		ICSArray iCSArray = iObjects.Data as ICSArray;
		double value9 = 2.0;
		double value10 = 2.0;
		double value11 = 0.0;
		double value12 = 0.0;
		OnNurbsCurve onNurbsCurve;
		if (value7)
		{
			onNurbsCurve = iProngBaseProfile.GetNurbsCurve();
			if (onNurbsCurve == null)
			{
				return UpdateMissing("Prong Base Profile");
			}
			iProngBaseX.GetDouble(ref value9);
			iProngBaseY.GetDouble(ref value10);
			iProngBaseNudge.GetDouble(ref value11);
			iProngVBaseAngle.GetDouble(ref value12);
		}
		else
		{
			onNurbsCurve = nurbsCurve.Duplicate();
			value9 = value3;
			value10 = value4;
			value11 = value5;
			value12 = value6;
		}
		OnXform onXform = new OnXform();
		OnXform onXform2 = new OnXform();
		OnBoundingBox tight_bbox = new OnBoundingBox();
		nurbsCurve.GetTightBoundingBox(ref tight_bbox);
		onXform.Scale(value3 / (tight_bbox.Max().x - tight_bbox.Min().x), value4 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		onXform2.Translation(0.0, value5, 0.0);
		OnNurbsCurve onNurbsCurve2 = nurbsCurve.Duplicate();
		nurbsCurve.Transform(onXform2 * onXform);
		OnXform onXform3 = new OnXform();
		onXform3.Translation(0.0, (0.0 - value4) / 2.0, 0.0);
		onNurbsCurve2.Transform(onXform3 * onXform);
		onNurbsCurve.GetTightBoundingBox(ref tight_bbox);
		onXform.Scale(value9 / (tight_bbox.Max().x - tight_bbox.Min().x), value10 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		onXform2.Translation(0.0, value11, 0.0);
		onNurbsCurve.Transform(onXform2 * onXform);
		OnNurbsSurface onNurbsSurface = CSUtilRailRevolve.RailRevolve(CSUtilGeometry.CreateDome(0.5), onNurbsCurve2, 1);
		onXform3.Translation(0.0, value4 / 2.0 + value5, 0.0);
		onNurbsSurface.Transform(onXform3);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				return UpdateMissing("GemArray missing");
			}
			OnNurbsCurve onNurbsCurve3 = iHeadTop.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve3 == null)
			{
				return UpdateMissing("HeadTop curve missing");
			}
			OnNurbsCurve onNurbsCurve4 = iHeadBase.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve4 == null)
			{
				return UpdateMissing("HeadBase curve missing");
			}
			if (!(iProngLines.GetArrayItem(i) is ICSArray iCSArray2))
			{
				return UpdateMissing("ProngLines missing");
			}
			if (!cSGemData.GetGemXformRC(out var _, out var xUp))
			{
				continue;
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			if (iCSArray != null && iCSArray.GetArrayCount() > 0)
			{
				ICSArray iCSArray3 = iCSArray.GetArrayItem(i) as ICSArray;
				for (int j = 0; j < iCSArray3.GetArrayCount(); j++)
				{
					cSDataArray2.Append(iCSArray3.GetArrayItem(j));
				}
			}
			List<CSHeadStyles2.HeadProngInfo> list = (value8 ? CSHeadStyles2.GetHeadStyleGameChanger(cSGemData.GemShape, value, cSGemData.VersionMajor) : CSHeadStyles2.GetHeadStyle(cSGemData.GemShape, value, cSGemData.VersionMajor));
			if (list == null)
			{
				return UpdateError("Head Style Invalid");
			}
			for (int k = 0; k < list.Count; k++)
			{
				OnNurbsCurve prongRail = iCSArray2.GetArrayItem(k).GetNurbsCurve().Convert();
				if (prongRail == null)
				{
					CS.CSUtil.Print("Invalid Head Crv\n");
					continue;
				}
				On3dPoint on3dPoint = prongRail.PointAtStart();
				On3dPoint on3dPoint2 = prongRail.PointAtEnd();
				double t = 0.0;
				onNurbsCurve3.GetClosestPoint(on3dPoint, ref t);
				double t2 = 0.0;
				onNurbsCurve4.GetClosestPoint(on3dPoint2, ref t2);
				IOnCurve[] profiles;
				if (list[k].Type == CSHeadStyles2.EHeadProngType.Straight && value2)
				{
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(onNurbsCurve3, t, value6, value4, value5, out var vProng))
					{
						return UpdateError("Error Making V Prong Top");
					}
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(onNurbsCurve4, t2, value12, value10, value11, out var vProng2))
					{
						return UpdateError("Error Making V Prong Top");
					}
					OnNurbsCurve nurbs_curve = new OnNurbsCurve();
					vProng.GetNurbForm(ref nurbs_curve);
					OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
					vProng2.GetNurbForm(ref nurbs_curve2);
					On3dPoint on3dPoint3 = prongRail.PointAtStart();
					On3dVector on3dVector = prongRail.TangentAt(prongRail.Domain().Min());
					on3dVector.Unitize();
					On3dPoint on3dPoint4 = on3dPoint3 - on3dVector * 0.5;
					OnLineCurve onLineCurve = new OnLineCurve(on3dPoint3, on3dPoint4);
					CSUtilCurves.MergeCurves(new IOnCurve[2] { prongRail, onLineCurve }, out OnCurve mergedCrv, 0.01);
					if ((mergedCrv.PointAtStart() - on3dPoint4).Length() > 0.01)
					{
						mergedCrv.Reverse();
					}
					OnXform onXform4 = new OnXform();
					onXform4.Translation(on3dPoint4 - on3dPoint3);
					nurbs_curve.Transform(onXform4);
					OnCurve[] array = new OnCurve[2] { nurbs_curve, nurbs_curve2 };
					double[] inputParameters = new double[2]
					{
						mergedCrv.Domain().Min(),
						mergedCrv.Domain().Max()
					};
					OnCurve inputRailCurve = mergedCrv;
					profiles = array;
					if (!CSUtilSweep.Sweep1(inputRailCurve, profiles, inputParameters, out var breps, closed: false))
					{
						return UpdateError("HeadProngs Failed Sweep");
					}
					OnBrep onBrep = CSUtilRhino.CapPlanarHoles(breps[0], 0.01);
					onBrep.Transform(xUp.Convert());
					CSData data = new CSOnGeometryData(onBrep.Convert(), xUp, applyTransform: false);
					cSDataArray2.Append(data);
					continue;
				}
				OnInterval onInterval = onNurbsCurve3.Domain();
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
				On3dVector on3dVector2 = onNurbsCurve3.TangentAt(num2) + onNurbsCurve3.TangentAt(num3);
				on3dVector2.Unitize();
				onInterval = onNurbsCurve4.Domain();
				num = (onInterval.Max() - onInterval.Min()) / 100.0;
				num2 = t2 - num;
				num3 = t2 + num;
				if (num2 < onInterval.Min())
				{
					num2 = onInterval.Max() + (num2 - onInterval.Min());
				}
				if (num3 > onInterval.Max())
				{
					num3 = onInterval.Min() + (num3 - onInterval.Max());
				}
				On3dVector on3dVector3 = onNurbsCurve4.TangentAt(num2) + onNurbsCurve4.TangentAt(num3);
				on3dVector3.Unitize();
				OnXform onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, on3dPoint, on3dVector2, OnUtil.On_zaxis);
				OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, on3dPoint2, on3dVector3, OnUtil.On_zaxis);
				OnNurbsCurve prongTopProfile = nurbsCurve.Duplicate();
				prongTopProfile.Transform(onXform5);
				OnNurbsCurve onNurbsCurve5 = onNurbsCurve.Duplicate();
				onNurbsCurve5.Transform(xform);
				OnCurve[] array2 = new OnCurve[2] { prongTopProfile, onNurbsCurve5 };
				double[] inputParameters2 = new double[2]
				{
					prongRail.Domain().Min(),
					prongRail.Domain().Max()
				};
				if (CSApplicationHelper.Application.IsCamMode && !CreateProngExtension(cSGemData.GemZMax, ref prongRail, ref prongTopProfile))
				{
					return UpdateError("Unable to create CAM prong extension.");
				}
				OnNurbsCurve inputRailCurve2 = prongRail;
				profiles = array2;
				if (!CSUtilSweep.Sweep1(inputRailCurve2, profiles, inputParameters2, out var breps2, closed: false))
				{
					return UpdateError("HeadProngs Sweep Failed");
				}
				OnNurbsSurface onNurbsSurface2 = onNurbsSurface.Duplicate();
				on3dPoint = prongRail.PointAt(prongRail.Domain().Min());
				on3dPoint.z = 0.0;
				On3dVector on3dVector4 = prongRail.TangentAt(prongRail.Domain().Min());
				on3dVector4.z = 0.0;
				On3dVector on3dVector5 = new On3dVector(on3dPoint);
				double num4 = on3dVector4.Length();
				if (on3dVector5.Length() < (on3dVector5 + on3dVector4).Length())
				{
					num4 = 0.0 - num4;
				}
				for (int l = 0; l < onNurbsSurface2.CVCount(0); l++)
				{
					for (int m = 0; m < onNurbsSurface2.CVCount(1); m++)
					{
						On4dPoint cv = new On3dPoint();
						onNurbsSurface2.GetCV(l, m, ref cv);
						cv.y += num4 * cv.z;
						onNurbsSurface2.SetCV(l, m, cv);
					}
				}
				onNurbsSurface2.Transform(onXform5);
				List<OnBrep> list2 = new List<OnBrep>();
				if (breps2.Length != 0)
				{
					list2.Add(breps2[0]);
				}
				list2.Add(onNurbsSurface2.BrepForm());
				OnBrep onBrep2 = CSUtilRhino.CapPlanarHoles(CSUtilRhino.JoinAllBreps(list2.ToArray(), 0.01), 0.01);
				onBrep2.Transform(xUp.Convert());
				CSData data2 = new CSOnGeometryData(onBrep2.Convert(), xUp, applyTransform: false);
				cSDataArray2.Append(data2);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oObjects.SetData(cSDataArray);
		return base.Update();
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
