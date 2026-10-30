using System.Collections.Generic;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlockDataUtils;

public static class CSUtilDataHelper
{
	public static CSDataArray CleanBrepArray(IEnumerable<Brep> outputSurfaces)
	{
		CSDataArray cSDataArray = new CSDataArray();
		if (outputSurfaces == null)
		{
			return cSDataArray;
		}
		foreach (Brep outputSurface in outputSurfaces)
		{
			Brep brep;
			(brep = outputSurface).Faces.ShrinkFaces();
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			cSDataArray.Append(new CSOnGeometryData(brep));
		}
		return cSDataArray;
	}

	public static List<Brep> GetCleanBrepInput(CSDataArray datas)
	{
		List<Brep> list = new List<Brep>();
		for (int i = 0; i < datas.GetArrayCount(); i++)
		{
			Brep brep = (datas.GetArrayItem(i) as ICSBrep)?.GetBrep();
			if (brep != null)
			{
				CSUtilGeometry.FixBrepSolidOrientation(ref brep);
				list.Add(brep);
			}
		}
		return list;
	}
}
