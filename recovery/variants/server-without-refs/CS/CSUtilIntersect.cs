using CS.CSDataUtilities;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CS;

public static class CSUtilIntersect
{
	public static bool GetIntersectSurface(CSData data, out Brep brep)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (CSDataHelper.GetBrep(data, out brep) && brep != null)
		{
			return true;
		}
		if (CSDataHelper.GetNurbsSurface(data, out var value) && value != null)
		{
			return CSBlockUtil.CSUtilIntersect.GetIntersectSurface((GeometryBase)(object)value, out brep);
		}
		if (CSDataHelper.GetNurbsCurve(data, out var value2) && value2 != null)
		{
			return CSBlockUtil.CSUtilIntersect.GetIntersectSurface((Curve)(object)value2, out brep);
		}
		if (CSDataHelper.GetTransform(data, out var value3) && ((Transform)(ref value3)).IsValid)
		{
			return CSBlockUtil.CSUtilIntersect.GetIntersectSurface(value3, out brep);
		}
		return false;
	}
}
