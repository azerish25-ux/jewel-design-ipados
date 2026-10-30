using CSBlockUtil;
using RMA.OpenNURBS;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

public static class HelperMakeProngProfileStraightObsolete
{
	public static bool MakeProngProfileStraightObsolete(OnNurbsCurve headProfile, double param, double vProngAngle, double prongY, double prongNudge, out OnCurve vProng)
	{
		vProng = null;
		if (headProfile == null)
		{
			return false;
		}
		On3dPoint on3dPoint = headProfile.PointAt(param);
		OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, 0.0, on3dPoint.z), new On3dPoint(on3dPoint.x * 2.0, on3dPoint.y * 2.0, on3dPoint.z));
		OnLineCurve onLineCurve2 = new OnLineCurve(new On3dPoint(0.0, 0.0, on3dPoint.z), new On3dPoint(on3dPoint.x * 2.0, on3dPoint.y * 2.0, on3dPoint.z));
		OnXform onXform = new OnXform();
		onXform.Rotation(CSUtilMath.ToRadians(vProngAngle), OnUtil.On_zaxis, OnUtil.On_origin);
		onLineCurve.Transform(onXform);
		onXform.Rotation(CSUtilMath.ToRadians(0.0 - vProngAngle), OnUtil.On_zaxis, OnUtil.On_origin);
		onLineCurve2.Transform(onXform);
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		headProfile.IntersectCurve(onLineCurve, ref x);
		if (x.Count() != 1)
		{
			return false;
		}
		double num = x[0].get_m_a(0);
		x = new ArrayOnX_EVENT();
		headProfile.IntersectCurve(onLineCurve2, ref x);
		if (x.Count() != 1)
		{
			return false;
		}
		double num2 = x[0].get_m_a(0);
		OnNurbsCurve nurbs_curve;
		if (num < num2)
		{
			nurbs_curve = headProfile.Duplicate();
			nurbs_curve.Trim(new OnInterval(num, num2));
		}
		else
		{
			OnNurbsCurve onNurbsCurve = headProfile.Duplicate();
			onNurbsCurve.Trim(new OnInterval(num, headProfile.Domain().Max()));
			OnNurbsCurve onNurbsCurve2 = headProfile.Duplicate();
			onNurbsCurve2.Trim(new OnInterval(headProfile.Domain().Min(), num2));
			if (!CSUtilCurves.MergeCurves(new IOnCurve[2] { onNurbsCurve, onNurbsCurve2 }, out OnCurve mergedCrv, 0.01))
			{
				return false;
			}
			nurbs_curve = new OnNurbsCurve();
			mergedCrv.GetNurbForm(ref nurbs_curve);
		}
		OnCurve onCurve = CSUtilGeometry.OffsetOld(nurbs_curve, OnUtil.On_zaxis, new On3dPoint(0.0, 0.0, on3dPoint.z), prongNudge, closed: false);
		OnCurve onCurve2 = CSUtilGeometry.OffsetOld(nurbs_curve, OnUtil.On_zaxis, new On3dPoint(0.0, 0.0, on3dPoint.z), prongY + prongNudge, closed: false);
		if (onCurve == null || onCurve2 == null)
		{
			return false;
		}
		double t = 0.0;
		onCurve2.GetNormalizedArcLengthPoint(0.5, ref t);
		On3dPoint on3dPoint2 = onCurve2.PointAt(t);
		On3dPoint on3dPoint3 = onCurve.PointAtStart();
		On3dPoint on3dPoint4 = onCurve2.PointAtStart();
		On3dPoint on3dPoint5 = (on3dPoint3 + on3dPoint4) / 2.0;
		On3dVector on3dVector = OnUtil.ON_CrossProduct(on3dPoint3 - on3dPoint5, OnUtil.On_zaxis);
		on3dVector.Unitize();
		on3dVector *= 0.01;
		OnArc onArc = new OnArc(on3dPoint3, on3dVector + on3dPoint5, on3dPoint4);
		On3dPoint on3dPoint6 = onCurve.PointAtEnd();
		On3dPoint on3dPoint7 = onCurve2.PointAtEnd();
		On3dPoint on3dPoint8 = (on3dPoint6 + on3dPoint7) / 2.0;
		On3dVector on3dVector2 = OnUtil.ON_CrossProduct(on3dPoint6 - on3dPoint8, OnUtil.On_zaxis);
		on3dVector2.Unitize();
		on3dVector2 *= -0.01;
		OnArc onArc2 = new OnArc(on3dPoint6, on3dVector2 + on3dPoint8, on3dPoint7);
		OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
		OnNurbsCurve nurbs_curve3 = new OnNurbsCurve();
		onArc.GetNurbForm(ref nurbs_curve2);
		onArc2.GetNurbForm(ref nurbs_curve3);
		if (!CSUtilCurves.MergeCurves(new IOnCurve[4] { onCurve, nurbs_curve2, onCurve2, nurbs_curve3 }, out OnCurve mergedCrv2, 0.01))
		{
			return false;
		}
		double t2 = 0.0;
		mergedCrv2.GetClosestPoint(on3dPoint2, ref t2);
		mergedCrv2.ChangeClosedCurveSeam(t2);
		double t3 = mergedCrv2.Domain().Min();
		On3dVector arg = mergedCrv2.TangentAt(t3);
		On3dVector arg2 = new On3dVector(on3dPoint2.x, on3dPoint2.y, 0.0);
		if (((On3dPoint)OnUtil.ON_CrossProduct(arg, arg2)).z < 0.0)
		{
			mergedCrv2.Reverse();
		}
		vProng = mergedCrv2;
		return true;
	}
}
