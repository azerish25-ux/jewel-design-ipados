using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Rails;

[CSBlock("RingRailCustom", "{B14A1813-4240-4a64-981D-8DB2CFE8D82C}")]
public sealed class CSRingRailCustom : CSBlock
{
	public CSInputChannel iRingRadius { get; }

	public CSInputChannel iCurve { get; }

	public CSInputChannel iSpan { get; }

	public CSInputChannel iBlend { get; }

	public CSInputChannel iWidthLock { get; }

	public CSInputChannel iWidth { get; }

	public CSOutputChannel oRingRail { get; }

	public CSRingRailCustom(string identifier)
		: base(identifier)
	{
		iRingRadius = new CSInputChannel(GetPropertyName(() => iRingRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iSpan = new CSInputChannel(GetPropertyName(() => iSpan), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlend = new CSInputChannel(GetPropertyName(() => iBlend), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWidthLock = new CSInputChannel(GetPropertyName(() => iWidthLock), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWidth = new CSInputChannel(GetPropertyName(() => iWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRingRail = new CSOutputChannel(GetPropertyName(() => oRingRail), this, ChannelDataType.RingRail, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		double value = 0.0;
		if (!iRingRadius.GetDouble(ref value))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Ring Rail\n");
		}
		OnNurbsCurve nurbsCurve = iCurve.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Ring Rail Custom:  Missing Curve\n");
		}
		double value2 = 0.0;
		if (!iSpan.GetDouble(ref value2))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Span\n");
		}
		double value3 = 0.0;
		if (!iBlend.GetDouble(ref value3))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Blend\n");
		}
		double value4 = 0.0;
		iWidthLock.GetDouble(ref value4);
		double value5 = 0.0;
		iWidth.GetDouble(ref value5);
		if (value2 > 340.0)
		{
			value2 = 340.0;
		}
		if (value3 < 10.0)
		{
			value3 = 10.0;
		}
		if (value2 + value3 > 350.0)
		{
			value3 = 350.0 - value2;
		}
		if (value2 == 0.0 || value5 == 0.0)
		{
			OnCircle onCircle = new OnCircle(OnUtil.On_zx_plane, OnUtil.On_origin, value);
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			onCircle.GetNurbForm(ref nurbs_curve);
			oRingRail.SetData(new CSRingRailData(nurbs_curve.Convert(), value));
		}
		else
		{
			if (nurbsCurve.TangentAt(nurbsCurve.Domain().Min()).x < 0.0)
			{
				nurbsCurve.Reverse();
			}
			OnBoundingBox tight_bbox = new OnBoundingBox();
			if (!nurbsCurve.GetTightBoundingBox(ref tight_bbox))
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			double num = value2 / 360.0;
			double num2 = tight_bbox.Max().x - tight_bbox.Min().x;
			double num3 = num2 / num;
			double num4 = 1.0 - (value2 / 360.0 + value3 / 360.0);
			double num5 = num3 * num4;
			OnXform onXform = new OnXform();
			onXform.Translation(new On3dVector(0.0 - (tight_bbox.Max().x - num2 / 2.0), 0.0, 0.0));
			nurbsCurve.Transform(onXform);
			On3dPoint arg = new On3dPoint(0.0 - num3 / 2.0, 0.0, 0.0);
			On3dPoint arg2 = new On3dPoint(0.0 - num3 / 2.0 + num5 / 2.0, 0.0, 0.0);
			OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
			On3dPoint arg3 = new On3dPoint(num3 / 2.0 - num5 / 2.0, 0.0, 0.0);
			On3dPoint arg4 = new On3dPoint(num3 / 2.0, 0.0, 0.0);
			OnLineCurve onLineCurve2 = new OnLineCurve(arg3, arg4);
			On3dPoint pointStart = onLineCurve.PointAt(onLineCurve.Domain().Max());
			On3dVector on3dVector = onLineCurve.TangentAt(onLineCurve.Domain().Max());
			on3dVector.Unitize();
			On3dPoint pointEnd = nurbsCurve.PointAt(nurbsCurve.Domain().Min());
			On3dVector on3dVector2 = nurbsCurve.TangentAt(nurbsCurve.Domain().Min());
			on3dVector2.Unitize();
			on3dVector2.Reverse();
			OnNurbsCurve onNurbsCurve = CSUtilCurves.BlendCurve(pointStart, pointEnd, on3dVector, on3dVector2);
			On3dPoint pointStart2 = nurbsCurve.PointAt(nurbsCurve.Domain().Max());
			On3dVector on3dVector3 = nurbsCurve.TangentAt(nurbsCurve.Domain().Max());
			on3dVector3.Unitize();
			On3dPoint pointEnd2 = onLineCurve2.PointAt(onLineCurve2.Domain().Min());
			On3dVector on3dVector4 = onLineCurve2.TangentAt(onLineCurve2.Domain().Min());
			on3dVector4.Unitize();
			on3dVector4.Reverse();
			OnNurbsCurve onNurbsCurve2 = CSUtilCurves.BlendCurve(pointStart2, pointEnd2, on3dVector3, on3dVector4);
			OnCurve[] obj = new OnCurve[5] { onLineCurve, onNurbsCurve, nurbsCurve, onNurbsCurve2, onLineCurve2 };
			OnCurve[] output = null;
			IOnCurve[] input_curves = obj;
			RhUtil.RhinoMergeCurves(input_curves, ref output);
			if (output.Length != 1)
			{
				return UpdateError("RhinoMergeCurves failed");
			}
			OnXform onXform2 = new OnXform();
			onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_zaxis, OnUtil.On_origin);
			output[0].Transform(onXform2);
			if (!output[0].GetTightBoundingBox(ref tight_bbox))
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			num2 = tight_bbox.Max().x - tight_bbox.Min().x;
			double num6 = tight_bbox.Max().y - tight_bbox.Min().y;
			if (value4 > 0.5)
			{
				value5 = num2;
			}
			OnXform onXform3 = new OnXform();
			onXform3.Translation(0.0 - tight_bbox.Min().x, 0.0 - tight_bbox.Min().y, 0.0);
			output[0].Transform(onXform3);
			OnCircle onCircle2 = new OnCircle(OnUtil.On_zx_plane, OnUtil.On_origin, value);
			OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
			onCircle2.GetNurbForm(ref nurbs_curve2);
			OnSurface onSurface = RhUtil.RhinoExtrudeCurveStraight(nurbs_curve2, OnUtil.On_yaxis, 0.0 - value5);
			if (onSurface == null)
			{
				return UpdateError("RhinoExtrudeCurveStraight failed");
			}
			OnXform onXform4 = new OnXform();
			onXform4.Translation((value4 > 0.5) ? new On3dVector(0.0, Math.Abs(tight_bbox.Min().x), 0.0) : new On3dVector(0.0, value5 * (Math.Abs(tight_bbox.Min().x) / num2), 0.0));
			onSurface.Transform(onXform4);
			OnInterval onInterval = onSurface.Domain(0);
			OnInterval onInterval2 = onSurface.Domain(1);
			OnXform onXform5 = new OnXform();
			onXform5.Scale((onInterval[1] - onInterval[0]) / num2, (onInterval2[1] - onInterval2[0]) / num6, 1.0);
			output[0].Transform(onXform5);
			OnXform onXform6 = new OnXform();
			onXform6.Translation(onInterval[0], onInterval2[0], 0.0);
			output[0].Transform(onXform6);
			OnCurve onCurve = onSurface.Pushup(output[0], CSApplicationHelper.Application.Tolerance);
			if (onCurve == null)
			{
				return UpdateError("Pushup failed");
			}
			OnXform onXform7 = new OnXform();
			onXform7.Rotation(OnUtil.On_PI, OnUtil.On_yaxis, OnUtil.On_origin);
			onCurve.Transform(onXform7);
			On3dPoint on3dPoint = onCurve.PointAtStart();
			double num7 = 0.0;
			bool flag = false;
			if (on3dPoint.x == 0.0 && on3dPoint.z < 0.0)
			{
				num7 = onCurve.Domain().Min();
				flag = true;
			}
			if (!flag)
			{
				OnLineCurve curveB = new OnLineCurve(new On3dPoint(0.0, -50.0, 0.0 - value), new On3dPoint(0.0, 50.0, 0.0 - value));
				ArrayOnX_EVENT x = new ArrayOnX_EVENT();
				onCurve.IntersectCurve(curveB, ref x);
				if (x.Count() > 0)
				{
					double num8 = 0.0;
					int index = 0;
					for (int i = 0; i < x.Count(); i++)
					{
						double num9 = Math.Abs(onCurve.PointAt(x[i].get_m_a(0)).y);
						if (i == 0 || num9 < num8)
						{
							num8 = num9;
							index = i;
						}
					}
					flag = true;
					num7 = x[index].get_m_a(0);
				}
			}
			if (flag)
			{
				On3dVector on3dVector5 = onCurve.TangentAt(num7);
				double length = 0.0;
				double length2 = 0.0;
				double t = 0.0;
				onCurve.GetLength(ref length);
				if (onCurve.Domain().Min() == num7)
				{
					length2 = 0.0;
				}
				else
				{
					OnCurve onCurve2 = onCurve.Duplicate();
					onCurve2.Trim(new OnInterval(onCurve2.Domain().Min(), num7));
					onCurve2.GetLength(ref length2);
				}
				if (length2 > length / 2.0)
				{
					double s = (length2 - length / 2.0) / length;
					onCurve.GetNormalizedArcLengthPoint(s, ref t);
					onCurve.ChangeClosedCurveSeam(t);
					RhUtil.RhinoMakeCurveClosed(onCurve, 0.01);
				}
				else
				{
					double s2 = 1.0 - (length / 2.0 - length2) / length;
					onCurve.GetNormalizedArcLengthPoint(s2, ref t);
					onCurve.ChangeClosedCurveSeam(t);
					RhUtil.RhinoMakeCurveClosed(onCurve, 0.01);
				}
				if (on3dVector5.x > 0.0)
				{
					onCurve.Reverse();
				}
			}
			oRingRail.SetData(new CSRingRailData(onCurve.Convert(), value));
		}
		return base.Update();
	}
}
