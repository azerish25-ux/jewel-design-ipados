using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Curves;

[CSBlock("CurveFrom2View", "{6CB790A8-EDCB-4508-8AAA-49BA74CCD4D2}")]
public sealed class CSCurveFrom2View : CSBlock
{
	public CSInputChannel iCurve1 { get; }

	public CSInputChannel iCurve2 { get; }

	public CSInputChannel iMatchCurve { get; }

	public CSOutputChannel oCurve { get; }

	public CSCurveFrom2View(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(GetPropertyName(() => iCurve1), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(GetPropertyName(() => iCurve2), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iMatchCurve = new CSInputChannel(GetPropertyName(() => iMatchCurve), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve1.GetFlattenArray();
		CSDataArray flattenArray2 = iCurve2.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve 1 is missing");
		}
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve 2 is missing");
		}
		OnNurbsCurve onNurbsCurve = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		OnNurbsCurve onNurbsCurve2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onNurbsCurve == null)
		{
			return UpdateMissing("Curve 1 is missing");
		}
		if (onNurbsCurve2 == null)
		{
			return UpdateMissing("Curve 2 is missing");
		}
		OnPlane onPlane = new OnPlane();
		OnPlane onPlane2 = new OnPlane();
		if (!onNurbsCurve.IsPlanar(onPlane))
		{
			return UpdateMissing("Curve 1 is not planar");
		}
		if (!onNurbsCurve2.IsPlanar(onPlane2))
		{
			return UpdateMissing("Curve 2 is not planar");
		}
		ValueListItem whichItem = ValueListItem.OrientCurveSecond;
		iMatchCurve.GetValueListItem(ref whichItem);
		OnBoundingBox bbox = new OnBoundingBox();
		OnBoundingBox bbox2 = new OnBoundingBox();
		onNurbsCurve.GetBoundingBox(ref bbox);
		onNurbsCurve2.GetBoundingBox(ref bbox2);
		On3dPoint arg = new On3dPoint();
		On3dPoint arg2 = new On3dPoint();
		bbox.GetFarPoint(bbox2, ref arg, ref arg2);
		double num = arg.DistanceTo(arg2);
		On3dVector on3dVector = new On3dVector(onPlane.Normal());
		On3dVector on3dVector2 = new On3dVector(onPlane2.Normal());
		OnNurbsCurve onNurbsCurve3 = onNurbsCurve.Duplicate();
		OnNurbsCurve onNurbsCurve4 = onNurbsCurve.Duplicate();
		OnNurbsCurve onNurbsCurve5 = onNurbsCurve2.Duplicate();
		OnNurbsCurve onNurbsCurve6 = onNurbsCurve2.Duplicate();
		onNurbsCurve3.Translate(on3dVector * num);
		onNurbsCurve5.Translate(on3dVector2 * num);
		onNurbsCurve4.Translate(on3dVector * (0.0 - num));
		onNurbsCurve6.Translate(on3dVector2 * (0.0 - num));
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		onNurbsSurface.CreateRuledSurface(onNurbsCurve3, onNurbsCurve4);
		OnNurbsSurface onNurbsSurface2 = new OnNurbsSurface();
		onNurbsSurface2.CreateRuledSurface(onNurbsCurve5, onNurbsCurve6);
		RhUtil.RhinoIntersectSurfaces(onNurbsSurface, onNurbsSurface2, CSApplicationHelper.Application.Tolerance, out var curves, out var _);
		OnCurve[] output = new OnCurve[1];
		IOnCurve[] input_curves = curves;
		RhUtil.RhinoMergeCurves(input_curves, ref output);
		if (output == null)
		{
			return UpdateError("Error with merge");
		}
		OnLineCurve curveB = ((whichItem == ValueListItem.Unknown) ? new OnLineCurve(onNurbsCurve3.PointAtStart(), onNurbsCurve4.PointAtStart()) : new OnLineCurve(onNurbsCurve5.PointAtStart(), onNurbsCurve6.PointAtStart()));
		base.OutputChannels.Clear();
		for (int i = 0; i < output.Length; i++)
		{
			OnCurve onCurve = output[i];
			if (onCurve == null)
			{
				continue;
			}
			ArrayOnX_EVENT x = new ArrayOnX_EVENT();
			onCurve.IntersectCurve(curveB, ref x);
			if (x.Count() == 1)
			{
				onCurve.ChangeClosedCurveSeam(x[0].get_m_a(0));
			}
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			onCurve.GetNurbForm(ref nurbs_curve);
			if (nurbs_curve != null)
			{
				double s = 0.0;
				double t = 0.0;
				On3dPoint on3dPoint = nurbs_curve.PointAtStart();
				onNurbsSurface.GetClosestPoint(on3dPoint, ref s, ref t);
				On3dVector on3dVector3 = nurbs_curve.TangentAt(nurbs_curve.Domain().Min());
				double s2 = 0.0;
				double t2 = 0.0;
				onNurbsSurface.GetClosestPoint(on3dPoint + on3dVector3 / 100.0, ref s2, ref t2);
				double t3 = 0.0;
				double t4 = 0.0;
				onNurbsSurface.GetDomain(0, ref t3, ref t4);
				double num2 = Math.Abs(t3 - t4);
				bool flag = Math.Abs(s - s2) > num2 * 0.9;
				if (s > s2 != flag)
				{
					nurbs_curve.Reverse();
				}
				new CSOutputChannel($"{base.Identifier}_{i}", this, ChannelDataType.Curve, ChannelArrayType.Individual).SetData(new CSOnGeometryData(nurbs_curve.Convert()));
			}
		}
		return base.Update();
	}
}
