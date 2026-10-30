using System.Collections.Generic;
using RMA.OpenNURBS;

namespace CS.Parametric;

public sealed class CSDisplayGroupTable
{
	public List<CSDisplayGroup> DisplayGroups { get; } = new List<CSDisplayGroup>();

	public CSDisplayGroupTable()
	{
		DisplayGroups.Add(new CSDisplayGroup("Group1"));
		DisplayGroups.Add(new CSDisplayGroup("Group2"));
		DisplayGroups.Add(new CSDisplayGroup("Group2"));
	}

	public bool Read(ref OnBinaryArchive archive)
	{
		DisplayGroups.Clear();
		bool flag = true;
		int num = 0;
		flag &= archive.ReadInt(ref num);
		for (int i = 0; i < num; i++)
		{
			CSDisplayGroup cSDisplayGroup = new CSDisplayGroup("Read Error");
			flag &= cSDisplayGroup.Read(ref archive);
			DisplayGroups.Add(cSDisplayGroup);
		}
		return flag;
	}

	public bool Write(ref OnBinaryArchive archive)
	{
		bool flag = true;
		flag &= archive.WriteInt(DisplayGroups.Count);
		for (int i = 0; i < DisplayGroups.Count; i++)
		{
			flag &= DisplayGroups[i].Write(ref archive);
		}
		return flag;
	}
}
