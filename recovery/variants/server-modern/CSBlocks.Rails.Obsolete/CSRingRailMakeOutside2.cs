using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino;
using Rhino.Geometry;

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
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		OnNurbsCurve nurbsCurve = iRingRail.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Ring Rail");
		}
		OnSurface nurbsSurface = (OnSurface)(object)iOutsideRailTube.GetNurbsSurface();
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
		OnXform val = new OnXform();
		val.Scale(3.0, 1.0 + value, 3.0);
		OnNurbsCurve val2 = nurbsCurve.Duplicate();
		val2.Transform((IOnXform)(object)val);
		OnNurbsSurface val3 = new OnNurbsSurface();
		if (val3.CreateRuledSurface((IOnCurve)(object)nurbsCurve, (IOnCurve)(object)val2) <= 0)
		{
			return UpdateError("CreateRuledSurface failed");
		}
		if (!CSUtilGeometry.IntersectionSurfaces((IOnSurface)(object)val3, (IOnSurface)(object)nurbsSurface, out var outputCurve))
		{
			return UpdateError("IntersectionSurfaces failed");
		}
		OnNurbsCurve val4 = new OnNurbsCurve();
		if (outputCurve.GetNurbForm(ref val4) <= 0)
		{
			return UpdateError("GetNurbForm failed");
		}
		if (((OnCurve)val4).IsClosed())
		{
			ArrayOnX_EVENT val5 = new ArrayOnX_EVENT();
			((OnCurve)val4).IntersectCurve((IOnCurve)(object)nurbsCurve2, ref val5);
			if (val5.Count() > 0)
			{
				double num = -1.23432101234321E+308;
				int num2 = 0;
				for (int i = 0; i < val5.Count(); i++)
				{
					double num3 = ((OnCurve)val4).PointAt(val5[i][0]).DistanceTo(OnUtil.On_origin);
					if (!RhinoMath.IsValidDouble(num) || !(num3 >= num))
					{
						num = num3;
						num2 = i;
					}
				}
				if (!((OnCurve)val4).ChangeClosedCurveSeam(val5[num2][0]))
				{
					return UpdateError("ChangeClosedCurveSeam failed");
				}
			}
		}
		if (((OnCurve)nurbsCurve).IsClosed() && ((OnCurve)val4).IsClosed())
		{
			CSUtilSweep.MatchCurveDirection2((OnCurve)(object)nurbsCurve, (OnCurve)(object)val4, 16);
		}
		else
		{
			CSUtilSweep.MatchCurveDirection((OnCurve)(object)nurbsCurve, (OnCurve)(object)val4, 16);
		}
		oOutsideRingRail.SetData(new CSOutsideRingRailData((Curve)(object)val4.Convert(), value));
		return base.Update();
	}
}
