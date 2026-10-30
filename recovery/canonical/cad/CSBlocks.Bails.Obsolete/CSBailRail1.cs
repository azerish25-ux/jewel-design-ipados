using System;
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
[CSBlock("BailRail_V1", "{6A5C5084-E59F-480f-973D-4082A55B5810}")]
public sealed class CSBailRail1 : CSBlock
{
	private sealed class BailRail : OnSpaceMorph
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

	public CSInputChannel iRail { get; }

	public CSInputChannel iEndPinch { get; }

	public CSOutputChannel oRail { get; }

	public CSBailRail1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iEndPinch = new CSInputChannel(GetPropertyName(() => iEndPinch), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRail = new CSOutputChannel(GetPropertyName(() => oRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSData data = iRail.Data;
		if (data == null)
		{
			return UpdateMissing("Rail input mssing");
		}
		OnNurbsCurve onNurbsCurve = data.GetNurbsCurve().Convert();
		if (onNurbsCurve == null)
		{
			return UpdateError("Rail input is not a curve");
		}
		double value = 0.0;
		iEndPinch.GetDouble(ref value);
		onNurbsCurve.RemoveShortSegments(0.2);
		onNurbsCurve.RepairBadKnots();
		RhUtil.RhinoRepairCurve(onNurbsCurve, 0.2, 3);
		OnBoundingBox tight_bbox = new OnBoundingBox();
		if (!onNurbsCurve.GetTightBoundingBox(ref tight_bbox))
		{
			return UpdateError("Bounding Box failed");
		}
		OnLineCurve curveB = new OnLineCurve(new On3dPoint(0.0, tight_bbox.Max().y, tight_bbox.Max().z), new On3dPoint(0.0, tight_bbox.Max().y, 0.0));
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		onNurbsCurve.IntersectCurve(curveB, ref x);
		if (x.Count() == 0)
		{
			return UpdateError("Intersection failed");
		}
		double t = x[0].get_m_a(0);
		On3dPoint on3dPoint = onNurbsCurve.PointAt(t);
		foreach (OnX_EVENT item in x)
		{
			if (item.get_m_pointA(0).z > on3dPoint.z)
			{
				t = item.get_m_a(0);
				on3dPoint = onNurbsCurve.PointAt(t);
			}
		}
		OnCurve left_side = null;
		OnCurve right_side = null;
		if (!onNurbsCurve.Split(t, ref left_side, ref right_side))
		{
			return UpdateError("Split failed");
		}
		if (left_side == null || right_side == null)
		{
			return UpdateError("Split curves failed");
		}
		BailRail bailRail = new BailRail();
		bailRail.SetUp(value, on3dPoint.y);
		bailRail.SetTolerance(0.05);
		if (!left_side.Morph(bailRail))
		{
			return UpdateError("RailLhs Morph Failed");
		}
		OnCurve[] obj = new OnCurve[2] { left_side, right_side };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return UpdateError("Merge Curves Failed");
		}
		if (output.Length != 1)
		{
			return UpdateError("Merge Curves Failed Multi Curves returned");
		}
		OnCurve crv = output[0];
		oRail.SetData(new CSOnGeometryData(crv.Convert()));
		return base.Update();
	}
}
