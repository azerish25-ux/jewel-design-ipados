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

namespace CSBlocks.Bails.Obsolete;

[Obsolete]
[CSBlock("BailRail_V2", "{BAED5A7F-0A27-482c-A456-470C6061770C}")]
public sealed class CSBailRail2 : CSBlock
{
	private sealed class EndPinchMorph : OnSpaceMorph
	{
		private double _opening;

		private double _length;

		public override On3dPoint MorphPoint(IOn3dPoint inputPoint)
		{
			On3dPoint on3dPoint = new On3dPoint(inputPoint);
			on3dPoint.z += _opening * Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(on3dPoint.y / _length * 90.0));
			return on3dPoint;
		}

		public void SetUp(double newOpening, double newLength)
		{
			_opening = newOpening;
			_length = newLength;
		}
	}

	private sealed class AngleMorph : OnSpaceMorph
	{
		private double _distance;

		private double _length;

		public override On3dPoint MorphPoint(IOn3dPoint inputPoint)
		{
			On3dPoint on3dPoint = new On3dPoint(inputPoint);
			if (on3dPoint.y > 0.0)
			{
				on3dPoint.x += _distance * (1.0 - Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(on3dPoint.y / _length * 90.0)));
			}
			return on3dPoint;
		}

		public void SetUp(double newDistance, double newLength)
		{
			_distance = newDistance;
			_length = newLength;
		}
	}

	public CSInputChannel iRail { get; }

	public CSInputChannel iLength { get; }

	public CSInputChannel iOpening { get; }

	public CSInputChannel iEndPinch { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iClosed { get; }

	public CSInputChannel iBlendAmount { get; }

	public CSInputChannel iBlendAmount1 { get; }

	public CSInputChannel iBlendAmount2 { get; }

	public CSOutputChannel oRail { get; }

	public CSOutputChannel oParam1 { get; }

	public CSOutputChannel oParam2 { get; }

	public CSOutputChannel oParam3 { get; }

	public CSOutputChannel oBailOpeningInsideHeight { get; }

	public CSOutputChannel oBailOpeningInsideDepth { get; }

	public CSBailRail2(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iLength = new CSInputChannel(GetPropertyName(() => iLength), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOpening = new CSInputChannel(GetPropertyName(() => iOpening), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndPinch = new CSInputChannel(GetPropertyName(() => iEndPinch), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iClosed = new CSInputChannel(GetPropertyName(() => iClosed), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iBlendAmount = new CSInputChannel(GetPropertyName(() => iBlendAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmount1 = new CSInputChannel(GetPropertyName(() => iBlendAmount1), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmount2 = new CSInputChannel(GetPropertyName(() => iBlendAmount2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRail = new CSOutputChannel(GetPropertyName(() => oRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oParam1 = new CSOutputChannel(GetPropertyName(() => oParam1), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParam2 = new CSOutputChannel(GetPropertyName(() => oParam2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParam3 = new CSOutputChannel(GetPropertyName(() => oParam3), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBailOpeningInsideHeight = new CSOutputChannel("BailOpeningInsideHeight", this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBailOpeningInsideDepth = new CSOutputChannel("BailOpeningInsideDepth", this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnNurbsCurve nurbsCurve = iRail.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing rail");
		}
		double value = 0.0;
		iLength.GetDouble(ref value);
		double value2 = 0.0;
		iOpening.GetDouble(ref value2);
		double value3 = 0.0;
		iEndPinch.GetDouble(ref value3);
		double value4 = 0.0;
		iAngle.GetDouble(ref value4);
		value4 = CSBlockUtil.CSUtilMath.ToRadians(value4);
		bool value5 = false;
		iClosed.GetBoolean(ref value5);
		double value6 = 0.0;
		iBlendAmount.GetDouble(ref value6);
		double value7 = 0.0;
		iBlendAmount1.GetDouble(ref value7);
		double value8 = 0.0;
		iBlendAmount2.GetDouble(ref value8);
		nurbsCurve.RemoveShortSegments(0.2);
		nurbsCurve.RepairBadKnots();
		RhUtil.RhinoRepairCurve(nurbsCurve, 0.2, 3);
		OnBoundingBox tight_bbox = new OnBoundingBox();
		if (!nurbsCurve.GetTightBoundingBox(ref tight_bbox))
		{
			return UpdateError("Missing rail bounding box");
		}
		OnXform onXform = new OnXform();
		onXform.Scale(1.0, value / (tight_bbox.Max().y - tight_bbox.Min().y), value2 / (tight_bbox.Max().z - tight_bbox.Min().z));
		nurbsCurve.Transform(onXform);
		OnLineCurve curveB = new OnLineCurve(new On3dPoint(0.0, value, value2), new On3dPoint(0.0, value, 0.0));
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		nurbsCurve.IntersectCurve(curveB, ref x);
		if (x.Count() == 0)
		{
			return UpdateError("Error Intersect");
		}
		double t = x[0].get_m_a(0);
		On3dPoint on3dPoint = nurbsCurve.PointAt(t);
		foreach (OnX_EVENT item in x)
		{
			if (item.get_m_pointA(0).z > on3dPoint.z)
			{
				t = item.get_m_a(0);
				on3dPoint = nurbsCurve.PointAt(t);
			}
		}
		OnCurve left_side = null;
		OnCurve right_side = null;
		if (!nurbsCurve.Split(t, ref left_side, ref right_side))
		{
			return UpdateError("Error Split");
		}
		if (left_side == null || right_side == null)
		{
			return UpdateError("Error Split");
		}
		EndPinchMorph endPinchMorph = new EndPinchMorph();
		endPinchMorph.SetUp(value3, on3dPoint.y);
		endPinchMorph.SetTolerance(0.05);
		if (!left_side.Morph(endPinchMorph))
		{
			return UpdateError("Error Morph");
		}
		IOnCurve[] input_curves = new IOnCurve[2] { left_side, right_side };
		OnCurve[] output = null;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return UpdateError("Error Merge Curves");
		}
		if (output.Length != 1)
		{
			return UpdateError("Error Merge Curves");
		}
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		output[0].GetNurbForm(ref nurbs_curve);
		OnNurbsCurve onNurbsCurve = nurbs_curve.Duplicate();
		if (value4 != 0.0)
		{
			double num = -1.0 * Math.Tan(value4) * value;
			AngleMorph angleMorph = new AngleMorph();
			angleMorph.SetUp(num, value);
			angleMorph.SetTolerance(0.05);
			if (!onNurbsCurve.Morph(angleMorph))
			{
				return UpdateError("Error Morph Angle Rail");
			}
			on3dPoint.x += num;
		}
		OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
		double t2 = 0.0;
		double t3 = 0.0;
		double t4 = 0.0;
		if (value5)
		{
			OnNurbsCurve onNurbsCurve2 = CSUtilCurves.BlendCurve(nurbs_curve.PointAtStart(), nurbs_curve.PointAtEnd(), -nurbs_curve.TangentAt(nurbs_curve.Domain().Min()), nurbs_curve.TangentAt(nurbs_curve.Domain().Max()), value6, value7, value8);
			IOnCurve[] input_curves2 = new IOnCurve[2] { onNurbsCurve, onNurbsCurve2 };
			OnCurve[] output2 = null;
			if (!RhUtil.RhinoMergeCurves(input_curves2, ref output2))
			{
				return UpdateError("Error Merge Curves");
			}
			if (output2.Length != 1)
			{
				return UpdateError("Error Merge Curves");
			}
			output2[0].GetNurbForm(ref nurbs_curve2);
			nurbs_curve2.GetClosestPoint(onNurbsCurve.PointAtStart(), ref t2);
			nurbs_curve2.GetClosestPoint(onNurbsCurve.PointAtEnd(), ref t3);
			nurbs_curve2.GetClosestPoint(on3dPoint, ref t4);
			double length = 0.0;
			nurbs_curve2.GetLength(ref length);
			double length2 = 0.0;
			nurbs_curve2.GetLength(ref length2, CSApplicationHelper.Application.Tolerance, new OnInterval(nurbs_curve2.Domain().Min(), t2));
			t2 = length2 / length;
			double length3 = 0.0;
			nurbs_curve2.GetLength(ref length3, CSApplicationHelper.Application.Tolerance, new OnInterval(nurbs_curve2.Domain().Min(), t3));
			t3 = length3 / length;
			double length4 = 0.0;
			nurbs_curve2.GetLength(ref length4, CSApplicationHelper.Application.Tolerance, new OnInterval(nurbs_curve2.Domain().Min(), t4));
			t4 = length4 / length;
		}
		else
		{
			nurbs_curve2 = onNurbsCurve;
			t2 = nurbs_curve2.Domain().Min();
			t3 = nurbs_curve2.Domain().Max();
			nurbs_curve2.GetClosestPoint(on3dPoint, ref t4);
		}
		OnBoundingBox tight_bbox2 = new OnBoundingBox();
		if (!nurbs_curve2.GetTightBoundingBox(ref tight_bbox2))
		{
			return UpdateError("Error Get Tight Bounding Box from Output Rail");
		}
		double number = tight_bbox2.Max().y + tight_bbox2.Min().y;
		double number2 = tight_bbox2.Max().z + tight_bbox2.Min().z;
		oRail.SetData(new CSOnGeometryData(nurbs_curve2.Convert()));
		oParam1.SetData(new CSNumberData(t2));
		oParam2.SetData(new CSNumberData(t3));
		oParam3.SetData(new CSNumberData(t4));
		oBailOpeningInsideHeight.SetData(new CSNumberData(number));
		oBailOpeningInsideDepth.SetData(new CSNumberData(number2));
		return base.Update();
	}
}
