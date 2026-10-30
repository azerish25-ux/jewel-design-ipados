using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino;

namespace CSBlocks.Rails.Obsolete;

[Obsolete]
[CSBlock("RingRailMakeOutside_V2", "{61038942-7A2E-4058-AA40-9890AE9FEF5C}")]
public sealed class CSRingRailMakeOutside2 : CSBlock
{
	public CSInputChannel iRingRail { get; }

	public CSInputChannel iOutsideRailTube { get; }

	public CSInputChannel iOutsideRailLine { get; }

	public CSInputChannel iScale { get; }

	public CSOutputChannel oOutsideRingRail { get; }

	public CSRingRailMakeOutside2(string identifier)
		: base(identifier)
	{
		iRingRail = new CSInputChannel(GetPropertyName(() => iRingRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRailTube = new CSInputChannel(GetPropertyName(() => iOutsideRailTube), this, ChannelDataType.Surface, ChannelArrayType.Individual);
		iOutsideRailLine = new CSInputChannel(GetPropertyName(() => iOutsideRailLine), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iScale = new CSInputChannel(GetPropertyName(() => iScale), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oOutsideRingRail = new CSOutputChannel(GetPropertyName(() => oOutsideRingRail), this, ChannelDataType.RingRail, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnNurbsCurve nurbsCurve = iRingRail.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Ring Rail");
		}
		OnSurface nurbsSurface = iOutsideRailTube.GetNurbsSurface();
		if (nurbsSurface == null)
		{
			return UpdateMissing("Outside Ring Rail Tube");
		}
		OnNurbsCurve nurbsCurve2 = iOutsideRailLine.GetNurbsCurve();
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("Outside Ring Rail Line");
		}
		double value = 1.0;
		iScale.GetDouble(ref value);
		OnXform onXform = new OnXform();
		onXform.Scale(3.0, 1.0 + value, 3.0);
		OnNurbsCurve onNurbsCurve = nurbsCurve.Duplicate();
		onNurbsCurve.Transform(onXform);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		if (onNurbsSurface.CreateRuledSurface(nurbsCurve, onNurbsCurve) <= 0)
		{
			return UpdateError("CreateRuledSurface failed");
		}
		if (!CSUtilGeometry.IntersectionSurfaces(onNurbsSurface, nurbsSurface, out var outputCurve))
		{
			return UpdateError("IntersectionSurfaces failed");
		}
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		if (outputCurve.GetNurbForm(ref nurbs_curve) <= 0)
		{
			return UpdateError("GetNurbForm failed");
		}
		if (nurbs_curve.IsClosed())
		{
			ArrayOnX_EVENT x = new ArrayOnX_EVENT();
			nurbs_curve.IntersectCurve(nurbsCurve2, ref x);
			if (x.Count() > 0)
			{
				double num = -1.23432101234321E+308;
				int index = 0;
				for (int i = 0; i < x.Count(); i++)
				{
					double num2 = nurbs_curve.PointAt(x[i].get_m_a(0)).DistanceTo(OnUtil.On_origin);
					if (!RhinoMath.IsValidDouble(num) || !(num2 >= num))
					{
						num = num2;
						index = i;
					}
				}
				if (!nurbs_curve.ChangeClosedCurveSeam(x[index].get_m_a(0)))
				{
					return UpdateError("ChangeClosedCurveSeam failed");
				}
			}
		}
		if (nurbsCurve.IsClosed() && nurbs_curve.IsClosed())
		{
			CSUtilSweep.MatchCurveDirection2(nurbsCurve, nurbs_curve, 16);
		}
		else
		{
			CSUtilSweep.MatchCurveDirection(nurbsCurve, nurbs_curve, 16);
		}
		oOutsideRingRail.SetData(new CSOutsideRingRailData(nurbs_curve.Convert(), value));
		return base.Update();
	}
}
