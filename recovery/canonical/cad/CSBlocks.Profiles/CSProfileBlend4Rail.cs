using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Profiles;

[CSBlock("ProfileBlend4Rail", "{DF22A32B-3F5C-4092-ACE6-B72683CB3317}")]
public sealed class CSProfileBlend4Rail : CSBlock
{
	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSInputChannel iBlendAmount;

	public readonly CSInputChannel iBlendAmount1;

	public readonly CSInputChannel iBlendAmount2;

	public CSOutputChannel oSurface;

	public CSProfileBlend4Rail(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iBlendAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmount1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmount2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData arrayItem = iCurve1.GetFlattenArray().GetArrayItem(0);
		OnNurbsCurve onNurbsCurve = arrayItem.GetNurbsCurve().Convert();
		if (onNurbsCurve == null)
		{
			return UpdateMissing("Missing curve 1");
		}
		CSData arrayItem2 = iCurve2.GetFlattenArray().GetArrayItem(0);
		OnNurbsCurve onNurbsCurve2 = arrayItem2.GetNurbsCurve().Convert();
		if (onNurbsCurve2 == null)
		{
			return UpdateMissing("Missing curve 2");
		}
		double value = 1.0;
		double value2 = 1.0;
		double value3 = 1.0;
		iBlendAmount.GetDouble(ref value3);
		iBlendAmount1.GetDouble(ref value);
		iBlendAmount2.GetDouble(ref value2);
		onNurbsCurve = onNurbsCurve.Duplicate();
		onNurbsCurve2 = onNurbsCurve2.Duplicate();
		OnPlane onPlane = new OnPlane();
		OnPlane onPlane2 = new OnPlane();
		onNurbsCurve.IsPlanar(onPlane);
		onNurbsCurve2.IsPlanar(onPlane2);
		if (!onPlane.IsValid() || !onPlane2.IsValid())
		{
			return UpdateError("Nonplanar curves");
		}
		if (!onNurbsCurve.IsClosed() || !onNurbsCurve2.IsClosed())
		{
			return UpdateError("Open curves");
		}
		On3dPoint on3dPoint = onNurbsCurve.PointAtStart();
		On3dPoint on3dPoint2 = onNurbsCurve2.PointAtStart();
		On3dVector on3dVector = on3dPoint - on3dPoint2;
		on3dVector.Unitize();
		On3dVector on3dVector2 = new On3dVector(onPlane.Normal());
		On3dVector on3dVector3 = new On3dVector(onPlane2.Normal());
		if (on3dVector2 * on3dVector > 0.0)
		{
			on3dVector2.Reverse();
		}
		if (on3dVector3 * on3dVector > 0.0)
		{
			on3dVector3.Reverse();
		}
		if (Math.Abs(on3dVector2 * on3dVector) < 0.01 && Math.Abs(on3dVector3 * on3dVector) < 0.01)
		{
			on3dVector3 = new On3dVector(on3dVector2);
		}
		if (value2 < 0.0)
		{
			on3dVector3.Reverse();
			value2 = 0.0 - value2;
		}
		if (value < 0.0)
		{
			on3dVector2.Reverse();
			value = 0.0 - value;
		}
		double num = on3dPoint.DistanceTo(on3dPoint2);
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Empty();
		on3dPointArray.Append(on3dPoint);
		on3dPointArray.Append(on3dPoint + on3dVector2 * num * 0.2 * value3 * value);
		on3dPointArray.Append(on3dPoint + on3dVector2 * num * 0.4 * value3 * value);
		on3dPointArray.Append(on3dPoint2 - on3dVector3 * num * 0.4 * value3 * value2);
		on3dPointArray.Append(on3dPoint2 - on3dVector3 * num * 0.2 * value3 * value2);
		on3dPointArray.Append(on3dPoint2);
		OnBezierCurve onBezierCurve = new OnBezierCurve(on3dPointArray);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onBezierCurve.GetNurbForm(ref nurbs_curve);
		nurbs_curve.RemoveShortSegments(0.01);
		nurbs_curve.RepairBadKnots();
		List<IOnCurve> list = new List<IOnCurve>
		{
			arrayItem.GetCurveSegment(0, 4, fromStart: false).Convert(),
			arrayItem.GetCurveSegment(1, 4, fromStart: false).Convert(),
			arrayItem.GetCurveSegment(2, 4, fromStart: false).Convert(),
			arrayItem.GetCurveSegment(3, 4, fromStart: false).Convert(),
			arrayItem2.GetCurveSegment(0, 4, fromStart: false).Convert(),
			arrayItem2.GetCurveSegment(1, 4, fromStart: false).Convert(),
			arrayItem2.GetCurveSegment(2, 4, fromStart: false).Convert(),
			arrayItem2.GetCurveSegment(3, 4, fromStart: false).Convert()
		};
		List<double> list2 = new List<double>
		{
			onBezierCurve.Domain().Min(),
			onBezierCurve.Domain().Max()
		};
		OnBrep brep = CSUtilRhino.JoinAllBreps(CSUtilSweep.Sweep1C4Simple(nurbs_curve, list.ToArray(), list2.ToArray(), closed: false).ToArray(), CSApplicationHelper.Application.Tolerance);
		oSurface.SetData(new CSOnGeometryData(brep.Convert()));
		return base.Update();
	}
}
