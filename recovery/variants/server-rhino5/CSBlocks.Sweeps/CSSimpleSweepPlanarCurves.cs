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
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		CSDataArray flattenArray = iRail.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Rails");
		}
		OnNurbsCurve val = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val == null)
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
			OnCurve val2 = (OnCurve)(object)flattenArray2.GetArrayItem(i).GetNurbsCurve().Convert();
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
			double num = ((OnCurve)val).Domain().Min();
			if (CSBlockUtil.CSUtilMath.AveragePlaneOfCurve(list[j], out var plane))
			{
				OnPlaneSurface val3 = new OnPlaneSurface((IOnPlane)(object)plane);
				On3dPoint val4 = plane.PointAt(0.5, 0.5);
				OnXform val5 = new OnXform();
				val5.Scale((IOn3dPoint)(object)val4, 1000.0);
				val3.Transform((IOnXform)(object)val5);
				ArrayOnX_EVENT val6 = new ArrayOnX_EVENT();
				((OnCurve)val).IntersectSurface((IOnSurface)(object)val3, ref val6, CSApplicationHelper.Application.Tolerance);
				if (val6.Count() > 0)
				{
					int num2 = 0;
					for (int k = 1; k < val6.Count(); k++)
					{
						if (val6[k][0].DistanceTo((IOn3dPoint)(object)list[j].PointAtStart()) < val6[num2][0].DistanceTo((IOn3dPoint)(object)list[j].PointAtStart()))
						{
							num2 = k;
						}
					}
					num = val6[num2][0];
					flag = true;
				}
				if (!flag)
				{
					((OnCurve)val).GetClosestPoint((IOn3dPoint)(object)list[j].PointAtStart(), ref num);
				}
				bool value = true;
				if (!iToggle.GetBoolean(ref value))
				{
					value = true;
				}
				if (value)
				{
					On3dPoint val7 = new On3dPoint();
					On3dVector val8 = new On3dVector();
					((OnCurve)val).EvTangent(num, ref val7, ref val8);
					On3dVector val9 = new On3dVector(plane.Normal());
					if (OnUtil.ON_DotProduct((IOn3dVector)(object)val8, (IOn3dVector)(object)val9) < 0.0)
					{
						OnCurve val10 = list[j].DuplicateCurve();
						val10.Reverse();
						list[j] = (IOnCurve)(object)val10;
					}
					if (list[j].IsClosed())
					{
						double num3 = 0.0;
						list[j].GetClosestPoint((IOn3dPoint)(object)val7, ref num3);
						On3dPoint val11 = list[j].PointAt(num3);
						if ((list[j].PointAtStart() - (IOn3dPoint)(object)val11).Length() > 0.01)
						{
							OnCurve val12 = list[j].DuplicateCurve();
							val12.ChangeClosedCurveSeam(num3);
							list[j] = (IOnCurve)(object)val12;
						}
					}
				}
				list2.Add(num);
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
		if (!CSUtilSweep.Sweep1((IOnCurve)(object)val, list.ToArray(), list2.ToArray(), out var breps, value2 > 0.5) || breps.Length == 0)
		{
			return UpdateError("Failed Sweep1");
		}
		base.OutputChannels.Clear();
		for (int l = 0; l < breps.Length; l++)
		{
			OnBrep val13 = breps[l];
			if (val13 != null)
			{
				new CSOutputChannel($"{base.Identifier}_{l}", this, ChannelDataType.Any, ChannelArrayType.Individual).SetData(new CSOnGeometryData(val13.Convert()));
			}
		}
		return base.Update();
	}
}
