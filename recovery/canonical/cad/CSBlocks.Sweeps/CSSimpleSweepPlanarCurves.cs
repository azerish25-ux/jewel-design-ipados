using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Sweeps;

[CSBlock("SimpleSweepPlanarCurves", "{D296F2B7-FFD6-4012-B82B-DF349008EB5F}")]
public sealed class CSSimpleSweepPlanarCurves : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iCurves;

	public readonly CSInputChannel iClosed;

	public readonly CSInputChannel iToggle;

	public readonly CSOutputChannel oBrep;

	public CSSimpleSweepPlanarCurves(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iClosed = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iRail.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Rails");
		}
		OnNurbsCurve onNurbsCurve = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onNurbsCurve == null)
		{
			return UpdateMissing("Missing Rail");
		}
		CSDataArray flattenArray2 = iCurves.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curves");
		}
		List<IOnCurve> list = new List<IOnCurve>();
		for (int i = 0; i < flattenArray2.GetArrayCount(); i++)
		{
			if (flattenArray2.GetArrayItem(i) == null)
			{
				return UpdateMissing("Missing Curve In Array (No Object)");
			}
			OnCurve onCurve = flattenArray2.GetArrayItem(i).GetNurbsCurve().Convert();
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
			if (CSBlockUtil.CSUtilMath.AveragePlaneOfCurve(list[j], out var plane))
			{
				OnPlaneSurface onPlaneSurface = new OnPlaneSurface(plane);
				On3dPoint fixed_point = plane.PointAt(0.5, 0.5);
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
				if (!flag)
				{
					onNurbsCurve.GetClosestPoint(list[j].PointAtStart(), ref t);
				}
				bool value = true;
				if (!iToggle.GetBoolean(ref value))
				{
					value = true;
				}
				if (value)
				{
					On3dPoint point = new On3dPoint();
					On3dVector tangent = new On3dVector();
					onNurbsCurve.EvTangent(t, ref point, ref tangent);
					On3dVector arg = new On3dVector(plane.Normal());
					if (OnUtil.ON_DotProduct(tangent, arg) < 0.0)
					{
						OnCurve onCurve2 = list[j].DuplicateCurve();
						onCurve2.Reverse();
						list[j] = onCurve2;
					}
					if (list[j].IsClosed())
					{
						double t2 = 0.0;
						list[j].GetClosestPoint(point, ref t2);
						On3dPoint on3dPoint = list[j].PointAt(t2);
						if ((list[j].PointAtStart() - on3dPoint).Length() > 0.01)
						{
							OnCurve onCurve3 = list[j].DuplicateCurve();
							onCurve3.ChangeClosedCurveSeam(t2);
							list[j] = onCurve3;
						}
					}
				}
				list2.Add(t);
				continue;
			}
			oBrep.SetData(null);
			return UpdateError("Input Curve not planar");
		}
		double value2 = 0.0;
		if (iClosed != null)
		{
			iClosed.GetDouble(ref value2);
		}
		if (!CSUtilSweep.Sweep1(onNurbsCurve, list.ToArray(), list2.ToArray(), out var breps, value2 > 0.5) || breps.Length == 0)
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
