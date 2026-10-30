using System.Collections.Generic;

namespace CS.Pattern;

public sealed class UniqueUserData
{
	public List<string> usedInPatchedWires = new List<string>();

	public string userDataString;

	public UniqueUserData()
	{
	}

	public UniqueUserData(string s)
	{
		userDataString = s;
	}

	public void Add(ComparePatchWireConnections cpw)
	{
		if (!usedInPatchedWires.Contains(cpw.name))
		{
			usedInPatchedWires.Add(cpw.name);
		}
	}
}
