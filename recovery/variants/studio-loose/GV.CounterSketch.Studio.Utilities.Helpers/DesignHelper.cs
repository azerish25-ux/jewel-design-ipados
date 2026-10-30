using System;
using System.Linq;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class DesignHelper
{
	public static bool IsNewDesign(int designId)
	{
		CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		try
		{
			DesignSearch designSearch = ((IQueryable<DesignSearch>)counterSketchLibraryDataEntities.DesignSearches).FirstOrDefault((DesignSearch _) => _.DesignId == (long)designId);
			if (designSearch != null)
			{
				return designSearch.IsNewDesign == 1;
			}
		}
		finally
		{
			((IDisposable)counterSketchLibraryDataEntities)?.Dispose();
		}
		return false;
	}
}
