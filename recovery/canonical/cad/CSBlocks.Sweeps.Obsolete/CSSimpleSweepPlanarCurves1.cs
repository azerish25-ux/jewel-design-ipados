using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Sweeps.Obsolete;

[Obsolete]
[CSBlock("SimpleSweepPlanarCurves_V1", "{98F0A68E-0A0B-484b-9EEA-C90C3524FE5D}")]
public sealed class CSSimpleSweepPlanarCurves1 : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iCurves { get; }

	public CSInputChannel iClosed { get; }

	public CSOutputChannel oBrep { get; }

	public CSSimpleSweepPlanarCurves1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurves = new CSInputChannel(GetPropertyName(() => iCurves), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iClosed = new CSInputChannel(GetPropertyName(() => iClosed), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnNurbsCurve onNurbsCurve = iRail.GetFlattenArray().GetArrayItem(0).GetNurbsCurve()
			.Convert();
		if (onNurbsCurve == null)
		{
			return UpdateMissing("Missing Rail");
		}
		List<IOnCurve> list = new List<IOnCurve>();
		CSDataArray flattenArray = CSDataHelper.GetFlattenArray(iCurves.Data);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (flattenArray.GetArrayItem(i) == null)
			{
				return UpdateMissing("Missing Curve In Array (No Object)");
			}
			OnCurve onCurve = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onCurve == null)
			{
				return UpdateMissing("Missing Curve In Array (No Object)");
			}
			list.Add(onCurve);
			if (list[i] == null)
			{
				return UpdateMissing("Missing curve in array (No NurbsCurve):" + i);
			}
		}
		List<double> list2 = new List<double>();
		for (int j = 0; j < list.Count; j++)
		{
			bool flag = false;
			double t = onNurbsCurve.Domain().Min();
			OnPlane onPlane = new OnPlane();
			if (list[j].IsPlanar(onPlane))
			{
				OnPlaneSurface onPlaneSurface = new OnPlaneSurface(onPlane);
				On3dPoint fixed_point = onPlane.PointAt(0.5, 0.5);
				OnXform onXform = new OnXform();
				onXform.Scale(fixed_point, 1000.0);
				onPlaneSurface.Transform(onXform);
				ArrayOnX_EVENT x = new ArrayOnX_EVENT();
				onNurbsCurve.IntersectSurface(onPlaneSurface, ref x, CSApplicationHelper.Application.Tolerance);
				if (x.Count() > 0)
				{
					int index = 0;
					for (int k = 1; k < x.Count(); k++)
					{
						if (x[k].get_m_pointA(0).DistanceTo(list[j].PointAtStart()) < x[index].get_m_pointA(0).DistanceTo(list[j].PointAtStart()))
						{
							index = k;
						}
					}
					t = x[index].get_m_a(0);
					flag = true;
				}
			}
			if (!flag)
			{
				onNurbsCurve.GetClosestPoint(list[j].PointAtStart(), ref t);
			}
			list2.Add(t);
		}
		double value = 0.0;
		if (iClosed != null)
		{
			iClosed.GetDouble(ref value);
		}
		if (!CSUtilSweep.Sweep1(onNurbsCurve, list.ToArray(), list2.ToArray(), out var breps, value > 0.5) || breps.Length == 0)
		{
			return UpdateError("Failed Sweep1");
		}
		base.OutputChannels.Clear();
		for (int l = 0; l < breps.Length; l++)
		{
			OnBrep onBrep = breps[l];
			if (onBrep != null)
			{
				new CSOutputChannel($"{base.Identifier}_{l}", this, ChannelDataType.Any, ChannelArrayType.Individual).SetData(new CSOnGeometryData(onBrep.Convert()));
			}
		}
		return base.Update();
	}
}
