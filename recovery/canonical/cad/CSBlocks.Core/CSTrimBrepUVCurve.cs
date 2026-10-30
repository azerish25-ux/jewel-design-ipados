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
		BrepFace brepFace = value.Faces[0];
		bool value2 = false;
		iTrim.GetBoolean(ref value2);
		Interval interval = brepFace.Domain(0);
		Interval interval2 = brepFace.Domain(1);
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
		double x = interval.ParameterAt(value4);
		double y = interval2.ParameterAt(value5);
		Point3d point3d = new Point3d(x, y, 0.0);
		Vector3d vector3d = new Vector3d(1.0, 0.0, 0.0);
		vector3d.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value3), Vector3d.ZAxis);
		double num = interval.Length + interval2.Length;
		vector3d *= num;
		Line line = new Line(point3d - vector3d, point3d + vector3d);
		Line lineB = new Line(new Point3d(interval.Min, num, 0.0), new Point3d(interval.Min, 0.0 - num, 0.0));
		Line lineB2 = new Line(new Point3d(interval.Max, num, 0.0), new Point3d(interval.Max, 0.0 - num, 0.0));
		Line lineB3 = new Line(new Point3d(num, interval2.Min, 0.0), new Point3d(0.0 - num, interval2.Min, 0.0));
		Line lineB4 = new Line(new Point3d(num, interval2.Max, 0.0), new Point3d(0.0 - num, interval2.Max, 0.0));
		double a = 0.0;
		double b = 0.0;
		if (Intersection.LineLine(line, lineB, out a, out b) && a > 0.0 && a < 1.0)
		{
			if (line.To.X < line.From.X)
			{
				line.To = line.PointAt(a);
			}
			else
			{
				line.From = line.PointAt(a);
			}
		}
		if (Intersection.LineLine(line, lineB2, out a, out b) && a > 0.0 && a < 1.0)
		{
			if (line.To.X < line.From.X)
			{
				line.From = line.PointAt(a);
			}
			else
			{
				line.To = line.PointAt(a);
			}
		}
		if (Intersection.LineLine(line, lineB3, out a, out b) && a > 0.0 && a < 1.0)
		{
			if (line.To.Y > line.From.Y)
			{
				line.From = line.PointAt(a);
			}
			else
			{
				line.To = line.PointAt(a);
			}
		}
		if (Intersection.LineLine(line, lineB4, out a, out b) && a > 0.0 && a < 1.0)
		{
			if (line.To.Y > line.From.Y)
			{
				line.To = line.PointAt(a);
			}
			else
			{
				line.From = line.PointAt(a);
			}
		}
		Curve curve = new LineCurve(line);
		if (value2)
		{
			List<NurbsCurve> list = new List<NurbsCurve>();
			for (int i = 0; i < brepFace.Loops.Count; i++)
			{
				BrepLoop brepLoop = brepFace.Loops[i];
				for (int j = 0; j < brepLoop.Trims.Count; j++)
				{
					list.Add(brepLoop.Trims[j].ToNurbsCurve());
				}
			}
			List<double> list2 = new List<double>();
			foreach (NurbsCurve item in list)
			{
				CurveIntersections curveIntersections = Intersection.CurveCurve(curve, item, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				for (int k = 0; k < curveIntersections.Count; k++)
				{
					list2.Add(curveIntersections[k].ParameterA);
				}
			}
			list2.Sort();
			if (list2.Count >= 2)
			{
				curve = curve.Trim(new Interval(list2[0], list2[list2.Count - 1]));
			}
		}
		Curve curve2 = brepFace.Pushup(curve, 0.01);
		if (curve2 == null)
		{
			return UpdateError("Failed Pushup");
		}
		oCurve.SetData(new CSOnGeometryData(curve2));
		return base.Update();
	}
}
