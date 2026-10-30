using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rhino.Geometry;

namespace CSGemInstance;

public static class CSUtilCpt
{
	private const string _format = "{0:##0.000000}";

	public static bool GetCptStr(NurbsCurve crv, out string strPoints, out string strKnots, out string strWeights, out string strDegree, out string strDomain, out string strBoundingBox)
	{
		GetCptArray(crv, out var points, out var knots, out var weights, out var degree, out var domain, out var boundingbox);
		strPoints = StrFromPointArray(points);
		strKnots = StrFromDoubleArray(knots);
		strWeights = StrFromDoubleArray(weights);
		strDegree = degree.ToString();
		strDomain = StrFromDoubleArray(domain);
		strBoundingBox = $"{StrFromPoint(boundingbox[0])};{StrFromPoint(boundingbox[1])}";
		return true;
	}

	private static bool GetCptArray(NurbsCurve crv, out List<Point3d> points, out List<double> knots, out List<double> weights, out int degree, out List<double> domain, out List<Point3d> boundingbox)
	{
		points = new List<Point3d>();
		weights = new List<double>();
		foreach (ControlPoint item in (IEnumerable<ControlPoint>)crv.Points)
		{
			points.Add(item.Location);
			weights.Add(item.Weight);
		}
		knots = crv.Knots.ToList();
		degree = crv.Degree;
		domain = new List<double>
		{
			crv.Domain.T0,
			crv.Domain.T1
		};
		BoundingBox boundingBox = crv.GetBoundingBox(accurate: true);
		boundingbox = new List<Point3d> { boundingBox.Min, boundingBox.Max };
		return crv.IsValid;
	}

	private static string StrFromPoint(Point3d p)
	{
		string text = string.Format(CultureInfo.InvariantCulture, "{0:##0.000000}", new object[1] { p.X });
		string text2 = string.Format(CultureInfo.InvariantCulture, "{0:##0.000000}", new object[1] { p.Y });
		string text3 = string.Format(CultureInfo.InvariantCulture, "{0:##0.000000}", new object[1] { p.Z });
		return string.Join(",", text, text2, text3);
	}

	private static string StrFromDoubleArray(ICollection<double> inValues)
	{
		string seed = $"[{inValues.Count - 1}]";
		return inValues.Aggregate(seed, (string current, double inValue) => current + ";" + string.Format(CultureInfo.InvariantCulture, "{0:##0.000000}", new object[1] { inValue }));
	}

	private static string StrFromPointArray(ICollection<Point3d> inValues)
	{
		string seed = $"[{inValues.Count - 1}]";
		return inValues.Aggregate(seed, (string current, Point3d inValue) => current + ";" + StrFromPoint(inValue));
	}
}
