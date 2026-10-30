using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Rhino.Collections;
using Rhino.Geometry;

namespace CSBlockUtil;

public static class CSUtilSweepRC
{
	public sealed class Sweep4Profile
	{
		public Curve Crv { get; }

		public Plane Pln { get; }

		public bool IsSetup
		{
			get
			{
				if (!double.IsNaN(CrvParam1) && !double.IsNaN(CrvParam2) && !double.IsNaN(CrvParam3))
				{
					return !double.IsNaN(CrvParam4);
				}
				return false;
			}
		}

		public bool IsValid
		{
			get
			{
				if (Crv != null && Crv.IsValid)
				{
					return IsSetup;
				}
				return false;
			}
		}

		public double CrvParam1 { get; private set; }

		public double CrvParam2 { get; private set; }

		public double CrvParam3 { get; private set; }

		public double CrvParam4 { get; private set; }

		public Sweep4Profile(Curve crv)
			: this(crv, double.NaN, double.NaN, double.NaN, double.NaN)
		{
		}

		public Sweep4Profile(Curve crv, double crvParam1, double crvParam2, double crvParam3, double crvParam4)
		{
			Crv = crv.DuplicateCurve();
			Pln = Plane.Unset;
			if (Crv.IsPlanar() && Crv.TryGetPlane(out var plane))
			{
				Pln = plane;
			}
			CrvParam1 = crvParam1;
			CrvParam2 = crvParam2;
			CrvParam3 = crvParam3;
			CrvParam4 = crvParam4;
		}

		public bool MatchCurveDirection(Curve refCrv, int pointCount = 20)
		{
			if (Crv == null)
			{
				return false;
			}
			bool flag = false;
			Point3d testPoint = Point3d.Unset;
			Point3d testPoint2 = Point3d.Unset;
			Point3d testPoint3 = Point3d.Unset;
			Point3d testPoint4 = Point3d.Unset;
			if (IsSetup)
			{
				flag = true;
				testPoint = Crv.PointAt(CrvParam1);
				testPoint2 = Crv.PointAt(CrvParam2);
				testPoint3 = Crv.PointAt(CrvParam3);
				testPoint4 = Crv.PointAt(CrvParam4);
				ResetParams();
				if (!testPoint.IsValid || !testPoint2.IsValid || !testPoint3.IsValid || !testPoint4.IsValid)
				{
					return false;
				}
			}
			CSUtilSweepRC.MatchCurveDirection(refCrv, Crv, pointCount);
			if (!flag)
			{
				return true;
			}
			if (!Crv.ClosestPoint(testPoint, out var t))
			{
				return false;
			}
			if (!Crv.ClosestPoint(testPoint2, out var t2))
			{
				return false;
			}
			if (!Crv.ClosestPoint(testPoint3, out var t3))
			{
				return false;
			}
			if (!Crv.ClosestPoint(testPoint4, out var t4))
			{
				return false;
			}
			CrvParam1 = t;
			CrvParam2 = t2;
			CrvParam3 = t3;
			CrvParam4 = t4;
			return true;
		}

		public void ResetParams()
		{
			CrvParam1 = double.NaN;
			CrvParam2 = double.NaN;
			CrvParam3 = double.NaN;
			CrvParam4 = double.NaN;
		}

		public bool SetupFromRails(Curve rail1, Curve rail2, Curve rail3, Curve rail4)
		{
			if (Crv == null)
			{
				return false;
			}
			if (IsSetup)
			{
				List<double> list = new List<double> { CrvParam1, CrvParam2, CrvParam3, CrvParam4 };
				list.Sort();
				CrvParam1 = list[0];
				CrvParam2 = list[1];
				CrvParam3 = list[2];
				CrvParam4 = list[3];
			}
			else
			{
				if (!GetCrvParamFromRail(rail1, out var crvParam))
				{
					return false;
				}
				if (!GetCrvParamFromRail(rail2, out var crvParam2))
				{
					return false;
				}
				if (!GetCrvParamFromRail(rail3, out var crvParam3))
				{
					return false;
				}
				if (!GetCrvParamFromRail(rail4, out var crvParam4))
				{
					return false;
				}
				CrvParam1 = crvParam;
				CrvParam2 = crvParam2;
				CrvParam3 = crvParam3;
				CrvParam4 = crvParam4;
			}
			return true;
		}

		public bool Split(out Curve crvPiece0, out Curve crvPiece1, out Curve crvPiece2, out Curve crvPiece3)
		{
			crvPiece0 = null;
			crvPiece1 = null;
			crvPiece2 = null;
			crvPiece3 = null;
			if (!IsValid)
			{
				return false;
			}
			Curve crv = Crv.DuplicateCurve();
			if (crv == null)
			{
				return false;
			}
			double[] crvParams = new double[4] { CrvParam1, CrvParam2, CrvParam3, CrvParam4 };
			if (!CSUtilCurves.ChangeClosedCurveSeam(ref crv, ref crvParams, CrvParam1))
			{
				return false;
			}
			Curve[] array = crv.Split(crvParams);
			if (array.Count() != 4)
			{
				return false;
			}
			crvPiece0 = array[0];
			crvPiece1 = array[1];
			crvPiece2 = array[2];
			crvPiece3 = array[3];
			return true;
		}

		private bool GetCrvParamFromRail(Curve rail, out double crvParam)
		{
			crvParam = double.NaN;
			if (rail == null)
			{
				return false;
			}
			if (!rail.IsValid)
			{
				return false;
			}
			if (Crv == null)
			{
				return false;
			}
			if (!Crv.IsValid)
			{
				return false;
			}
			Point3d pointOnOtherCurve;
			Point3d pointOnThisCurve;
			if (Pln.IsValid)
			{
				if (!CSUtilIntersect.GetClosestIntersection(rail, Pln, Crv, out var bestIntersectionEvent))
				{
					return false;
				}
				pointOnOtherCurve = bestIntersectionEvent.PointA;
			}
			else if (!Crv.ClosestPoints(rail, out pointOnThisCurve, out pointOnOtherCurve))
			{
				return false;
			}
			if (!Crv.ClosestPoint(pointOnOtherCurve, out var t))
			{
				return false;
			}
			crvParam = t;
			return true;
		}
	}

	public static Brep Loft(IEnumerable<Curve> profiles, LoftType loftType, bool doCapped = false)
	{
		Brep[] array = Brep.CreateFromLoft(profiles, Point3d.Unset, Point3d.Unset, loftType, closed: false);
		Brep brep = ((array.Length == 1) ? array[0].DuplicateBrep() : CSUtilRhino.JoinAllBreps(array, CSUtil.Tolerance));
		if (brep != null && doCapped)
		{
			brep = CSUtilRhino.CapPlanarHoles(brep, CSUtil.Tolerance);
		}
		return brep;
	}

	public static bool MatchCurveDirection(Curve curve1, Curve curve2, int pointCount)
	{
		double[] array = new double[pointCount];
		double[] array2 = new double[pointCount];
		for (int i = 0; i < pointCount; i++)
		{
			array[i] = (double)i / (double)(curve1.IsClosed ? pointCount : (pointCount - 1));
			array2[i] = (double)i / (double)(curve2.IsClosed ? pointCount : (pointCount - 1));
		}
		array = curve1.NormalizedLengthParameters(array, CSUtil.Tolerance);
		array2 = curve2.NormalizedLengthParameters(array2, CSUtil.Tolerance);
		Point3dList point3dList = new Point3dList();
		Point3dList point3dList2 = new Point3dList();
		for (int j = 0; j < array.Length; j++)
		{
			point3dList.Add(curve1.PointAt(array[j]));
			point3dList2.Add(curve2.PointAt(array2[j]));
		}
		double num = 0.0;
		double num2 = 0.0;
		int num3 = 0;
		int num4 = 0;
		double num5 = double.MaxValue;
		if (curve1.IsClosed || curve2.IsClosed)
		{
			for (int k = 0; k < point3dList2.Count; k++)
			{
				if (point3dList[0].DistanceTo(point3dList2[k]) < num5)
				{
					num5 = point3dList[0].DistanceTo(point3dList2[k]);
					num4 = k;
				}
			}
		}
		else
		{
			num4 = 0;
		}
		for (int l = 0; l < point3dList.Count; l++)
		{
			num += point3dList[(l + num3) % pointCount].DistanceTo(point3dList2[(l + num4) % pointCount]);
			num2 += point3dList[(l + num3) % pointCount].DistanceTo(point3dList2[(pointCount - l - 1 + num4) % pointCount]);
		}
		if (num2 < num)
		{
			curve2.Reverse();
			return true;
		}
		return false;
	}

	public static bool MatchCurveDirection2(Curve curve1, Curve curve2, int pointCount)
	{
		List<double> list = new List<double>();
		for (int i = 0; i < pointCount; i++)
		{
			list.Add((double)i / (double)pointCount);
		}
		double[] array = curve1.NormalizedLengthParameters(list.ToArray(), CSUtil.Tolerance);
		double[] array2 = curve2.NormalizedLengthParameters(list.ToArray(), CSUtil.Tolerance);
		Point3dList point3dList = new Point3dList();
		Point3dList point3dList2 = new Point3dList();
		for (int j = 0; j < array.Length; j++)
		{
			point3dList.Add(curve1.PointAt(array[j]));
			point3dList2.Add(curve2.PointAt(array2[j]));
		}
		double num = double.MaxValue;
		int num2 = 0;
		bool flag = false;
		for (int k = 0; k < 2; k++)
		{
			bool flag2 = k == 1;
			for (int l = 0; l < pointCount; l++)
			{
				double num3 = CalculateMatchDirectionValue(point3dList, point3dList2, 0, l, flag2);
				if (num3 < num)
				{
					num2 = l;
					flag = flag2;
					num = num3;
				}
			}
		}
		curve2.ChangeClosedCurveSeam(array2[num2]);
		if (flag)
		{
			curve2.Reverse();
		}
		return flag;
	}

	public static bool Sweep1(Curve railCurve, Curve[] profiles, double[] parameters, out Brep[] breps, bool closed, bool globalShapeBlend)
	{
		breps = null;
		if (profiles == null)
		{
			CSUtil.PrintLine("Missing Profiles");
			return false;
		}
		if (profiles.Length == 0)
		{
			CSUtil.PrintLine("No Prameters");
			return false;
		}
		if (profiles.Length != parameters.Length)
		{
			CSUtil.PrintLine("Prameters  Profiles do not match");
			return false;
		}
		for (int i = 0; i < profiles.Length; i++)
		{
			if (profiles[i] == null)
			{
				CSUtil.PrintLine("Missing a profile");
				return false;
			}
		}
		if (railCurve.IsClosed)
		{
			List<Point3d> list = new List<Point3d>();
			foreach (double t in parameters)
			{
				list.Add(railCurve.PointAt(t));
			}
			railCurve.ChangeClosedCurveSeam(parameters[0]);
			parameters[0] = railCurve.Domain.Min;
			for (int j = 1; j < list.Count; j++)
			{
				railCurve.ClosestPoint(list[j], out parameters[j]);
			}
		}
		SweepOneRail sweepOneRail = new SweepOneRail
		{
			AngleToleranceRadians = RhinoDoc.ActiveDoc.ModelAngleToleranceRadians,
			ClosedSweep = (railCurve.IsClosed && closed),
			GlobalShapeBlending = globalShapeBlend
		};
		int[] array = new int[profiles.Length];
		for (int k = 0; k < profiles.Length; k++)
		{
			array[k] = k;
		}
		if (profiles.Length > 1)
		{
			for (int l = 0; l < profiles.Length; l++)
			{
				for (int m = 0; m < profiles.Length - 1; m++)
				{
					if (parameters[array[m]] > parameters[array[m + 1]])
					{
						int num = array[m];
						array[m] = array[m + 1];
						array[m + 1] = num;
					}
				}
			}
		}
		List<Curve> list2 = new List<Curve>();
		List<double> list3 = new List<double>();
		for (int n = 0; n < profiles.Length; n++)
		{
			list2.Add(profiles[array[n]]);
			list3.Add(parameters[array[n]]);
		}
		breps = sweepOneRail.PerformSweep(railCurve, list2, list3);
		return true;
	}

	public static bool Sweep2(Curve railCurve1, Curve railCurve2, Curve[] profile, double[] parameters1, double[] parameters2, out Brep breps, bool closed, bool maintainHeight)
	{
		List<Curve> profiles = new List<Curve>(profile);
		List<double> parameters1In = new List<double>(parameters1);
		List<double> parameters2In = new List<double>(parameters2);
		Brep[] breps2;
		bool num = Sweep2(railCurve1, railCurve2, profiles, parameters1In, parameters2In, out breps2, closed, maintainHeight);
		if (num && breps2.Length != 0)
		{
			breps = breps2[0];
			return num;
		}
		breps = null;
		return num;
	}

	public static bool Sweep2(Curve railCurve1, Curve railCurve2, List<Curve> profile, List<double> parameters1, List<double> parameters2, out Brep breps, bool closed, bool maintainHeight)
	{
		Brep[] breps2;
		bool num = Sweep2(railCurve1, railCurve2, profile, parameters1, parameters2, out breps2, closed, maintainHeight);
		if (num && breps2.Length != 0)
		{
			breps = breps2[0];
			return num;
		}
		breps = null;
		return num;
	}

	public static bool Sweep2(Curve railCurve1, Curve railCurve2, List<Curve> profiles, List<double> parameters1In, List<double> parameters2In, out Brep[] breps, bool closed, bool maintainHeight)
	{
		breps = null;
		if (profiles == null)
		{
			CSUtil.PrintLine("Missing Profiles");
			return false;
		}
		if (parameters1In == null)
		{
			CSUtil.PrintLine("Missing Prameters");
			return false;
		}
		if (parameters2In == null)
		{
			CSUtil.PrintLine("Missing Prameters");
			return false;
		}
		if (profiles.Count == 0)
		{
			CSUtil.PrintLine("No Profiles");
			return false;
		}
		if (profiles.Count != parameters1In.Count)
		{
			CSUtil.PrintLine("No param1");
			return false;
		}
		if (profiles.Count != parameters2In.Count)
		{
			CSUtil.PrintLine("No param2");
			return false;
		}
		if (railCurve1 == null)
		{
			CSUtil.PrintLine("Missing Rail1");
			return false;
		}
		if (railCurve2 == null)
		{
			CSUtil.PrintLine("Missing Rail2");
			return false;
		}
		if (profiles.Count == 0)
		{
			CSUtil.PrintLine("No Prameters");
			return false;
		}
		Curve curve = railCurve1.DuplicateCurve();
		Curve curve2 = railCurve2.DuplicateCurve();
		double[] array = new double[parameters1In.Count];
		double[] array2 = new double[parameters2In.Count];
		if (curve.IsClosed && curve2.IsClosed)
		{
			curve.ChangeClosedCurveSeam(parameters1In[0]);
			curve2.ChangeClosedCurveSeam(parameters2In[0]);
			array[0] = curve.Domain.Min;
			array2[0] = curve2.Domain.Min;
			if (profiles.Count > 1)
			{
				for (int i = 1; i < profiles.Count; i++)
				{
					curve.ClosestPoint(railCurve1.PointAt(parameters1In[i]), out var t);
					array[i] = t;
					curve2.ClosestPoint(railCurve2.PointAt(parameters2In[i]), out t);
					array2[i] = t;
				}
			}
		}
		else
		{
			for (int j = 0; j < profiles.Count; j++)
			{
				array[j] = parameters1In[j];
				array2[j] = parameters2In[j];
				if (array[j] < curve.Domain.Min)
				{
					array[j] = curve.Domain.Min;
				}
				if (array[j] > curve.Domain.Max)
				{
					array[j] = curve.Domain.Max;
				}
				if (array2[j] < curve2.Domain.Min)
				{
					array2[j] = curve2.Domain.Min;
				}
				if (array2[j] > curve2.Domain.Max)
				{
					array2[j] = curve2.Domain.Max;
				}
			}
		}
		int[] array3 = new int[profiles.Count];
		for (int k = 0; k < profiles.Count; k++)
		{
			array3[k] = k;
		}
		if (profiles.Count > 1)
		{
			for (int l = 0; l < profiles.Count; l++)
			{
				for (int m = 0; m < profiles.Count - 1; m++)
				{
					if (array[array3[m]] > array[array3[m + 1]])
					{
						int num = array3[m];
						array3[m] = array3[m + 1];
						array3[m + 1] = num;
					}
				}
			}
		}
		List<Curve> list = new List<Curve>();
		List<double> list2 = new List<double>();
		List<double> list3 = new List<double>();
		for (int n = 0; n < profiles.Count; n++)
		{
			Curve curve3 = profiles[array3[n]].DuplicateCurve();
			if (curve3 != null)
			{
				list2.Add(array[array3[n]]);
				list3.Add(array2[array3[n]]);
				list.Add(curve3);
			}
			else
			{
				CSUtil.PrintLine("Unable to duplicate curve");
			}
		}
		SweepTwoRail sweepTwoRail = new SweepTwoRail
		{
			SweepTolerance = CSUtil.Tolerance,
			AngleToleranceRadians = RhinoDoc.ActiveDoc.ModelAngleToleranceRadians,
			MaintainHeight = maintainHeight,
			ClosedSweep = (railCurve1.IsClosed && railCurve2.IsClosed && closed)
		};
		breps = sweepTwoRail.PerformSweep(curve, curve2, list, list2, list3);
		return true;
	}

	public static bool Sweep4NoParam(Curve rail1, Curve rail2, Curve rail3, Curve rail4, List<Sweep4Profile> profiles, bool isClosed, bool doSweepSides, bool doTryMatchCurveDirectionRails, bool doTryMatchCurveDirectionProfiles, out Brep sweep, out string msg)
	{
		sweep = null;
		msg = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		if (rail1 == null)
		{
			stringBuilder.AppendLine("Missing rail 1");
			flag = false;
		}
		if (rail2 == null)
		{
			stringBuilder.AppendLine("Missing rail 2");
			flag = false;
		}
		if (rail3 == null)
		{
			stringBuilder.AppendLine("Missing rail 3");
			flag = false;
		}
		if (rail4 == null)
		{
			stringBuilder.AppendLine("Missing rail 4");
			flag = false;
		}
		profiles = profiles.Where((Sweep4Profile _) => _.Crv != null && _.Crv.IsValid && _.Crv.IsPlanar()).ToList();
		if (profiles.Count < 2)
		{
			stringBuilder.AppendLine("Minimum of 2 profiles required");
			flag = false;
		}
		if (!flag)
		{
			msg = stringBuilder.ToString();
			return false;
		}
		Curve curve = rail1.DuplicateCurve();
		Curve curve2 = rail2.DuplicateCurve();
		Curve curve3 = rail3.DuplicateCurve();
		Curve curve4 = rail4.DuplicateCurve();
		if (doTryMatchCurveDirectionRails)
		{
			MatchCurveDirection(curve, curve2, 20);
			MatchCurveDirection(curve, curve3, 20);
			MatchCurveDirection(curve, curve4, 20);
		}
		if (doTryMatchCurveDirectionProfiles)
		{
			for (int num = 1; num < profiles.Count; num++)
			{
				if (!profiles[num].MatchCurveDirection(profiles[0].Crv))
				{
					msg = $"MatchCurveDirection failed for profile {num} to first unsorted profile";
					return false;
				}
			}
		}
		Sweep4Profile sweep4Profile = profiles.First();
		if (!CSUtilIntersect.GetClosestIntersection(curve, sweep4Profile.Pln, sweep4Profile.Crv, out var bestIntersectionEvent))
		{
			msg = "GetClosestIntersection failed for rail 1 to first unsorted profile";
			return false;
		}
		Point3d pointA = bestIntersectionEvent.PointA;
		if (!sweep4Profile.Crv.ClosestPoint(pointA, out var t))
		{
			msg = "ClosestPoint failed for rail 1 to first unsorted profile";
			return false;
		}
		if (!CSUtilIntersect.GetClosestIntersection(curve2, sweep4Profile.Pln, sweep4Profile.Crv, out var bestIntersectionEvent2))
		{
			msg = "GetClosestIntersection failed for rail 2 to first unsorted profile";
			return false;
		}
		Point3d pointA2 = bestIntersectionEvent2.PointA;
		if (!sweep4Profile.Crv.ClosestPoint(pointA2, out var t2))
		{
			msg = "ClosestPoint failed for rail 2 to first unsorted profile";
			return false;
		}
		if (!CSUtilIntersect.GetClosestIntersection(curve3, sweep4Profile.Pln, sweep4Profile.Crv, out var bestIntersectionEvent3))
		{
			msg = "GetClosestIntersection failed for rail 3 to first unsorted profile";
			return false;
		}
		Point3d pointA3 = bestIntersectionEvent3.PointA;
		if (!sweep4Profile.Crv.ClosestPoint(pointA3, out var t3))
		{
			msg = "ClosestPoint failed for rail 3 to first unsorted profile";
			return false;
		}
		if (!CSUtilIntersect.GetClosestIntersection(curve4, sweep4Profile.Pln, sweep4Profile.Crv, out var bestIntersectionEvent4))
		{
			msg = "GetClosestIntersection failed for rail 4 to first unsorted profile";
			return false;
		}
		Point3d pointA4 = bestIntersectionEvent4.PointA;
		if (!sweep4Profile.Crv.ClosestPoint(pointA4, out var t4))
		{
			msg = "ClosestPoint failed for rail 4 to first unsorted profile";
			return false;
		}
		List<Curve> list = (from _ in new List<Tuple<Curve, double>>
			{
				new Tuple<Curve, double>(curve, t),
				new Tuple<Curve, double>(curve2, t2),
				new Tuple<Curve, double>(curve3, t3),
				new Tuple<Curve, double>(curve4, t4)
			}
			orderby _.Item2
			select _.Item1).ToList();
		curve = list[0];
		curve2 = list[1];
		curve3 = list[2];
		curve4 = list[3];
		List<Tuple<Sweep4Profile, double>> list2 = new List<Tuple<Sweep4Profile, double>>();
		for (int num2 = 0; num2 < profiles.Count; num2++)
		{
			if (!CSUtilIntersect.GetClosestIntersection(curve, profiles[num2].Pln, profiles[num2].Crv, out var bestIntersectionEvent5))
			{
				msg = $"GetClosestIntersection failed for sorted rail 1 to profile[{num2}]";
				return false;
			}
			Point3d pointA5 = bestIntersectionEvent5.PointA;
			if (!curve.ClosestPoint(pointA5, out var t5))
			{
				msg = $"ClosestPoint failed for sorted rail 1 to profile[{num2}]";
				return false;
			}
			list2.Add(new Tuple<Sweep4Profile, double>(profiles[num2], t5));
		}
		List<Sweep4Profile> list3 = (from _ in list2
			orderby _.Item2
			select _.Item1).ToList();
		for (int num3 = 0; num3 < list3.Count; num3++)
		{
			if (!list3[num3].SetupFromRails(curve, curve2, curve3, curve4))
			{
				msg = $"SetupFromRails failed for profile[{num3}]";
				return false;
			}
		}
		Curve curve5;
		Curve curve6;
		Curve curve7;
		Curve curve8;
		if (doSweepSides)
		{
			curve5 = curve;
			curve6 = curve2;
			curve7 = curve3;
			curve8 = curve4;
		}
		else
		{
			curve5 = curve2;
			curve6 = curve3;
			curve7 = curve4;
			curve8 = curve;
		}
		List<Curve> list4 = new List<Curve>();
		List<Curve> list5 = new List<Curve>();
		List<Curve> list6 = new List<Curve>();
		List<Curve> list7 = new List<Curve>();
		for (int num4 = 0; num4 < list3.Count; num4++)
		{
			if (!list3[num4].Split(out var crvPiece, out var crvPiece2, out var crvPiece3, out var crvPiece4))
			{
				msg = $"Split failed for sortedProfiles[{num4}]";
				return false;
			}
			if (doSweepSides)
			{
				list4.Add(crvPiece);
				list5.Add(crvPiece3);
				list6.Add(crvPiece2);
				list7.Add(crvPiece4);
			}
			else
			{
				list4.Add(crvPiece2);
				list5.Add(crvPiece4);
				list6.Add(crvPiece3);
				list7.Add(crvPiece);
			}
		}
		if (!GetRailParams(curve5, curve6, list4, out var paramsRail, out var paramsRail2))
		{
			msg = "GetRailParams failed for rails Lhs";
			return false;
		}
		if (!GetRailParams(curve7, curve8, list5, out var paramsRail3, out var paramsRail4))
		{
			msg = "GetRailParams failed for rails Rhs";
			return false;
		}
		if (!Sweep2(curve5, curve6, list4, paramsRail, paramsRail2, out Brep[] breps, isClosed, false))
		{
			msg = "Sweep2 failed for Lhs";
			return false;
		}
		Brep brep = ((breps.Length == 1) ? breps[0] : CSUtilRhino.JoinAllBreps(breps, CSUtil.Tolerance));
		if (brep == null)
		{
			msg = "Unable to join sweep 2 results";
			return false;
		}
		if (!Sweep2(curve7, curve8, list5, paramsRail3, paramsRail4, out Brep[] breps2, isClosed, false))
		{
			msg = "Sweep2 failed for Rhs";
			return false;
		}
		Brep brep2 = ((breps2.Length == 1) ? breps2[0] : CSUtilRhino.JoinAllBreps(breps2, CSUtil.Tolerance));
		if (brep2 == null)
		{
			msg = "Unable to join sweep 2 results";
			return false;
		}
		Point3d pointAtStart = list4.First().PointAtStart;
		Point3d pointAtStart2 = list4.Last().PointAtStart;
		Point3d pointAtEnd = list4.First().PointAtEnd;
		Curve curveFromBrepEdges = CSUtilBrepCrvs.GetCurveFromBrepEdges(brep, pointAtStart, pointAtStart2, pointAtEnd, isThirdPointMid: false);
		if (curveFromBrepEdges == null)
		{
			msg = "Unable to find sweep 2 edge for Lhs rail 1";
			return false;
		}
		Point3d pointAtEnd2 = list4.First().PointAtEnd;
		Point3d pointAtEnd3 = list4.Last().PointAtEnd;
		Point3d pointAtStart3 = list4.First().PointAtStart;
		Curve curveFromBrepEdges2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(brep, pointAtEnd2, pointAtEnd3, pointAtStart3, isThirdPointMid: false);
		if (curveFromBrepEdges2 == null)
		{
			msg = "Unable to find sweep 2 edge for Lhs rail 2";
			return false;
		}
		Point3d pointAtStart4 = list5.First().PointAtStart;
		Point3d pointAtStart5 = list5.Last().PointAtStart;
		Point3d pointAtEnd4 = list5.First().PointAtEnd;
		Curve curveFromBrepEdges3 = CSUtilBrepCrvs.GetCurveFromBrepEdges(brep2, pointAtStart4, pointAtStart5, pointAtEnd4, isThirdPointMid: false);
		if (curveFromBrepEdges3 == null)
		{
			msg = "Unable to find sweep 2 edge for Rhs rail 1";
			return false;
		}
		Point3d pointAtEnd5 = list5.First().PointAtEnd;
		Point3d pointAtEnd6 = list5.Last().PointAtEnd;
		Point3d pointAtStart6 = list5.First().PointAtStart;
		Curve curveFromBrepEdges4 = CSUtilBrepCrvs.GetCurveFromBrepEdges(brep2, pointAtEnd5, pointAtEnd6, pointAtStart6, isThirdPointMid: false);
		if (curveFromBrepEdges4 == null)
		{
			msg = "Unable to find sweep 2 edge for Rhs rail 2";
			return false;
		}
		Curve curve9 = curveFromBrepEdges2;
		Curve curve10 = curveFromBrepEdges3;
		Curve curve11 = curveFromBrepEdges4;
		Curve curve12 = curveFromBrepEdges;
		if (!GetRailParams(curve9, curve10, list6, out var paramsRail5, out var paramsRail6))
		{
			msg = "GetRailParams failed for rails Bottom";
			return false;
		}
		if (!GetRailParams(curve11, curve12, list7, out var paramsRail7, out var paramsRail8))
		{
			msg = "GetRailParams failed for rails Top";
			return false;
		}
		if (!Sweep2(curve9, curve10, list6, paramsRail5, paramsRail6, out Brep[] breps3, isClosed, false))
		{
			msg = "Sweep2 failed for Bot";
			return false;
		}
		Brep brep3 = ((breps3.Length == 1) ? breps3[0] : CSUtilRhino.JoinAllBreps(breps3, CSUtil.Tolerance));
		if (brep3 == null)
		{
			msg = "Unable to join sweep 2 results";
			return false;
		}
		if (!Sweep2(curve11, curve12, list7, paramsRail7, paramsRail8, out Brep[] breps4, isClosed, false))
		{
			msg = "Sweep2 failed for Top";
			return false;
		}
		Brep brep4 = ((breps4.Length == 1) ? breps4[0] : CSUtilRhino.JoinAllBreps(breps4, CSUtil.Tolerance));
		if (brep4 == null)
		{
			msg = "Unable to join sweep 2 results";
			return false;
		}
		Brep brep5 = CSUtilRhino.JoinAllBreps(new Brep[4] { brep, brep3, brep2, brep4 }, CSUtil.Tolerance);
		if (brep5 == null)
		{
			msg = "Unable to join sweep pieces";
			return false;
		}
		if (!brep5.IsValid)
		{
			msg = "Joined sweep is invalid";
			return false;
		}
		sweep = brep5;
		return true;
	}

	private static double CalculateMatchDirectionValue(Point3dList p1, Point3dList p2, int offset1, int offset2, bool reverse)
	{
		if (p1.Count != p2.Count)
		{
			return 0.0;
		}
		int count = p1.Count;
		double num = 0.0;
		if (reverse)
		{
			return num + p1.Select((Point3d t, int i) => p1[(i + offset1) % count].DistanceTo(p2[(count - i - 1 + offset2) % count])).Sum();
		}
		return num + p1.Select((Point3d t, int i) => p1[(i + offset1) % count].DistanceTo(p2[(i + offset2) % count])).Sum();
	}

	private static bool GetRailParams(Curve rail1, Curve rail2, IEnumerable<Curve> profiles, out List<double> paramsRail1, out List<double> paramsRail2)
	{
		paramsRail1 = new List<double>();
		paramsRail2 = new List<double>();
		foreach (Curve profile in profiles)
		{
			Point3d pointAtStart = profile.PointAtStart;
			Point3d pointAtEnd = profile.PointAtEnd;
			if (!rail1.ClosestPoint(pointAtStart, out var t))
			{
				return false;
			}
			paramsRail1.Add(t);
			if (!rail2.ClosestPoint(pointAtEnd, out var t2))
			{
				return false;
			}
			paramsRail2.Add(t2);
		}
		return paramsRail1.Count == paramsRail2.Count;
	}
}
