using System;
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

namespace CSBlocks.Collars.Obsolete;

[Obsolete]
[CSBlock("SweepCollarRail_V1", "{7B55FDB0-1904-4d6e-ADCC-C9D9CEEA3987}")]
public sealed class CSSweepCollarRail1 : CSBlock
{
	public CSInputChannel InProfile { get; }

	public CSInputChannel InBottomTrim { get; }

	public CSOutputChannel OutRail { get; }

	public CSSweepCollarRail1(string identifier)
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
			Point3d point3d = new Point3d(value2.PointAt(num2));
			point3d.Y = 0.0;
			Point3d to = point3d;
			point3d = new Point3d(value2.PointAt(num4));
			point3d.Y = 0.0;
			Point3d to2 = point3d;
			LineCurve lineCurve = new LineCurve(value2.PointAt(num2), to);
			LineCurve lineCurve2 = new LineCurve(value2.PointAt(num4), to2);
			Curve[] array = Curve.JoinCurves(new Curve[3] { lineCurve, value2, lineCurve2 });
			if (array == null)
			{
				return UpdateError("Error merging curves");
			}
			if (array.Length != 1)
			{
				return UpdateError("Error merging curves");
			}
			value2 = array[0].ToNurbsCurve();
			if (value2 == null)
			{
				return UpdateError("Error merging curves");
			}
			value2.Transform(xUp);
			cSDataArray.Append(new CSOnGeometryData(value2, xUp, applyTransform: false));
		}
		OutRail.SetData(cSDataArray);
		return base.Update();
	}
}
