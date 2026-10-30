using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("SurfaceBezelOutsideOnRingRail_V1", "{D7741502-5975-4aca-8995-15302B797A5B}")]
public sealed class CSSurfaceBezelOutsideOnRingRail1 : CSBlock
{
	public CSInputChannel iOutsideRail { get; }

	public CSInputChannel iOutsideProfile { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iSeatHeight { get; }

	public CSInputChannel iTube { get; }

	public CSOutputChannel oBrep { get; }

	public CSOutputChannel oCurve { get; }

	public CSSurfaceBezelOutsideOnRingRail1(string identifier)
		: base(identifier)
	{
		iOutsideRail = new CSInputChannel(GetPropertyName(() => iOutsideRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOutsideProfile = new CSInputChannel(GetPropertyName(() => iOutsideProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSeatHeight = new CSInputChannel(GetPropertyName(() => iSeatHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTube = new CSInputChannel(GetPropertyName(() => iTube), this, ChannelDataType.Surface, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iOutsideRail.GetFlattenArray();
		OnNurbsCurve nurbsCurve = iOutsideProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing outside profile curve");
		}
		double value = 0.0;
		iPlacement.GetDouble(ref value);
		double value2 = 0.0;
		iAngle.GetDouble(ref value2);
		double value3 = 0.0;
		iSeatHeight.GetDouble(ref value3);
		OnNurbsSurface nurbsSurface = iTube.GetNurbsSurface();
		CSData outputSurfaceData = null;
		CSDataArray cSDataArray = new CSDataArray();
		CSData outputCurveData = null;
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value4);
			if (!value4.IsValid)
			{
				value4 = Transform.Identity;
			}
			value4.TryGetInverse(out var inverseTransform);
			OnNurbsCurve onNurbsCurve = arrayItem.GetNurbsCurve().Convert();
			if (onNurbsCurve != null)
			{
				OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
				onNurbsCurve2.Transform(inverseTransform.Convert());
				OnNurbsCurve onNurbsCurve3 = nurbsCurve.Duplicate();
				if (onNurbsCurve3 == null)
				{
					return UpdateError("Duplicate failed");
				}
				if (!BezelOutsideOnRingRail(value4.Convert(), onNurbsCurve2, onNurbsCurve3, value, value2, value3, nurbsSurface, ref outputSurfaceData, ref outputCurveData))
				{
					return UpdateError("BezelOutsideOnRingRail failed");
				}
				cSDataArray.Append(outputSurfaceData);
				cSDataArray2.Append(outputCurveData);
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oBrep.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oBrep.SetData(cSDataArray);
		}
		if (cSDataArray2.GetArrayCount() == 1)
		{
			oCurve.SetData(cSDataArray2.GetArrayItem(0));
		}
		else
		{
			oCurve.SetData(cSDataArray2);
		}
		return base.Update();
	}

	private static bool BezelOutsideOnRingRail(OnXform xUp, OnNurbsCurve curveNC, OnNurbsCurve profileNC, double placement, double angle, double seatHeight, OnNurbsSurface tube, ref CSData outputSurfaceData, ref CSData outputCurveData)
	{
		if (tube == null)
		{
			return false;
		}
		On3dPoint on3dPoint = new On3dPoint();
		On3dPoint on3dPoint2 = new On3dPoint();
		On3dPoint on3dPoint3 = new On3dPoint();
		on3dPoint2 = profileNC.PointAtStart();
		On3dVector on3dVector = new On3dVector(on3dPoint2);
		on3dVector.Reverse();
		OnXform onXform = new OnXform();
		onXform.Translation(on3dVector);
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform onXform3 = new OnXform();
		onXform3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_zaxis, OnUtil.On_origin);
		profileNC.Transform(onXform3 * onXform2 * onXform);
		on3dPoint3 = profileNC.PointAtEnd();
		double num = CSBlockUtil.CSUtilMath.AngleOfPointD(on3dPoint3.y, 0.0 - on3dPoint3.z);
		OnXform onXform4 = new OnXform();
		onXform4.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle) - num, OnUtil.On_xaxis, OnUtil.On_origin);
		profileNC.Transform(onXform4);
		OnLineCurve onLineCurve = new OnLine(OnUtil.On_origin, new On3dPoint(0.0, 0.0, -50.0));
		onLineCurve.Transform(xUp);
		OnCurve[] curves = null;
		On3dPointArray points = null;
		if (!RhUtil.RhinoCurveBrepIntersect(onLineCurve, tube.BrepForm(), CSApplicationHelper.Application.Tolerance, out curves, out points))
		{
			return false;
		}
		if (curves.Length != 0)
		{
			OnCurve[] array = curves;
			foreach (OnCurve onCurve in array)
			{
				points.Append(onCurve.PointAtStart());
				points.Append(onCurve.PointAtEnd());
			}
		}
		double num2 = 10.0;
		for (int j = 0; j < points.Count(); j++)
		{
			double num3 = onLineCurve.PointAtStart().DistanceTo(points[j]);
			if (j == 0 || num3 < num2)
			{
				num2 = num3;
			}
		}
		num2 += placement + seatHeight;
		double num4 = num2 / (0.0 - profileNC.PointAtEnd().z);
		onXform.Scale(num4, num4, num4);
		profileNC.Transform(onXform);
		on3dPoint = curveNC.PointAtStart();
		on3dPoint2 = profileNC.PointAtStart();
		onXform.Translation(on3dPoint - on3dPoint2);
		profileNC.Transform(onXform);
		OnBrep onBrep = CSUtilRailRevolve.RailRevolve(profileNC, curveNC, 0).BrepForm();
		onBrep.Transform(xUp);
		outputSurfaceData = new CSOnGeometryData(onBrep.Convert(), xUp.Convert(), applyTransform: false);
		On3dPoint on3dPoint4 = profileNC.PointAtStart();
		on3dPoint4.z = 0.0;
		On3dPoint on3dPoint5 = profileNC.PointAtEnd();
		on3dPoint5.z = 0.0;
		double num5 = on3dPoint4.DistanceTo(OnUtil.On_origin);
		num4 = on3dPoint5.DistanceTo(OnUtil.On_origin) / num5;
		onXform.Scale(num4, num4, 1.0);
		curveNC.Transform(onXform);
		onXform.Translation(0.0, 0.0, 0.0 - num2);
		curveNC.Transform(xUp * onXform);
		outputCurveData = new CSOnGeometryData(curveNC.Convert(), xUp.Convert() * onXform.Convert(), applyTransform: false);
		return true;
	}
}
