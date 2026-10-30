using System.Collections.Generic;

namespace GV.CounterSketch.Studio.Model;

public class TileItemComparer : IEqualityComparer<TileItem>
{
	public bool Equals(TileItem tileItem1, TileItem tielItem2)
	{
		return tileItem1.Id == tielItem2.Id;
	}

	public int GetHashCode(TileItem obj)
	{
		return obj.Id;
	}
}
