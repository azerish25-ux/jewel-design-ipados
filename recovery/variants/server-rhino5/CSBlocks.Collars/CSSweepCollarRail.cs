using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Collars;

[CSBlock("SweepCollarRail", "{0EB40227-05EF-4DE5-98FB-33EF628A72D4}")]
public sealed class CSSweepCollarRail : CSBlock
{
	public CSInputChannel InProfile { get; }

	public CSInputChannel InBottomTrim { get; }

	public CSOutputChannel OutRail { get; }

	public CSSweepCollarRail(string identifier)
		: base(identifier)
	{
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InBottomTrim = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRail = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InProfile.GetFlattenArray();
		double num = InBottomTrim.Double;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				return UpdateError("Rail Data Missing");
			}
			Transform value;
			if (arrayItem is ICSTransform iCSTransform)
			{
				if (!iCSTransform.GetTransform(out value))
				{
					value = Transform.Identity;
				}
			}
			else
			{
				value = Transform.Identity;
			}
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value, out var xUp, out var xDown))
			{
				return UpdateError("Error getting Transform");
			}
			CSDataHelper.GetNurbsCurve(arrayItem, out var value2);
			if (value2 == null)
			{
				return UpdateError("Error getting nurbscurve");
			}
			value2.Transform(xDown);
			BoundingBox boundingBox = value2.GetBoundingBox(accurate: true);
			if (!boundingBox.IsValid)
			{
				return UpdateError("Error getting boundingbox");
			}
			if (!(num > 0.0) || !(num < boundingBox.Max.Y))
			{
				continue;
			}
			LineCurve curveB = new LineCurve(new Point3d(-50.0, num, 0.0), new Point3d(50.0, num, 0.0));
			CurveIntersections curveIntersections = Intersection.CurveCurve(value2, curveB, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count < 2)
			{
				return UpdateError("Error intersection");
			}
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			for (int j = 0; j < curveIntersections.Count; j++)
			{
				double x = value2.PointAt(curveIntersections[j].ParameterA).X;
				if (j == 0 || x < num3)
				{
					num3 = x;
					num2 = curveIntersections[j].ParameterA;
				}
				if (j == 0 || x > num5)
				{
					num5 = x;
					num4 = curveIntersections[j].ParameterA;
				}
			}
			value2 = value2.Trim(new Interval(num2, num4)).ToNurbsCurve();
			if (!value2.IsValid)
			{
				return UpdateError("Error triming");
			}
			Point3d point3d = value2.PointAt(num2);
			Point3d point3d2 = value2.PointAt(num4);
			Vector3d vector3d = value2.TangentAt(num2);
			vector3d.Reverse();
			vector3d.Unitize();
			Vector3d vector3d2 = value2.TangentAt(num4);
			vector3d2.Unitize();
			Point3d to = point3d + vector3d * 50.0;
			Point3d to2 = point3d2 + vector3d2 * 50.0;
			Line line = new Line(point3d, to);
			Line line2 = new Line(point3d2, to2);
			if (!Intersection.LinePlane(line, Plane.WorldZX, out var lineParameter))
			{
				return UpdateError("ZX intersection failed on Rhs");
			}
			to = line.PointAt(lineParameter);
			if (!Intersection.LinePlane(new Line(point3d2, to2), Plane.WorldZX, out var lineParameter2))
			{
				return UpdateError("ZX intersection failed on Rhs");
			}
			to2 = line2.PointAt(lineParameter2);
			CurveEnd side;
			CurveEnd side2;
			if (value2.PointAtStart.DistanceTo(point3d) < value2.PointAtStart.DistanceTo(point3d2))
			{
				side = CurveEnd.Start;
				side2 = CurveEnd.End;
			}
			else
			{
				side = CurveEnd.End;
				side2 = CurveEnd.Start;
			}
			Curve curve = value2.DuplicateCurve();
			curve = curve.Extend(side, point3d.DistanceTo(to), CurveExtensionStyle.Line);
			if (curve == null)
			{
				return UpdateError("Extend failed on rail Lhs");
			}
			curve = curve.Extend(side2, point3d2.DistanceTo(to2), CurveExtensionStyle.Line);
			if (curve == null)
			{
				return UpdateError("Extend failed on rail Rhs");
			}
			curve.Transform(xUp);
			cSDataArray.Append(new CSOnGeometryData(curve, xUp, applyTransform: false));
		}
		OutRail.SetData(cSDataArray);
		return base.Update();
	}
}
