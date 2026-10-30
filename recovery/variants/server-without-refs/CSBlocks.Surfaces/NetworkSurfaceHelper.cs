using System.Collections.Generic;
using CSDatas;
using CSDatas.Interfaces;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Surfaces;

public static class NetworkSurfaceHelper
{
	public static bool GetContinuityMode(ValueListItem whichContinuityMode, out int continuityMode)
	{
		continuityMode = -1;
		switch (whichContinuityMode)
		{
		case ValueListItem.ContinuityModeLoose:
			continuityMode = 0;
			return true;
		case ValueListItem.ContinuityModePos:
			continuityMode = 1;
			return true;
		case ValueListItem.ContinuityModeTan:
			continuityMode = 2;
			return true;
		case ValueListItem.ContinuityModeCurvature:
			continuityMode = 3;
			return true;
		default:
			return false;
		}
	}

	public static List<Curve> GetCurves(ICSArray curveDatas)
	{
		List<Curve> list = new List<Curve>();
		for (int i = 0; i < curveDatas.GetArrayCount(); i++)
		{
			CSData arrayItem = curveDatas.GetArrayItem(i);
			if (arrayItem != null && arrayItem is ICSCurve iCSCurve)
			{
				Curve curve = iCSCurve.GetCurve();
				if (curve != null)
				{
					list.Add(curve);
				}
			}
		}
		return list;
	}

	public static string GetError(int error)
	{
		return error switch
		{
			1 => "Curve sorter failed", 
			2 => "Network initializing failed", 
			3 => "Failed to build surface", 
			4 => "Network surface not valid", 
			_ => "Unknown", 
		};
	}
}
