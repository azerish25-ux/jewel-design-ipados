using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.SMI;

[CSBlock("SMICutterBand", "{E7861C52-7377-419e-AEFA-71D89DF6EC24}")]
public sealed class CSSMICutterBand : CSBlock
{
	public CSInputChannel iProfile { get; }

	public CSInputChannel iRadius { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iSubCount { get; }

	public CSOutputChannel oObject { get; }

	public CSSMICutterBand(string identifier)
		: base(identifier)
	{
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRadius = new CSInputChannel(GetPropertyName(() => iRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSubCount = new CSInputChannel(GetPropertyName(() => iSubCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(GetPropertyName(() => oObject), this, ChannelDataType.Any, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSData data = iProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Missing profile data");
		}
		OnCurve onCurve = data.GetNurbsCurve().Convert();
		if (onCurve == null)
		{
			return UpdateMissing("Missing profile curve");
		}
		double value = 76.2;
		iRadius.GetDouble(ref value);
		double value2 = 45.0;
		iAngle.GetDouble(ref value2);
		value2 = CSBlockUtil.CSUtilMath.ToRadians(value2);
		int value3 = 1;
		iSubCount.GetInteger(ref value3);
		OnCurve left_side = null;
		OnCurve right_side = null;
		if (!onCurve.Split(onCurve.Domain().Mid(), ref left_side, ref right_side))
		{
			return UpdateError("Split failed");
		}
		int num = 2 * value3;
		double angle_radians = OnUtil.On_PI / (double)value3;
		double num2 = value2 / 2.0;
		double num3 = (OnUtil.On_PI - OnUtil.On_PI / (double)value3) / 2.0;
		On3dPoint rotation_center = new On3dPoint(0.0, value, 0.0);
		OnCurve onCurve2 = left_side.DuplicateCurve();
		onCurve2.Rotate(num2, OnUtil.On_xaxis, rotation_center);
		OnCurve onCurve3 = left_side.DuplicateCurve();
		onCurve3.Rotate(num2 / 2.0, OnUtil.On_xaxis, rotation_center);
		OnCurve onCurve4 = left_side.DuplicateCurve();
		onCurve4.Rotate(0.0 - num3, OnUtil.On_yaxis, OnUtil.On_origin);
		OnBoundingBox tight_bbox = new OnBoundingBox();
		if (!onCurve2.GetTightBoundingBox(ref tight_bbox))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		OnBoundingBox tight_bbox2 = new OnBoundingBox();
		if (!onCurve4.GetTightBoundingBox(ref tight_bbox2))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		double num4 = Math.Abs(tight_bbox.Max().x - tight_bbox.Min().x);
		double num5 = Math.Abs(tight_bbox2.Max().x - tight_bbox2.Min().x);
		double num6 = num4 / num5;
		OnXform onXform = new OnXform();
		onXform.Scale(num6, 1.0, num6);
		onCurve4.Transform(onXform);
		if (!CSUtilSweep.Sweep1((OnArcCurve)new OnArc(onCurve2.PointAtStart(), onCurve3.PointAtStart(), onCurve4.PointAtStart()), onCurve2, onCurve4, out var breps))
		{
			return UpdateError("Sweep1 failed");
		}
		if (breps.Length != 1)
		{
			return UpdateError("Sweep1 failed");
		}
		OnBrep onBrep = breps[0];
		OnXform onXform2 = new OnXform();
		onXform2.Mirror(OnUtil.On_origin, OnUtil.On_xaxis);
		OnBrep onBrep2 = onBrep.Duplicate();
		onBrep2.Transform(onXform2);
		OnBrep onBrep3 = CSUtilRhino.JoinAllBreps(new OnBrep[2] { onBrep, onBrep2 }, CSApplicationHelper.Application.Tolerance);
		if (onBrep3 == null)
		{
			return UpdateError("RhinoJoinBreps failed");
		}
		OnXform onXform3 = new OnXform();
		onXform3.Rotation(angle_radians, OnUtil.On_yaxis, OnUtil.On_origin);
		OnBrep[] array = new OnBrep[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = onBrep3.Duplicate();
			onBrep3.Transform(onXform3);
		}
		OnBrep onBrep4 = CSUtilRhino.JoinAllBreps(array, CSApplicationHelper.Application.Tolerance);
		if (!onBrep4.IsValidGeometry())
		{
			return UpdateError("RhinoJoinBreps failed");
		}
		OnBrep onBrep5 = CSUtilRhino.CapPlanarHoles(onBrep4, CSApplicationHelper.Application.Tolerance);
		if (onBrep5 == null)
		{
			return UpdateError("RhinoBrepCapPlanarHoles failed");
		}
		if (!onBrep5.IsValidGeometry())
		{
			return UpdateError("RhinoBrepCapPlanarHoles failed");
		}
		oObject.SetData(new CSOnGeometryData(onBrep5.Convert()));
		return base.Update();
	}
}
