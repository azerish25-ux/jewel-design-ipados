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

namespace CSBlocks.Bails;

[CSBlock("BailRail", "{006F6CB4-2C39-4CC1-BC61-72CFC5469FE6}")]
public sealed class CSBailRail : CSBlock
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

	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InLength;

	public readonly CSInputChannel InOpening;

	public readonly CSInputChannel InEndPinch;

	public readonly CSInputChannel InAngle;

	public readonly CSInputChannel InClosed;

	public readonly CSInputChannel InBlendAmount;

	public readonly CSInputChannel InBlendAmount1;

	public readonly CSInputChannel InBlendAmount2;

	public readonly CSOutputChannel OutRail;

	public readonly CSOutputChannel OutParam1;

	public readonly CSOutputChannel OutParam2;

	public readonly CSOutputChannel OutParam3;

	public readonly CSOutputChannel OutBailOpeningInsideHeight;

	public readonly CSOutputChannel OutBailOpeningInsideDepth;

	public CSBailRail(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOpening = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndPinch = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InBlendAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendAmount1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendAmount2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRail = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutParam1 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutParam3 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutBailOpeningInsideHeight = new CSOutputChannel("BailOpeningInsideHeight", this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutBailOpeningInsideDepth = new CSOutputChannel("BailOpeningInsideDepth", this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnNurbsCurve nurbsCurve = InRail.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing rail");
		}
		double value = 0.0;
		InLength.GetDouble(ref value);
		double value2 = 0.0;
		InOpening.GetDouble(ref value2);
		double value3 = 0.0;
		InEndPinch.GetDouble(ref value3);
		double value4 = 0.0;
		InAngle.GetDouble(ref value4);
		value4 = CSBlockUtil.CSUtilMath.ToRadians(value4);
		bool value5 = false;
		InClosed.GetBoolean(ref value5);
		double value6 = 0.0;
		InBlendAmount.GetDouble(ref value6);
		double value7 = 0.0;
		InBlendAmount1.GetDouble(ref value7);
		double value8 = 0.0;
		InBlendAmount2.GetDouble(ref value8);
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
		double number = tight_bbox2.Max().y - tight_bbox2.Min().y;
		double number2 = tight_bbox2.Max().z - tight_bbox2.Min().z;
		OutRail.SetData(new CSOnGeometryData(nurbs_curve2.Convert()));
		OutParam1.SetData(new CSNumberData(t2));
		OutParam2.SetData(new CSNumberData(t3));
		OutParam3.SetData(new CSNumberData(t4));
		OutBailOpeningInsideHeight.SetData(new CSNumberData(number));
		OutBailOpeningInsideDepth.SetData(new CSNumberData(number2));
		return base.Update();
	}
}
