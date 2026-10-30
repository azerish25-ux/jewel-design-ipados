using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadRail_V2", "{357943F0-C64C-4226-87DE-DA19755111BD}")]
public sealed class CSHeadRail2 : CSBlock
{
	public CSInputChannel IGem { get; }

	public CSInputChannel IHeadSideX { get; }

	public CSInputChannel IHeadSideY { get; }

	public CSInputChannel IWhichRail { get; }

	public CSInputChannel IRailCount { get; }

	public CSInputChannel IRailZPos { get; }

	public CSInputChannel IRailProfile { get; }

	public CSInputChannel IRailX { get; }

	public CSInputChannel IRailY { get; }

	public CSInputChannel IEnableSecondControl { get; }

	public CSInputChannel IRailProfile2 { get; }

	public CSInputChannel IRailX2 { get; }

	public CSInputChannel IRailY2 { get; }

	public CSInputChannel IObjects { get; }

	public CSOutputChannel OObjects { get; }

	public CSHeadRail2(string identifier)
		: base(identifier)
	{
		IGem = new CSInputChannel(GetPropertyName(() => IGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		IHeadSideX = new CSInputChannel(GetPropertyName(() => IHeadSideX), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IHeadSideY = new CSInputChannel(GetPropertyName(() => IHeadSideY), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IWhichRail = new CSInputChannel(GetPropertyName(() => IWhichRail), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IRailCount = new CSInputChannel(GetPropertyName(() => IRailCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IRailZPos = new CSInputChannel(GetPropertyName(() => IRailZPos), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IRailProfile = new CSInputChannel(GetPropertyName(() => IRailProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IRailX = new CSInputChannel(GetPropertyName(() => IRailX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IRailY = new CSInputChannel(GetPropertyName(() => IRailY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IEnableSecondControl = new CSInputChannel(GetPropertyName(() => IEnableSecondControl), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		IRailProfile2 = new CSInputChannel(GetPropertyName(() => IRailProfile2), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IRailX2 = new CSInputChannel(GetPropertyName(() => IRailX2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IRailY2 = new CSInputChannel(GetPropertyName(() => IRailY2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IObjects = new CSInputChannel(GetPropertyName(() => IObjects), this, ChannelDataType.Any, ChannelArrayType.Individual);
		OObjects = new CSOutputChannel(GetPropertyName(() => OObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = IGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Gem input missing");
		}
		double value = 0.0;
		IWhichRail.GetDouble(ref value);
		double value2 = 0.0;
		IRailCount.GetDouble(ref value2);
		double value3 = 0.0;
		IRailZPos.GetDouble(ref value3);
		bool value4 = false;
		IEnableSecondControl.GetBoolean(ref value4);
		double value5 = 1.0;
		double value6 = 1.0;
		OnNurbsCurve nurbsCurve;
		if (!value4)
		{
			nurbsCurve = IRailProfile.GetNurbsCurve();
			if (nurbsCurve == null)
			{
				return UpdateMissing("Missing rail profile");
			}
			IRailX.GetDouble(ref value5);
			IRailY.GetDouble(ref value6);
		}
		else
		{
			nurbsCurve = IRailProfile2.GetNurbsCurve();
			if (nurbsCurve == null)
			{
				return UpdateMissing("Missing rail profile 2");
			}
			IRailX2.GetDouble(ref value5);
			IRailY2.GetDouble(ref value6);
		}
		ICSArray iCSArray = IObjects.Data as ICSArray;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataArray cSDataArray2 = new CSDataArray();
			if (iCSArray != null && iCSArray.GetArrayCount() > 0)
			{
				if (!(iCSArray.GetArrayItem(i) is ICSArray iCSArray2))
				{
					continue;
				}
				for (int j = 0; j < iCSArray2.GetArrayCount(); j++)
				{
					cSDataArray2.Append(iCSArray2.GetArrayItem(j));
				}
			}
			if (value <= value2)
			{
				if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
				{
					return UpdateError("Invalid gem");
				}
				OnNurbsCurve onNurbsCurve = IHeadSideX.GetArrayItem(i).GetNurbsCurve().Convert();
				if (onNurbsCurve == null)
				{
					return UpdateError("Invalid side X curve");
				}
				OnNurbsCurve onNurbsCurve2 = IHeadSideY.GetArrayItem(i).GetNurbsCurve().Convert();
				if (onNurbsCurve2 == null)
				{
					return UpdateError("Invalid side Y curve");
				}
				CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform.Convert(), out var xUp, out var xDown);
				double num = onNurbsCurve.PointAtEnd().z * (1.0 - value3);
				OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, 0.0, num), new On3dPoint(100.0, 0.0, num));
				OnLineCurve onLineCurve2 = new OnLineCurve(new On3dPoint(0.0, 0.0, num), new On3dPoint(0.0, 100.0, num));
				ArrayOnX_EVENT x = new ArrayOnX_EVENT();
				ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
				onLineCurve.IntersectCurve(onNurbsCurve, ref x);
				onLineCurve2.IntersectCurve(onNurbsCurve2, ref x2);
				if (x.Count() <= 0)
				{
					return UpdateError("IntersectCurve X curve");
				}
				if (x2.Count() <= 0)
				{
					return UpdateError("IntersectCurve Y curve");
				}
				double x3 = x[0].get_m_pointA(0).x;
				double y = x2[0].get_m_pointA(0).y;
				double t = x[0].get_m_b(0);
				double num2 = 0.0 - onNurbsCurve.TangentAt(t).x;
				CSGemData cSGemData2 = new CSGemData(cSGemData);
				cSGemData2.ApplyTransform(xDown.Convert());
				OnXform onXform = new OnXform();
				onXform.Scale(new On3dVector(x3 / cSGemData.GemXMax, y / cSGemData.GemYMax, 1.0));
				cSGemData2.ApplyTransform(onXform.Convert());
				if (!GemOffsetCurveV1.GemOffset(cSGemData2, 0.0, out OnCurve curve, false, out string error))
				{
					return UpdateError("Curve Rail:" + error);
				}
				if (!GemOffsetCurveV1.GemOffset(cSGemData2, 0.0 - value5, out OnCurve curve2, false, out error))
				{
					return UpdateError("Curve Rail In:" + error);
				}
				On3dPoint on3dPoint = cSGemData2.GetNurbsCurve().PointAt(cSGemData.BezelParam).Convert();
				double t2 = 0.0;
				curve.GetClosestPoint(on3dPoint, ref t2);
				On3dPoint on3dPoint2 = curve.PointAt(t2);
				On3dVector on3dVector = OnUtil.ON_CrossProduct(curve.TangentAt(t2), new On3dVector(0.0, 0.0, 1.0));
				on3dVector.Unitize();
				on3dVector *= value5;
				double t3 = 0.0;
				curve2.GetClosestPoint(on3dPoint2 + on3dVector, ref t3);
				OnNurbsCurve onNurbsCurve3 = nurbsCurve.Duplicate();
				OnBoundingBox tight_bbox = new OnBoundingBox();
				onNurbsCurve3.GetTightBoundingBox(ref tight_bbox);
				OnXform onXform2 = new OnXform();
				onXform2.Translation(0.0 - tight_bbox.m_min.x, 0.0 - tight_bbox.m_min.y, 0.0 - tight_bbox.m_min.z);
				OnXform onXform3 = new OnXform();
				onXform3.Scale(value5 / (tight_bbox.m_max.x - tight_bbox.m_min.x), value6 / (tight_bbox.m_max.y - tight_bbox.m_min.y), 0.0);
				OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(value5, 0.0, 0.0), new On3dVector(-1.0, 0.0, 0.0), new On3dVector(0.0, 1.0, 0.0), on3dPoint, on3dVector, new On3dVector(0.0, 0.0, 1.0));
				onNurbsCurve3.Transform(onXform3 * onXform2);
				for (int k = 0; k < onNurbsCurve3.m_cv_count; k++)
				{
					On4dPoint vertex_value = new On4dPoint();
					onNurbsCurve3.GetCV(k, ref vertex_value);
					vertex_value.x += num2 * vertex_value.y;
					onNurbsCurve3.SetCV(k, vertex_value);
				}
				onNurbsCurve3.Transform(xform);
				if (!CSUtilSweep.Sweep2(profiles: new IOnCurve[1] { onNurbsCurve3 }, parameters1In: new double[1] { t2 }, parameters2In: new double[1] { t3 }, railCurve1: curve, railCurve2: curve2, breps: out var breps, closed: true, maintainHeight: true))
				{
					return UpdateError("Sweep 2");
				}
				OnXform onXform4 = new OnXform();
				onXform4.Translation(0.0, 0.0, num);
				if (breps.Length == 0)
				{
					return UpdateError("Sweep 1 rails");
				}
				breps[0].Transform(xUp * onXform4);
				CSData data = new CSOnGeometryData(breps[0].Convert(), xUp.Convert() * onXform4.Convert(), applyTransform: false);
				cSDataArray2.Append(data);
			}
			cSDataArray.Append(cSDataArray2);
		}
		OObjects.SetData(cSDataArray);
		return base.Update();
	}
}
