using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Runtime;

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
				if (Crv != null && ((CommonObject)Crv).IsValid)
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
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			Crv = crv.DuplicateCurve();
			Pln = Plane.Unset;
			Plane val = default(Plane);
			if (Crv.IsPlanar() && Crv.TryGetPlane(ref val))
			{
				Pln = val;
			}
			CrvParam1 = crvParam1;
			CrvParam2 = crvParam2;
			CrvParam3 = crvParam3;
			CrvParam4 = crvParam4;
		}

		public bool MatchCurveDirection(Curve refCrv, int pointCount = 20)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			if (Crv == null)
			{
				return false;
			}
			bool flag = false;
			Point3d val = Point3d.Unset;
			Point3d val2 = Point3d.Unset;
			Point3d val3 = Point3d.Unset;
			Point3d val4 = Point3d.Unset;
			if (IsSetup)
			{
				flag = true;
				val = Crv.PointAt(CrvParam1);
				val2 = Crv.PointAt(CrvParam2);
				val3 = Crv.PointAt(CrvParam3);
				val4 = Crv.PointAt(CrvParam4);
				ResetParams();
				if (!((Point3d)(ref val)).IsValid || !((Point3d)(ref val2)).IsValid || !((Point3d)(ref val3)).IsValid || !((Point3d)(ref val4)).IsValid)
				{
					return false;
				}
			}
			CSUtilSweepRC.MatchCurveDirection(refCrv, Crv, pointCount);
			if (!flag)
			{
				return true;
			}
			double crvParam = default(double);
			if (!Crv.ClosestPoint(val, ref crvParam))
			{
				return false;
			}
			double crvParam2 = default(double);
			if (!Crv.ClosestPoint(val2, ref crvParam2))
			{
				return false;
			}
			double crvParam3 = default(double);
			if (!Crv.ClosestPoint(val3, ref crvParam3))
			{
				return false;
			}
			double crvParam4 = default(double);
			if (!Crv.ClosestPoint(val4, ref crvParam4))
			{
				return false;
			}
			CrvParam1 = crvParam;
			CrvParam2 = crvParam2;
			CrvParam3 = crvParam3;
			CrvParam4 = crvParam4;
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
			Curve[] array = crv.Split((IEnumerable<double>)crvParams);
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
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			crvParam = double.NaN;
			if (rail == null)
			{
				return false;
			}
			if (!((CommonObject)rail).IsValid)
			{
				return false;
			}
			if (Crv == null)
			{
				return false;
			}
			if (!((CommonObject)Crv).IsValid)
			{
				return false;
			}
			Plane pln = Pln;
			Point3d pointA = default(Point3d);
			Point3d val = default(Point3d);
			if (((Plane)(ref pln)).IsValid)
			{
				if (!CSUtilIntersect.GetClosestIntersection(rail, Pln, Crv, out var bestIntersectionEvent))
				{
					return false;
				}
				pointA = bestIntersectionEvent.PointA;
			}
			else if (!Crv.ClosestPoints(rail, ref val, ref pointA))
			{
				return false;
			}
			double num = default(double);
			if (!Crv.ClosestPoint(pointA, ref num))
			{
				return false;
			}
			crvParam = num;
			return true;
		}
	}

	public static Brep Loft(IEnumerable<Curve> profiles, LoftType loftType, bool doCapped = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Brep[] array = Brep.CreateFromLoft(profiles, Point3d.Unset, Point3d.Unset, loftType, false);
		Brep val = ((array.Length == 1) ? array[0].DuplicateBrep() : CSUtilRhino.JoinAllBreps(array, CSUtil.Tolerance));
		if (val != null && doCapped)
		{
			val = CSUtilRhino.CapPlanarHoles(val, CSUtil.Tolerance);
		}
		return val;
	}

	public static bool MatchCurveDirection(Curve curve1, Curve curve2, int pointCount)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		double[] array = new double[pointCount];
		double[] array2 = new double[pointCount];
		for (int i = 0; i < pointCount; i++)
		{
			array[i] = (double)i / (double)(curve1.IsClosed ? pointCount : (pointCount - 1));
			array2[i] = (double)i / (double)(curve2.IsClosed ? pointCount : (pointCount - 1));
		}
		array = curve1.NormalizedLengthParameters(array, CSUtil.Tolerance);
		array2 = curve2.NormalizedLengthParameters(array2, CSUtil.Tolerance);
		Point3dList val = new Point3dList();
		Point3dList val2 = new Point3dList();
		for (int j = 0; j < array.Length; j++)
		{
			((RhinoList<Point3d>)(object)val).Add(curve1.PointAt(array[j]));
			((RhinoList<Point3d>)(object)val2).Add(curve2.PointAt(array2[j]));
		}
		double num = 0.0;
		double num2 = 0.0;
		int num3 = 0;
		int num4 = 0;
		double num5 = double.MaxValue;
		Point3d val3;
		if (curve1.IsClosed || curve2.IsClosed)
		{
			for (int k = 0; k < ((RhinoList<Point3d>)(object)val2).Count; k++)
			{
				val3 = ((RhinoList<Point3d>)(object)val)[0];
				if (((Point3d)(ref val3)).DistanceTo(((RhinoList<Point3d>)(object)val2)[k]) < num5)
				{
					val3 = ((RhinoList<Point3d>)(object)val)[0];
					num5 = ((Point3d)(ref val3)).DistanceTo(((RhinoList<Point3d>)(object)val2)[k]);
					num4 = k;
				}
			}
		}
		else
		{
			num4 = 0;
		}
		for (int l = 0; l < ((RhinoList<Point3d>)(object)val).Count; l++)
		{
			double num6 = num;
			val3 = ((RhinoList<Point3d>)(object)val)[(l + num3) % pointCount];
			num = num6 + ((Point3d)(ref val3)).DistanceTo(((RhinoList<Point3d>)(object)val2)[(l + num4) % pointCount]);
			double num7 = num2;
			val3 = ((RhinoList<Point3d>)(object)val)[(l + num3) % pointCount];
			num2 = num7 + ((Point3d)(ref val3)).DistanceTo(((RhinoList<Point3d>)(object)val2)[(pointCount - l - 1 + num4) % pointCount]);
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
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		List<double> list = new List<double>();
		for (int i = 0; i < pointCount; i++)
		{
			list.Add((double)i / (double)pointCount);
		}
		double[] array = curve1.NormalizedLengthParameters(list.ToArray(), CSUtil.Tolerance);
		double[] array2 = curve2.NormalizedLengthParameters(list.ToArray(), CSUtil.Tolerance);
		Point3dList val = new Point3dList();
		Point3dList val2 = new Point3dList();
		for (int j = 0; j < array.Length; j++)
		{
			((RhinoList<Point3d>)(object)val).Add(curve1.PointAt(array[j]));
			((RhinoList<Point3d>)(object)val2).Add(curve2.PointAt(array2[j]));
		}
		double num = double.MaxValue;
		int num2 = 0;
		bool flag = false;
		for (int k = 0; k < 2; k++)
		{
			bool flag2 = k == 1;
			for (int l = 0; l < pointCount; l++)
			{
				double num3 = CalculateMatchDirectionValue(val, val2, 0, l, flag2);
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
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
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
			foreach (double num in parameters)
			{
				list.Add(railCurve.PointAt(num));
			}
			railCurve.ChangeClosedCurveSeam(parameters[0]);
			Interval domain = railCurve.Domain;
			parameters[0] = ((Interval)(ref domain)).Min;
			for (int j = 1; j < list.Count; j++)
			{
				railCurve.ClosestPoint(list[j], ref parameters[j]);
			}
		}
		SweepOneRail val = new SweepOneRail
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
						int num2 = array[m];
						array[m] = array[m + 1];
						array[m + 1] = num2;
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
		breps = val.PerformSweep(railCurve, (IEnumerable<Curve>)list2, (IEnumerable<double>)list3);
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
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Expected O, but got Unknown
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
		Curve val = railCurve1.DuplicateCurve();
		Curve val2 = railCurve2.DuplicateCurve();
		double[] array = new double[parameters1In.Count];
		double[] array2 = new double[parameters2In.Count];
		Interval domain;
		if (val.IsClosed && val2.IsClosed)
		{
			val.ChangeClosedCurveSeam(parameters1In[0]);
			val2.ChangeClosedCurveSeam(parameters2In[0]);
			domain = val.Domain;
			array[0] = ((Interval)(ref domain)).Min;
			domain = val2.Domain;
			array2[0] = ((Interval)(ref domain)).Min;
			if (profiles.Count > 1)
			{
				double num = default(double);
				for (int i = 1; i < profiles.Count; i++)
				{
					val.ClosestPoint(railCurve1.PointAt(parameters1In[i]), ref num);
					array[i] = num;
					val2.ClosestPoint(railCurve2.PointAt(parameters2In[i]), ref num);
					array2[i] = num;
				}
			}
		}
		else
		{
			for (int j = 0; j < profiles.Count; j++)
			{
				array[j] = parameters1In[j];
				array2[j] = parameters2In[j];
				double num2 = array[j];
				domain = val.Domain;
				if (num2 < ((Interval)(ref domain)).Min)
				{
					int num3 = j;
					domain = val.Domain;
					array[num3] = ((Interval)(ref domain)).Min;
				}
				double num4 = array[j];
				domain = val.Domain;
				if (num4 > ((Interval)(ref domain)).Max)
				{
					int num5 = j;
					domain = val.Domain;
					array[num5] = ((Interval)(ref domain)).Max;
				}
				double num6 = array2[j];
				domain = val2.Domain;
				if (num6 < ((Interval)(ref domain)).Min)
				{
					int num7 = j;
					domain = val2.Domain;
					array2[num7] = ((Interval)(ref domain)).Min;
				}
				double num8 = array2[j];
				domain = val2.Domain;
				if (num8 > ((Interval)(ref domain)).Max)
				{
					int num9 = j;
					domain = val2.Domain;
					array2[num9] = ((Interval)(ref domain)).Max;
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
						int num10 = array3[m];
						array3[m] = array3[m + 1];
						array3[m + 1] = num10;
					}
				}
			}
		}
		List<Curve> list = new List<Curve>();
		List<double> list2 = new List<double>();
		List<double> list3 = new List<double>();
		for (int n = 0; n < profiles.Count; n++)
		{
			Curve val3 = profiles[array3[n]].DuplicateCurve();
			if (val3 != null)
			{
				list2.Add(array[array3[n]]);
				list3.Add(array2[array3[n]]);
				list.Add(val3);
			}
			else
			{
				CSUtil.PrintLine("Unable to duplicate curve");
			}
		}
		SweepTwoRail val4 = new SweepTwoRail
		{
			SweepTolerance = CSUtil.Tolerance,
			AngleToleranceRadians = RhinoDoc.ActiveDoc.ModelAngleToleranceRadians,
			MaintainHeight = maintainHeight,
			ClosedSweep = (railCurve1.IsClosed && railCurve2.IsClosed && closed)
		};
		breps = val4.PerformSweep(val, val2, (IEnumerable<Curve>)list, (IEnumerable<double>)list2, (IEnumerable<double>)list3);
		return true;
	}

	public static bool Sweep4NoParam(Curve rail1, Curve rail2, Curve rail3, Curve rail4, List<Sweep4Profile> profiles, bool isClosed, bool doSweepSides, bool doTryMatchCurveDirectionRails, bool doTryMatchCurveDirectionProfiles, out Brep sweep, out string msg)
	{
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
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
		profiles = profiles.Where((Sweep4Profile _) => _.Crv != null && ((CommonObject)_.Crv).IsValid && _.Crv.IsPlanar()).ToList();
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
		Curve val = rail1.DuplicateCurve();
		Curve val2 = rail2.DuplicateCurve();
		Curve val3 = rail3.DuplicateCurve();
		Curve val4 = rail4.DuplicateCurve();
		if (doTryMatchCurveDirectionRails)
		{
			MatchCurveDirection(val, val2, 20);
			MatchCurveDirection(val, val3, 20);
			MatchCurveDirection(val, val4, 20);
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
		if (!CSUtilIntersect.GetClosestIntersection(val, sweep4Profile.Pln, sweep4Profile.Crv, out var bestIntersectionEvent))
		{
			msg = "GetClosestIntersection failed for rail 1 to first unsorted profile";
			return false;
		}
		Point3d pointA = bestIntersectionEvent.PointA;
		double item = default(double);
		if (!sweep4Profile.Crv.ClosestPoint(pointA, ref item))
		{
			msg = "ClosestPoint failed for rail 1 to first unsorted profile";
			return false;
		}
		if (!CSUtilIntersect.GetClosestIntersection(val2, sweep4Profile.Pln, sweep4Profile.Crv, out var bestIntersectionEvent2))
		{
			msg = "GetClosestIntersection failed for rail 2 to first unsorted profile";
			return false;
		}
		Point3d pointA2 = bestIntersectionEvent2.PointA;
		double item2 = default(double);
		if (!sweep4Profile.Crv.ClosestPoint(pointA2, ref item2))
		{
			msg = "ClosestPoint failed for rail 2 to first unsorted profile";
			return false;
		}
		if (!CSUtilIntersect.GetClosestIntersection(val3, sweep4Profile.Pln, sweep4Profile.Crv, out var bestIntersectionEvent3))
		{
			msg = "GetClosestIntersection failed for rail 3 to first unsorted profile";
			return false;
		}
		Point3d pointA3 = bestIntersectionEvent3.PointA;
		double item3 = default(double);
		if (!sweep4Profile.Crv.ClosestPoint(pointA3, ref item3))
		{
			msg = "ClosestPoint failed for rail 3 to first unsorted profile";
			return false;
		}
		if (!CSUtilIntersect.GetClosestIntersection(val4, sweep4Profile.Pln, sweep4Profile.Crv, out var bestIntersectionEvent4))
		{
			msg = "GetClosestIntersection failed for rail 4 to first unsorted profile";
			return false;
		}
		Point3d pointA4 = bestIntersectionEvent4.PointA;
		double item4 = default(double);
		if (!sweep4Profile.Crv.ClosestPoint(pointA4, ref item4))
		{
			msg = "ClosestPoint failed for rail 4 to first unsorted profile";
			return false;
		}
		List<Curve> list = (from _ in new List<Tuple<Curve, double>>
			{
				new Tuple<Curve, double>(val, item),
				new Tuple<Curve, double>(val2, item2),
				new Tuple<Curve, double>(val3, item3),
				new Tuple<Curve, double>(val4, item4)
			}
			orderby _.Item2
			select _.Item1).ToList();
		val = list[0];
		val2 = list[1];
		val3 = list[2];
		val4 = list[3];
		List<Tuple<Sweep4Profile, double>> list2 = new List<Tuple<Sweep4Profile, double>>();
		double item5 = default(double);
		for (int num2 = 0; num2 < profiles.Count; num2++)
		{
			if (!CSUtilIntersect.GetClosestIntersection(val, profiles[num2].Pln, profiles[num2].Crv, out var bestIntersectionEvent5))
			{
				msg = $"GetClosestIntersection failed for sorted rail 1 to profile[{num2}]";
				return false;
			}
			Point3d pointA5 = bestIntersectionEvent5.PointA;
			if (!val.ClosestPoint(pointA5, ref item5))
			{
				msg = $"ClosestPoint failed for sorted rail 1 to profile[{num2}]";
				return false;
			}
			list2.Add(new Tuple<Sweep4Profile, double>(profiles[num2], item5));
		}
		List<Sweep4Profile> list3 = (from _ in list2
			orderby _.Item2
			select _.Item1).ToList();
		for (int num3 = 0; num3 < list3.Count; num3++)
		{
			if (!list3[num3].SetupFromRails(val, val2, val3, val4))
			{
				msg = $"SetupFromRails failed for profile[{num3}]";
				return false;
			}
		}
		Curve val5;
		Curve val6;
		Curve val7;
		Curve val8;
		if (doSweepSides)
		{
			val5 = val;
			val6 = val2;
			val7 = val3;
			val8 = val4;
		}
		else
		{
			val5 = val2;
			val6 = val3;
			val7 = val4;
			val8 = val;
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
		if (!GetRailParams(val5, val6, list4, out var paramsRail, out var paramsRail2))
		{
			msg = "GetRailParams failed for rails Lhs";
			return false;
		}
		if (!GetRailParams(val7, val8, list5, out var paramsRail3, out var paramsRail4))
		{
			msg = "GetRailParams failed for rails Rhs";
			return false;
		}
		if (!Sweep2(val5, val6, list4, paramsRail, paramsRail2, out Brep[] breps, isClosed, maintainHeight: false))
		{
			msg = "Sweep2 failed for Lhs";
			return false;
		}
		Brep val9 = ((breps.Length == 1) ? breps[0] : CSUtilRhino.JoinAllBreps(breps, CSUtil.Tolerance));
		if (val9 == null)
		{
			msg = "Unable to join sweep 2 results";
			return false;
		}
		if (!Sweep2(val7, val8, list5, paramsRail3, paramsRail4, out Brep[] breps2, isClosed, maintainHeight: false))
		{
			msg = "Sweep2 failed for Rhs";
			return false;
		}
		Brep val10 = ((breps2.Length == 1) ? breps2[0] : CSUtilRhino.JoinAllBreps(breps2, CSUtil.Tolerance));
		if (val10 == null)
		{
			msg = "Unable to join sweep 2 results";
			return false;
		}
		Point3d pointAtStart = list4.First().PointAtStart;
		Point3d pointAtStart2 = list4.Last().PointAtStart;
		Point3d pointAtEnd = list4.First().PointAtEnd;
		Curve curveFromBrepEdges = CSUtilBrepCrvs.GetCurveFromBrepEdges(val9, pointAtStart, pointAtStart2, pointAtEnd, isThirdPointMid: false);
		if (curveFromBrepEdges == null)
		{
			msg = "Unable to find sweep 2 edge for Lhs rail 1";
			return false;
		}
		Point3d pointAtEnd2 = list4.First().PointAtEnd;
		Point3d pointAtEnd3 = list4.Last().PointAtEnd;
		Point3d pointAtStart3 = list4.First().PointAtStart;
		Curve curveFromBrepEdges2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(val9, pointAtEnd2, pointAtEnd3, pointAtStart3, isThirdPointMid: false);
		if (curveFromBrepEdges2 == null)
		{
			msg = "Unable to find sweep 2 edge for Lhs rail 2";
			return false;
		}
		Point3d pointAtStart4 = list5.First().PointAtStart;
		Point3d pointAtStart5 = list5.Last().PointAtStart;
		Point3d pointAtEnd4 = list5.First().PointAtEnd;
		Curve curveFromBrepEdges3 = CSUtilBrepCrvs.GetCurveFromBrepEdges(val10, pointAtStart4, pointAtStart5, pointAtEnd4, isThirdPointMid: false);
		if (curveFromBrepEdges3 == null)
		{
			msg = "Unable to find sweep 2 edge for Rhs rail 1";
			return false;
		}
		Point3d pointAtEnd5 = list5.First().PointAtEnd;
		Point3d pointAtEnd6 = list5.Last().PointAtEnd;
		Point3d pointAtStart6 = list5.First().PointAtStart;
		Curve curveFromBrepEdges4 = CSUtilBrepCrvs.GetCurveFromBrepEdges(val10, pointAtEnd5, pointAtEnd6, pointAtStart6, isThirdPointMid: false);
		if (curveFromBrepEdges4 == null)
		{
			msg = "Unable to find sweep 2 edge for Rhs rail 2";
			return false;
		}
		Curve val11 = curveFromBrepEdges2;
		Curve val12 = curveFromBrepEdges3;
		Curve val13 = curveFromBrepEdges4;
		Curve val14 = curveFromBrepEdges;
		if (!GetRailParams(val11, val12, list6, out var paramsRail5, out var paramsRail6))
		{
			msg = "GetRailParams failed for rails Bottom";
			return false;
		}
		if (!GetRailParams(val13, val14, list7, out var paramsRail7, out var paramsRail8))
		{
			msg = "GetRailParams failed for rails Top";
			return false;
		}
		if (!Sweep2(val11, val12, list6, paramsRail5, paramsRail6, out Brep[] breps3, isClosed, maintainHeight: false))
		{
			msg = "Sweep2 failed for Bot";
			return false;
		}
		Brep val15 = ((breps3.Length == 1) ? breps3[0] : CSUtilRhino.JoinAllBreps(breps3, CSUtil.Tolerance));
		if (val15 == null)
		{
			msg = "Unable to join sweep 2 results";
			return false;
		}
		if (!Sweep2(val13, val14, list7, paramsRail7, paramsRail8, out Brep[] breps4, isClosed, maintainHeight: false))
		{
			msg = "Sweep2 failed for Top";
			return false;
		}
		Brep val16 = ((breps4.Length == 1) ? breps4[0] : CSUtilRhino.JoinAllBreps(breps4, CSUtil.Tolerance));
		if (val16 == null)
		{
			msg = "Unable to join sweep 2 results";
			return false;
		}
		Brep val17 = CSUtilRhino.JoinAllBreps((IEnumerable<Brep>)(object)new Brep[4] { val9, val15, val10, val16 }, CSUtil.Tolerance);
		if (val17 == null)
		{
			msg = "Unable to join sweep pieces";
			return false;
		}
		if (!((CommonObject)val17).IsValid)
		{
			msg = "Joined sweep is invalid";
			return false;
		}
		sweep = val17;
		return true;
	}

	private static double CalculateMatchDirectionValue(Point3dList p1, Point3dList p2, int offset1, int offset2, bool reverse)
	{
		if (((RhinoList<Point3d>)(object)p1).Count != ((RhinoList<Point3d>)(object)p2).Count)
		{
			return 0.0;
		}
		int count = ((RhinoList<Point3d>)(object)p1).Count;
		double num = 0.0;
		if (reverse)
		{
			return num + ((IEnumerable<Point3d>)p1).Select(delegate(Point3d t, int i)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				Point3d val = ((RhinoList<Point3d>)(object)p1)[(i + offset1) % count];
				return ((Point3d)(ref val)).DistanceTo(((RhinoList<Point3d>)(object)p2)[(count - i - 1 + offset2) % count]);
			}).Sum();
		}
		return num + ((IEnumerable<Point3d>)p1).Select(delegate(Point3d t, int i)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			Point3d val = ((RhinoList<Point3d>)(object)p1)[(i + offset1) % count];
			return ((Point3d)(ref val)).DistanceTo(((RhinoList<Point3d>)(object)p2)[(i + offset2) % count]);
		}).Sum();
	}

	private static bool GetRailParams(Curve rail1, Curve rail2, IEnumerable<Curve> profiles, out List<double> paramsRail1, out List<double> paramsRail2)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		paramsRail1 = new List<double>();
		paramsRail2 = new List<double>();
		double item = default(double);
		double item2 = default(double);
		foreach (Curve profile in profiles)
		{
			Point3d pointAtStart = profile.PointAtStart;
			Point3d pointAtEnd = profile.PointAtEnd;
			if (!rail1.ClosestPoint(pointAtStart, ref item))
			{
				return false;
			}
			paramsRail1.Add(item);
			if (!rail2.ClosestPoint(pointAtEnd, ref item2))
			{
				return false;
			}
			paramsRail2.Add(item2);
		}
		return paramsRail1.Count == paramsRail2.Count;
	}
}
