using System;
using System.Linq;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class CastingHelper
{
	public static bool TryGetSingleMetalCastingCount(int designId, out int singleMetalCastingCount)
	{
		singleMetalCastingCount = 0;
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		Design design = counterSketchLibraryDataEntities.Designs.FirstOrDefault((Design _) => _.DesignId == (long)designId);
		if (design == null)
		{
			return false;
		}
		GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute designAttribute = design.DesignAttributes.FirstOrDefault((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute _) => _.AttributeId == 367);
		if (designAttribute == null)
		{
			return false;
		}
		singleMetalCastingCount = Convert.ToInt32(designAttribute.AttributeValue);
		return true;
	}
}
