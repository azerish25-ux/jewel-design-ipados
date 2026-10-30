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
[CSBlock("HeadProngs_V3", "{1F38016B-4A69-4aca-A2BF-BA916700E93F}")]
public sealed class CSHeadProngs3 : CSBlock
{
	public CSInputChannel IGems { get; }

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

	public CSInputChannel IObjects { get; }

	public CSOutputChannel OObjects { get; }

	public CSHeadProngs3(string identifier)
		: base(identifier)
	{
		IGems = new CSInputChannel(GetPropertyName(() => IGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
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
		IObjects = new CSInputChannel(GetPropertyName(() => IObjects), this, ChannelDataType.Any, ChannelArrayType.Individual);
		OObjects = new CSOutputChannel(GetPropertyName(() => OObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = IGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Gems missing");
		}
		int value = 0;
		IStyle.GetInteger(ref value);
		OnNurbsCurve nurbsCurve = IProngTopProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("ProngTopProfile missing");
		}
		double value2 = 2.0;
		IProngTopX.GetDouble(ref value2);
		double value3 = 2.0;
		IProngTopY.GetDouble(ref value3);
		double value4 = 0.0;
		IProngTopNudge.GetDouble(ref value4);
		bool value5 = false;
		IEnableBase.GetBoolean(ref value5);
		ICSArray iCSArray = IObjects.Data as ICSArray;
		double value6 = 2.0;
		double value7 = 2.0;
		double value8 = 0.0;
		OnNurbsCurve onNurbsCurve;
		if (value5)
		{
			onNurbsCurve = IProngBaseProfile.GetNurbsCurve();
			if (onNurbsCurve == null)
			{
				return UpdateMissing("ProngBaseProfile missing");
			}
			IProngBaseX.GetDouble(ref value6);
			IProngBaseY.GetDouble(ref value7);
			IProngBaseNudge.GetDouble(ref value8);
		}
		else
		{
			onNurbsCurve = nurbsCurve.Duplicate();
			value6 = value2;
			value7 = value3;
			value8 = value4;
		}
		OnXform onXform = new OnXform();
		OnXform onXform2 = new OnXform();
		OnBoundingBox tight_bbox = new OnBoundingBox();
		nurbsCurve.GetTightBoundingBox(ref tight_bbox);
		onXform.Scale(value2 / (tight_bbox.Max().x - tight_bbox.Min().x), value3 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		onXform2.Translation(0.0, value4, 0.0);
		OnNurbsCurve onNurbsCurve2 = nurbsCurve.Duplicate();
		nurbsCurve.Transform(onXform2 * onXform);
		OnXform onXform3 = new OnXform();
		onXform3.Translation(0.0, (0.0 - value3) / 2.0, 0.0);
		onNurbsCurve2.Transform(onXform3 * onXform);
		onNurbsCurve.GetTightBoundingBox(ref tight_bbox);
		onXform.Scale(value6 / (tight_bbox.Max().x - tight_bbox.Min().x), value7 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		onXform2.Translation(0.0, value8, 0.0);
		onNurbsCurve.Transform(onXform2 * onXform);
		new OnNurbsCurve();
		OnNurbsCurve profile = CSUtilGeometry.CreateDome(0.5);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		onNurbsSurface = CSUtilRailRevolve.RailRevolve(profile, onNurbsCurve2, 1);
		onXform3.Translation(0.0, value3 / 2.0 + value4, 0.0);
		onNurbsSurface.Transform(onXform3);
		On3dPoint source = new On3dPoint(0.0, 0.0, 0.0);
		On3dVector source2 = new On3dVector(1.0, 0.0, 0.0);
		On3dVector on3dVector = new On3dVector(0.0, 0.0, 1.0);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				return UpdateError("GemData invalid");
			}
			OnNurbsCurve onNurbsCurve3 = IHeadTop.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve3 == null)
			{
				return UpdateError("HeadTop curve invalid");
			}
			OnNurbsCurve onNurbsCurve4 = IHeadBase.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve4 == null)
			{
				return UpdateError("HeadBase curve invalid");
			}
			if (!(IProngLines.GetArrayItem(i) is ICSArray iCSArray2))
			{
				return UpdateError("ProngLines invalid");
			}
			cSGemData.GetGemXformRC(out var _, out var xUp);
			_ = cSGemData.GemXSize;
			_ = cSGemData.GemYSize;
			_ = cSGemData.GemXMax;
			_ = cSGemData.GemYMax;
			CSHeadStyles1.GetHeadData1(cSGemData.GemShape, value, out var position, out var _, out var _);
			if (position == null)
			{
				return UpdateError("HeadStyle invalid");
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
			for (int k = 0; k < position.Length; k++)
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
				OnXform onXform4 = new OnXform();
				OnXform onXform5 = new OnXform();
				onXform4 = CSBlockUtil.CSUtilMath.Orient3Point(source, source2, on3dVector, on3dPoint, on3dVector2, on3dVector);
				onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(source, source2, on3dVector, on3dPoint2, on3dVector3, on3dVector);
				OnNurbsCurve onNurbsCurve5 = new OnNurbsCurve();
				OnNurbsCurve onNurbsCurve6 = new OnNurbsCurve();
				onNurbsCurve5 = nurbsCurve.Duplicate();
				onNurbsCurve5.Transform(onXform4);
				onNurbsCurve6 = onNurbsCurve.Duplicate();
				onNurbsCurve6.Transform(onXform5);
				OnCurve[] array = new OnCurve[2] { onNurbsCurve5, onNurbsCurve6 };
				double[] inputParameters = new double[2]
				{
					prongRail.Domain().Min(),
					prongRail.Domain().Max()
				};
				OnBrep[] breps = null;
				if (CSApplicationHelper.Application.IsCamMode && !CreateProngExtension(cSGemData.GemZMax, ref prongRail, ref onNurbsCurve5))
				{
					return UpdateError("Unable to create CAM prong extension.");
				}
				OnNurbsCurve inputRailCurve = prongRail;
				IOnCurve[] profiles = array;
				if (!CSUtilSweep.Sweep1(inputRailCurve, profiles, inputParameters, out breps, closed: false))
				{
					return UpdateError("HeadProng Sweep failed");
				}
				OnNurbsSurface onNurbsSurface2 = new OnNurbsSurface();
				onNurbsSurface2 = onNurbsSurface.Duplicate();
				On3dVector on3dVector4 = new On3dVector();
				on3dPoint = prongRail.PointAt(prongRail.Domain().Min());
				on3dVector4 = prongRail.TangentAt(prongRail.Domain().Min());
				on3dPoint.z = 0.0;
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
				onNurbsSurface2.Transform(onXform4);
				List<OnBrep> list = new List<OnBrep>();
				if (breps.Length != 0)
				{
					list.Add(breps[0]);
				}
				list.Add(onNurbsSurface2.BrepForm());
				OnBrep onBrep = CSUtilRhino.CapPlanarHoles(CSUtilRhino.JoinAllBreps(list.ToArray(), 0.01), 0.01);
				if (onBrep != null)
				{
					onBrep.Transform(xUp.Convert());
					CSData data = new CSOnGeometryData(onBrep.Convert(), xUp, applyTransform: false);
					cSDataArray2.Append(data);
				}
			}
			cSDataArray.Append(cSDataArray2);
		}
		OObjects.SetData(cSDataArray);
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
