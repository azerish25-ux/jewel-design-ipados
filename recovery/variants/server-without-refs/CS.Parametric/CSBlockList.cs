using System.Collections.Generic;
using System.Linq;
using CS.CSObjects;

namespace CS.Parametric;

public sealed class CSBlockList : List<CSBlockDescription>
{
	public CSBlockDescription FindBlockFrom(CSObject obj)
	{
		return this.FirstOrDefault((CSBlockDescription item) => item.IsBlockInstance(obj));
	}
}
