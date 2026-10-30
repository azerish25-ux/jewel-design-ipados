using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSGemInstance;

public static class CSUtilCpt
{
	private const string _format = "{0:##0.000000}";

	public static bool GetCptStr(NurbsCurve crv, out string strPoints, out string strKnots, out string strWeights, out string strDegree, out string strDomain, out string strBoundingBox)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		points = new List<Point3d>();
		weights = new List<double>();
		foreach (ControlPoint item in (IEnumerable<ControlPoint>)crv.Points)
		{
			ControlPoint current = item;
			points.Add(((ControlPoint)(ref current)).Location);
			weights.Add(((ControlPoint)(ref current)).Weight);
		}
		knots = ((IEnumerable<double>)crv.Knots).ToList();
		degree = ((Curve)crv).Degree;
		List<double> list = new List<double>();
		Interval domain2 = ((Curve)crv).Domain;
		list.Add(((Interval)(ref domain2)).T0);
		domain2 = ((Curve)crv).Domain;
		list.Add(((Interval)(ref domain2)).T1);
		domain = list;
		BoundingBox boundingBox = ((GeometryBase)crv).GetBoundingBox(true);
		boundingbox = new List<Point3d>
		{
			((BoundingBox)(ref boundingBox)).Min,
			((BoundingBox)(ref boundingBox)).Max
		};
		return ((CommonObject)crv).IsValid;
	}

	private static string StrFromPoint(Point3d p)
	{
		string text = string.Format(CultureInfo.InvariantCulture, "{0:##0.000000}", new object[1] { ((Point3d)(ref p)).X });
		string text2 = string.Format(CultureInfo.InvariantCulture, "{0:##0.000000}", new object[1] { ((Point3d)(ref p)).Y });
		string text3 = string.Format(CultureInfo.InvariantCulture, "{0:##0.000000}", new object[1] { ((Point3d)(ref p)).Z });
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
