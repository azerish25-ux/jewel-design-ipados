using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadProngs_V1", "{0A4C2741-A988-4498-88A5-EAB94A116465}")]
public sealed class CSHeadProngs1 : CSBlock
{
	public CSInputChannel IGem { get; }

	public CSInputChannel IStyle { get; }

	public CSInputChannel IProngLines { get; }

	public CSInputChannel IHeadTop { get; }

	public CSInputChannel IHeadBase { get; }

	public CSInputChannel IProngTopProfile { get; }

	public CSInputChannel IProngTopX { get; }

	public CSInputChannel IProngTopY { get; }

	public CSInputChannel IProngTopNudge { get; }

	public CSInputChannel IEnableBase { get; }

	public CSInputChannel IProngBaseProfile { get; }

	public CSInputChannel IProngBaseX { get; }

	public CSInputChannel IProngBaseY { get; }

	public CSInputChannel IProngBaseNudge { get; }

	public CSOutputChannel OProngs { get; }

	public CSHeadProngs1(string identifier)
		: base(identifier)
	{
		IGem = new CSInputChannel(GetPropertyName(() => IGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		IStyle = new CSInputChannel(GetPropertyName(() => IStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IProngLines = new CSInputChannel(GetPropertyName(() => IProngLines), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		IHeadTop = new CSInputChannel(GetPropertyName(() => IHeadTop), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IHeadBase = new CSInputChannel(GetPropertyName(() => IHeadBase), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IProngTopProfile = new CSInputChannel(GetPropertyName(() => IProngTopProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IProngTopX = new CSInputChannel(GetPropertyName(() => IProngTopX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IProngTopY = new CSInputChannel(GetPropertyName(() => IProngTopY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IProngTopNudge = new CSInputChannel(GetPropertyName(() => IProngTopNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IEnableBase = new CSInputChannel(GetPropertyName(() => IEnableBase), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		IProngBaseProfile = new CSInputChannel(GetPropertyName(() => IProngBaseProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IProngBaseX = new CSInputChannel(GetPropertyName(() => IProngBaseX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IProngBaseY = new CSInputChannel(GetPropertyName(() => IProngBaseY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IProngBaseNudge = new CSInputChannel(GetPropertyName(() => IProngBaseNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OProngs = new CSOutputChannel(GetPropertyName(() => OProngs), this, ChannelDataType.Brep, ChannelArrayType.Any);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = IGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Input Gem Missing");
		}
		if (!(flattenArray.GetArrayItem(0) is CSGemData cSGemData))
		{
			return UpdateMissing("Input Gem Mssing");
		}
		int value = 0;
		IStyle.GetInteger(ref value);
		OnNurbsCurve nurbsCurve = IHeadTop.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("HeadTop Curve Missing");
		}
		OnNurbsCurve nurbsCurve2 = IHeadBase.GetNurbsCurve();
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("HeadBase Curve Missing");
		}
		OnNurbsCurve nurbsCurve3 = IProngTopProfile.GetNurbsCurve();
		if (nurbsCurve3 == null)
		{
			return UpdateMissing("ProngTopProfile Curve Missing");
		}
		CSDataArray flattenArray2 = IProngLines.GetFlattenArray();
		if (flattenArray2 == null)
		{
			return UpdateMissing("ProngLines Missing");
		}
		double value2 = 2.0;
		IProngTopX.GetDouble(ref value2);
		double value3 = 2.0;
		IProngTopY.GetDouble(ref value3);
		double value4 = 0.0;
		IProngTopNudge.GetDouble(ref value4);
		bool value5 = false;
		IEnableBase.GetBoolean(ref value5);
		double value6 = 2.0;
		double value7 = 2.0;
		double value8 = 0.0;
		OnNurbsCurve onNurbsCurve;
		if (value5)
		{
			onNurbsCurve = IProngBaseProfile.GetNurbsCurve();
			if (onNurbsCurve == null)
			{
				return UpdateMissing("ProngBaseProfile Missing");
			}
			IProngBaseX.GetDouble(ref value6);
			IProngBaseY.GetDouble(ref value7);
			IProngBaseNudge.GetDouble(ref value8);
		}
		else
		{
			onNurbsCurve = nurbsCurve3.Duplicate();
			value6 = value2;
			value7 = value3;
			value8 = value4;
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform.Convert(), out var xUp, out var _);
		CSHeadStyles1.GetHeadData1(cSGemData.GemShape, value, out var position, out var _, out var _);
		if (position == null)
		{
			return UpdateError("Head Style Missing");
		}
		OnXform onXform = new OnXform();
		OnXform onXform2 = new OnXform();
		OnBoundingBox tight_bbox = new OnBoundingBox();
		nurbsCurve3.GetTightBoundingBox(ref tight_bbox);
		onXform.Scale(value2 / (tight_bbox.Max().x - tight_bbox.Min().x), value3 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		onXform2.Translation(0.0, value4, 0.0);
		OnNurbsCurve onNurbsCurve2 = nurbsCurve3.Duplicate();
		nurbsCurve3.Transform(onXform2 * onXform);
		OnXform onXform3 = new OnXform();
		onXform3.Translation(0.0, (0.0 - value3) / 2.0, 0.0);
		onNurbsCurve2.Transform(onXform3 * onXform);
		onNurbsCurve.GetTightBoundingBox(ref tight_bbox);
		onXform.Scale(value6 / (tight_bbox.Max().x - tight_bbox.Min().x), value7 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		onXform2.Translation(0.0, value8, 0.0);
		onNurbsCurve.Transform(onXform2 * onXform);
		OnNurbsSurface onNurbsSurface = CSUtilRailRevolve.RailRevolve(CSUtilGeometry.CreateDome(0.5), onNurbsCurve2, 1);
		onXform3.Translation(0.0, value3 / 2.0 + value4, 0.0);
		onNurbsSurface.Transform(onXform3);
		On3dPoint source = new On3dPoint(0.0, 0.0, 0.0);
		On3dVector source2 = new On3dVector(1.0, 0.0, 0.0);
		On3dVector on3dVector = new On3dVector(0.0, 0.0, 1.0);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < position.Length; i++)
		{
			OnNurbsCurve prongRail = flattenArray2.GetArrayItem(i).GetNurbsCurve().Convert();
			On3dPoint on3dPoint = prongRail.PointAtStart();
			On3dPoint on3dPoint2 = prongRail.PointAtEnd();
			double t = 0.0;
			nurbsCurve.GetClosestPoint(on3dPoint, ref t);
			double t2 = 0.0;
			nurbsCurve2.GetClosestPoint(on3dPoint2, ref t2);
			OnInterval onInterval = nurbsCurve.Domain();
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
			On3dVector on3dVector2 = nurbsCurve.TangentAt(num2) + nurbsCurve.TangentAt(num3);
			on3dVector2.Unitize();
			onInterval = nurbsCurve2.Domain();
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
			On3dVector on3dVector3 = nurbsCurve2.TangentAt(num2) + nurbsCurve2.TangentAt(num3);
			on3dVector3.Unitize();
			OnXform onXform4 = CSBlockUtil.CSUtilMath.Orient3Point(source, source2, on3dVector, on3dPoint, on3dVector2, on3dVector);
			OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(source, source2, on3dVector, on3dPoint2, on3dVector3, on3dVector);
			OnNurbsCurve prongTopProfile = nurbsCurve3.Duplicate();
			prongTopProfile.Transform(onXform4);
			OnNurbsCurve onNurbsCurve3 = onNurbsCurve.Duplicate();
			onNurbsCurve3.Transform(xform);
			OnCurve[] array = new OnCurve[2] { prongTopProfile, onNurbsCurve3 };
			double[] inputParameters = new double[2]
			{
				prongRail.Domain().Min(),
				prongRail.Domain().Max()
			};
			OnBrep[] breps = null;
			if (CSApplicationHelper.Application.IsCamMode && !CreateProngExtension(cSGemData.GemZMax, ref prongRail, ref prongTopProfile))
			{
				return UpdateError("Unable to create CAM prong extension.");
			}
			OnNurbsCurve inputRailCurve = prongRail;
			IOnCurve[] profiles = array;
			if (!CSUtilSweep.Sweep1(inputRailCurve, profiles, inputParameters, out breps, closed: false))
			{
				return UpdateError("Head Prong Sweep Failed");
			}
			breps[0].Transform(xUp);
			OnNurbsSurface onNurbsSurface2 = onNurbsSurface.Duplicate();
			on3dPoint = prongRail.PointAt(prongRail.Domain().Min());
			On3dVector on3dVector4 = prongRail.TangentAt(prongRail.Domain().Min());
			on3dPoint.z = 0.0;
			on3dVector4.z = 0.0;
			On3dVector on3dVector5 = new On3dVector(on3dPoint);
			double num4 = on3dVector4.Length();
			if (on3dVector5.Length() < (on3dVector5 + on3dVector4).Length())
			{
				num4 = 0.0 - num4;
			}
			for (int j = 0; j < onNurbsSurface2.CVCount(0); j++)
			{
				for (int k = 0; k < onNurbsSurface2.CVCount(1); k++)
				{
					On4dPoint cv = new On3dPoint();
					onNurbsSurface2.GetCV(j, k, ref cv);
					cv.y += num4 * cv.z;
					onNurbsSurface2.SetCV(j, k, cv);
				}
			}
			onNurbsSurface2.Transform(onXform4);
			onNurbsSurface2.Transform(xUp);
			OnBrep brep = CSUtilRhino.CapPlanarHoles(CSUtilRhino.JoinAllBreps(new List<OnBrep>
			{
				breps[0],
				onNurbsSurface2.BrepForm()
			}.ToArray(), 0.01), 0.01);
			cSDataArray.Append(new CSOnGeometryData(brep.Convert()));
		}
		OProngs.SetData(cSDataArray);
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
