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
using Rhino.Geometry;

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
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		OnNurbsCurve val = iRail.GetFlattenArray().GetArrayItem(0).GetNurbsCurve()
			.Convert();
		if (val == null)
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
			OnCurve val2 = (OnCurve)(object)flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val2 == null)
			{
				return UpdateMissing("Missing Curve In Array (No Object)");
			}
			list.Add((IOnCurve)(object)val2);
			if (list[i] == null)
			{
				return UpdateMissing("Missing curve in array (No NurbsCurve):" + i);
			}
		}
		List<double> list2 = new List<double>();
		for (int j = 0; j < list.Count; j++)
		{
			bool flag = false;
			double item = ((OnCurve)val).Domain().Min();
			OnPlane val3 = new OnPlane();
			if (list[j].IsPlanar(val3))
			{
				OnPlaneSurface val4 = new OnPlaneSurface((IOnPlane)(object)val3);
				On3dPoint val5 = val3.PointAt(0.5, 0.5);
				OnXform val6 = new OnXform();
				val6.Scale((IOn3dPoint)(object)val5, 1000.0);
				val4.Transform((IOnXform)(object)val6);
				ArrayOnX_EVENT val7 = new ArrayOnX_EVENT();
				((OnCurve)val).IntersectSurface((IOnSurface)(object)val4, ref val7, CSApplicationHelper.Application.Tolerance);
				if (val7.Count() > 0)
				{
					int num = 0;
					for (int k = 1; k < val7.Count(); k++)
					{
						if (val7[k][0].DistanceTo((IOn3dPoint)(object)list[j].PointAtStart()) < val7[num][0].DistanceTo((IOn3dPoint)(object)list[j].PointAtStart()))
						{
							num = k;
						}
					}
					item = val7[num][0];
					flag = true;
				}
			}
			if (!flag)
			{
				((OnCurve)val).GetClosestPoint((IOn3dPoint)(object)list[j].PointAtStart(), ref item);
			}
			list2.Add(item);
		}
		double value = 0.0;
		if (iClosed != null)
		{
			iClosed.GetDouble(ref value);
		}
		if (!CSUtilSweep.Sweep1((IOnCurve)(object)val, list.ToArray(), list2.ToArray(), out var breps, value > 0.5) || breps.Length == 0)
		{
			return UpdateError("Failed Sweep1");
		}
		base.OutputChannels.Clear();
		for (int l = 0; l < breps.Length; l++)
		{
			OnBrep val8 = breps[l];
			if (val8 != null)
			{
				new CSOutputChannel($"{base.Identifier}_{l}", this, ChannelDataType.Any, ChannelArrayType.Individual).SetData(new CSOnGeometryData((GeometryBase)(object)val8.Convert()));
			}
		}
		return base.Update();
	}
}
