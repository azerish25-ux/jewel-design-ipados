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
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
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
		((GeometryBase)nurbsSurface).GetBoundingBox(true);
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
			Point3dList val = new Point3dList();
			if (value == 0)
			{
				Interval val2 = ((Surface)nurbsSurface2).Domain(1);
				Interval val3 = ((Surface)nurbsSurface2).Domain(0);
				for (double num = 0.0; num < (double)list.Count; num += 1.0)
				{
					((RhinoList<Point3d>)(object)val).Add(new Point3d(((Interval)(ref val3)).ParameterAt(list[(int)num]), ((Interval)(ref val2)).ParameterAt(num / ((double)list.Count - 1.0)), 0.0));
				}
			}
			else
			{
				Interval val4 = ((Surface)nurbsSurface2).Domain(1);
				Interval val5 = ((Surface)nurbsSurface2).Domain(0);
				for (double num2 = 0.0; num2 < (double)list.Count; num2 += 1.0)
				{
					((RhinoList<Point3d>)(object)val).Add(new Point3d(((Interval)(ref val5)).ParameterAt(num2 / ((double)list.Count - 1.0)), ((Interval)(ref val4)).ParameterAt(list[(int)num2]), 0.0));
				}
			}
			NurbsCurve val6 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val, 3, (CurveKnotStyle)0).ToNurbsCurve();
			if (val6 == null)
			{
				continue;
			}
			NurbsCurve val7 = ((Surface)nurbsSurface2).Pushup((Curve)(object)val6, 0.01).ToNurbsCurve();
			if (value2 > 0.0)
			{
				double num3 = Math.Max(3.0, Math.Min(value2, 300.0));
				if (value2 == 1.0)
				{
					num3 = Math.Max(3.0, Math.Min(num3, 300.0));
				}
				val7 = ((Curve)val7).Rebuild((int)num3, 3, true);
			}
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val7));
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
