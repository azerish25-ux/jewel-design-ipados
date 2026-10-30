using System.Collections.Generic;
using CS;

namespace CounterSketchServer.Utilities.Helpers;

public class IterationHelper
{
	public static int GetDesignIterationCount()
	{
		if (CSApplicationHelper.Application.Document != null && CSApplicationHelper.Application.Document.DocumentIterations != null)
		{
			return CSApplicationHelper.Application.Document.DocumentIterations.GetTotalIterationCount();
		}
		return 0;
	}

	public static List<int> GetFeaturedDesignIterations()
	{
		List<int> list = new List<int>();
		if (CSApplicationHelper.Application.Document != null && CSApplicationHelper.Application.Document.DocumentIterations != null)
		{
			int totalIterationCount = CSApplicationHelper.Application.Document.DocumentIterations.GetTotalIterationCount();
			for (int i = 0; i < totalIterationCount; i++)
			{
				if (CSApplicationHelper.Application.Document.DocumentUserData.FindValue("FeaturedIterations", i.ToString()) == "True")
				{
					list.Add(i);
				}
			}
		}
		return list;
	}

	public static void SetDesignIteration(int iterationId)
	{
		if (CSApplicationHelper.Application.Document != null && CSApplicationHelper.Application.Document.DocumentIterations != null)
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.DocumentIterations.SetIterationFromIndex(iterationId);
			CSApplicationHelper.Application.Document.Graph.GenerateAll();
			CSUtil.RedrawUI();
			CSUtil.Redraw();
		}
	}
}
