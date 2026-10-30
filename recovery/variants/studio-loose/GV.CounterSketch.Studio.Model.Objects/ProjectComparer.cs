using System.Collections.Generic;

namespace GV.CounterSketch.Studio.Model.Objects;

public class ProjectComparer : IComparer<Project>
{
	public int Compare(Project x, Project y)
	{
		return string.Compare(x.Name, y.Name);
	}
}
