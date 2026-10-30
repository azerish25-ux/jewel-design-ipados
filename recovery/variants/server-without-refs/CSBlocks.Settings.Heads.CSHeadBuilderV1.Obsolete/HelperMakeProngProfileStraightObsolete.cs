using CSBlockUtil;
using RMA.OpenNURBS;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

public static class HelperMakeProngProfileStraightObsolete
{
	public static bool MakeProngProfileStraightObsolete(OnNurbsCurve headProfile, double param, double vProngAngle, double prongY, double prongNudge, out OnCurve vProng)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ac: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		vProng = null;
		if (headProfile == null)
		{
			return false;
		}
		On3dPoint val = ((OnCurve)headProfile).PointAt(param);
		OnLineCurve val2 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, val.z), (IOn3dPoint)new On3dPoint(val.x * 2.0, val.y * 2.0, val.z));
		OnLineCurve val3 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, val.z), (IOn3dPoint)new On3dPoint(val.x * 2.0, val.y * 2.0, val.z));
		OnXform val4 = new OnXform();
		val4.Rotation(CSUtilMath.ToRadians(vProngAngle), OnUtil.On_zaxis, OnUtil.On_origin);
		val2.Transform((IOnXform)(object)val4);
		val4.Rotation(CSUtilMath.ToRadians(0.0 - vProngAngle), OnUtil.On_zaxis, OnUtil.On_origin);
		val3.Transform((IOnXform)(object)val4);
		ArrayOnX_EVENT val5 = new ArrayOnX_EVENT();
		((OnCurve)headProfile).IntersectCurve((IOnCurve)(object)val2, ref val5);
		if (val5.Count() != 1)
		{
			return false;
		}
		double num = val5[0][0];
		val5 = new ArrayOnX_EVENT();
		((OnCurve)headProfile).IntersectCurve((IOnCurve)(object)val3, ref val5);
		if (val5.Count() != 1)
		{
			return false;
		}
		double num2 = val5[0][0];
		OnNurbsCurve val6;
		if (num < num2)
		{
			val6 = headProfile.Duplicate();
			((OnCurve)val6).Trim((IOnInterval)new OnInterval(num, num2));
		}
		else
		{
			OnNurbsCurve val7 = headProfile.Duplicate();
			((OnCurve)val7).Trim((IOnInterval)new OnInterval(num, ((OnCurve)headProfile).Domain().Max()));
			OnNurbsCurve val8 = headProfile.Duplicate();
			((OnCurve)val8).Trim((IOnInterval)new OnInterval(((OnCurve)headProfile).Domain().Min(), num2));
			if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
			{
				(IOnCurve)val7,
				(IOnCurve)val8
			}, out OnCurve mergedCrv, 0.01))
			{
				return false;
			}
			val6 = new OnNurbsCurve();
			mergedCrv.GetNurbForm(ref val6);
		}
		OnCurve val9 = CSUtilGeometry.OffsetOld((IOnCurve)(object)val6, OnUtil.On_zaxis, (IOn3dPoint)new On3dPoint(0.0, 0.0, val.z), prongNudge, closed: false);
		OnCurve val10 = CSUtilGeometry.OffsetOld((IOnCurve)(object)val6, OnUtil.On_zaxis, (IOn3dPoint)new On3dPoint(0.0, 0.0, val.z), prongY + prongNudge, closed: false);
		if (val9 == null || val10 == null)
		{
			return false;
		}
		double num3 = 0.0;
		val10.GetNormalizedArcLengthPoint(0.5, ref num3);
		On3dPoint val11 = val10.PointAt(num3);
		On3dPoint obj = val9.PointAtStart();
		On3dPoint val12 = val10.PointAtStart();
		On3dPoint val13 = (obj + (IOn3dPoint)(object)val12) / 2.0;
		On3dVector val14 = OnUtil.ON_CrossProduct((IOn3dVector)(object)(obj - (IOn3dPoint)(object)val13), OnUtil.On_zaxis);
		val14.Unitize();
		val14 *= 0.01;
		OnArc val15 = new OnArc((IOn3dPoint)(object)obj, (IOn3dPoint)(object)(val14 + (IOn3dPoint)(object)val13), (IOn3dPoint)(object)val12);
		On3dPoint obj2 = val9.PointAtEnd();
		On3dPoint val16 = val10.PointAtEnd();
		On3dPoint val17 = (obj2 + (IOn3dPoint)(object)val16) / 2.0;
		On3dVector val18 = OnUtil.ON_CrossProduct((IOn3dVector)(object)(obj2 - (IOn3dPoint)(object)val17), OnUtil.On_zaxis);
		val18.Unitize();
		val18 *= -0.01;
		OnArc val19 = new OnArc((IOn3dPoint)(object)obj2, (IOn3dPoint)(object)(val18 + (IOn3dPoint)(object)val17), (IOn3dPoint)(object)val16);
		OnNurbsCurve val20 = new OnNurbsCurve();
		OnNurbsCurve val21 = new OnNurbsCurve();
		((OnCircle)val15).GetNurbForm(ref val20);
		((OnCircle)val19).GetNurbForm(ref val21);
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[4]
		{
			(IOnCurve)val9,
			(IOnCurve)val20,
			(IOnCurve)val10,
			(IOnCurve)val21
		}, out OnCurve mergedCrv2, 0.01))
		{
			return false;
		}
		double num4 = 0.0;
		mergedCrv2.GetClosestPoint((IOn3dPoint)(object)val11, ref num4);
		mergedCrv2.ChangeClosedCurveSeam(num4);
		double num5 = mergedCrv2.Domain().Min();
		On3dVector obj3 = mergedCrv2.TangentAt(num5);
		On3dVector val22 = new On3dVector(val11.x, val11.y, 0.0);
		if (On3dPoint.op_Implicit((IOn3dVector)(object)OnUtil.ON_CrossProduct((IOn3dVector)(object)obj3, (IOn3dVector)(object)val22)).z < 0.0)
		{
			mergedCrv2.Reverse();
		}
		vProng = mergedCrv2;
		return true;
	}
}
