using CS.CSDataUtilities;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CS;

public static class CSUtilIntersect
{
	public static bool GetIntersectSurface(CSData data, out Brep brep)
	{
		if (CSDataHelper.GetBrep(data, out brep) && brep != null)
		{
			return true;
		}
		if (CSDataHelper.GetNurbsSurface(data, out var value) && value != null)
		{
			return CSBlockUtil.CSUtilIntersect.GetIntersectSurface(value, out brep);
		}
		if (CSDataHelper.GetNurbsCurve(data, out var value2) && value2 != null)
		{
			return CSBlockUtil.CSUtilIntersect.GetIntersectSurface(value2, out brep);
		}
		if (CSDataHelper.GetTransform(data, out var value3) && value3.IsValid)
		{
			return CSBlockUtil.CSUtilIntersect.GetIntersectSurface(value3, out brep);
		}
		return false;
	}
}
