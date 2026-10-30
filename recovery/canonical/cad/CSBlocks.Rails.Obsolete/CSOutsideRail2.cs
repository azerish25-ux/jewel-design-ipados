using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Rails.Obsolete;

[Obsolete]
[CSBlock("OutsideRail_V2", "{342781AD-D61B-4986-B868-046655761C54}")]
public sealed class CSOutsideRail2 : CSBlock
{
	public CSInputChannel InRingRailRadius { get; }

	public CSInputChannel InShape { get; }

	public CSInputChannel InThicknessTop { get; }

	public CSInputChannel InThicknessSide { get; }

	public CSInputChannel InThicknessBottom { get; }

	public CSInputChannel InEnableLock { get; }

	public CSOutputChannel OutOutsideRingRail { get; }

	public CSOutsideRail2(string identifier)
		: base(identifier)
	{
		InRingRailRadius = new CSInputChannel(GetPropertyName(() => InRingRailRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShape = new CSInputChannel(GetPropertyName(() => InShape), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InThicknessTop = new CSInputChannel(GetPropertyName(() => InThicknessTop), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InThicknessSide = new CSInputChannel(GetPropertyName(() => InThicknessSide), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InThicknessBottom = new CSInputChannel(GetPropertyName(() => InThicknessBottom), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEnableLock = new CSInputChannel(GetPropertyName(() => InEnableLock), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutOutsideRingRail = new CSOutputChannel(GetPropertyName(() => OutOutsideRingRail), this, ChannelDataType.OutsideRingRail, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		double value = 3.0;
		InThicknessTop.GetDouble(ref value);
		double value2 = 2.0;
		InThicknessSide.GetDouble(ref value2);
		double value3 = 1.0;
		InThicknessBottom.GetDouble(ref value3);
		bool value4 = false;
		InEnableLock.GetBoolean(ref value4);
		double value5 = 0.0;
		if (!InRingRailRadius.GetDouble(ref value5))
		{
			OnNurbsCurve nurbsCurve = InRingRailRadius.GetNurbsCurve();
			if (nurbsCurve == null)
			{
				return UpdateMissing("Outside Ring Rail: Missing Ring Rail\n");
			}
			OnBoundingBox tight_bbox = new OnBoundingBox();
			nurbsCurve.GetTightBoundingBox(ref tight_bbox);
			value5 = tight_bbox.m_max.z;
		}
		double num = value5;
		double num2 = 0.0 - value5;
		double num3 = value5;
		double num4 = value5;
		OnNurbsCurve nurbsCurve2 = InShape.GetNurbsCurve();
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("Outside Ring Rail: Missing Custom Rail (No Curve)\n");
		}
		CSUtilGeometry.GetMinMax(nurbsCurve2, OnUtil.On_origin, 50, out var min, out var _);
		if (min == 0.0 || min == double.MaxValue)
		{
			return UpdateError("Intersect of custom Rail Failed\n");
		}
		if (num4 == 0.0)
		{
			return UpdateError("Intersect of custom Rail Failed\n");
		}
		OnNurbsCurve onNurbsCurve = nurbsCurve2.Duplicate();
		onNurbsCurve.MakeDeformable();
		onNurbsCurve.Scale(num4 / min);
		CSUtilRingRail.GetRingRailPointExtents(onNurbsCurve, out var pointExtentTop, out var pointExtentBottom, out var pointExtentRight, out var pointExtentLeft);
		if (!value4)
		{
			double num5 = pointExtentTop - pointExtentBottom;
			double num6 = num + value - pointExtentBottom;
			On3dPoint on3dPoint = new On3dPoint(0.0, 0.0, pointExtentBottom);
			On3dPoint on3dPoint2 = new On3dPoint(0.0, 0.0, pointExtentTop);
			OnXform onXform = new OnXform();
			onXform.Scale(new OnPlane(on3dPoint, OnUtil.On_xaxis, OnUtil.On_zaxis), 1.0, num6 / num5, 1.0);
			onNurbsCurve.Transform(onXform);
			CSUtilRingRail.GetRingRailPointExtents(onNurbsCurve, out pointExtentTop, out pointExtentBottom, out pointExtentRight, out pointExtentLeft);
			on3dPoint.z = pointExtentBottom;
			on3dPoint2.z = pointExtentTop;
			num5 = pointExtentTop - pointExtentBottom;
			num6 = pointExtentTop - (num2 - value3);
			OnXform onXform2 = new OnXform();
			onXform2.Scale(new OnPlane(on3dPoint2, OnUtil.On_xaxis, OnUtil.On_zaxis), 1.0, num6 / num5, 1.0);
			onNurbsCurve.Transform(onXform2);
			CSUtilRingRail.GetRingRailPointExtents(onNurbsCurve, out pointExtentTop, out pointExtentBottom, out pointExtentRight, out pointExtentLeft);
			num5 = pointExtentRight;
			num6 = num3 + value2;
			OnXform onXform3 = new OnXform();
			onXform3.Scale(num6 / num5, 1.0, 1.0);
			onNurbsCurve.Transform(onXform3);
		}
		else
		{
			OnLineCurve curveB = new OnLineCurve(new On3dPoint(-200.0, 0.0, 0.0), new On3dPoint(200.0, 0.0, 0.0));
			double num7 = pointExtentRight;
			double num8 = num3 + value2;
			OnXform onXform4 = new OnXform();
			onXform4.Scale(num8 / num7, 1.0, 1.0);
			onNurbsCurve.Transform(onXform4);
			ArrayOnX_EVENT x = new ArrayOnX_EVENT();
			onNurbsCurve.IntersectCurve(curveB, ref x);
			if (x.Count() != 2)
			{
				return UpdateError("Intersect of custom Rail Failed\n");
			}
			OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
			onNurbsCurve2.ChangeClosedCurveSeam(x[0].get_m_a(0));
			OnNurbsCurve onNurbsCurve3 = onNurbsCurve2.Duplicate();
			onNurbsCurve3.Reverse();
			double t = 0.0;
			onNurbsCurve2.GetClosestPoint(x[1].get_m_pointA(0), ref t);
			onNurbsCurve2.Trim(new OnInterval(onNurbsCurve2.Domain().Min(), t));
			onNurbsCurve3.GetClosestPoint(x[1].get_m_pointA(0), ref t);
			onNurbsCurve3.Trim(new OnInterval(onNurbsCurve3.Domain().Min(), t));
			if (onNurbsCurve2.PointAt(onNurbsCurve2.Domain().Mid()).z < 0.0)
			{
				OnNurbsCurve onNurbsCurve4 = onNurbsCurve2;
				onNurbsCurve2 = onNurbsCurve3;
				onNurbsCurve3 = onNurbsCurve4;
			}
			OnXform onXform5 = new OnXform();
			onXform5.Scale(1.0, 1.0, (value + num) / pointExtentTop);
			OnXform onXform6 = new OnXform();
			onXform6.Scale(1.0, 1.0, (0.0 - value3 + num2) / pointExtentBottom);
			onNurbsCurve2.Transform(onXform5);
			onNurbsCurve3.Transform(onXform6);
			OnCurve[] obj = new OnCurve[2] { onNurbsCurve2, onNurbsCurve3 };
			OnCurve[] output = null;
			IOnCurve[] input_curves = obj;
			RhUtil.RhinoMergeCurves(input_curves, ref output);
			if (output.Length != 0)
			{
				output[0].NurbsCurve(onNurbsCurve);
			}
			OnLineCurve curveB2 = new OnLineCurve(new On3dPoint(0.0, 0.0, 0.0), new On3dPoint(0.0, 0.0, 100.0));
			ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
			onNurbsCurve.IntersectCurve(curveB2, ref x2);
			if (x2.Count() > 0)
			{
				t = x2[0].get_m_a(0);
			}
			else
			{
				onNurbsCurve.GetClosestPoint(new On3dPoint(0.0, 0.0, 100.0), ref t);
			}
			onNurbsCurve.ChangeClosedCurveSeam(t);
			if (onNurbsCurve.TangentAt(onNurbsCurve.Domain().Min()) * OnUtil.On_xaxis < 0.0)
			{
				onNurbsCurve.Reverse();
			}
		}
		OutOutsideRingRail.SetData(new CSOutsideRingRailData(onNurbsCurve.Convert()));
		return base.Update();
	}
}
