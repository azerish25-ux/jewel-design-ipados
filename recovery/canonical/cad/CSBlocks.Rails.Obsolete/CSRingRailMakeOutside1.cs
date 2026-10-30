using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Rails.Obsolete;

[Obsolete]
[CSBlock("RingRailMakeOutside_V1", "{F7C6F6F3-2F35-44a1-9C2B-8BCB7B51C771}")]
public sealed class CSRingRailMakeOutside1 : CSBlock
{
	public CSInputChannel iRingRail { get; }

	public CSInputChannel iOutsideRailTube { get; }

	public CSInputChannel iOutsideRailLine { get; }

	public CSInputChannel iScale { get; }

	public CSOutputChannel oOutsideRingRail { get; }

	public CSOutputChannel oOutsideScale { get; }

	public CSRingRailMakeOutside1(string identifier)
		: base(identifier)
	{
		iRingRail = new CSInputChannel(GetPropertyName(() => iRingRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRailTube = new CSInputChannel(GetPropertyName(() => iOutsideRailTube), this, ChannelDataType.Surface, ChannelArrayType.Individual);
		iOutsideRailLine = new CSInputChannel(GetPropertyName(() => iOutsideRailLine), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iScale = new CSInputChannel(GetPropertyName(() => iScale), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oOutsideRingRail = new CSOutputChannel(GetPropertyName(() => oOutsideRingRail), this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		oOutsideScale = new CSOutputChannel(GetPropertyName(() => oOutsideScale), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnSurface nurbsSurface = iOutsideRailTube.GetNurbsSurface();
		if (nurbsSurface == null)
		{
			return UpdateError("Missing Surface");
		}
		OnNurbsCurve nurbsCurve = iOutsideRailLine.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateError("Missing Ring Rail Line");
		}
		OnNurbsCurve nurbsCurve2 = iRingRail.GetNurbsCurve();
		if (nurbsCurve2 == null)
		{
			return UpdateError("Missing Ring Rail");
		}
		double value = 0.0;
		if (!iScale.GetDouble(ref value))
		{
			return UpdateError("Missing Scale");
		}
		OnXform onXform = new OnXform();
		onXform.Scale(3.0, 1.0 + value, 3.0);
		OnNurbsCurve onNurbsCurve = nurbsCurve2.Duplicate();
		OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
		onNurbsCurve2.Transform(onXform);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		onNurbsSurface.CreateRuledSurface(onNurbsCurve, onNurbsCurve2);
		CSUtilGeometry.IntersectionSurfaces(nurbsSurface, onNurbsSurface, out var outputCurve);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		outputCurve.GetNurbForm(ref nurbs_curve);
		if (nurbs_curve.IsClosed())
		{
			ArrayOnX_EVENT x = new ArrayOnX_EVENT();
			nurbs_curve.IntersectCurve(nurbsCurve, ref x);
			if (x.Count() > 0)
			{
				double num = 0.0;
				int index = 0;
				for (int i = 0; i < x.Count(); i++)
				{
					double num2 = Math.Abs(nurbs_curve.PointAt(x[i].get_m_a(0)).y);
					if (i == 0 || !(num2 >= num))
					{
						num = num2;
						index = i;
					}
				}
				nurbs_curve.ChangeClosedCurveSeam(x[index].get_m_a(0));
			}
		}
		if (nurbsCurve2.IsClosed() && outputCurve.IsClosed())
		{
			CSUtilSweep.MatchCurveDirection2(nurbsCurve2, nurbs_curve, 16);
		}
		else
		{
			CSUtilSweep.MatchCurveDirection(nurbsCurve2, nurbs_curve, 16);
		}
		oOutsideRingRail.SetData(new CSOutsideRingRailData(nurbs_curve.Convert()));
		oOutsideScale.SetData(new CSNumberData(value));
		return base.Update();
	}
}
