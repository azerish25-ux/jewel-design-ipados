using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadRail_V1", "{6865A811-1C30-4cc3-A55D-513C06A2D09E}")]
public sealed class CSHeadRail1 : CSBlock
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

	public CSOutputChannel ORails { get; }

	public CSHeadRail1(string identifier)
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
		ORails = new CSOutputChannel(GetPropertyName(() => ORails), this, ChannelDataType.Any, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = IGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Gems");
		}
		if (!(flattenArray.GetArrayItem(0) is CSGemData cSGemData))
		{
			return UpdateMissing("Gems");
		}
		OnNurbsCurve nurbsCurve = IHeadSideX.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing Head X Curve");
		}
		OnNurbsCurve nurbsCurve2 = IHeadSideY.GetNurbsCurve();
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("Missing Head Y Curve");
		}
		double value = 0.0;
		IWhichRail.GetDouble(ref value);
		double value2 = 0.0;
		IRailCount.GetDouble(ref value2);
		if (value > value2)
		{
			return base.Update();
		}
		double value3 = 0.0;
		IRailZPos.GetDouble(ref value3);
		bool value4 = false;
		IEnableSecondControl.GetBoolean(ref value4);
		double value5 = 1.0;
		double value6 = 1.0;
		OnNurbsCurve nurbsCurve3;
		if (!value4)
		{
			nurbsCurve3 = IRailProfile.GetNurbsCurve();
			if (nurbsCurve3 == null)
			{
				return UpdateMissing("Missing Rail Profile");
			}
			IRailX.GetDouble(ref value5);
			IRailY.GetDouble(ref value6);
		}
		else
		{
			nurbsCurve3 = IRailProfile2.GetNurbsCurve();
			if (nurbsCurve3 == null)
			{
				return UpdateMissing("Missing Rail Profile 2");
			}
			IRailX2.GetDouble(ref value5);
			IRailY2.GetDouble(ref value6);
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform.Convert(), out var xUp, out var xDown);
		double num = nurbsCurve.PointAtEnd().z * (1.0 - value3);
		OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, 0.0, num), new On3dPoint(100.0, 0.0, num));
		OnLineCurve onLineCurve2 = new OnLineCurve(new On3dPoint(0.0, 0.0, num), new On3dPoint(0.0, 100.0, num));
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
		onLineCurve.IntersectCurve(nurbsCurve, ref x);
		onLineCurve2.IntersectCurve(nurbsCurve2, ref x2);
		if (x.Count() <= 0)
		{
			return UpdateError("Intersect X");
		}
		if (x2.Count() <= 0)
		{
			return UpdateError("Intersect Y");
		}
		double x3 = x[0].get_m_pointA(0).x;
		double y = x2[0].get_m_pointA(0).y;
		double t = x[0].get_m_b(0);
		double num2 = 0.0 - nurbsCurve.TangentAt(t).x;
		CSGemData cSGemData2 = new CSGemData(cSGemData);
		cSGemData2.ApplyTransform(xDown.Convert());
		OnXform onXform = new OnXform();
		onXform.Scale(new On3dVector(x3 / cSGemData.GemXMax, y / cSGemData.GemYMax, 1.0));
		cSGemData2.ApplyTransform(onXform.Convert());
		if (!GemOffsetCurveV1.GemOffset(cSGemData2, 0.0, out OnCurve curve, false, out string error))
		{
			return UpdateError(error);
		}
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		curve.GetNurbForm(ref nurbs_curve);
		if (!GemOffsetCurveV1.GemOffset(cSGemData2, 0.0 - value5, out curve, false, out error))
		{
			return UpdateError(error);
		}
		OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
		curve.GetNurbForm(ref nurbs_curve2);
		On3dPoint on3dPoint = cSGemData2.GetNurbsCurve().PointAt(cSGemData.BezelParam).Convert();
		double t2 = 0.0;
		nurbs_curve.GetClosestPoint(on3dPoint, ref t2);
		On3dPoint on3dPoint2 = nurbs_curve.PointAt(t2);
		On3dVector on3dVector = OnUtil.ON_CrossProduct(nurbs_curve.TangentAt(t2), new On3dVector(0.0, 0.0, 1.0));
		on3dVector.Unitize();
		on3dVector *= value5;
		double t3 = 0.0;
		nurbs_curve2.GetClosestPoint(on3dPoint2 + on3dVector, ref t3);
		OnBoundingBox tight_bbox = new OnBoundingBox();
		nurbsCurve3.GetTightBoundingBox(ref tight_bbox);
		OnXform onXform2 = new OnXform();
		onXform2.Translation(0.0 - tight_bbox.m_min.x, 0.0 - tight_bbox.m_min.y, 0.0 - tight_bbox.m_min.z);
		OnXform onXform3 = new OnXform();
		onXform3.Scale(value5 / (tight_bbox.m_max.x - tight_bbox.m_min.x), value6 / (tight_bbox.m_max.y - tight_bbox.m_min.y), 0.0);
		OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(value5, 0.0, 0.0), new On3dVector(-1.0, 0.0, 0.0), new On3dVector(0.0, 1.0, 0.0), on3dPoint, on3dVector, new On3dVector(0.0, 0.0, 1.0));
		nurbsCurve3.Transform(onXform3 * onXform2);
		for (int i = 0; i < nurbsCurve3.m_cv_count; i++)
		{
			On4dPoint vertex_value = new On4dPoint();
			nurbsCurve3.GetCV(i, ref vertex_value);
			vertex_value.x += num2 * vertex_value.y;
			nurbsCurve3.SetCV(i, vertex_value);
		}
		nurbsCurve3.Transform(xform);
		if (!CSUtilSweep.Sweep2(profiles: new IOnCurve[1] { nurbsCurve3 }, parameters1In: new double[1] { t2 }, parameters2In: new double[1] { t3 }, railCurve1: nurbs_curve, railCurve2: nurbs_curve2, breps: out var breps, closed: true, maintainHeight: true))
		{
			return UpdateError("Sweep 2");
		}
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0, 0.0, num);
		nurbs_curve.Transform(onXform4);
		nurbs_curve2.Transform(onXform4);
		nurbsCurve3.Transform(onXform4);
		breps[0].Transform(onXform4);
		breps[0].Transform(xUp);
		ORails.SetData(new CSOnGeometryData(breps[0].Convert(), xUp.Convert() * onXform4.Convert(), applyTransform: false));
		return base.Update();
	}
}
