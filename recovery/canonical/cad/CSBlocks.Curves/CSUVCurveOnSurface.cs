using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Collections;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("UVCurveOnSurface", "{61E1BBC2-2516-4d33-9608-5C8C64054B8F}")]
public sealed class CSUVCurveOnSurface : CSBlock
{
	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iParameters;

	public readonly CSInputChannel iMode;

	public readonly CSInputChannel iRebuild;

	public readonly CSOutputChannel oCurve;

	public CSUVCurveOnSurface(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iParameters = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iMode = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRebuild = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iSurface.GetFlattenArray();
		CSDataArray flattenArray2 = iParameters.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface");
		}
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Parameters");
		}
		NurbsSurface nurbsSurface = flattenArray.GetArrayItem(0).GetNurbsSurface();
		if (nurbsSurface == null)
		{
			return UpdateMissing("Missing Surface");
		}
		nurbsSurface.GetBoundingBox(accurate: true);
		int value = 0;
		iMode.GetInteger(ref value);
		double value2 = 0.0;
		iRebuild.GetDouble(ref value2);
		CSDataArray cSDataArray = new CSDataArray();
		List<double> list = new List<double>();
		for (int i = 0; i < flattenArray2.GetArrayCount(); i++)
		{
			double value3 = 0.0;
			if (flattenArray2.GetArrayItem(i).GetDouble(ref value3))
			{
				list.Add(value3);
			}
		}
		if (list.Count < 1)
		{
			return UpdateError("Missing Parameters");
		}
		if (list.Count == 1)
		{
			list.Add(list[0]);
		}
		for (int j = 0; j < flattenArray.GetArrayCount(); j++)
		{
			NurbsSurface nurbsSurface2 = flattenArray.GetArrayItem(j).GetNurbsSurface();
			if (nurbsSurface2 == null)
			{
				continue;
			}
			Point3dList point3dList = new Point3dList();
			if (value == 0)
			{
				Interval interval = nurbsSurface2.Domain(1);
				Interval interval2 = nurbsSurface2.Domain(0);
				for (double num = 0.0; num < (double)list.Count; num += 1.0)
				{
					point3dList.Add(new Point3d(interval2.ParameterAt(list[(int)num]), interval.ParameterAt(num / ((double)list.Count - 1.0)), 0.0));
				}
			}
			else
			{
				Interval interval3 = nurbsSurface2.Domain(1);
				Interval interval4 = nurbsSurface2.Domain(0);
				for (double num2 = 0.0; num2 < (double)list.Count; num2 += 1.0)
				{
					point3dList.Add(new Point3d(interval4.ParameterAt(num2 / ((double)list.Count - 1.0)), interval3.ParameterAt(list[(int)num2]), 0.0));
				}
			}
			NurbsCurve nurbsCurve = Curve.CreateInterpolatedCurve(point3dList, 3, CurveKnotStyle.Uniform).ToNurbsCurve();
			if (nurbsCurve == null)
			{
				continue;
			}
			NurbsCurve nurbsCurve2 = nurbsSurface2.Pushup(nurbsCurve, 0.01).ToNurbsCurve();
			if (value2 > 0.0)
			{
				double num3 = Math.Max(3.0, Math.Min(value2, 300.0));
				if (value2 == 1.0)
				{
					num3 = Math.Max(3.0, Math.Min(num3, 300.0));
				}
				nurbsCurve2 = nurbsCurve2.Rebuild((int)num3, 3, preserveTangents: true);
			}
			cSDataArray.Append(new CSOnGeometryData(nurbsCurve2));
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
