using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Rails;

[CSBlock("RingRailTube", "{2D87963C-25DA-4d6a-850E-5229B36BD53A}")]
public sealed class CSRingRailTube : CSBlock
{
	public CSInputChannel InRingRail { get; }

	public CSInputChannel InLength { get; }

	public CSInputChannel InRingRailRadius { get; }

	public CSInputChannel InOffset { get; }

	public CSInputChannel InAngle { get; }

	public CSInputChannel InAngleOffset { get; }

	public CSOutputChannel OutSurface { get; }

	public CSOutputChannel OutTopLine { get; }

	public CSRingRailTube(string identifier)
		: base(identifier)
	{
		InRingRail = new CSInputChannel(GetPropertyName(() => InRingRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InLength = new CSInputChannel(GetPropertyName(() => InLength), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRingRailRadius = new CSInputChannel(GetPropertyName(() => InRingRailRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOffset = new CSInputChannel(GetPropertyName(() => InOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(GetPropertyName(() => InAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngleOffset = new CSInputChannel(GetPropertyName(() => InAngleOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(GetPropertyName(() => OutSurface), this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		OutTopLine = new CSOutputChannel(GetPropertyName(() => OutTopLine), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnCurve onCurve = InRingRail.GetNurbsCurve();
		if (onCurve == null)
		{
			double value = 0.0;
			InRingRailRadius.GetDouble(ref value);
			OnCircle onCircle = new OnCircle(OnUtil.On_zx_plane, OnUtil.On_origin, value);
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			onCircle.GetNurbForm(ref nurbs_curve);
			onCurve = nurbs_curve;
			if (onCurve == null)
			{
				return UpdateError("Failed to create ring rail from radius");
			}
		}
		double value2 = 60.0;
		InLength.GetDouble(ref value2);
		double value3 = 0.0;
		InOffset.GetDouble(ref value3);
		double value4 = 360.0;
		InAngle.GetDouble(ref value4);
		double value5 = 0.0;
		InAngleOffset.GetDouble(ref value5);
		if (Math.Abs(value3) > 0.01)
		{
			IOn3dVector normalDir = OnUtil.On_yaxis;
			OnBoundingBox bbox = new OnBoundingBox();
			onCurve.GetBoundingBox(ref bbox);
			IOn3dPoint origin = bbox.Center();
			OnPlane onPlane = new OnPlane();
			if (onCurve.IsPlanar(onPlane))
			{
				normalDir = new On3dVector(onPlane.Normal());
			}
			OnCurve onCurve2 = CSUtilGeometry.OffsetOld(onCurve, normalDir, origin, value3, closed: true);
			if (onCurve2 != null)
			{
				onCurve = onCurve2;
			}
		}
		if (value4 < 360.0 && value4 > 0.0)
		{
			OnBoundingBox bbox2 = new OnBoundingBox();
			onCurve.GetBoundingBox(ref bbox2);
			OnLine onLine = new OnLine(OnUtil.On_origin, new On3dPoint(0.0, 0.0, bbox2.Max().z * 2.0));
			OnXform onXform = new OnXform();
			onXform.Rotation(CSBlockUtil.CSUtilMath.ToRadians((0.0 - value4) / 2.0), OnUtil.On_yaxis, OnUtil.On_origin);
			onLine.Transform(onXform);
			OnLine onLine2 = new OnLine(OnUtil.On_origin, new On3dPoint(0.0, 0.0, bbox2.Max().z * 2.0));
			OnXform onXform2 = new OnXform();
			onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value4 / 2.0), OnUtil.On_yaxis, OnUtil.On_origin);
			onLine2.Transform(onXform2);
			On3dPointArray on3dPointArray = new On3dPointArray();
			on3dPointArray.Append(onLine.from);
			on3dPointArray.Append(onLine.to);
			On3dPoint on3dPoint = new On3dPoint(onLine.to.x + bbox2.Diagonal().x, onLine.to.y, onLine.to.z);
			if (onLine.to.x < 0.0)
			{
				on3dPoint = new On3dPoint(onLine.to.x - bbox2.Diagonal().x, onLine.to.y, onLine.to.z);
			}
			on3dPointArray.Append(on3dPoint);
			on3dPointArray.Append(new On3dPoint(on3dPoint.x, on3dPoint.y, on3dPoint.z + bbox2.Diagonal().z * 2.0));
			On3dPoint on3dPoint2 = new On3dPoint(onLine2.to.x + bbox2.Diagonal().x, onLine2.to.y, onLine2.to.z);
			if (onLine2.to.x < 0.0)
			{
				on3dPoint2 = new On3dPoint(onLine2.to.x - bbox2.Diagonal().x, onLine2.to.y, onLine2.to.z);
			}
			on3dPointArray.Append(new On3dPoint(on3dPoint2.x, on3dPoint2.y, on3dPoint2.z + bbox2.Diagonal().z * 2.0));
			on3dPointArray.Append(on3dPoint2);
			on3dPointArray.Append(onLine2.to);
			on3dPointArray.Append(onLine.from);
			OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
			onPolylineCurve.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value5), OnUtil.On_yaxis, OnUtil.On_origin);
			IOnCurve[] array = RhUtil.RhinoCurveBooleanIntersection(onPolylineCurve, onCurve);
			IOnCurve[] array2 = array;
			if (array2 == null)
			{
				RhUtil.RhinoApp().ActiveDoc().AddCurveObject(onPolylineCurve);
			}
			OnCurve[] output = new OnCurve[0];
			if (array2 != null)
			{
				RhUtil.RhinoMergeCurves(array2, ref output);
				if (output.Length == 1)
				{
					onCurve = output[0];
				}
			}
		}
		OnSurface onSurface = RhUtil.RhinoExtrudeCurveStraight(onCurve, new On3dVector(0.0, 1.0, 0.0), value2);
		if (onSurface == null)
		{
			return UpdateError("RhinoExtrudeCurveStraight failed");
		}
		double s = 0.0;
		double t = 0.0;
		onSurface.GetClosestPoint(new On3dPoint(0.0, 0.0, 100.0), ref s, ref t);
		if (onSurface.NormalAt(s, t) * new On3dVector(0.0, 0.0, 1.0) < 0.0)
		{
			onSurface.Reverse(0);
		}
		OnXform onXform3 = new OnXform();
		onXform3.Translation(new On3dVector(0.0, 0.0 - value2 / 2.0, 0.0));
		onSurface.Transform(onXform3);
		OutSurface.SetData(new CSOnGeometryData(onSurface.Convert(), onXform3.Convert(), applyTransform: false));
		OnLineCurve curveB = new OnLineCurve(new On3dPoint(0.0, 0.0, 0.0), new On3dPoint(0.0, 0.0, 100.0));
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		onCurve.IntersectCurve(curveB, ref x);
		OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
		if (x.Count() > 0)
		{
			double z = x[0].get_m_pointA(0).z;
			new OnLineCurve(new On3dPoint(0.0, (0.0 - value2) / 2.0, z), new On3dPoint(0.0, value2 / 2.0, z)).GetNurbForm(ref nurbs_curve2);
			OutTopLine.SetData(new CSOnGeometryData(nurbs_curve2.Convert()));
		}
		return base.Update();
	}
}
