using System;
using System.Collections.Generic;
using System.Linq;

namespace GV.CounterSketch.Studio.Utilities.Extensions;

public static class EnumerableExtentions<T>
{
	public static IEnumerable<T> MoveToTop(IEnumerable<T> list, Func<T, bool> func)
	{
		return list.Where(func).Concat(list.Where((T item) => !func(item)));
	}
}
