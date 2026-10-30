using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GV.CounterSketch.Studio.Model.Objects;

public class SortableObservableCollection<T> : ObservableCollection<T>
{
	public SortableObservableCollection()
	{
	}

	public SortableObservableCollection(IEnumerable<T> rootCollection)
		: base(rootCollection)
	{
	}

	public void Sort()
	{
	}

	public void Sort(IComparer<T> comparer)
	{
	}
}
