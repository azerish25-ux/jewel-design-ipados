using System.Collections.Generic;
using CSCompatibility;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlockUtil;

public static class RhUtilHelper
{
	private static readonly object ParallelLockRhUtil = new object();

	public static void RhinoConvertCurveToPolyline(OnNurbsCurve curve, int i, int i1, double d, int i2, double d1, double tolerance, double minEdgeLength, double maxEdgeLength, out OnPolylineCurve polylineCurve)
	{
		lock (ParallelLockRhUtil)
		{
			RhUtil.RhinoConvertCurveToPolyline((IOnCurve)(object)curve, i, i1, d, (double)i2, d1, tolerance, minEdgeLength, maxEdgeLength, ref polylineCurve);
		}
	}

	public static OnSurface RhinoCreateSurfaceFromCorners(IOn3dPoint c1, IOn3dPoint c2, ref On3dPoint c3, ref On3dPoint c4)
	{
		lock (ParallelLockRhUtil)
		{
			return (OnSurface)(object)RhUtil.RhinoCreateSurfaceFromCorners(c1, c2, ref c3, ref c4);
		}
	}

	public static OnCurve RhinoInterpCurve(int i, ArrayOn3dPoint endSegmentPoints, On3dVector o, On3dVector o1, int i1)
	{
		lock (ParallelLockRhUtil)
		{
			return (OnCurve)(object)RhUtil.RhinoInterpCurve(i, (IArrayOn3dPoint)(object)endSegmentPoints, (IOn3dVector)(object)o, (IOn3dVector)(object)o1, i1);
		}
	}

	public static bool RhinoIntersectSurfaces(IOnSurface surfaceLhs, IOnSurface surfaceRhs, double tolerance, out OnCurve[] intersectionCurves, out On3dPointArray intersectionPoints)
	{
		lock (ParallelLockRhUtil)
		{
			return RhUtil.RhinoIntersectSurfaces(surfaceLhs, surfaceRhs, tolerance, ref intersectionCurves, ref intersectionPoints);
		}
	}

	public static bool RhinoJoinCurves(IOnCurve[] offsetRetC, ref OnCurve[] joinRet, double tolerance)
	{
		lock (ParallelLockRhUtil)
		{
			return RhUtil.RhinoMergeCurves(offsetRetC, ref joinRet, tolerance);
		}
	}

	public static Curve[] RhinoJoinCurves(IEnumerable<Curve> offsetRetC, double tolerance)
	{
		lock (ParallelLockRhUtil)
		{
			return Curve.JoinCurves(offsetRetC, tolerance);
		}
	}

	public static void RhinoMakeCurveEndsMeet(OnCurve onCurve, int i, OnCurve onCurve1, int i1)
	{
		lock (ParallelLockRhUtil)
		{
			RhUtil.RhinoMakeCurveEndsMeet(onCurve, i, onCurve1, i1);
		}
	}

	public static bool RhinoMakeCurveEndsMeet(Curve curveA, bool doAdjustCurveA, Curve curveB, bool doAdjustCurveB)
	{
		lock (ParallelLockRhUtil)
		{
			return Curve.MakeEndsMeet(curveA, doAdjustCurveA, curveB, doAdjustCurveB);
		}
	}

	public static bool RhinoOffsetCurve(IOnCurve crv, IOn3dVector normalDir, IOn3dPoint origin, double distance, double tolerance, double angleTolerance, int cornerStyle, out OnCurve[] offsetCurves)
	{
		lock (ParallelLockRhUtil)
		{
			return RhUtil.RhinoOffsetCurve(crv, normalDir, origin, distance, ref offsetCurves, tolerance, angleTolerance, cornerStyle);
		}
	}

	public static bool RhinoOffsetCurve(Curve crv, Vector3d normalDir, Point3d origin, double distance, double tolerance, double angleTolerance, CurveOffsetCornerStyle cornerStyle, out Curve[] offsetCurves)
	{
		offsetCurves = new Curve[0];
		lock (ParallelLockRhUtil)
		{
			OnCurve crv2 = crv.Convert();
			On3dVector normalDir2 = normalDir.Convert();
			On3dPoint origin2 = origin.Convert();
			if (!RhinoOffsetCurve((IOnCurve)(object)crv2, (IOn3dVector)(object)normalDir2, (IOn3dPoint)(object)origin2, distance, tolerance, angleTolerance, (int)cornerStyle, out var offsetCurves2))
			{
				return false;
			}
			offsetCurves = new Curve[offsetCurves2.Length];
			for (int i = 0; i < offsetCurves2.Length; i++)
			{
				offsetCurves[i] = offsetCurves2[i].Convert();
			}
			return true;
		}
	}
}
