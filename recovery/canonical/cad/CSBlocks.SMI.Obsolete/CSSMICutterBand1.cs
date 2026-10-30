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

namespace CSBlocks.SMI.Obsolete;

[Obsolete]
[CSBlock("SMICutterBand_V1", "{988903BB-0F38-4b8c-866B-AA2C49387011}")]
public sealed class CSSMICutterBand1 : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iProfilePlacement { get; }

	public CSInputChannel iProfileShape { get; }

	public CSInputChannel iRadius { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iSubCount { get; }

	public CSOutputChannel oObject { get; }

	public CSOutputChannel oString { get; }

	public CSSMICutterBand1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfilePlacement = new CSInputChannel(GetPropertyName(() => iProfilePlacement), this, ChannelDataType.Transform, ChannelArrayType.Individual);
		iProfileShape = new CSInputChannel(GetPropertyName(() => iProfileShape), this, ChannelDataType.Any, ChannelArrayType.Any);
		iRadius = new CSInputChannel(GetPropertyName(() => iRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSubCount = new CSInputChannel(GetPropertyName(() => iSubCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(GetPropertyName(() => oObject), this, ChannelDataType.Any, ChannelArrayType.Individual);
		oString = new CSOutputChannel(GetPropertyName(() => oString), this, ChannelDataType.String, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSData data = iRail.Data;
		if (data == null)
		{
			return UpdateMissing("Missing rail data");
		}
		OnCurve onCurve = data.GetNurbsCurve().Convert();
		if (onCurve == null)
		{
			return UpdateMissing("Missing rail curve");
		}
		CSData data2 = iProfilePlacement.Data;
		if (data2 == null)
		{
			return UpdateMissing("Missing placement curve data");
		}
		OnCurve onCurve2 = data2.GetNurbsCurve().Convert();
		if (onCurve2 == null)
		{
			return UpdateMissing("Missing placement curve curve");
		}
		CSData data3 = iProfileShape.Data;
		if (data3 == null)
		{
			return UpdateMissing("Missing profile shape data");
		}
		OnCurve onCurve3 = data3.GetNurbsCurve().Convert();
		if (onCurve3 == null)
		{
			return UpdateMissing("Missing profile shape curve");
		}
		double value = 76.2;
		iRadius.GetDouble(ref value);
		double value2 = 45.0;
		iAngle.GetDouble(ref value2);
		value2 = CSBlockUtil.CSUtilMath.ToRadians(value2);
		int value3 = 1;
		iSubCount.GetInteger(ref value3);
		string input = "[SMI Cutter Band]" + Environment.NewLine + "Radius = " + value + "mm" + Environment.NewLine + "Angle = " + value2 + "Â°" + Environment.NewLine + "Sub-Count = " + value3;
		OnCurve left_side = null;
		OnCurve right_side = null;
		if (!onCurve3.Split(onCurve3.Domain().Mid(), ref left_side, ref right_side))
		{
			return UpdateError("Split failed");
		}
		int num = 2 * value3;
		double angle_radians = OnUtil.On_PI / (double)value3;
		double num2 = value2 / 2.0;
		double num3 = (OnUtil.On_PI - OnUtil.On_PI / (double)value3) / 2.0;
		On3dPoint rotation_center = new On3dPoint(0.0, value, 0.0);
		OnCurve onCurve4 = left_side.DuplicateCurve();
		onCurve4.Rotate(num2, OnUtil.On_xaxis, rotation_center);
		OnCurve onCurve5 = left_side.DuplicateCurve();
		onCurve5.Rotate(num2 / 2.0, OnUtil.On_xaxis, rotation_center);
		OnCurve onCurve6 = left_side.DuplicateCurve();
		onCurve6.Rotate(0.0 - num3, OnUtil.On_yaxis, OnUtil.On_origin);
		OnBoundingBox tight_bbox = new OnBoundingBox();
		if (!onCurve4.GetTightBoundingBox(ref tight_bbox))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		OnBoundingBox tight_bbox2 = new OnBoundingBox();
		if (!onCurve6.GetTightBoundingBox(ref tight_bbox2))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		double num4 = Math.Abs(tight_bbox.Max().x - tight_bbox.Min().x);
		double num5 = Math.Abs(tight_bbox2.Max().x - tight_bbox2.Min().x);
		double num6 = num4 / num5;
		OnXform onXform = new OnXform();
		onXform.Scale(num6, 1.0, num6);
		onCurve6.Transform(onXform);
		if (!CSUtilSweep.Sweep1((OnArcCurve)new OnArc(onCurve4.PointAtStart(), onCurve5.PointAtStart(), onCurve6.PointAtStart()), onCurve4, onCurve6, out var breps))
		{
			return UpdateError("Sweep1 failed");
		}
		if (breps.Length != 1)
		{
			return UpdateError("Sweep1 results failed");
		}
		OnBrep onBrep = breps[0];
		OnXform onXform2 = new OnXform();
		onXform2.Mirror(OnUtil.On_origin, OnUtil.On_xaxis);
		OnBrep onBrep2 = onBrep.Duplicate();
		onBrep2.Transform(onXform2);
		OnBrep onBrep3 = CSUtilRhino.JoinAllBreps(new OnBrep[2] { onBrep, onBrep2 }, CSApplicationHelper.Application.Tolerance);
		if (onBrep3 == null)
		{
			return UpdateError("JoinBreps failed");
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
			return UpdateError("JoinBreps failed");
		}
		OnBrep onBrep5 = CSUtilRhino.CapPlanarHoles(onBrep4, CSApplicationHelper.Application.Tolerance);
		if (!onBrep5.IsValidGeometry())
		{
			return UpdateError("CapPlanarHoles failed");
		}
		CSDataHelper.GetTransformNoArray(iProfilePlacement.Data, out var value4);
		value4.TryGetInverse(out var inverseTransform);
		onCurve2.Transform(inverseTransform.Convert());
		OnBoundingBox tight_bbox3 = new OnBoundingBox();
		if (!onCurve2.GetTightBoundingBox(ref tight_bbox3))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		double dy = Math.Abs(tight_bbox3.Max().y - tight_bbox3.Min().y);
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0, dy, 0.0);
		OnXform onXform5 = new OnXform();
		onXform5.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_yaxis, OnUtil.On_origin);
		OnXform x = new OnXform();
		if (!CSUtilGeometry.OrientProfileToCurveNalp(onCurve, 0.0, ref x, 1.0))
		{
			return UpdateError("OrientProfileToCurveNalp failed");
		}
		onBrep5.Transform(x * onXform5 * onXform4);
		oObject.SetData(new CSOnGeometryData(onBrep5.Convert(), x.Convert() * onXform5.Convert() * onXform4.Convert(), applyTransform: false));
		oString.SetData(new CSStringData(input));
		return base.Update();
	}
}
