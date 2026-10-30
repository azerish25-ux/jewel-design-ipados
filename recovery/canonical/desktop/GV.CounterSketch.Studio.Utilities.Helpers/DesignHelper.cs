using System.Linq;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class DesignHelper
{
	public static bool IsNewDesign(int designId)
	{
		using (CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString))
		{
			DesignSearch designSearch = counterSketchLibraryDataEntities.DesignSearches.FirstOrDefault((DesignSearch _) => _.DesignId == (long)designId);
			if (designSearch != null)
			{
				return designSearch.IsNewDesign == 1;
			}
		}
		return false;
	}
}
