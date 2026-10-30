using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CS.Pattern;

[Serializable]
[XmlRoot("ComparePatchWireConnections")]
public sealed class ComparePatchWireConnections
{
	public List<CompareBlockConnections> blocks = new List<CompareBlockConnections>();

	public string name;

	public List<string> usedIn = new List<string>();

	public int usedInCount;

	public List<string> uUserdata;

	private int hash;

	public int PatchwireStartIndex { get; set; }

	public int PatchwireEndIndex { get; set; }

	public ComparePatchWireConnections(CSParaGraph graph, int start, int end, string filename)
	{
		name = graph.GraphList[start].Identifier;
		PatchwireStartIndex = start;
		PatchwireEndIndex = end;
		for (int i = start; i < graph.GraphList.Count && i <= end; i++)
		{
			blocks.Add(new CompareBlockConnections(graph.GraphList[i], CSApplicationHelper.Application.BlockList, start, end));
		}
		GenerateHashCode();
	}

	public ComparePatchWireConnections()
	{
	}

	public void ConvertUserDataToStrings()
	{
		foreach (CompareBlockConnections block in blocks)
		{
			block.ConvertUserDataToStrings();
		}
		uUserdata = new List<string>();
		foreach (CompareBlockConnections block2 in blocks)
		{
			foreach (string item in block2.userDataString)
			{
				if (!uUserdata.Contains(item))
				{
					uUserdata.Add(item);
				}
			}
		}
		uUserdata.Sort();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ComparePatchWireConnections comparePatchWireConnections))
		{
			return false;
		}
		if (comparePatchWireConnections.hash != hash)
		{
			return false;
		}
		if (comparePatchWireConnections.blocks.Count != blocks.Count)
		{
			return false;
		}
		for (int i = 0; i < blocks.Count; i++)
		{
			if (!comparePatchWireConnections.blocks[i].Equals(blocks[i]))
			{
				return false;
			}
		}
		return true;
	}

	public int GenerateHashCode()
	{
		hash = 0;
		for (int i = 0; i < blocks.Count; i++)
		{
			hash ^= blocks[i].GetHashCode();
		}
		return hash;
	}

	public override int GetHashCode()
	{
		return hash;
	}

	public void MergeUserData(ComparePatchWireConnections cpw)
	{
		for (int i = 0; i < blocks.Count && i < cpw.blocks.Count; i++)
		{
			blocks[i].userData.Add(cpw.blocks[i].userData);
		}
	}
}
