using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Core;

[CSBlock("TrimBrepUVCurve", "{4CB7BAD5-8C05-4A61-AFB3-B86B8C25FD16}")]
public sealed class CSTrimBrepUVCurve : CSBlock
{
	public readonly CSInputChannel iBrep;

	public readonly CSInputChannel iAngle;

	public readonly CSInputChannel iStartX;

	public readonly CSInputChannel iStartY;

	public readonly CSInputChannel iTrim;

	public readonly CSOutputChannel oCurve;

	public CSTrimBrepUVCurve(string identifier)
		: base(identifier)
	{
		iBrep = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStartX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStartY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTrim = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		if (iBrep == null)
		{
			return UpdateMissing("Missing Surface");
		}
		CSData data = iBrep.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Surface");
		}
		CSDataHelper.GetBrep(data, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing Surface");
		}
		if (value.Faces.Count < 1)
		{
			return UpdateMissing("Bad Brep");
		}
		BrepFace val = value.Faces[0];
		bool value2 = false;
		iTrim.GetBoolean(ref value2);
		Interval val2 = ((Surface)val).Domain(0);
		Interval val3 = ((Surface)val).Domain(1);
		double value3 = 0.0;
		if (iAngle != null)
		{
			iAngle.GetDouble(ref value3);
		}
		double value4 = 0.5;
		double value5 = 0.5;
		if (iStartX != null)
		{
			iStartX.GetDouble(ref value4);
		}
		if (iStartY != null)
		{
			iStartY.GetDouble(ref value5);
		}
		double num = ((Interval)(ref val2)).ParameterAt(value4);
		double num2 = ((Interval)(ref val3)).ParameterAt(value5);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(num, num2, 0.0);
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(1.0, 0.0, 0.0);
		((Vector3d)(ref val5)).Rotate(CSBlockUtil.CSUtilMath.ToRadians(value3), Vector3d.ZAxis);
		double num3 = ((Interval)(ref val2)).Length + ((Interval)(ref val3)).Length;
		val5 *= num3;
		Line val6 = default(Line);
		((Line)(ref val6))._002Ector(val4 - val5, val4 + val5);
		Line val7 = default(Line);
		((Line)(ref val7))._002Ector(new Point3d(((Interval)(ref val2)).Min, num3, 0.0), new Point3d(((Interval)(ref val2)).Min, 0.0 - num3, 0.0));
		Line val8 = default(Line);
		((Line)(ref val8))._002Ector(new Point3d(((Interval)(ref val2)).Max, num3, 0.0), new Point3d(((Interval)(ref val2)).Max, 0.0 - num3, 0.0));
		Line val9 = default(Line);
		((Line)(ref val9))._002Ector(new Point3d(num3, ((Interval)(ref val3)).Min, 0.0), new Point3d(0.0 - num3, ((Interval)(ref val3)).Min, 0.0));
		Line val10 = default(Line);
		((Line)(ref val10))._002Ector(new Point3d(num3, ((Interval)(ref val3)).Max, 0.0), new Point3d(0.0 - num3, ((Interval)(ref val3)).Max, 0.0));
		double num4 = 0.0;
		double num5 = 0.0;
		Point3d val11;
		if (Intersection.LineLine(val6, val7, ref num4, ref num5) && num4 > 0.0 && num4 < 1.0)
		{
			val11 = ((Line)(ref val6)).To;
			double x = ((Point3d)(ref val11)).X;
			val11 = ((Line)(ref val6)).From;
			if (x < ((Point3d)(ref val11)).X)
			{
				((Line)(ref val6)).To = ((Line)(ref val6)).PointAt(num4);
			}
			else
			{
				((Line)(ref val6)).From = ((Line)(ref val6)).PointAt(num4);
			}
		}
		if (Intersection.LineLine(val6, val8, ref num4, ref num5) && num4 > 0.0 && num4 < 1.0)
		{
			val11 = ((Line)(ref val6)).To;
			double x2 = ((Point3d)(ref val11)).X;
			val11 = ((Line)(ref val6)).From;
			if (x2 < ((Point3d)(ref val11)).X)
			{
				((Line)(ref val6)).From = ((Line)(ref val6)).PointAt(num4);
			}
			else
			{
				((Line)(ref val6)).To = ((Line)(ref val6)).PointAt(num4);
			}
		}
		if (Intersection.LineLine(val6, val9, ref num4, ref num5) && num4 > 0.0 && num4 < 1.0)
		{
			val11 = ((Line)(ref val6)).To;
			double y = ((Point3d)(ref val11)).Y;
			val11 = ((Line)(ref val6)).From;
			if (y > ((Point3d)(ref val11)).Y)
			{
				((Line)(ref val6)).From = ((Line)(ref val6)).PointAt(num4);
			}
			else
			{
				((Line)(ref val6)).To = ((Line)(ref val6)).PointAt(num4);
			}
		}
		if (Intersection.LineLine(val6, val10, ref num4, ref num5) && num4 > 0.0 && num4 < 1.0)
		{
			val11 = ((Line)(ref val6)).To;
			double y2 = ((Point3d)(ref val11)).Y;
			val11 = ((Line)(ref val6)).From;
			if (y2 > ((Point3d)(ref val11)).Y)
			{
				((Line)(ref val6)).To = ((Line)(ref val6)).PointAt(num4);
			}
			else
			{
				((Line)(ref val6)).From = ((Line)(ref val6)).PointAt(num4);
			}
		}
		Curve val12 = (Curve)new LineCurve(val6);
		if (value2)
		{
			List<NurbsCurve> list = new List<NurbsCurve>();
			for (int i = 0; i < val.Loops.Count; i++)
			{
				BrepLoop val13 = val.Loops[i];
				for (int j = 0; j < val13.Trims.Count; j++)
				{
					list.Add(((Curve)val13.Trims[j]).ToNurbsCurve());
				}
			}
			List<double> list2 = new List<double>();
			foreach (NurbsCurve item in list)
			{
				CurveIntersections val14 = Intersection.CurveCurve(val12, (Curve)(object)item, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				for (int k = 0; k < val14.Count; k++)
				{
					list2.Add(val14[k].ParameterA);
				}
			}
			list2.Sort();
			if (list2.Count >= 2)
			{
				val12 = val12.Trim(new Interval(list2[0], list2[list2.Count - 1]));
			}
		}
		Curve val15 = ((Surface)val).Pushup(val12, 0.01);
		if (val15 == null)
		{
			return UpdateError("Failed Pushup");
		}
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)val15));
		return base.Update();
	}
}
