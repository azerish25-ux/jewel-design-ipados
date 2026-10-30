using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

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
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		OnSurface nurbsSurface = (OnSurface)(object)iOutsideRailTube.GetNurbsSurface();
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
		OnXform val = new OnXform();
		val.Scale(3.0, 1.0 + value, 3.0);
		OnNurbsCurve val2 = nurbsCurve2.Duplicate();
		OnNurbsCurve val3 = val2.Duplicate();
		val3.Transform((IOnXform)(object)val);
		OnNurbsSurface val4 = new OnNurbsSurface();
		val4.CreateRuledSurface((IOnCurve)(object)val2, (IOnCurve)(object)val3);
		CSUtilGeometry.IntersectionSurfaces((IOnSurface)(object)nurbsSurface, (IOnSurface)(object)val4, out var outputCurve);
		OnNurbsCurve val5 = new OnNurbsCurve();
		outputCurve.GetNurbForm(ref val5);
		if (((OnCurve)val5).IsClosed())
		{
			ArrayOnX_EVENT val6 = new ArrayOnX_EVENT();
			((OnCurve)val5).IntersectCurve((IOnCurve)(object)nurbsCurve, ref val6);
			if (val6.Count() > 0)
			{
				double num = 0.0;
				int num2 = 0;
				for (int i = 0; i < val6.Count(); i++)
				{
					double num3 = Math.Abs(((OnCurve)val5).PointAt(val6[i][0]).y);
					if (i == 0 || !(num3 >= num))
					{
						num = num3;
						num2 = i;
					}
				}
				((OnCurve)val5).ChangeClosedCurveSeam(val6[num2][0]);
			}
		}
		if (((OnCurve)nurbsCurve2).IsClosed() && outputCurve.IsClosed())
		{
			CSUtilSweep.MatchCurveDirection2((OnCurve)(object)nurbsCurve2, (OnCurve)(object)val5, 16);
		}
		else
		{
			CSUtilSweep.MatchCurveDirection((OnCurve)(object)nurbsCurve2, (OnCurve)(object)val5, 16);
		}
		oOutsideRingRail.SetData(new CSOutsideRingRailData((Curve)(object)val5.Convert()));
		oOutsideScale.SetData(new CSNumberData(value));
		return base.Update();
	}
}
