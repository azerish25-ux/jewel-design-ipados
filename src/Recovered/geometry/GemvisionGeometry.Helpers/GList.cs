using System.Collections.Generic;

namespace GemvisionGeometry.Helpers;

public class GList<T>
{
	internal List<T> MItems;

	internal int Size => MItems?.Count ?? 0;

	public T First
	{
		get
		{
			return MItems[0];
		}
		set
		{
			MItems[0] = value;
		}
	}

	public T Last
	{
		get
		{
			return MItems[Size - 1];
		}
		set
		{
			MItems[Size - 1] = value;
		}
	}
}
